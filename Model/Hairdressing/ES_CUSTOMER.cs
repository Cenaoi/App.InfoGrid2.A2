using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 客户档案
    /// </summary>
    [DBTable("ES_CUSTOMER")]
    [Description("客户档案")]
    [Serializable]
    public class ES_CUSTOMER : LightModel
    {

        /// <summary>
        /// 主键
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("主键")]
        [UIField(visible = true)]
        public Int32 ES_CUSTOMER_ID { get; set; }


        /// <summary>
        /// 真实名称
        /// </summary>
        [DBField]
        [DisplayName("真实名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TRUE_NAME { get; set; }


        /// <summary>
        /// 门店名称
        /// </summary>
        [DBField]
        [DisplayName("门店名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STORE_NAME { get; set; }


        /// <summary>
        /// 职位
        /// </summary>
        [DBField]
        [DisplayName("职位")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CUS_POSITION { get; set; }


        /// <summary>
        /// 手机
        /// </summary>
        [DBField]
        [DisplayName("手机")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CUS_PHONE { get; set; }


        /// <summary>
        /// 收货地址
        /// </summary>
        [DBField]
        [DisplayName("收货地址")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String RECEIPT_ADDRESS { get; set; }


        /// <summary>
        /// 微信昵称
        /// </summary>
        [DBField]
        [DisplayName("微信昵称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String WECHAT_NICKNAME { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        [DBField]
        [DisplayName("性别")]
        [LMValidate(maxLen = 5)]
        [UIField(visible = true)]
        public String CUS_SEX { get; set; }


        /// <summary>
        /// 区域
        /// </summary>
        [DBField]
        [DisplayName("区域")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CUS_REGIONAL { get; set; }


        /// <summary>
        /// 上级公司
        /// </summary>
        [DBField]
        [DisplayName("上级公司")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PARENT_COMPANY { get; set; }


        /// <summary>
        /// 真实门店
        /// </summary>
        [DBField]
        [DisplayName("真实门店")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TRUE_STORE { get; set; }

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
