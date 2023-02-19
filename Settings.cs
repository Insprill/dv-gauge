using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Header("All settings require a restart")]
        [Draw("Rail gauge")]
        public Gauge gauge = Gauge.ThreeFoot;

        [Draw("Width (meters)", Tooltip = "The track gauge, in millimeters. Must be greater than 350 and less than 2000.", VisibleOn = "gauge|Custom")]
        public int width = 1435;

        [Draw("Sleeper Spacing (meters)", Tooltip = "The distance, in meters, between each sleeper. Doesn't apply to switches. Must be greater than 350 and less than 2000.", VisibleOn = "gauge|Custom")]
        public int sleeperSpacing = 750;

        [Draw("Adjust Ballast Width", Tooltip = "Whether track ballast should be adjusted according to the track gauge. May cause holes in the map.")]
        public bool adjustBallastWidth = true;

        public void OnChange()
        {
            // yup
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            // The normal min/max properties make it impossible to type, so we just do the validation here.
            width = (int)ClampGauge(width);
            sleeperSpacing = Mathf.Clamp(sleeperSpacing, 350, 2000);
            Save(this, modEntry);
        }

        public static float ClampGauge(float gaugeMillis)
        {
            return Mathf.Clamp(gaugeMillis, 350, 1435);
        }
    }
}
