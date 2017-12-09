using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 字符串帮助类。
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class StringHelper
    {

        #region ==================== Private Filed ====================
        #endregion


        #region ==================== Property ====================
        #endregion


        #region ==================== Constructed Method ====================
        #endregion


        #region ==================== Private Method ====================
        #endregion


        #region ==================== Public Method ====================

        /// <summary>
        /// 截取文件路径字符串的前n级目录
        /// 如:传入 d:\aaa\bbb\ccc\ddd, n=2, 则返回 d:\aaa\bbb 
        /// </summary>
        /// <remarks>
        /// 2008.7.4: zhineng.li, Created.
        /// </remarks>
        /// <param name="filePath">需要截取的文件路径字符串</param>
        /// <param name="n">截取级别</param>
        /// <returns>截取后的文件路径</returns>
        public static string SubFilePath(string filePath, int level)
        {
            string result = string.Empty;
            string[] filePathArray = filePath.Split('\\');

            for (int i = 0; i < filePathArray.Length - level; i++)
            {
                result += filePathArray[i] + @"\";
            }

            return result;
        }

        /// <summary>
        /// 获取安全的SQL字符串，过滤掉非法SQL字符
        /// </summary>
        /// <param name="sqlString">原始SQL字符串</param>
        /// <returns>过滤后的安全SQL字符串</returns>
        public static string GetSafeSqlString(string sqlString)
        {
            string result = String.Empty;
            if (!String.IsNullOrEmpty(sqlString))
            {
                result = sqlString;
                result = result.Replace("'", "''");
                result = result.Replace("{", "");
                result = result.Replace("}", "");
                result = result.Replace("%", "");
                result = result.Replace("+", "");
                result = result.Replace("--", "");
                result = result.Replace("/*", "");
                result = result.Replace("*/", "");
                result = result.Replace("delete", "");
                result = result.Replace("drop", "");
                result = result.Replace("insert", "");
                result = result.Replace("select", "");
                result = result.Replace("update", "");
                result = result.Replace("grant", "");
                result = result.Replace("alter", "");
                result = result.Replace("|", "");
                result = result.Replace("0x", "");
            }
            return result;
        }

        #endregion

    }
}
