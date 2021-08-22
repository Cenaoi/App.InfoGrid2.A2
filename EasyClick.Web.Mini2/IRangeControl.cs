using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 区间控件
    /// </summary>
    public interface IRangeControl
    {
        /// <summary>
        /// 开始值
        /// </summary>
        string StartValue { get; set; }

        /// <summary>
        /// 结束值
        /// </summary>
        string EndValue { get; set; }
    }
}
