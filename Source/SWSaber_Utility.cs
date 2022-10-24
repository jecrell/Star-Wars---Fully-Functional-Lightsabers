using Verse;

namespace SWSaber
{
    public static class Utility
    {
        public static bool modCheck = false;
        public static bool loadedForcePowers = false;
        public static bool loadedFactions = false;

        public static bool AreForcePowersLoaded()
        {
            if (!modCheck) ModCheck();
            return loadedForcePowers;
        }
        public static bool AreFactionsLoaded()
        {
            if (!modCheck) ModCheck();
            return loadedFactions;
        }

        public static void ModCheck()
        {
            Log.Message("Mod Check Called");
            loadedForcePowers = false;
            loadedFactions = false;
            foreach (ModContentPack ResolvedMod in LoadedModManager.RunningMods)
            {
                if (loadedForcePowers && loadedFactions) break; //Save some loading
                if (ResolvedMod.Name.Contains("Star Wars - The Force"))
                {
                    Log.Message("Lightsabers :: Star Wars - The Force Detected.");
                    loadedForcePowers = true;
                }
                if (ResolvedMod.Name.Contains("Star Wars - Factions"))
                {
                    Log.Message("Lightsabers :: Star Wars - Factions Detected.");
                    loadedFactions = true;
                }
            }
            modCheck = true;
            return;
        }
    }
}
