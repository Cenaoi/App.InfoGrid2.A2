using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.BizCoder
{
    /// <summary>
    /// 编码模式
    /// </summary>
    public enum BizCodeMode:byte
    {
        /// <summary>
        /// 自动，无限制
        /// </summary>
        Auto = 0,

        /// <summary>
        /// 根据年份
        /// </summary>
        Year = 1,

        /// <summary>
        /// 根据月份
        /// </summary>
        Month = 2,

        /// <summary>
        /// 根据日期
        /// </summary>
        Day = 3,

        /// <summary>
        /// 根据季度
        /// </summary>
        Quarter = 4,
        
        /// <summary>
        /// 根据周
        /// </summary>
        Week = 5

    }

}
