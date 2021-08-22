using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限2-表流程节点
    /// </summary>
    [DBTable("SEC_TABLE_FLOW_NODE")]
    [Description("权限2-表流程节点")]
    [Serializable]
    public class SEC_TABLE_FLOW_NODE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_TABLE_FLOW_NODE_ID { get; set; }


        /// <summary>
        /// 外键-表流程编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("外键-表流程编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_SEC_TF_CODE { get; set; }

        /// <summary>
        /// 页面id
        /// </summary>
        [DBField]
        public int PAGE_ID { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("数据表名")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String DB_TABLE { get; set; }


        /// <summary>
        /// 流程编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("流程编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FLOW_CODE { get; set; }


        /// <summary>
        /// 节点编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FLOW_NODE_CODE { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [DBField]
        [DisplayName("节点名称")]
        [LMValidate(maxLen = 20)]
        public string FLOW_NODE_TEXT { get; set; }


        /// <summary>
        /// 表-头部名称
        /// </summary>
        [DBField]
        [DisplayName("表-头部名称")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_HEADER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_HEADER_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_HEADER_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_FOOTER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_FOOTER_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_FOOTER_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_1 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_2 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_3 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_4 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_5 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_6 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_7 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_8 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String T_SUB_9 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_1_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_2_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_3_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_4_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_5_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_6_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_7_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_8_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_9_V_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_1_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_2_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_3_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_4_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_5_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_6_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_7_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII, true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_8_H_FIELDS { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField(StringEncoding.ASCII,true)]
        [DisplayName("")]
        [LMValidate(maxLen = 1024)]
        [UIField(visible = true)]
        public String T_SUB_9_H_FIELDS { get; set; }

        
        [DBField]
        public bool T_SUB_1_EDITOR { get; set; }


        [DBField]
        public bool T_SUB_2_EDITOR { get; set; }

        [DBField]
        public bool T_SUB_3_EDITOR { get; set; }

        [DBField]
        public bool T_SUB_4_EDITOR { get; set; }


        [DBField]
        public bool T_SUB_5_EDITOR { get; set; }

        [DBField]
        public bool T_SUB_6_EDITOR { get; set; }


        [DBField]
        public bool T_SUB_7_EDITOR { get; set; }


        [DBField]
        public bool T_SUB_8_EDITOR { get; set; }


        [DBField]
        public bool T_SUB_9_EDITOR { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


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
