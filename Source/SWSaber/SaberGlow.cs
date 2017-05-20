using Verse;
using Harmony;
using System.Threading;
using System;

namespace SWSaber
{
    class SaberGlow : CompGlower
    {
        Timer t;
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            Traverse.Create(this).Field("glowOnInt").SetValue(true);
            this.parent.MapHeld.glowGrid.RegisterGlower(this);

            this.t = new Timer(this.GlowTick, null, 200, 100);
        }

        IntVec3 pos = IntVec3.Invalid;

        public void GlowTick(object state)
        {
            try
            {
                this.parent.Map.glowGrid.MarkGlowGridDirty(this.parent.Position);
            } catch(Exception ex) { Log.Error(ex.Message + "\n" + ex.StackTrace); }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            this.t.Dispose();
            this.parent.Map.glowGrid.DeRegisterGlower(this);
            base.PostDestroy(mode, previousMap);
        }
    }
}