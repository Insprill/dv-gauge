using System;

namespace Gauge
{
    public enum Gauge
    {
        TwoFoot,
        ThreeFoot,
        Cape,
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
                case Gauge.TwoFoot:
                    return 0.6096f;
                case Gauge.ThreeFoot:
                    return 0.9144f;
                case Gauge.Cape:
                    return 1.067f;
                case Gauge.Standard:
                    return 1.3451f;
                case Gauge.Custom:
                    return Main.Settings.width / 1000.0f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gauge), gauge, null);
            }
        }

        public static float GetSleeperDistance(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.TwoFoot:
                    return 1.0f;
                case Gauge.ThreeFoot:
                    return 0.9f;
                case Gauge.Cape:
                    return 0.8f;
                case Gauge.Standard:
                    return 0.75f; // From RailType
                case Gauge.Custom:
                    return Main.Settings.sleeperSpacing / 1000.0f;
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
