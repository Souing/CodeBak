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
    /// IP地址帮助类。用于获取客户端IP、服务器IP以及转换IP地址格式。
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
        /// 得到客户端IP
        /// </summary>
        /// <param name="httpContext">HttpContext对象</param>
        /// <returns>客户端ip字符串</returns>
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
        /// 得到客户端SessionId
        /// </summary>
        /// <param name="httpContext">HttpContext对象</param>
        /// <returns>客户端的SessionId</returns>
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
        /// 获得服务器IP
        /// </summary>
        /// <returns>服务器IP</returns>
        [Obsolete("请不要使用该方法，使用GetServerIpV4IP或者GetServerIpV6IP来获取Ip地址。")]
        public static string GetServerIp()
        {
            string result = String.Empty;
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList; //获得 IP 列表
            result = addressList[0].ToString();
            return result;
        }


        /// <summary>
        /// 通过IP类型获取Ip
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
        /// 获取IpV4IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetServerV4IP()
        {
            return GetIPByType(AddressFamily.InterNetwork);
        }

        /// <summary>
        /// 获取IpV6IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetServerV6IP()
        {
            return GetIPByType(AddressFamily.InterNetworkV6);
        }


        /// <summary>
        /// 将127.0.0.1形式的IP地址转换成10进制整数形式IP地址
        /// </summary>
        /// <param name="Ip">127.0.0.1形式的IP</param>
        /// <returns>10进制整数形式的IP</returns>
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
        /// 将10进制整数形式转换成127.0.0.1形式的IP地址
        /// </summary>
        /// <param name="longIP">10进制整数形式的IP</param>
        /// <returns>127.0.0.1形式的IP</returns>
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
