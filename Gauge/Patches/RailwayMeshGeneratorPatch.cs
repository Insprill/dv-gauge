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
            if (Gauge.Settings.RailGauge.IsStandard() && Gauge.Settings.RailQuality.IsVanilla())
            {
                Gauge.Logger.Log("Gauge settings are fully vanilla, skipping.");
                return;
            }

            Gauge.Logger.Log($"Preparing railway meshes with presets [{Gauge.Settings.railGaugePreset}][{Gauge.Settings.railQualityPreset}]");

            RailTrack[] railTracks = Object.FindObjectsOfType<RailTrack>();

            foreach (BaseType baseType in railTracks.Select(track => track.baseType).Distinct())
                UpdateBaseType(baseType);
            foreach (RailType railType in railTracks.Select(rt => rt.railType))
            {
                railType.gauge = Gauge.Settings.RailGauge.Gauge;
                
                railType.kinkScale = Gauge.Settings.RailQuality.KinkScale;
                railType.kinkFrequency = Gauge.Settings.RailQuality.KinkFrequency;
                railType.verticalKinkScale = Gauge.Settings.RailQuality.VerticalKinkScale;
                railType.rotationKinkScale = Gauge.Settings.RailQuality.RotationKinkScale;
            }

            Mesh anchorMesh = Assets.GetMesh(__instance.anchorMesh);
            if (anchorMesh == null)
                return;
            __instance.anchorMesh = anchorMesh;
            Symmetrical.ScaleToGauge(anchorMesh);
        }

        private static void UpdateBaseType(BaseType baseType)
        {
            baseType.sleeperDistance = Gauge.Settings.RailGauge.SleeperSpacing;
            if (baseType.baseShape != null && Gauge.Settings.adjustBallastWidth)
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
