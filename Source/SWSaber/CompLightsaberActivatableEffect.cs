using System.Linq;
using CompSlotLoadable;
using Verse;
using RimWorld;

namespace SWSaber
{
    public class CompLightsaberActivatableEffect : CompActivatableEffect.CompActivatableEffect
    {
        public override Graphic PostGraphicEffects(Graphic graphic)
        {
            if (graphic != null && graphic.Shader != null)
            {
                if (this.parent.AllComps.FirstOrDefault((ThingComp x) => x is CompSlotLoadable.CompSlotLoadable) is CompSlotLoadable.CompSlotLoadable comp &&
                    comp.Slots.FirstOrDefault((SlotLoadable x) => ((SlotLoadableDef)x.def).doesChangeColor == true) is SlotLoadable colorSlot &&
                    colorSlot.SlotOccupant != null && colorSlot.SlotOccupant.TryGetComp<CompSlottedBonus>() is CompSlottedBonus slotBonus)
                {
                    Graphic result = graphic.GetColoredVersion(graphic.Shader, slotBonus.Props.color, slotBonus.Props.color);
                    if (result != null) return result;
                }
            }
            return base.PostGraphicEffects(graphic);
        }

        public override bool CanActivate()
        {
            if (this.parent.SpawnedOrAnyParentSpawned)
            {
                //Log.Message("1");
                ThingComp comp = this.parent.AllComps.FirstOrDefault((ThingComp x) => x is CompSlotLoadable.CompSlotLoadable);
                if (comp != null)
                {
                    //Log.Message("2");
                    CompSlotLoadable.CompSlotLoadable compSlotLoadable = comp as CompSlotLoadable.CompSlotLoadable;
                    SlotLoadable colorSlot = compSlotLoadable.Slots.FirstOrDefault((SlotLoadable x) => ((SlotLoadableDef)x.def).doesChangeColor == true);
                    if (colorSlot != null)
                    {
                        //Log.Message("3");
                        if (colorSlot.SlotOccupant != null)
                        {
                            return true;
                        }
                    }
                }
                Messages.Message("KyberCrystalRequired".Translate(), MessageTypeDefOf.RejectInput);
            }
            return false;
        }

        public override void Activate()
        {
            base.Activate();
            MakeGlower();
        }

        public void MakeGlower()
        {
            SaberGlow sb;
            Pawn p = this.parent.holdingOwner.Owner.ParentHolder as Pawn;
            p.AllComps.Add(sb = new SaberGlow()
            {
                parent = p,
                props = new CompProperties_Glower()
                {
                    compClass = typeof(SaberGlow),
                    glowRadius = 5f,
                    glowColor = ColorIntUtility.AsColorInt(this.parent.TryGetComp<CompSlotLoadable.CompSlotLoadable>()?.Slots.FirstOrDefault(x => (x.def as SlotLoadableDef).doesChangeColor == true)?.SlotOccupant?.TryGetComp<CompSlottedBonus>()?.Props.color ?? ColorLibrary.Violet),
                    overlightRadius = 5f
                }
            });
            sb.PostSpawnSetup(false);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            if (this.parent is ThingWithComps t && t.holdingOwner is ThingOwner o &&
                o.Owner.ParentHolder is Pawn p && p.GetComp<SaberGlow>() is SaberGlow sb &&
                t?.MapHeld?.glowGrid != null)
            {
                try
                {
                    this.parent.MapHeld.glowGrid.DeRegisterGlower(sb);
                    sb.PostDestroy(DestroyMode.Vanish, this.parent.Map);
                    p.AllComps.Remove(sb);
                }
                catch { }
            }
        }
    }
}