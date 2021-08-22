using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 视图总表
    /// </summary>
    [DBTable("IG2_VIEW")]
    [Description("视图总表")]
    [Serializable]
    public class IG2_VIEW : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey]
        [DisplayName("ID")]
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
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
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

    }
}
