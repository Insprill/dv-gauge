using System.Collections.Generic;
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
                Axle.ModifyMesh(mesh);
                modifiedMeshes.Add(mesh);
            }
        }
    }
}
