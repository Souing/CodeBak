using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Yst.Framework.Utils
{

    /// <summary>
    /// 数字帮助类。包含检测字符串是否是数字等方法。
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NumberHelper
    {

        #region ==================== Private Filed ====================
        #endregion


        #region ==================== Property ====================

        /// <summary>
        /// 是否是数字的正则表达式（不包含大小检测）
        /// </summary>
        public Regex NumberRegex
        {
            get { return m_NumberRegex; }
            set { m_NumberRegex = value; }
        }
        private static Regex m_NumberRegex = new Regex(@"^\d+$");

        #endregion


        #region ==================== Constructed Method ====================
        #endregion


        #region ==================== Private Method ====================
        #endregion


        #region ==================== Public Method ====================

        /// <summary>
        /// 是否是数字（不包含大小检测）
        /// </summary>
        /// <param name="theValue">待检测字符串</param>
        /// <returns>是否是数字</returns>
        public static bool IsNumber(string theValue)
        {
            Match m = m_NumberRegex.Match(theValue);
            return m.Success;
        }

        /// <summary>
        /// 是否是32位Int类型字符串（检测大小）
        /// </summary>
        /// <param name="theValue">待检测字符串</param>
        /// <returns>是否是Int32</returns>
        public static bool IsInt32(string theValue)
        {
            bool result = false;

            Match m = m_NumberRegex.Match(theValue);
            result = m.Success;
            if (result)
            {
                //如果是数字，再检测数字大小
                try
                {
                    int tempInt = int.Parse(theValue);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 是否是64位Int类型字符串（检测大小）
        /// </summary>
        /// <param name="theValue">待检测字符串</param>
        /// <returns>是否是Int64</returns>
        public static bool IsInt64(string theValue)
        {
            bool result = false;

            Match m = m_NumberRegex.Match(theValue);
            result = m.Success;
            if (result)
            {
                //如果是数字，再检测数字大小
                try
                {
                    long tempInt = long.Parse(theValue);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 转化成Int32，不抛出异常,出错则返回-1。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>int32</returns>
        public static int ParseInt32(string source)
        {
            return ParseInt32(source, -1);
        }

        /// <summary>
        /// 转化成Int32，不抛出异常。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>int32</returns>
        public static int ParseInt32(string source, int defaultValue)
        {
            int result = defaultValue;
            try
            {
                result = Convert.ToInt32(source);
            }
            catch
            {
                result = defaultValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转化成Int64，不抛出异常,出错则返回-1。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>int64</returns>
        public static long ParseInt64(string source)
        {
            return ParseInt64(source, -1L);
        }

        /// <summary>
        /// 转化成Int64，不抛出异常。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>int64</returns>
        public static long ParseInt64(string source, long defaultValue)
        {
            long result = defaultValue;
            try
            {
                result = Convert.ToInt64(source);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 转化成float，不抛出异常,出错则返回-1。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>float</returns>
        public static float ParseFloat(string source)
        {
            return ParseFloat(source, -1F);
        }

        /// <summary>
        /// 转化成float，不抛出异常。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>float</returns>
        public static float ParseFloat(string source, float defaultValue)
        {
            float result = defaultValue;
            try
            {
                result = Convert.ToSingle(source);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 转化成double，不抛出异常,出错则返回-1。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>double</returns>
        public static double ParseDouble(string source)
        {
            return ParseDouble(source, -1D);
        }

        /// <summary>
        /// 转化成double，不抛出异常。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>double</returns>
        public static double ParseDouble(string source, double defaultValue)
        {
            double result = defaultValue;
            try
            {
                result = Convert.ToDouble(source);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 转化成decimal，不抛出异常,出错则返回-1。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>decimal</returns>
        public static decimal ParseDecimal(string source)
        {
            return ParseDecimal(source, -1M);
        }

        /// <summary>
        /// 转化成decimal，不抛出异常。
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>decimal</returns>
        public static decimal ParseDecimal(string source, decimal defaultValue)
        {
            decimal result = defaultValue;
            try
            {
                result = Convert.ToDecimal(source);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        #endregion

    }
}
