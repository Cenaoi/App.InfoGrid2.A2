using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace EC5.DbCascade.Model
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    [DBTable("IG2_ACTION_FILTER")]
    [Description("过滤条件")]
    [Serializable]
    public class IG2_ACTION_FILTER : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_FILTER_ID { get; set; }

        /// <summary>
        /// 关联ID
        /// </summary>
        [DBField]
        [DisplayName("关联ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_ID { get; set; }


        /// <summary>
        /// 左右:L-左边;R-右边
        /// </summary>
        [DBField]
        [DisplayName("左右:L-左边;R-右边")]
        [LMValidate(maxLen = 1)]
        [UIField(visible = true)]
        public String L_R_TAG { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_TABLE_NAME { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        [DBField]
        [DisplayName("字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_COL { get; set; }

        /// <summary>
        /// 字段名称描述
        /// </summary>
        [DBField]
        public string A_COL_TEXT { get; set; }



        /// <summary>
        /// 逻辑.大于,小于,等于....
        /// </summary>
        [DBField]
        [DisplayName("逻辑.大于,小于,等于....")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_LOGIN { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_MODE { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_VALUE_TABLE { get; set; }

        /// <summary>
        /// 表名描述
        /// </summary>
        [DBField]
        public string B_VALUE_TABLE_TEXT { get; set; }



        /// <summary>
        /// 字段名
        /// </summary>
        [DBField]
        [DisplayName("字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_VALUE_COL { get; set; }

        
        /// <summary>
        /// 字段名描述
        /// </summary>
        [DBField]
        public string B_VALUE_COL_TEXT { get; set; }


        /// <summary>
        /// 逻辑.大于,小于,等于....
        /// </summary>
        [DBField]
        [DisplayName("逻辑.大于,小于,等于....")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_LOGIN { get; set; }


        /// <summary>
        /// 值模式
        /// </summary>
        [DBField]
        [DisplayName("值模式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_VALUE_MODE { get; set; }


        /// <summary>
        /// 固定值
        /// </summary>
        [DBField]
        [DisplayName("固定值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_VALUE_FIXED { get; set; }


        /// <summary>
        /// 函数值
        /// </summary>
        [DBField]
        [DisplayName("函数值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String B_VALUE_FUN { get; set; }


        /// <summary>
        /// 自定义公式
        /// </summary>
        [DBField]
        public string B_VALUE_USER_FUNC { get; set; }


        #region 记录常规属性

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
        [DBField,DefaultValue("(GETDATE())")]
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
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
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

    }
}
