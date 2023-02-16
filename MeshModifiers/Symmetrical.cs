using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class Symmetrical
    {
        private const float BASE_THRESHOLD = 0.05f;

        public static void ScaleToGauge(Mesh mesh, bool useGaugeAsThreshold = false, float? baseGauge = null)
        {
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] = ScaleToGauge(verts[i], useGaugeAsThreshold, baseGauge);

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

        private static Vector3 ScaleToGauge(Vector3 vec, bool useGaugeAsThreshold = false, float? baseGauge = null)
        {
            float threshold = useGaugeAsThreshold ? Main.Settings.gauge.GetGauge() / 2 : BASE_THRESHOLD;
            float gaugeDiff = baseGauge == null ? Main.Settings.gauge.GetDiffToStandard() : Main.Settings.gauge.GetDiffFrom(baseGauge.Value);
            if (vec.x > threshold) vec.x = Mathf.Max(0, vec.x - gaugeDiff);
            else if (vec.x < -threshold) vec.x = Mathf.Min(0, vec.x + gaugeDiff);
            return vec;
        }
    }
}
