using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DBTable("IG2_VALUE_TABLE")]
    [Description("")]
    [Serializable]
    public class IG2_VALUE_TABLE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_TABLE_ID { get; set; }


        /// <summary>
        /// 表名--UT_001
        /// </summary>
        [DBField]
        [Description("表名--UT_001")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 表显示名
        /// </summary>
        [DBField]
        [Description("表显示名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_DISPAY { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [Description("备注")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        public String REMARKS { get; set; }

        /// <summary>
        /// 激活,布尔值
        /// </summary>
        [DBField]
        [Description("激活,布尔值")]
        [UIField(visible = true)]
        public Boolean ENABLED { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [Description("")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [Description("")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }
}
