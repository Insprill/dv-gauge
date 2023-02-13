using HarmonyLib;
using UnityEngine;

namespace Gauge.Patches
{
    [HarmonyPatch(typeof(RailwayMeshGenerator), "Start")]
    public static class RailwayMeshGenerator_Start_Patch
    {
        private static bool Prefix()
        {
            if (Main.Settings.gauge == Gauge.Standard)
                return true;

            Main.Logger.Log($"Changing gauge to {Main.Settings.gauge}");

            RailTrack[] railTracks = Object.FindObjectsOfType<RailTrack>();
            RailType baseRailType = railTracks[0].railType;

            RailType railType = ScriptableObject.CreateInstance<RailType>();
            railType.railShape = baseRailType.railShape;
            railType.railMaterial = baseRailType.railMaterial;
            railType.gauge = Main.Settings.gauge.GetGauge();
            railType.railEdgeOffset = Main.Settings.gauge.GetEdgeOffset();

            foreach (RailTrack railTrack in railTracks)
            {
                railTrack.railType = railType;
            }

            return true;
        }
    }
}
