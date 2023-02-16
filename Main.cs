using System;
using HarmonyLib;
using UnityModManagerNet;

namespace Gauge
{
    public static class Main
    {

        private static UnityModManager.ModEntry ModEntry;
        public static UnityModManager.ModEntry.ModLogger Logger => ModEntry.Logger;
        public static Settings Settings;

        private static readonly UnityModManager.ModEntry CCLMod = UnityModManager.FindMod("DVCustomCarLoader");
        public static bool IsCCLEnabled => CCLMod != null && CCLMod.Enabled && CCLMod.Version >= Version.Parse("1.8.3");

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Settings = Settings.Load<Settings>(modEntry);
            ModEntry.OnGUI = DrawGUI;
            ModEntry.OnSaveGUI = SaveGUI;

            Harmony harmony = null;

            try
            {
                Logger.Log("Patching...");
                harmony = new Harmony(ModEntry.Info.Id);
                harmony.PatchAll();
                Logger.Log("Successfully patched");
            }
            catch (Exception ex)
            {
                Logger.LogException("Failed to load Gauge:", ex);
                harmony?.UnpatchAll();
                return false;
            }

            return true;
        }

        private static void DrawGUI(UnityModManager.ModEntry entry)
        {
            Settings.Draw(entry);
        }

        private static void SaveGUI(UnityModManager.ModEntry entry)
        {
            Settings.Save(entry);
        }

    }
}
