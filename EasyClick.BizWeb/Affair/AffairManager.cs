using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

namespace EasyClick.BizWeb.Affair
{
    /// <summary>
    /// 事务管理
    /// </summary>
    public static class AffairManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        static AffairCategory m_Items = new AffairCategory();

        public static void Exe(string categoryName)
        {
            if (!m_Items.ContainsKey(categoryName))
            {
                return;
            }

            m_Items[categoryName].Exe();
        }

        private static SortedList<string, string> ToObjectAttr(XmlNode xNode)
        {
            SortedList<string, string> attrs = new SortedList<string, string>();

            if (xNode != null)
            {
                foreach (XmlAttribute attr in xNode.Attributes)
                {
                    string name = attr.Name.ToLower();

                    if (attrs.ContainsKey(name))
                    {
                        attrs[name] = attr.Name;
                    }
                    else
                    {
                        attrs.Add(name, attr.Name);
                    }
                }
            }

            return attrs;
        }

        private static void SetObjectValues(XmlNode xNode, object obj)
        {
            SortedList<string, string> attrs = ToObjectAttr(xNode);


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
        /// 配置事务
        /// </summary>
        /// <param name="filename">文件名</param>
        public static void Configer(string filename)
        {
            if (!File.Exists(filename))
            {
                return;
            }


            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(filename);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return;
            }

            XmlElement xRoot = doc.DocumentElement;
            m_Items.Clear();

            #region 处理 identityFactoryConfigs 节点

            XmlNodeList xCategorys = xRoot.SelectNodes("category");

            foreach (XmlNode xCategory in xCategorys)
            {
                AffairInfoList affairs = new AffairInfoList();
                SetObjectValues(xCategory, affairs);

                XmlNodeList xItems = xCategory.SelectNodes("add");

                foreach (XmlNode xItem in xItems)
                {
                    AffairInfo ai = new AffairInfo();

                    SetObjectValues(xItem, ai);

                    affairs.Add(ai);
                }

                if (affairs.Count > 0)
                {
                    m_Items.Add(affairs.Name, affairs);
                }

            }


            #endregion


        }

    }
}
