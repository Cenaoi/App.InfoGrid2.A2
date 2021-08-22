using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Mall
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [DBTable("MALL_ORDER_ITEM")]
    [Description("订单明细")]
    [Serializable]
    public class MALL_ORDER_ITEM : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 MALL_ORDER_ITEM_ID { get; set; }

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
        /// 订单明细主键编码
        /// </summary>
        [DBField]
        [DisplayName("订单明细主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PK_ORDER_ITEM_CODE { get; set; }


        /// <summary>
        /// 订单单头主键编码
        /// </summary>
        [DBField]
        [DisplayName("订单单头主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FK_ORDER_CODE { get; set; }


        /// <summary>
        /// 外键，微信用户主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，微信用户主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FK_W_CODE { get; set; }



        /// <summary>
        /// 商品编码
        /// </summary>
        [DBField]
        [DisplayName("商品编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_CODE { get; set; }

        /// <summary>
        /// 客户自定义的产品编号
        /// </summary>
        [DBField]
        [DisplayName("客户自定义的产品编号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_NO { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        [DBField]
        [DisplayName("产品名称")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String PROD_TEXT { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [DBField]
        [DisplayName("商品价格")]
        [UIField(visible = true)]
        public Decimal PROD_PRICE { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [DBField]
        [DisplayName("商品数量")]
        [UIField(visible = true)]
        public Int32 PROD_NUM { get; set; }


        /// <summary>
        /// 产品简介
        /// </summary>
        [DBField]
        [DisplayName("产品简介")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String PROD_INTRO { get; set; }


        /// <summary>
        /// 规格1编码
        /// </summary>
        [DBField]
        [DisplayName("规格1编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_CODE_1 { get; set; }


        /// <summary>
        /// 规格1编号
        /// </summary>
        [DBField]
        [DisplayName("规格1编号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_NO_1 { get; set; }


        /// <summary>
        /// 规格1名称
        /// </summary>
        [DBField]
        [DisplayName("规格1名称")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String SPEC_TEXT_1 { get; set; }


        /// <summary>
        /// 规格2编码
        /// </summary>
        [DBField]
        [DisplayName("规格2编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_CODE_2 { get; set; }


        /// <summary>
        /// 规格2编号
        /// </summary>
        [DBField]
        [DisplayName("规格2编号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_NO_2 { get; set; }


        /// <summary>
        /// 规格2名称
        /// </summary>
        [DBField]
        [DisplayName("规格2名称")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String SPEC_TEXT_2 { get; set; }


        /// <summary>
        /// 产品缩略图
        /// </summary>
        [DBField]
        [DisplayName("产品缩略图")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String PROD_THUMB { get; set; }

        /// <summary>
        /// 产品ID(斌哥那边的)
        /// </summary>
        [DBField]
        [DisplayName("产品ID(斌哥那边的)")]
        [UIField(visible = true)]
        public Int32 PROD_ID { get; set; }


    }
}
