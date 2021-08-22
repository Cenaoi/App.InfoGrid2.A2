using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DBTable("LOG_DB_TRACE", DBTableMode.Virtual)]
    [Description("")]
    [Serializable]
    public class LOG_DB_TRACE :LightModel
    {

        /// <summary>
        /// id
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("id")]
        [UIField(visible = true)]
        public Int64 LOG_DB_TRACE_ID { get; set; }


        /// <summary>
        /// 操作表名
        /// </summary>
        [DBField]
        [DisplayName("操作表名")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String DB_TABLE { get; set; }


        /// <summary>
        /// 操作方法
        /// </summary>
        [DBField]
        [DisplayName("操作方法")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DB_METHOD { get; set; }


        /// <summary>
        /// 操作内容
        /// </summary>
        [DBField]
        [DisplayName("操作内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String OP_CONTENT { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        [DBField]
        [DisplayName("执行时间")]
        [UIField(visible = true)]
        public decimal EXEC_TIMESPAN { get; set; }


        /// <summary>
        /// 更新的字段集合
        /// </summary>
        [DBField]
        [DisplayName("更新的字段集合")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String UPDATE_FIELDS { get; set; }

        /// <summary>
        /// 执行的开始时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("执行的开始时间")]
        [UIField(visible = true)]
        public DateTime OP_TIME { get; set; }


        /// <summary>
        /// 用户id
        /// </summary>
        [DBField]
        [DisplayName("用户id")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String LOGIN_ID { get; set; }


        /// <summary>
        /// 用户编码
        /// </summary>
        [DBField]
        [DisplayName("用户编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String USER_CODE { get; set; }


        /// <summary>
        /// 用户真实名称
        /// </summary>
        [DBField]
        [DisplayName("用户真实名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String LOING_NAME { get; set; }


        /// <summary>
        /// session_id
        /// </summary>
        [DBField]
        [DisplayName("session_id")]
        [LMValidate(maxLen = 40)]
        [UIField(visible = true)]
        public String SESSION_ID { get; set; }


        /// <summary>
        /// 线程名称
        /// </summary>
        [DBField]
        [DisplayName("线程名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String THREAD { get; set; }


        /// <summary>
        /// 访问的地址
        /// </summary>
        [DBField]
        [DisplayName("访问的地址")]
        [LMValidate(maxLen = 1000)]
        [UIField(visible = true)]
        public String URL { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }


        /// <summary>
        /// 浏览器版本
        /// </summary>
        [DBField]
        [DisplayName("浏览器版本")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String BROWSER { get; set; }


        /// <summary>
        /// 服务器ip
        /// </summary>
        [DBField]
        [DisplayName("服务器ip")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String HOST { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

    }

}
