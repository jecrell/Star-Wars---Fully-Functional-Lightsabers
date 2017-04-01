using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using System.Reflection;
using UnityEngine;

namespace SWSaber
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.jecrell.starwars.lightsaber");
            harmony.Patch(AccessTools.Method(typeof(Pawn_EquipmentTracker), "AddEquipment"), null, new HarmonyMethod(typeof(HarmonyPatches).GetMethod("AddEquipment_PostFix")), null);
            
        }

        public static void AddEquipment_PostFix(Pawn_EquipmentTracker __instance, ThingWithComps newEq)
        {
            Pawn pawn = (Pawn)AccessTools.Field(typeof(Pawn_EquipmentTracker), "pawn").GetValue(__instance);

            CompLightsaberActivatableEffect lightsaberEffect = newEq.TryGetComp<CompLightsaberActivatableEffect>();
            if (lightsaberEffect != null)
            {
                if (pawn != null)
                {
                    if (pawn.Faction != Faction.OfPlayer)
                    {
                        CompCrystalSlotLoadable crystalSlot = newEq.GetComp<CompCrystalSlotLoadable>();
                        if (crystalSlot != null)
                        {
                            List<string> randomCrystals = new List<string>()
                            {
                                "PJ_KyberCrystal",
                                "PJ_KyberCrystalBlue",
                                "PJ_KyberCrystalCyan",
                                "PJ_KyberCrystalAzure",
                                "PJ_KyberCrystalRed",
                                "PJ_KyberCrystalPurple",
                            };
                            ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(ThingDef.Named(randomCrystals.RandomElement<string>()), null);
                            foreach (CompSlotLoadable.SlotLoadable slot in crystalSlot.Slots)
                            {
                                slot.TryLoadSlot(thingWithComps);
                            }
                            lightsaberEffect.Activate();
                        }
                    }
                }
            }
        }
    }
}
