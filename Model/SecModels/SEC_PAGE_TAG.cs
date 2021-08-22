using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限-用户的页面标记
    /// </summary>
    [DBTable("SEC_PAGE_TAG")]
    [Description("权限-用户的页面标记")]
    [Serializable]
    public class SEC_PAGE_TAG : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_PAGE_TAG_ID { get; set; }

        /// <summary>
        /// 页面ID，或菜单ID
        /// </summary>
        [DBField]
        [DisplayName("页面ID，或菜单ID")]
        [UIField(visible = true)]
        public Int32 UI_PAGE_ID { get; set; }


        /// <summary>
        /// 用户代码
        /// </summary>
        [DBField]
        [DisplayName("用户代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SEC_USER_CODE { get; set; }

        /// <summary>
        /// 是否采用自定义权限
        /// </summary>
        [DBField]
        [DisplayName("是否采用自定义权限")]
        [UIField(visible = true)]
        public Boolean IS_CUSTOM { get; set; }

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
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }
}
