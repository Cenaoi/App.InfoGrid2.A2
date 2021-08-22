using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Mall
{
    /// <summary>
    /// 订单单头
    /// </summary>
    [DBTable("MALL_ORDER")]
    [Description("订单单头")]
    [Serializable]
    public class MALL_ORDER : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 MALL_ORDER_ID { get; set; }

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

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }


        /// <summary>
        /// 订单单头主键编码
        /// </summary>
        [DBField]
        [DisplayName("订单单头主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PK_ORDER_CODE { get; set; }


        /// <summary>
        /// 外键，微信用户主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，微信用户主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FK_W_CODE { get; set; }


        /// <summary>
        /// 订单编号
        /// </summary>
        [DBField]
        [DisplayName("订单编号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_NO { get; set; }

        /// <summary>
        /// 订单合计总金额
        /// </summary>
        [DBField]
        [DisplayName("订单合计总金额")]
        [UIField(visible = true)]
        public Decimal MONEY_TOTAL { get; set; }


        /// <summary>
        /// 付款类型
        /// </summary>
        [DBField]
        [DisplayName("付款类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PAY_TYPE_TEXT { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [DBField]
        [DisplayName("商品数量")]
        [UIField(visible = true)]
        public Int32 GOODS_NUM { get; set; }


        /// <summary>
        /// 收货人
        /// </summary>
        [DBField]
        [DisplayName("收货人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONSIGNEE { get; set; }


        /// <summary>
        /// 收货地址
        /// </summary>
        [DBField]
        [DisplayName("收货地址")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String ADDRESS { get; set; }


        /// <summary>
        /// 收货人电话
        /// </summary>
        [DBField]
        [DisplayName("收货人电话")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONSIGNEE_TEL { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 2000)]
        [UIField(visible = true)]
        public String REMART { get; set; }


        /// <summary>
        /// 订单详情
        /// </summary>
        [DBField]
        [DisplayName("订单详情")]
        [LMValidate(maxLen = 2000)]
        [UIField(visible = true)]
        public String ORDER_INTRO { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        [DBField]
        [DisplayName("付款时间")]
        [UIField(visible = true)]
        public DateTime? PAY_DATE { get; set; }


        /// <summary>
        /// 快递单号
        /// </summary>
        [DBField]
        [DisplayName("快递单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EXPRESS_NO { get; set; }


        /// <summary>
        /// 微信支付成功单号
        /// </summary>
        [DBField]
        [DisplayName("微信支付成功单号")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String W_P_ORDER_NO { get; set; }


        /// <summary>
        /// 订单状态信息
        /// </summary>
        [DBField]
        [DisplayName("订单状态信息")]
        [LMValidate(maxLen = 1000)]
        [UIField(visible = true)]
        public String O_SID_TEXT { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        [DBField]
        [DisplayName("微信昵称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_NICKNAME { get; set; }


        /// <summary>
        /// 微信昵称
        /// </summary>
        [DBField]
        [DisplayName("微信昵称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EXPRESS_TEXT { get; set; }



        /// <summary>
        /// 订单状态
        /// </summary>
        [DBField]
        [DisplayName("订单状态")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_SID { get; set; }


        /// <summary>
        /// 订单状态名称
        /// </summary>
        [DBField]
        [DisplayName("订单状态名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_SID_TEXT { get; set; }


    }
}
