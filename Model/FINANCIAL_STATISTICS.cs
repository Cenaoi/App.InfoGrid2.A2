using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 财务统计
    /// </summary>
    [DBTable("FINANCIAL_STATISTICS")]
    [Description("财务统计")]
    [Serializable]
    public class FINANCIAL_STATISTICS : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 FINANCIAL_STATISTICS_ID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DBField]
        [Description("时间")]
        [UIField(visible = true)]
        public DateTime DATE_TIME { get; set; }


        /// <summary>
        /// sessionID
        /// </summary>
        [DBField]
        [Description("sessionID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SESSION_ID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField]
        [Description("创建时间")]
        [UIField(visible = true)]
        public DateTime CREATE_DATE_TIME { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [DBField]
        [Description("客户ID")]
        [UIField(visible = true)]
        public Int32 CUSTOMER_ID { get; set; }

        /// <summary>
        /// 应收款--期初的
        /// </summary>
        [DBField]
        [Description("应收款--期初的")]
        [UIField(visible = true)]
        public Decimal BEG_RECEIVABLES { get; set; }


        /// <summary>
        /// 单号
        /// </summary>
        [DBField]
        [Description("单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_NO { get; set; }

        /// <summary>
        /// 预收款--期初的
        /// </summary>
        [DBField]
        [Description("预收款--期初的")]
        [UIField(visible = true)]
        public Decimal BEG_PRE_RECEIVABLES { get; set; }

        /// <summary>
        /// 实际应收--期初的
        /// </summary>
        [DBField]
        [Description("实际应收--期初的")]
        [UIField(visible = true)]
        public Decimal BEG_AARECEIVABLE { get; set; }


        /// <summary>
        /// 摘要
        /// </summary>
        [DBField]
        [Description("摘要")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ABSTRACT { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DBField]
        [Description("数量")]
        [UIField(visible = true)]
        public Decimal F_NUMBER { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DBField]
        [Description("单价")]
        [UIField(visible = true)]
        public Decimal F_PRICE { get; set; }

        /// <summary>
        /// 应收款
        /// </summary>
        [DBField]
        [Description("应收款")]
        [UIField(visible = true)]
        public Decimal RECEIVABLES { get; set; }

        /// <summary>
        /// 预收款
        /// </summary>
        [DBField]
        [Description("预收款")]
        [UIField(visible = true)]
        public Decimal PRE_RECEIVABLES { get; set; }

        /// <summary>
        /// 应收款--期末的
        /// </summary>
        [DBField]
        [Description("应收款--期末的")]
        [UIField(visible = true)]
        public Decimal END_RECEIVABLES { get; set; }

        /// <summary>
        /// 预收款--期末的
        /// </summary>
        [DBField]
        [Description("预收款--期末的")]
        [UIField(visible = true)]
        public Decimal END_PRE_RECEIVABLES { get; set; }

        /// <summary>
        /// 实际应收款
        /// </summary>
        [DBField]
        [Description("实际应收款")]
        [UIField(visible = true)]
        public Decimal END_AARECEIVABLES { get; set; }


        /// <summary>
        /// 仓库编码
        /// </summary>
        [DBField]
        [Description("仓库编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String WAREHOUSE_CODE { get; set; }


        /// <summary>
        /// 仓库名称
        /// </summary>
        [DBField]
        [Description("仓库名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String WAREHOUSE_NAME { get; set; }




        /// <summary>
        /// 进仓--数量
        /// </summary>
        [DBField]
        [Description("进仓--数量")]
        [UIField(visible = true)]
        public Decimal NUMBER_IN { get; set; }

        /// <summary>
        /// 进仓--金额
        /// </summary>
        [DBField]
        [Description("进仓--金额")]
        [UIField(visible = true)]
        public Decimal MONERY_IN { get; set; }



        /// <summary>
        /// 出仓--数量
        /// </summary>
        [DBField]
        [Description("出仓--数量")]
        [UIField(visible = true)]
        public Decimal NUMBER_OUT { get; set; }

        /// <summary>
        /// 出仓--金额
        /// </summary>
        [DBField]
        [Description("出仓--金额")]
        [UIField(visible = true)]
        public Decimal MONERY_OUT { get; set; }




        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [Description("备注")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String REMARKS { get; set; }

    }

}
