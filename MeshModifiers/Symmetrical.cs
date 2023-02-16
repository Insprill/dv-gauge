using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class Symmetrical
    {

        public static void ScaleToGauge(Mesh mesh, float? baseGauge = null)
        {
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] = ScaleToGauge(verts[i], baseGauge);

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

        private static Vector3 ScaleToGauge(Vector3 vec, float? baseGauge = null)
        {
            float threshold = Main.Settings.gauge.GetGauge();
            float gaugeDiff = baseGauge == null ? Main.Settings.gauge.GetDiffToStandard() : GaugeExtensions.GetDiffToStandard(baseGauge.Value);
            if (vec.x > threshold) vec.x = Mathf.Max(0, vec.x - gaugeDiff);
            else if (vec.x < -threshold) vec.x = Mathf.Min(0, vec.x + gaugeDiff);
            return vec;
        }
    }
}
