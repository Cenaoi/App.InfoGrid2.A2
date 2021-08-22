using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.CMS
{
    /// <summary>
    /// 网站表
    /// </summary>
    [DBTable("CMS_WEBSITE")]
    [Description("网站表")]
    [Serializable]
    public class CMS_WEBSITE : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 CMS_WEBSITE_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_WEBSITE_CODE { get; set; }


        /// <summary>
        /// 网站标识码
        /// </summary>
        [DBField]
        [DisplayName("网站标识码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String WEB_IDENTIFIER { get; set; }


        /// <summary>
        /// 网站标题
        /// </summary>
        [DBField]
        [DisplayName("网站标题")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String WEB_TITLE { get; set; }


        /// <summary>
        /// 网站关键字
        /// </summary>
        [DBField]
        [DisplayName("网站关键字")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String WEB_KEYWORD { get; set; }


        /// <summary>
        /// 网站描述
        /// </summary>
        [DBField]
        [DisplayName("网站描述")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String WEB_DESC { get; set; }


        /// <summary>
        /// 底部信息
        /// </summary>
        [DBField]
        [DisplayName("底部信息")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String FOOTER_CONTENT { get; set; }


        /// <summary>
        /// 统计代码
        /// </summary>
        [DBField]
        [DisplayName("统计代码")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String STATISTICS_CODE { get; set; }


        /// <summary>
        /// 客服代码
        /// </summary>
        [DBField]
        [DisplayName("客服代码")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String CS_CODE { get; set; }


        /// <summary>
        /// 水印
        /// </summary>
        [DBField]
        [DisplayName("水印")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String WATERMARK { get; set; }


        /// <summary>
        /// SEO优化
        /// </summary>
        [DBField]
        [DisplayName("SEO优化")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String SEO_OPTIMIZATION { get; set; }


        /// <summary>
        /// 公司全称
        /// </summary>
        [DBField]
        [DisplayName("公司全称")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String COMP_FULL_TEXT { get; set; }


        /// <summary>
        /// 公司简称
        /// </summary>
        [DBField]
        [DisplayName("公司简称")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String COMP_ABBREVIATION { get; set; }


        /// <summary>
        /// SEO关键字
        /// </summary>
        [DBField]
        [DisplayName("SEO关键字")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String SEO_KEYWORD { get; set; }


        /// <summary>
        /// SEO描述
        /// </summary>
        [DBField]
        [DisplayName("SEO描述")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String SEO_DESC { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
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

    }
}
