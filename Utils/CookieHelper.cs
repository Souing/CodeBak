using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// Cookie帮助类。用于读写单条Cookies的值。
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CookieHelper
    {

        #region ==================== Private Filed ====================
        #endregion


        #region ==================== Property ====================
        #endregion


        #region ==================== Constructed Method ====================
        public CookieHelper()
        {
        }    
        #endregion


        #region ==================== Private Method ====================
        #endregion


        #region ==================== Public Method ====================

        #region SetCookie() : 设置Cookies方法

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="day">过期天数</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string cookieValue, int days)
        {
            return CookieHelper.SetCookie(cookieName, cookieValue, DateTime.Now.AddDays(days));
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="expireTime">过期日期</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string cookieValue, DateTime expireTime)
        {
            return CookieHelper.SetCookie(cookieName, null, cookieValue, expireTime);
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="subKeyName">Cookies子键的名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="day">过期天数</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string subKeyName, string cookieValue, int days)
        {
            return CookieHelper.SetCookie(cookieName, subKeyName, cookieValue, DateTime.Now.AddDays(days));
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="expireTime">日期</param>
        /// <returns>是否成功</returns>
        public static bool SetCookie(string cookieName, string subKeyName, string cookieValue, DateTime expireTime)
        {
            if (string.IsNullOrEmpty(cookieName)) return false;

			//Cookies的作用域
            HttpResponse response = HttpContext.Current.Response;
            HttpRequest request = HttpContext.Current.Request;
            HttpCookie objCookie = null;

            //先判断是否有Cookies对象。如果有则需要先获取。
            if (response.Cookies[cookieName] == null)
            {
                objCookie = new HttpCookie(cookieName);
            }
            else
            {
                objCookie = response.Cookies[cookieName];
            }           



            //设定cookie 域名.
            string domain = request.Params["HTTP_HOST"];
            //如果是多级域名,需要转化为xxx.com
            if (domain.IndexOf(".") > -1)
            {
                string[] temp = domain.Split('.');

                if (temp.Length >= 3)
                {
                    domain = temp[temp.Length - 2].Trim() + "." + temp[temp.Length - 1].Trim();
                }
            }

            //设定cookie 过期时间.
            objCookie.Expires = expireTime;
			//设定cookie域名
			objCookie.Domain = domain;
            //写入Cookies值   
            if (!String.IsNullOrEmpty(subKeyName))
            {
                objCookie[subKeyName] = HttpUtility.UrlEncode(cookieValue.Trim());
            }
            else
            {
                objCookie.Value = HttpUtility.UrlEncode(cookieValue.Trim());
            }
			//保存cookie，可以输出到客户端。
			response.Cookies.Set(objCookie);

            return true;
        }

        #endregion

        #region GetCookie() : 获取Cookies
        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="cookieName">Cookie键名</param>
        /// <returns>Cookies值</returns>
        public static string GetCookie(string cookieName)
        {
            return CookieHelper.GetCookie(cookieName, null);
        }

        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="cookieName">Cookie键名</param>
        /// <param name="subKeyName">子键名</param>
        /// <returns>Cookies值</returns>
        public static string GetCookie(string cookieName, string subKeyName)
        {
            string result = string.Empty;
            HttpRequest request = HttpContext.Current.Request;

            try
            {
                if (request.Cookies[cookieName] != null)
                {
                    if (!String.IsNullOrEmpty(subKeyName))
                    {
                        result = HttpUtility.UrlDecode(request.Cookies[cookieName][subKeyName]);
                    }
                    else if (request.Cookies[cookieName] != null)
                    {
						result = HttpUtility.UrlDecode(request.Cookies[cookieName].Value.Trim());
                    }
                }
            }
            catch(Exception e)
            {
                result = string.Empty;
            }

            return result;
        }

        #endregion

        #endregion             

    }    
}
