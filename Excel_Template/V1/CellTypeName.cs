using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 单元格类型的枚举
    /// </summary>
    public enum CellTypeName
    {
        /// <summary>
        /// 这是字符串类型
        /// </summary>
        String,
        /// <summary>
        /// 这是数字类型
        /// </summary>
        Double,
        /// <summary>
        /// 这是时间类型
        /// </summary>
        Date,
        /// <summary>
        /// 这是空类型
        /// </summary>
        Blank
    }
}
