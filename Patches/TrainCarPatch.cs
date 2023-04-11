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
                case "ext Wheels Driving 4":
                case "ext Wheels Driving 2":
                case "ext Wheels Driving 1":
                case "ext Wheels Driving 3":
                case "ext Support":
                case "ext Locomotive Body":
                case "ext Suspension":
                case "ext Brakes":
                case "ext Wheels Front Support":
                    SH282.ModifyMesh(mesh);
                    mesh.SetModified();
                    break;
            }
        }
    }
}
