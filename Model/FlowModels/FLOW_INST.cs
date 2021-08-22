using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.FlowModels
{
    /// <summary>
    /// 流程实例
    /// </summary>
    [DBTable("FLOW_INST")]
    [Description("流程实例")]
    [Serializable]
    public class FLOW_INST : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 FLOW_INST_ID { get; set; }

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
        /// 实例编码
        /// </summary>
        [DBField]
        [DisplayName("实例编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String INST_CODE { get; set; }


        /// <summary>
        /// 实例名称
        /// </summary>
        [DBField]
        [DisplayName("实例名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String INST_TEXT { get; set; }

        /// <summary>
        /// 当前-节点ID
        /// </summary>
        [DBField]
        [DisplayName("当前-节点ID")]
        [UIField(visible = true)]
        public Int32 CUR_NODE_ID { get; set; }


        /// <summary>
        /// 当前-节点编码
        /// </summary>
        [DBField]
        [DisplayName("当前-节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CUR_NODE_CODE { get; set; }


        /// <summary>
        /// 当前节点的名称
        /// </summary>
        [DBField]
        public string CUR_NODE_TEXT { get; set; }

        /// <summary>
        /// 当前步骤的编码
        /// </summary>
        [DBField]
        public string CUR_INST_STEP_CODE { get; set; }



        /// <summary>
        /// 启动流程用户编码
        /// </summary>
        [DBField]
        [DisplayName("启动流程用户编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String START_USER_CODE { get; set; }


        /// <summary>
        /// 启动流程的用户名称
        /// </summary>
        [DBField]
        [DisplayName("启动流程的用户名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String START_USER_TEXT { get; set; }

        /// <summary>
        /// 流程状态
        /// </summary>
        [DBField]
        [DisplayName("流程状态")]
        [UIField(visible = true)]
        public Int32 FLOW_SID { get; set; }

        /// <summary>
        /// 流程启动时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("流程启动时间")]
        [UIField(visible = true)]
        public DateTime DATE_START { get; set; }

        /// <summary>
        /// 流程结束时间
        /// </summary>
        [DBField]
        [DisplayName("流程结束时间")]
        [UIField(visible = true)]
        public DateTime? DATE_END { get; set; }

        /// <summary>
        /// 流程类型
        /// </summary>
        [DBField]
        [DisplayName("流程类型")]
        [UIField(visible = true)]
        public Int32 FLOW_TYPE { get; set; }


        /// <summary>
        /// 实例备注
        /// </summary>
        [DBField]
        [DisplayName("实例备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String INST_REMARK { get; set; }


        /// <summary>
        /// 流程管理员编码
        /// </summary>
        [DBField]
        [DisplayName("流程管理员编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ADMIN_CODE { get; set; }


        /// <summary>
        /// 流程管理员名称
        /// </summary>
        [DBField]
        [DisplayName("流程管理员名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ADMIN_TEXT { get; set; }


        #region 扩展字段

        /// <summary>
        /// 页面ID
        /// </summary>
        [DBField]
        public int EXTEND_PAGE_ID { get; set; }


        /// <summary>
        /// 用户表名
        /// </summary>
        [DBField]
        public string EXTEND_TABLE { get; set; }

        /// <summary>
        /// 用户的记录ID
        /// </summary>
        [DBField]
        public int EXTEND_ROW_ID { get; set; }


        /// <summary>
        /// 用户的记录编码
        /// </summary>
        [DBField]
        public string EXTEND_ROW_CODE { get; set; }

        /// <summary>
        /// 用菜单菜单ID
        /// </summary>
        [DBField]
        public int EXTEND_MENU_ID { get; set; }

        /// <summary>
        /// 用的菜单唯一编码
        /// </summary>
        [DBField]
        public string EXTEND_MENU_IDENTIFIER { get; set; }

        /// <summary>
        /// 超链接
        /// </summary>
        [DBField]
        public string EXTEND_DOC_URL { get; set; }


        /// <summary>
        /// 文档名
        /// </summary>
        [DBField]
        public string EXTEND_DOC_TEXT { get; set; }

        /// <summary>
        /// 单据编码的字段名
        /// </summary>
        [DBField]
        public string EXTEND_BILL_CODE_FIELD { get; set; }
        
        /// <summary>
        /// 单据编码
        /// </summary>
        [DBField]
        public string EXTEND_BILL_CODE { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        [DBField]
        public string EXTEND_BILL_TYPE { get; set; }

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

        #endregion


        /// <summary>
        /// 产生自动步骤的 id
        /// </summary>
        [DBField]
        public int N_STEP_IDENTITY { get; set; }




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
