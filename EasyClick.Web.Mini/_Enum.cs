using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 单元格横排序
    /// </summary>
    public enum CellAlign
    {
        Left,
        Right,
        Center
    }

    /// <summary>
    /// 排序模式
    /// </summary>
    public enum SortMode
    {
        /// <summary>
        /// 没有
        /// </summary>
        None = 0,
        /// <summary>
        /// 默认模式
        /// </summary>
        Default,
        /// <summary>
        /// 自定义
        /// </summary>
        User
    }

    /// <summary>
    /// 指定哪些滚动条在控件上可见。
    /// </summary>
    public enum ScrollBars
    {
        /// <summary>
        /// 不显示任何滚动条。
        /// </summary>
        None = 0,
        /// <summary>
        /// 自动模式
        /// </summary>
        Auto = 1,
        /// <summary>
        /// 只显示水平滚动条。
        /// </summary>
        Horizontal = 2,
        /// <summary>
        /// 只显示垂直滚动条。
        /// </summary>
        Vertical = 3,
        /// <summary>
        /// 同时显示水平滚动条和垂直滚动条。
        /// </summary>
        Both = 4,
    }

    /// <summary>
    /// 记录展开的模板类型
    /// </summary>
    public enum DataGridRowExpandTemplate
    {
        /// <summary>
        /// 没有
        /// </summary>
        None,
        /// <summary>
        /// 字符串模板
        /// </summary>
        StringTemplate,
        /// <summary>
        /// 控件模板
        /// </summary>
        ControlTemplate
    }

    /// <summary>
    /// 记录展开的模式
    /// </summary>
    public enum DataGridRowExpandMode
    {
        /// <summary>
        /// 用户点击
        /// </summary>
        User,
        /// <summary>
        /// 全部展开
        /// </summary>
        Expand,
        /// <summary>
        /// 闭合
        /// </summary>
        Closed
    }


    public enum DataStoreRowState
    {
        // 摘要:
        //     该行已被创建，但不属于任何 System.Data.DataRowCollection。
        //     System.Data.DataRow 在以下情况下立即处于此状态：创建之后添加到集合中之前；
        //     或从集合中移除之后。
        Detached = 1,
        //
        // 摘要:
        //     该行自上次调用 System.Data.DataRow.AcceptChanges() 以来尚未更改。
        Unchanged = 2,
        //
        // 摘要:
        //     该行已添加到 System.Data.DataRowCollection 中，System.Data.DataRow.AcceptChanges()
        //     尚未调用。
        Added = 4,
        //
        // 摘要:
        //     该行已通过 System.Data.DataRow 的 System.Data.DataRow.Delete() 方法被删除。
        Deleted = 8,
        //
        // 摘要:
        //     该行已被修改，System.Data.DataRow.AcceptChanges() 尚未调用。
        Modified = 16
    }
}
