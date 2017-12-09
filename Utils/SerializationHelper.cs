using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using System.Xml.Serialization;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// ���л��뷴���л�������.Load���ļ������л�Ϊ����.Save���������л����ļ�.
    /// ����ʹ��Save��Load����Ϊ�Ǳ�׼�����л������������������͵ĳ���ʹ�á����Ǵ˷�����֧���ֵ伯�ϡ�
    /// SaveByDataContract��LoadByDataContract��ʹ��DataContractSerializer�������л���
    /// ����WCF��ʹ�õ����л�����֧���ֵ伯�ϣ�����ϵ�л���ĸ�ʽ�Ƚ����⡣
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <example>
    /// ���������
    /// <code>
    /// SimpleFileDemoConfig m_SimpleFileDemoConfig = new SimpleFileDemoConfig();
    /// //��SimpleFileDemoConfig�������л�Ϊһ���ļ�
    /// SerializationHelper.Save(m_SimpleFileDemoConfig, @"d:\demo.config");
    /// //���ļ������л�һ��SimpleFileDemoConfig�����
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
        /// ���л���ʹ�ñ�׼��XmlSerializer�����ȿ���ʹ�á�
        /// �������л�IDictionary�ӿ�.
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="filename">�ļ�·��</param>
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
        /// �����л���ʹ�ñ�׼��XmlSerializer�����ȿ���ʹ�á�
        /// �������л�IDictionary�ӿ�.
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="filename">�ļ�·��</param>
        /// <returns>type���͵Ķ���ʵ��</returns>
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
        /// ���л���ʹ��DataContractSerializer�� ֧��IDictionary�ӿ�.
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="filename">�ļ�·��</param>
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
        /// �����л�.ʹ��DataContractSerializer���л���, ֧��IDictionary�ӿ�.
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="filename">�ļ�·��</param>
        /// <returns>����ʵ��</returns>
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
        /// ������ʹ�ö����Ƹ�ʽ���л���byte����
        /// </summary>
        /// <param name="obj">������Ķ���</param>
        /// <returns>byte����</returns>
        public static byte[] SaveToBinaryBytes(object obj)
        {
            //���������л���MemoryStream��
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            //��MemoryStream�л�ȡ��ȡbyte����
            return ms.ToArray();
        }

        /// <summary>
        /// ��ʹ�ö����Ƹ�ʽ�����byte���鷴���л��ɶ���
        /// </summary>
        /// <param name="bytes">byte����</param>
        /// <returns>����</returns>
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
        /// ʹ��BinaryFormatter������ϵ�л���MemoryStream�С�
        /// </summary>
        /// <param name="obj">����</param>
        /// <returns>��������MemoryStream</returns>
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