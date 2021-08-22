using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 临时视图表
    /// </summary>
    [DBTable("IG2_TMP_VIEW")]
    [Description("临时视图表")]
    [Serializable]
    public class IG2_TMP_VIEW : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 IG2_TMP_VIEW_ID { get; set; }


        /// <summary>
        /// 真实视图表ID
        /// </summary>
        [DBField]
        [DisplayName("真实视图表ID")]
        [UIField(visible = true)]
        public Int32 IG2_VIEW_ID { get; set; }


        /// <summary>
        /// 显示视图名称
        /// </summary>
        [DBField]
        [DisplayName("显示视图名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DISPLAY { get; set; }


        /// <summary>
        /// 真实视图名称
        /// </summary>
        [DBField]
        [DisplayName("真实视图名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String VIEW_NAME { get; set; }


        /// <summary>
        /// 主键表
        /// </summary>
        [DBField]
        [DisplayName("主键表")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MAIN_TABLE_NAME { get; set; }


        /// <summary>
        /// 主键ID
        /// </summary>
        [DBField]
        [DisplayName("主键ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MAIN_ID_FIELD { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [DisplayName("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
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

        /// <summary>
        /// 记录用户自定义排序
        /// </summary>
        [DBField]
        [DisplayName("记录用户自定义排序")]
        [UIField(visible = true)]
        public Decimal ROW_USER_SEQ { get; set; }


        /// <summary>
        /// 记录创建角色代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建角色代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ROLE_CODE { get; set; }


        /// <summary>
        /// 记录创建公司代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建公司代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_COMP_CODE { get; set; }


        /// <summary>
        /// 记录创建部门代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建部门代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ORG_CODE { get; set; }


        /// <summary>
        /// 记录创建人员代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建人员代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_USER_CODE { get; set; }


        /// <summary>
        /// 记录-临时操作。A-新建，D-删除，E-修改
        /// </summary>
        [DBField]
        [DisplayName("记录-临时操作。A-新建，D-删除，E-修改")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TMP_OP_ID { get; set; }

        /// <summary>
        /// 记录-临时GUID
        /// </summary>
        [DBField]
        [DisplayName("记录-临时GUID")]
        [UIField(visible = true)]
        public Guid TMP_GUID { get; set; }

        /// <summary>
        /// 记录-操作时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
        [DisplayName("记录-操作时间")]
        [UIField(visible = true)]
        public DateTime TMP_OP_TIME { get; set; }

        [DBField]
        public string TMP_SESSION_ID { get; set; }

        

    }
}
