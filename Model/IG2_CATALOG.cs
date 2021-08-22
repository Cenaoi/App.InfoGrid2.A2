using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 目录
    /// </summary>
    [DBTable("IG2_CATALOG")]
    [Description("目录")]
    [Serializable]
    public class IG2_CATALOG : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_CATALOG_ID { get; set; }

        /// <summary>
        /// 权限等级.0-用户自定义;10-业务层;20-设计师预留;40-固定的业务层;60-系统内部
        /// </summary>
        [DBField]
        [DisplayName("权限等级.0-用户自定义;10-业务层;20-设计师预留;40-固定的业务层;60-系统内部")]
        [UIField(visible = true)]
        public Int32 SEC_LEVEL { get; set; }

        /// <summary>
        /// 默认表类型.
        /// TABLE = 工作表;
        /// VIEW = 视图表;
        /// AREA = 复杂表的区域;
        /// </summary>
        [DBField]
        public string DEFAULT_TABLE_TYPE { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DBField]
        [DisplayName("父ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        [DBField]
        [DisplayName("类型ID")]
        [UIField(visible = true)]
        public Int32 IG2_CATALOG_TYPE_ID { get; set; }


        /// <summary>
        /// 类型描述
        /// </summary>
        [DBField]
        [DisplayName("类型描述")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String IG2_CATALOG_TYPE_TEXT { get; set; }


        /// <summary>
        /// 目录代码
        /// </summary>
        [DBField]
        [DisplayName("目录代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CODE { get; set; }


        /// <summary>
        /// 目录名称
        /// </summary>
        [DBField]
        [DisplayName("目录名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

        /// <summary>
        /// 记录-状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录-状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录-创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("记录-创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录-更新时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
        [DisplayName("记录-更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录-删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录-删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }


        /// <summary>
        /// 记录-作者ID
        /// </summary>
        [DBField]
        [DisplayName("记录-作者ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ID { get; set; }


        /// <summary>
        /// 记录-作者名称
        /// </summary>
        [DBField]
        [DisplayName("记录-作者名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_TEXT { get; set; }

    }
}
