using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(Bogie), "Start")]
    public static class Bogie_Start_Patch
    {
        private static readonly ushort[] DE2_AXLE_SKIP_VERTS = {
            264, 267, 269, 271, 273, 275, 277, 279, 281, 283, 285, 287, 289, 291, 293, 295, 297, 628, 631, 633, 635, 637, 639, 641, 643, 645, 647, 649, 651, 653, 655, 657, 659, 661
        };

        public static void Postfix(Bogie __instance)
        {
            if (Mathf.Abs(Main.Settings.gauge.GetGauge() - __instance.Car.GetGauge()) < 0.001f)
                return;

            __instance.gameObject.ModifyMeshes(HandleMesh, __instance);
        }

        private static void HandleMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                case "ext axle_F":
                case "ext axle_R":
                    Symmetrical.ScaleToGauge(mesh, true, ((Bogie)component).Car.GetGauge(), DE2_AXLE_SKIP_VERTS);
                    mesh.SetModified();
                    break;
                default:
                    Symmetrical.ScaleToGauge(mesh, true, ((Bogie)component).Car.GetGauge());
                    mesh.SetModified();
                    break;
            }
        }
    }
}
