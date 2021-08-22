using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限-可操作的人员信息
    /// </summary>
    [DBTable("SEC_ARR_ACCOUNT")]
    [Description("权限-可操作的人员信息")]
    [Serializable]
    public class SEC_ARR_ACCOUNT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_ARR_ACCOUNT_ID { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        [DBField]
        [DisplayName("用户ID")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_LOGIN_CODE { get; set; }


        /// <summary>
        /// 可操作的用户ID
        /// </summary>
        [DBField]
        [DisplayName("可操作的用户ID")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String REF_CODE { get; set; }


        /// <summary>
        /// 用户名称
        /// </summary>
        [DBField]
        [DisplayName("用户名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REF_TEXT { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        [DBField]
        [DisplayName("激活")]
        [UIField(visible = true)]
        public Boolean ENABLED { get; set; }

        /// <summary>
        /// 子公司代码
        /// </summary>
        [DBField]
        public string ARR_COMP_CODE { get; set; }

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

    }
}
