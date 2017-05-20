using Harmony;
using RimWorld;
using System.Linq;
using Verse;

namespace SWSaber
{
    public class CompLightsaberDeflection : CompDeflector.CompDeflector
    {
        public readonly int defenseMeleeBlockChance = 15;
        public readonly FloatRange reflectionReturnChance = new FloatRange(0.15f, 0.25f);

        //Determines new accuracy based on skills.
        public override Verb CopyAndReturnNewVerb_PostFix(Verb newVerb)
        {
            Verb deflectVerb = newVerb;
            if (SWSaber.Utility.AreForcePowersLoaded()) deflectVerb = CopyAndReturnNewVerb_ForceAdjustments(deflectVerb);
            return deflectVerb;
        }

        //Determines new deflection target depending on accuracy.
        public override Pawn ResolveDeflectionTarget(Pawn defaultTarget = null)
        {
            Pawn result = base.ResolveDeflectionTarget(defaultTarget);
            if (SWSaber.Utility.AreForcePowersLoaded()) ResolveDeflectionTarget_ForceAdjustments(result);
            return result;
        }

        public override void ReflectionAccuracy_InFix(ref int modifier, ref int difficulty)
        {
            if (SWSaber.Utility.AreForcePowersLoaded())
            {
                difficulty = CalculatedAccuracy_ForceDifficulty();
                modifier = CalculatedAccuracy_ForceModifier();
            }

        }
        public override float DeflectionChance_InFix(float calc)
        {
            float result = calc;        
            if (SWSaber.Utility.AreForcePowersLoaded())
            {
                result = CalculatedBlock_ForceModifier();
            }
            return result;

        }


        //
        #region ForceUsers

        public override bool TrySpecialMeleeBlock()
        {
            bool result = false;
            ThingComp forceUser = this.GetPawn.AllComps.FirstOrDefault<ThingComp>((ThingComp y) => y.GetType().ToString().Contains("CompForceUser"));
            if (forceUser != null)
            {
                int modifier = (int)AccessTools.Method(forceUser.GetType(), "ForceSkillLevel").Invoke(forceUser, new object[] { "PJ_LightsaberDefense" });
                int blockChance = 0;
                if (modifier > 0)
                {
                    for (int i = 0; i < modifier; i++)
                    {
                        blockChance += this.defenseMeleeBlockChance;
                    }
                    if (blockChance > Rand.Range(0, 100))
                    {
                        result = true;
                        MoteMaker.ThrowText(forceUser.parent.Position.ToVector3(), forceUser.parent.Map, "SWSaber_Block".Translate(), 2f);
                    }
                }
            }
            return result;
        }

        public float CalculatedBlock_ForceModifier()
        {
            float result = 0;
            ThingComp forceUser = this.GetPawn.AllComps.FirstOrDefault<ThingComp>((ThingComp y) => y.GetType().ToString().Contains("CompForceUser"));
            if (forceUser != null)
            {
                int modifier = (int)AccessTools.Method(forceUser.GetType(), "ForceSkillLevel").Invoke(forceUser, new object[] { "PJ_LightsaberDefense" });
                if (modifier > 0)
                {
                    for (int i = 0; i < modifier; i++)
                    {
                        result += Rand.Range(this.reflectionReturnChance.min, this.reflectionReturnChance.max);
                    }
                }
                //Log.Message("Lightsabers: :: New Modifier " + modifier.ToString());
            }
            return result;
        }


        public int CalculatedAccuracy_ForceModifier()
        {
            //Log.Message("Lightsabers :: ForceModifier Called");
            int result = 0;
            ThingComp forceUser = this.GetPawn.AllComps.FirstOrDefault<ThingComp>((ThingComp y) => y.GetType().ToString().Contains("CompForceUser"));
            if (forceUser != null)
            {
                int modifier = (int)AccessTools.Method(forceUser.GetType(), "ForceSkillLevel").Invoke(forceUser, new object[] { "PJ_LightsaberReflection" });
                if (modifier > 0)
                {
                    for (int i = 0; i < modifier; i++)
                    {
                        result += Rand.Range(15, 25);
                    }
                }
                //Log.Message("Lightsabers: :: New Modifier " + modifier.ToString());
            }
            return result;
        }

        //Placeholder for now.
        public int CalculatedAccuracy_ForceDifficulty() => 100;

        //Placeholder for now.
        public Verb CopyAndReturnNewVerb_ForceAdjustments(Verb newVerb) => newVerb;

        //Placeholder for now.
        public Pawn ResolveDeflectionTarget_ForceAdjustments(Pawn defaultTarget = null) => defaultTarget;
        #endregion ForceUsers

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<AccuracyRoll>(ref this.lastAccuracyRoll, "lastAccuracyRoll", AccuracyRoll.Failure);
        }
    }
}
