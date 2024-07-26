using Gauge.MeshModifiers;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailTrack))]
    internal static class RailTrackPatch
    {
        private static HashSet<BaseType> s_updatedBaseTypes = new HashSet<BaseType>();

        // Point sets are generated and cached on Awake, which means when RailwayMeshGenerator
        // accesses them to change params it's already too late.
        // Instead of nullifying that cache it's easier to just apply it here.
        [HarmonyPrefix, HarmonyPatch("Awake")]
        private static void AwakePrefix(RailTrack __instance)
        {
            UpdateBaseType(__instance.baseType);

            __instance.railType.gauge = Gauge.Settings.RailGauge.Gauge;

            __instance.railType.kinkScale = Gauge.Settings.RailQuality.KinkScale;
            __instance.railType.kinkFrequency = Gauge.Settings.RailQuality.KinkFrequency;
            __instance.railType.verticalKinkScale = Gauge.Settings.RailQuality.VerticalKinkScale;
            __instance.railType.rotationKinkScale = Gauge.Settings.RailQuality.RotationKinkScale;
        }

        private static void UpdateBaseType(BaseType baseType)
        {
            // Since this step is a bit more costly than the rest, cache to avoid repeating it.
            if (s_updatedBaseTypes.Contains(baseType))
            {
                return;
            }

            baseType.sleeperDistance = Gauge.Settings.RailGauge.SleeperSpacing;

            if (baseType.baseShape != null && Gauge.Settings.adjustBallastWidth)
            {
                Symmetrical.ScaleToGauge(baseType.baseShape.transform);
            }

            foreach (MeshFilter filter in baseType.sleeperPrefabs.SelectMany(obj => obj.GetComponentsInChildren<MeshFilter>()))
            {
                Mesh mesh = Assets.GetMesh(filter.sharedMesh);
                if (mesh == null || !mesh.isReadable)
                    continue;
                filter.sharedMesh = mesh;
                Symmetrical.ScaleToGauge(mesh);
            }

            s_updatedBaseTypes.Add(baseType);
        }

        public static void ClearCache()
        {
            s_updatedBaseTypes.Clear();
        }
    }
}
