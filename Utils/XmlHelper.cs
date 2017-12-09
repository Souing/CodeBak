using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// XML实用类
    /// </summary>
    public class XmlHelper
    {
        #region 得到指定键名和值的XML字符串

        /// <summary>
        /// 得到指定键名和值的XML字符串
        /// </summary>
        /// <param name="Name">键名</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public static string GetNameValue(string Name, object Value)
        {
            if (Value != null)
                return "<" + Name + ">" + System.Web.HttpUtility.HtmlEncode(Value.ToString()) + "</" + Name + ">\r\n";
            else
                return "<" + Name + "/>\r\n";
        }

        #endregion

        #region 将XML反序列成对象
        /// <summary>
        /// XML反序列成对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object LoadSerializedObject(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        #endregion

        #region 将对象序列化成XML
        /// <summary>
        /// 将对象序列化成XML
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="filename">指定文件</param>
        public static void SerializeObject(object o, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());

            // Create an XmlSerializerNamespaces object.
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            // Add two namespaces with prefixes.
            ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create an XmlTextWriter using a FileStream.
            Stream fs = new FileStream(filename, FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, new UTF8Encoding());

            // Serialize using the XmlTextWriter.
            serializer.Serialize(writer, o, ns);
            writer.Close();
            fs.Close();
        }



        /// <summary>
        /// 将对象序列化为XMl
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializableXML(object o)
        {
            string orderxml = "";
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(o.GetType());
                xmls.Serialize(ms, o);
                ms.Position = 0;
                orderxml = System.Text.Encoding.GetEncoding("utf-8").GetString(ms.ToArray());
            }
            catch
            {

            }
            return orderxml;
        }

        #endregion

        #region 安全的XPATH
        public static string SafeXpathString(string input)
        {
            // In comes the string.
            // Out comes a safely quoted string 
            // for use in XPATH.
            bool sngl_qt = input.IndexOf("'") > -1;
            bool reg_qt = input.IndexOf("\"") > -1;
            string output = "";

            if (sngl_qt && reg_qt)
            {
                // Build a concat function to 
                // make our string work.
                string[] parts = input.Split('\'');
                output = "concat('";
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i > 0) output += ",\"'\",'";
                    output += parts[i] + "'";
                }
                output += ")";
            }
            else if (sngl_qt && !reg_qt)
            {
                //Wrap just single quotes with regualar quotes
                output = "\"" + input + "\"";
            }
            else
            {
                //Wrap everything else in single quotes
                output = "'" + input + "'";
            }

            return output;
        }

        //http://www.holmok.com/quotes_xpath.aspx 


        //======================================

        //Quotes in Strings in XPath

        //Argh!!! The Quotes in XPATH String Expressions.
        //I have to admit it.  I am somewhat of a XML/XSLT/XPATH junky.  I am aware of all the shortcomings.  I will only talk about one of them here.  I am talking about single quotes(') and regular quotes(") in strings in XPATH expressions. Ugh.

        //Let me start by telling you a story.  I made a file persisted object store using the XmlSerializer Class.  The cool thing about it is that you can use XPATH to search the stored files, and use XSLT for reporting on the stored objects.  I was very proud of this object store. It worked great and did not take many lines of code to right.  It was a tight piece of code.

        //So when I ran the test methods in Nunit, and it choked. It choked on strings with single quotes. Here is the offending line of code:

        //string xpath = string.Format(
        // "./Username='{0}' and ./Password='{1}'",
        // username,
        // password);Whenever a username or a password had a single quote in it, it broke.  The other option is to wrap the string expressions in the XPATH with regular quotes ("), but then if username or password had a regular quote in it, it would break.

        //I know what you are thinking, just see if it has a single or regular quote and wrap the string expression in the XPATH with the other type of quote.  That will work until a string has both a single quote and a regular quote. Then what?

        //Ah! The Solution!
        //XPATH has no real way to escape quotes in a string expression. So now what? Well, XPATH does include a string concatenation function. After thinking about it, and sleeping on it, I came up with the following static method.


        //The first if-block checks to if the string has single quotes or regular quotes.  If there are both in the string we convert the input string into a concat function to be used for the XPATH expression. It will create something that is illustrated in the following example:

        //Input string: This "cat" is old and it's grey.

        //Method ouput: concat('This "cat" is old and it', "'",'s grey.')

        //This way the XPATH expression will, in effect, return the input string. And it will have both single and regular quotes in the string.  Pretty neat, huh? And don't forget that if the input string just has single quotes in it, the method will wrap it in regular quotes and vise-versa.  By default will return the string wrapped in single quotes by default.

        //Using It
        //So remember that code that broke, in the beginning of this article.  Well here is the new code that implements this method.

        //string xpath = string.Format(
        // "./Username={0} and ./Password={1}",
        // SafeXpathString(username),
        // SafeXpathString(password));Take notice that since the SafeXpathString method adds the quotes around the string, you should NOT add them when you make the actual XPATH string. Woot! Now I can search for any string in my object store using XPATH.


         #endregion
    }
}
