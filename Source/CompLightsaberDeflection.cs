using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SWSaber
{
    public class CompLightsaberDeflection : CompDeflector.CompDeflector
    {
        private readonly int defenseMeleeBlockChance = 15;
        private readonly FloatRange reflectionReturnChance = new FloatRange(0.15f, 0.25f);

        //Determines new accuracy based on skills.
        public override Verb CopyAndReturnNewVerb_PostFix(Verb newVerb)
        {
            var verb = newVerb;
            if (Utility.AreForcePowersLoaded())
            {
                verb = CopyAndReturnNewVerb_ForceAdjustments(verb);
            }

            return verb;
        }

        //Determines new deflection target depending on accuracy.
        public override Pawn ResolveDeflectionTarget(Pawn defaultTarget = null)
        {
            var result = base.ResolveDeflectionTarget(defaultTarget);
            if (Utility.AreForcePowersLoaded())
            {
                ResolveDeflectionTarget_ForceAdjustments(result);
            }

            return result;
        }

        public override void ReflectionAccuracy_InFix(ref int modifier, ref int difficulty)
        {
            if (!Utility.AreForcePowersLoaded())
            {
                return;
            }

            difficulty = CalculatedAccuracy_ForceDifficulty();
            modifier = CalculatedAccuracy_ForceModifier();
        }

        public override float DeflectionChance_InFix(float calc)
        {
            var result = calc;
            if (Utility.AreForcePowersLoaded())
            {
                result = CalculatedBlock_ForceModifier();
            }

            return result;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref lastAccuracyRoll, "lastAccuracyRoll", AccuracyRoll.Failure);
        }


        //

        #region ForceUsers

        public override bool TrySpecialMeleeBlock()
        {
            var forceUser = GetPawn.AllComps.FirstOrDefault(y => y.GetType().ToString().Contains("CompForceUser"));
            if (forceUser == null)
            {
                return false;
            }

            var modifier = (int) AccessTools.Method(forceUser.GetType(), "ForceSkillLevel")
                .Invoke(forceUser, new object[] {"PJ_LightsaberDefense"});
            var blockChance = 0;
            if (modifier <= 0)
            {
                return false;
            }

            for (var i = 0; i < modifier; i++)
            {
                blockChance += defenseMeleeBlockChance;
            }

            if (blockChance <= Rand.Range(0, 100))
            {
                return false;
            }

            MoteMaker.ThrowText(forceUser.parent.Position.ToVector3(), forceUser.parent.Map,
                "SWSaber_Block".Translate(), 2f);

            return true;
        }

        private float CalculatedBlock_ForceModifier()
        {
            float result = 0;
            var forceUser = GetPawn.AllComps.FirstOrDefault(y => y.GetType().ToString().Contains("CompForceUser"));
            if (forceUser == null)
            {
                return result;
                //Log.Message("Lightsabers: :: New Modifier " + modifier.ToString());
            }

            var modifier = (int) AccessTools.Method(forceUser.GetType(), "ForceSkillLevel")
                .Invoke(forceUser, new object[] {"PJ_LightsaberDefense"});
            if (modifier <= 0)
            {
                return result;
            }

            for (var i = 0; i < modifier; i++)
            {
                result += Rand.Range(reflectionReturnChance.min, reflectionReturnChance.max);
            }

            return result;
        }


        private int CalculatedAccuracy_ForceModifier()
        {
            //Log.Message("Lightsabers :: ForceModifier Called");
            var result = 0;
            var forceUser = GetPawn.AllComps.FirstOrDefault(y => y.GetType().ToString().Contains("CompForceUser"));
            if (forceUser == null)
            {
                return result;
                //Log.Message("Lightsabers: :: New Modifier " + modifier.ToString());
            }

            var modifier = (int) AccessTools.Method(forceUser.GetType(), "ForceSkillLevel")
                .Invoke(forceUser, new object[] {"PJ_LightsaberReflection"});
            if (modifier <= 0)
            {
                return result;
            }

            for (var i = 0; i < modifier; i++)
            {
                result += Rand.Range(15, 25);
            }

            return result;
        }

        //Placeholder for now.
        private int CalculatedAccuracy_ForceDifficulty()
        {
            return 100;
        }

        //Placeholder for now.
        private Verb CopyAndReturnNewVerb_ForceAdjustments(Verb newVerb)
        {
            return newVerb;
        }

        //Placeholder for now.
        private Pawn ResolveDeflectionTarget_ForceAdjustments(Pawn defaultTarget = null)
        {
            return defaultTarget;
        }

        #endregion ForceUsers
    }
}