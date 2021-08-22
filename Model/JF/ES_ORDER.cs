using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.JF
{
    /// <summary>
    /// 订单头
    /// </summary>
    [DBTable("ES_ORDER")]
    [Description("订单头")]
    [Serializable]
    public class ES_ORDER : LightModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("主键")]
        [UIField(visible = true)]
        public Int32 ES_ORDER_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_O_CODE { get; set; }


        /// <summary>
        /// 外键，微信用户自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，微信用户自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_W_CODE { get; set; }


        /// <summary>
        /// 微信唯一标识码
        /// </summary>
        [DBField]
        [DisplayName("微信唯一标识码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_OPENID { get; set; }


        /// <summary>
        /// 订单号
        /// </summary>
        [DBField]
        [DisplayName("订单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_CODE { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        [DBField]
        [DisplayName("付款状态")]
        [UIField(visible = true)]
        public Int32 PAY_SID { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        [DBField]
        [DisplayName("付款时间")]
        [UIField(visible = true)]
        public DateTime? PAY_DATE { get; set; }


        /// <summary>
        /// 订单说明
        /// </summary>
        [DBField]
        [DisplayName("订单说明")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String ORDER_INTRO { get; set; }


        /// <summary>
        /// 币别（人民币）
        /// </summary>
        [DBField]
        [DisplayName("币别（人民币）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CURRENCY_ID { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [DBField]
        [DisplayName("商品数量")]
        [UIField(visible = true)]
        public Int32 GOOD_NUM { get; set; }

        /// <summary>
        /// 产品金额
        /// </summary>
        [DBField]
        [DisplayName("产品金额")]
        [UIField(visible = true)]
        public Decimal MONEY_GOODS { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        [DBField]
        [DisplayName("合计金额")]
        [UIField(visible = true)]
        public Decimal MONEY_TOTAL { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        [DBField]
        [DisplayName("联系人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTACTER_NAME { get; set; }


        /// <summary>
        /// 用户手机号码
        /// </summary>
        [DBField]
        [DisplayName("用户手机号码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_PHONE { get; set; }


        /// <summary>
        /// 联系地址(省城区)
        /// </summary>
        [DBField]
        [DisplayName("联系地址(省城区)")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ADDRESS_T1 { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [DBField]
        [DisplayName("详细地址")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String ADDRESS_T2 { get; set; }


        /// <summary>
        /// 邮政编码
        /// </summary>
        [DBField]
        [DisplayName("邮政编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ZIP_CODE { get; set; }


        /// <summary>
        /// 用户邮箱
        /// </summary>
        [DBField]
        [DisplayName("用户邮箱")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_EMAIL { get; set; }


        /// <summary>
        /// 付款类型(支付宝)
        /// </summary>
        [DBField]
        [DisplayName("付款类型(支付宝)")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PAYMENT_TYPE_TEXT { get; set; }


        /// <summary>
        /// 订单备注
        /// </summary>
        [DBField]
        [DisplayName("订单备注")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String ORDER_REMART { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DBField]
        [DisplayName("处理时间")]
        [UIField(visible = true)]
        public DateTime? OP_TIME { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [DBField]
        [DisplayName("完成时间")]
        [UIField(visible = true)]
        public DateTime? END_TIME { get; set; }


        /// <summary>
        /// 订单名称
        /// </summary>
        [DBField]
        [DisplayName("订单名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_NAME { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        [DBField]
        [DisplayName("发货状态")]
        [UIField(visible = true)]
        public Int32 DEL_SID { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [DBField]
        [DisplayName("发货时间")]
        [UIField(visible = true)]
        public DateTime? DEL_DATE { get; set; }


        /// <summary>
        /// 快递单号
        /// </summary>
        [DBField]
        [DisplayName("快递单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EXPRESS_NO { get; set; }

        /// <summary>
        /// 收货状态
        /// </summary>
        [DBField]
        [DisplayName("收货状态")]
        [UIField(visible = true)]
        public Int32 REC_SID { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        [DBField]
        [DisplayName("收货时间")]
        [UIField(visible = true)]
        public DateTime? REC_DATE { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [DisplayName("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建日期
        /// </summary>
        [DBField]
        [DisplayName("记录创建日期")]
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
        /// 记录用户自定义排序
        /// </summary>
        [DBField]
        [DisplayName("记录用户自定义排序")]
        [UIField(visible = true)]
        public Decimal ROW_USER_SEQ { get; set; }


        /// <summary>
        /// 业务所结构编码
        /// </summary>
        [DBField]
        [DisplayName("业务所结构编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BIZ_CATA_CODE { get; set; }


        /// <summary>
        /// 微信统一下单的JSON数据
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("微信统一下单的JSON数据")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String W_UNIFIED_ORDER_JSON { get; set; }


        /// <summary>
        /// 微信支付流水号（系统生成的）
        /// </summary>
        [DBField]
        [DisplayName("微信支付流水号（系统生成的）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_P_SERIAL_CODE { get; set; }


        /// <summary>
        /// 微信支付成功订单号
        /// </summary>
        [DBField]
        [DisplayName("微信支付成功订单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_P_ORDER_NO { get; set; }

        /// <summary>
        /// 是否显示，用来给用户界面删除的
        /// </summary>
        [DBField]
        [DisplayName("是否显示，用来给用户界面删除的")]
        [UIField(visible = true)]
        public Boolean IS_VISIBLE { get; set; }

        /// <summary>
        /// 分销状态
        /// </summary>
        [DBField]
        [DisplayName("分销状态")]
        [UIField(visible = true)]
        public Int32 DISTR_SID { get; set; }

        /// <summary>
        /// 分销时间
        /// </summary>
        [DBField]
        [DisplayName("分销时间")]
        [UIField(visible = true)]
        public DateTime? DISTR_DATE { get; set; }

    }
}
