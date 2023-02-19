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
            if (Main.Settings.gauge == Gauge.Standard)
                return true;

            Main.Logger.Log($"Changing gauge to {Main.Settings.gauge}");

            HashSet<Mesh> modifiedMeshes = new HashSet<Mesh>();
            RailTrack[] railTracks = Object.FindObjectsOfType<RailTrack>();

            BaseType baseType = railTracks[0].baseType.Clone();
            baseType.sleeperDistance = Main.Settings.gauge.GetSleeperDistance();
            if (Main.Settings.adjustBallastWidth) Symmetrical.ScaleToGauge(baseType.baseShape);
            foreach (MeshFilter filter in baseType.sleeperPrefabs.SelectMany(obj => obj.GetComponentsInChildren<MeshFilter>()))
            {
                Mesh mesh = filter.sharedMesh;
                if (modifiedMeshes.Contains(mesh))
                    continue;
                Symmetrical.ScaleToGauge(mesh);
                modifiedMeshes.Add(mesh);
            }

            if (__instance.anchorMesh != null)
            {
                Symmetrical.ScaleToGauge(__instance.anchorMesh);
                modifiedMeshes.Add(__instance.anchorMesh);
            }

            RailType railType = railTracks[0].railType.Clone();
            railType.gauge = Main.Settings.gauge.GetGauge() - (GaugeExtensions.railEdgeOffset * 2);
            foreach (RailTrack railTrack in railTracks)
            {
                railTrack.railType = railType;
                railTrack.baseType = baseType;
                if (railTrack.isJunctionTrack && Main.Settings.switchType == SwitchType.Dynamic) railTrack.generateMeshes = true;
            }

            Main.Logger.Log("Modifying static meshes");

            foreach (MeshFilter filter in Object.FindObjectsOfType<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (modifiedMeshes.Contains(mesh))
                    continue;
                switch (mesh.name)
                {
                    // Switches
                    case "rails_static":
                    case "rails_moving": {
                        if (Main.Settings.switchType == SwitchType.Dynamic)
                        {
                            filter.GetComponent<MeshRenderer>().enabled = false;
                            break;
                        }

                        if (mesh.name.Equals("rails_moving"))
                            MovingSwitch.ModifyMesh(mesh);
                        else
                            StaticSwitch.ModifyMesh(mesh);
                        modifiedMeshes.Add(mesh);
                        break;
                    }
                    // Switch anchors
                    case "anchors":
                        SwitchAnchors.ModifyMesh(mesh);
                        modifiedMeshes.Add(mesh);
                        break;
                    // Switch ballast
                    case "ballast" when Main.Settings.adjustBallastWidth:
                    case "ballast-outersign" when Main.Settings.adjustBallastWidth:
                    // Switch sleepers
                    case "sleepers":
                    case "sleepers-outersign":
                    // Turntable rails
                    case "TurntableRail":
                    case "TurntableRail_ShadowCaster":
                    // Roundhouse rails
                    // case "TurntableRail.002": // Not modifiable
                    // Buffer stops
                    case "buffer_stop_sleeper_n_1":
                    case "buffer_stop_rails":
                    case "buffer_stop_rails_LOD1":
                    case "buffer_stop_holders":
                        Symmetrical.ScaleToGauge(mesh);
                        modifiedMeshes.Add(mesh);
                        break;
                }
            }

            return true;
        }
    }
}
