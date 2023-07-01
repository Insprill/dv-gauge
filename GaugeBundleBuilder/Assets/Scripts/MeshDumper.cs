#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Gauge.GaugeBundleBuilder
{
    public class MeshDumper : EditorWindow
    {
        private static string InstallationDirectory {
            get => EditorPrefs.GetString("Gauge.MeshDumper.InstallationDirectory");
            set => EditorPrefs.SetString("Gauge.MeshDumper.InstallationDirectory", value);
        }

        [MenuItem("Gauge/Dump Game Meshes")]
        public static void OpenMenu()
        {
            MeshDumper window = GetWindow<MeshDumper>("Mesh Dumper");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter Derail Valley's installation directory", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            InstallationDirectory = EditorGUILayout.TextField(InstallationDirectory);
            if (GUILayout.Button("Select"))
                InstallationDirectory = EditorUtility.OpenFolderPanel("Select Derail Valley's installation directory", "", "");
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Dump Meshes"))
                DumpMeshes();
        }

        private static void DumpMeshes()
        {
            string dataDirectory = Path.Combine(InstallationDirectory, "DerailValley_Data");
            if (!Directory.Exists(dataDirectory))
            {
                EditorUtility.DisplayDialog(
                    "Gauge",
                    $"Failed to find Derail Valley installation at '{InstallationDirectory}'",
                    "Ok"
                );
                return;
            }

            int dotNetVersion = GetDotNetVersion();
            if (dotNetVersion == -1)
                return;
            if (dotNetVersion != 7)
            {
                EditorUtility.DisplayDialog(
                    "Gauge",
                    "You must have .NET 7 installed to use this tool.",
                    "Ok"
                );
                return;
            }

            try
            {
                ClearExistingMeshes();
                RunAssetStudio(dataDirectory);
                Debug.Log("Finished dumping meshes!");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static int GetDotNetVersion()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = "dotnet",
                Arguments = "--version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process process = Process.Start(startInfo);
            if (process == null)
            {
                Debug.LogError("Failed to start dotnet process!");
                return -1;
            }

            string err = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err))
            {
                Debug.LogError(err);
                return -1;
            }

            string versionString = process.StandardOutput.ReadToEnd();
            if (!int.TryParse($"{versionString[0]}", out int version))
            {
                Debug.LogError($"Failed to parse dotnet version '{versionString}'!");
                return -1;
            }

            return version;
        }

        private static void RunAssetStudio(string installDirectory)
        {
            string assetList = string.Join(",",
                File.ReadLines(AssetBundleBuilder.MESH_LIST_PATH).Select(s => Path.ChangeExtension(s, null))
            );
            Debug.Log(assetList);
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = "dotnet",
                Arguments = $"Assets/Scripts/AssetStudio/AssetStudioModCLI.dll \"{installDirectory}\" -o \"{AssetBundleBuilder.MESH_PATH}\" -t mesh --filter-by-name \"{assetList}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process process = Process.Start(startInfo);
            if (process == null)
            {
                Debug.LogError("Failed to start dotnet process!");
                return;
            }

            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line == null)
                    continue;
                EditorUtility.DisplayProgressBar(
                    "Dumping meshes",
                    line,
                    0f
                );
            }

            string err = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err))
                Debug.LogError(err);
        }

        private static void ClearExistingMeshes()
        {
            string[] files = Directory.GetFiles(AssetBundleBuilder.MESH_PATH);
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                if (file.EndsWith(AssetBundleBuilder.MESH_LIST_PATH))
                    continue;
                EditorUtility.DisplayProgressBar("Deleting meshes", file, i / (float)files.Length);
                File.Delete(file);
                File.Delete($"{file}.meta");
            }
        }
    }
}
#endif
