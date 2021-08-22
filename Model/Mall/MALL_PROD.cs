using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Mall
{
    /// <summary>
    /// 商品档案表
    /// </summary>
    [DBTable("MALL_PROD")]
    [Description("商品档案表")]
    [Serializable]
    public class MALL_PROD : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 MALL_PROD_ID { get; set; }

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
        /// 产品主键编码
        /// </summary>
        [DBField]
        [DisplayName("产品主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PK_PROD_CODE { get; set; }


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
        public String PROD_TYPE_TEXT { get; set; }

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
        /// 客户自定义商品ID
        /// </summary>
        [DBField]
        [DisplayName("客户自定义商品ID")]
        [UIField(visible = true)]
        public Int32 PROD_ID { get; set; }


        /// <summary>
        /// 产品标签，可以用来过滤一些数据的
        /// </summary>
        [DBField]
        [DisplayName("产品标签，可以用来过滤一些数据的")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String PROD_TAG { get; set; }

        /// <summary>
        /// 是否为公共产品
        /// </summary>
        [DBField]
        [DisplayName("是否为公共产品")]
        [UIField(visible = true)]
        public Boolean IS_COMMON { get; set; }



    }
}
