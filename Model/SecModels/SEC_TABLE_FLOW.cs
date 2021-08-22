using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限2-表流程
    /// </summary>
    [DBTable("SEC_TABLE_FLOW")]
    [Description("权限2-表流程")]
    [Serializable]
    public class SEC_TABLE_FLOW : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_TABLE_FLOW_ID { get; set; }


        /// <summary>
        /// 主键编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_SEC_TF_CODE { get; set; }


        /// <summary>
        /// 页面id
        /// </summary>
        [DBField]
        public int PAGE_ID { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("数据表名")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String DB_TABLE { get; set; }


        /// <summary>
        /// 流程名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("流程名")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FLOW_CODE { get; set; }

        /// <summary>
        /// 流程名
        /// </summary>
        [DBField]
        public string FLOW_TEXT { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_HEADER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_FOOTER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_1 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_2 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_3 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_4 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_5 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_6 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_7 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_8 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_9 { get; set; }



        
        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        #region 记录常规字段

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

        #endregion
    }
}
