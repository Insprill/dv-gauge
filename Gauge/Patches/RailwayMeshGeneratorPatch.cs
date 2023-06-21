using System.Linq;
using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailwayMeshGenerator), "Start")]
    public static class RailwayMeshGenerator_Start_Patch
    {
        private static void Prefix(RailwayMeshGenerator __instance)
        {
            if (Gauge.Instance.RailGauge.IsStandard())
                return;

            RailTrack[] railTracks = Object.FindObjectsOfType<RailTrack>();

            foreach (BaseType baseType in railTracks.Select(track => track.baseType).Distinct())
                UpdateBaseType(baseType);
            foreach (RailType railType in railTracks.Select(rt => rt.railType).Distinct())
                railType.gauge = Gauge.Instance.RailGauge.Gauge;

            Mesh anchorMesh = Assets.GetMesh(__instance.anchorMesh);
            if (anchorMesh == null)
                return;
            __instance.anchorMesh = anchorMesh;
            Symmetrical.ScaleToGauge(anchorMesh);
        }

        private static void UpdateBaseType(BaseType baseType)
        {
            baseType.sleeperDistance = Gauge.Instance.RailGauge.SleeperSpacing;
            if (baseType.baseShape != null && Gauge.Instance.Settings.adjustBallastWidth.Value)
                Symmetrical.ScaleToGauge(baseType.baseShape.transform);
            foreach (MeshFilter filter in baseType.sleeperPrefabs.SelectMany(obj => obj.GetComponentsInChildren<MeshFilter>()))
            {
                Mesh mesh = Assets.GetMesh(filter.sharedMesh);
                if (mesh == null || !mesh.isReadable)
                    continue;
                filter.sharedMesh = mesh;
                Symmetrical.ScaleToGauge(mesh);
            }
        }
    }
}
