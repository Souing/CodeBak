using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using System.Xml.Serialization;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 序列化与反序列化帮助类.Load将文件反序列化为对象.Save将对象序列化成文件.
    /// 优先使用Save和Load，因为是标准的序列化方法，可以其他类型的程序使用。但是此方法不支持字典集合。
    /// SaveByDataContract和LoadByDataContract是使用DataContractSerializer进行序列化，
    /// 这是WCF中使用的序列化器，支持字典集合，但是系列化后的格式比较特殊。
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <example>
    /// 代码举例：
    /// <code>
    /// SimpleFileDemoConfig m_SimpleFileDemoConfig = new SimpleFileDemoConfig();
    /// //将SimpleFileDemoConfig对象序列化为一个文件
    /// SerializationHelper.Save(m_SimpleFileDemoConfig, @"d:\demo.config");
    /// //从文件反序列化一个SimpleFileDemoConfig类对象
    /// m_SimpleFileDemoConfig = SerializationHelper.Load(typeof(SimpleFileDemoConfig), @"d:\demo.config") as SimpleFileDemoConfig;
    /// </code>
    /// </example>
    [System.Serializable]
    public class SerializationHelper
    {
        private SerializationHelper()
        {
        }


        #region ========== XmlSerializer ==========
        /// <summary>
        /// 序列化，使用标准的XmlSerializer，优先考虑使用。
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }

        /// <summary>
        /// 反序列化，使用标准的XmlSerializer，优先考虑使用。
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns>type类型的对象实例</returns>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        #endregion


        #region ========== DataContractSerializer ==========
        /// <summary>
        /// 序列化，使用DataContractSerializer， 支持IDictionary接口.
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void SaveByDataContract(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 反序列化.使用DataContractSerializer序列化器, 支持IDictionary接口.
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns>对象实例</returns>
        public static object LoadByDataContract(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                DataContractSerializer serializer = new DataContractSerializer(type);
                return serializer.ReadObject(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        #endregion


        #region ========== BinaryBytes ==========
        /// <summary>
        /// 将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="obj">待保存的对象</param>
        /// <returns>byte数组</returns>
        public static byte[] SaveToBinaryBytes(object obj)
        {
            //将对象序列化到MemoryStream中
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            //从MemoryStream中获取获取byte数组
            return ms.ToArray();
        }

        /// <summary>
        /// 将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>对象</returns>
        public static object LoadFromBinaryBytes(byte[] bytes)
        {
            object result = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (bytes != null)
            {
                MemoryStream ms = new MemoryStream(bytes);
                result = formatter.Deserialize(ms);
            }
            return result;
        }
        #endregion


        #region ========= other ==========
        /// <summary>
        /// 使用BinaryFormatter将对象系列化到MemoryStream中。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>保存对象的MemoryStream</returns>
        public static MemoryStream SaveToMemoryStream(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BufferedStream stream = new BufferedStream(ms);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms;
        }


        /// <summary>
        /// Creates xml string from given dto.
        /// </summary>
        /// <param name="dto">DTO</param>
        /// <returns>XML</returns>
        public static string SerializeDTO(Object dto)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(dto.GetType());
                StringWriter sWriter = new StringWriter();
                // Serialize the dto to xml.
                xmlSer.Serialize(sWriter, dto);
                // Return the string of xml.
                return sWriter.ToString();
            }
            catch (Exception ex)
            {
                // Propogate the exception.
                //throw ex;
                return ex.Message;
            }
        }

        /// <summary>
        /// Load xml string 
        /// </summary>
        /// <param name="dto">DTO</param>
        /// <returns>XML</returns>
        public static object LoadDTO(Type type, string xmlString)
        {
            object result;
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(type);
                StringReader sReader = new StringReader(xmlString);
                System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(new StringReader(xmlString));

                // Serialize the dto to xml.
                result = xmlSer.Deserialize(xmlReader);
               
            }
            catch (Exception ex)
            {
                // Propogate the exception.
                //throw ex;
                return ex.Message;
            }
            return result;
        }

        #endregion
        
    }
}