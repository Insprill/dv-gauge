using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailwayMeshGenerator), "Start")]
    public static class RailwayMeshGenerator_Start_Patch
    {
        private static Material s_defaultMat;
        private static Material s_newMat = DV.Globals.G.GetRailType(000).railType.railMaterial;
        private static Material s_medMat = DV.Globals.G.GetRailType(050).railType.railMaterial;
        private static Material s_oldMat = DV.Globals.G.GetRailType(100).railType.railMaterial;

        private static void Prefix(RailwayMeshGenerator __instance)
        {
            // Will only be null the first time, so it's safe to
            // use to return to default.
            // It's the same as the medium.
            if (s_defaultMat == null)
            {
                s_defaultMat = __instance.railMat;
            }

            switch (Gauge.Settings.railMaterial)
            {
                case RailMaterial.New:
                    __instance.railMat = s_newMat;
                    break;
                case RailMaterial.Medium:
                    __instance.railMat = s_medMat;
                    break;
                case RailMaterial.Old:
                    __instance.railMat = s_oldMat;
                    break;
                default:
                    __instance.railMat = s_defaultMat;
                    break;
            }

            if (Gauge.Settings.RailGauge.IsStandard() && Gauge.Settings.RailQuality.IsVanilla())
            {
                Gauge.Logger.Log("Gauge settings are fully vanilla, skipping.");
                return;
            }

            Mesh anchorMesh = Assets.GetMesh(__instance.anchorMesh);
            if (anchorMesh == null)
            {
                return;
            }
            __instance.anchorMesh = anchorMesh;
            Symmetrical.ScaleToGauge(anchorMesh);

            // Clear the cache so it can be run again on the next load.
            RailTrackPatch.ClearCache();
        }
    }
}
