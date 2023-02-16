using System.Collections.Generic;
using DVCustomCarLoader;
using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(Bogie), "Start")]
    public static class Bogie_Start_Patch
    {
        private static readonly HashSet<Mesh> modifiedMeshes = new HashSet<Mesh>();

        public static void Postfix(Bogie __instance)
        {
            if (Main.Settings.gauge == Gauge.Standard)
                return;

            foreach (MeshFilter filter in __instance.gameObject.GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (!mesh.isReadable || modifiedMeshes.Contains(mesh)) continue;
                Symmetrical.ScaleToGauge(mesh, true, GetGauge(__instance.Car));
                modifiedMeshes.Add(mesh);
            }
        }

        private static float? GetGauge(TrainCar car)
        {
            if (!Main.IsCCLEnabled || !CarTypeInjector.IsInCustomRange(car.carType) || !CarTypeInjector.TryGetCustomCarByType(car.carType, out CustomCar customCar))
                return Gauge.Standard.GetGauge();
            float? gauge = customCar.FrontBogieConfig?.Gauge;
            return gauge == null || gauge < 0.01 ? Gauge.Standard.GetGauge() : gauge / 1000.0f;
        }
    }
}
