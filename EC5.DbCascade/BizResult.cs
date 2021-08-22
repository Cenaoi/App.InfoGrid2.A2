using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade
{
    public enum BizResult
    {
        /// <summary>
        /// 停止执行
        /// </summary>
        Stoped,


        /// <summary>
        /// 继续执行
        /// </summary>
        Resume,

        /// <summary>
        /// 下一环节开始 
        /// </summary>
        Continue
    }
}
