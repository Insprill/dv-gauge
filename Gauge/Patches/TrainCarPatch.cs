using DV.Wheels;
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
        static void Postfix(TrainCar __instance)
        {
            if (__instance.IsCorrectGauge())
                return;

            var sparkAnchors = __instance.GetComponentInChildren<WheelSlideSparksController>().sparkAnchors;
            foreach (var anchor in sparkAnchors)
            {
                Symmetrical.ScaleToGauge(anchor, __instance.GetGauge());
            }

            // DE2, S282A and S060 only have modifications when narrow.
            var narrow = Gauge.Settings.RailGauge.Gauge < RailGaugePreset.Standard.RailGauge().Gauge;

            switch (__instance.carLivery.parentType.id)
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

        static void HandleDM3(Component trainCar)
        {
            trainCar.gameObject.ModifyMeshes(DM3.ModifyMesh, trainCar);

            using var children = TempList<Transform>.Get;
            trainCar.GetComponentsInChildren(children.List);
            foreach (var child in children.List)
            {
                var childName = child.name;
                if (childName is not ("crank_pivot_L" or "crank_pivot_R"))
                    continue;
                var pos = child.localPosition;
                var diff = Gauge.Settings.RailGauge.DiffToStandard;
                pos.x += childName.EndsWith("L") ? diff : -diff;
                child.localPosition = pos;
            }
        }

        static void TryHandleCustom(TrainCar car)
        {
            if (!CCL.IsActive)
                return;

            CCL.HandleCustomMeshes(car);
        }
    }
}
