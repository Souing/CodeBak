using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// 枚举操作类
    /// </summary>
    public class EnumHelper
    {

        /// <summary>
        /// 获取所有的枚举名字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetAllEnumNames(Type type)
        {
            return Enum.GetNames(type).ToList();
        }

        /// <summary>
        /// 获取所有的枚举名字
        /// </summary>
        /// <param name="enm"></param>
        /// <returns></returns>
        public static List<string> GetAllEnumNames(Enum enm)
        {
            return Enum.GetNames(enm.GetType()).ToList();
        }

        /// <summary>
        /// 获取所有枚举和枚举值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetAllNameAndValue(Type type)
        {
            var dic = new Dictionary<string, int>();
            foreach (var value in Enum.GetValues(type))
            {
                dic.Add(value.ToString(), Convert.ToInt32(value));
            }
            return dic;
        }

        /// <summary>
        /// 通过枚举获取描述信息
        /// </summary>
        /// <param name="enm"></param>
        /// <param name="defaultDes">默认描述</param>
        /// <returns>返回描述信息，可能为空</returns>
        public static string GetDescriptionByEnum(Enum enm, string defaultDes = "")
        {
            string description = defaultDes;
            object[] objAttrs = enm.GetType().GetField(enm.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (objAttrs != null &&
                objAttrs.Length > 0)
            {
                DescriptionAttribute descAttr = objAttrs[0] as DescriptionAttribute;
                description = descAttr.Description;
            }
            return description;
        }

        /// <summary>
        /// 字符串转换成枚举，转换失败会抛异常
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enm">枚举名字</param>
        /// <returns>返回转换成功以后的值，转换失败会抛异常</returns>
        public static TEnum GetEnumTypeByName<TEnum>(string enm) where TEnum : struct
        {
            TEnum type;
            // 需要通过此方法再次确定是否是枚举实际定义的内容
            if (Enum.TryParse(enm, true, out type) && Enum.IsDefined(typeof(TEnum), type))
            {
                return type;
            }
            else
            {
                throw new ArgumentException("非枚举成员");
            }
        }
    }
}
