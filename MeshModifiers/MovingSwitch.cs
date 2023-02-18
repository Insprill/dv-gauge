﻿using Gauge.Utils;
using UnityEngine;

namespace Gauge.MeshModifiers
{
    public static class MovingSwitch
    {
        // All variable names are assuming a switch that diverges to the left.
        private const ushort START_VERT = 1884;
        private const ushort START_VERT_BACK = 1890;
        private const ushort END_VERT = 1714;
        private static readonly ushort[] TOP_LEFT_VERTS = {
            32, 35, 36, 39, 40, 42, 43, 44, 45, 46, 47, 48, 50, 51, 52, 53, 54, 55, 56, 57, 89, 90, 91, 92, 93, 94, 95, 96, 115, 116, 117, 118, 119, 120, 121, 122, 141, 142, 143, 144, 145, 146, 147,
            148, 167, 168, 169, 170, 171, 172, 173, 174, 193, 194, 195, 196, 197, 198, 199, 200, 227, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 259, 260, 261, 262, 263, 264, 265, 266,
            267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 279, 305, 306, 307, 308, 309, 310, 311, 312, 355, 356, 357, 358, 359, 360, 361, 362, 366, 367, 368, 369, 370, 371, 372, 373, 375,
            376, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412,
            413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449,
            450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477
        };
        private static readonly ushort[] BOTTOM_LEFT_VERTS = {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 33, 34, 37, 38, 41, 49, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68,
            69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 123, 124, 125, 126,
            127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 175, 176, 177, 178, 179,
            180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224,
            225, 226, 228, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 278, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293,
            294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338,
            339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 363, 364, 365, 374, 478, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488
        };
        private static readonly ushort[] BOTTOM_RIGHT_VERTS = {
            489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 598, 599, 600, 601,
            602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714,
            715, 716, 717, 718, 719, 720, 721, 722, 723, 724, 725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751,
            752, 753, 754, 755, 756, 757, 758, 759, 760, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850, 851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864,
            865, 866, 867, 868, 869, 870, 871, 872, 873, 874, 875, 876, 877, 878, 993, 994, 995, 996, 997, 998, 999, 1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013,
            1014, 1015, 1016, 1017, 1018, 1019, 1020, 1097, 1098, 1099, 1100, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1110, 1111, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120,
            1121, 1122, 1123, 1124, 1201, 1202, 1203, 1204, 1205, 1206, 1207, 1208, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221, 1222, 1223, 1224, 1225, 1226, 1227,
            1228, 1305, 1306, 1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316, 1317, 1318, 1319, 1320, 1321, 1322, 1323, 1324, 1325, 1326, 1327, 1328, 1329, 1330, 1331, 1332, 1485, 1486,
            1487, 1488, 1489, 1490, 1491, 1492, 1493, 1494, 1495, 1496, 1687, 1688, 1876, 1880, 1881, 1882, 1883, 1884, 1885, 1886, 1887, 1888, 1889, 1890, 1891, 1892, 1893, 1894, 1895, 1896, 1897,
            1898, 1899, 1900, 1901, 1902, 1903, 1904, 1905, 1906, 1907, 1908, 1909, 1910, 1911, 1912, 1913, 1914, 1915, 1916, 1917, 1918, 1919, 1920, 1921, 1922, 1923, 1924, 1925, 1926, 1927, 1928,
            1929, 1930, 1931, 1932, 1933, 1934, 1935, 1936, 1937, 1938, 1939, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034, 2035,
            2036, 2037, 2038, 2039, 2040, 2041, 2042, 2138, 2139, 2140, 2141, 2142, 2143, 2144, 2145
        };
        private static readonly ushort[][] MIDDLE_RIGHT_VERTS = {
            new ushort[] { 522, 523, 615, 616, 761, 762, 879, 880, 881, 1021, 1022, 1125, 1126, 1229, 1230, 1333, 1334, 1483, 1484, 1497, 1498, 1684, 1685, 1686, 1871, 1875, 1877, 1878, 1879, 2014, 2015 },
            new ushort[] { 524, 525, 617, 618, 763, 764, 882, 883, 884, 1023, 1024, 1127, 1128, 1231, 1232, 1335, 1336, 1481, 1482, 1499, 1500, 1681, 1682, 1683, 1866, 1870, 1872, 1873, 1874, 2012, 2013 },
            new ushort[] { 526, 527, 619, 620, 765, 766, 885, 886, 887, 1025, 1026, 1129, 1130, 1233, 1234, 1337, 1338, 1479, 1480, 1501, 1502, 1678, 1679, 1680, 1861, 1865, 1867, 1868, 1869, 2010, 2011 },
            new ushort[] { 528, 529, 621, 622, 767, 768, 888, 889, 890, 1027, 1028, 1131, 1132, 1235, 1236, 1339, 1340, 1477, 1478, 1503, 1504, 1675, 1676, 1677, 1856, 1860, 1862, 1863, 1864, 2008, 2009 },
            new ushort[] { 530, 531, 623, 624, 769, 770, 891, 892, 893, 1029, 1030, 1133, 1134, 1237, 1238, 1341, 1342, 1475, 1476, 1505, 1506, 1672, 1673, 1674, 1851, 1855, 1857, 1858, 1859, 2006, 2007 },
            new ushort[] { 532, 533, 625, 626, 771, 772, 894, 895, 896, 1031, 1032, 1135, 1136, 1239, 1240, 1343, 1344, 1473, 1474, 1507, 1508, 1669, 1670, 1671, 1846, 1850, 1852, 1853, 1854, 2004, 2005 },
            new ushort[] { 534, 535, 627, 628, 773, 774, 897, 898, 899, 1033, 1034, 1137, 1138, 1241, 1242, 1345, 1346, 1471, 1472, 1509, 1510, 1666, 1667, 1668, 1841, 1845, 1847, 1848, 1849, 2002, 2003 },
            new ushort[] { 536, 537, 629, 630, 775, 776, 900, 901, 902, 1035, 1036, 1139, 1140, 1243, 1244, 1347, 1348, 1469, 1470, 1511, 1512, 1663, 1664, 1665, 1836, 1840, 1842, 1843, 1844, 2000, 2001 },
            new ushort[] { 538, 539, 631, 632, 777, 778, 903, 904, 905, 1037, 1038, 1141, 1142, 1245, 1246, 1349, 1350, 1467, 1468, 1513, 1514, 1660, 1661, 1662, 1831, 1835, 1837, 1838, 1839, 1998, 1999 },
            new ushort[] { 540, 541, 633, 634, 779, 780, 906, 907, 908, 1039, 1040, 1143, 1144, 1247, 1248, 1351, 1352, 1465, 1466, 1515, 1516, 1657, 1658, 1659, 1826, 1830, 1832, 1833, 1834, 1996, 1997 },
            new ushort[] { 542, 543, 635, 636, 781, 782, 909, 910, 911, 1041, 1042, 1145, 1146, 1249, 1250, 1353, 1354, 1463, 1464, 1517, 1518, 1654, 1655, 1656, 1821, 1825, 1827, 1828, 1829, 1994, 1995 },
            new ushort[] { 544, 545, 637, 638, 783, 784, 912, 913, 914, 1043, 1044, 1147, 1148, 1251, 1252, 1355, 1356, 1461, 1462, 1519, 1520, 1651, 1652, 1653, 1816, 1820, 1822, 1823, 1824, 1992, 1993 },
            new ushort[] { 546, 547, 639, 640, 785, 786, 915, 916, 917, 1045, 1046, 1149, 1150, 1253, 1254, 1357, 1358, 1459, 1460, 1521, 1522, 1648, 1649, 1650, 1811, 1815, 1817, 1818, 1819, 1990, 1991 },
            new ushort[] { 548, 549, 641, 642, 787, 788, 918, 919, 920, 1047, 1048, 1151, 1152, 1255, 1256, 1359, 1360, 1457, 1458, 1523, 1524, 1645, 1646, 1647, 1806, 1810, 1812, 1813, 1814, 1988, 1989 },
            new ushort[] { 550, 551, 643, 644, 789, 790, 921, 922, 923, 1049, 1050, 1153, 1154, 1257, 1258, 1361, 1362, 1455, 1456, 1525, 1526, 1642, 1643, 1644, 1801, 1805, 1807, 1808, 1809, 1986, 1987 },
            new ushort[] { 552, 553, 645, 646, 791, 792, 924, 925, 926, 1051, 1052, 1155, 1156, 1259, 1260, 1363, 1364, 1453, 1454, 1527, 1528, 1639, 1640, 1641, 1796, 1800, 1802, 1803, 1804, 1984, 1985 },
            new ushort[] { 554, 555, 647, 648, 793, 794, 927, 928, 929, 1053, 1054, 1157, 1158, 1261, 1262, 1365, 1366, 1451, 1452, 1529, 1530, 1636, 1637, 1638, 1791, 1795, 1797, 1798, 1799, 1982, 1983 },
            new ushort[] { 556, 557, 649, 650, 795, 796, 930, 931, 932, 1055, 1056, 1159, 1160, 1263, 1264, 1367, 1368, 1449, 1450, 1531, 1532, 1633, 1634, 1635, 1786, 1790, 1792, 1793, 1794, 1980, 1981 },
            new ushort[] { 558, 559, 651, 652, 797, 798, 933, 934, 935, 1057, 1058, 1161, 1162, 1265, 1266, 1369, 1370, 1447, 1448, 1533, 1534, 1630, 1631, 1632, 1781, 1785, 1787, 1788, 1789, 1978, 1979 },
            new ushort[] { 560, 561, 653, 654, 799, 800, 936, 937, 938, 1059, 1060, 1163, 1164, 1267, 1268, 1371, 1372, 1445, 1446, 1535, 1536, 1627, 1628, 1629, 1776, 1780, 1782, 1783, 1784, 1976, 1977 },
            new ushort[] { 562, 563, 655, 656, 801, 802, 939, 940, 941, 1061, 1062, 1165, 1166, 1269, 1270, 1373, 1374, 1443, 1444, 1537, 1538, 1624, 1625, 1626, 1771, 1775, 1777, 1778, 1779, 1974, 1975 },
            new ushort[] { 564, 565, 657, 658, 803, 804, 942, 943, 944, 1063, 1064, 1167, 1168, 1271, 1272, 1375, 1376, 1441, 1442, 1539, 1540, 1621, 1622, 1623, 1766, 1770, 1772, 1773, 1774, 1972, 1973 },
            new ushort[] { 566, 567, 659, 660, 805, 806, 945, 946, 947, 1065, 1066, 1169, 1170, 1273, 1274, 1377, 1378, 1439, 1440, 1541, 1542, 1618, 1619, 1620, 1761, 1765, 1767, 1768, 1769, 1970, 1971 },
            new ushort[] { 568, 569, 661, 662, 807, 808, 948, 949, 950, 1067, 1068, 1171, 1172, 1275, 1276, 1379, 1380, 1437, 1438, 1543, 1544, 1615, 1616, 1617, 1756, 1760, 1762, 1763, 1764, 1968, 1969 },
            new ushort[] { 570, 571, 663, 664, 809, 810, 951, 952, 953, 1069, 1070, 1173, 1174, 1277, 1278, 1381, 1382, 1435, 1436, 1545, 1546, 1612, 1613, 1614, 1751, 1755, 1757, 1758, 1759, 1966, 1967 },
            new ushort[] { 572, 573, 665, 666, 811, 812, 954, 955, 956, 1071, 1072, 1175, 1176, 1279, 1280, 1383, 1384, 1433, 1434, 1547, 1548, 1609, 1610, 1611, 1746, 1750, 1752, 1753, 1754, 1964, 1965 },
            new ushort[] { 574, 575, 667, 668, 813, 814, 957, 958, 959, 1073, 1074, 1177, 1178, 1281, 1282, 1385, 1386, 1431, 1432, 1549, 1550, 1606, 1607, 1608, 1741, 1745, 1747, 1748, 1749, 1962, 1963 },
            new ushort[] { 576, 577, 669, 670, 815, 816, 960, 961, 962, 1075, 1076, 1179, 1180, 1283, 1284, 1387, 1388, 1429, 1430, 1551, 1552, 1603, 1604, 1605, 1736, 1740, 1742, 1743, 1744, 1960, 1961 },
            new ushort[] { 578, 579, 671, 672, 817, 818, 963, 964, 965, 1077, 1078, 1181, 1182, 1285, 1286, 1389, 1390, 1427, 1428, 1553, 1554, 1600, 1601, 1602, 1731, 1735, 1737, 1738, 1739, 1958, 1959 },
            new ushort[] { 580, 581, 673, 674, 819, 820, 966, 967, 968, 1079, 1080, 1183, 1184, 1287, 1288, 1391, 1392, 1425, 1426, 1555, 1556, 1597, 1598, 1599, 1726, 1730, 1732, 1733, 1734, 1956, 1957 },
            new ushort[] { 582, 583, 675, 676, 821, 822, 969, 970, 971, 1081, 1082, 1185, 1186, 1289, 1290, 1393, 1394, 1423, 1424, 1557, 1558, 1594, 1595, 1596, 1721, 1725, 1727, 1728, 1729, 1954, 1955 },
            new ushort[] { 584, 585, 677, 678, 823, 824, 972, 973, 974, 1083, 1084, 1187, 1188, 1291, 1292, 1395, 1396, 1421, 1422, 1559, 1560, 1591, 1592, 1593, 1716, 1720, 1722, 1723, 1724, 1952, 1953 },
            new ushort[] { 586, 587, 679, 680, 825, 826, 975, 976, 977, 1085, 1086, 1189, 1190, 1293, 1294, 1397, 1398, 1419, 1420, 1561, 1562, 1588, 1589, 1590, 1713, 1715, 1717, 1718, 1719, 1950, 1951 }
        };
        private static readonly ushort[] TOP_RIGHT_VERTS = {
            588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 681, 682, 683, 684, 685, 686, 687, 688, 689, 690, 827, 828, 829, 830, 831, 832, 833, 834, 835, 836, 978, 979, 980, 981, 982, 983, 984,
            985, 986, 987, 988, 989, 990, 991, 992, 1087, 1088, 1089, 1090, 1091, 1092, 1093, 1094, 1095, 1096, 1191, 1192, 1193, 1194, 1195, 1196, 1197, 1198, 1199, 1200, 1295, 1296, 1297, 1298,
            1299, 1300, 1301, 1302, 1303, 1304, 1399, 1400, 1401, 1402, 1403, 1404, 1405, 1406, 1407, 1408, 1409, 1410, 1411, 1412, 1413, 1414, 1415, 1416, 1417, 1418, 1563, 1564, 1565, 1566, 1567,
            1568, 1569, 1570, 1571, 1572, 1573, 1574, 1575, 1576, 1577, 1578, 1579, 1580, 1581, 1582, 1583, 1584, 1585, 1586, 1587, 1689, 1690, 1691, 1692, 1693, 1694, 1695, 1696, 1697, 1698, 1699,
            1700, 1701, 1702, 1703, 1704, 1705, 1706, 1707, 1708, 1709, 1710, 1711, 1712, 1714, 1940, 1941, 1942, 1943, 1944, 1945, 1946, 1947, 1948, 1949, 2043, 2044, 2045, 2046, 2047, 2048, 2049,
            2050, 2051, 2052, 2053, 2054, 2055, 2056, 2057, 2058, 2059, 2060, 2061, 2062, 2063, 2064, 2065, 2066, 2067, 2068, 2069, 2070, 2071, 2072, 2073, 2074, 2075, 2076, 2077, 2078, 2079, 2080,
            2081, 2082, 2083, 2084, 2085, 2086, 2087, 2088, 2089, 2090, 2091, 2092, 2093, 2094, 2095, 2096, 2097, 2098, 2099, 2100, 2101, 2102, 2103, 2104, 2105, 2106, 2107, 2108, 2109, 2110, 2111,
            2112, 2113, 2114, 2115, 2116, 2117, 2118, 2119, 2120, 2121, 2122, 2123, 2124, 2125, 2126, 2127, 2128, 2129, 2130, 2131, 2132, 2133, 2134, 2135, 2136, 2137
        };

        public static void ModifyMesh(Mesh mesh)
        {
            Vector3[] verts = mesh.vertices;
            Vector3 initialStartVert = verts[START_VERT];
            Vector3 initialEndVert = verts[END_VERT];

            float gaugeDiff = Main.Settings.gauge.GetDiffToStandard();
            float baseZOffset = gaugeDiff * StaticSwitch.Z_OFFSET_FACTOR;

            // Frog
            foreach (ushort i in TOP_LEFT_VERTS)
            {
                verts[i].x -= gaugeDiff;
                verts[i].z += baseZOffset;
            }

            foreach (ushort i in TOP_RIGHT_VERTS)
            {
                verts[i].x -= gaugeDiff;
                verts[i].z += baseZOffset;
            }

            // Points
            foreach (ushort i in BOTTOM_LEFT_VERTS) verts[i].x -= gaugeDiff;
            foreach (ushort i in BOTTOM_RIGHT_VERTS) verts[i].x += gaugeDiff;

            // Curved point
            Vector3 startVert = verts[START_VERT];
            Vector3 endVert = verts[END_VERT];
            float initialZOffset = Mathf.Abs((initialStartVert.z - initialEndVert.z - (startVert.z - endVert.z)) / MIDDLE_RIGHT_VERTS.Length);
            float zOffset = initialZOffset;

            Vector3[] curve = BezierCurve.Interpolate(startVert, verts[START_VERT_BACK], endVert, endVert, MIDDLE_RIGHT_VERTS.Length);
            for (int seg = 0; seg < MIDDLE_RIGHT_VERTS.Length; seg++)
            {
                ushort[] segment = MIDDLE_RIGHT_VERTS[seg];
                float baseLine = Mathf.Lerp(startVert.x, endVert.x, seg / (float)MIDDLE_RIGHT_VERTS.Length);
                int railHeadCenterVert = segment[segment.Length - 3];
                float xOffset = verts[railHeadCenterVert].x - baseLine;
                foreach (ushort i in segment)
                {
                    verts[i].x += gaugeDiff;
                    verts[i].x -= xOffset;
                    verts[i].z += zOffset;
                }

                xOffset = verts[railHeadCenterVert].x - curve[seg].x;
                foreach (ushort i in segment) verts[i].x -= xOffset;

                zOffset += initialZOffset;
            }

            mesh.ApplyVerts(verts);
        }
    }
}
