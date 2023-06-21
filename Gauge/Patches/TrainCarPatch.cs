using Gauge.Meshes;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(TrainCar), "Start")]
    public static class TrainCar_Start_Patch
    {
        private static void Postfix(TrainCar __instance)
        {
            if (__instance.IsCorrectGauge())
                return;

            foreach (Transform t in __instance.GetComponentsInChildren<Transform>())
            {
                if (t.name != "[wheel sparks]") continue;
                Symmetrical.ScaleToGauge(t, __instance.GetGauge());
            }

            // TODO: doesn't work
            if (__instance.carLivery.id == "LocoS282" && Gauge.Instance.RailGauge.Gauge < RailGauge.STANDARD.Gauge)
            {
                Gauge.Log("Modify SH282");
                __instance.gameObject.ModifyMeshes(HandleMesh, __instance);
            }

            // TODO: S060?

            // TODO: doesn't work
            if (__instance.carLivery.id == "LocoDM3")
            {
                __instance.gameObject.ModifyMeshes(HandleMesh, __instance);
                foreach (Transform child in __instance.transform)
                    if (child.name == "mech_wheels_connect" || child.name == "mech_1")
                        Symmetrical.ScaleToGauge(child);
            }
        }

        private static void HandleMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                // SH282
                case "s282_wheels_driving_1":
                case "s282_wheels_driving_2":
                case "s282_wheels_driving_3":
                case "s282_wheels_driving_4":
                case "s282_support":
                case "s282_locomotive_body":
                case "s282_suspension":
                case "s282_brakes":
                case "s282_wheels_front_support":
                    Gauge.Log($"Modifying SH282 mesh {mesh.name}");
                    SH282.ModifyMesh(mesh);
                    break;
                // DM3
                case "dm3_wheel_01":
                case "dm3_wheel_02":
                case "dm3_wheel_03":
                case "dm3_wheel_01_LOD1":
                case "dm3_wheel_02_LOD1":
                case "dm3_wheel_03_LOD1":
                case "dm3_brake_shoes":
                    DM3.ModifyMesh(mesh);
                    break;
            }
        }
    }
}
