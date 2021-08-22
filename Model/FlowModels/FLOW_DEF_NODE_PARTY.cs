using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 节点当事人
    /// </summary>
    [DBTable("FLOW_DEF_NODE_PARTY")]
    [Description("节点当事人")]
    [Serializable]
    public class FLOW_DEF_NODE_PARTY : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_DEF_NODE_PARTY_ID { get; set; }

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
        /// 节点名称
        /// </summary>
        [DBField]
        [DisplayName("节点名称")]
        [UIField(visible = true)]
        public Int32 NODE_ID { get; set; }


        /// <summary>
        /// 节点编码
        /// </summary>
        [DBField]
        [DisplayName("节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String NODE_CODE { get; set; }


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

    }
}
