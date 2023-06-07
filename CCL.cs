// using DVCustomCarLoader;
// using Gauge.Utils;

// namespace Gauge
// {
//     public static class CCL
//     {
//         public static float GetGauge(Bogie bogie)
//         {
//             if (!CarTypeInjector.IsInCustomRange(bogie.Car.carType) || !CarTypeInjector.TryGetCustomCarByType(bogie.Car.carType, out CustomCar customCar))
//                 return Gauge.Standard.GetGauge();
//             return (customCar.FrontBogieConfig != null && bogie.IsFront()) || (customCar.RearBogieConfig != null && !bogie.IsFront())
//                 ? customCar.Gauge / 1000.0f
//                 : Gauge.Standard.GetGauge();
//         }
//     }
// }
