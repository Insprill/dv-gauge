using System;

namespace Gauge
{
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
                    return 1.3451f;
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
                    return 0.0451f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }
        public static float GetSleeperDistance(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.ThreeFootNarrow:
                    return 0.9f;
                case Gauge.Standard:
                    throw new InvalidOperationException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }
        public static float GetDiffToStandard(this Gauge gauge)
        {
            // TODO: Doesn't work with all gauges
            return (Gauge.Standard.GetGauge() - gauge.GetGauge()) / 2 + (Gauge.Standard.GetEdgeOffset() + gauge.GetEdgeOffset() + 0.005f);
        }
    }
}
