using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
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
        /// 订单号
        /// </summary>
        [DBField]
        [DisplayName("订单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_NUM { get; set; }


        /// <summary>
        /// 币别（人民币）
        /// </summary>
        [DBField]
        [DisplayName("币别（人民币）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CURRENCY_ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DBField]
        [DisplayName("用户ID")]
        [UIField(visible = true)]
        public Int32 USER_ID { get; set; }


        /// <summary>
        /// 用户名称
        /// </summary>
        [DBField]
        [DisplayName("用户名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_NAME { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        [DBField]
        [DisplayName("合计金额")]
        [UIField(visible = true)]
        public Decimal MONEY_TOTAL { get; set; }

        /// <summary>
        /// 产品金额
        /// </summary>
        [DBField]
        [DisplayName("产品金额")]
        [UIField(visible = true)]
        public Decimal MONEY_GOODS { get; set; }

        /// <summary>
        /// 记录创建日期
        /// </summary>
        [DBField]
        [DisplayName("记录创建日期")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        [DBField]
        [DisplayName("联系人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTACTER_NAME { get; set; }


        /// <summary>
        /// 联系地址(省份-城市-区域)
        /// </summary>
        [DBField]
        [DisplayName("联系地址(省份-城市-区域)")]
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
        /// 用户手机号码
        /// </summary>
        [DBField]
        [DisplayName("用户手机号码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_PHONE { get; set; }


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
        /// 折扣金额
        /// </summary>
        [DBField]
        [DisplayName("折扣金额")]
        [UIField(visible = true)]
        public Decimal DISCOUNT_PAYMENY { get; set; }

        /// <summary>
        /// 赠送-金额
        /// </summary>
        [DBField]
        [DisplayName("赠送-金额")]
        [UIField(visible = true)]
        public Decimal PRESENT_MONEY { get; set; }

        /// <summary>
        /// 赠送-点数
        /// </summary>
        [DBField]
        [DisplayName("赠送-点数")]
        [UIField(visible = true)]
        public Decimal PRESENT_POINT { get; set; }

        /// <summary>
        /// 赠送-经验证
        /// </summary>
        [DBField]
        [DisplayName("赠送-经验证")]
        [UIField(visible = true)]
        public Decimal PRESENT_EXP { get; set; }


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
        public DateTime OP_TIME { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [DBField]
        [DisplayName("完成时间")]
        [UIField(visible = true)]
        public DateTime? END_TIME { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [DisplayName("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

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
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }





    }
}
