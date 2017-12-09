using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Yst.Framework.Utils
{
    public class HtmlUtility
    {
        public static List<string> GetMatchList(string text,string regex,int index)
        {
            List<string> list = new List<string>();
            MatchCollection mc =  Regex.Matches(text, regex, RegexOptions.IgnoreCase);
            foreach (Match match in mc)
            {                
                if (match.Success)
                {
                    string t = match.Result("$" + index.ToString());
                    list.Add(t);
                }                    
            }
            return list;
        }
        /// <summary>
        /// 移除Tag
        /// </summary>
        /// <param name="TagName">tag名，如:a</param>
        /// <param name="IsRemoveText">是否仅仅移除标签名，保留标签里面文字内容</param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string TagName, bool IsOnlyTag,string htmlData,bool IsIgnoreCase)
        {
            string RegxString = string.Format("<{0} ([\\w\\W]+?)>([\\w\\W]+?)</{0}>",TagName);
            string temp = htmlData; 
            MatchCollection mc = IsIgnoreCase?Regex.Matches(htmlData,RegxString, RegexOptions.IgnoreCase):Regex.Matches(htmlData,RegxString);
            foreach (Match match in mc)
            {
                if (IsOnlyTag)
                {
                    temp = temp.Replace(match.Result("$0"), match.Result("$2"));
                }
                else
                {
                    temp = temp.Replace(match.Result("$0"), string.Empty);
                }
            }

            return temp;
        }

        /// <summary>
        /// 只得到图片文件名
        /// </summary>
        /// <param name="ImageUrl"></param>
        /// <returns></returns>
        public static string GetFileName(string ImageUrl)
        {
            if (string.IsNullOrEmpty(ImageUrl))
                return string.Empty;

            string temp = ImageUrl.Replace("\\", "/");
            if (temp.StartsWith("/"))
                return string.Empty;

            string[] arr = temp.Split('/');

            return arr[arr.Length-1];

        }

        /// <summary>
        /// 根据页面URL得到Domain
        /// </summary>
        /// <param name="pageUrl">页面URL</param>
        /// <returns></returns>
        public static string GetDomain(string pageUrl)
        {
            string Domain = pageUrl.ToLower().Replace("http://", "");
            int i = Domain.IndexOf("/");
            if (i != -1)
            {
                Domain = Domain.Substring(0, i);
            }
            else
            {
                i = Domain.IndexOf("?");
                if (i != -1)
                    Domain = Domain.Substring(0, i);
            }
            return Domain;
        }
    }
}
