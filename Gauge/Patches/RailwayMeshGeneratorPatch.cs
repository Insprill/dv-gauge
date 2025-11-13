using Gauge.MeshModifiers;
using HarmonyLib;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailwayMeshGenerator), "Start")]
    public static class RailwayMeshGenerator_Start_Patch
    {
        static void Prefix(RailwayMeshGenerator __instance)
        {
            __instance.railMat = RailMaterials.GetSelectedRailMaterial(__instance.railMat);

            if (Gauge.Settings.RailGauge.IsStandard() && Gauge.Settings.RailQuality.IsVanilla())
            {
                Gauge.Logger.Log("Gauge settings are fully vanilla, skipping.");
                return;
            }

            if (Assets.GetMesh(__instance.anchorMesh).IsSome(out var aMesh))
            {
                __instance.anchorMesh = aMesh;
                Symmetrical.ScaleToGauge(aMesh);
            }

            // Clear the cache so it can be run again on the next load.
            RailTrackPatch.ClearCache();
        }
    }
}
