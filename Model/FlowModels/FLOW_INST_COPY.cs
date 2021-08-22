using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 抄送的流程
    /// </summary>
    [DBTable("FLOW_INST_COPY")]
    [Description("抄送的流程")]
    [Serializable]
    public class FLOW_INST_COPY : LightModel
    {

        /// <summary>
        /// 抄送的流程
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 FLOW_INST_COPY_ID { get; set; }

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
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String INST_STEP_CODE { get; set; }

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
        /// 当事人类型
        /// </summary>
        [DBField]
        [DisplayName("当事人类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARTY_TYPE { get; set; }


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
        /// 流程状态
        /// </summary>
        [DBField]
        [DisplayName("流程状态")]
        [UIField(visible = true)]
        public Int32 FLOW_SID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 EXTEND_PAGE_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EXTEND_ROW_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 EXTEND_MENU_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EXTEND_MENU_IDENTIFIER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String EXTEND_DOC_URL { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EXTEND_DOC_TEXT { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EXTEND_BILL_TYPE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EXTEND_BILL_CODE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EXTEND_BILL_CODE_FIELD { get; set; }

        /// <summary>
        /// 是否已经打开查看
        /// </summary>
        [DBField]
        public bool IS_OPEN { get; set; }

        /// <summary>
        /// 打开的时间
        /// </summary>
        [DBField]
        public DateTime? OPEN_DATE { get; set; }



        /// <summary>
        /// 扩展字段1
        /// </summary>
        [DBField]
        public string EXT_COL_VALUE_1 { get; set; }

        /// <summary>
        /// 扩展字段2
        /// </summary>
        [DBField]
        public string EXT_COL_VALUE_2 { get; set; }

        /// <summary>
        /// 扩展字段3
        /// </summary>
        [DBField]
        public string EXT_COL_VALUE_3 { get; set; }

        /// <summary>
        /// 扩展字段4
        /// </summary>
        [DBField]
        public string EXT_COL_VALUE_4 { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        [DBField]
        public string EXT_COL_1 { get; set; }

        /// <summary>
        /// 扩展字段2
        /// </summary>
        [DBField]
        public string EXT_COL_2 { get; set; }

        /// <summary>
        /// 扩展字段3
        /// </summary>
        [DBField]
        public string EXT_COL_3 { get; set; }

        /// <summary>
        /// 扩展字段4
        /// </summary>
        [DBField]
        public string EXT_COL_4 { get; set; }


        /// <summary>
        /// 扩展字段的标题
        /// </summary>
        [DBField]
        public string EXT_COL_TEXT_1 { get; set; }
        /// <summary>
        /// 扩展字段的标题
        /// </summary>
        [DBField]
        public string EXT_COL_TEXT_2 { get; set; }
        /// <summary>
        /// 扩展字段的标题
        /// </summary>
        [DBField]
        public string EXT_COL_TEXT_3 { get; set; }
        /// <summary>
        /// 扩展字段的标题
        /// </summary>
        [DBField]
        public string EXT_COL_TEXT_4 { get; set; }


        /// <summary>
        /// 流程发起人编码
        /// </summary>
        [DBField]
        public string START_USER_CODE { get; set; }

        /// <summary>
        /// 流程发起人
        /// </summary>
        [DBField]
        public string START_USER_TEXT { get; set; }


        #region 记录常规字段

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

        #endregion
    }
}
