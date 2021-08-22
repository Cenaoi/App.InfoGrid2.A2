using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    /// <summary>
    /// 操作表
    /// </summary>
    [Serializable]
    public class XOperateTable: XActionItem
    {
        /// <summary>
        /// 操作数据表
        /// </summary>
        public XOperateTable()
        {

        }

        /// <summary>
        /// 操作数据表
        /// </summary>
        /// <param name="method"></param>
        /// <param name="table"></param>
        public XOperateTable(string method, string table)
        {
            this.Method = method;
            this.Table = table;
        }

        /// <summary>
        /// 数据表名
        /// </summary>
        [XmlAttribute("table")]
        public string Table { get; set; }

        /// <summary>
        /// 数据表描述
        /// </summary>
        [XmlAttribute("table-text")]
        public string TableText { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; } = true;


        /// <summary>
        /// 操作表命令
        /// </summary>
        [XmlAttribute("method")]
        public string Method { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("remark")]
        public string Remark { get; set; }

        List<XOperateField> m_NewFields;
        List<XOperateField> m_UpdateFields;

        /// <summary>
        /// 新建的字段集合
        /// </summary>
        [XmlArray("new-fields")]
        [XmlArrayItem("field")]
        public List<XOperateField> NewFields
        {
            get
            {
                if(m_NewFields == null)
                {
                    m_NewFields = new List<XOperateField>();
                }

                return m_NewFields;
            }
            set { m_NewFields = value; }
        }

        /// <summary>
        /// 更新的字段集合
        /// </summary>
        [XmlArray("update-fields")]
        [XmlArrayItem("field")]
        public List<XOperateField> UpdateFields
        {
            get
            {
                if(m_UpdateFields == null)
                {
                    m_UpdateFields = new List<XOperateField>();
                }
                return m_UpdateFields;
            }
            set { m_UpdateFields = value; }
        }

        XScript m_FilterScript;

        /// <summary>
        /// 过滤条件
        /// </summary>
        [XmlElement("filter-script")]
        public XScript FilterScript
        {
            get { return m_FilterScript; }
            set { m_FilterScript = value; }
        }

        /// <summary>
        /// 更新后的代码
        /// </summary>
        [XmlElement("updated-script")]
        public XScript UpdatedScript { get; set; }

        /// <summary>
        /// 更新前编码
        /// </summary>
        [XmlElement("updating-script")]
        public XScript UpdatingScript { get; set; }

        /// <summary>
        /// 插入后代码
        /// </summary>
        [XmlElement("inserted-script")]
        public XScript InsertedScript { get; set; }

        /// <summary>
        /// 插入前代码
        /// </summary>
        [XmlElement("inserting-script")]
        public XScript InsertingScript { get; set; }

        /// <summary>
        /// 删除前编码
        /// </summary>
        [XmlElement("deleting-script")]
        public XScript DeletingScript { get; set; }

        /// <summary>
        /// 删除后编码
        /// </summary>
        [XmlElement("deleted-script")]
        public XScript DeletedScript { get; set; }


        /// <summary>
        /// (以后备用)查询后编码
        /// </summary>
        [XmlElement("selected-script")]
        public XScript SelectedScript { get; set; }

        /// <summary>
        /// (以后备用)查询前编码
        /// </summary>
        [XmlElement("selecting-script")]
        public XScript SelectingScript { get; set; }



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
