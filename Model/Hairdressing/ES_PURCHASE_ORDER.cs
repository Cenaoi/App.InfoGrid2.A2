using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 进货单头
    /// </summary>
    [DBTable("ES_PURCHASE_ORDER")]
    [Description("进货单头")]
    [Serializable]
    public class ES_PURCHASE_ORDER : LightModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("主键")]
        [UIField(visible = true)]
        public Int32 ES_PURCHASE_ORDER_ID { get; set; }


        /// <summary>
        /// 订单号
        /// </summary>
        [DBField]
        [DisplayName("订单号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ORDER_NUM { get; set; }


        /// <summary>
        /// 制单人
        /// </summary>
        [DBField]
        [DisplayName("制单人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CREATE_USER { get; set; }


        /// <summary>
        /// 供应商编码
        /// </summary>
        [DBField]
        [DisplayName("供应商编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUPPLIER_CODE { get; set; }


        /// <summary>
        /// 供应商名称
        /// </summary>
        [DBField]
        [DisplayName("供应商名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUPPLIER_NAME { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        [DBField]
        [DisplayName("联系人")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTACTS { get; set; }


        /// <summary>
        /// 手机
        /// </summary>
        [DBField]
        [DisplayName("手机")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PHONE { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        [DBField]
        [DisplayName("地址")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ADDRESS { get; set; }


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
