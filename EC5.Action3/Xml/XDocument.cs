using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    /// <summary>
    /// 文档
    /// </summary>
    [Serializable]
    [XmlRoot("ac3")]
    public class XDocument
    {
        XLibrary m_Library;

        [XmlElement("lab")]
        public XLibrary Library
        {
            get
            {
                if(m_Library == null)
                {
                    m_Library = new XLibrary();
                }
                return m_Library;
            }
            set { m_Library = value; }
        }
    }

    
}
