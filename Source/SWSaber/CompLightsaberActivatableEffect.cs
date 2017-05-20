using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompActivatableEffect;
using CompSlotLoadable;
using Verse;

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
            Messages.Message("KyberCrystalRequired".Translate(), MessageSound.RejectInput);
            return false;
        }

        public override void ActiveTick()
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
                    if (colorSlot.IsEmpty())
                    {
                        base.Deactivate();
                    }
                }
            }
        }
    }
}
