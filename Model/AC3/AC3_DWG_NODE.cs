using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 图形节点
    /// </summary>
    [DBTable("AC3_DWG_NODE")]
    [Description("图形节点")]
    [Serializable]
    public class AC3_DWG_NODE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 AC3_DWG_NODE_ID { get; set; }


        /// <summary>
        /// 图纸编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("图纸编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_DWG_CODE { get; set; }


        /// <summary>
        /// 节点编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_NODE_CODE { get; set; }


        /// <summary>
        /// 节点名称
        /// </summary>
        [DBField]
        [DisplayName("节点名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String NODE_TEXT { get; set; }


        /// <summary>
        /// 节点类型. 监听节点,执行节点. 正常节点, 判断节点, ...
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("节点类型. 监听节点,执行节点. 正常节点, 判断节点, ...")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String NODE_TYPE { get; set; }


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
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 节点标识
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("节点标识")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String NODE_IDENTIFIER { get; set; }


        /// <summary>
        /// 图形
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("图形")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String GRAPHICS { get; set; }


        /// <summary>
        /// 动作类型
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("动作类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ACTION_TYPE { get; set; }


        /// <summary>
        /// 动作脚本
        /// </summary>
        [DBField( StringEncoding.UTF8,true)]
        [DisplayName("动作脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String ACTION_SCRIPT { get; set; }


        /// <summary>
        /// 退回动作类型
        /// </summary>
        [DBField(20, StringEncoding.ASCII, false)]
        [DisplayName("退回动作类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String BACK_ACTION_TYPE { get; set; }


        /// <summary>
        /// 退回动作脚本
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("退回动作脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String BACK_ACTION_SCRIPT { get; set; }

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
        /// 删除批次号
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        public string ROW_DELETE_BATCH_CODE { get; set; }


        /// <summary>
        /// 扩展的节点类型.
        /// </summary>
        [DBField]
        public string EX_NODE_TYPE { get; set; }


        /// <summary>
        /// 监听类型
        /// </summary>
        [DBField(20,StringEncoding.ASCII,false)]
        [DisplayName("监听类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String LISTEN_TYPE { get; set; }


        /// <summary>
        /// 操作类型
        /// </summary>
        [DBField]
        [DisplayName("操作类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String OPERATE_TYPE { get; set; }


        /// <summary>
        /// insert, update, delete
        /// </summary>
        [DBField(20, StringEncoding.ASCII, false)]
        [DisplayName("insert, update, delete")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EX_METHOD { get; set; }


        /// <summary>
        /// 监听的数据表
        /// </summary>
        [DBField]
        [DisplayName("监听的数据表")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EX_TABLE { get; set; }


        /// <summary>
        /// 表名称
        /// </summary>
        [DBField]
        [DisplayName("表名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EX_TABLE_TEXT { get; set; }

    }
}
