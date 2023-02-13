using Gauge.Utils;
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

            RailType standardRailType = railTracks[0].railType;
            RailType railType = standardRailType.Clone();
            railType.gauge = Main.Settings.gauge.GetGauge();
            railType.railEdgeOffset = Main.Settings.gauge.GetEdgeOffset();

            BaseType standardBaseType = railTracks[0].baseType;
            BaseType baseType = standardBaseType.Clone();
            baseType.sleeperDistance = Main.Settings.gauge.GetSleeperDistance();

            foreach (RailTrack railTrack in railTracks)
            {
                railTrack.railType = railType;
                railTrack.baseType = baseType;
            }

            return true;
        }
    }
}
