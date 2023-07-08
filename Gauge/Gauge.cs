using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Gauge
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Gauge : BaseUnityPlugin
    {
        public static Gauge Instance { get; private set; }

        public Settings Settings;
        private Harmony harmony;
        public RailGauge RailGauge { get; private set; }
        internal new ManualLogSource Logger => base.Logger;

        private void Awake()
        {
            if (Instance != null)
            {
                Logger.LogFatal($"{Info.Metadata.Name} is already loaded!");
                Destroy(this);
                return;
            }

            Instance = this;

            Settings = new Settings(Config);
            RailGauge = new RailGauge(Settings.gauge.Value, Settings.sleeperSpacing.Value);

            try
            {
                Vertices.Load(Path.GetDirectoryName(Info.Location));
                Patch();
            }
            catch (Exception ex)
            {
                Logger.LogFatal($"Failed to load Gauge: {ex}");
                Destroy(this);
            }
        }

        private void Patch()
        {
            Logger.LogInfo("Patching...");
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Successfully patched");
        }

        private void OnDestroy()
        {
            Logger.LogInfo("Unpatching...");
            harmony?.UnpatchSelf();
            Logger.LogInfo("Successfully Unpatched");
        }
    }
}
