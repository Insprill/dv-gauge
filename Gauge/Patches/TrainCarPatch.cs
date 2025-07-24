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

            // DE2, S282A and S060 only have modifications when narrow.
            bool narrow = Gauge.Settings.RailGauge.Gauge < RailGaugePreset.Standard.RailGauge().Gauge;

            switch (__instance.carLivery.id)
            {
                case "LocoDE2":
                    if (narrow)
                        __instance.gameObject.ModifyMeshes(DE2.ModifyMesh, __instance);
                    break;
                case "LocoS282A":
                    if (narrow)
                        __instance.gameObject.ModifyMeshes(SH282.ModifyMesh, __instance);
                    break;
                case "LocoS060":
                    if (narrow)
                        __instance.gameObject.ModifyMeshes(S060.ModifyMesh, __instance);
                    break;
                case "LocoDM3":
                    HandleDM3(__instance);
                    break;
                default:
                    TryHandleCustom(__instance);
                    break;
            }
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
                        pos.x += Gauge.Settings.RailGauge.DiffToStandard;
                        child.localPosition = pos;
                        break;
                    case "crank_pivot_R":
                        pos = child.localPosition;
                        pos.x += -Gauge.Settings.RailGauge.DiffToStandard;
                        child.localPosition = pos;
                        break;
                }
        }

        private static void TryHandleCustom(TrainCar car)
        {
            if (!CCL.IsActive) return;

            CCL.HandleCustomMeshes(car);
        }
    }
}
