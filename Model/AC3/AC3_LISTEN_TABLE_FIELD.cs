

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 监听的表字段
    /// </summary>
    [DBTable("AC3_LISTEN_TABLE_FIELD")]
    [Description("监听的表字段")]
    [Serializable]
    public class AC3_LISTEN_TABLE_FIELD : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 AC3_LISTEN_TABLEFIELD_ID { get; set; }


        /// <summary>
        /// 联动编码
        /// </summary>
        [DBField]
        [DisplayName("联动编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_DWG_CODE { get; set; }


        /// <summary>
        /// 节点编码
        /// </summary>
        [DBField]
        [DisplayName("节点编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_NODE_CODE { get; set; }


        /// <summary>
        /// 监听编码
        /// </summary>
        [DBField]
        [DisplayName("监听编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_LISTEN_CODE { get; set; }




        /// <summary>
        /// 内部数据表名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("内部数据表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TABLE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TABLE_TEXT { get; set; }


        /// <summary>
        /// 内部字段名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("内部字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_FIELD { get; set; }


        /// <summary>
        /// 字段描述
        /// </summary>
        [DBField]
        [DisplayName("字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_FIELD_TEXT { get; set; }


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


        /// <summary>
        /// 记录删除的批次编码
        /// </summary>
        [DBField]
        [DisplayName("记录删除的批次编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ROW_DELETE_BATCH_CODE { get; set; }

        #endregion
    }

}