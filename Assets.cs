using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gauge
{
    public static class Assets
    {
        private const string ASSET_BUNDLE_NAME = "gauge.assetbundle";
        private static readonly Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        public static bool Init(string modPath)
        {
            string assetBundlePath = Path.Combine(modPath, ASSET_BUNDLE_NAME);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (assetBundle == null)
            {
                Main.Logger.Error($"Failed to load asset bundle at {assetBundlePath}. Some assets may not be regauged.");
                return false;
            }

            foreach (Mesh mesh in assetBundle.LoadAllAssets<Mesh>())
                Meshes.Add(mesh.name, mesh);

            assetBundle.Unload(false);

            return true;
        }

        public static Mesh GetMesh(string name)
        {
            switch (name)
            {
                case "TurntableRail.002":
                    if (Meshes.ContainsKey(name))
                        return Meshes[name];

                    Mesh combinedMesh = new Mesh();

                    // Rails must go first
                    combinedMesh.CombineMeshes(new[] {
                        new CombineInstance {
                            mesh = Meshes[$"{name}_0"],
                            transform = Matrix4x4.identity
                        },
                        new CombineInstance {
                            mesh = Meshes[$"{name}_1"],
                            transform = Matrix4x4.identity
                        }
                    }, false);

                    Meshes.Add(name, combinedMesh);
                    return combinedMesh;
                default:
                    bool foundMesh = Meshes.TryGetValue($"{name}_0", out Mesh mesh);
                    return foundMesh ? mesh : null;
            }
        }
    }
}
