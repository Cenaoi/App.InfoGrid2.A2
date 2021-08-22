using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Model.XmlModel
{

    [Serializable]
    [XmlRoot("TableJoinConfig")]
    public class TableJoinConfig
    {
        public string join_table { get; set; }

        List<join_item> m_items;


        /// <summary>
        /// 
        /// </summary>
        public List<join_item> items
        {
            get
            {
                if (m_items == null)
                {
                    m_items = new List<join_item>();
                }
                return m_items;
            }
            set { m_items = value; }
        }


    }



    [Serializable]
    public class join_item
    {
        [XmlAttribute]
        [DefaultValue("")]
        public string field { get; set; }

        [XmlAttribute]
        [DefaultValue("")]
        public string field_text { get; set; }


        [XmlAttribute]
        [DefaultValue("")]
        public string join_field { get; set; }

        [XmlAttribute]
        [DefaultValue("")]
        public string join_field_text { get; set; }
    }
}
