using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    /// <summary>
    /// 条件脚本
    /// </summary>
    public class XScript
    {
        /// <summary>
        /// 脚本语言
        /// </summary>
        [XmlAttribute("lang")]
        public string Lang { get; set; } = "json";

        [XmlElement("content")]
        [XmlText]
        public string Content { get; set; }


    }
}
