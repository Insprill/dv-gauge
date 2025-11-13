using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

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
