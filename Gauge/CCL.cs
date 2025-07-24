using DV.ThingTypes;
using Gauge.Utils;
using UnityModManagerNet;

namespace Gauge
{
    internal static class CCL
    {
        public static bool IsActive => UnityModManager.modEntries.TryFind(x => x.Info.Id == "DVCustomCarLoader", out var mod) && mod.Active;

        public static bool HasCustomGauge(TrainCarType_v2 car, out float gauge)
        {
            var t = car.GetType();
            var isCustom = t.GetField("UseCustomGauge");

            if (isCustom != null && (bool)(isCustom.GetValue(car)))
            {
                var customGauge = (int)(t.GetField("Gauge").GetValue(car));
                gauge = customGauge / 1000.0f;
                return true;
            }

            gauge = RailGauge.Standard;
            return false;
        }

        public static bool HasCustomGauge(Bogie bogie, out float gauge)
        {
            return HasCustomGauge(bogie.Car.carLivery.parentType, out gauge);
        }

        public static bool HasCustomGauge(TrainCar car, out float gauge)
        {
            return HasCustomGauge(car.carLivery.parentType, out gauge);
        }
    }
}
