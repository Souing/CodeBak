using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 用于通信时的数据加密
    /// </summary>
    [System.Serializable]
    public class SecureEncoder
    {

        private static string sKey = "flaksdjfaoiuoiweuroiuw";//加密,密匙.静态方法.
        public SecureEncoder()
        {

        }

        /// <summary>
        /// 加/解密函数
        /// </summary>
        /// <param name="strEncrypted"></param>
        /// <returns></returns>
        public static string NumCryptStatic(string strEncrypted)
        {
            string g_Key = sKey;
            int iKeyChar;
            int iStringChar;
            string strDecrypted = "";
            int iDeCryptChar;
            for (int i = 0; i < strEncrypted.Length; i++)
            {
                iKeyChar = (int)System.Convert.ToChar(g_Key.Substring(i, 1));
                iStringChar = (int)System.Convert.ToChar(strEncrypted.Substring(i, 1));
                iDeCryptChar = iKeyChar ^ iStringChar;
                strDecrypted = strDecrypted + (char)iDeCryptChar;
            }
            return strDecrypted;
        }

        /// <summary>
        /// 加/解密函数.可以提供密匙.
        /// </summary>
        /// <param name="strEncrypted"></param>
        /// <returns></returns>
        public static string NumCryptStatic(string strEncrypted, string strKey)
        {
            string g_Key = strKey;
            int iKeyChar;
            int iStringChar;
            string strDecrypted = "";
            int iDeCryptChar;
            for (int i = 0; i < strEncrypted.Length; i++)
            {
                iKeyChar = (int)System.Convert.ToChar(g_Key.Substring(i, 1));
                iStringChar = (int)System.Convert.ToChar(strEncrypted.Substring(i, 1));
                iDeCryptChar = iKeyChar ^ iStringChar;
                strDecrypted = strDecrypted + (char)iDeCryptChar;
            }
            return strDecrypted;
        }

    }
}
