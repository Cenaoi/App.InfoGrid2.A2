using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace EasyClick.BizWeb2.Xml
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    public class XmlWindow
    {
        public string Name { get; set; }



        public void Parane(XmlNode node)
        {
            this.Name = XmlUtil.GetAttrValue(node, "name");
        }

        public XmlNode CreateNode(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement("Control");

            XmlUtil.SetAttrValue(node, "name", this.Name);

            return node;
        }

    }


    public class Node
    {
        NodeCollection m_Childs;

        public NodeCollection Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new NodeCollection();
                }
                return m_Childs;
            }
        }


    }

    public class NodeCollection : List<Node>
    {

    }



}
