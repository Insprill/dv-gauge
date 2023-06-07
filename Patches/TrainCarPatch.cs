using DV.ThingTypes;
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

            // The SH282's mesh modifications will *not* work on broader gauges, don't even try.
            if (__instance.carType != TrainCarType.LocoSteamHeavy || Main.Settings.gauge.GetGauge() >= Gauge.Standard.GetGauge())
                return;

            __instance.gameObject.ModifyMeshes(HandleMesh, __instance);
        }

        private static void HandleMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                case "s282_wheels_driving_1":
                case "s282_wheels_driving_2":
                case "s282_wheels_driving_3":
                case "s282_wheels_driving_4":
                case "s282_support":
                case "s282_locomotive_body":
                case "s282_suspension":
                case "s282_brakes":
                case "s282_wheels_front_support":
                    SH282.ModifyMesh(mesh);
                    mesh.SetModified();
                    break;
            }
        }
    }
}
