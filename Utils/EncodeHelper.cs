using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// �����������ࡣ
    /// ��ȡ����ʱ��ʹ�ô˷���������ʹ��Ĭ�ϵı��롣
    /// ����url���ݲ���ͨ��ʹ��utf�����ʽ�����Ƿ���������Ϊʹ��GB2312�����ʽ�������������
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
        /// ���ݱ������ͻ�ȡ�������������POST�����룬��ΪPOST���Դ������ġ�
        /// ͨ��ʹ�ô˷�����
        /// </summary>
        /// <remarks>
        /// 2009.8.18       zhineng.li         created
        /// </remarks>
        /// <param name="request">��ǰ�����request����</param>
        /// <param name="encode">����ö��ֵ</param>
        /// <returns>��Ϊ������,ֵΪ����ֵ��NameValue����</returns>
        public static NameValueCollection GetRequestParameters(HttpRequest request, EEncodingType encoding)
        {
            NameValueCollection result = null;
            Encoding destEncode = null;

            //��ȡָ�������ʽ��Encoding����
            try
            {
                //��ȡָ���ı����ʽ
                destEncode = Encoding.GetEncoding((int)encoding);
            }
            catch
            {
                //�����ȡָ�������ʽʧ��,��ʹ��Ĭ�ϵı�������
                destEncode = Encoding.GetEncoding(0);
            }


            //���ݲ�ͬ��HttpMethod��ʽ,��ȡ����Ĳ���.�����POST�򲻽��б��롣
            if (request.HttpMethod == "POST")
            {
                result = request.Form;
            }
            else
            {
                result = System.Web.HttpUtility.ParseQueryString(request.Url.Query, destEncode);
            }

            //���ؽ��
            return result;
        }

        /// <summary>
        /// ���ݱ������ͻ�ȡ�������������POST����ͬ�����롣
        /// </summary>
        /// <remarks>
        /// 2009.8.18       zhineng.li         created
        /// </remarks>
        /// <param name="request">��ǰ�����request����</param>
        /// <param name="encode">����ö��ֵ</param>
        /// <returns>��Ϊ������,ֵΪ����ֵ��NameValue����</returns>
        public static NameValueCollection GetRequestParametersDecode(HttpRequest request, EEncodingType encoding)
        {
            NameValueCollection result = null;
            Encoding destEncode = null;

            //��ȡָ�������ʽ��Encoding����
            try
            {
                //��ȡָ���ı����ʽ
                destEncode = Encoding.GetEncoding((int)encoding);
            }
            catch
            {
                //�����ȡָ�������ʽʧ��,��ʹ��Ĭ�ϵı�������
                destEncode = Encoding.GetEncoding(0);
            }


            //���ݲ�ͬ��HttpMethod��ʽ,��ȡ����Ĳ���.���û��Encoding������ʹ�÷�������Ĭ�ϵı���.
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

            //���ؽ��
            return result;
        }

        public static NameValueCollection GetParameters(string _requestString, EEncodingType encoding)
        {
            NameValueCollection result = null;
            Encoding destEncode = null;

            //��ȡָ�������ʽ��Encoding����
            try
            {
                //��ȡָ���ı����ʽ
                destEncode = Encoding.GetEncoding((int)encoding);
            }
            catch
            {
                //�����ȡָ�������ʽʧ��,��ʹ��Ĭ�ϵı�������
                destEncode = Encoding.GetEncoding(0);
            }
            //����ָ���������ͽ��н���
            result = System.Web.HttpUtility.ParseQueryString(_requestString, destEncode);
            return result;
        }
        #endregion
        
    }

    /// <summary>
    /// �������͵�ö��ֵ
    /// </summary>
    /// <remarks>
    /// 2009.9.18       zhineng.li         created
    /// </remarks>
    public enum EEncodingType
    {
        /// <summary>
        /// ʹ��Ĭ�ϱ�������
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
