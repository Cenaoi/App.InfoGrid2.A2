

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 操作字段
    /// </summary>
    [DBTable("AC3_OPERATE_TABLE_FIELD")]
    [Description("操作字段")]
    [Serializable]
    public class AC3_OPERATE_TABLE_FIELD : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 AC3_OPERATE_TABLE_FIELD_ID { get; set; }


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
        public String FK_NODE_CODE { get; set; }


        /// <summary>
        /// 监听编码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("监听编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FK_OPERATE_CODE { get; set; }

        /// <summary>
        /// 操作类型: update, insert 共两种
        /// </summary>
        [Description("操作类型")]
        [DBField(StringEncoding.ASCII)]
        [LMValidate(maxLen =20)]
        public string METHOD_TYPE { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        [DBField]
        [Description("排列顺序")]
        public decimal SEQ_NUM { get; set; }


        /// <summary>
        /// 数据表名称
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("数据表名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TABLE { get; set; }


        /// <summary>
        /// 表描述
        /// </summary>
        [DBField]
        [DisplayName("表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TABLE_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("字段名")]
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
        /// 逻辑.= , , >=
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("逻辑.= , <> , >=")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DB_LOGIC { get; set; }


        /// <summary>
        /// 逻辑值
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("逻辑值")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String DB_VALUE { get; set; }


        /// <summary>
        /// 逻辑值2, 针对有带范围的..例如:数值,日期
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("逻辑值2, 针对有带范围的..例如:数值,日期")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String DB_VALUE_2 { get; set; }


        /// <summary>
        /// 值类型
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("值类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String VALUE_TYPE { get; set; }


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