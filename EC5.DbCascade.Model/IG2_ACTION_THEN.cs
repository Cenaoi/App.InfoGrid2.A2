using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EC5.DbCascade.Model
{
    /// <summary>
    /// 条件判断
    /// </summary>
    [DBTable("IG2_ACTION_THEN")]
    [Description("条件判断")]
    [Serializable]
    public class IG2_ACTION_THEN : LightModel
    {

        /// <summary>
        /// 条件判断ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("条件判断ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_THEN_ID { get; set; }

        /// <summary>
        /// 关联操作
        /// </summary>
        [DBField]
        [DisplayName("关联操作")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_ID { get; set; }


        /// <summary>
        /// 判断类型:COUNT-记录数量, FIRST_FIELD-第一行字段的某个值，TOTAL_FUN-统计函数
        /// </summary>
        [DBField]
        [DisplayName("判断类型:COUNT-记录数量, FIRST_FIELD-第一行字段的某个值，TOTAL_FUN-统计函数")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String A_TYPE_ID { get; set; }


        /// <summary>
        /// 字段描述
        /// </summary>
        [DBField]
        [DisplayName("字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_FIELD_TEXT { get; set; }


        /// <summary>
        /// 内部字段名
        /// </summary>
        [DBField]
        [DisplayName("内部字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_FIELD { get; set; }


        /// <summary>
        /// 逻辑
        /// </summary>
        [DBField]
        [DisplayName("逻辑")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String A_LOGIN { get; set; }


        /// <summary>
        /// 值
        /// </summary>
        [DBField]
        [DisplayName("值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_VALUE { get; set; }


        /// <summary>
        /// 统计函数
        /// </summary>
        [DBField]
        [DisplayName("统计函数")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String A_TOTAL_FUN { get; set; }

        /// <summary>
        /// 是否停止继续执行
        /// </summary>
        [DBField]
        [DisplayName("是否停止继续执行")]
        [UIField(visible = true)]
        public Boolean IS_STOP { get; set; }


        /// <summary>
        /// 返回的消息
        /// </summary>
        [DBField]
        [DisplayName("返回的消息")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String RSULT_MESSAGE { get; set; }


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
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
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
        /// 记录自定义排序
        /// </summary>
        [DBField]
        [DisplayName("记录自定义排序")]
        [UIField(visible = true)]
        public Decimal ROW_USER_SEQ { get; set; }

        #endregion
    }

}
