using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace EC5.BizLogger.Model
{
    /// <summary>
    /// 日志-动作
    /// </summary>
    [DBTable("LOG_ACT")]
    [Description("日志-动作")]
    [Serializable]
    public class LOG_ACT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 LOG_ACT_ID { get; set; }

        /// <summary>
        /// 执行总时间
        /// </summary>
        [DBField]
        [DisplayName("执行总时间")]
        [UIField(visible = true)]
        public Int32 TIME_EXEC { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("执行开始时间")]
        [UIField(visible = true)]
        [XmlIgnore]
        public DateTime DATE_EXEC_FROM { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("执行结束时间")]
        [UIField(visible = true)]
        [XmlIgnore]
        public DateTime DATE_EXEC_TO { get; set; }


        /// <summary>
        /// SessionID
        /// </summary>
        [DBField]
        [DisplayName("SessionID")]
        [LMValidate(maxLen = 32)]
        [UIField(visible = true)]
        public String SESSION_ID { get; set; }


        /// <summary>
        /// 操作名称
        /// </summary>
        [DBField]
        [DisplayName("操作名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String OP_TEXT { get; set; }


        /// <summary>
        /// 页面Url
        /// </summary>
        [DBField]
        [DisplayName("页面Url")]
        [LMValidate(maxLen = 255)]
        [UIField(visible = true)]
        public String PAGE_URL { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


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
        /// 记录创建时间
        /// </summary>
        [DefaultValue("(GETDATE())")]
        [DBField]
        [XmlIgnore]
        public DateTime ROW_DATE_CREATE { get; set; }

    }
}
