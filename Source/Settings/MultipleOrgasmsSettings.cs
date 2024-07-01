using rjw;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using SettingsHelper;

namespace Multipleorgasm.Settings
{
    public class MultipleOrgasmsSettings : ModSettings
    {
        public enum Behavior
        {
            None,
            SpeedUp,
            SlowDown
        };

        public static Behavior maleBehavior = Behavior.SlowDown;
        public static Behavior femaleBehavior = Behavior.SpeedUp;
        public static Behavior futaBehavior = Behavior.SpeedUp;


        public static float heatup_severity_increase_per_orgasm = 0.1f;
        public static float drained_severity_increase_per_orgasm = 0.15f;

        public static float min_ticks_to_orgasm = 180f; //180 Ticks = 3 Seconds at normal speed

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref maleBehavior, "mo_male_behavior", maleBehavior, true);
            Scribe_Values.Look(ref femaleBehavior, "mo_female_behavior", femaleBehavior, true);
            Scribe_Values.Look(ref futaBehavior, "mo_futa_behavior", futaBehavior, true);


            Scribe_Values.Look(ref heatup_severity_increase_per_orgasm, "mo_heatup_severity_increase", heatup_severity_increase_per_orgasm, true);
            Scribe_Values.Look(ref drained_severity_increase_per_orgasm, "mo_drained_severity_increase", drained_severity_increase_per_orgasm, true);
            Scribe_Values.Look(ref min_ticks_to_orgasm, "mo_min_ticks_to_orgasm", min_ticks_to_orgasm, true);
        }

        public  static void DoWindowContents(Rect inRect)
        {
            Rect outRect = new Rect(0f, 30f, inRect.width, inRect.height - 30f);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, inRect.height + 300f);

            Listing_Standard listing_Standard = new Listing_Standard(); 
            listing_Standard.maxOneColumn = true;
            listing_Standard.ColumnWidth = viewRect.width / 2.05f;
            listing_Standard.Begin(viewRect);
            listing_Standard.Gap(30f);

            listing_Standard.Label("Minimum Ticks to come: " + Math.Round(min_ticks_to_orgasm), -1f, "How many ingame ticks are minimum necessary to orgasm? 180 are ~3 seconds of wall-clock time.");
            min_ticks_to_orgasm = listing_Standard.Slider(min_ticks_to_orgasm, 180f, 10000f);
            listing_Standard.Gap(10f);
            listing_Standard.Label("Heatup Per Orgasm: " + Math.Round(heatup_severity_increase_per_orgasm,2), -1.0f, "How much severity gains the heat-up-hediff on a single orgasm");
            heatup_severity_increase_per_orgasm = listing_Standard.Slider(heatup_severity_increase_per_orgasm, 0.05f, 1.0f);
            listing_Standard.Gap(10f);
            listing_Standard.Label("Drain Per Orgasm: " + Math.Round(drained_severity_increase_per_orgasm, 2), -1.0f, "How much severity gains the drain-hediff on a single orgasm");
            drained_severity_increase_per_orgasm = listing_Standard.Slider(drained_severity_increase_per_orgasm, 0.05f, 1.0f);

            listing_Standard.Gap(30f);

            listing_Standard.AddLabeledSlider<Behavior>("Male Behavior", ref maleBehavior);
            listing_Standard.Gap(10f);

            listing_Standard.AddLabeledSlider<Behavior>("Female Behavior", ref femaleBehavior);
            listing_Standard.Gap(10f);

            listing_Standard.AddLabeledSlider<Behavior>("Futa Behavior", ref futaBehavior);
            listing_Standard.Gap(10f);
        }


    }
}
