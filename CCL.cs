using DVCustomCarLoader;

namespace Gauge
{
    public static class CCL
    {
        public static float GetGauge(TrainCar car)
        {
            if (!CarTypeInjector.IsInCustomRange(car.carType) || !CarTypeInjector.TryGetCustomCarByType(car.carType, out CustomCar customCar))
                return Gauge.Standard.GetGauge();
            return customCar.Gauge / 1000.0f;
        }
    }
}
