using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class MovingSwitch
    {
        private const short VERT_COUNT = 2146;

        public static void ModifyMesh(MeshFilter filter)
        {
            Mesh mesh = filter.mesh;
            Vector3[] verts = mesh.vertices;
            if (verts.Length != VERT_COUNT)
            {
                Main.Logger.Error($"Unexpected vertex count for rails_moving. Expected {VERT_COUNT}, got {verts.Length}.");
                return;
            }

            float gaugeDiff = Main.Settings.gauge.GetDiffToStandard();

            // TODO: Modify mesh

            mesh.vertices = verts;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
}
