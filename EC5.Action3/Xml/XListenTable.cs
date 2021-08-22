using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    /// <summary>
    /// 监听表
    /// </summary>
    [Serializable]
    [XmlRoot("listen-table")]
    public class XListenTable: XActionItem
    {

        public XListenTable()
        {
        }

        public XListenTable(string method, string table)
        {
            this.Method = method;
            this.Table = table;
        }


        /// <summary>
        /// 表名
        /// </summary>
        [XmlAttribute("table")]
        public string Table { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        [XmlAttribute("table-text")]
        public string TableText { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        [DefaultValue(true)]
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("remark")]
        public string Remark { get ; set; }

        /// <summary>
        /// 方法
        /// </summary>
        [XmlAttribute("method")]
        public string Method { get; set; }

        XScript m_CondScript;

        XScript m_VChangeScript;

        /// <summary>
        /// 脚本条件
        /// </summary>
        [XmlElement("cond-script")]
        public XScript CondScript
        {
            get { return m_CondScript; }
            set { m_CondScript = value; }
        }

        /// <summary>
        /// 值检测变化的脚本
        /// </summary>
        [XmlElement("vchange-script")]
        public XScript VChangeScript
        {
            get { return m_VChangeScript; }
            set { m_VChangeScript = value; }
        }

        /// <summary>
        /// 监听的字段
        /// </summary>
        List<XListenField> m_ListenFields;

        /// <summary>
        /// 监听的字段集合
        /// </summary>
        [XmlArray("listen-fields")]
        [XmlArrayItem("field")]
        public List<XListenField> ListenFields
        {
            get
            {
                if (m_ListenFields == null)
                {
                    m_ListenFields = new List<XListenField>();
                }
                return m_ListenFields;
            }
            set { m_ListenFields = value; }
        }

        List<XRoute> m_Routes = new List<XRoute>();

        /// <summary>
        /// 路由集合
        /// </summary>
        [XmlArrayItem("route")]
        [XmlArray("route-list")]
        public List<XRoute> Routes
        {
            get
            {
                if (m_Routes == null)
                {
                    m_Routes = new List<XRoute>();
                }
                return m_Routes;
            }
            set { m_Routes = value; }
        }
    }
}
