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
    [DBTable("IG2_VALUE_TRIGGER")]
    [Description("")]
    [Serializable]
    public class IG2_VALUE_TRIGGER : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [Description("")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_TRIGGER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_TABLE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [UIField(visible = true)]
        public Int32 IG2_VALUE_IF_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_NAME { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_DISPLAY { get; set; }

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
