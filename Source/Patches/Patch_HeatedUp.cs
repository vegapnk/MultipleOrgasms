using HarmonyLib;
using Multipleorgasm.Settings;
using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Multipleorgasm.Patches
{


    [HarmonyPatch(typeof(JobDriver_Sex), "SetupOrgasmTicks")]
    public static class Patch_HeatedUp
    {


        public static void Postfix(JobDriver_Sex __instance)
        {
            Pawn orgasmingPawn = __instance.pawn;
            if (orgasmingPawn != null && !orgasmingPawn.IsAnimal())
            {
                // Case 1: Handle All that should speed up
                if (
                    (Genital_Helper.is_futa(orgasmingPawn) && MultipleOrgasmsSettings.futaBehavior == MultipleOrgasmsSettings.Behavior.SpeedUp)
                    || (Genital_Helper.has_penis_fertile(orgasmingPawn) && MultipleOrgasmsSettings.maleBehavior == MultipleOrgasmsSettings.Behavior.SpeedUp)
                    || (Genital_Helper.has_penis_infertile(orgasmingPawn) && MultipleOrgasmsSettings.maleBehavior == MultipleOrgasmsSettings.Behavior.SpeedUp)
                    || (Genital_Helper.has_vagina(orgasmingPawn) && MultipleOrgasmsSettings.femaleBehavior == MultipleOrgasmsSettings.Behavior.SpeedUp)
                    )
                    HeatUpPawn(orgasmingPawn, __instance);

                // Case 2: Handle all that should slow down
                if (
                    (Genital_Helper.is_futa(orgasmingPawn) && MultipleOrgasmsSettings.futaBehavior == MultipleOrgasmsSettings.Behavior.SlowDown)
                    || (Genital_Helper.has_penis_fertile(orgasmingPawn) && MultipleOrgasmsSettings.maleBehavior == MultipleOrgasmsSettings.Behavior.SlowDown)
                    || (Genital_Helper.has_penis_infertile(orgasmingPawn) && MultipleOrgasmsSettings.maleBehavior == MultipleOrgasmsSettings.Behavior.SlowDown)
                    || (Genital_Helper.has_vagina(orgasmingPawn) && MultipleOrgasmsSettings.femaleBehavior == MultipleOrgasmsSettings.Behavior.SlowDown)
                    )
                    DrainPawn(orgasmingPawn, __instance);

                // Case 3: Pawns sex and/or target-behavior are none, no heat up or drain
            }

        }


        public static void HeatUpPawn(Pawn pawn, JobDriver_Sex sexInstance)
        {
            var heatedUpHediff = GetHeatedUpHediff(pawn);

            if (sexInstance.orgasms > 0 && heatedUpHediff.Severity < 1.0)
            {
                heatedUpHediff.Severity += MultipleOrgasmsSettings.heatup_severity_increase_per_orgasm;
            }
            // Adjust Time
            float orgasm_time_reduction = Math.Max(1.0f - heatedUpHediff.Severity, 0.25f);
            sexInstance.sex_ticks = (int)(sexInstance.sex_ticks * orgasm_time_reduction);
            // Backup: Do not break your game, lol
            if (sexInstance.sex_ticks < (int) MultipleOrgasmsSettings.min_ticks_to_orgasm)
                sexInstance.sex_ticks = (int) MultipleOrgasmsSettings.min_ticks_to_orgasm;
        }

        public static void DrainPawn(Pawn pawn, JobDriver_Sex sexInstance)
        {
            var drainedHediff = GetDrainedHediff(pawn);

            if (sexInstance.orgasms > 0 && drainedHediff.Severity < 1.0)
            {
                drainedHediff.Severity += MultipleOrgasmsSettings.drained_severity_increase_per_orgasm;
            }
            float orgasm_time_increase = 1.0f + drainedHediff.Severity;
            sexInstance.sex_ticks = (int)(sexInstance.sex_ticks * orgasm_time_increase);
        }


        /// <summary>
        /// Helps to get the Heated Up Hediff of a Pawn. If it does not exist, one is added. 
        /// </summary>
        /// <param name="orgasmed">The pawn that had the orgasm, for which a hediff is looked up or created.</param>
        /// <returns></returns>
        public static Hediff GetHeatedUpHediff(Pawn orgasmed)
        {
            Hediff heatedUpHediff = orgasmed.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.mo_heated_up);
            if (heatedUpHediff == null)
            {
                heatedUpHediff = HediffMaker.MakeHediff(HediffDefOf.mo_heated_up, orgasmed);
                heatedUpHediff.Severity = 0;
                orgasmed.health.AddHediff(heatedUpHediff);
            }
            return heatedUpHediff;
        }


        /// <summary>
        /// Helps to get the Drained Hediff of a Pawn. If it does not exist, one is added. 
        /// </summary>
        /// <param name="orgasmed">The pawn that had the orgasm, for which a hediff is looked up or created.</param>
        /// <returns></returns>
        public static Hediff GetDrainedHediff(Pawn orgasmed)
        {
            Hediff drainedHediff = orgasmed.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.mo_drained);
            if (drainedHediff == null)
            {
                drainedHediff = HediffMaker.MakeHediff(HediffDefOf.mo_drained, orgasmed);
                drainedHediff.Severity = 0;
                orgasmed.health.AddHediff(drainedHediff);
            }
            return drainedHediff;
        }

    }


}
