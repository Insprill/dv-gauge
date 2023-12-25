#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Gauge.GaugeBundleBuilder
{
    public class MeshDumper : EditorWindow
    {
        private const byte DOTNET_VERSION = 7;
        private static readonly Regex VersionRegex = new Regex(@"(\d+)\.\d+\.\d+");

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

            List<int> dotNetVersions = GetDotNetVersions();
            if (!dotNetVersions.Contains(DOTNET_VERSION))
            {
                Debug.Log($"Failed to find .NET {DOTNET_VERSION}! Found: {string.Join(", ", dotNetVersions)}");
                EditorUtility.DisplayDialog(
                    "Gauge",
                    $"You must have .NET {DOTNET_VERSION} installed to use this tool.",
                    "Ok"
                );
                return;
            }

            try
            {
                ClearExistingMeshes();
                RunAssetStudio(dataDirectory);
                AssetDatabase.Refresh();
                Debug.Log("Finished dumping meshes!");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static List<int> GetDotNetVersions()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = "dotnet",
                Arguments = "--list-runtimes",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process process = Process.Start(startInfo);
            List<int> versions = new List<int>();

            if (process == null)
            {
                Debug.LogError("Failed to start dotnet process!");
                return versions;
            }

            string err = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err))
            {
                Debug.LogError(err);
                return versions;
            }

            string versionString = process.StandardOutput.ReadToEnd();
            foreach (Match match in VersionRegex.Matches(versionString))
            {
                if (match.Groups.Count < 2)
                    continue;

                Group group = match.Groups[1];
                if (!group.Success)
                    continue;

                versionString = group.Value;
                versions.Add(int.Parse(versionString));
            }

            return versions;
        }

        private static void RunAssetStudio(string installDirectory)
        {
            string assetList = string.Join(",",
                File.ReadAllLines(AssetBundleBuilder.MESH_LIST_PATH).Select(s => Path.ChangeExtension(s, null))
            );
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = "dotnet",
                Arguments = $"Assets/Scripts/AssetStudio/AssetStudioModCLI.dll \"{installDirectory}\" -o \"{AssetBundleBuilder.MESH_PATH}\" -t mesh --filter-by-name \"{assetList}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
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
                if (Path.GetFileName(file) == Path.GetFileName(AssetBundleBuilder.MESH_LIST_PATH))
                    continue;
                EditorUtility.DisplayProgressBar("Deleting meshes", file, i / (float)files.Length);
                File.Delete(file);
                File.Delete($"{file}.meta");
            }
        }
    }
}
#endif
