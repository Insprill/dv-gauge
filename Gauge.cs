using System;
using UnityEngine;

namespace Gauge
{
    public enum Gauge
    {
        ThreeFoot,
        Cape,
        Standard,
        Custom
    }

    public static class GaugeExtensions
    {
        // Width between rail shape center and the edge of rail head. Value from RailType.
        public const float railEdgeOffset = 0.0351f;

        public static float GetGauge(this Gauge gauge)
        {
            switch (gauge)
            {
                case Gauge.ThreeFoot:
                    return 0.914f;
                case Gauge.Cape:
                    return 1.067f;
                case Gauge.Standard:
                    return 1.435f;
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
                case Gauge.ThreeFoot:
                    return 0.9f;
                case Gauge.Cape:
                    return 0.85f;
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
            return gauge.GetDiffFrom(Gauge.Standard.GetGauge());
        }

        public static float GetDiffFrom(this Gauge to, float from)
        {
            return (from - to.GetGauge()) / 2;
        }

        public static bool IsStandard(this Gauge gauge)
        {
            return Mathf.Abs(gauge.GetGauge() - Gauge.Standard.GetGauge()) < 0.001;
        }
    }
}
