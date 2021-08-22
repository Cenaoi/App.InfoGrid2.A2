using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 业务-目录类型
    /// </summary>
    [DBTable("BIZ_CATALOG_TYPE")]
    [Description("业务-目录类型")]
    [Serializable]
    public class BIZ_CATALOG_TYPE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_CATALOG_TYPE_ID { get; set; }


        /// <summary>
        /// 类型代码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("类型代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CATA_TYPE_CODE { get; set; }


        /// <summary>
        /// 类型名称
        /// </summary>
        [DBField]
        [DisplayName("类型名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATA_TYPE_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        #region 记录常规字段

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

        #endregion

    }

}