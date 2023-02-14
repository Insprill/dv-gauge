using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(Bogie), "Start")]
    public static class Bogie_Start_Patch
    {
        public static void Postfix(Bogie __instance)
        {
            if (Main.Settings.gauge == Gauge.Standard)
                return;

            foreach (MeshFilter filter in __instance.gameObject.GetComponentsInChildren<MeshFilter>())
            {
                // TODO: Modify the shared mesh instead of this wackiness.
                Mesh mesh = filter.mesh;
                string meshName = mesh.name.Replace(" Instance", "");
                switch (meshName)
                {
                    // SH282 leading/trailing wheels
                    // case "ext Wheels Rear": // Clips through the frame
                    case "ext Wheels Front":
                    // DE6 wheels
                    case "diesel_bogie_wheels":
                    case "diesel_bogie_wheels_LOD1":
                    // DE6 bogies
                    case "diesel_bogie_frame":
                    case "diesel_bogie_frame_LOD1":
                    case "diesel_bogie_frame_LOD2":
                    case "diesel_bogie_frame_LOD3":
                    case "diesel_bogie_LOD4":
                    // DE2 wheels
                    case "ext axle_F":
                    case "ext axle_R":
                    // Standard wheels
                    case "axle_LOD0":
                    case "axle_LOD1":
                    // Brake rigging
                    case "Bogie2Brakes02":
                    case "Bogie2Brakes01":
                    case "Bogie2BrakesCylinder":
                    // Standard bogies
                    case "bogie2_LOD0":
                    case "bogie2_LOD1":
                    case "bogie2_LOD2":
                    case "bogie2_LOD3":
                    case "bogie2_LOD4":
                        Axle.ModifyMesh(mesh);
                        break;
                }
            }
        }
    }
}
