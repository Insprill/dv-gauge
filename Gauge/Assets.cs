using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace Gauge
{
    public static class Assets
    {
        private const string ASSET_BUNDLE_NAME = "gauge.assetbundle";
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        private static AssetBundle assetBundle;

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

        [CanBeNull]
        public static Mesh GetMesh([CanBeNull] Mesh mesh)
        {
            return mesh == null ? null : GetMesh(mesh.name);
        }

        [CanBeNull]
        public static Mesh GetMesh([NotNull] string name)
        {
            switch (name)
            {
                case "TurntableRail":
                case "TurntableRail.002":
                    if (Meshes.TryGetValue(name, out Mesh railMesh))
                        return railMesh;

                    if (!Meshes.TryGetValue($"{name}_0", out Mesh mesh1))
                        return null;
                    if (!Meshes.TryGetValue($"{name}_1", out Mesh mesh2))
                        return null;

                    Mesh combinedMesh = new Mesh();
                    // Rails must go first
                    combinedMesh.CombineMeshes(new[] {
                        new CombineInstance {
                            mesh = mesh1,
                            transform = Matrix4x4.identity
                        },
                        new CombineInstance {
                            mesh = mesh2,
                            transform = Matrix4x4.identity
                        }
                    }, false);

                    Meshes.Add(name, combinedMesh);
                    return combinedMesh;
                default:
                    bool foundMesh = Meshes.TryGetValue($"{name.Replace(' ', '_')}_0", out Mesh mesh);
                    return foundMesh ? mesh : null;
            }
        }
    }
}
