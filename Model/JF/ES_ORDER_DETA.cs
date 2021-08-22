using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.JF
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [DBTable("ES_ORDER_DETA")]
    [Description("订单明细")]
    [Serializable]
    public class ES_ORDER_DETA : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 ES_ORDER_DETA_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_OD_CODE { get; set; }


        /// <summary>
        /// 外键，订单头自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，订单头自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_O_CODE { get; set; }


        /// <summary>
        /// 外键，商品自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，商品自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_P_CODE { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        [DBField]
        [DisplayName("产品名称")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String PROD_NAME { get; set; }


        /// <summary>
        /// 产品编码
        /// </summary>
        [DBField]
        [DisplayName("产品编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_CODE { get; set; }


        /// <summary>
        /// 产品类型（字符串）
        /// </summary>
        [DBField]
        [DisplayName("产品类型（字符串）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_TYPE_ID { get; set; }


        /// <summary>
        /// 单位名称
        /// </summary>
        [DBField]
        [DisplayName("单位名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String UNIT_NAME { get; set; }


        /// <summary>
        /// 产品缩略图
        /// </summary>
        [DBField]
        [DisplayName("产品缩略图")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_THUMB { get; set; }


        /// <summary>
        /// 多图
        /// </summary>
        [DBField]
        [DisplayName("多图")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MULTIPLE_PHOTO_LIST { get; set; }


        /// <summary>
        /// 服务期限（单位名称）
        /// </summary>
        [DBField]
        [DisplayName("服务期限（单位名称）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SERVICE_TERM_UNIT { get; set; }


        /// <summary>
        /// 服务期限
        /// </summary>
        [DBField]
        [DisplayName("服务期限")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SERVICE_TERM { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DBField]
        [DisplayName("价格")]
        [UIField(visible = true)]
        public Decimal PRICE { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        [DBField]
        [DisplayName("市场价")]
        [UIField(visible = true)]
        public Decimal PRICE_MARKET { get; set; }

        /// <summary>
        /// 会员价
        /// </summary>
        [DBField]
        [DisplayName("会员价")]
        [UIField(visible = true)]
        public Decimal PRICE_MEMBER { get; set; }

        /// <summary>
        /// 代理商价格
        /// </summary>
        [DBField]
        [DisplayName("代理商价格")]
        [UIField(visible = true)]
        public Decimal PRICE_AGENT { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [DBField]
        [DisplayName("重量")]
        [UIField(visible = true)]
        public Decimal WEIGHT { get; set; }


        /// <summary>
        /// 商标
        /// </summary>
        [DBField]
        [DisplayName("商标")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TRADEMARK_NAME { get; set; }


        /// <summary>
        /// 条形码
        /// </summary>
        [DBField]
        [DisplayName("条形码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BAR_CODE { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        [DBField]
        [DisplayName("简介")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_INTRO { get; set; }


        /// <summary>
        /// 商品说明书
        /// </summary>
        [DBField]
        [DisplayName("商品说明书")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_EXPLAIN { get; set; }

        /// <summary>
        /// 可以销售
        /// </summary>
        [DBField]
        [DisplayName("可以销售")]
        [UIField(visible = true)]
        public Boolean CAN_SALE { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 STATUS_ID { get; set; }


        /// <summary>
        /// 开始销售日期（字符串）
        /// </summary>
        [DBField]
        [DisplayName("开始销售日期（字符串）")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DATE_SELL { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 人气值
        /// </summary>
        [DBField]
        [DisplayName("人气值")]
        [UIField(visible = true)]
        public Decimal POPULARITY_NUM { get; set; }

        /// <summary>
        /// 库存量
        /// </summary>
        [DBField]
        [DisplayName("库存量")]
        [UIField(visible = true)]
        public Decimal STOCKS { get; set; }


        /// <summary>
        /// 配件说明
        /// </summary>
        [DBField]
        [DisplayName("配件说明")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ACCESSORY { get; set; }


        /// <summary>
        /// 商品品牌
        /// </summary>
        [DBField]
        [DisplayName("商品品牌")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_BRAND { get; set; }


        /// <summary>
        /// 品类
        /// </summary>
        [DBField]
        [DisplayName("品类")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_CATEGORY { get; set; }


        /// <summary>
        /// 商品功效
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("商品功效")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String PROD_FUN_TEXT { get; set; }


        /// <summary>
        /// 商品型号
        /// </summary>
        [DBField]
        [DisplayName("商品型号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_MODEL { get; set; }

        /// <summary>
        /// 采购单价
        /// </summary>
        [DBField]
        [DisplayName("采购单价")]
        [UIField(visible = true)]
        public Decimal PURCHASE_PRICE { get; set; }


        /// <summary>
        /// 供应商编码
        /// </summary>
        [DBField]
        [DisplayName("供应商编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUPPLIERS_CODE { get; set; }

        /// <summary>
        /// 最低安全库存
        /// </summary>
        [DBField]
        [DisplayName("最低安全库存")]
        [UIField(visible = true)]
        public Int32 MINIMUM_SAFETY_STOCKS { get; set; }

        /// <summary>
        /// 最大安全库存
        /// </summary>
        [DBField]
        [DisplayName("最大安全库存")]
        [UIField(visible = true)]
        public Int32 MAXIMUM_SAFETY_STOCK { get; set; }

        /// <summary>
        /// 最小采购数量
        /// </summary>
        [DBField]
        [DisplayName("最小采购数量")]
        [UIField(visible = true)]
        public Int32 MIN_PURCHASE_QUANTITY { get; set; }

        /// <summary>
        /// 销售量
        /// </summary>
        [DBField]
        [DisplayName("销售量")]
        [UIField(visible = true)]
        public Int32 SALE_NUM { get; set; }


        /// <summary>
        /// 规格名称
        /// </summary>
        [DBField]
        [DisplayName("规格名称")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String SPEC_TEXT { get; set; }


        /// <summary>
        /// 组编码，用来区分商品是否是同一组的
        /// </summary>
        [DBField]
        [DisplayName("组编码，用来区分商品是否是同一组的")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String GROUP_CODE { get; set; }

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
        /// 小计
        /// </summary>
        [DBField]
        [DisplayName("小计")]
        [UIField(visible = true)]
        public Decimal SUB_TOTAL { get; set; }


    }
}
