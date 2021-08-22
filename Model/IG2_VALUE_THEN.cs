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
    [DBTable("IG2_VALUE_THEN")]
    [Description("")]
    [Serializable]
    public class IG2_VALUE_THEN : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_THEN_ID { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        [DBField]
        [Description("字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_NAME { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [DBField]
        [Description("字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_DISPAY { get; set; }

        /// <summary>
        /// 逻辑
        /// </summary>
        [DBField]
        [Description("逻辑")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_LOGIC { get; set; }


        /// <summary>
        /// 值为
        /// </summary>
        [DBField]
        [Description("值为")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String VALUE_TO { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DBField]
        [Description("父ID")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_IF_ID { get; set; }

        /// <summary>
        /// 父父ID
        /// </summary>
        [DBField]
        [Description("父父ID")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_TABLE_ID { get; set; }

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
