using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 字段值类型
    /// </summary>
    public enum RFieldValueMode
    {
        None,

        /// <summary>
        /// 从数据库读取
        /// </summary>
        DBValue,
        /// <summary>
        /// 代码计算出结果
        /// </summary>
        Code,
        /// <summary>
        /// 用户设定的固定值
        /// </summary>
        FixedValue,

        /// <summary>
        /// 内部代码(一个报表,只能存在一个),
        /// 
        /// </summary>
        InnerCode
    }
}
