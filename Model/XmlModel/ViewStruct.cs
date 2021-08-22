using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Model.XmlModel
{
    [XmlRoot("control")]
    public class ViewStruct
    {
        [XmlIgnore]
        private List<ViewStruct> m_viewStruct;


        /// <summary>
        /// 显示名
        /// </summary>
        [XmlAttribute("text")]
        public string text { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        [XmlAttribute("table")]
        public string tableName { get; set; }

        /// <summary>
        /// 显示模式
        /// </summary>
        [XmlAttribute("displayMode")]
        public string displayMode { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        [XmlAttribute("id")]
        public string id { get; set; }

        /// <summary>
        /// 表ID
        /// </summary>
        [XmlAttribute("tableId")]
        public string tableId { get; set; }


        /// <summary>
        /// true -- 普通节点
        /// </summary>
        [XmlAttribute("isPanel")]
        public bool isPanel { get; set; }


        [XmlElement("control")]
        public List<ViewStruct> viewStruct 
        {
            get 
            {
                if (m_viewStruct == null) 
                {
                    m_viewStruct = new List<ViewStruct>();
                }

                return m_viewStruct;

            }

            set { m_viewStruct = value; }
        }


        


    }
}
