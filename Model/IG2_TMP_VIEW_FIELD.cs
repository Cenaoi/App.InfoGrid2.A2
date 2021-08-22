using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 临时列名表
    /// </summary>
    [DBTable("IG2_TMP_VIEW_FIELD")]
    [Description("临时列名表")]
    [Serializable]
    public class IG2_TMP_VIEW_FIELD : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 IG2_TMP_VIEW_FIELD_ID { get; set; }

        /// <summary>
        /// 源数据表字段主键
        /// </summary>
        [DBField]
        public int IG2_VIEW_FIELD_ID { get; set; }


        /// <summary>
        /// 视图ID
        /// </summary>
        [DBField]
        [DisplayName("视图ID")]
        [UIField(visible = true)]
        public Int32 IG2_VIEW_ID { get; set; }


        /// <summary>
        /// 真实列名
        /// </summary>
        [DBField]
        [DisplayName("真实列名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FIELD_NAME { get; set; }


        /// <summary>
        /// 真实表名
        /// </summary>
        [DBField]
        [DisplayName("真实表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 别名
        /// </summary>
        [DBField]
        [DisplayName("别名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ALIAS { get; set; }

        /// <summary>
        /// 正式数据表ID
        /// </summary>
        [DBField]
        [DisplayName("正式数据表ID")]
        [UIField(visible = true)]
        public Int32 TABLE_ID { get; set; }


        /// <summary>
        /// 显示表名
        /// </summary>
        [DBField]
        [DisplayName("显示表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_TEXT { get; set; }


        /// <summary>
        /// 显示列名
        /// </summary>
        [DBField]
        [DisplayName("显示列名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FIELD_TEXT { get; set; }


        /// <summary>
        /// 字段显示
        /// </summary>
        [DBField]
        public bool FIELD_VISIBLE { get; set; }


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


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SORT_TYPE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String SORT_ORDER { get; set; }



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
        /// 记录-临时操作。A-新建，D-删除，E-修改
        /// </summary>
        [DBField]
        [DisplayName("记录-临时操作。A-新建，D-删除，E-修改")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TMP_OP_ID { get; set; }

        /// <summary>
        /// 记录-临时GUID
        /// </summary>
        [DBField]
        [DisplayName("记录-临时GUID")]
        [UIField(visible = true)]
        public Guid TMP_GUID { get; set; }

        /// <summary>
        /// 记录-操作时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
        [DisplayName("记录-操作时间")]
        [UIField(visible = true)]
        public DateTime TMP_OP_TIME { get; set; }

        [DBField]
        public string TMP_SESSION_ID { get; set; }
    }
}
