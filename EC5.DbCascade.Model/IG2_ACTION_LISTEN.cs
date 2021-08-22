using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace EC5.DbCascade.Model
{
    /// <summary>
    /// 监听右边字段
    /// </summary>
    [DBTable("IG2_ACTION_LISTEN")]
    [Description("监听右边字段")]
    [Serializable]
    public class IG2_ACTION_LISTEN : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_LISTEN_ID { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DBField]
        [DisplayName("父ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_ID { get; set; }


        /// <summary>
        /// 数据库字段
        /// </summary>
        [DBField]
        [DisplayName("数据库字段")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_DB_FIELD { get; set; }


        /// <summary>
        /// 字段描述
        /// </summary>
        [DBField]
        [DisplayName("字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_FIELD_TEXT { get; set; }

        /// <summary>
        /// 激活监听
        /// </summary>
        [DBField]
        [DisplayName("激活监听")]
        [UIField(visible = true)]
        public Boolean LISTEN_ENABLED { get; set; }


        /// <summary>
        /// 起始值
        /// </summary>
        [DisplayName("起始值")]
        [DBField]
        public string VALUE_FROM { get; set; }

        /// <summary>
        /// 结束值
        /// </summary>
        [DisplayName("结束值")]
        [DBField]
        public string VALUE_TO { get; set; }


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
        [DBField, DefaultValue("(GetDate())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField,DefaultValue("(GetDate())")]
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
        /// 记录排序
        /// </summary>
        [DBField]
        [DisplayName("记录排序")]
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
