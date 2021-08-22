using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 导入规则
    /// </summary>
    [DBTable("IG2_IMPORT_RULE")]
    [Description("导入规则")]
    [Serializable]
    public class IG2_IMPORT_RULE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_IMPORT_RULE_ID { get; set; }


        /// <summary>
        /// 规则名称
        /// </summary>
        [DBField]
        [DisplayName("规则名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String RULE_NAME { get; set; }


        /// <summary>
        /// 数据表的数量
        /// </summary>
        [DBField]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 TABLE_COUNT { get; set; }

        

        /// <summary>
        /// 主表
        /// </summary>
        [DBField]
        [DisplayName("主表")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MAIN_TABLE { get; set; }

        
        /// <summary>
        /// 主表描述
        /// </summary>
        [DBField]
        [DisplayName("主表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MAIN_TABLE_TEXT { get; set; }


        

        /// <summary>
        /// 主表连接字段
        /// </summary>
        [DBField]
        [DisplayName("主表连接字段")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_MAIN_FIELE { get; set; }


        

        /// <summary>
        /// 子表连接字段
        /// </summary>
        [DBField]
        [DisplayName("子表连接字段")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_SUB_FIELE { get; set; }



        /// <summary>
        /// 子表连接父ID字段
        /// </summary>
        [DBField]
        [DisplayName("子表连接父ID字段")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_SUB_P_FIELE { get; set; }

        


       

        /// <summary>
        /// 目标表
        /// </summary>
        [DBField]
        [DisplayName("目标表")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TARGET_TABLE { get; set; }


        /// <summary>
        /// 目标表描述
        /// </summary>
        [DBField]
        [DisplayName("目标表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TARGET_TABLE_TEXT { get; set; }


        /// <summary>
        /// 导入源-文件路径
        /// </summary>
        [DBField]
        [DisplayName("导入源-文件路径")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String SRC_FILE { get; set; }


        /// <summary>
        /// 导入源-表名
        /// </summary>
        [DBField]
        [DisplayName("导入源-表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SRC_TABLE { get; set; }


        /// <summary>
        /// 导入源-描述
        /// </summary>
        [DBField]
        [DisplayName("导入源-描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SRC_TEXT { get; set; }

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

    }
}
