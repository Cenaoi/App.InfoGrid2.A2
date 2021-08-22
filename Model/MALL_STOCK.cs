using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{

    /// <summary>
    /// 商品库存
    /// </summary>
    [DBTable("MALL_STOCK")]
    [Description("商品库存")]
    [Serializable]
    public class MALL_STOCK : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 MALL_STOCK_ID { get; set; }

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
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
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
        /// 主键编码
        /// </summary>
        [DBField]
        [DisplayName("主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PK_STOCK_CODE { get; set; }


        /// <summary>
        /// 外键，产品主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，产品主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FK_PROD_CODE { get; set; }


        /// <summary>
        /// 产品编号
        /// </summary>
        [DBField]
        [DisplayName("产品编号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_NO { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        [DBField]
        [DisplayName("产品名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_TEXT { get; set; }


        /// <summary>
        /// 产品类型编码
        /// </summary>
        [DBField]
        [DisplayName("产品类型编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_TYPE_CODE { get; set; }


        /// <summary>
        /// 产品类型名称
        /// </summary>
        [DBField]
        [DisplayName("产品类型名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_TEXT_TEXT { get; set; }

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
        /// 重量
        /// </summary>
        [DBField]
        [DisplayName("重量")]
        [UIField(visible = true)]
        public Decimal WEIGHT { get; set; }


        /// <summary>
        /// 条形码
        /// </summary>
        [DBField]
        [DisplayName("条形码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BAR_CODE { get; set; }


        /// <summary>
        /// 产品简介
        /// </summary>
        [DBField]
        [DisplayName("产品简介")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String PROD_INTRO { get; set; }

        /// <summary>
        /// 可以销售
        /// </summary>
        [DBField]
        [DisplayName("可以销售")]
        [UIField(visible = true)]
        public Boolean CAN_SALE { get; set; }


        /// <summary>
        /// 产品缩略图
        /// </summary>
        [DBField]
        [DisplayName("产品缩略图")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String PROD_THUMB { get; set; }


        /// <summary>
        /// 多图
        /// </summary>
        [DBField]
        [DisplayName("多图")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String MULTIPLE_PHOTO_LIST { get; set; }

        /// <summary>
        /// 首页是否显示
        /// </summary>
        [DBField]
        [DisplayName("首页是否显示")]
        [UIField(visible = true)]
        public Boolean HOME_VISIBLE { get; set; }


        /// <summary>
        /// 仓库编码
        /// </summary>
        [DBField]
        [DisplayName("仓库编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STORE_CODE { get; set; }


        /// <summary>
        /// 仓库名称
        /// </summary>
        [DBField]
        [DisplayName("仓库名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STORE_TEXT { get; set; }


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
        /// 库存数量
        /// </summary>
        [DBField]
        [DisplayName("库存数量")]
        [UIField(visible = true)]
        public Decimal STOCK_NUM { get; set; }


        /// <summary>
        /// 库存数量明细表的主键ID
        /// </summary>
    	[DBField]
        [DisplayName("库存数量明细表的主键ID")]
        [UIField(visible = true)]
        public Int32 UT_118_ID { get; set; }


        /// <summary>
        /// 内置码
        /// </summary>
        [DBField]
        [DisplayName("内置码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COL_6 { get; set; }

    }



}
