using System.Collections.Generic;
using System.IO;
using DV;
using JetBrains.Annotations;
using UnityEngine;

namespace Gauge
{
    public static class Assets
    {
        const string ASSET_BUNDLE_NAME = "gauge.assetbundle";
        static readonly Dictionary<string, Mesh> Meshes = new();
        static readonly HashSet<Mesh> ModifiedMeshes = [];
        static AssetBundle assetBundle;

        public static bool Init(string modPath)
        {
            string assetBundlePath = Path.Combine(modPath, ASSET_BUNDLE_NAME);
            assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (assetBundle == null)
            {
                Gauge.Logger.Critical($"Failed to load asset bundle at '{assetBundlePath}'. Most assets won't be regauged!");
                return false;
            }

            Gauge.Logger.Log($"Loaded asset bundle from '{assetBundlePath}'.");

            Meshes.Clear();
            foreach (Mesh mesh in assetBundle.LoadAllAssets<Mesh>())
                Meshes.Add(mesh.name, mesh);

            assetBundle.Unload(false);

            return true;
        }

        public static void MarkMeshModified(Mesh mesh)
        {
            ModifiedMeshes.Add(mesh);
        }

        public static bool IsMeshModified(Mesh mesh)
        {
            return ModifiedMeshes.Contains(mesh);
        }

        public static Option<Mesh> GetMesh([NotNull] MeshFilter filter)
        {
            return GetMesh(filter.sharedMesh.name);
        }

        public static Option<Mesh> GetMesh([CanBeNull] Mesh mesh)
        {
            return mesh == null ? null : GetMesh(mesh.name);
        }

        public static Option<Mesh> GetMesh([NotNull] string name)
        {
            switch (name)
            {
                case "RailwayMuseumTurntable_LOD0":
                    return CombineMeshes(name, 5);
                case "TurntableRail":
                case "TurntableRail.002":
                    return CombineMeshes(name, 2);
                default:
                    var foundMesh = Meshes.TryGetValue($"{name.Replace(' ', '_')}_0", out var mesh);
                    return foundMesh ? Option<Mesh>.Some(mesh) : Option<Mesh>.None;
            }
        }

        static Option<Mesh> CombineMeshes(string name, int count)
        {
            if (Meshes.TryGetValue(name, out var railMesh))
                return Option<Mesh>.Some(railMesh);

            var instances = new CombineInstance[count];
            for (var i = 0; i < count; i++)
            {
                if (!Meshes.TryGetValue($"{name}_{i}", out var m))
                    return Option<Mesh>.None;
                instances[i] = new CombineInstance { mesh = m, transform = Matrix4x4.identity };
            }

            var combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(instances, false);

            Meshes.Add(name, combinedMesh);
            return Option<Mesh>.Some(combinedMesh);
        }
    }
}
