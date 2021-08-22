using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.Steps
{


    /// <summary>
    /// 步骤区域块
    /// </summary>
    public class CodeRegion : TreeNode
    {

        /// <summary>
        /// 已经执行过
        /// </summary>
        public bool IsPass { get; set; }

        public StepStatus Status { get; set; } = StepStatus.None;

        public virtual bool Read()
        {
            return false;
        }
    }
}
