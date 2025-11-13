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
            // Apply material to fixed-mesh tracks from turntables.
            if (TryUpdateTurntableParkingTrack(__instance))
            {
                return;
            }

            // Apply material to fixed-mesh tracks on turntables.
            if (TryUpdateTurntableTrack(__instance))
            {
                return;
            }

            // Apply material to base type for regular tracks and adjust kinks

            UpdateBaseType(__instance.baseType);

            __instance.railType.gauge = Gauge.Settings.RailGauge.Gauge;

            __instance.railType.kinkScale = Gauge.Settings.RailQuality.KinkScale;
            __instance.railType.kinkFrequency = Gauge.Settings.RailQuality.KinkFrequency;
            __instance.railType.verticalKinkScale = Gauge.Settings.RailQuality.VerticalKinkScale;
            __instance.railType.rotationKinkScale = Gauge.Settings.RailQuality.RotationKinkScale;

            __instance.overrideDefaultJointsSpan = Gauge.Settings.customJointDistance;
            __instance.jointsSpan = Gauge.Settings.jointDistance;
        }

        static bool TryUpdateTurntableParkingTrack(RailTrack __instance)
        {
            if (!__instance.TryGetComponent<TurntableOutgoingTrack>(out _))
            {
                return false;
            }

            var visualTransform = __instance.transform.Find("visual");
            // This component is on some buffer stops without a fixed mesh.
            if (visualTransform == null)
            {
                return false;
            }

            var meshTransform = visualTransform.Find("roundhouse_railroad_track");
            var renderer = meshTransform.GetComponent<MeshRenderer>();
            // Museum tracks have one material, others have two. Both have the rail as the first.
            var sharedMats = renderer.sharedMaterials;
            sharedMats[0] = RailMaterials.GetSelectedRailMaterial(renderer.sharedMaterial);
            renderer.sharedMaterials = sharedMats;
            return true;
        }

        static bool TryUpdateTurntableTrack(RailTrack __instance)
        {
            if (!__instance.TryGetComponent<TurntableRailTrack>(out _))
                return false;
            var visualTransform = __instance.transform.Find("bridge").Find("visual");
            var railTransform = visualTransform.GetChild(0);
            var renderer = railTransform.GetComponent<MeshRenderer>();
            var mat = RailMaterials.GetSelectedRailMaterial(renderer.sharedMaterial);
            var sharedMats = renderer.sharedMaterials;
            switch (railTransform.name)
            {
                case "RailwayMuseumTurntable_LOD0":
                    sharedMats[2] = mat;
                    break;
                case "TurntableRail":
                    sharedMats[1] = mat;
                    break;
                default:
                    Gauge.Logger.Warning($"Unknown turntable '{railTransform.name}'! It's material won't change.");
                    break;
            }
            renderer.sharedMaterials = sharedMats;
            return true;
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
