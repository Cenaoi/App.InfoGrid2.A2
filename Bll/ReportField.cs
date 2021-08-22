using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Bll
{
    public class ReportField
    {
        /// <summary>
        /// 值
        /// </summary>
        [XmlElementAttribute("value")]
        public string  value { get; set; }

        /// <summary>
        /// 文本值
        /// </summary>
        [XmlElementAttribute("text")]
        public string text { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [XmlElementAttribute("type")]
        public string type { get; set; }

        /// <summary>
        /// 运算符
        /// </summary>
        [XmlElementAttribute("operators")]
        public string operators { get; set; }

    }
}
