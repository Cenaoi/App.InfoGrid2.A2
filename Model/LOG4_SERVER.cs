using HWQ.Entity.LightModels;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 服务器端的日志
    /// </summary>
    [DBTable("LOG4_SERVER")]
    [ProtoContract]
    [Description("服务器端的日志")]
    [Serializable]
    public class LOG4_SERVER : LightModel
    {

        /// <summary>
        /// 日志ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("日志ID")]
        [UIField(visible = true)]
        public Int32 LOG4_SERVER_ID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField]
        [Description("创建时间")]
        [UIField(visible = true)]
        [ProtoMember(1)]
        public DateTime LOG_DATE { get; set; }


        /// <summary>
        /// 线程
        /// </summary>
        [DBField]
        [Description("线程")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        [ProtoMember(2)]
        public String THREAD { get; set; }


        /// <summary>
        /// 日志级别
        /// </summary>
        [DBField]
        [Description("日志级别")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        [ProtoMember(3)]
        public String LOG4_LEVEL { get; set; }


        /// <summary>
        /// 出错误类名
        /// </summary>
        [DBField]
        [Description("出错误类名")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        [ProtoMember(4)]
        public String LOGGER { get; set; }


        /// <summary>
        /// 错误消息
        /// </summary>
        [DBField( StringEncoding.UTF8,true)]
        [Description("错误消息")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        [ProtoMember(5)]
        public String MESSAGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [UIField(visible = true)]
        [ProtoMember(6)]
        public Int32 ACTION_TYPE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [LMValidate(maxLen = 300)]
        [UIField(visible = true)]
        [ProtoMember(7)]
        public String OPERAND { get; set; }


        /// <summary>
        /// IP地址
        /// </summary>
        [DBField]
        [Description("IP地址")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        [ProtoMember(8)]
        public String IP { get; set; }


        /// <summary>
        /// 服务器地址
        /// </summary>
        [DBField]
        [Description("服务器地址")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        [ProtoMember(9)]
        public String MACHINE_NAME { get; set; }


        /// <summary>
        /// 浏览器版本
        /// </summary>
        [DBField]
        [Description("浏览器版本")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        [ProtoMember(10)]
        public String BROWSER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        [ProtoMember(11)]
        public String LOCATION { get; set; }


        /// <summary>
        /// 具体错误消息
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [Description("具体错误消息")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        [ProtoMember(12)]
        public String EXCEPTION { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DBField]
        [Description("用户ID")]
        [UIField(visible = true)]
        [ProtoMember(13)]
        public Int32 U_ID { get; set; }


        /// <summary>
        /// 用户登录名
        /// </summary>
        [DBField]
        [Description("用户登录名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        [ProtoMember(14)]
        public String U_LOGIN { get; set; }

        /// <summary>
        /// 用户登录时间
        /// </summary>
        [DBField]
        [Description("用户登录时间")]
        [UIField(visible = true)]
        [ProtoMember(15)]
        public DateTime? U_TIME_LOGIN { get; set; }


        /// <summary>
        /// 页面路径
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [Description("页面路径")]
        [ProtoMember(16)]
        public String URL { get; set; }


        /// <summary>
        /// sessionID
        /// </summary>
        [DBField]
        [Description("sessionID")]
        [ProtoMember(17)]
        public String SESSION_ID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DBField]
        [Description("公司名称")]
        [ProtoMember(18)]
        public String COMPANY_NAME { get; set; }


        /// <summary>
        /// 提交上服务器  0--未提交，2--已提交
        /// </summary>
        [DBField]
        [Description("提交上服务器  0--未提交，2--已提交")]
        [ProtoMember(19)]
        public Int32 BIZ_SID { get; set; }





    }
}
