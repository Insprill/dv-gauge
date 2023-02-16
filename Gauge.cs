using System;

namespace Gauge
{
    public enum Gauge
    {
        TwoFootNarrow,
        ThreeFootNarrow,
        Metre,
        Standard,
        Custom
    }

    public static class GaugeExtensions
    {
        // Width between rail shape center and the edge of rail head. Value from RailType.
        private const float railEdgeOffset = 0.0351f;

        public static float GetGauge(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.TwoFootNarrow:
                    return 0.6096f;
                case Gauge.ThreeFootNarrow:
                    return 0.9144f;
                case Gauge.Metre:
                    return 1.0f;
                case Gauge.Standard:
                    return 1.3451f;
                case Gauge.Custom:
                    return Main.Settings.width;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }

        public static float GetSleeperDistance(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.TwoFootNarrow:
                    return 1.0f;
                case Gauge.ThreeFootNarrow:
                    return 0.9f;
                case Gauge.Metre:
                    return 0.8f;
                case Gauge.Standard:
                    return 0.75f; // From RailType
                case Gauge.Custom:
                    return Main.Settings.sleeperSpacing;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }

        public static float GetDiffToStandard(this Gauge gauge)
        {
            return (Gauge.Standard.GetGauge() - gauge.GetGauge()) / 2 + railEdgeOffset;
        }
    }
}
