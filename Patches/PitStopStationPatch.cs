using Gauge.MeshModifiers;
using Gauge.Utils;
using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(PitStopStation), "OnEnable")]
    public static class PitStopStationPatch
    {
        public static void Postfix(PitStopStation __instance)
        {
            foreach (MeshFilter filter in __instance.transform.parent.GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (mesh == null || mesh.IsModified())
                    continue;

                string name = mesh.name;

                if (!mesh.isReadable)
                {
                    Mesh m = Assets.GetMesh(mesh.name);
                    if (m == null)
                        continue;
                    filter.sharedMesh = mesh = m;
                }

                switch (name)
                {
                    // Service station markers
                    case "ServiceStationMarker01":
                    case "ServiceStationMarker01_LOD1":
                    case "ServiceStationMarker02":
                    case "ServiceStationMarker02_LOD1":
                        Symmetrical.ScaleToGauge(mesh, scale: 0.1875f);
                        mesh.SetModified();
                        break;
                }
            }
        }
    }
}
