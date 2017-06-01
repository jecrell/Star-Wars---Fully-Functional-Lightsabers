using Verse;
using Harmony;
using System.Threading;
using System;

namespace SWSaber
{
    class SaberGlow : CompGlower
    {
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            Traverse.Create(this).Field("glowOnInt").SetValue(true);
            this.parent.MapHeld.glowGrid.RegisterGlower(this);
        }

        IntVec3 pos = IntVec3.Invalid;

        public override void CompTick() => 
            this.parent.Map.glowGrid.MarkGlowGridDirty(this.parent.Position);

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            this.parent.Map.glowGrid.DeRegisterGlower(this);
            base.PostDestroy(mode, previousMap);
        }
    }
}