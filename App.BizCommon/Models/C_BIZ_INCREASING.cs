using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.BizCommon.Models
{
    /// <summary>
    /// 递增
    /// </summary>
    [DBTable("C_BIZ_INCREASING")]
    public class C_BIZ_INCREASING
    {
        [DBField,DBKey]
        public string C_BIZ_INCREASING_ID { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        [DBField,LMOnlyRead]
        public int CUR_NUM { get; set; }

        /// <summary>
        /// 默认值,起始值
        /// </summary>
        [DBField,LMOnlyRead,DefaultValue(1)]
        public int DEFAULT_NUM { get; set; }


    }
}
