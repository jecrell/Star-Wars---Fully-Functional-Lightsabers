using Verse;

namespace SWSaber
{
    /// <summary>
    ///     Definition of the settings for the mod
    /// </summary>
    internal class SWSaberSettings : ModSettings
    {
        public bool LightsabersGlowEffect = true;

        /// <summary>
        ///     Saving and loading the values
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref LightsabersGlowEffect, "LightsabersGlowEffect", true);
        }
    }
}