using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        private const ushort MIN_RAIL_WIDTH = 350;
        private const ushort MAX_RAIL_WIDTH = 1600;
        private const ushort MIN_SLEEPER_SPACING = 350;
        private const ushort MAX_SLEEPER_SPACING = 2000;

        [Header("All settings require a restart")]
        [Draw("Rail gauge")]
        public Gauge gauge = Gauge.ThreeFoot;

        [Draw("Width (meters)", Tooltip = "The track gauge, in millimeters. Must be greater than 350 and less than 1600.", VisibleOn = "gauge|Custom")]
        public int width = 1435;

        [Draw("Sleeper Spacing (meters)", Tooltip = "The distance, in millimeters, between the center of each sleeper. Doesn't apply to switches. Must be greater than 350 and less than 2000.", VisibleOn = "gauge|Custom")]
        public int sleeperSpacing = 750;

        [Draw("Allow Unsafe Values", Tooltip = "Removes restrictions on track width. Using unsafe values are NOT supported, and things WILL break.", VisibleOn = "gauge|Custom")]
        public bool allowUnsafeValues = false;

        [Draw("Adjust Ballast Width", Tooltip = "Whether track ballast should be adjusted according to the track gauge. May cause holes in the map.")]
        public bool adjustBallastWidth = true;

        public void OnChange()
        {
            // yup
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            if (!allowUnsafeValues)
            {
                // The normal min/max properties make it impossible to type, so we just do the validation here.
                width = (int)ClampGauge(width);
                sleeperSpacing = Mathf.Clamp(sleeperSpacing, MIN_SLEEPER_SPACING, MAX_SLEEPER_SPACING);
            }

            Save(this, modEntry);
        }

        public static float ClampGauge(float gaugeMillis)
        {
            return Mathf.Clamp(gaugeMillis, MIN_RAIL_WIDTH, MAX_RAIL_WIDTH);
        }
    }
}
