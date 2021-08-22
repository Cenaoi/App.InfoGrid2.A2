using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 联动v3-图纸
    /// </summary>
    [DBTable("AC3_DWG")]
    [Description("联动v3-图纸")]
    [Serializable]
    public class AC3_DWG : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 AC3_DWG_ID { get; set; }


        /// <summary>
        /// 图纸编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("图纸编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_DWG_CODE { get; set; }


        /// <summary>
        /// 图纸名称
        /// </summary>
        [DBField]
        [DisplayName("图纸名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DWG_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 版本号
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("版本号")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String V_VERSION { get; set; }


        /// <summary>
        /// 作者编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("作者编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String AUTHOR_CODE { get; set; }


        /// <summary>
        /// 作者名称
        /// </summary>
        [DBField]
        [DisplayName("作者名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String AUTHOR_TEXT { get; set; }


        /// <summary>
        /// 图形
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("图形")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String GRAPHICS { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
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


        [DBField]
        public int N_NODE_IDENTITY { get; set; }


        [DBField]
        public int N_LINE_IDENTITY { get; set; }

    }
}
