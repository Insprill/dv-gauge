using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class DE2
    {
        public static void ModifyMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                case "ext 621_exterior":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.de2_body_include);
                    break;
                case "ext_brakes_F":
                case "ext_brakes_R":
                    Symmetrical.ScaleToGauge(mesh);
                    break;
            }
        }
    }
}
