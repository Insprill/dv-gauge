using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class StaticSwitch
    {
        public const float Z_OFFSET_FACTOR = 18;

        private const ushort SPLIT_MAX_VERT = 344;
        private const ushort RIGHT_RAIL_MIN_VERT = 344;
        private const ushort RIGHT_RAIL_MAX_VERT = 381;
        private const ushort LEFT_RAIL_MIN_VERT = 382;
        private const ushort LEFT_RAIL_MAX_VERT = 1369;
        private static readonly ushort[] DIVERGING_EXTEND_VERTS = {
            17, 18, 19, 21, 23, 25, 35, 40, 43, 48, 55, 60, 62, 77, 79, 85, 90, 93, 98, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222
        };
        private static readonly ushort[] STRAIGHT_EXTEND_VERTS = {
            223, 226, 227, 230, 233, 238, 240, 241, 243, 246, 250, 253, 257, 260, 264, 269, 277, 301, 308, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343
        };

        public static void ModifyMesh(Mesh mesh)
        {
            Vector3[] verts = mesh.vertices;

            float gaugeDiff = Main.Settings.gauge.GetDiffToStandard();
            float zOffset = gaugeDiff * Z_OFFSET_FACTOR;

            // Split
            for (int i = 0; i < SPLIT_MAX_VERT; i++)
            {
                verts[i].x -= gaugeDiff;
                verts[i].z += zOffset;
            }

            foreach (ushort idx in DIVERGING_EXTEND_VERTS)
            {
                verts[idx].x += gaugeDiff * 2;
                verts[idx].z -= zOffset;
            }

            foreach (ushort idx in STRAIGHT_EXTEND_VERTS) verts[idx].z -= zOffset;

            // Right rail
            for (int i = RIGHT_RAIL_MIN_VERT; i <= RIGHT_RAIL_MAX_VERT; i++) verts[i].x += gaugeDiff;

            // Left rail
            for (int i = LEFT_RAIL_MIN_VERT + 1; i <= LEFT_RAIL_MAX_VERT; i++) verts[i].x -= gaugeDiff;

            mesh.ApplyVerts(verts);
        }
    }
}
