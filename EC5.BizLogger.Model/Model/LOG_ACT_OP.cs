using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace EC5.BizLogger.Model
{
    /// <summary>
    /// 日志-影响的表
    /// </summary>
    [DBTable("LOG_ACT_OP")]
    [Description("日志-影响的表")]
    [Serializable]
    public class LOG_ACT_OP : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 LOG_ACT_OP_ID { get; set; }

        /// <summary>
        /// 日志-动作ID
        /// </summary>
        [DBField]
        [DisplayName("日志-动作ID")]
        [UIField(visible = true)]
        public Int32 LOG_ACT_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }


        /// <summary>
        /// 操作名称
        /// </summary>
        [DBField]
        [DisplayName("操作名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ACT_CODE { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String C_TABLE { get; set; }


        /// <summary>
        /// 表描述
        /// </summary>
        [DBField]
        [DisplayName("表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String C_DISPLAY { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        [DBField]
        [DisplayName("执行时间")]
        [UIField(visible = true)]
        public Int32 TIME_EXEC { get; set; }

        /// <summary>
        /// 采用的联动ID
        /// </summary>
        [DBField]
        [DisplayName("采用的联动ID")]
        [UIField(visible = true)]
        public Int32 ACTION_ID { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        [DBField]
        [DisplayName("深度")]
        [UIField(visible = true)]
        public Int32 DEPTH { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        [DBField]
        [XmlIgnore]
        [DefaultValue("(GETDATE())")]
        [DisplayName("执行开始时间")]
        [UIField(visible = true)]
        public DateTime DATE_EXEC_FROM { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        [XmlIgnore]
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("执行结束时间")]
        [UIField(visible = true)]
        public DateTime DATE_EXEC_TO { get; set; }

        /// <summary>
        /// 反馈的消息信息
        /// </summary>
        [XmlIgnore]
        [Description("反馈的消息信息")]
        [DBField]
        public string RESULT_MESSAGE { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [XmlIgnore]
        [Description("记录创建时间")]
        [DefaultValue("(GETDATE())")]
        [DBField]
        public DateTime ROW_DATE_CREATE { get; set; }

    }

}
