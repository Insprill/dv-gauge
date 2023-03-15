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

        public static float GetGauge(this TrainCar car)
        {
            return Main.IsCCLEnabled ? CCL.GetGauge(car) : Gauge.Standard.GetGauge();
        }
    }
}
