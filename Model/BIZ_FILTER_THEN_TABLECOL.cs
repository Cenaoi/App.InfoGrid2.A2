using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 过滤-表的字段属性
    /// </summary>
    [DBTable("BIZ_FILTER_THEN_TABLECOL")]
    [Description("过滤-表的字段属性")]
    [Serializable]
    public class BIZ_FILTER_THEN_TABLECOL : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_THEN_TABLECOL_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_THEN_TABLE_ID { get; set; }

        /// <summary>
        /// 上上级ID
        /// </summary>
        [DBField]
        [DisplayName("上上级ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_ID { get; set; }


        /// <summary>
        /// 数据库字段名
        /// </summary>
        [DBField]
        [DisplayName("数据库字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_FIELD { get; set; }


        /// <summary>
        /// 字段描述
        /// </summary>
        [DBField]
        [DisplayName("字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FIELD_TEXT { get; set; }

        /// <summary>
        /// 是否可视
        /// </summary>
        [DBField]
        [DisplayName("是否可视")]
        [UIField(visible = true)]
        public Boolean IS_VISIBLE { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        [DBField]
        [DisplayName("是否只读")]
        [UIField(visible = true)]
        public Boolean IS_READONLY { get; set; }


        /// <summary>
        /// 排序类型
        /// </summary>
        [DBField]
        [DisplayName("排序类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SORT_TYPE { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        [DBField]
        [DisplayName("排序顺序")]
        [UIField(visible = true)]
        public Int32 SORT_ORDER { get; set; }


        /// <summary>
        /// 筛选器
        /// </summary>
        [DBField]
        [DisplayName("筛选器")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_1 { get; set; }


        /// <summary>
        /// 筛选器
        /// </summary>
        [DBField]
        [DisplayName("筛选器")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_2 { get; set; }


        /// <summary>
        /// 筛选器
        /// </summary>
        [DBField]
        [DisplayName("筛选器")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_3 { get; set; }

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
        [DBField, DefaultValue("(GETDATE())")]
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
        /// 记录创建角色代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建角色代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ROLE_CODE { get; set; }


        /// <summary>
        /// 记录创建公司代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建公司代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_COMP_CODE { get; set; }


        /// <summary>
        /// 记录创建部门代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建部门代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ORG_CODE { get; set; }


        /// <summary>
        /// 记录创建人员代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建人员代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_USER_CODE { get; set; }

    }
}
