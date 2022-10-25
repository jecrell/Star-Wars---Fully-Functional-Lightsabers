using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SWSaber
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.jecrell.starwars.lightsaber");
            harmony.Patch(AccessTools.Method(typeof(Pawn_EquipmentTracker), "AddEquipment"), null,
                new HarmonyMethod(typeof(HarmonyPatches).GetMethod("AddEquipment_PostFix")));
            harmony.Patch(AccessTools.Method(typeof(PawnInventoryGenerator), "GenerateInventoryFor"), null,
                new HarmonyMethod(typeof(HarmonyPatches).GetMethod("GenerateInventoryFor_PostFix")));
        }

        public static Thing GenerateCrystal(ThingDef crystalDef, float chance = 1.0f)
        {
            if (Rand.Value > chance)
            {
                return null;
            }

            if (crystalDef == null)
            {
                return null;
            }

            var thing = ThingMaker.MakeThing(crystalDef);
            thing.stackCount = 1;
            return thing;
        }

        public static void GenerateCrystalFor(Pawn p)
        {
            if (p.inventory == null)
            {
                return;
            }

            if (p.inventory.innerContainer == null)
            {
                return;
            }

            var legendaryCrystals = new List<ThingDef>
            {
                ThingDef.Named("PJ_UltimaPearl"),
                ThingDef.Named("PJ_BlackPearl"),
                ThingDef.Named("PJ_KaiburrCrystal"),
                ThingDef.Named("PJ_UltimaPearl"),
                ThingDef.Named("PJ_AnkSapphire")
            };
            var rareCrystals = new List<ThingDef>
            {
                ThingDef.Named("PJ_BarabIngot"),
                ThingDef.Named("PJ_PontiteCrystal"),
                ThingDef.Named("PJ_FirkrannCrystal"),
                ThingDef.Named("PJ_RubatCrystal"),
                ThingDef.Named("PJ_HurCrystal"),
                ThingDef.Named("PJ_DragiteCrystal"),
                ThingDef.Named("PJ_DamindCrystal"),
                ThingDef.Named("PJ_AdeganCrystal"),
                ThingDef.Named("PJ_EralamCrystal"),
                ThingDef.Named("PJ_PontiteCrystal")
            };
            var result = GenerateCrystal(legendaryCrystals.RandomElement(), 0.7f);
            if (result != null)
            {
                //Log.Message("5a");

                p.inventory.innerContainer.TryAdd(result);
            }
            else
            {
                //Log.Message("5b");

                result = GenerateCrystal(rareCrystals.RandomElement());
                p.inventory.innerContainer.TryAdd(result);
            }
        }

        // RimWorld.PawnInventoryGenerator
        public static void GenerateInventoryFor_PostFix(Pawn p, PawnGenerationRequest request)
        {
            //Log.Message("1");
            if (!Utility.AreFactionsLoaded())
            {
                return;
            }
            //Log.Message("2");

            if (p.kindDef == null)
            {
                return;
            }
            //Log.Message("3");

            if (p.kindDef.defName == "PJ_ImpCommander" ||
                p.kindDef.defName == "PJ_RebCouncilman" ||
                p.kindDef.defName == "PJ_ScumBoss")
            {
                //Log.Message("4");

                GenerateCrystalFor(p);
            }
        }

        //public static void Remove_PostFix(Pawn_EquipmentTracker __instance, ThingWithComps eq)
        //{
        //    CompLightsaberActivatableEffect lightsaberEffect = eq.TryGetComp<CompLightsaberActivatableEffect>();
        //    if (lightsaberEffect != null)
        //    {

        //    }
        //}

        public static void CrystalSlotter(CompCrystalSlotLoadable crystalSlot,
            CompLightsaberActivatableEffect lightsaberEffect)
        {
            //
            crystalSlot.Initialize();
            var randomCrystals = new List<string>
            {
                "PJ_KyberCrystal",
                "PJ_KyberCrystalBlue",
                "PJ_KyberCrystalCyan",
                "PJ_KyberCrystalAzure",
                "PJ_KyberCrystalRed",
                "PJ_KyberCrystalPurple"
            };
            var thingWithComps = (ThingWithComps) ThingMaker.MakeThing(ThingDef.Named(randomCrystals.RandomElement()));
            Log.Message(thingWithComps.Label);
            foreach (var slot in crystalSlot.Slots)
            {
                slot.TryLoadSlot(thingWithComps);
            }

            lightsaberEffect.TryActivate();
        }

        public static void AddEquipment_PostFix(Pawn_EquipmentTracker __instance, ThingWithComps newEq)
        {
            var pawn = (Pawn) AccessTools.Field(typeof(Pawn_EquipmentTracker), "pawn").GetValue(__instance);

            var lightsaberEffect = newEq.TryGetComp<CompLightsaberActivatableEffect>();
            if (lightsaberEffect == null)
            {
                return;
            }

            if (pawn == null)
            {
                return;
            }

            if (pawn.Faction == null)
            {
                return;
            }

            if (pawn.Faction == Faction.OfPlayerSilentFail)
            {
                return;
            }

            var crystalSlot = newEq.GetComp<CompCrystalSlotLoadable>();
            if (crystalSlot == null)
            {
                return;
            }

            CrystalSlotter(crystalSlot, lightsaberEffect);
        }
    }
}