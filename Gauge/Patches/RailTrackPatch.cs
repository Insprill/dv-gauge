using System.Collections.Generic;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailTrack))]
    static class RailTrackPatch
    {
        static readonly HashSet<BaseType> s_updatedBaseTypes = [];

        // Point sets are generated and cached on Awake, which means when RailwayMeshGenerator
        // accesses them to change params it's already too late.
        // Instead of nullifying that cache it's easier to just apply it here.
        [HarmonyPrefix, HarmonyPatch("Awake")]
        static void AwakePrefix(RailTrack __instance)
        {
            UpdateBaseType(__instance.baseType);

            // Don't adjust kinks for tracks with static meshes
            if (!__instance.generateMeshes)
                return;

            __instance.railType.gauge = Gauge.Settings.RailGauge.Gauge;

            __instance.railType.kinkScale = Gauge.Settings.RailQuality.KinkScale;
            __instance.railType.kinkFrequency = Gauge.Settings.RailQuality.KinkFrequency;
            __instance.railType.verticalKinkScale = Gauge.Settings.RailQuality.VerticalKinkScale;
            __instance.railType.rotationKinkScale = Gauge.Settings.RailQuality.RotationKinkScale;

            __instance.overrideDefaultJointsSpan = Gauge.Settings.customJointDistance;
            __instance.jointsSpan = Gauge.Settings.jointDistance;
        }

        static void UpdateBaseType(BaseType baseType)
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

            foreach (var obj in baseType.sleeperPrefabs)
            {
                using var filters = TempList<MeshFilter>.Get;
                obj.GetComponentsInChildren(filters.List);
                foreach (var filter in filters.List)
                {
                    if (!Assets.GetMesh(filter).IsSome(out var mesh))
                        continue;
                    if (!mesh.isReadable)
                        continue;
                    filter.sharedMesh = mesh;
                    Symmetrical.ScaleToGauge(mesh);
                }
            }

            s_updatedBaseTypes.Add(baseType);
        }

        public static void ClearCache()
        {
            s_updatedBaseTypes.Clear();
        }
    }
}
