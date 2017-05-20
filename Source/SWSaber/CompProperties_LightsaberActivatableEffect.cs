using CompActivatableEffect;

namespace SWSaber
{
    public class CompProperties_LightsaberActivatableEffect : CompProperties_ActivatableEffect
    {
        public CompProperties_LightsaberActivatableEffect() => this.compClass = typeof(CompLightsaberActivatableEffect);
    }
}
