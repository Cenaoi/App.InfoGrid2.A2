using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 打印模板
    /// </summary>
    [DBTable("BIZ_PRINT_TEMPLATE")]
    [Description("打印模板")]
    [Serializable]
    public class BIZ_PRINT_TEMPLATE : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 BIZ_PRINT_TEMPLATE_ID { get; set; }





        /// <summary>
        /// 页面ID
        /// </summary>
        [DBField]
        [DisplayName("页面ID")]
        [UIField(visible = true)]
        public Int32 PAGE_ID { get; set; }


        /// <summary>
        /// 页面名称
        /// </summary>
        [DBField]
        [DisplayName("页面名称")]
        [UIField(visible = true)]
        public String PAGE_TEXT { get; set; }



        /// <summary>
        /// 主表ID
        /// </summary>
        [DBField]
        [DisplayName("主表ID")]
        [UIField(visible = true)]
        public Int32 MAIN_TABLE_ID { get; set; }


        /// <summary>
        /// 主表名称
        /// </summary>
        [DBField]
        [DisplayName("主表名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MAIN_TABLE_NAME { get; set; }


        /// <summary>
        /// 主表类型
        /// </summary>
        [DBField]
        [DisplayName("主表类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MAIN_TABLE_TYPE { get; set; }


        /// <summary>
        /// 子表名称
        /// </summary>
        [DBField]
        [DisplayName("子表名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUB_TABLE_NAME { get; set; }


        /// <summary>
        /// 子表对应主表ID列
        /// </summary>
        [DBField]
        [DisplayName("子表对应主表ID列")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SUB_F_FIELD { get; set; }


        /// <summary>
        /// 模板路径
        /// </summary>
        [DBField]
        [DisplayName("模板路径")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String TEMPLATE_URL { get; set; }


        /// <summary>
        /// 模板名称
        /// </summary>
        [DBField]
        [DisplayName("模板名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TEMPLATE_NAME { get; set; }


        /// <summary>
        /// 模板类型
        /// </summary>
        [DBField]
        [DisplayName("模板类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TEMPLATE_TYPE { get; set; }

        /// <summary>
        /// 数据表数量,0--为单表,1--两张表
        /// </summary>
        [DBField]
        [DisplayName("数据表数量,0--为单表,1--两张表")]
        [UIField(visible = true)]
        public Int32 TABLE_NUMBER { get; set; }

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
        /// 结构权限编码
        /// </summary>
        [DBField]
        public string ROW_STRUCE_CODE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        public String REMARKS { get; set; }


    }
}
