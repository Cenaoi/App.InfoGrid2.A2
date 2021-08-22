using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 展示规则
    /// </summary>
    [DBTable("IG2_DISPLAY_RULE")]
    [Description("展示规则")]
    [Serializable]
    public class IG2_DISPLAY_RULE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 IG2_DISPLAY_RULE_ID { get; set; }


        /// <summary>
        /// 规则编码
        /// </summary>
        [DBField]
        [Description("规则编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String RULE_CODE { get; set; }


        /// <summary>
        /// 规则名称
        /// </summary>
        [DBField]
        [Description("规则名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String RULE_TEXT { get; set; }


        /// <summary>
        /// Html 样式
        /// </summary>
        [DBField]
        [Description("Html 样式")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        public String EX_CSS { get; set; }


        /// <summary>
        /// Html JS
        /// </summary>
        [DBField]
        [Description("Html JS")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        public String EX_JS { get; set; }

        /// <summary>
        /// 返回JS函数
        /// </summary>
        [DBField]
        [Description("返回JS函数")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        public String EX_RETURN_JS_FUN { get; set; }

        

        /// <summary>
        /// 规则是否激活
        /// </summary>
        [DBField]
        [Description("规则是否激活")]
        [UIField(visible = true)]
        public Boolean ENABLED { get; set; }

        /// <summary>
        /// 权限等级。
        /// </summary>
        [DBField]
        [Description("权限等级。")]
        [UIField(visible = true)]
        public Int32 SEC_LEVEL { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [Description("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [Description("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField]
        [Description("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField]
        [Description("记录更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除时间
        /// </summary>
        [DBField]
        [Description("记录删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }
}
