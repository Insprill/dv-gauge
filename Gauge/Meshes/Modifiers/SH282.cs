using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class SH282
    {
        public static void ModifyMesh(string name, Mesh mesh)
        {
            Gauge.Instance.Logger.LogDebug($"Modifying S282 mesh {name}");
            switch (name)
            {
                case "s282_wheels_driving_1":
                case "s282_wheels_driving_2":
                case "s282_wheels_driving_4":
                    Symmetrical.ScaleToGauge(mesh, skipVerts: Vertices.Verts.s282_driving_wheels_1_2_4_skip);
                    break;
                case "s282_wheels_driving_3":
                    Symmetrical.ScaleToGauge(mesh, skipVerts: Vertices.Verts.s282_driving_wheel_3_skip);
                    break;
                case "s282_locomotive_body":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.s282_body_include);
                    break;
                case "s282_locomotive_body_LOD1":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.s282_body_lod1_include);
                    break;
                case "s282_cab_LOD1":
                    Symmetrical.ScaleToGauge(mesh, includeVerts: Vertices.Verts.s282_cab_include);
                    break;
                case "s282_wheels_driving_1_LOD1":
                case "s282_wheels_driving_2_LOD1":
                case "s282_wheels_driving_3_LOD1":
                case "s282_wheels_driving_4_LOD1":
                case "s282_brake_shoes":
                case "s282_wheels_front":
                case "s282_wheels_front_LOD1":
                case "s282_wheels_rear":
                    Symmetrical.ScaleToGauge(mesh);
                    break;
                case "s282_wheels_front_support":
                    Symmetrical.ScaleToGauge(mesh, true);
                    break;
            }
        }
    }
}
