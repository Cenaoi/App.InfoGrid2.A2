using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 导入规则映射
    /// </summary>
    [DBTable("IG2_IMPORT_RULE_MAP")]
    [Description("导入规则映射")]
    [Serializable]
    public class IG2_IMPORT_RULE_MAP : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_IMPORT_RULE_MAP_ID { get; set; }

        /// <summary>
        /// 导入规则ID，上级ID
        /// </summary>
        [DBField]
        [DisplayName("导入规则ID，上级ID")]
        [UIField(visible = true)]
        public Int32 IG2_IMPORT_RULE_ID { get; set; }



        
        /// <summary>
        /// 目标表名
        /// </summary>
        [DBField]
        [DisplayName("目标表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TARGET_TABLE { get; set; }


        /// <summary>
        /// 目标字段名
        /// </summary>
        [DBField]
        [DisplayName("目标字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TARGET_FIELD { get; set; }


        /// <summary>
        /// 目标字段描述
        /// </summary>
        [DBField]
        [DisplayName("目标字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TARGET_FIELD_TEXT { get; set; }


        /// <summary>
        /// 导入源-字段描述
        /// </summary>
        [DBField]
        [DisplayName("导入源-字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SRC_FIELD_TEXT { get; set; }


        /// <summary>
        /// 导入源-字段名
        /// </summary>
        [DBField]
        [DisplayName("导入源-字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SRC_FIELD { get; set; }

        /// <summary>
        /// 导入源-列索引
        /// </summary>
        [DBField]
        [DisplayName("导入源-列索引")]
        [UIField(visible = true)]
        public Int32 SRC_FIELD_INDEX { get; set; }


        /// <summary>
        /// 唯一性
        /// </summary>
        [DBField]
        [DisplayName("唯一性")]
        public bool VALID_UNIQUE { get; set; }

        /// <summary>
        /// 必填
        /// </summary>
        [DBField]
        [DisplayName("必填")]
        public bool VALID_REQUIRED { get; set; }

        /// <summary>
        /// 如果相同，就自动跳过
        /// </summary>
        [DBField]
        [DisplayName("如果相同，就自动跳过")]
        public bool VALID_AUTO_SKIP { get; set; }


        /// <summary>
        /// 如果空白，就自动跳过
        /// </summary>
        [DBField]
        [DisplayName("如果空白，就自动跳过")]
        public bool VALID_BLANK_SKIP { get; set; }



        /// <summary>
        /// 外数据-激活
        /// </summary>
        [DBField]
        [DisplayName("外数据-激活")]
        [UIField(visible = true)]
        public Boolean FOREIGN_ENABLED { get; set; }


        /// <summary>
        /// 外数据-表名
        /// </summary>
        [DBField]
        [DisplayName("外数据-表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FOREIGN_TABLE { get; set; }


        /// <summary>
        /// 外数据-返回的字段名
        /// </summary>
        [DBField]
        [DisplayName("外数据-返回的字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FOREIGN_RE_FIELD { get; set; }


        /// <summary>
        /// 外数据-过滤条件
        /// </summary>
        [DBField]
        [DisplayName("外数据-过滤条件")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String FOREIGN_FILTER { get; set; }

        /// <summary>
        /// 外数据-强制。不允许空，默认True
        /// </summary>
        [DBField]
        [DisplayName("外数据-强制。不允许空，默认True")]
        [UIField(visible = true)]
        public Boolean FOREIGN_MANDATORY { get; set; }

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


        /// <summary>
        /// 等于
        /// </summary>
        [DBField]
        [DisplayName("等于")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String EQUAL { get; set; }


        /// <summary>
        /// 默认值
        /// </summary>
        [DBField]
        [DisplayName("默认值")]
        public string DEFAULT_VALUE { get; set; }

    }
}
