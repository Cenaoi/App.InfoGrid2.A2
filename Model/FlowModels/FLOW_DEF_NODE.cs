using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 流程节点
    /// </summary>
    [DBTable("FLOW_DEF_NODE")]
    [Description("流程节点")]
    [Serializable]
    public class FLOW_DEF_NODE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_DEF_NODE_ID { get; set; }

        /// <summary>
        /// 流程ID
        /// </summary>
        [DBField]
        [DisplayName("流程ID")]
        [UIField(visible = true)]
        public Int32 DEF_ID { get; set; }


        /// <summary>
        /// 流程编码
        /// </summary>
        [DBField]
        [DisplayName("流程编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DEF_CODE { get; set; }


        /// <summary>
        /// 节点类型. 正常节点, 判断节点, ...
        /// </summary>
        [DBField]
        [DisplayName("节点类型. 正常节点, 判断节点, ...")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String NODE_TYPE { get; set; }


        /// <summary>
        /// 节点编码
        /// </summary>
        [DBField]
        [DisplayName("节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String NODE_CODE { get; set; }


        /// <summary>
        /// 节点名称
        /// </summary>
        [DBField]
        [DisplayName("节点名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String NODE_TEXT { get; set; }

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
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String NODE_IDENTIFIER { get; set; }


        /// <summary>
        /// 人员参与: 全部参与,部分参与
        /// </summary>
        [DBField]
        [DisplayName("人员参与: 全部参与,部分参与")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_MODE_ID { get; set; }

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
        /// 会签通过最低人数
        /// </summary>
        [DBField]
        [DisplayName("会签通过最低人数")]
        public int P_CONSIGN_MIN_COUNT { get; set; }

        /// <summary>
        /// 会签时退回方式: 立即退回
        /// </summary>
        [DBField]
        [DisplayName("会签时退回方式: 立即退回")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_CONSIGN_BACK_TYPE { get; set; }

        /// <summary>
        /// 允许选择其他人员
        /// </summary>
        [DBField]
        [DisplayName("允许选择其他人员")]
        [UIField(visible = true)]
        public Boolean P_ALLOW_OTHER_PARTY { get; set; }

        /// <summary>
        /// 自动对应部门人员
        /// </summary>
        [DBField]
        [DisplayName("自动对应部门人员")]
        [UIField(visible = true)]
        public Boolean P_AUTO_ORG { get; set; }


        /// <summary>
        /// 消息类型:邮件...
        /// </summary>
        [DBField]
        [DisplayName("消息类型:邮件...")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String MSG_TYPE { get; set; }


        /// <summary>
        /// 新任务到达时标题
        /// </summary>
        [DBField]
        [DisplayName("新任务到达时标题")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MSG_NEW_TITLE { get; set; }


        /// <summary>
        /// 新任务到达时内容
        /// </summary>
        [DBField]
        [DisplayName("新任务到达时内容")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String MSG_NEW_CONTENT { get; set; }


        /// <summary>
        /// 重新分派标题
        /// </summary>
        [DBField]
        [DisplayName("重新分派标题")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MSG_RESET_TITLE { get; set; }


        /// <summary>
        /// 重新分派内容
        /// </summary>
        [DBField]
        [DisplayName("重新分派内容")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String MSG_RESET_CONTENT { get; set; }


        /// <summary>
        /// 收回时标题
        /// </summary>
        [DBField]
        [DisplayName("收回时标题")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MSG_RECYCLE_TITLE { get; set; }


        /// <summary>
        /// 收回时内容
        /// </summary>
        [DBField]
        [DisplayName("收回时内容")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String MSG_RECYCLE_CONTENT { get; set; }


        /// <summary>
        /// 审核完成时标题
        /// </summary>
        [DBField]
        [DisplayName("审核完成时标题")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MSG_CHECK_TITLE { get; set; }


        /// <summary>
        /// 审核完成时内容
        /// </summary>
        [DBField]
        [DisplayName("审核完成时内容")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String MSG_CHECK_CONTENT { get; set; }

        /// <summary>
        /// 显示提交流程按钮
        /// </summary>
        [DBField]
        [DisplayName("显示提交流程按钮")]
        [UIField(visible = true)]
        public Boolean BTN_SUBMIT_VISIBLE { get; set; }

        /// <summary>
        /// 显示暂存流程按钮
        /// </summary>
        [DBField]
        [DisplayName("显示暂存流程按钮")]
        [UIField(visible = true)]
        public Boolean BTN_TS_VISIBLE { get; set; }

        /// <summary>
        /// 显示退回首环节按钮
        /// </summary>
        [DBField]
        [DisplayName("显示退回首环节按钮")]
        [UIField(visible = true)]
        public Boolean BTN_BACKFIRST_VISIBLE { get; set; }

        /// <summary>
        /// 显示退回上一环节按钮
        /// </summary>
        [DBField]
        [DisplayName("显示退回上一环节按钮")]
        [UIField(visible = true)]
        public Boolean BTN_BACK_VISIBLE { get; set; }

        /// <summary>
        /// 显示回收流程按钮
        /// </summary>
        [DBField]
        [DisplayName("显示回收流程按钮")]
        [UIField(visible = true)]
        public Boolean BTN_RECYCLE_VISIBLE { get; set; }


        /// <summary>
        /// 意见类型
        /// </summary>
        [DBField]
        [DisplayName("意见类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String IDEA_TYPE { get; set; }


        /// <summary>
        /// 填写意见: 默认 "通过"
        /// </summary>
        [DBField]
        [DisplayName("填写意见: 默认 通过")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String IEEA_CONTENT { get; set; }

        /// <summary>
        /// 开启催办设置
        /// </summary>
        [DBField]
        [DisplayName("开启催办设置")]
        [UIField(visible = true)]
        public Boolean URGE_ENABLED { get; set; }

        /// <summary>
        /// 活动持续周期(小时) 72
        /// </summary>
        [DBField]
        [DisplayName("活动持续周期(小时) 72")]
        [UIField(visible = true)]
        public Int32 URGE_ACTION_HOUR { get; set; }

        /// <summary>
        /// 逾期后自动审核
        /// </summary>
        [DBField]
        [DisplayName("逾期后自动审核")]
        [UIField(visible = true)]
        public Boolean URGE_AUTO_CHECK { get; set; }

        /// <summary>
        /// 每隔多少分钟催办一次
        /// </summary>
        [DBField]
        [DisplayName("每隔多少分钟催办一次")]
        [UIField(visible = true)]
        public Int32 URGE_TIEMSPAN { get; set; }


        /// <summary>
        /// 催办标题
        /// </summary>
        [DBField]
        [DisplayName("催办标题")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String URGE_TITLE { get; set; }


        /// <summary>
        /// 催办内容
        /// </summary>
        [DBField]
        [DisplayName("催办内容")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String URGE_CONTENT { get; set; }


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


        /// <summary>
        /// 动作类型
        /// </summary>
        [DBField]
        public string ACTION_TYPE { get; set; }


        /// <summary>
        /// 活动脚本
        /// </summary>
        [DBField]
        public string ACTION_SCRIPT { get; set; }



        /// <summary>
        /// 动作类型
        /// </summary>
        [DBField]
        public string BACK_ACTION_TYPE { get; set; }


        /// <summary>
        /// 活动脚本
        /// </summary>
        [DBField]
        public string BACK_ACTION_SCRIPT { get; set; }

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

        #region 记录状态


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
