using System;
using UnityModManagerNet;

namespace Gauge
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Rail gauge (requires restart)")]
        public Gauge gauge = Gauge.ThreeFootNarrow;

        public void OnChange()
        {
            // yup
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    public enum Gauge
    {
        ThreeFootNarrow,
        Standard
    }

    public static class GaugeExtensions
    {
        public static float GetGauge(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.ThreeFootNarrow:
                    return 0.9144f;
                case Gauge.Standard:
                    throw new InvalidOperationException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }
        public static float GetEdgeOffset(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.ThreeFootNarrow:
                    return 0.0144f;
                case Gauge.Standard:
                    throw new InvalidOperationException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }
    }
}
