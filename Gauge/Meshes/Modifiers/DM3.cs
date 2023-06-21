using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class DM3
    {
        public static void ModifyMesh(string name, Mesh mesh)
        {
            switch (name)
            {
                case "dm3_wheel_01":
                case "dm3_wheel_02":
                case "dm3_wheel_03":
                case "dm3_wheel_01_LOD1":
                case "dm3_wheel_02_LOD1":
                case "dm3_wheel_03_LOD1":
                case "dm3_brake_shoes":
                    Symmetrical.ScaleToGauge(mesh);
                    break;
            }
        }
    }
}
