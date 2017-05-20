using CompSlotLoadable;
using System;
using System.Collections.Generic;
using Verse;

namespace SWSaber
{
    public class CompProperties_CrystalSlotLoadable : CompProperties_SlotLoadable
    {
        public CompProperties_CrystalSlotLoadable() => this.compClass = typeof(CompCrystalSlotLoadable);
    }
}
