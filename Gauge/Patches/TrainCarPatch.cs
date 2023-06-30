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

            if (__instance.carLivery.id == "LocoS282" && Gauge.Instance.RailGauge.Gauge < RailGauge.STANDARD.Gauge)
                __instance.gameObject.ModifyMeshes(SH282.ModifyMesh, __instance);

            if (__instance.carLivery.id == "LocoDM3")
                HandleDM3(__instance);
        }

        private static void HandleDM3(Component trainCar)
        {
            trainCar.gameObject.ModifyMeshes(DM3.ModifyMesh, trainCar);
            Vector3 pos;
            foreach (Transform child in trainCar.GetComponentsInChildren<Transform>())
                switch (child.name)
                {
                    case "crank_pivot_L":
                        pos = child.localPosition;
                        pos.x += Gauge.Instance.RailGauge.DiffToStandard;
                        child.localPosition = pos;
                        break;
                    case "crank_pivot_R":
                        pos = child.localPosition;
                        pos.x += -Gauge.Instance.RailGauge.DiffToStandard;
                        child.localPosition = pos;
                        break;
                }
        }
    }
}
