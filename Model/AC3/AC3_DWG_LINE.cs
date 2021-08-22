using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 图形线段
    /// </summary>
    [DBTable("AC3_DWG_LINE")]
    [Description("图形线段")]
    [Serializable]
    public class AC3_DWG_LINE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 AC3_DWG_LINE_ID { get; set; }


        /// <summary>
        /// 图纸编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("图纸编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_DWG_CODE { get; set; }


        /// <summary>
        /// 线段编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("线段编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_LINE_CODE { get; set; }


        /// <summary>
        /// 线段编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("线段唯一编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String LINE_IDENTIFIER { get; set; }


        /// <summary>
        /// 线段名称
        /// </summary>
        [DBField]
        [DisplayName("线段名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LINE_TEXT { get; set; }


        /// <summary>
        /// 开始节点编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("开始节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FROM_NODE_CODE { get; set; }


        /// <summary>
        /// 开始节点名称
        /// </summary>
        [DBField]
        [DisplayName("开始节点名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FROM_NODE_TEXT { get; set; }


        /// <summary>
        /// 结束节点编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("结束节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TO_NODE_CODE { get; set; }


        /// <summary>
        /// 结束节点名称
        /// </summary>
        [DBField]
        [DisplayName("结束节点名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TO_NODE_TEXT { get; set; }


        /// <summary>
        /// 样式名称
        /// </summary>
        [DBField]
        [DisplayName("样式名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STYLE_NAME { get; set; }


        /// <summary>
        /// 图形命名空间
        /// </summary>
        [DBField]
        [DisplayName("图形命名空间")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String G_FULLNAME { get; set; }


        /// <summary>
        /// 路由类型
        /// </summary>
        [DBField(20, StringEncoding.ASCII,false)]
        [DisplayName("路由类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ROUTES_TYPE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 图形
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("图形")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String GRAPHICS { get; set; }

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
        /// 删除的批次编码
        /// </summary>
        [DBField]
        public string ROW_DELETE_BATCH_CODE { get; set; }

    }
}
