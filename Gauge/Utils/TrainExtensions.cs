using UnityEngine;

namespace Gauge.Utils
{
    public static class TrainExtensions
    {
        public static float GetGauge(this TrainCar trainCar)
        {
            return trainCar.Bogies[0].GetGauge();
        }

        public static float GetGauge(this Bogie bogie)
        {
            return /*Main.IsCCLEnabled ? CCL.GetGauge(bogie) :*/ RailGaugePreset.Standard.RailGauge().Gauge;
        }

        public static bool IsCorrectGauge(this TrainCar trainCar)
        {
            return trainCar.Bogies[0].IsCorrectGauge();
        }

        public static bool IsCorrectGauge(this Bogie bogie)
        {
            return Mathf.Abs(Gauge.Settings.RailGauge.Gauge - bogie.GetGauge()) < 0.001f;
        }

        public static bool IsFront(this Bogie bogie)
        {
            return bogie.Car.Bogies[0] == bogie;
        }
    }
}
