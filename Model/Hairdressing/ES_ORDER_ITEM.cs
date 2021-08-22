using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [DBTable("ES_ORDER_ITEM")]
    [Description("订单明细")]
    [Serializable]
    public class ES_ORDER_ITEM : LightModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("主键")]
        [UIField(visible = true)]
        public Int32 ES_ORDER_ITEM_ID { get; set; }

        /// <summary>
        /// 订单头ID
        /// </summary>
        [DBField]
        [DisplayName("订单头ID")]
        [UIField(visible = true)]
        public Int32 ORDER_ID { get; set; }


        /// <summary>
        /// SESSION_ID
        /// </summary>
        [DBField]
        [DisplayName("SESSION_ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SESSION_ID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [DBField]
        [DisplayName("产品ID")]
        [UIField(visible = true)]
        public Int32 PROD_ID { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        [DBField]
        [DisplayName("产品名称")]
        [LMValidate(maxLen = 50)]
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
        /// 产品规格ID
        /// </summary>
        [DBField]
        [DisplayName("产品规格ID")]
        [UIField(visible = true)]
        public Int32 PROD_DATA_ID { get; set; }


        /// <summary>
        /// 产品规格名称
        /// </summary>
        [DBField]
        [DisplayName("产品规格名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_DATA_NAME { get; set; }


        /// <summary>
        /// 产品缩略图
        /// </summary>
        [DBField]
        [DisplayName("产品缩略图")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_THUMB { get; set; }

        /// <summary>
        /// 产品数量
        /// </summary>
        [DBField]
        [DisplayName("产品数量")]
        [UIField(visible = true)]
        public Int32 PROD_NUM { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        [DBField]
        [DisplayName("小计")]
        [UIField(visible = true)]
        public Decimal SUB_TOTAL_MONEY { get; set; }


        /// <summary>
        /// 附件
        /// </summary>
        [DBField]
        [DisplayName("附件")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String ACCESSORY { get; set; }


        /// <summary>
        /// 产品规格详情
        /// </summary>
        [DBField]
        [DisplayName("产品规格详情")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String SPEC_REMARK { get; set; }


        /// <summary>
        /// 产品单位
        /// </summary>
        [DBField]
        [DisplayName("产品单位")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_UNIT { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DBField]
        [DisplayName("价格")]
        [UIField(visible = true)]
        public Decimal PRICE { get; set; }

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
        /// 市场价
        /// </summary>
        [DBField]
        [DisplayName("市场价")]
        [UIField(visible = true)]
        public Decimal PRICE_MARKET { get; set; }

        /// <summary>
        /// 产品重量
        /// </summary>
        [DBField]
        [DisplayName("产品重量")]
        [UIField(visible = true)]
        public Decimal PROD_WEIGHT { get; set; }


        /// <summary>
        /// 分类名称
        /// </summary>
        [DBField]
        [DisplayName("分类名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATEGORY_TEXT { get; set; }

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


    }
}
