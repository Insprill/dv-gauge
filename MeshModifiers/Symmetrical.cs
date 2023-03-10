using System;
using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class Symmetrical
    {
        private const float BASE_THRESHOLD = 0.05f;

        public static void ScaleToGauge(Mesh mesh, bool useGaugeAsThreshold = false, float? baseGauge = null, ushort[] skipVerts = null, ushort[] includeVerts = null)
        {
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                if (skipVerts != null && i < ushort.MaxValue && Array.BinarySearch(skipVerts, (ushort)i) >= 0) continue;
                if (includeVerts != null && i < ushort.MaxValue && Array.BinarySearch(includeVerts, (ushort)i) < 0) continue;
                verts[i] = ScaleToGauge(verts[i], useGaugeAsThreshold, baseGauge);
            }

            mesh.ApplyVerts(verts);
        }

        public static void ScaleToGauge(Shape shape)
        {
            for (int i = 0; i < shape.transform.childCount; ++i)
            {
                Transform t = shape.transform.GetChild(i);
                t.localPosition = ScaleToGauge(t.localPosition);
            }
        }

        private static Vector3 ScaleToGauge(Vector3 vert, bool useGaugeAsThreshold = false, float? baseGauge = null)
        {
            bool isNarrow = Main.Settings.gauge.GetGauge() < Gauge.Standard.GetGauge();
            float threshold = useGaugeAsThreshold
                ? isNarrow
                    ? (Main.Settings.gauge.GetGauge() / 2) - BASE_THRESHOLD
                    : (baseGauge.GetValueOrDefault(Gauge.Standard.GetGauge()) / 2) - BASE_THRESHOLD
                : BASE_THRESHOLD;
            float gaugeDiff = baseGauge == null ? Main.Settings.gauge.GetDiffToStandard() : Main.Settings.gauge.GetDiffFrom(baseGauge.Value);
            if (vert.x > threshold) vert.x = Mathf.Max(0.001f, vert.x - gaugeDiff);
            else if (vert.x < -threshold) vert.x = Mathf.Min(-0.001f, vert.x + gaugeDiff);
            return vert;
        }
    }
}
