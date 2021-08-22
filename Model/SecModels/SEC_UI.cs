using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限-界面UI
    /// </summary>
    [DBTable("SEC_UI")]
    [Description("权限-界面UI")]
    [Serializable]
    public class SEC_UI : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 SEC_UI_ID { get; set; }

        /// <summary>
        /// 权限模式。0-没有权限，1-角色，2-用户
        /// </summary>
        [DBField]
        [DisplayName("权限模式。0-没有权限，1-角色，2-用户")]
        [UIField(visible = true)]
        public Int32 SEC_MODE_ID { get; set; }

        /// <summary>
        /// 页面ID
        /// </summary>
        [DBField]
        [DisplayName("页面ID")]
        public int UI_PAGE_ID { get; set; }

        /// <summary>
        /// UI 类型.TABLE 表格（单表），PAGE 复杂表（多表模式）
        /// </summary>
        [DBField]
        [DisplayName("UI 类型.TABLE 表格（单表），PAGE 复杂表（多表模式）")]
        public string UI_TYPE_ID { get; set; }

        /// <summary>
        /// UI 子类型.ONE_FORM=PAGE 的表单模式
        /// </summary>
        [DBField]
        [DisplayName("UI 子类型.ONE_FORM=PAGE 的表单模式")]
        public string UI_SUB_TYPE_ID { get; set; }


        /// <summary>
        /// 菜单ID
        /// </summary>
        [DBField]
        [DisplayName("菜单ID")]
        public int MENU_ID { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        [DBField]
        [DisplayName("用户编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_USER_CODE { get; set; }


        /// <summary>
        /// 角色编码
        /// </summary>
        [DBField]
        [DisplayName("角色编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_ROLE_CODE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        #region 记录公共字段

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
        [DBField,DefaultValue("(GetDate())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField,DefaultValue("(GetDate())")]
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

        #endregion

    }

}
