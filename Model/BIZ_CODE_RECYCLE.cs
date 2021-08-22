using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 编码回收
    /// </summary>
    [DBTable("BIZ_CODE_RECYCLE")]
    [Description("编码回收")]
    [Serializable]
    public class BIZ_CODE_RECYCLE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_CODE_RECYCLE_ID { get; set; }


        /// <summary>
        /// 编码
        /// </summary>
        [DBField]
        [DisplayName("编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String T_CODE { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        [DBField]
        [DisplayName("模式")]
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


        /// <summary>
        /// 已经产生的编码
        /// </summary>
        [DBField]
        [DisplayName("已经产生的编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_CODE { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }

}
