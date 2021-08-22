using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade
{

    /// <summary>
    /// 步骤类型
    /// </summary>
    public enum BizDbStepType
    {
        /// <summary>
        /// 没有任何操作
        /// </summary>
        NONE,

        /// <summary>
        /// 删除操作
        /// </summary>
        DELETE,

        /// <summary>
        /// 更新操作
        /// </summary>
        UPDATE,

        /// <summary>
        /// 插入操作
        /// </summary>
        INSERT

    }
}
