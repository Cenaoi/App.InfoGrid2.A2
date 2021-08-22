using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 步骤状态
    /// </summary>
    public enum StepStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        None,

        /// <summary>
        /// 运行中
        /// </summary>
        Running,

        /// <summary>
        /// 已经结束
        /// </summary>
        End

    }
}
