using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace EC5.BizLogger.Model
{
    /// <summary>
    /// 日志-影响的表数据
    /// </summary>
    [DBTable("LOG_ACT_OPDATA")]
    [Description("日志-影响的表数据")]
    [Serializable]
    public class LOG_ACT_OPDATA : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 LOG_ACT_OPDATA_ID { get; set; }

        /// <summary>
        /// 上上级ID
        /// </summary>
        [DBField]
        [DisplayName("上上级ID")]
        [UIField(visible = true)]
        public Int32 LOG_ACT_ID { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        [DBField]
        [DisplayName("上级Id")]
        [UIField(visible = true)]
        public Int32 LOG_ACT_OP_ID { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String C_TABLE { get; set; }

        /// <summary>
        /// 主键值
        /// </summary>
        [DBField]
        public string C_PK_VALUE { get; set; }

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
        /// 操作的数据内容，已XML 格式存放
        /// </summary>
        [DBField]
        [DisplayName("操作的数据内容，已XML 格式存放")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String OPDATE_XML { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [XmlIgnore]
        [DefaultValue("(GETDATE())")]
        [DBField]
        public DateTime ROW_DATE_CREATE { get; set; }
    }
}
