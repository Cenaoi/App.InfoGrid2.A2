
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 登陆账号
    /// </summary>
    [DBTable("SEC_LOGIN_ACCOUNT")]
    [Description("登陆账号")]
    [Serializable]
    public class SEC_LOGIN_ACCOUNT : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 SEC_LOGIN_ACCOUNT_ID { get; set; }


        /// <summary>
        /// 用户编码（配合业务使用）
        /// </summary>
        [DBField]
        [DisplayName("用户编码（配合业务使用）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BIZ_USER_CODE { get; set; }


        /// <summary>
        /// 组织机构编码（配合业务使用）
        /// </summary>
        [DBField]
        [DisplayName("组织机构编码（配合业务使用）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BIZ_ORG_CODE { get; set; }




        /// <summary>
        /// 真实名称
        /// </summary>
        [DBField]
        [DisplayName("真实名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TRUE_NAME { get; set; }


        /// <summary>
        /// 登录账号
        /// </summary>
        [DBField]
        [DisplayName("登录账号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LOGIN_NAME { get; set; }


        /// <summary>
        /// 登录密码
        /// </summary>
        [DBField]
        [DisplayName("登录密码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LOGIN_PASS { get; set; }


        /// <summary>
        /// 角色ID
        /// </summary>
        [DBField]
        [DisplayName("角色ID")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String ARR_ROLE_ID { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [DBField]
        [DisplayName("角色名称")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String ARR_ROLE_NAME { get; set; }

        /// <summary>
        /// 公司代码
        /// </summary>
        [DBField]
        public string ARR_COMP_CODE { get; set; }

        /// <summary>
        /// 系统行
        /// </summary>
        [DBField]
        [DisplayName("系统行")]
        [UIField(visible = true)]
        public Boolean SYS_FIXED { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ROW_STR_PK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 ROW_INT_PK { get; set; }


        /// <summary>
        /// 用户标签
        /// </summary>
        [DBField]
        [DisplayName("用户标签")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_TAG { get; set; }

        /// <summary>
        /// 安全模式,0-空白 ,1-角色,2-独立权限,3-角色和独立权限
        /// </summary>
        [DBField]
        [DisplayName("安全模式,0-空白 ,1-角色,2-独立权限,3-角色和独立权限")]
        public int SEC_MODE_ID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        public string REMARK { get; set; }

        /// <summary>
        /// ARR 权限控制模式.0-没有，1-角色，2-用户
        /// </summary>
        [DBField]
        public int ARR_MODE_ID { get; set; }

        /// <summary>
        /// 可管理的下级人员编码
        /// </summary>
        [DBField]
        public string REF_ARR_USER_CODE { get; set; }

        /// <summary>
        /// 可管理的角色编码
        /// </summary>
        [DBField]
        public string REF_ARR_ROLE_CODE { get; set; }

        /// <summary>
        /// 权限结构集
        /// </summary>
        [DBField]
        public string REF_ARR_STRUCT_CODE { get; set; } 


        /// <summary>
        /// 是否采用自定义权限
        /// </summary>
        [VField]
        public bool IS_USER_SEC { get; set; }


        #region 数据库记录信息

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [DisplayName("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_STATUS_ID { get; set; }

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

        #endregion


        /// <summary>
        /// 微信用户主键编码
        /// </summary>
        [DBField]
        [DisplayName("微信用户主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_CODE { get; set; }



    }

}