using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class Symmetrical
    {
        private const float CENTER_THRESHOLD = 0.05f;

        public static void ScaleToGauge(Mesh mesh)
        {
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] = ScaleToGauge(verts[i]);

            mesh.ApplyVertsAndRecalculate(verts);
        }

        public static void ScaleToGauge(Shape shape)
        {
            for (int i = 0; i < shape.transform.childCount; ++i)
            {
                Transform t = shape.transform.GetChild(i);
                t.localPosition = ScaleToGauge(t.localPosition);
            }
        }

        private static Vector3 ScaleToGauge(Vector3 vec)
        {
            if (vec.x > CENTER_THRESHOLD) vec.x = Mathf.Max(0, vec.x - Main.Settings.gauge.GetDiffToStandard());
            else if (vec.x < -CENTER_THRESHOLD) vec.x = Mathf.Min(0, vec.x + Main.Settings.gauge.GetDiffToStandard());
            return vec;
        }
    }
}
