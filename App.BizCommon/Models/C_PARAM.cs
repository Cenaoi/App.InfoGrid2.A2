using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.BizCommon.Models
{
    /// <summary>
    /// 参数
    /// </summary>
    [DBTable("C_PARAM")]
    [Description("参数")]
    [Serializable]
    public class C_PARAM : LightModel
    {


        /// <summary>
        /// 参数名称
        /// </summary>
        [DBField, DBKey]
        [DisplayName("参数名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARAM_NAME { get; set; }


        /// <summary>
        /// 参数值
        /// </summary>
        [DBField]
        [DisplayName("参数值")]
        [LMValidate(maxLen = 255)]
        [UIField(visible = true)]
        public String PARAM_VALEU { get; set; }


        /// <summary>
        /// 参数类型
        /// </summary>
        [DBField]
        [DisplayName("参数类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PARAM_TYPE { get; set; }

        /// <summary>
        /// 数据更新时间
        /// </summary>
        [DBField]
        [DisplayName("数据更新时间")]
        [UIField(visible = true)]
        public DateTime DATE_UPDATE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DESCRIPTION { get; set; }


        /// <summary>
        /// 是否为数组(Y/N)
        /// </summary>
        [DBField]
        [DisplayName("是否为数组(Y/N)")]
        [LMValidate(maxLen = 1)]
        [UIField(visible = true)]
        public String IS_ARRAY { get; set; }

    }
}
