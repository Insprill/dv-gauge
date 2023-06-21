﻿using System;
using Gauge.Meshes;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class Symmetrical
    {
        private const float BASE_THRESHOLD_METERS = 0.05f;

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

        public static void ScaleToGauge(Transform parent, float? baseGauge = null)
        {
            for (int i = 0; i < parent.childCount; ++i)
            {
                Transform child = parent.GetChild(i);
                child.localPosition = ScaleToGauge(child.localPosition, baseGauge: baseGauge);
            }
        }

        private static Vector3 ScaleToGauge(Vector3 vert, bool useGaugeAsThreshold = false, float? baseGauge = null)
        {
            float threshold = useGaugeAsThreshold
                ? baseGauge.GetValueOrDefault(RailGauge.STANDARD.Gauge) / 2f - BASE_THRESHOLD_METERS
                : BASE_THRESHOLD_METERS;
            float gaugeDiff = baseGauge == null ? Gauge.Instance.RailGauge.DiffToStandard : Gauge.Instance.RailGauge.GetDiffFrom(baseGauge.Value);
            if (vert.x > threshold) vert.x = Mathf.Max(0.001f, vert.x - gaugeDiff);
            else if (vert.x < -threshold) vert.x = Mathf.Min(-0.001f, vert.x + gaugeDiff);
            return vert;
        }
    }
}
