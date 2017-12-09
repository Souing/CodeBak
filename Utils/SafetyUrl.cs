using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Com.Yst.Framework.Utils
{


    public class SafetyUrl
    {
        /// <summary>
        /// 判断是否为合法的url
        /// </summary>
        /// <param name="url">输入需要检测的Url</param>
        /// <returns>如果非法返回false，正常Url返回true</returns>
        public static bool CheckUrl(string url)
        {
            bool safety = true;
            try
            {
                string param = string.Empty;
                try
                {
                    Uri checkUrl = new Uri(url);
                    //可判断Host
                    //判断参数
                    param = checkUrl.Query;
                    param = HttpUtility.UrlDecode(param.Replace("?", ""));
                }
                catch
                {
                    param = url;
                }
                if (!string.IsNullOrEmpty(param))
                {
                    //规则1：不允许含有<>同时出现的情况
                    Regex reg = new Regex(".*<.*>.*");
                    safety = !reg.IsMatch(param);
                }
                //其他规则判断

            }
            catch (Exception ex)
            {
                
                return false;
            }
            return safety;
        }

        /// <summary>
        /// 替换相应的字符串
        /// </summary>
        /// <param name="url">输入要替换的Url</param>
        /// <returns>将所有非法字符替换为空格</returns>
        public static string ReplaceUrl(string url)
        {
            string tempUrl = "";
            if (!string.IsNullOrEmpty(url))
            {
                Regex reg = new Regex("[<|>]");
                tempUrl = reg.Replace(url, "");
            }
            return tempUrl;

        }
    }

}
 
