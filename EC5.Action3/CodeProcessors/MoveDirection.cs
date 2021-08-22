using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 移动方向
    /// </summary>
    public enum MoveDirection
    {
        /// <summary>
        /// 没有移动
        /// </summary>
        None,

        /// <summary>
        /// 下一节点
        /// </summary>
        Next,

        /// <summary>
        /// 子节点
        /// </summary>
        Child,

        /// <summary>
        /// 退回
        /// </summary>
        Back
    }
}
