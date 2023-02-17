using System.Collections.Generic;
using Gauge.MeshModifiers;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(TrainCar), "Start")]
    public static class TrainCar_Start_Patch
    {
        private static readonly HashSet<Mesh> modifiedMeshes = new HashSet<Mesh>();

        public static void Postfix(TrainCar __instance)
        {
            // The SH282's mesh modifications will *not* work on broader gauges, don't even try.
            if (__instance.carType != TrainCarType.LocoSteamHeavy || Main.Settings.gauge.GetGauge() >= Gauge.Standard.GetGauge())
                return;

            foreach (MeshFilter filter in __instance.gameObject.GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (!mesh.isReadable || modifiedMeshes.Contains(mesh)) continue;
                switch (mesh.name)
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
                        modifiedMeshes.Add(mesh);
                        break;
                }
            }
        }
    }
}
