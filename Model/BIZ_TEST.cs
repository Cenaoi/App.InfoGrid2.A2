using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 测试
    /// </summary>
    [DBTable("BIZ_TEST")]
    [Description("测试")]
    [Serializable]
    public class BIZ_TEST : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField,DBIdentity,DBKey]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_TEST_ID { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        [DBField]
        [DisplayName("数值")]
        [UIField(visible = true)]
        public Decimal NUM_1 { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [DBField]
        [DisplayName("日期")]
        [UIField(visible = true)]
        public DateTime? DATE_1 { get; set; }


        /// <summary>
        /// 字符串
        /// </summary>
        [DBField]
        [DisplayName("字符串")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STR_1 { get; set; }

    }
}
