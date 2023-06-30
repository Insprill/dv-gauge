using System;
using System.IO;
using Newtonsoft.Json;

namespace Gauge
{
    [Serializable]
    public struct Vertices
    {
        private const string FILE_NAME = "vertices.json";
        public static Vertices Verts { get; private set; }

        public static void Load(string directory)
        {
            Gauge.Instance.Logger.LogInfo("Loading vertices...");
            string json = File.ReadAllText(Path.Combine(directory, FILE_NAME));
            Verts = JsonConvert.DeserializeObject<Vertices>(json);
        }

        #region DM3

        public ushort[] dm3_body_include;
        public ushort[] dm3_body_lod1_include;

        #endregion

        #region DE2

        public ushort[] de2_axle_skip;

        #endregion

        # region S282

        public ushort[] s282_body_include;
        public ushort[] s282_body_lod1_include;
        public ushort[] s282_driving_wheels_1_2_4_skip;
        public ushort[] s282_driving_wheel_3_skip;
        public ushort[] s282_cab_include;

        #endregion

        #region Switches

        public ushort[] switch_anchor_remove;
        public ushort[] switch_sleeper_skip;
        public ushort[] switch_moving_top_left;
        public ushort[] switch_moving_top_right;
        public ushort[] switch_moving_bottom_left;
        public ushort[] switch_moving_bottom_right;
        public ushort[][] switch_moving_middle_right;
        public ushort switch_moving_curve_start;
        public ushort switch_moving_curve_start_back;
        public ushort switch_moving_curve_end_narrow;
        public ushort switch_moving_curve_end_broad;

        #endregion
    }
}
