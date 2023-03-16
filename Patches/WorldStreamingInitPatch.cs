using System.Collections.Generic;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(WorldStreamingInit), "Awake")]
    public static class WorldStreamingInit_Awake_Patch
    {
        public static void Postfix()
        {
            if (Main.Settings.gauge.IsStandard())
                return;
            WorldStreamingInit.LoadingFinished += OnLoadingFinished;
        }

        private static void OnLoadingFinished()
        {
            Main.Logger.Log("Modifying static meshes");
            foreach (MeshFilter filter in SingletonBehaviour<WorldStreamingInit>.Instance.originShiftParent.GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (mesh == null || mesh.IsModified())
                    continue;

                string name = mesh.name;

                if (!mesh.isReadable)
                {
                    Mesh m = Assets.GetMesh(mesh.name);
                    if (m == null)
                        continue;
                    filter.sharedMesh = mesh = m;
                }

                switch (name)
                {
                    // Switches
                    case "rails_static":
                        StaticSwitch.ModifyMesh(mesh);
                        mesh.SetModified();
                        break;
                    case "rails_moving":
                        MovingSwitch.ModifyMesh(mesh);
                        mesh.SetModified();
                        break;
                    // Switch anchors
                    case "anchors":
                        SwitchAnchors.ModifyMesh(mesh);
                        mesh.SetModified();
                        break;
                    // Roundhouse rails
                    case "TurntableRail.002":
                        Symmetrical.ScaleToGauge(mesh, scale: 0.1f - GaugeExtensions.railEdgeOffset * 0.1f * 8); // ???
                        mesh.AdjustY(-0.025f * 0.1f); // The rails are visually a bit too high in vanilla, so might as well fix that while we're here
                        mesh.SetModified();
                        break;
                    // Service station markers
                    case "ServiceStationMarker01":
                    case "ServiceStationMarker01_LOD1":
                    case "ServiceStationMarker02":
                    case "ServiceStationMarker02_LOD1":
                    // Switch ballast
                    case "ballast" when Main.Settings.adjustBallastWidth:
                    case "ballast-outersign" when Main.Settings.adjustBallastWidth:
                    // Switch sleepers
                    case "sleepers":
                    case "sleepers-outersign":
                    // Turntable rails
                    case "TurntableRail":
                    case "TurntableRail_ShadowCaster":
                    // Buffer stops
                    case "buffer_stop_sleeper_n_1":
                    case "buffer_stop_rails":
                    case "buffer_stop_rails_LOD1":
                    case "buffer_stop_holders":
                        Symmetrical.ScaleToGauge(mesh);
                        mesh.SetModified();
                        break;
                }
            }
        }
    }
}
