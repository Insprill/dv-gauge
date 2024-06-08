using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    [Serializable]
    [DrawFields(DrawFieldMask.OnlyDrawAttr)]
    public struct RailQuality
    {
        public static readonly ReadOnlyDictionary<RailQualityPreset, RailQuality> PRESETS = new ReadOnlyDictionary<RailQualityPreset, RailQuality>(new Dictionary<RailQualityPreset, RailQuality> {
            { RailQualityPreset.Vanilla, new RailQuality(0.7f, 0.7f) },
            { RailQualityPreset.HighSpeedCorridor, new RailQuality(0.1f, 0.7f) },
            { RailQualityPreset.DisusedBranchLine, new RailQuality(1.2f, 1.1f) }
        });

        [Draw("Kink Scale", Tooltip = "How much to scale the kinks of the track.")]
        public float KinkScale;
        [Draw("Kink Frequency", Tooltip = "How often kinks show up in the track.")]
        public float KinkFrequency;

        public RailQuality(float kinkScale, float kinkFrequency)
        {
            KinkScale = kinkScale;
            KinkFrequency = kinkFrequency;
        }
        
        public bool IsVanilla()
        {
            return Mathf.Approximately(KinkScale, PRESETS[RailGaugePreset.Vanilla].KinkScale) && Mathf.Approximately(KinkFrequency, PRESETS[RailGaugePreset.Vanilla].KinkFrequency);
        }
    }
}
