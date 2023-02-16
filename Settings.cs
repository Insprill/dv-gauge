using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Header("All settings require a restart")]
        [Draw("Rail gauge")]
        public Gauge gauge = Gauge.ThreeFootNarrow;

        [Draw("Width (meters)", Tooltip = "The track gauge, in meters. Anything less than 0.5 or greater than 2.0 may cause issues.", VisibleOn = "gauge|Custom", Precision = 3)]
        public float width = 1.345f;

        [Draw("Sleeper Spacing (meters)", Tooltip = "The distance, in meters, between each sleeper. Doesn't apply to switches.", VisibleOn = "gauge|Custom", Min = 0.25f, Max = 2.0f)]
        public float sleeperSpacing = 0.75f;

        [Draw("Switch Type", Tooltip = "Dynamic switches are smoother but don't have points or frogs")]
        public SwitchType switchType = SwitchType.Dynamic;

        [Draw("Adjust Ballast Width (requires restart)", Tooltip = "Whether track ballast should be adjusted according to the track gauge. May cause holes in the map.")]
        public bool adjustBallastWidth = true;

        public void OnChange()
        {
            // yup
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    public enum SwitchType
    {
        Dynamic,
        Modified
    }
}
