using System;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class SwitchAnchors
    {
        private static readonly ushort[] REMOVE_VERTS = {
            832, 833, 834, 835, 836, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850, 851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864, 865, 866, 867, 868, 869, 870, 871,
            872, 873, 874, 875, 876, 877, 878, 879, 880, 881, 882, 883, 884, 885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 898, 899, 900, 901, 902, 903, 904, 905, 906, 907, 908, 909, 910, 911,
            912, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 930, 931, 932, 933, 934, 935, 936, 937, 938, 939, 940, 941, 942, 943, 944, 945, 946, 947, 948, 949, 950, 951,
            952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 970, 971, 972, 973, 974, 975, 976, 977, 978, 979, 980, 981, 982, 983, 984, 985, 986, 987, 988, 989, 990, 991,
            1208, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221, 1222, 1223
        };

        public static void ModifyMesh(Mesh mesh)
        {
            // This is super hacky instead of just removing them, but i don't feel like figuring that out right now
            Vector3[] verts = mesh.vertices;
            for (ushort i = 0; i < verts.Length; i++)
            {
                if (Array.BinarySearch(REMOVE_VERTS, i) < 0) continue;
                verts[i] = Vector3.zero;
            }

            mesh.vertices = verts;
            Symmetrical.ScaleToGauge(mesh);
        }
    }
}
