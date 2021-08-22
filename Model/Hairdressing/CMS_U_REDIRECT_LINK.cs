using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 外链接
    /// </summary>
    [DBTable("CMS_U_REDIRECT_LINK")]
    [Description("外链接")]
    [Serializable]
    public class CMS_U_REDIRECT_LINK : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 CMS_U_REDIRECT_LINK_ID { get; set; }


        /// <summary>
        /// 显示文字
        /// </summary>
        [DBField]
        [DisplayName("显示文字")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String T_TEXT { get; set; }


        /// <summary>
        /// 链接地址
        /// </summary>
        [DBField]
        [DisplayName("链接地址")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String T_HREF { get; set; }


        /// <summary>
        /// 图片地址
        /// </summary>
        [DBField]
        [DisplayName("图片地址")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String IMAGE_URL { get; set; }

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
