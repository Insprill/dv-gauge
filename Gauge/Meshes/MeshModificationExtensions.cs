using System.Collections.Generic;
using Gauge.Utils;
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

        /// <inheritdoc cref="ApplyVerts(Mesh, Vector3[])"/>
        public static void ApplyVerts(this Mesh mesh, List<Vector3> vertices)
        {
            mesh.SetVertices(vertices);
            mesh.RecalculateBounds();
            mesh.UploadMeshData(true);
        }

        public static void ModifyMeshes(this GameObject gameObject, HandleMesh scaleFunc, Component component = null)
        {
            gameObject.transform.ModifyMeshes(scaleFunc, null, component);
        }

        public static void ModifyMeshes(this Transform transform, HandleMesh scaleFunc, HandleMesh otherFunc = null, Component component = null)
        {
            using var filters = TempList<MeshFilter>.Get;
            transform.GetComponentsInChildren(filters.List);
            foreach (var filter in filters.List)
            {
                if (component is TrainCar && filter.GetComponentInParent<Bogie>())
                    continue; // Prevents the body modifier from locking the bogie's mesh preventing the bogie modifier from modifying it

                Mesh mesh = filter.sharedMesh;
                if (mesh == null)
                    continue;

                // The new mesh's name usually has a suffix, so we save the original to make switching easier.
                var name = mesh.name;

                otherFunc?.Invoke(name, mesh, component ?? filter.transform);

                if (!mesh.isReadable)
                {
                    if (!Assets.GetMesh(mesh.name).IsSome(out var m))
                        continue;
                    filter.sharedMesh = mesh = m;
                    if (!m.isReadable)
                        continue; // Already modified.
                }

                scaleFunc(name, mesh, component ?? filter.transform);
            }
        }

        public delegate void HandleMesh(string name, Mesh mesh, Component component = null);
    }
}
