using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.ComponentModel;

namespace EC5.SystemBoard.Web
{
    [Obsolete]
    public static class XmlUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static SortedList<string, string> GetAttrDict(XmlNode xNode)
        {
            SortedList<string, string> attrs = new SortedList<string, string>();

            if (xNode != null && xNode.Attributes != null)
            {
                foreach (XmlAttribute attr in xNode.Attributes)
                {
                    attrs.Add(attr.Name, attr.Value);
                }
            }

            return attrs;
        }

        public static void RemoveAttr(XmlNode xNode, string attrName)
        {
            XmlAttribute attr = null;

            for (int i = 0; i < xNode.Attributes.Count; i++)
            {
                XmlAttribute tmpAttr = xNode.Attributes[i];

                if (tmpAttr.Name == attrName)
                {
                    attr = tmpAttr;
                    break;
                }
            }

            if (attr != null)
            {
                xNode.Attributes.Remove(attr);
            }

        }

        public static void SetAttrValue(XmlNode xNode, string attrName, string value)
        {
            string  attrNameLower = attrName.ToLower();

            SortedList<string, XmlAttribute> attrs = new SortedList<string, XmlAttribute>();

            if (xNode != null && xNode.Attributes != null)
            {
                foreach (XmlAttribute attr in xNode.Attributes)
                {
                    string name = attr.Name.ToLower();
                    attrs[name] = attr;
                }
            }

            if (attrs.ContainsKey(attrNameLower))
            {
                attrs[attrNameLower].Value = value;
            }
            else
            {
                XmlAttribute attr = xNode.OwnerDocument.CreateAttribute(attrName);

                xNode.Attributes.Append(attr);
                attr.Value = value;

            }
        }

        public static void SetAttrValue(XmlNode xNode, string attrName, bool value)
        {
            SetAttrValue(xNode, attrName, value.ToString());
        }

        public static void SetAttrValue(XmlNode xNode, string attrName, Int32 value)
        {
            SetAttrValue(xNode, attrName, value.ToString());
        }


        public static void SetAttrValue(XmlNode xNode, string attrName, Int64 value)
        {
            SetAttrValue(xNode, attrName, value.ToString());
        }

        public static void SetAttrValue(XmlNode xNode, string attrName, DateTime value)
        {
            SetAttrValue(xNode, attrName, value.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
        }

        public static string GetAttrValue(XmlNode xNode, string attrName)
        {
            return GetAttrValue(xNode, attrName, string.Empty);
        }


        public static string GetAttrValue(XmlNode xNode, string attrName, string emptyValue)
        {
            attrName = attrName.ToLower();

            SortedList<string, string> attrs = new SortedList<string, string>();

            if (xNode != null && xNode.Attributes != null)
            {
                foreach (XmlAttribute attr in xNode.Attributes)
                {
                    string name = attr.Name.ToLower();
                    attrs[name] = attr.Value;
                }
            }

            if (attrs.ContainsKey(attrName))
            {
                return attrs[attrName];
            }

            return emptyValue;
        }

        public static void Copy(object obj, XmlNode xNode)
        {
            if (obj is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor customTs = (ICustomTypeDescriptor)obj;

                PropertyDescriptorCollection propList = customTs.GetProperties();


                foreach (PropertyDescriptor prop in propList)
                {
                    object srcV = prop.GetValue(obj);

                    if (srcV == null) { srcV = string.Empty; }

                    SetAttrValue(xNode, prop.Name, srcV.ToString());
                }

            }
            else
            {
                Type objT = obj.GetType();

                PropertyInfo[] props = objT.GetProperties();

                foreach (PropertyInfo prop in props)
                {
                    if (!prop.CanRead )
                    {
                        continue;
                    }

                    object srcV = prop.GetValue(obj, null);

                    if (srcV == null) { srcV = string.Empty; }

                    SetAttrValue(xNode, prop.Name, srcV.ToString());

                }
            }
        }


        public static void Copy(XmlNode xNode, object obj)
        {
            if (xNode == null || obj == null) { return; }

            SortedList<string, string> attrs = new SortedList<string, string>();

            foreach (XmlAttribute attr in xNode.Attributes)
            {
                string name = attr.Name.ToLower();
                attrs[name] = attr.Name;
            }



            Type objT = obj.GetType();

            PropertyInfo[] props = objT.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (!prop.CanRead || !prop.CanWrite)
                {
                    continue;
                }

                if (!attrs.ContainsKey(prop.Name.ToLower()))
                {
                    continue;
                }

                string name = attrs[prop.Name.ToLower()];

                string valueStr = xNode.Attributes[name].Value;

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(obj, valueStr, null);
                }
                else
                {
                    try
                    {
                        object value = Convert.ChangeType(valueStr, prop.PropertyType);
                        prop.SetValue(obj, value, null);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }

            }


        }

        /// <summary>
        /// [作废] 采用 Copy
        /// </summary>
        /// <param name="xNode"></param>
        /// <param name="obj"></param>
        public static void SetObjectAttr(XmlNode xNode, object obj)
        {
            Copy(xNode, obj);    
        }

        public static string GetInnerText(XmlNode parentNode, string nodeName)
        {
            XmlNode node = parentNode.SelectSingleNode(nodeName);

            if (node == null)
            {
                return string.Empty;
            }

            return node.InnerText;
        }

        static Hashtable m_Serializers = new Hashtable();

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            return Deserialize<T>(Encoding.UTF8.GetBytes(xml));
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] xmlData)
        {
            Type entityT = typeof(T);
            string fullName = entityT.FullName;

            XmlSerializer serializer;

            #if(DEBUG)

            log.Debug("反序列化:" + fullName);

            #endif

            if (m_Serializers.Contains(fullName) && m_Serializers[fullName] != null)
            {
                serializer = (XmlSerializer)m_Serializers[fullName];
            }
            else
            {
                serializer = new XmlSerializer(entityT);

                m_Serializers[fullName] = serializer;
            }

            //StringReader tr = new StringReader(xml);

            //T t3 = default(T);

            //try
            //{

            //    t3 = (T)serializer.Deserialize(tr);
            //}

            MemoryStream stream = new MemoryStream(xmlData);
            XmlTextReader reader = new XmlTextReader(stream);

            T t3 = default(T);

            try
            {
                reader.Normalization = false;
                t3 = (T)serializer.Deserialize(reader);
            }
            finally
            {
                reader.Close();

                stream.Close();
                stream.Dispose();
            }

            return t3;
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream)
        {
            Type entityT = typeof(T);
            string fullName = entityT.FullName;

            XmlSerializer serializer;


#if(DEBUG)

            log.Debug("反序列化:" + fullName);

#endif

            if (m_Serializers.Contains(fullName) && m_Serializers[fullName] != null)
            {
                serializer = (XmlSerializer)m_Serializers[fullName];
            }
            else
            {
                serializer = new XmlSerializer(entityT);

                m_Serializers[fullName] = serializer;
            }

            T t3 = default(T);

            try
            {
                t3 = (T)serializer.Deserialize(stream);
            }
            finally
            {
            }

            return t3;
        }

        /// <summary>
        /// 序列化为 Xml 文档
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument SerializeXmlDoc(object obj)
        {

#if(DEBUG)

            log.Debug("反序列化:" + obj.GetType().FullName);

#endif

            XmlSerializer ser = new XmlSerializer(obj.GetType());

            MemoryStream ms = new MemoryStream();
            
            TextWriter writer = new StreamWriter(ms, Encoding.UTF8);

            
            XmlDocument doc = new XmlDocument();

            try
            {
                ser.Serialize(writer, obj);

                byte[] mm = ms.ToArray();

                string txt;

                if (mm[0] == 0xEF && mm[1] == 0xBB && mm[2] == 0xBF)
                {
                    txt = Encoding.UTF8.GetString(mm, 3, mm.Length - 3);
                }
                else
                {
                    txt = Encoding.UTF8.GetString(mm);
                }

                doc.LoadXml(txt);
            }
            finally
            {
                writer.Dispose();
                ms.Dispose();
            }

            return doc;

        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
#if(DEBUG)

            log.Debug("反序列化:" + obj.GetType().FullName);

#endif

            XmlSerializer ser = new XmlSerializer(obj.GetType());

            MemoryStream ms = new MemoryStream();

            TextWriter writer = new StreamWriter(ms, Encoding.UTF8);

            string txt = null;

            try
            {
                ser.Serialize(writer, obj);

                byte[] mm = ms.ToArray();
                byte[] bs;

                if (mm[0] == 0xEF && mm[1] == 0xBB && mm[2] == 0xBF)
                {
                    int newLen = (int)ms.Length - 3;

                    bs = new byte[newLen];
                    Array.Copy(mm, 3, bs, 0, newLen);
                }
                else
                {
                    bs = mm;
                }

                txt = Encoding.UTF8.GetString(bs);
            }
            finally
            {
                writer.Dispose();
                ms.Dispose();

            }

            return txt;

        }




        /// <summary>
        /// 序列化后保存
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="obj"></param>
        public static void SaveXmlFile(string filename, object obj)
        {
#if(DEBUG)

            log.Debug("反序列化:" + obj.GetType().FullName);

#endif

            XmlSerializer ser = new XmlSerializer(obj.GetType());
            TextWriter writer = null;

            try
            {
                writer = new StreamWriter(filename);
                ser.Serialize(writer, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }
        }

        /// <summary>
        /// 打开 XML 的序列化文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T OpenXmlFile<T>(string filename)
        {
#if(DEBUG)

            log.Debug("反序列化:" + typeof(T).FullName);

#endif

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream fs = null;

            T t3 = default(T);


            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                t3 = (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }


            return t3;
        }

        public static XmlAttribute AppendAttr(XmlNode owner, string name, string value)
        {
            XmlDocument doc = owner.OwnerDocument;

            XmlAttribute attr = doc.CreateAttribute(name);
            attr.Value = value;
            owner.Attributes.Append(attr);

            return attr;
        }

        public static XmlAttribute AppendAttr(XmlNode owner, string name, object value)
        {
            XmlDocument doc = owner.OwnerDocument;

            XmlAttribute attr = doc.CreateAttribute(name);
            attr.Value = (value == null) ? string.Empty : value.ToString();
            owner.Attributes.Append(attr);

            return attr;
        }

    }
}
