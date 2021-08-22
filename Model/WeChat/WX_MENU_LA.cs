using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.WeChat
{
    /// <summary>
    /// 微信自定义菜单子表
    /// </summary>
    [DBTable("WX_MENU_LA")]
    [Description("微信自定义菜单子表")]
    [Serializable]
    public class WX_MENU_LA : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 WX_MENU_LA_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_MENU_LA_CODE { get; set; }


        /// <summary>
        /// 外键  主表的自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("外键  主表的自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_MENU_CODE { get; set; }


        /// <summary>
        /// 菜单文字
        /// </summary>
        [DBField]
        [DisplayName("菜单文字")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MENU_TEXT { get; set; }


        /// <summary>
        /// 菜单类型
        /// </summary>
        [DBField]
        [DisplayName("菜单类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MENU_TYPE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String MENU_URL { get; set; }


        /// <summary>
        /// 点击类型
        /// </summary>
        [DBField]
        [DisplayName("点击类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CLICK_TYPE { get; set; }


        /// <summary>
        /// 菜单关键字用来给服务响应客户的请求的
        /// </summary>
        [DBField]
        [DisplayName("菜单关键字用来给服务响应客户的请求的")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MENU_KEY { get; set; }

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

    }
}
