using System.IO;
using DV.Utils;
using Gauge.Meshes;
using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(WorldStreamingInit), "Awake")]
    public static class WorldStreamingInit_Awake_Patch
    {
        private static void Postfix()
        {
            if (Gauge.Instance.RailGauge.IsStandard())
                return;
            if (Assets.Init(Path.GetDirectoryName(Gauge.Instance.Info.Location)))
                WorldStreamingInit.LoadingFinished += OnLoadingFinished;
        }

        private static void OnLoadingFinished()
        {
            Gauge.Instance.Logger.LogDebug("Modifying static meshes");
            SingletonBehaviour<WorldStreamingInit>.Instance.originShiftParent.ModifyMeshes(HandleMesh);
        }

        private static void HandleMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                // Switches
                case "rails_static":
                    StaticSwitch.ModifyMesh(mesh);
                    break;
                case "rails_moving":
                    MovingSwitch.ModifyMesh(mesh);
                    break;
                // Switch anchors
                case "anchors":
                    SwitchAnchors.ModifyMesh(mesh);
                    break;
                // Switch sleepers
                case "sleepers":
                case "sleepers-outersign":
                    Symmetrical.ScaleToGauge(mesh, skipVerts: Vertices.Verts.switch_sleeper_skip);
                    break;
                // Roundhouse rails
                case "TurntableRail.002":
                // Switch ballast
                case "ballast" when Gauge.Instance.Settings.adjustBallastWidth.Value:
                case "ballast-outersign" when Gauge.Instance.Settings.adjustBallastWidth.Value:
                // Turntable rails
                case "TurntableRail":
                case "TurntableRail_ShadowCaster":
                // Buffer stops
                case "buffer_stop_rails":
                case "buffer_stop_rails_LOD1":
                case "buffer_stop_holders":
                    Symmetrical.ScaleToGauge(mesh);
                    break;
            }
        }
    }
}
