using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 表名总表
    /// </summary>
    [DBTable("IG2_VIEW_TABLE")]
    [Description("表名总表")]
    [Serializable]
    public class IG2_VIEW_TABLE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_VIEW_TABLE_ID { get; set; }

        /// <summary>
        /// 视图ID
        /// </summary>
        [DBField]
        [DisplayName("视图ID")]
        [UIField(visible = true)]
        public Int32 IG2_VIEW_ID { get; set; }

        /// <summary>
        /// 数据表ID
        /// </summary>
        [DBField]
        [DisplayName("数据表ID")]
        [UIField(visible = true)]
        public Int32 TABLE_ID { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        [DBField]
        [DisplayName("关联类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String RELATION_TYPE { get; set; }


        /// <summary>
        /// 真实字段1
        /// </summary>
        [DBField]
        [DisplayName("真实字段1")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_FIELD { get; set; }


        /// <summary>
        /// 真实字段2
        /// </summary>
        [DBField]
        [DisplayName("真实字段2")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_DB_FIELD { get; set; }


        /// <summary>
        /// 真实表名1
        /// </summary>
        [DBField]
        [DisplayName("真实表名1")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 真实表名2
        /// </summary>
        [DBField]
        [DisplayName("真实表名2")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_TABLE_NAME { get; set; }


        /// <summary>
        /// 显示表名1
        /// </summary>
        [DBField]
        [DisplayName("显示表名1")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_TEXT { get; set; }


        /// <summary>
        /// 显示表名2
        /// </summary>
        [DBField]
        [DisplayName("显示表名2")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_TABLE_TEXT { get; set; }


        /// <summary>
        /// 显示字段1
        /// </summary>
        [DBField]
        [DisplayName("显示字段1")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FIELD_TEXT { get; set; }


        /// <summary>
        /// 显示字段2
        /// </summary>
        [DBField]
        [DisplayName("显示字段2")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_FIELD_TEXT { get; set; }

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
