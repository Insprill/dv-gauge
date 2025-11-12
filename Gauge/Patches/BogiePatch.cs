using Gauge.Meshes;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(Bogie), "Start")]
    public static class Bogie_Start_Patch
    {
        private static void Postfix(Bogie __instance)
        {
            if (__instance.IsCorrectGauge())
                return;

            __instance.gameObject.ModifyMeshes(HandleMesh, __instance);
        }

        private static void HandleMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                case "ext axle_F":
                case "ext axle_R":
                    Symmetrical.ScaleToGauge(mesh, true, skipVerts: Vertices.Verts.de2_axle_skip);
                    break;
                case "dh4_bogie_frame":
                    Symmetrical.ScaleToGauge(mesh, false, ((Bogie)component).GetGauge());
                    break;
                default:
                    Symmetrical.ScaleToGauge(mesh, true, ((Bogie)component).GetGauge());
                    break;
            }
        }
    }
}
