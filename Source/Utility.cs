using Verse;

namespace SWSaber
{
    public static class Utility
    {
        private static bool modCheck;
        private static bool loadedForcePowers;
        private static bool loadedFactions;

        public static bool AreForcePowersLoaded()
        {
            if (!modCheck)
            {
                ModCheck();
            }

            return loadedForcePowers;
        }

        public static bool AreFactionsLoaded()
        {
            if (!modCheck)
            {
                ModCheck();
            }

            return loadedFactions;
        }

        private static void ModCheck()
        {
            Log.Message("Mod Check Called");
            loadedForcePowers = false;
            loadedFactions = false;
            foreach (var ResolvedMod in LoadedModManager.RunningMods)
            {
                if (loadedForcePowers && loadedFactions)
                {
                    break; //Save some loading
                }

                if (ResolvedMod.Name.Contains("Star Wars - The Force"))
                {
                    Log.Message("Lightsabers :: Star Wars - The Force Detected.");
                    loadedForcePowers = true;
                }

                if (!ResolvedMod.Name.Contains("Star Wars - Factions"))
                {
                    continue;
                }

                Log.Message("Lightsabers :: Star Wars - Factions Detected.");
                loadedFactions = true;
            }

            modCheck = true;
        }
    }
}