/****************************************************************************************************
 * *
 * *        文件名    : SharpZipLibHelp.cs
 * *        功能说明  : 压缩加密字符串
 * *
 * *        备注      : 
 * *  
 * ****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Com.Yst.Framework.Utils
{
    [Serializable]
    public class SharpZipLibHelp
    {
        /// <summary>
        /// 压缩字符串
        /// add by zhuzhijia
        /// </summary>
        /// <param name="inString">待压缩的字符串</param>
        /// <returns>压缩后的字符串</returns>
        public static string ZipString(string inString)
        {
            string outString = string.Empty;
            IFormatter mFormat = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            mFormat.Serialize(ms, inString);
            byte[] byt = Compress(ms.ToArray());
            outString = Convert.ToBase64String(byt);

            return outString;
        }

        /// <summary>
        /// 解压字符串
        /// add by zhuzhijia
        /// </summary>
        /// <param name="inString">待解压的字符串</param>
        /// <returns>解压后的字符串</returns>
        public static string UnZipString(string inString)
        {
            string outString = string.Empty;
            IFormatter mFormat = new BinaryFormatter();
            byte[] byt = DeCompress(System.Convert.FromBase64String(inString));
            MemoryStream ms = new MemoryStream(byt);
            outString = mFormat.Deserialize(ms).ToString();
            return outString;
        }
        /// <summary>
        /// 压缩
        /// add by zhuzhijia
        /// </summary>
        /// <param name="pBytes">byte数组</param>
        /// <returns>byte数组</returns>
        private static byte[] Compress(byte[] pBytes)
        {
            MemoryStream mMemory = new MemoryStream();

            Deflater mDeflater = new Deflater(Deflater.BEST_COMPRESSION);
            //DeflaterOutputStream mStream = new DeflaterOutputStream( mMemory,mDeflater,131072 );
            DeflaterOutputStream mStream = new DeflaterOutputStream(mMemory, mDeflater);

            mStream.Write(pBytes, 0, pBytes.Length);
            mStream.Close();

            return mMemory.ToArray();
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="pBytes">byte数组</param>
        /// <returns>byte数组</returns>
        private static byte[] DeCompress(byte[] pBytes)
        {
            InflaterInputStream mStream = new InflaterInputStream(new MemoryStream(pBytes));

            MemoryStream mMemory = new MemoryStream();
            Int32 mSize;

            byte[] mWriteData = new byte[4096];

            while (true)
            {
                mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
                if (mSize > 0)
                {
                    mMemory.Write(mWriteData, 0, mSize);
                }
                else
                {
                    break;
                }
            }

            mStream.Close();
            return mMemory.ToArray();
        }
    }
}
