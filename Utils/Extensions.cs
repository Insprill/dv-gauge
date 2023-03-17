using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Gauge.Utils
{
    public static class Extensions
    {
        public static BaseType Clone(this BaseType standardBaseType)
        {
            BaseType baseType = ScriptableObject.CreateInstance<BaseType>();
            baseType.baseShape = standardBaseType.baseShape;
            baseType.baseMaterial = standardBaseType.baseMaterial;
            baseType.baseOffset = standardBaseType.baseOffset;
            baseType.basePathUV = standardBaseType.basePathUV;
            baseType.basePathUVScale = standardBaseType.basePathUVScale;
            baseType.baseShapeUV = standardBaseType.baseShapeUV;
            baseType.baseShapeUVScale = standardBaseType.baseShapeUVScale;
            baseType.baseKinkFrequency = standardBaseType.baseKinkFrequency;
            baseType.baseKinkScale = standardBaseType.baseKinkScale;
            baseType.sleeperPrefabs = standardBaseType.sleeperPrefabs;
            baseType.randomizeDirection = standardBaseType.randomizeDirection;
            baseType.sleeperDistance = standardBaseType.sleeperDistance;
            baseType.sleeperVerticalOffset = standardBaseType.sleeperVerticalOffset;
            baseType.anchorPrefabs = standardBaseType.anchorPrefabs;
            baseType.randomizeAnchorDirection = standardBaseType.randomizeAnchorDirection;
            baseType.anchorVerticalOffset = standardBaseType.anchorVerticalOffset;
            baseType.collidersPrefab = standardBaseType.collidersPrefab;
            return baseType;
        }

        public static RailType Clone(this RailType standardRailType)
        {
            RailType railType = ScriptableObject.CreateInstance<RailType>();
            railType.railShape = standardRailType.railShape;
            railType.railMaterial = standardRailType.railMaterial;
            railType.gauge = standardRailType.gauge;
            railType.railEdgeOffset = standardRailType.railEdgeOffset;
            return railType;
        }

        public static float GetGauge(this TrainCar car)
        {
            return Main.IsCCLEnabled ? CCL.GetGauge(car) : Gauge.Standard.GetGauge();
        }

        #region Meshes

        private static readonly HashSet<int> modifiedMeshes = new HashSet<int>();

        public static void ApplyVerts(this Mesh mesh, Vector3[] verts)
        {
            mesh.vertices = verts;
            mesh.RecalculateBounds();
        }

        public static void AdjustY(this Mesh mesh, float offset)
        {
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++) vertices[i].y += offset;
            mesh.ApplyVerts(vertices);
        }

        public static bool IsModified(this Mesh mesh)
        {
            return modifiedMeshes.Contains(mesh.vertices.Hash());
        }

        public static void SetModified(this Mesh mesh)
        {
            modifiedMeshes.Add(mesh.vertices.Hash());
        }

        private static int Hash(this IReadOnlyCollection<Vector3> vectors)
        {
            StringBuilder sb = new StringBuilder(vectors.Count * 3 * sizeof(float)); // Close enough
            foreach (Vector3 vector in vectors)
            {
                sb.Append(vector.x);
                sb.Append(vector.y);
                sb.Append(vector.z);
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                return BitConverter.ToInt32(hashBytes, 0);
            }
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

                string name = mesh.name;

                if (!mesh.isReadable)
                {
                    Mesh m = Assets.GetMesh(mesh.name);
                    if (m == null) continue;
                    filter.sharedMesh = mesh = m;
                }

                if (mesh.IsModified())
                    continue;

                func(name, mesh, component);
            }
        }

        public delegate void HandleMesh(string name, Mesh mesh, Component component = null);

        #endregion
    }
}
