using System.Linq;
using CompSlotLoadable;
using RimWorld;
using Verse;

namespace SWSaber
{
    public class CompLightsaberActivatableEffect : CompActivatableEffect.CompActivatableEffect
    {
        public override Graphic PostGraphicEffects(Graphic graphic)
        {
            if (graphic == null || graphic.Shader == null)
            {
                return base.PostGraphicEffects(graphic);
            }

            if (!(parent.AllComps.FirstOrDefault(x => x is CompSlotLoadable.CompSlotLoadable) is
                    CompSlotLoadable.CompSlotLoadable comp) ||
                !(comp.Slots.FirstOrDefault(x =>
                    ((SlotLoadableDef) x.def).doesChangeColor) is { } colorSlot) ||
                !(colorSlot.SlotOccupant?.TryGetComp<CompSlottedBonus>() is { } slotBonus))
            {
                return base.PostGraphicEffects(graphic);
            }

            var result = graphic.GetColoredVersion(graphic.Shader, slotBonus.Props.color, slotBonus.Props.color);
            if (result != null)
            {
                return result;
            }

            return base.PostGraphicEffects(graphic);
        }

        public override bool CanActivate()
        {
            if (!parent.SpawnedOrAnyParentSpawned)
            {
                return false;
            }

            //Log.Message("1");
            var comp = parent.AllComps.FirstOrDefault(x => x is CompSlotLoadable.CompSlotLoadable);
            if (comp != null)
            {
                //Log.Message("2");
                var compSlotLoadable = comp as CompSlotLoadable.CompSlotLoadable;
                var colorSlot =
                    compSlotLoadable?.Slots.FirstOrDefault(x => ((SlotLoadableDef) x.def).doesChangeColor);
                //Log.Message("3");
                if (colorSlot?.SlotOccupant != null)
                {
                    return true;
                }
            }

            Messages.Message("KyberCrystalRequired".Translate(), MessageTypeDefOf.RejectInput);

            return false;
        }

        public override void Activate()
        {
            base.Activate();
            MakeGlower();
        }

        private void MakeGlower()
        {
            if (!LoadedModManager.GetMod<SWSaberMod>().GetSettings<SWSaberSettings>().LightsabersGlowEffect)
            {
                return;
            }

            SaberGlow sb = null;
            var ownerParentHolder = parent.holdingOwner.Owner.ParentHolder as Pawn;
            ownerParentHolder?.AllComps.Add(sb = new SaberGlow
            {
                parent = ownerParentHolder,
                props = new CompProperties_Glower
                {
                    compClass = typeof(SaberGlow),
                    glowRadius = 5f,
                    glowColor = ColorIntUtility.AsColorInt(
                        parent.TryGetComp<CompSlotLoadable.CompSlotLoadable>()?.Slots
                            .FirstOrDefault(x => ((SlotLoadableDef) x.def).doesChangeColor)?.SlotOccupant
                            ?.TryGetComp<CompSlottedBonus>()?.Props.color ?? ColorLibrary.Violet),
                    overlightRadius = 5f
                }
            });
            sb?.PostSpawnSetup(false);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            if (!(parent is { } t) || !(t.holdingOwner is { } o) ||
                !(o.Owner.ParentHolder is Pawn p) || !(p.GetComp<SaberGlow>() is { } sb) ||
                t.MapHeld?.glowGrid == null)
            {
                return;
            }

            try
            {
                parent.MapHeld.glowGrid.DeRegisterGlower(sb);
                sb.PostDestroy(DestroyMode.Vanish, parent.Map);
                p.AllComps.Remove(sb);
            }
            catch
            {
                // ignored
            }
        }
    }
}