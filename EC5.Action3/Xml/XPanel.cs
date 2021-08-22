using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    [Serializable]
    [XmlRoot("panel")]
    public class XPanel
    {
        /// <summary>
        /// 编码
        /// </summary>
        [XmlAttribute("code")]
        public string Code { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        [XmlAttribute("enabled")]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        ArrayList m_Items = new ArrayList();

        [XmlArray("items")]
        [XmlArrayItem(Type = typeof(XListenTable),ElementName ="listen-table")]
        [XmlArrayItem(Type = typeof(XOperateTable),ElementName ="operate-table")]
        public ArrayList Items
        {
            get
            {
                if(m_Items == null)
                {
                    m_Items = new ArrayList();
                }
                return m_Items;
            }
            set { m_Items = value; }
        }

    }
}
