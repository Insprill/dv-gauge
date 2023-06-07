using System.Collections.Generic;
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

        public static float GetGauge(this TrainCar trainCar)
        {
            return trainCar.Bogies[0].GetGauge();
        }

        public static float GetGauge(this Bogie bogie)
        {
            return /*Main.IsCCLEnabled ? CCL.GetGauge(bogie) :*/ Gauge.Standard.GetGauge();
        }

        public static bool IsCorrectGauge(this TrainCar trainCar)
        {
            return trainCar.Bogies[0].IsCorrectGauge();
        }

        public static bool IsCorrectGauge(this Bogie bogie)
        {
            return Mathf.Abs(Main.Settings.gauge.GetGauge() - bogie.GetGauge()) < 0.001f;
        }

        public static bool IsFront(this Bogie bogie)
        {
            return bogie.Car.Bogies[0] == bogie;
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
            return modifiedMeshes.Contains(mesh.GetHashCode());
        }

        public static void SetModified(this Mesh mesh)
        {
            modifiedMeshes.Add(mesh.GetHashCode());
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
