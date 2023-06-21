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
        private static readonly ushort[] SLEEPER_SKIP_VERTS = { 498, 499, 500, 501, 503, 506 };

        private static void Postfix()
        {
            if (Gauge.Instance.RailGauge.IsStandard())
                return;
            WorldStreamingInit.LoadingFinished += OnLoadingFinished;
        }

        private static void OnLoadingFinished()
        {
            Gauge.LogDebug("Modifying static meshes");
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
                    Symmetrical.ScaleToGauge(mesh, skipVerts: SLEEPER_SKIP_VERTS);
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