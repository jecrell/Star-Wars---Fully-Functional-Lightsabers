using UnityEngine;
using Verse;

namespace SWSaber
{
    [StaticConstructorOnStartup]
    internal class SWSaberMod : Mod
    {
        /// <summary>
        ///     The instance of the settings to be read by the mod
        /// </summary>
        private static SWSaberMod instance;

        /// <summary>
        ///     The private settings
        /// </summary>
        private SWSaberSettings settings;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="content"></param>
        public SWSaberMod(ModContentPack content) : base(content)
        {
            instance = this;
        }

        /// <summary>
        ///     The instance-settings for the mod
        /// </summary>
        private SWSaberSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = GetSettings<SWSaberSettings>();
                }

                return settings;
            }
        }

        /// <summary>
        ///     The title for the mod-settings
        /// </summary>
        /// <returns></returns>
        public override string SettingsCategory()
        {
            return "Star Wars - Fully Functional Lightsabers";
        }

        /// <summary>
        ///     The settings-window
        ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
        /// </summary>
        /// <param name="rect"></param>
        public override void DoSettingsWindowContents(Rect rect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.Gap();
            listing_Standard.CheckboxLabeled("Use lightsaber glow-effects", ref Settings.LightsabersGlowEffect,
                "Enables a glowing effect on active lightsabers");
            listing_Standard.End();
            Settings.Write();
        }
    }
}