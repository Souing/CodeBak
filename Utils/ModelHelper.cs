using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Data;

using System.Data.SqlClient;

namespace Com.Yst.Framework.Utils
{    
    /// <summary>
    /// 用于读取Model属性和设置Model属性值.
    /// </summary>
    [System.Serializable]
    public class ModelReflect
    {
        /// <summary>        
        /// 返回所有的公共属性        
        /// </summary>        
        /// <param name="obj"></param>        
        /// <returns></returns>        
        public static PropertyInfo[] GetPropertys(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] infos = type.GetProperties();
            return infos;
        }

        /// <summary>        
        /// 设置实例的指定属性值        
        /// </summary>        
        /// <param name="obj">实例</param>        
        /// <param name="property">属性名</param>        
        /// <param name="value">值</param>        
        public static void SetPropertyValue(object obj, string property, object value)
        {
            Type type = obj.GetType();
            PropertyInfo info = type.GetProperty(property);
            if (info != null)
            {
                info.SetValue(obj, value, null);
            }
        }

        /// <summary>        
        /// 返回指定属性值        
        /// </summary>        
        /// <param name="obj">实例</param>        
        /// <param name="property">属性名</param>        
        /// <returns></returns>        
        public static object GetPropertyValue(object obj, string property)
        {
            Type type = obj.GetType();
            PropertyInfo info = type.GetProperty(property);
            if (info == null)
            {
                return null;
            }
            return info.GetValue(obj, null);
        }

        /// <summary>
        /// 得到对象属性类型名
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyTypeName(Type objectType, string propertyName)
        {
            try
            {
                PropertyInfo info = objectType.GetProperty(propertyName);
                return info.PropertyType.FullName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static object ToType(Type type, string value)
        {
            if (type.FullName.Contains("System.DateTime"))
            {
                return DateTime.Parse(value);
            }

            MethodInfo[] mis = type.GetMethods();
            foreach (MethodInfo mi in mis)
            {
                if (mi.GetParameters().Length == 1 && mi.Name == "Parse")
                {
                    return mi.Invoke(null, new object[] { value });
                }
            }
            return value;
        }
    }
}
