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

        [Draw("Adjust Ballast Width", Tooltip = "Whether track ballast should be adjusted according to the track gauge. May cause holes in the map.")]
        public bool adjustBallastWidth = true;

        [Draw("Enable Hidden settings", Tooltip = "Shows hidden settings that may cause things to break. Support will not be given if these cause issues!")]
        public bool enableHiddenSettings = false;

        [Draw("Remove Gauge Restrictions", Tooltip = "Removes restrictions from custom track width.", VisibleOn = "enableHiddenSettings|true")]
        public bool removeGaugeRestrictions = false;

        public void OnChange()
        {
            // yup
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            if (!enableHiddenSettings || !removeGaugeRestrictions)
            {
                // The normal min/max properties make it impossible to type, so we just do the validation here.
                width = Mathf.Clamp(width, MIN_RAIL_WIDTH, MAX_RAIL_WIDTH);
                sleeperSpacing = Mathf.Clamp(sleeperSpacing, MIN_SLEEPER_SPACING, MAX_SLEEPER_SPACING);
            }

            Save(this, modEntry);
        }
    }
}
