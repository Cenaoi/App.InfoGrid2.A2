using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Mall
{
    /// <summary>
    /// 规格档案表
    /// </summary>
    [DBTable("MALL_SPEC")]
    [Description("规格档案表")]
    [Serializable]
    public class MALL_SPEC : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 MALL_SPEC_ID { get; set; }

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
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }


        /// <summary>
        /// 主键编码
        /// </summary>
        [DBField]
        [DisplayName("主键编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PK_SPEC_CODE { get; set; }


        /// <summary>
        /// 客户自定义的规格编号
        /// </summary>
        [DBField]
        [DisplayName("客户自定义的规格编号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_NO { get; set; }


        /// <summary>
        /// 规格名称
        /// </summary>
        [DBField]
        [DisplayName("规格名称")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String SPEC_TEXT { get; set; }


        /// <summary>
        /// 规格类型
        /// </summary>
        [DBField]
        [DisplayName("规格类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SPEC_TYPE { get; set; }


        /// <summary>
        /// 分类编码
        /// </summary>
        [DBField]
        [DisplayName("分类编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATALOG_CODE { get; set; }


        /// <summary>
        /// 分类名称
        /// </summary>
        [DBField]
        [DisplayName("分类名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATALOG_TEXT { get; set; }

        /// <summary>
        /// 客户自定义的规格ID
        /// </summary>
        [DBField]
        [DisplayName("客户自定义的规格ID")]
        [UIField(visible = true)]
        public Int32 SPEC_ID { get; set; }

        /// <summary>
        /// 内置码
        /// </summary>
        [DBField]
        [DisplayName("内置码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COL_6 { get; set; }


    }

}
