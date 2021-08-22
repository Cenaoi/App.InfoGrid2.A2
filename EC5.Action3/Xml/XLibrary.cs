using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    [Serializable]
    [XmlRoot("library")]
    public class XLibrary
    {
        /// <summary>
        /// 库编码
        /// </summary>
        [XmlAttribute("code")]
        public string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("remark")]
        public string Remark { get; set; }


        List<XPanel> m_Panels = new List<XPanel>();


        [XmlArray("penels")]
        [XmlArrayItem("panel")]
        public List<XPanel> Panels
        {
            get
            {
                if(m_Panels == null)
                {
                    m_Panels = new List<XPanel>();
                }
                return m_Panels;
            }
            set { m_Panels = value; }
        }

    }

    
}
