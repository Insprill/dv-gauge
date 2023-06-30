using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class DM3
    {
        public static void ModifyMesh(string name, Mesh mesh, Component component)
        {
            switch (name)
            {
                case "dm3_exterior_body":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.dm3_body_include);
                    break;
                case "dm3_exterior_body_LOD1":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.dm3_body_lod1_include);
                    break;
                case "dm3_wheel_01":
                case "dm3_wheel_01_LOD1":
                case "dm3_wheel_02":
                case "dm3_wheel_02_LOD1":
                case "dm3_wheel_03":
                case "dm3_wheel_03_LOD1":
                case "dm3_brake_shoes":
                    Symmetrical.ScaleToGauge(mesh);
                    break;
            }
        }
    }
}
