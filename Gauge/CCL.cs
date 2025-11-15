using AwesomeTechnologies.Utility;
using CCL.Importer.Types;
using CCL.Types.Components;
using Gauge.MeshModifiers;
using Gauge.Utils;
using UnityEngine;
using static UnityModManagerNet.UnityModManager;

namespace Gauge
{
    internal static class CCL
    {
        private static ModEntry s_mod = null;
        public static ModEntry Entry
        {
            get
            {
                if (s_mod == null)
                {
                    s_mod = modEntries.Find(x => x.Info.Id == "DVCustomCarLoader");
                }

                return s_mod;
            }
        }

        public static bool IsActive => Entry != null && Entry.Active;

        public static bool IsCustomCar(TrainCar car)
        {
            return car.carLivery.parentType is CCL_CarType;
        }

        private static bool IsCustomCar(TrainCar car, out CCL_CarType custom)
        {
            if (car.carLivery.parentType is CCL_CarType temp)
            {
                custom = temp;
                return true;
            }

            custom = null;
            return false;
        }

        public static bool HasCustomGauge(TrainCar car, out float gauge)
        {
            if (IsCustomCar(car, out var custom) && custom.UseCustomGauge)
            {
                gauge = custom.Gauge / 1000.0f;
                return true;
            }

            gauge = RailGauge.Standard;
            return false;
        }

        public static bool HasCustomGauge(Bogie bogie, out float gauge)
        {
            return HasCustomGauge(bogie.Car, out gauge);
        }

        static void ModifySpecificMeshes(MeshFilter[] meshes, int length, float? gauge = null)
        {
            for (var i = 0; i < length; i++)
            {
                var filter = meshes[i];
                var mesh = filter.sharedMesh;
                if (mesh == null)
                    continue;

                if (!mesh.isReadable)
                {
                    if (!Assets.GetMesh(mesh.name).IsSome(out var m))
                        continue;
                    filter.sharedMesh = mesh = m;
                }
                if (Assets.IsMeshModified(mesh))
                    continue;

                Symmetrical.ScaleToGauge(mesh, baseGauge: gauge);
            }
        }

        private static void ModifyChildMeshes(GameObject[] gos, float? gauge = null)
        {
            foreach (var go in gos)
            {
                using var filters = TempList<MeshFilter>.Get;
                var filtersList = filters.List;
                go.GetComponentsInChildren(filtersList);
                ModifySpecificMeshes(filtersList.GetInternalArray(), filtersList.Count, gauge);
            }
        }

        public static void HandleCustomMeshes(TrainCar car)
        {
            if (IsCustomCar(car, out var custom) && car.TryGetComponent(out RegaugeableMeshes meshes))
            {
                var gauge = custom.UseCustomGauge ? custom.Gauge / 1000.0f : (float?)null;

                ModifyChildMeshes(meshes.Objects, gauge);
                ModifySpecificMeshes(meshes.Meshes, meshes.Meshes.Length, gauge);
            }
        }
    }
}
