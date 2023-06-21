using UnityEngine;

namespace Gauge.Meshes
{
    public static class MeshModificationExtensions
    {
        /// <summary>
        ///     Updates the vertices of a mesh, recalculates its bounds, and uploads it to the GPU, removing it from CPU memory.
        ///     The mesh cannot be accessed from the CPU after this!
        /// </summary>
        public static void ApplyVerts(this Mesh mesh, Vector3[] vertices)
        {
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            mesh.UploadMeshData(true);
        }

        public static void ModifyMeshes(this GameObject gameObject, HandleMesh func, Component component = null)
        {
            gameObject.transform.ModifyMeshes(func, component);
        }

        public static void ModifyMeshes(this Transform transform, HandleMesh func, Component component = null)
        {
            foreach (MeshFilter filter in transform.GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                if (mesh == null)
                    continue;

                // The new mesh's name usually has a suffix, so we save the original to make switching easier.
                string name = mesh.name;

                if (!mesh.isReadable)
                {
                    Mesh m = Assets.GetMesh(mesh.name);
                    if (m == null) continue;
                    filter.sharedMesh = mesh = m;
                    if (!m.isReadable)
                        continue; // Already modified.
                }

                func(name, mesh, component);
            }
        }

        public delegate void HandleMesh(string name, Mesh mesh, Component component = null);
    }
}
