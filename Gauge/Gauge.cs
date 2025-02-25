using System;
using HarmonyLib;
using UnityModManagerNet;

namespace Gauge
{
    public static class Gauge
    {
        internal static UnityModManager.ModEntry ModEntry;
        public static UnityModManager.ModEntry.ModLogger Logger => ModEntry.Logger;
        public static Settings Settings;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Settings = Settings.Load<Settings>(modEntry);
            ModEntry.OnGUI = Settings.Draw;
            ModEntry.OnSaveGUI = Settings.Save;

            Harmony harmony = null;

            try
            {
                Logger.Log("Patching...");
                harmony = new Harmony(ModEntry.Info.Id);
                harmony.PatchAll();
                Logger.Log("Successfully patched");

                Logger.Log("Loading assets...");
                if (Assets.Init(ModEntry.Path))
                    Logger.Log("Successfully loaded assets");

                Logger.Log("Loading vertices...");
                Vertices.Load(ModEntry.Path);
            }
            catch (Exception ex)
            {
                Logger.LogException("Failed to load Gauge:", ex);
                harmony?.UnpatchAll();
                return false;
            }

            return true;
        }
    }
}
