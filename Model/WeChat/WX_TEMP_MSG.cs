using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.WeChat
{
    /// <summary>
    /// 模板消息表
    /// </summary>
    [DBTable("WX_TEMP_MSG")]
    [Description("模板消息表")]
    [Serializable]
    public class WX_TEMP_MSG : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 WX_TEMP_MSG_ID { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
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
        /// 微信用户唯一标示
        /// </summary>
        [DBField]
        [DisplayName("微信用户唯一标示")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_OPENID { get; set; }


        /// <summary>
        /// 模板消息内容
        /// </summary>
        [DBField]
        [DisplayName("模板消息内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String TMEP_MSG_DATA { get; set; }


        /// <summary>
        /// 外键，微信用户编码
        /// </summary>
        [DBField]
        [DisplayName("外键，微信用户编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_W_CODE { get; set; }


        /// <summary>
        /// 发送模板消息返回的数据
        /// </summary>
        [DBField]
        [DisplayName("发送模板消息返回的数据")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String RETURN_DATA { get; set; }

    }

}
