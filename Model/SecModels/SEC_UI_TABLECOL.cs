using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using HWQ.Entity.LightModels;
using System.Diagnostics;

namespace App.InfoGrid2.Model.SecModels
{
 /// <summary>
    /// 权限-界面表列
    /// </summary>
    [DBTable("SEC_UI_TABLECOL")]
    [Description("权限-界面表列")]
    [Serializable]
    [DebuggerDisplay("DB_FIELD={DB_FIELD}, FIELD_TEXT={FIELD_TEXT}, IS_VISIBLE={IS_VISIBLE}, IS_READONLY={IS_READONLY}")]
    public class SEC_UI_TABLECOL : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_TABLECOL_ID { get; set; }

        /// <summary>
        /// 上上级ID
        /// </summary>
        [DBField]
        [DisplayName("上上级ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_TABLE_ID { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        [DBField]
        [DisplayName("字段名")]
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
        /// 只读
        /// </summary>
        [DBField]
        [DisplayName("只读")]
        [UIField(visible = true)]
        public Boolean IS_READONLY { get; set; }

        /// <summary>
        /// 可视
        /// </summary>
        [DBField]
        [DisplayName("可视")]
        [UIField(visible = true)]
        public Boolean IS_VISIBLE { get; set; }

        /// <summary>
        /// 查询可视
        /// </summary>
        [DBField]
        [DisplayName("查询可视")]
        [UIField(visible = true)]
        public Boolean IS_SEARCH_VISIBLE { get; set; }

        /// <summary>
        /// 列表可视
        /// </summary>
        [DBField]
        [DisplayName("列表可视")]
        [UIField(visible = true)]
        public Boolean IS_LIST_VISIBLE { get; set; }


        /// <summary>
        /// 排序类型
        /// </summary>
        [DBField]
        [DisplayName("排序类型")]
        [LMValidate(maxLen = 5)]
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
        /// 过滤1
        /// </summary>
        [DBField]
        [DisplayName("过滤1")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_1 { get; set; }


        /// <summary>
        /// 过滤2
        /// </summary>
        [DBField]
        [DisplayName("过滤2")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_2 { get; set; }


        /// <summary>
        /// 过滤3
        /// </summary>
        [DBField]
        [DisplayName("过滤3")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_3 { get; set; }






        /// <summary>
        /// 只读
        /// </summary>
        [DBField]
        [DisplayName("只读")]
        [UIField(visible = true)]
        public Boolean IS_READONLY_B { get; set; }

        /// <summary>
        /// 可视
        /// </summary>
        [DBField]
        [DisplayName("可视")]
        [UIField(visible = true)]
        public Boolean IS_VISIBLE_B { get; set; }

        /// <summary>
        /// 查询可视
        /// </summary>
        [DBField]
        [DisplayName("查询可视")]
        [UIField(visible = true)]
        public Boolean IS_SEARCH_VISIBLE_B { get; set; }

        /// <summary>
        /// 列表可视
        /// </summary>
        [DBField]
        [DisplayName("列表可视")]
        [UIField(visible = true)]
        public Boolean IS_LIST_VISIBLE_B { get; set; }


        /// <summary>
        /// 排序类型
        /// </summary>
        [DBField]
        [DisplayName("排序类型")]
        [LMValidate(maxLen = 5)]
        [UIField(visible = true)]
        public String SORT_TYPE_B { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        [DBField]
        [DisplayName("排序顺序")]
        [UIField(visible = true)]
        public Int32 SORT_ORDER_B { get; set; }


        /// <summary>
        /// 过滤1
        /// </summary>
        [DBField]
        [DisplayName("过滤1")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_1_B { get; set; }


        /// <summary>
        /// 过滤2
        /// </summary>
        [DBField]
        [DisplayName("过滤2")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_2_B { get; set; }


        /// <summary>
        /// 过滤3
        /// </summary>
        [DBField]
        [DisplayName("过滤3")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_3_B { get; set; }





        #region 记录公共字段

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
        [DBField, DefaultValue("(GetDate())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField, DefaultValue("(GetDate())")]
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

        #endregion

        /// <summary>
        /// 数据表
        /// </summary>
        [DBField]
        public string DB_TABLE { get; set; }
    }
}
