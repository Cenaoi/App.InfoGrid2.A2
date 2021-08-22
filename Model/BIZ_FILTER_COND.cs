using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    [DBTable("BIZ_FILTER_COND")]
    [Description("过滤条件")]
    [Serializable]
    public class BIZ_FILTER_COND : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_COND_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_ID { get; set; }


        /// <summary>
        /// 参数类型。参数的来源，对应组件名称.
        /// </summary>
        [DBField]
        [DisplayName("参数类型。参数的来源，对应组件名称.")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PARAM_TYPE_ID { get; set; }


        /// <summary>
        /// 参数名
        /// </summary>
        [DBField]
        [DisplayName("参数名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PARAM_NAME { get; set; }

        /// <summary>
        /// 参数名描述
        /// </summary>
        [DBField]
        public string PARAM_TEXT { get; set; }


        /// <summary>
        /// 数据类型。整形，字符串，日期，...数组
        /// </summary>
        [DBField]
        [DisplayName("数据类型。整形，字符串，日期，...数组")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARAM_DBTYPE { get; set; }


        /// <summary>
        /// 逻辑运算。= 等于，> 大于，< 小于....
        /// </summary>
        [DBField]
        [DisplayName("逻辑运算。= 等于，> 大于，< 小于....")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String LOGIC { get; set; }


        /// <summary>
        /// 参数值
        /// </summary>
        [DBField]
        [DisplayName("参数值")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String P_VALUE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

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
