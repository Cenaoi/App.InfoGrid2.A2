using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 流程线段
    /// </summary>
    [DBTable("FLOW_DEF_LINE")]
    [Description("流程线段")]
    [Serializable]
    public class FLOW_DEF_LINE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_DEF_LINE_ID { get; set; }

        /// <summary>
        /// 流程定义ID
        /// </summary>
        [DBField]
        [DisplayName("流程定义ID")]
        [UIField(visible = true)]
        public Int32 DEF_ID { get; set; }


        /// <summary>
        /// 流程定义编码
        /// </summary>
        [DBField]
        [DisplayName("流程定义编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DEF_CODE { get; set; }


        /// <summary>
        /// 线段编码
        /// </summary>
        [DBField]
        [DisplayName("线段编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String LINE_CODE { get; set; }


        /// <summary>
        /// 线段名称
        /// </summary>
        [DBField]
        [DisplayName("线段名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LINE_TEXT { get; set; }


        /// <summary>
        /// 路由类型
        /// </summary>
        [DBField]
        [DisplayName("路由类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ROUTES_TYPE { get; set; }

        /// <summary>
        /// 起始节点ID
        /// </summary>
        [DBField]
        [DisplayName("起始节点ID")]
        [UIField(visible = true)]
        public Int32 FROM_NODE_ID { get; set; }


        /// <summary>
        /// 起始节点编码
        /// </summary>
        [DBField]
        [DisplayName("起始节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FROM_NODE_CODE { get; set; }


        /// <summary>
        /// 起始节点名称
        /// </summary>
        [DBField]
        [DisplayName("起始节点名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FROM_NODE_TEXT { get; set; }

        /// <summary>
        /// 结束节点ID
        /// </summary>
        [DBField]
        [DisplayName("结束节点ID")]
        [UIField(visible = true)]
        public Int32 TO_NODE_ID { get; set; }


        /// <summary>
        /// 结束节点编码
        /// </summary>
        [DBField]
        [DisplayName("结束节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TO_NODE_CODE { get; set; }


        /// <summary>
        /// 结束节点名称
        /// </summary>
        [DBField]
        [DisplayName("结束节点名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TO_NODE_TEXT { get; set; }


        /// <summary>y
        /// 样式名称
        /// </summary>
        [DBField]
        [DisplayName("样式名称")]
        public string STYLE_NAME { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        [DBField]
        [DisplayName("记录ID")]
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
        /// 图形
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("图形")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String GRAPHICS { get; set; }

        /// <summary>
        /// 图形的命名函数
        /// </summary>
        [DBField]
        public string G_FULLNAME { get; set; }

    }
}
