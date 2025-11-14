using DV.Simulation.Brake;
using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(BrakeCylinderReleaseButtonFeeder), "Start")]
    static class BrakeCylinderReleaseButtonFeeder_Start_Patch
    {
        static void Postfix(BrakeCylinderReleaseButtonFeeder __instance)
        {
            var transform = __instance.transform;
            var car = TrainCar.Resolve(transform);
            switch (car.carLivery.parentType.id)
            {
                case "LocoS282A":
                    Symmetrical.ScaleToGauge(transform.parent, car.GetGauge());
                    break;
            }
        }
    }
}
