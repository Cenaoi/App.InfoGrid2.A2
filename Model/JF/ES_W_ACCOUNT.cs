using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.JF
{
    /// <summary>
    /// 微信用户
    /// </summary>
    [DBTable("ES_W_ACCOUNT")]
    [Description("微信用户")]
    [Serializable]
    public class ES_W_ACCOUNT : LightModel
    {

        /// <summary>
        /// #
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("#")]
        [UIField(visible = true)]
        public Int32 ES_W_ACCOUNT_ID { get; set; }


        /// <summary>
        /// 自定义主键
        /// </summary>
        [DBField]
        [DisplayName("自定义主键")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_W_CODE { get; set; }


        /// <summary>
        /// 微信昵称
        /// </summary>
        [DBField]
        [DisplayName("微信昵称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_NICKNAME { get; set; }


        /// <summary>
        /// 微信唯一标示码
        /// </summary>
        [DBField]
        [DisplayName("微信唯一标示码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_OPENID { get; set; }


        /// <summary>
        /// 微信头像地址
        /// </summary>
        [DBField]
        [DisplayName("微信头像地址")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String HEAD_IMG_URL { get; set; }


        /// <summary>
        /// 微信里面填写的地址
        /// </summary>
        [DBField]
        [DisplayName("微信里面填写的地址")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String W_ADDRESS { get; set; }

        /// <summary>
        /// 是否激活（默认激活）
        /// </summary>
        [DBField]
        [DisplayName("是否激活（默认激活）")]
        [UIField(visible = true)]
        public Boolean IS_ENABLE { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        [DBField]
        [DisplayName("性别")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String SEX { get; set; }

        /// <summary>
        /// 这是GUID随机码，验证用的
        /// </summary>
        [DBField]
        [DisplayName("这是GUID随机码，验证用的")]
        [UIField(visible = true)]
        public Guid W_GUID { get; set; }

        /// <summary>
        /// GUID的过期时间
        /// </summary>
        [DBField]
        [DisplayName("GUID的过期时间")]
        [UIField(visible = true)]
        public DateTime? GUID_LIMIT_DATE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARKS { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField]
        [DisplayName("创建时间")]
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


        /// <summary>
        /// 上级编码
        /// </summary>
        [DBField]
        [DisplayName("上级编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARENT_CODE { get; set; }


        /// <summary>
        /// 上上级编码
        /// </summary>
        [DBField]
        [DisplayName("上上级编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARENT_CODE_2 { get; set; }


        /// <summary>
        /// 上上上级编码
        /// </summary>
        [DBField]
        [DisplayName("上上上级编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARENT_CODE_3 { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        [DBField]
        [DisplayName("联系人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTACTER_NAME { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        [DBField]
        [DisplayName("联系电话")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTACTER_TEL { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [DBField]
        [DisplayName("详细地址")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String ADDRESS_T2 { get; set; }

        /// <summary>
        /// 是否分销
        /// </summary>
        [DBField]
        [DisplayName("是否分销")]
        [UIField(visible = true)]
        public Boolean IS_DISTR { get; set; }


        /// <summary>
        /// 专属二维码图片地址
        /// </summary>
        [DBField]
        [DisplayName("专属二维码图片地址")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String QR_CODE_IMG_URL { get; set; }

        /// <summary>
        /// 代理佣金
        /// </summary>
        [DBField]
        [DisplayName("代理佣金")]
        [UIField(visible = true)]
        public Decimal AGENT_MONEY { get; set; }

        /// <summary>
        /// 计划提现代理佣金
        /// </summary>
        [DBField]
        [DisplayName("计划提现代理佣金")]
        [UIField(visible = true)]
        public Decimal PLAN_WD_MONEY { get; set; }

        /// <summary>
        /// 已提现的代理佣金
        /// </summary>
        [DBField]
        [DisplayName("已提现的代理佣金")]
        [UIField(visible = true)]
        public Decimal WD_AGENT_MONEY { get; set; }

    }
}
