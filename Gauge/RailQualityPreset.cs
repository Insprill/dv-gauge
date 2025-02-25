namespace Gauge
{
    public enum RailQualityPreset
    {
        Custom = 0,
        Vanilla = 1,
        HighSpeedCorridor = 2,
        DisusedBranchLine = 3
    }

    public static class RailQualityPresetExtensions
    {
        public static RailQuality RailQuality(this RailQualityPreset preset)
        {
            return global::Gauge.RailQuality.PRESETS[preset];
        }
    }
}
