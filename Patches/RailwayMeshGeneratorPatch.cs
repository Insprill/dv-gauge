using System.Collections.Generic;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailwayMeshGenerator), "Start")]
    public static class RailwayMeshGenerator_Start_Patch
    {
        private static bool Prefix()
        {
            if (Main.Settings.gauge == Gauge.Standard)
                return true;

            Main.Logger.Log($"Changing gauge to {Main.Settings.gauge}");

            RailTrack[] railTracks = Object.FindObjectsOfType<RailTrack>();

            RailType standardRailType = railTracks[0].railType;
            RailType railType = standardRailType.Clone();
            railType.gauge = Main.Settings.gauge.GetGauge();
            railType.railEdgeOffset = Main.Settings.gauge.GetEdgeOffset();

            BaseType standardBaseType = railTracks[0].baseType;
            BaseType baseType = standardBaseType.Clone();
            baseType.sleeperDistance = Main.Settings.gauge.GetSleeperDistance();

            foreach (RailTrack railTrack in railTracks)
            {
                railTrack.railType = railType;
                railTrack.baseType = baseType;
                if (railTrack.isJunctionTrack && Main.Settings.switchType == SwitchType.Dynamic) railTrack.generateMeshes = true;
            }

            Main.Logger.Log("Modifying static meshes");

            HashSet<Mesh> modifiedMeshes = new HashSet<Mesh>();
            foreach (MeshFilter filter in Object.FindObjectsOfType<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (modifiedMeshes.Contains(mesh))
                    continue;
                switch (mesh.name)
                {
                    case "rails_static": {
                        if (Main.Settings.switchType == SwitchType.Modified)
                        {
                            StaticSwitch.ModifyMesh(mesh);
                            modifiedMeshes.Add(mesh);
                        }
                        else
                        {
                            filter.GetComponent<MeshRenderer>().enabled = false;
                        }

                        break;
                    }
                    case "rails_moving": {
                        if (Main.Settings.switchType == SwitchType.Modified)
                        {
                            MovingSwitch.ModifyMesh(mesh);
                            modifiedMeshes.Add(mesh);
                        }
                        else
                        {
                            filter.GetComponent<MeshRenderer>().enabled = false;
                        }

                        break;
                    }
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
                        modifiedMeshes.Add(mesh);
                        // This is fine, right?
                        Axle.ModifyMesh(mesh);
                        break;
                }
            }

            return true;
        }
    }
}
