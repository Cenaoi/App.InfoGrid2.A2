using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{

    /// <summary>
    /// 操作
    /// </summary>
    public enum OperatorTypes
    {
        /// <summary>
        /// 等于符号
        /// </summary>
        Equals,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 大于或等于
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 不等于
        /// </summary>
        Inequality,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// 小于或等于
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// 模糊查询
        /// </summary>
        Like,
        /// <summary>
        /// 左模糊查询
        /// </summary>
        LeftLike,
        /// <summary>
        /// 右模糊查询
        /// </summary>
        RightLike,
        /// <summary>
        /// 列表
        /// </summary>
        In,
        NotIn,
        NotLike
    }
}
