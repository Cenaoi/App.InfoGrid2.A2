using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 登录账号扩展表
    /// </summary>
    [DBTable("SEC_LOGIN_ACCOUNT_EX")]
    [Description("登录账号扩展表")]
    [Serializable]
    public class SEC_LOGIN_ACCOUNT_EX : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 SEC_LOGIN_ACCOUNT_EX_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_ACCOUNT_EX_CODE { get; set; }


        /// <summary>
        /// 外键 登录用户表的用户编码
        /// </summary>
        [DBField]
        [DisplayName("外键 登录用户表的用户编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_ACCOUNT_CODE { get; set; }

        /// <summary>
        /// 自动登录用的GUID
        /// </summary>
        [DBField]
        [DisplayName("自动登录用的GUID")]
        [UIField(visible = true)]
        public Guid LOGIN_GUID { get; set; }

        /// <summary>
        /// 自动登录的Guid过期时间
        /// </summary>
        [DBField]
        [DisplayName("自动登录的Guid过期时间")]
        [UIField(visible = true)]
        public DateTime GUID_LIMIT_DATE { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [DBField]
        [DisplayName("登录名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LOGIN_NAME { get; set; }


    }
}
