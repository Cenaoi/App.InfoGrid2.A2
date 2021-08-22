using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    public class XActionItem
    {
        [XmlAttribute("code")]
        public string Code { get; set; }

        /// <summary>
        /// 自动继续
        /// </summary>
        [DefaultValue(true)]
        [XmlAttribute("auto-continue")]
        public bool AutoContinue { get; set; } = true;

    }
}
