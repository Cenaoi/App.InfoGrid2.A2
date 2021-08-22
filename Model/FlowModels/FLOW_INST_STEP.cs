using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 流程步骤
    /// </summary>
    [DBTable("FLOW_INST_STEP")]
    [Description("流程步骤")]
    [Serializable]
    public class FLOW_INST_STEP : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_INST_STEP_ID { get; set; }

        /// <summary>
        /// 流程实例ID
        /// </summary>
        [DBField]
        [DisplayName("流程实例ID")]
        [UIField(visible = true)]
        public Int32 INST_ID { get; set; }


        /// <summary>
        /// 流程实例编码
        /// </summary>
        [DBField]
        [DisplayName("流程实例编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String INST_CODE { get; set; }

        /// <summary>
        /// 步骤编码
        /// </summary>
        [DBField]
        [DisplayName("步骤编码")]
        public string INST_STEP_CODE { get; set; }

        /// <summary>
        /// 步骤顺序
        /// </summary>
        [DBField]
        [DisplayName("步骤顺序")]
        public int SEQ_NUM { get; set; }

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
        /// 流程节点ID
        /// </summary>
        [DBField]
        [DisplayName("流程节点ID")]
        [UIField(visible = true)]
        public Int32 DEF_NODE_ID { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        [DBField]
        [Description("节点类型")]
        public string DEF_NODE_TYPE { get; set; }

        /// <summary>
        /// 流程节点编码
        /// </summary>
        [DBField]
        [DisplayName("流程节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DEF_NODE_CODE { get; set; }


        /// <summary>
        /// 流程节点名称
        /// </summary>
        [DBField]
        [DisplayName("流程节点名")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DEF_NODE_TEXT { get; set; }


        /// <summary>
        /// 步骤状态ID;
        /// 2-审核中. 4-已审核
        /// </summary>
        [DBField]
        [DisplayName("步骤状态ID")]
        [UIField(visible = true)]
        public Int32 STEP_SID { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        public DateTime DATE_START { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DBField]
        public DateTime? DATE_END { get; set; }

        /// <summary>
        /// 最大值的 TAG_SID .
        /// </summary>
        [DBField]
        public int MAX_TAG_SID { get; set; }

        /// <summary>
        /// 是否会签
        /// </summary>
        [DBField]
        [DisplayName("是否会签")]
        [UIField(visible = true)]
        public Boolean P_IS_CONSIGN { get; set; }


        /// <summary>
        /// 会签类型
        /// </summary>
        [DBField]
        [DisplayName("会签类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_CONSIGN_TYPE { get; set; }



        /// <summary>
        /// 会签通过最低人数(作为参考,不参与业务逻辑计算使用)
        /// </summary>
        [DBField]
        [DisplayName("会签通过最低人数")]
        public int P_CONSIGN_MIN_COUNT { get; set; }

        /// <summary>
        /// 必须多少人会签
        /// </summary>
        [DBField]
        [DisplayName("必须多少人会签")]
        [UIField(visible = true)]
        public Int32 P_MEET_COUNT { get; set; }

        /// <summary>
        /// 当前已经签署数量
        /// </summary>
        [DBField]
        [DisplayName("当前已经签署数量")]
        [UIField(visible = true)]
        public Int32 P_CUR_COUNT { get; set; }

        /// <summary>
        /// 剩余多少人未签署
        /// </summary>
        [DBField]
        [DisplayName("剩余多少人未签署")]
        [UIField(visible = true)]
        public Int32 P_SURPLUS_COUNT { get; set; }


        /// <summary>
        /// 操作审核的描述信息
        /// </summary>
        [DBField]
        public string OP_CHECK_DESC { get; set; }

        /// <summary>
        /// 审核人的建议内容
        /// </summary>
        [DBField]
        public string OP_CHECK_COMMENTS { get; set; }


        /// <summary>
        /// 执行动态代码,反馈的结果
        /// </summary>
        [DBField]
        public string ACTION_RESULT { get; set; }


        /// <summary>
        /// 执行回滚后, 反馈的结果
        /// </summary>
        [DBField]
        public string BACK_ACTION_RESULT { get; set; }

        /// <summary>
        /// 上一个步骤的编码
        /// </summary>
        [DBField]
        [DisplayName("上一个步骤的编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PREV_STEP_CODE { get; set; }

        /// <summary>
        /// 来源的线段编码
        /// </summary>
        [DBField]
        public string FROM_LINE_CODE { get; set; }

        /// <summary>
        /// 来源的线段名称
        /// </summary>
        [DBField]
        public string FROM_LINE_TEXT { get; set; }

        /// <summary>
        /// 来源的节点编码
        /// </summary>
        [DBField]
        public string FROM_NODE_CODE { get; set; }

        /// <summary>
        /// 来源的节点名称
        /// </summary>
        [DBField]
        public string FROM_NODE_TEXT { get; set; }

        /// <summary>
        /// 来源的节点类型
        /// </summary>
        [DBField]
        public string FROM_NODE_TYPE { get; set; }


        /// <summary>
        /// 退回的意见
        /// </summary>
        [DBField]
        [DisplayName("退回的意见")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String BACK_COMMENT { get; set; }

        /// <summary>
        /// 退回的
        /// </summary>
        [DBField]
        [DisplayName("退回操作")]
        [UIField(visible = true)]
        public Boolean IS_BACK_OPERATE { get; set; }

        /// <summary>
        /// 回收的
        /// </summary>
        [DBField]
        [DisplayName("回收的操作")]
        [UIField(visible = true)]
        public Boolean IS_RECYCLE_OPERATE { get; set; }

        /// <summary>
        /// 作废的节点
        /// </summary>
        [DBField]
        [DisplayName("作废的节点")]
        [UIField(visible = true)]
        public Boolean IS_REVOKED { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        [DBField]
        [DisplayName("作废时间")]
        [UIField(visible = true)]
        public DateTime? REVOKED_DATE { get; set; }


        /// <summary>
        /// 作废的批操作编码
        /// </summary>
        [DBField]
        [DisplayName("作废的批操作编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String REVOKED_OP_BATCH_CODE { get; set; }


        /// <summary>
        /// 作废的作者编码
        /// </summary>
        [DBField]
        [DisplayName("作废的作者编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String REVOKED_PARTY_CODE { get; set; }


        /// <summary>
        /// 作废的作者名称
        /// </summary>
        [DBField]
        [DisplayName("作废的作者名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String REVOKED_PARTY_TEXT { get; set; }


        /// <summary>
        /// 作废的触发源节点(编码)
        /// </summary>
        [DBField]
        [DisplayName("作废的触发源节点(编码)")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String REVOKED_TRIGGER_NODE_CODE { get; set; }


        /// <summary>
        /// 作废的触发源节点(名称)
        /// </summary>
        [DBField]
        [DisplayName("作废的触发源节点(名称)")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String REVOKED_TRIGGER_NODE_TEXT { get; set; }

        /// <summary>
        /// 激活抄送
        /// </summary>
        [DBField]
        public bool IS_COPY_ENABLED { get; set; }

        /// <summary>
        /// 抄送给发起人
        /// </summary>
        [DBField]
        public bool COPY_TO_START_PARTY_ENABLED { get; set; }

        #region 记录常规字段

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


        #endregion
    }

}
