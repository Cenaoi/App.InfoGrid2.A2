using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 打印客户端日志
    /// </summary>
    [DBTable("LOG_PRINT_CLIENT")]
    [Description("打印客户端日志")]
    [Serializable]
    public class LOG_PRINT_CLIENT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 LOG_PRINT_CLIENT_ID { get; set; }


        /// <summary>
        /// 客户端名称|GUID|IP地址
        /// </summary>
        [DBField]
        [Description("客户端名称|GUID|IP地址")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CLIENT_NAME { get; set; }


        /// <summary>
        /// 错误消息
        /// </summary>
        [DBField]
        [Description("错误消息")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String LOG_MESSAGE { get; set; }


        /// <summary>
        /// ERROR|DEBUG
        /// </summary>
        [DBField]
        [Description("ERROR|DEBUG")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String LOG_TYPE { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [Description("创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

    }
}
