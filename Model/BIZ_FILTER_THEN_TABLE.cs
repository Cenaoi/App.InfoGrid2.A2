using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 过滤-执行表操作
    /// </summary>
    [DBTable("BIZ_FILTER_THEN_TABLE")]
    [Description("过滤-执行表操作")]
    [Serializable]
    public class BIZ_FILTER_THEN_TABLE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_THEN_TABLE_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_THEN_ID { get; set; }

        /// <summary>
        /// 上上级ID
        /// </summary>
        [DBField]
        [DisplayName("上上级ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILTER_ID { get; set; }


        /// <summary>
        /// 表类型。复制 IG2_TABLE 同字段
        /// </summary>
        [DBField]
        [DisplayName("表类型。复制 IG2_TABLE 同字段")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TABLE_TYPE_ID { get; set; }

        /// <summary>
        /// 表ID。复制 IG2_TABLE 同字段
        /// </summary>
        [DBField]
        [DisplayName("表ID。复制 IG2_TABLE 同字段")]
        [UIField(visible = true)]
        public Int32 TABLE_ID { get; set; }

        /// <summary>
        /// 表 GUID。复制 IG2_TABLE 同字段
        /// </summary>
        [DBField]
        [DisplayName("表 GUID。复制 IG2_TABLE 同字段")]
        [UIField(visible = true)]
        public Guid TABLE_UID { get; set; }


        /// <summary>
        /// 表名。复制 IG2_TABLE 同字段
        /// </summary>
        [DBField]
        [DisplayName("表名。复制 IG2_TABLE 同字段")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 页面控件ID
        /// </summary>
        [DBField]
        [DisplayName("页面控件ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTROL_ID { get; set; }


        /// <summary>
        /// 排序字符串
        /// </summary>
        [DBField]
        [DisplayName("排序字符串")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SORT_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
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
