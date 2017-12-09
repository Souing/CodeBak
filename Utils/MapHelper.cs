using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 高斯投影中所选用的参考椭球
    /// </summary>
    public enum GaussSphere
    {
        Beijing54,
        Xian80,
        WGS84,
    }

    /// <summary>
    /// 地图帮助类
    /// </summary>
    public class MapHelper
    {
        /// <summary>
        /// 计算亮点间的距离
        /// </summary>
        /// <param name="lng1"></param>
        /// <param name="lat1"></param>
        /// <param name="lng2"></param>
        /// <param name="lat2"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static double DistanceOfTwoPoints(double lng1, double lat1, double lng2, double lat2, GaussSphere gs)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lng1) - Rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = (s * (gs == GaussSphere.WGS84 ? 6378137.0 : (gs == GaussSphere.Xian80 ? 6378140.0 : 6378245.0)));
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }
    }
}
