using System.Collections.Generic;
using System.Linq;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailwayMeshGenerator), "Start")]
    public static class RailwayMeshGenerator_Start_Patch
    {
        private static bool Prefix(RailwayMeshGenerator __instance)
        {
            if (Main.Settings.gauge.IsStandard())
                return true;

            Main.Logger.Log($"Changing gauge to {Main.Settings.gauge}");

            RailTrack[] railTracks = Object.FindObjectsOfType<RailTrack>();

            BaseType baseType = railTracks[0].baseType.Clone();
            baseType.sleeperDistance = Main.Settings.gauge.GetSleeperDistance();
            if (Main.Settings.adjustBallastWidth) Symmetrical.ScaleToGauge(baseType.baseShape);
            foreach (MeshFilter filter in baseType.sleeperPrefabs.SelectMany(obj => obj.GetComponentsInChildren<MeshFilter>()))
            {
                Mesh mesh = filter.sharedMesh;
                if (!mesh.isReadable || mesh.IsModified())
                    continue;
                Symmetrical.ScaleToGauge(mesh);
                mesh.SetModified();
            }

            if (__instance.anchorMesh != null && __instance.anchorMesh.isReadable)
            {
                Symmetrical.ScaleToGauge(__instance.anchorMesh);
                __instance.anchorMesh.SetModified();
            }

            RailType railType = railTracks[0].railType.Clone();
            railType.gauge = Main.Settings.gauge.GetGauge() - GaugeExtensions.railEdgeOffset * 2;
            foreach (RailTrack railTrack in railTracks)
            {
                railTrack.railType = railType;
                railTrack.baseType = baseType;
            }

            return true;
        }
    }
}
