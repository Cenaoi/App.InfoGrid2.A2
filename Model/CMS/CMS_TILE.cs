using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.CMS
{
    /// <summary>
    /// 单页表
    /// </summary>
    [DBTable("CMS_TILE")]
    [Description("单页表")]
    [Serializable]
    public class CMS_TILE : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 CMS_TILE_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_TILE_CODE { get; set; }


        /// <summary>
        /// 外键，菜单自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键，菜单自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_MENU_CODE { get; set; }


        /// <summary>
        /// 菜单名称
        /// </summary>
        [DBField]
        [DisplayName("菜单名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MENU_TEXT { get; set; }


        /// <summary>
        /// 单页内容
        /// </summary>
        [DBField]
        [DisplayName("单页内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String TILE_CONTEXE { get; set; }

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
