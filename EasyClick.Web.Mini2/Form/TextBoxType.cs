using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 文本框类型
    /// </summary>
    public enum TextBoxType
    {
        /// <summary>
        /// 默认普通文本框
        /// </summary>
        Text,
        /// <summary>
        /// 密码框
        /// </summary>
        Password,

        /// <summary>
        /// 数字框
        /// </summary>
        Number,

        Email,

        Url,

        Range,

        /// <summary>
        /// 电话号码
        /// </summary>
        Tel,

        /// <summary>
        /// 颜色框
        /// </summary>
        Color
    }
}
