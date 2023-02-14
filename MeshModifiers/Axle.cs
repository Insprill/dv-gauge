using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class Axle
    {
        public static void ModifyMesh(Mesh mesh)
        {
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                if (verts[i].x > 0.05) verts[i].x -= Main.Settings.gauge.GetDiffToStandard();
                else if (verts[i].x < -0.05) verts[i].x += Main.Settings.gauge.GetDiffToStandard();

            mesh.ApplyVertsAndRecalculate(verts);
        }
    }
}
