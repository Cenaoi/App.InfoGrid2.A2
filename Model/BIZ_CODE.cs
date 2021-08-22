using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 编码管理
    /// </summary>
    [DBTable("BIZ_CODE")]
    [Description("编码管理")]
    [Serializable]
    public class BIZ_CODE : LightModel
    {

        public BIZ_CODE()
        {
            this.NUM_END = 99999999;
        }

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_CODE_ID { get; set; }


        /// <summary>
        /// 组名称，例：进货单，出货单。。。
        /// </summary>
        [DBField]
        [Description("组名称，例：进货单，出货单。。。")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String GROUP_NAME { get; set; }


        /// <summary>
        /// 组代码，用于给程序调用
        /// </summary>
        [DBField]
        [Description("组代码，用于给程序调用")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String T_CODE { get; set; }

        /// <summary>
        /// 模式：自动，年，年月，年月日
        /// </summary>
        [DBField]
        public string MODE_ID { get; set; }


        /// <summary>
        /// 格式化显示.
        /// </summary>
        [DBField]
        [Description("格式化显示.")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String T_FORMAT { get; set; }


        /// <summary>
        /// 编码前缀
        /// </summary>
        [DBField]
        [Description("编码前缀")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CODE_PRDFIX { get; set; }


        /// <summary>
        /// 编码后缀
        /// </summary>
        [DBField]
        [Description("编码后缀")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CODE_SUFFIX { get; set; }

        /// <summary>
        /// 起始值
        /// </summary>
        [DBField]
        [Description("起始值")]
        [UIField(visible = true)]
        public Int32 NUM_START { get; set; }

        /// <summary>
        /// 终止值
        /// </summary>
        [DBField]
        [Description("终止值")]
        [UIField(visible = true)]
        public Int32 NUM_END { get; set; }

        /// <summary>
        /// 递增数量
        /// </summary>
        [DBField]
        public int NUM_ADD { get; set; }


        /// <summary>
        /// 当前值
        /// </summary>
        [DBField]
        [Description("当前值")]
        [UIField(visible = true)]
        public Int32 NUM_CUR { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [Description("记录状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [Description("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
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


        /// <summary>
        /// 编码标记
        /// </summary>
        [DBField]
        [Description("编码标记")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATALOG_TEXT { get; set; }





    }
}
