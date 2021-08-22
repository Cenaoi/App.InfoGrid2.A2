using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 自动模式
        /// </summary>
        Auto,
        /// <summary>
        /// 数值
        /// </summary>
        Number,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 日期+时间
        /// </summary>
        DateTime,
        /// <summary>
        /// 时间
        /// </summary>
        Time,
        /// <summary>
        /// 布尔值
        /// </summary>
        Bool,
        /// <summary>
        /// Guid 值
        /// </summary>
        Guid
    }
}
