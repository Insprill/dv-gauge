using Gauge.Meshes;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class MovingSwitch
    {
        public static void ModifyMesh(Mesh mesh)
        {
            Vector3[] verts = mesh.vertices;

            float gaugeDiff = Gauge.Settings.RailGauge.DiffToStandard;
            bool isNarrow = gaugeDiff >= 0; // The logic related to this is sketchy, a proper fix should be found...
            float baseZOffset = gaugeDiff * StaticSwitch.Z_OFFSET_FACTOR;

            Vector3 initialStartVert = verts[Vertices.Verts.switch_moving_curve_start];
            Vector3 initialEndVert = verts[isNarrow ? Vertices.Verts.switch_moving_curve_end_narrow : Vertices.Verts.switch_moving_curve_end_broad];

            // Frog
            foreach (ushort i in Vertices.Verts.switch_moving_top_left)
            {
                verts[i].x -= gaugeDiff;
                verts[i].z += baseZOffset;
            }

            foreach (ushort i in Vertices.Verts.switch_moving_top_right)
            {
                verts[i].x -= gaugeDiff;
                verts[i].z += baseZOffset;
            }

            // Points
            foreach (ushort i in Vertices.Verts.switch_moving_bottom_left) verts[i].x -= gaugeDiff;
            foreach (ushort i in Vertices.Verts.switch_moving_bottom_right) verts[i].x += gaugeDiff;

            // Curved point
            Vector3 startVert = verts[Vertices.Verts.switch_moving_curve_start];
            Vector3 endVert = verts[isNarrow ? Vertices.Verts.switch_moving_curve_end_narrow : Vertices.Verts.switch_moving_curve_end_broad];
            ushort[][] middleRightVerts = Vertices.Verts.switch_moving_middle_right;
            float initialZOffset = (initialStartVert.z - initialEndVert.z - (startVert.z - endVert.z)) / middleRightVerts.Length;
            float zOffset = initialZOffset;

            Vector3[] curve = BezierCurve.Interpolate(startVert, verts[Vertices.Verts.switch_moving_curve_start_back], endVert, endVert, isNarrow ? middleRightVerts.Length : middleRightVerts.Length - 1);
            for (ushort seg = 0; seg < middleRightVerts.Length; seg++)
            {
                ushort[] segment = middleRightVerts[seg];
                float baseLine = Mathf.Lerp(startVert.x, endVert.x, seg / (float)middleRightVerts.Length);
                ushort railHeadCenterVert = segment[segment.Length - 3];
                float xOffset = verts[railHeadCenterVert].x - baseLine;
                foreach (ushort i in segment)
                {
                    verts[i].x += gaugeDiff;
                    verts[i].x -= xOffset;
                    verts[i].z += zOffset;
                }

                xOffset = verts[railHeadCenterVert].x - curve[seg].x;
                foreach (ushort i in segment) verts[i].x -= xOffset;

                zOffset += initialZOffset;
            }

            mesh.ApplyVerts(verts);
        }
    }
}
