using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class StaticSwitch
    {
        private const short VERT_COUNT = 1370;

        // Vertices going clockwise. All verts are duplicated for the edges of the rail, and the outline of the end
        private static readonly short[] DIVERGING_EXTEND_VERTS = {
            18, 17, 19, 21, 23, 25, 35, 40, 43, 48, 55, 60, 62, 77, 79, 85, 90, 93, 98,
            204, 207, 208, 210, 212, 215, 216, 214, 219, 218, 217, 222, 220, 221, 213, 211, 209, 205, 206
        };
        private static readonly short[] STRAIGHT_EXTEND_VERTS = {
            308, 301, 277, 269, 264, 260, 257, 253, 250, 246, 243, 241, 240, 238, 233, 230, 227, 226, 223,
            325, 328, 329, 331, 333, 336, 337, 335, 340, 339, 338, 343, 341, 342, 334, 332, 330, 326, 327
        };

        public static void ModifyMesh(MeshFilter filter)
        {
            Mesh mesh = filter.mesh;
            Vector3[] verts = mesh.vertices;
            if (verts.Length != VERT_COUNT)
            {
                Main.Logger.Error($"Unexpected vertex count for rails_static. Expected {VERT_COUNT}, got {verts.Length}.");
                return;
            }

            float gaugeDiff = Main.Settings.gauge.GetDiffToStandard();
            float frogZOffset = gaugeDiff * 16;

            // https://blender.stackexchange.com/a/3253

            // Split
            for (int i = 0; i < 344; i++)
            {
                verts[i].x -= gaugeDiff;
                verts[i].z += frogZOffset;
            }

            foreach (short idx in DIVERGING_EXTEND_VERTS)
            {
                verts[idx].x += gaugeDiff * 2;
                verts[idx].z -= frogZOffset;
            }

            foreach (short idx in STRAIGHT_EXTEND_VERTS) verts[idx].z -= frogZOffset;

            // Right rail
            for (int i = 344 - 1; i < 381; i++) verts[i].x += gaugeDiff;

            // Left rail
            for (int i = 382 - 1; i < 1370; i++) verts[i].x -= gaugeDiff;

            mesh.ApplyVertsAndRecalculate(verts);
        }
    }
}
