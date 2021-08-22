using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 流程定义
    /// </summary>
    [DBTable("FLOW_DEF")]
    [Description("流程定义")]
    [Serializable]
    public class FLOW_DEF : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_DEF_ID { get; set; }


        /// <summary>
        /// 流程编码
        /// </summary>
        [DBField]
        [DisplayName("流程编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DEF_CODE { get; set; }


        /// <summary>
        /// 流程名称
        /// </summary>
        [DBField]
        [DisplayName("流程名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DEF_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

        /// <summary>
        /// 健康指标 n% 以上超时属于不健康状态
        /// </summary>
        [DBField]
        [DisplayName("健康指标 n% 以上超时属于不健康状态")]
        [UIField(visible = true)]
        public Int32 EI_JK_BFB { get; set; }

        /// <summary>
        /// 优秀指标 n 小时内完成审核
        /// </summary>
        [DBField]
        [DisplayName("优秀指标 n 小时内完成审核")]
        [UIField(visible = true)]
        public Int32 EI_YX { get; set; }

        /// <summary>
        /// 良好指标 n 小时内完成审核
        /// </summary>
        [DBField]
        [DisplayName("良好指标 n 小时内完成审核")]
        [UIField(visible = true)]
        public Int32 EI_LH { get; set; }

        /// <summary>
        /// 合格指标 n 小时内完成审核
        /// </summary>
        [DBField]
        [DisplayName("合格指标 n 小时内完成审核")]
        [UIField(visible = true)]
        public Int32 EI_HG { get; set; }

        /// <summary>
        /// 严重超标 n 小时内未完成审核
        /// </summary>
        [DBField]
        [DisplayName("严重超标 n 小时内未完成审核")]
        [UIField(visible = true)]
        public Int32 EI_YZ { get; set; }


        /// <summary>
        /// 版本号
        /// </summary>
        [DBField]
        [DisplayName("版本号")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String V_VERSION { get; set; }


        /// <summary>
        /// 作者编码
        /// </summary>
        [DBField]
        [DisplayName("作者编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String AUTHOR_CODE { get; set; }


        /// <summary>
        /// 作者名称
        /// </summary>
        [DBField]
        [DisplayName("作者名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String AUTHOR_TEXT { get; set; }

        /// <summary>
        /// 流程创建时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("流程创建时间")]
        [UIField(visible = true)]
        public DateTime BIZ_DATE_CREATE { get; set; }

        /// <summary>
        /// 流程更新时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("流程更新时间")]
        [UIField(visible = true)]
        public DateTime BIZ_DATE_UPDATE { get; set; }

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
        /// 图形
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("图形")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String GRAPHICS { get; set; }


        /// <summary>
        /// 控制当前流程内节点的 ID 唯一值
        /// </summary>
        [DBField]
        public int N_NODE_IDENTITY { get; set; }


        /// <summary>
        /// 控制当前流程内线段你的ID
        /// </summary>
        [DBField]
        public int N_LINE_IDENTITY { get; set; }


        /// <summary>
        /// 权限结构编码
        /// </summary>
        [DBField]
        public string SEC_STRUCE_CODE { get; set; }

    }
}
