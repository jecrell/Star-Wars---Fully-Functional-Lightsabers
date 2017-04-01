using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace SWSaber
{
    public static class Utility
    {
        public static bool modCheck = false;
        public static bool loadedForcePowers = false;

        public static bool AreForcePowersLoaded()
        {
            if (!modCheck) ModCheck();
            return loadedForcePowers;
        }

        public static void ModCheck()
        {
            Log.Message("Mod Check Called");
            loadedForcePowers = false;
            foreach (ModContentPack ResolvedMod in LoadedModManager.RunningMods)
            {
                if (loadedForcePowers) break; //Save some loading
                if (ResolvedMod.Name.Contains("Star Wars - The Force"))
                {
                    Log.Message("Lightsabers :: Star Wars - The Force Detected.");
                    loadedForcePowers = true;
                }
            }
            modCheck = true;
            return;
        }
    }
}
