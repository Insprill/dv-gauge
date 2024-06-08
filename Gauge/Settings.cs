using System;
using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        private const ushort MIN_RAIL_WIDTH = 350;
        private const ushort MAX_RAIL_WIDTH = 1700;
        private const ushort MIN_SLEEPER_SPACING = 350;
        private const ushort MAX_SLEEPER_SPACING = 2000;
        private const float MIN_KINK_SCALE = 0.0f;
        private const float MAX_KINK_SCALE = 2.0f;
        private const float MIN_KINK_FREQUENCY = 0.0f;
        private const float MAX_KINK_FREQUENCY = 2.0f;

        [Header("All settings require a restart")]
        [Draw("Rail Gauge Preset")]
        public RailGaugePreset railGaugePreset = RailGaugePreset.Standard;

        [Draw("Rail Gauge", VisibleOn = "railGaugePreset|0")] // Only show when preset is Custom
        public RailGauge railGauge = RailGauge.PRESETS[RailGaugePreset.Standard];

        public RailGauge RailGauge => railGaugePreset == RailGaugePreset.Custom ? railGauge : railGaugePreset.RailGauge();

        [Draw("Rail Quality Preset")]
        public RailQualityPreset railQualityPreset = RailQualityPreset.Vanilla;

        [Draw("Rail Quality", VisibleOn = "railQualityPreset|0")] // Only show when preset is Custom
        public RailQuality railQuality = RailQuality.PRESETS[RailQualityPreset.Vanilla];

        public RailQuality RailQuality => railQualityPreset == RailQualityPreset.Custom ? railQuality : railQualityPreset.RailQuality();

        [Draw("Adjust Ballast Width", Tooltip = "Whether track ballast should be adjusted according to the track gauge. May cause holes in the map.")]
        public bool adjustBallastWidth = true;

        [Draw("Enable Hidden settings", Tooltip = "Shows hidden settings that may cause things to break. Support will not be given if these cause issues!")]
        public bool enableHiddenSettings;

        [Draw("Remove Gauge Restrictions", Tooltip = "Removes restrictions from custom track width.", VisibleOn = "enableHiddenSettings|true")]
        public bool removeGaugeRestrictions;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            if (!enableHiddenSettings || !removeGaugeRestrictions)
            {
                // The normal min/max properties make it impossible to type, so we just do the validation here.
                railGauge = new RailGauge(
                    (short)Mathf.Clamp(RailGauge.GaugeMillimeters, MIN_RAIL_WIDTH, MAX_RAIL_WIDTH),
                    (short)Mathf.Clamp(RailGauge.SleeperSpacingMillimeters, MIN_SLEEPER_SPACING, MAX_SLEEPER_SPACING)
                );
                
                railQuality = new RailQuality(
                    Mathf.Clamp(RailQuality.KinkScale, MIN_KINK_SCALE, MAX_KINK_SCALE),
                    Mathf.Clamp(RailQuality.KinkFrequency, MIN_KINK_FREQUENCY, MAX_KINK_FREQUENCY)
                );
            }

            Save(this, modEntry);
        }

        public void OnChange()
        {
            // yup
        }
    }
}
