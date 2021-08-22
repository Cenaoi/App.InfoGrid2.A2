using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 栏目的固定值
    /// </summary>
    public class ItemFixedValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 值的数据类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public OperatorTypes Operator { get; set; }

        /// <summary>
        /// 激活统计,默认 false
        /// </summary>
        public bool EnabledTotal { get; set; }

        /// <summary>
        /// 值模式
        /// </summary>
        public RFieldValueMode ValueMode { get; internal set; }

        /// <summary>
        /// 脚本编码
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// 列的宽度
        /// </summary>
        public int Width { get; internal set; }
        public string Format { get; internal set; }
        public string FormatType { get; internal set; }
    }
}
