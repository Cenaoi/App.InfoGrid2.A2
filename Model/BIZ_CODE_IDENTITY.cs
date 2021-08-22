using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 编码递增管理
    /// </summary>
    [DBTable("BIZ_CODE_IDENTITY")]
    [Description("编码递增管理")]
    [Serializable]
    public class BIZ_CODE_IDENTITY : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_CODE_IDENTITY_ID { get; set; }


        /// <summary>
        /// 编码号
        /// </summary>
        [DBField]
        [DisplayName("编码号")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String T_CODE { get; set; }

        /// <summary>
        /// 编码模式
        /// </summary>
        [DBField]
        [DisplayName("编码模式")]
        [UIField(visible = true)]
        public Int32 CODE_MODE_ID { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [DBField]
        [DisplayName("年份")]
        [UIField(visible = true)]
        public Int32 C_YEAR { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [DBField]
        [DisplayName("月份")]
        [UIField(visible = true)]
        public Int32 C_MONTH { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [DBField]
        [DisplayName("日期")]
        [UIField(visible = true)]
        public Int32 C_DAY { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        [DBField]
        [DisplayName("当前值")]
        [UIField(visible = true)]
        public Int32 NUM_CUR { get; set; }

    }
}
