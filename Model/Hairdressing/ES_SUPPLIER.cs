using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 供应商
    /// </summary>
    [DBTable("ES_SUPPLIER")]
    [Description("供应商")]
    [Serializable]
    public class ES_SUPPLIER : LightModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("主键")]
        [UIField(visible = true)]
        public Int32 ES_SUPPLIER_ID { get; set; }


        /// <summary>
        /// 供应商编码
        /// </summary>
        [DBField]
        [DisplayName("供应商编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUP_CODE { get; set; }


        /// <summary>
        /// 供应商名称
        /// </summary>
        [DBField]
        [DisplayName("供应商名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUP_NAME { get; set; }


        /// <summary>
        /// 供应商联系人
        /// </summary>
        [DBField]
        [DisplayName("供应商联系人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUP_CONTACTS { get; set; }


        /// <summary>
        /// 供应商电话
        /// </summary>
        [DBField]
        [DisplayName("供应商电话")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUP_PHONE { get; set; }


        /// <summary>
        /// 供应商地址
        /// </summary>
        [DBField]
        [DisplayName("供应商地址")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String SUP_ADDRESS { get; set; }

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
