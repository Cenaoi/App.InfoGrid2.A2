using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{
    /// <summary>
    /// 值模式
    /// </summary>
    public enum DbccValueModes
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL,
        /// <summary>
        /// 固定值
        /// </summary>
        Fixed,
        /// <summary>
        /// 函数返回值
        /// </summary>
        Fun,
        /// <summary>
        /// 表和字段
        /// </summary>
        Table,

        /// <summary>
        /// 自定义公式
        /// </summary>
        User_Func
    }
}
