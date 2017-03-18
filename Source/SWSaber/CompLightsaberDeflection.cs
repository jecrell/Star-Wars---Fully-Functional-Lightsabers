using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace SWSaber
{
    public class CompLightsaberDeflection : CompDeflector.CompDeflector
    {
 

        //Determines new accuracy based on skills.
        public override Verb CopyAndReturnNewVerb_PostFix(Verb newVerb)
        {
            //lastAccuracyRoll = CalculatedAccuracy();
            Verb deflectVerb = newVerb;

            ////Initialize VerbProperties
            //VerbProperties newVerbProps = new VerbProperties();

            ////Copy values over to a new verb props
            //newVerbProps.hasStandardCommand = newVerb.verbProps.hasStandardCommand;
            //newVerbProps.projectileDef = newVerb.verbProps.projectileDef;
            //newVerbProps.range = newVerb.verbProps.range;
            //newVerbProps.muzzleFlashScale = newVerb.verbProps.muzzleFlashScale;
            //newVerbProps.warmupTime = 0;
            //newVerbProps.defaultCooldownTime = 0;
            //newVerbProps.soundCast = this.Props.deflectSound;
            
            //switch (lastAccuracyRoll)
            //{
            //    case AccuracyRoll.CriticalSuccess:
            //        //if (GetPawn != null)
            //        //{
            //        //    MoteMaker.ThrowText(GetPawn.DrawPos, GetPawn.Map, "SWSaber_TextMote_CriticalSuccess".Translate(), 6f);
            //        //}
            //        newVerbProps.accuracyLong = 999.0f;
            //        newVerbProps.accuracyMedium = 999.0f;
            //        newVerbProps.accuracyShort = 999.0f;
            //        break;
            //    case AccuracyRoll.Failure:
            //        newVerbProps.forcedMissRadius = 50.0f;
            //        newVerbProps.accuracyLong = 0.0f;
            //        newVerbProps.accuracyMedium = 0.0f;
            //        newVerbProps.accuracyShort = 0.0f;
            //        break;

            //    case AccuracyRoll.CritialFailure:
            //        if (GetPawn != null)
            //        {
            //            MoteMaker.ThrowText(GetPawn.DrawPos, GetPawn.Map, "SWSaber_TextMote_CriticalFailure".Translate(), 6f);
            //        }
            //        newVerbProps.accuracyLong = 999.0f;
            //        newVerbProps.accuracyMedium = 999.0f;
            //        newVerbProps.accuracyShort = 999.0f;
            //        break;
            //    case AccuracyRoll.Success:
            //        newVerbProps.accuracyLong = 999.0f;
            //        newVerbProps.accuracyMedium = 999.0f;
            //        newVerbProps.accuracyShort = 999.0f;
            //        break;
            //}
            ////Apply values
            //deflectVerb.verbProps = newVerbProps;

            //Log.Message("Accuracy Roll Result: " + lastAccuracyRoll.ToString());
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

        #region ForceUsers
        //Placeholder for now.
        public int CalculatedAccuracy_ForceModifier()
        {
            return 0;
        }

        //Placeholder for now.
        public int CalculatedAccuracy_ForceDifficulty()
        {
            return 80;
        }

        //Placeholder for now.
        public Verb CopyAndReturnNewVerb_ForceAdjustments(Verb newVerb)
        {
            return newVerb;
        }

        //Placeholder for now.
        public Pawn ResolveDeflectionTarget_ForceAdjustments(Pawn defaultTarget = null)
        {
            return defaultTarget;
        }
        #endregion ForceUsers

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.LookValue<AccuracyRoll>(ref this.lastAccuracyRoll, "lastAccuracyRoll", AccuracyRoll.Failure);
        }
    }
}
