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
            { RailQualityPreset.Vanilla, new RailQuality(0.07f, 0.07f, 0.07f, 1.00f) },
            { RailQualityPreset.HighSpeedCorridor, new RailQuality(0.03f, 0.03f, 0.03f, 0.50f) },
            { RailQualityPreset.DisusedBranchLine, new RailQuality(0.20f, 0.20f, 0.20f, 3.00f) }
        });

        [Draw("Kink Scale", Tooltip = "The maximum offset of the kinks along the track.")]
        public float KinkScale;
        [Draw("Kink Frequency", Tooltip = "How often kinks show up along the track.")]
        public float KinkFrequency;
        [Draw("Vertical Kink Scale", Tooltip = "Extra offsetting vertically")]
        public float VerticalKinkScale;
        [Draw("Rotation Kink Scale", Tooltip = "Extra rotation")]
        public float RotationKinkScale;

        public RailQuality(float kinkScale, float kinkFrequency, float verticalKinkScale, float rotationKinkScale)
        {
            KinkScale = kinkScale;
            KinkFrequency = kinkFrequency;
            VerticalKinkScale = verticalKinkScale;
            RotationKinkScale = rotationKinkScale;
        }
        
        public bool IsVanilla()
        {
            return Mathf.Approximately(KinkScale, PRESETS[RailGaugePreset.Vanilla].KinkScale) &&
                Mathf.Approximately(KinkFrequency, PRESETS[RailGaugePreset.Vanilla].KinkFrequency) &&
                Mathf.Approximately(VerticalKinkScale, PRESETS[RailGaugePreset.Vanilla].VerticalKinkScale) &&
                Mathf.Approximately(RotationKinkScale, PRESETS[RailGaugePreset.Vanilla].RotationKinkScale);
        }
    }
}
