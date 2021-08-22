using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 批量操作
    /// </summary>
    [DBTable("IG2_BATCH_TABLE_OPERATE")]
    [Description("批量操作")]
    [Serializable]
    public class IG2_BATCH_TABLE_OPERATE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_BATCH_TABLE_OPERATE_ID { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        [DBField]
        [DisplayName("名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TBO_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 反馈
        /// </summary>
        [DBField]
        [DisplayName("反馈")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String RESULT_TEXT { get; set; }

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
