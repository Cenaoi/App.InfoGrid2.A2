using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 进货单明细
    /// </summary>
    [DBTable("ES_PURCHASE_ORDER_ITEM")]
    [Description("进货单明细")]
    [Serializable]
    public class ES_PURCHASE_ORDER_ITEM : LightModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("主键")]
        [UIField(visible = true)]
        public Int32 ES_PURCHASE_ORDER_ITEM_ID { get; set; }

        /// <summary>
        /// 单头ID
        /// </summary>
        [DBField]
        [DisplayName("单头ID")]
        [UIField(visible = true)]
        public Int32 ES_PURCHASE_ORDER_ID { get; set; }


        /// <summary>
        /// 产品编码
        /// </summary>
        [DBField]
        [DisplayName("产品编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_CODE { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        [DBField]
        [DisplayName("产品名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_NAME { get; set; }


        /// <summary>
        /// 规格编码
        /// </summary>
        [DBField]
        [DisplayName("规格编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_CODE { get; set; }


        /// <summary>
        /// 规格名称
        /// </summary>
        [DBField]
        [DisplayName("规格名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_NAME { get; set; }


        /// <summary>
        /// 单位
        /// </summary>
        [DBField]
        [DisplayName("单位")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String UNIT { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        [DBField]
        [DisplayName("成本价")]
        [UIField(visible = true)]
        public Decimal COST_PRICE { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DBField]
        [DisplayName("数量")]
        [UIField(visible = true)]
        public Int32 PROD_NUM { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DBField]
        [DisplayName("金额")]
        [UIField(visible = true)]
        public Decimal PROD_MONEY { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String REMARKS { get; set; }

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
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

    }
}
