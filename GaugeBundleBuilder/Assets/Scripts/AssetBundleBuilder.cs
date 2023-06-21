#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gauge.GaugeBundleBuilder
{
    public static class AssetBundleBuilder
    {
        private const string ASSET_BUNDLE_NAME = "gauge";
        private const string MESH_PATH = "Assets/Meshes";
        private const string BUNDLE_OUT_DIR = "Assets/Temp";
        private static readonly string MESH_LIST_PATH = $"{MESH_PATH}/meshes.txt";
        private static readonly string BUNDLE_OUT_DIR_META = $"{BUNDLE_OUT_DIR}.meta";
        private static readonly string BUNDLE_OUT_PATH = $"{BUNDLE_OUT_DIR}/{ASSET_BUNDLE_NAME}";
        private static readonly string BUNDLE_MOVE_PATH = $"../build/{ASSET_BUNDLE_NAME}.assetbundle";

        [MenuItem("Gauge/Build Asset Bundles")]
        public static void BuildAssetBundles()
        {
            if (!AllMeshesPresent())
                return;

            Debug.Log("Updating import settings");
            foreach (KeyValuePair<string, string> mesh in GetMeshPaths())
            {
                ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(mesh.Value);
                UpdateImportSettings(importer);
                importer.SaveAndReimport();
            }

            AssetBundleBuild build = new AssetBundleBuild {
                assetBundleName = ASSET_BUNDLE_NAME,
                assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(ASSET_BUNDLE_NAME)
            };

            Debug.Log("Building Asset Bundle");
            Directory.CreateDirectory(BUNDLE_OUT_DIR);
            BuildPipeline.BuildAssetBundles(BUNDLE_OUT_DIR, new[] { build }, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);

            if (File.Exists(BUNDLE_MOVE_PATH))
            {
                Debug.Log("Removing existing Asset Bundle");
                File.Delete(BUNDLE_MOVE_PATH);
            }

            Debug.Log("Moving Asset Bundle to build folder");
            Directory.CreateDirectory(Path.GetDirectoryName(BUNDLE_MOVE_PATH));
            File.Move(BUNDLE_OUT_PATH, BUNDLE_MOVE_PATH);

            Debug.Log("Deleting temp dir");
            Directory.Delete(BUNDLE_OUT_DIR, true);
            File.Delete(BUNDLE_OUT_DIR_META);

            Debug.Log("Finished!");
        }

        private static bool AllMeshesPresent()
        {
            if (!File.Exists(MESH_LIST_PATH))
            {
                Debug.LogError($"Failed to find required mesh list at '{MESH_LIST_PATH}'!");
                return false;
            }

            bool foundAllMeshes = true;
            foreach (KeyValuePair<string, string> mesh in GetMeshPaths())
            {
                if (File.Exists(mesh.Value)) continue;
                Debug.LogError($"Failed to find '{mesh.Key}' in '{MESH_PATH}'!");
                foundAllMeshes = false;
            }

            return foundAllMeshes;
        }

        private static void UpdateImportSettings(ModelImporter importer)
        {
            importer.assetBundleName = ASSET_BUNDLE_NAME;
            // Don't import stuff we don't need.
            importer.materialImportMode = ModelImporterMaterialImportMode.None;
            importer.importAnimation = false;
            importer.animationType = ModelImporterAnimationType.None;
            // Allow us to do the thing.
            importer.isReadable = true;
            // Some of our work depends on specific vertex indices, so we can't weld or optimize them.
            importer.weldVertices = false;
            importer.optimizeMeshVertices = false;
            // But polygons are free game.
            importer.optimizeMeshPolygons = true;
        }

        private static IEnumerable<KeyValuePair<string, string>> GetMeshPaths()
        {
            return File.ReadAllLines(MESH_LIST_PATH)
                .Select(s => new KeyValuePair<string, string>(s, $"{MESH_PATH}/{s}"));
        }
    }
}
#endif
