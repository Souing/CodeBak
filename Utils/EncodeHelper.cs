using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 编码解码帮助类。
    /// 获取参数时请使用此方法，避免使用默认的编码。
    /// 比如url传递参数通常使用utf编码格式，但是服务器配置为使用GB2312编码格式解码则产生错误。
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class EncodeHelper
    {

        #region ==================== Private Filed ====================
        #endregion


        #region ==================== Property ====================
        #endregion


        #region ==================== Constructed Method ====================
        private EncodeHelper()
        {
        }
        #endregion


        #region ==================== Private Method ====================
        #endregion


        #region ==================== Public Method ====================

        /// <summary>
        /// 根据编码类型获取请求参数。对于POST不解码，因为POST可以传递中文。
        /// 通常使用此方法。
        /// </summary>
        /// <remarks>
        /// 2009.8.18       zhineng.li         created
        /// </remarks>
        /// <param name="request">当前请求的request对象</param>
        /// <param name="encode">编码枚举值</param>
        /// <returns>键为参数名,值为参数值的NameValue集合</returns>
        public static NameValueCollection GetRequestParameters(HttpRequest request, EEncodingType encoding)
        {
            NameValueCollection result = null;
            Encoding destEncode = null;

            //获取指定编码格式的Encoding对象
            try
            {
                //获取指定的编码格式
                destEncode = Encoding.GetEncoding((int)encoding);
            }
            catch
            {
                //如果获取指定编码格式失败,则使用默认的编码类型
                destEncode = Encoding.GetEncoding(0);
            }


            //根据不同的HttpMethod方式,获取请求的参数.如果是POST则不进行编码。
            if (request.HttpMethod == "POST")
            {
                result = request.Form;
            }
            else
            {
                result = System.Web.HttpUtility.ParseQueryString(request.Url.Query, destEncode);
            }

            //返回结果
            return result;
        }

        /// <summary>
        /// 根据编码类型获取请求参数。对于POST请求同样解码。
        /// </summary>
        /// <remarks>
        /// 2009.8.18       zhineng.li         created
        /// </remarks>
        /// <param name="request">当前请求的request对象</param>
        /// <param name="encode">编码枚举值</param>
        /// <returns>键为参数名,值为参数值的NameValue集合</returns>
        public static NameValueCollection GetRequestParametersDecode(HttpRequest request, EEncodingType encoding)
        {
            NameValueCollection result = null;
            Encoding destEncode = null;

            //获取指定编码格式的Encoding对象
            try
            {
                //获取指定的编码格式
                destEncode = Encoding.GetEncoding((int)encoding);
            }
            catch
            {
                //如果获取指定编码格式失败,则使用默认的编码类型
                destEncode = Encoding.GetEncoding(0);
            }


            //根据不同的HttpMethod方式,获取请求的参数.如果没有Encoding对象则使用服务器端默认的编码.
            if (request.HttpMethod == "POST")
            {
                Stream resStream = request.InputStream;
                byte[] filecontent = new byte[resStream.Length];
                resStream.Read(filecontent, 0, filecontent.Length);
                string postquery = destEncode.GetString(filecontent);
                result = HttpUtility.ParseQueryString(postquery, destEncode);
            }
            else
            {
                result = System.Web.HttpUtility.ParseQueryString(request.Url.Query, destEncode);
            }

            //返回结果
            return result;
        }

        public static NameValueCollection GetParameters(string _requestString, EEncodingType encoding)
        {
            NameValueCollection result = null;
            Encoding destEncode = null;

            //获取指定编码格式的Encoding对象
            try
            {
                //获取指定的编码格式
                destEncode = Encoding.GetEncoding((int)encoding);
            }
            catch
            {
                //如果获取指定编码格式失败,则使用默认的编码类型
                destEncode = Encoding.GetEncoding(0);
            }
            //按照指定编码类型进行解码
            result = System.Web.HttpUtility.ParseQueryString(_requestString, destEncode);
            return result;
        }
        #endregion
        
    }

    /// <summary>
    /// 编码类型的枚举值
    /// </summary>
    /// <remarks>
    /// 2009.9.18       zhineng.li         created
    /// </remarks>
    public enum EEncodingType
    {
        /// <summary>
        /// 使用默认编码类型
        /// </summary>
        defaultType = 0,
        Utf7 = 65000,
        Utf8 = 65001,
        Utf16 = 1200,
        Utf32 = 65005,
        Gb2312 = 936,
        GB18030 = 54936,
        Big5 = 950,
        Iso88591 = 28591,
        Iso88592 = 28592,
        Iso88593 = 28593,
        Iso88594 = 28594,
        Iso88595 = 28595,
        Iso88596 = 28596,
        Iso88597 = 28597,
        Iso88598 = 28598,
        Iso88599 = 28599
    }

}
