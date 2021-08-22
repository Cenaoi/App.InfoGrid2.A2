using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.JF
{
    /// <summary>
    /// 购物车
    /// </summary>
    [DBTable("ES_S_CAR")]
    [Description("购物车")]
    [Serializable]
    public class ES_S_CAR : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 ES_S_CAR_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_SC_CODE { get; set; }


        /// <summary>
        /// 外键，微信用户自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，微信用户自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_W_CODE { get; set; }


        /// <summary>
        /// 外键，商品自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，商品自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_P_CODE { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        [DBField]
        [DisplayName("商品名称")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String PROD_TEXT { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        [DBField]
        [DisplayName("简介")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_INTRO { get; set; }



        /// <summary>
        /// 商品数量
        /// </summary>
        [DBField]
        [DisplayName("商品数量")]
        [UIField(visible = true)]
        public Int32 PROD_NUM { get; set; }


        /// <summary>
        /// 产品缩略图
        /// </summary>
        [DBField]
        [DisplayName("产品缩略图")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PROD_THUMB { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        [DBField]
        [DisplayName("小计")]
        [UIField(visible = true)]
        public Decimal SUB_TOTA { get; set; }

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
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

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
        [DBField, DefaultValue("(GETDATE())")]
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
        /// 记录用户自定义排序
        /// </summary>
        [DBField]
        [DisplayName("记录用户自定义排序")]
        [UIField(visible = true)]
        public Decimal ROW_USER_SEQ { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [DBField]
        [DisplayName("是否选中")]
        [UIField(visible = true)]
        public Boolean IS_CHECKED { get; set; }

    }
}
