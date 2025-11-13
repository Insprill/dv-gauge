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
            if (Gauge.Settings.RailGauge.IsStandard())
                return;
            if (Assets.Init(Gauge.ModEntry.Path))
                WorldStreamingInit.LoadingFinished += OnLoadingFinished;
        }

        private static void OnLoadingFinished()
        {
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
                // Museum turntable rails
                case "RailwayMuseumTurntable_LOD0":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.museum_turntable_include, rotationDegrees: 5f);
                    break;
                // Roundhouse rails
                case "TurntableRail.002":
                // Turntable rails
                case "TurntableRail":
                // Switch ballast
                case "ballast" when Gauge.Settings.adjustBallastWidth:
                case "ballast-outersign" when Gauge.Settings.adjustBallastWidth:
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
