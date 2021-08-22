using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 行的状态。
    /// </summary>
    [Flags]
    public enum MiniDataControlRowState
    {
        /// <summary>
        /// 指示该数据控件行处于正常状态。
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 指示该数据控件行是交替行。
        /// </summary>
        Alternate = 1,
        /// <summary>
        /// 指示该行已被用户选定。
        /// </summary>
        Selected = 2,
        /// <summary>
        /// 指示该行处于编辑状态，这通常是单击行的“编辑”按钮的结果。
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 指示该行是新行，这通常是单击“插入”按钮添加新行的结果。
        /// </summary>
        Insert = 8,
    }
}
