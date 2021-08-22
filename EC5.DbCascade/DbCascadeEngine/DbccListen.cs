using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{

    /// <summary>
    /// 监控
    /// </summary>
    public class DbccListen
    {
        /// <summary>
        /// 监控字段名称
        /// </summary>
        public string DBField { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string FieldText { get; set; }


        /// <summary>
        /// 起始值
        /// </summary>
        public string ValueFrom { get; set; }

        /// <summary>
        /// 结束值
        /// </summary>
        public string ValueTo { get; set; }

    }
}
