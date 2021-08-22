using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DBTable("IG2_BATCH_TO_COL")]
    [Description("")]
    [Serializable]
    public class IG2_BATCH_TO_COL : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 IG2_BATCH_TO_COL_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 BATCH_TABLE_OPERATE_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_FIELD { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TYPE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_NAME { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String IS_REMARK { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DISPLAY { get; set; }

        /// <summary>
        /// 必填
        /// </summary>
        [DBField]
        public bool IS_MANDATORY { get; set; } = true;


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DEFAULT_VALUE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 SEC_LEVEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 DB_DOT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 DB_LEN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }
}
