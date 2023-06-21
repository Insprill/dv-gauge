using UnityEngine;

namespace Gauge
{
    public class RailGauge
    {
        public static readonly RailGauge STANDARD = new RailGauge(1435, 750);
        public readonly float Gauge;
        public readonly float SleeperSpacing;

        public float DiffToStandard => GetDiffFrom(STANDARD.Gauge);

        public RailGauge(int gauge, int sleeperSpacing)
        {
            Gauge = Mathf.Clamp(gauge / 1000.0f, 0, 10);
            SleeperSpacing = Mathf.Clamp(sleeperSpacing / 1000.0f, 0, 10);
        }

        public float GetDiffFrom(float from)
        {
            return (from - Gauge) / 2;
        }

        public bool IsStandard()
        {
            return Mathf.Abs(Gauge - STANDARD.Gauge) < 0.001;
        }
    }
}
