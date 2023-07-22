using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class S060
    {
        public static void ModifyMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                case "s060_Wheels_01":
                    Symmetrical.ScaleToGauge(mesh, skipVerts: Vertices.Verts.s060_wheel_1_skip);
                    break;
                case "s060_Wheels_02":
                case "s060_Wheels_03":
                    Symmetrical.ScaleToGauge(mesh, skipVerts: Vertices.Verts.s060_wheels_2_3_skip);
                    break;
                case "s060_body":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.s060_body_include);
                    break;
                case "s060_body_LOD1":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.s060_body_lod1_include);
                    break;
                case "s060_Wheels_01_LOD1":
                case "s060_Wheels_02_LOD1":
                case "s060_Wheels_03_LOD1":
                case "s060_brake_shoes":
                    Symmetrical.ScaleToGauge(mesh);
                    break;
            }
        }
    }
}
