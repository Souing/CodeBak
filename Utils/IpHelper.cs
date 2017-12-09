using System;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// IP��ַ�����ࡣ���ڻ�ȡ�ͻ���IP��������IP�Լ�ת��IP��ַ��ʽ��
    /// </summary>
    /// <remarks>
    /// </remarks>
    [System.Serializable]
    public class IpHelper
    {

        #region ==================== Private Filed ====================
        #endregion


        #region ==================== Property ====================
        #endregion


        #region ==================== Constructed Method ====================

        public IpHelper()
        {
        }

        #endregion


        #region ==================== Private Method ====================
        #endregion


        #region ==================== Public Method ====================

        /// <summary>
        /// �õ��ͻ���IP
        /// </summary>
        /// <param name="httpContext">HttpContext����</param>
        /// <returns>�ͻ���ip�ַ���</returns>
        public static string GetClientIp(HttpContext httpContext)
        {
            string result = string.Empty;

            try
            {
                result = httpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch
            {
                return "";
            }

            return result;
        }

        /// <summary>
        /// �õ��ͻ���SessionId
        /// </summary>
        /// <param name="httpContext">HttpContext����</param>
        /// <returns>�ͻ��˵�SessionId</returns>
        public static string GetClientSessionId(HttpContext httpContext)
        {
            string result = String.Empty;
            if (httpContext.Session != null
                && !String.IsNullOrEmpty(httpContext.Session.SessionID))
            {
                result = httpContext.Session.SessionID;
            }

            return result;
        }

        /// <summary>
        /// ��÷�����IP
        /// </summary>
        /// <returns>������IP</returns>
        [Obsolete("�벻Ҫʹ�ø÷�����ʹ��GetServerIpV4IP����GetServerIpV6IP����ȡIp��ַ��")]
        public static string GetServerIp()
        {
            string result = String.Empty;
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList; //��� IP �б�
            result = addressList[0].ToString();
            return result;
        }


        /// <summary>
        /// ͨ��IP���ͻ�ȡIp
        /// </summary>
        /// <param name="ipType"></param>
        /// <returns></returns>
        private static string GetIPByType(AddressFamily ipType)
        {
            try
            {
                string result = String.Empty;
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    bool find = false;
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == ipType)
                            {
                                result = ip.Address.ToString();
                                find = true;
                                break;
                            }
                        }
                    }
                    if (find) break;
                }
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// ��ȡIpV4IP��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetServerV4IP()
        {
            return GetIPByType(AddressFamily.InterNetwork);
        }

        /// <summary>
        /// ��ȡIpV6IP��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetServerV6IP()
        {
            return GetIPByType(AddressFamily.InterNetworkV6);
        }


        /// <summary>
        /// ��127.0.0.1��ʽ��IP��ַת����10����������ʽIP��ַ
        /// </summary>
        /// <param name="Ip">127.0.0.1��ʽ��IP</param>
        /// <returns>10����������ʽ��IP</returns>
        public static long IpToLong(string Ip)
        {
            if (Ip == null) return 0;

            string[] IpArray = Ip.Split('.');

            if (IpArray.Length != 4) return 0;

            long[] ip = new long[4];

            for (int i = 0; i < 4; i++)
                ip[i] = NumberHelper.ParseInt64(IpArray[i], 0);

            return (ip[0] << 24) + (ip[1] << 16) + (ip[2] << 8) + ip[3];
        }

        /// <summary>
        /// ��10����������ʽת����127.0.0.1��ʽ��IP��ַ
        /// </summary>
        /// <param name="longIP">10����������ʽ��IP</param>
        /// <returns>127.0.0.1��ʽ��IP</returns>
        public static String LongToIP(long longIP)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(longIP >> 24);
            sb.Append(".");
            sb.Append((longIP & 0x00FFFFFF) >> 16);
            sb.Append(".");
            sb.Append((longIP & 0x0000FFFF) >> 8);
            sb.Append(".");
            sb.Append(longIP & 0x000000FF);
            return sb.ToString();
        }

        #endregion

    }
}
