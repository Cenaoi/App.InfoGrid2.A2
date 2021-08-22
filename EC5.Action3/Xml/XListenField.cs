using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    public class XListenField
    {
        /// <summary>
        /// 对应的表名
        /// </summary>
        [XmlAttribute("table")]
        public string Table { get; set; }

        /// <summary>
        /// 对应的表名称
        /// </summary>
        [XmlAttribute("table-text")]
        public string TableText { get; set; }


        ///// <summary>
        ///// 字段名
        ///// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [XmlAttribute("text")]
        public string Text { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 监听值变化前
        /// </summary>
        [XmlAttribute("from-value")]
        public string FromValue { get; set; }

        /// <summary>
        /// 监听值变化后
        /// </summary>
        [XmlAttribute("to-value")]
        public string ToValue { get; set; }
    }
}
