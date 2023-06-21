using System;
using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace Gauge
{
    [BepInPlugin("net.insprill.dv-gauge", "Gauge", "1.2.0")]
    public class Gauge : BaseUnityPlugin
    {
        public static Gauge Instance { get; private set; }

        public Settings Settings;
        private Harmony harmony;
        public RailGauge RailGauge { get; private set; }

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
                Logger.LogInfo("Loading assets...");
                if (Assets.Init(Path.GetDirectoryName(Info.Location)))
                {
                    Logger.LogInfo("Successfully loaded assets");
                }
                else
                {
                    Logger.LogFatal("Failed to load assets!");
                    return;
                }

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

        #region Logging

        public static void LogDebug(object msg)
        {
            Instance.Logger.LogDebug($"{msg}");
        }

        public static void Log(object msg)
        {
            Instance.Logger.LogInfo($"{msg}");
        }

        public static void LogWarning(object msg)
        {
            Instance.Logger.LogWarning($"{msg}");
        }

        public static void LogError(object msg)
        {
            Instance.Logger.LogError($"{msg}");
        }

        #endregion
    }
}
