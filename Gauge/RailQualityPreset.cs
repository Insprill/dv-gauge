using UnityEngine;

namespace Gauge
{
    public enum RailQualityPreset
    {
        Custom = 0,
        Vanilla = 1,
        HighSpeedCorridor = 2,
        DisusedBranchLine = 3
    }

    public enum RailMaterial
    {
        Default,
        New,
        Medium,
        Old
    }

    public static class RailMaterials
    {
        static readonly Material s_newMat = DV.Globals.G.GetRailType(000).railType.railMaterial;
        static readonly Material s_medMat = DV.Globals.G.GetRailType(050).railType.railMaterial;
        static readonly Material s_oldMat = DV.Globals.G.GetRailType(100).railType.railMaterial;

        public static Material GetSelectedRailMaterial(Material current)
        {
            return Gauge.Settings.railMaterial switch
            {
                RailMaterial.New => s_newMat,
                RailMaterial.Medium => s_medMat,
                RailMaterial.Old => s_oldMat,
                _ => current
            };
        }
    }

    public static class RailQualityPresetExtensions
    {
        public static RailQuality RailQuality(this RailQualityPreset preset)
        {
            return global::Gauge.RailQuality.PRESETS[preset];
        }
    }
}
