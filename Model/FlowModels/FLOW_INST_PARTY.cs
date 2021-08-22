using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 流程实例当事人
    /// </summary>
    [DBTable("FLOW_INST_PARTY")]
    [Description("流程实例当事人")]
    [Serializable]
    public class FLOW_INST_PARTY : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_INST_PARTY_ID { get; set; }

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
        public string INST_STEP_CODE { get; set; }


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
        [DisplayName("流程节点名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DEF_NODE_TEXT { get; set; }


        /// <summary>
        /// 业务状态...0-未审核,2-已审核
        /// </summary>
        [DBField]
        [DisplayName("业务状态...0-未审核,2-已审核")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }


        /// <summary>
        /// 建议内容
        /// </summary>
        [DBField]
        [DisplayName("建议内容")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COMMENT { get; set; }

        /// <summary>
        /// 提交标识状态. 用来排序...
        /// 值越大, 排列在最前面;
        /// </summary>
        [DBField]
        public int TAG_SID { get; set; }

        /// <summary>
        /// 当事人类型
        /// </summary>
        [DBField]
        [DisplayName("当事人类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARTY_TYPE { get; set; }


        /// <summary>
        /// 提交时间
        /// </summary>
        [DBField]
        public DateTime? DATE_SUBMIT { get; set; }



        /// <summary>
        /// 用户编码
        /// </summary>
        [DBField]
        [DisplayName("用户编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_USER_CODE { get; set; }


        /// <summary>
        /// 用户名称
        /// </summary>
        [DBField]
        [DisplayName("用户名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String P_USER_TEXT { get; set; }


        /// <summary>
        /// 部门编码
        /// </summary>
        [DBField]
        [DisplayName("部门编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_ORG_CODE { get; set; }


        /// <summary>
        /// 部门名称
        /// </summary>
        [DBField]
        [DisplayName("部门名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String P_ORG_TEXT { get; set; }


        /// <summary>
        /// 公司编码
        /// </summary>
        [DBField]
        [DisplayName("公司编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_COMP_CODE { get; set; }


        /// <summary>
        /// 公司名称
        /// </summary>
        [DBField]
        [DisplayName("公司名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String P_COMP_TEXT { get; set; }


        /// <summary>
        /// 角色编码
        /// </summary>
        [DBField]
        [DisplayName("角色编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String P_ROLE_CODE { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [DBField]
        [DisplayName("角色名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String P_ROLE_TEXT { get; set; }

        /// <summary>
        /// 记录状态Id
        /// </summary>
        [DBField]
        [DisplayName("记录状态Id")]
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
        /// 提交的加急标记, 配合 TAG_SID .
        /// </summary>
        [DBField]
        public int SUBMIT_TAG_SID { get; set; }
    }

}
