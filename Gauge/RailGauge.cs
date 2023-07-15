using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    [Serializable]
    [DrawFields(DrawFieldMask.OnlyDrawAttr)]
    public struct RailGauge
    {
        public static readonly ReadOnlyDictionary<RailGaugePreset, RailGauge> PRESETS = new ReadOnlyDictionary<RailGaugePreset, RailGauge>(new Dictionary<RailGaugePreset, RailGauge> {
            { RailGaugePreset.Standard, new RailGauge(1435, 750) },
            { RailGaugePreset.Cape, new RailGauge(1067, 850) },
            { RailGaugePreset.ThreeFoot, new RailGauge(914, 900) }
        });

        [Draw("Gauge (millimeters)", Tooltip = "The track gauge, in millimeters. Must be greater than 350 and less than 1700.")]
        public int GaugeMillimeters;
        [Draw("Sleeper Spacing (millimeters)", Tooltip = "The distance, in millimeters, between the center of each sleeper. Doesn't apply to switches. Must be greater than 350 and less than 2000.")]
        public int SleeperSpacingMillimeters;

        public float Gauge => GaugeMillimeters / 1000f;
        public float SleeperSpacing => SleeperSpacingMillimeters / 1000f;

        public float DiffToStandard => GetDiffFrom(PRESETS[RailGaugePreset.Standard].Gauge);

        public RailGauge(int gauge, int sleeperSpacing)
        {
            GaugeMillimeters = gauge;
            SleeperSpacingMillimeters = sleeperSpacing;
        }

        public float GetDiffFrom(float from)
        {
            return (from - Gauge) / 2;
        }

        public bool IsStandard()
        {
            return Mathf.Abs(Gauge - PRESETS[RailGaugePreset.Standard].Gauge) < 0.001;
        }
    }
}
