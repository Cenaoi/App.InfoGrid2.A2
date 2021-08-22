using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 业务-核心-公司信息
    /// </summary>
    [DBTable("BIZ_C_COMPANY")]
    [Description("业务-核心-公司信息")]
    [Serializable]
    public class BIZ_C_COMPANY : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_C_COMPANY_ID { get; set; }


        /// <summary>
        /// 代码
        /// </summary>
        [DBField]
        [DisplayName("代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CODE { get; set; }


        /// <summary>
        /// 简称
        /// </summary>
        [DBField]
        [DisplayName("简称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SHORT_NAME { get; set; }


        /// <summary>
        /// 全称
        /// </summary>
        [DBField]
        [DisplayName("全称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FULL_NAME { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        [DBField]
        [DisplayName("地址")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String ADDRSSS { get; set; }


        /// <summary>
        /// 邮编
        /// </summary>
        [DBField]
        [DisplayName("邮编")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PORTCODE { get; set; }


        /// <summary>
        /// 电话
        /// </summary>
        [DBField]
        [DisplayName("电话")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TEL { get; set; }


        /// <summary>
        /// 传真
        /// </summary>
        [DBField]
        [DisplayName("传真")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FAX { get; set; }


        /// <summary>
        /// 公司主页
        /// </summary>
        [DBField]
        [DisplayName("公司主页")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String WEBSITE { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        [DBField]
        [DisplayName("邮箱")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EMAIL { get; set; }


        /// <summary>
        /// 开户银行
        /// </summary>
        [DBField]
        [DisplayName("开户银行")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BANK_NAME { get; set; }


        /// <summary>
        /// 银行账号
        /// </summary>
        [DBField]
        [DisplayName("银行账号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BANK_ACCOUNT { get; set; }


        /// <summary>
        /// 税号
        /// </summary>
        [DBField]
        [DisplayName("税号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CORPORATION_TAX { get; set; }

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
        [DBField]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField]
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

    }
}
