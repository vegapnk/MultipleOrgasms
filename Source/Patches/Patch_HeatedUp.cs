using HarmonyLib;
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

        private const float SEVERITY_INCREASE_PER_ORGASM_HEATED = 0.1f;
        private const float SEVERITY_INCREASE_PER_ORGASM_DRAINED = 0.15f;

        private const int MINIMUM_TICKS_TO_COME = 180; //180 Ticks = 3 Seconds at normal speed

        public static void Postfix(JobDriver_Sex __instance)
        {
            Pawn orgasmingPawn = __instance.pawn;
            if (orgasmingPawn != null && !orgasmingPawn.IsAnimal())
            {
                if (Genital_Helper.has_vagina(orgasmingPawn)) { 
                    var heatedUpHediff = GetHeatedUpHediff(orgasmingPawn);

                    if (__instance.orgasms > 0 &&  heatedUpHediff.Severity < 1.0)
                    {
                        heatedUpHediff.Severity += SEVERITY_INCREASE_PER_ORGASM_HEATED;
                    }
                    // Adjust Time
                    float orgasm_time_reduction = Math.Max(1.0f - heatedUpHediff.Severity, 0.25f);
                    __instance.sex_ticks = (int)(__instance.sex_ticks * orgasm_time_reduction);
                    // Backup: Do not break your game, lol
                    if (__instance.sex_ticks < MINIMUM_TICKS_TO_COME)
                        __instance.sex_ticks = MINIMUM_TICKS_TO_COME;

                } else if (Genital_Helper.has_penis_fertile(orgasmingPawn)) {
                    var drainedHediff = GetDrainedHediff(orgasmingPawn);

                    if (__instance.orgasms > 0 && drainedHediff.Severity < 1.0)
                    {
                        drainedHediff.Severity += SEVERITY_INCREASE_PER_ORGASM_DRAINED;
                    }
                    float orgasm_time_increase = 1.0f + drainedHediff.Severity;
                    __instance.sex_ticks = (int)(__instance.sex_ticks * orgasm_time_increase);
                }
            }

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
