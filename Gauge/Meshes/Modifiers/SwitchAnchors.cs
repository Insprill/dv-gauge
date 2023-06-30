using System;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class SwitchAnchors
    {
        public static void ModifyMesh(Mesh mesh)
        {
            // This is super hacky instead of just removing them, but i don't feel like figuring that out right now
            Vector3[] verts = mesh.vertices;
            for (ushort i = 0; i < verts.Length; i++)
            {
                if (Array.BinarySearch(Vertices.Verts.switch_anchor_remove, i) < 0) continue;
                verts[i] = Vector3.zero;
            }

            mesh.vertices = verts;
            Symmetrical.ScaleToGauge(mesh);
        }
    }
}
