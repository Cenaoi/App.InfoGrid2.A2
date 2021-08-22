using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 记录状态
    /// </summary>
    public enum DataRecordAction
    {
        /// <summary>
        /// 没有状态
        /// </summary>
        None,

        /// <summary>
        /// 新建的
        /// </summary>
        Added,
        /// <summary>
        /// 删除的数据
        /// </summary>
        Deleted,
        /// <summary>
        /// 修改
        /// </summary>
        Modified
    }
}
