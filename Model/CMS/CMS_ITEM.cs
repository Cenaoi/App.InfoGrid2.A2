using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;


namespace App.InfoGrid2.Model.CMS
{
    /// <summary>
    /// 
    /// </summary>
    [DBTable("CMS_ITEM")]
    [Description("")]
    [Serializable]
    public class CMS_ITEM : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 CMS_ITEM_ID { get; set; }


        /// <summary>
        /// 目录编码
        /// </summary>
        [DBField]
        [DisplayName("目录编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CATA_CODE { get; set; }

        /// <summary>
        /// 目录名称
        /// </summary>
        [DBField]
        public string CATA_TEXT { get; set; }


        /// <summary>
        /// 类别标签
        /// </summary>
        [DBField]
        [DisplayName("类别标签")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String CATE_TEXT { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [DBField]
        public string C_IMAGE_URL { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        [DBField]
        [DisplayName("标题")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String C_TITLE { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        [DBField]
        [DisplayName("简介")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String C_INTRO { get; set; }


        /// <summary>
        /// 内容
        /// </summary>
        [DBField]
        [DisplayName("内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String C_CONTENT { get; set; }


        /// <summary>
        /// 作者编码
        /// </summary>
        [DBField]
        [DisplayName("作者编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String AUTHOR_CODE { get; set; }


        /// <summary>
        /// 作者名称
        /// </summary>
        [DBField]
        [DisplayName("作者名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String AUTHOR_TEXT { get; set; }

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
