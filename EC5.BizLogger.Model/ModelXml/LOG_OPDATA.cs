using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EC5.BizLogger.ModelXml
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Serializable]
    public class LOG_OPDATA : List<LOG_SET>
    {
        public LOG_OPDATA()
        {
        }

        public LOG_OPDATA(int capacity)
            : base(capacity)
        {

        }


    }

    [Serializable]
    public class LOG_SET
    {
        /// <summary>
        /// 操作
        /// </summary>
        [XmlAttribute]
        public string OP { get; set; }

        /// <summary>
        /// 操作 ID
        /// </summary>
        [XmlAttribute]
        public string OP_ID { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [XmlAttribute]
        public string FIELD { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [XmlAttribute]
        public string DISPLAY { get; set; }

        /// <summary>
        /// 原值
        /// </summary>
        [XmlAttribute]
        public string SRC_VALUE { get; set; }

        /// <summary>
        /// 改变后
        /// </summary>
        [XmlAttribute]
        public string TAR_VALUE { get; set; }
    }
}
