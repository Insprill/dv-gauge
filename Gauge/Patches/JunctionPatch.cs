using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(Junction))]
    static class JunctionPatch
    {
        [HarmonyPostfix, HarmonyPatch("Awake")]
        static void AwakePrefix(Junction __instance)
        {
            /*
               junc-left
               ├─ Graphical
               │  ├─ rails_static
               │  ├─ rails_moving
               ├─ in_junction <- Junction component is here
             */
            var graphicTransform = __instance.transform.parent.Find("Graphical");
            UpdateBaseType(graphicTransform.Find("rails_static"));
            UpdateBaseType(graphicTransform.Find("rails_moving"));
        }

        static void UpdateBaseType(Transform meshTransform)
        {
            var renderer = meshTransform.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = RailMaterials.GetSelectedRailMaterial(renderer.sharedMaterial);
        }
    }
}
