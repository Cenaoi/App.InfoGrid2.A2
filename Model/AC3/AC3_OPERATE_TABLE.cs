

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 操作表
    /// </summary>
    [DBTable("AC3_OPERATE_TABLE")]
    [Description("操作表")]
    [Serializable]
    public class AC3_OPERATE_TABLE : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 AC3_OPERATE_TABLE_ID { get; set; }


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
        public String PK_OPERATE_CODE { get; set; }


        /// <summary>
        /// 数据表名称
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("数据表名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TABLE { get; set; }


        /// <summary>
        /// 数据表描述
        /// </summary>
        [DBField]
        [DisplayName("数据表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TABLE_TEXT { get; set; }


        /// <summary>
        /// 操作函数: insert, update, delate
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("操作函数: insert, update, delate")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String OP_METHOD { get; set; }


        /// <summary>
        /// 动作类型. XML, CS=C#代码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("动作类型. XML, CS=C#代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ACTION_TYPE { get; set; }


        /// <summary>
        /// XML 数据内容
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("XML 数据内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String ACTION_XML { get; set; }


        /// <summary>
        /// C# 数据内容
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("C# 数据内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String ACTION_CS { get; set; }


        /// <summary>
        /// 判断条件类型. XML, CS=C#代码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("判断条件类型. XML, CS=C#代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String COND_SCRIPT_TYPE { get; set; }


        /// <summary>
        /// XML 数据内容
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("XML 数据内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String COND_SCRIPT_XML { get; set; }


        /// <summary>
        /// C# 数据内容
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("C# 数据内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String COND_SCRIPT_CS { get; set; }

        /// <summary>
        /// JSON 数据内容
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("JSON 数据内容")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String COND_SCRIPT_JSON { get; set; }




        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 插入后脚本
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("插入后脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String INSERTED_SCRIPT { get; set; }


        /// <summary>
        /// 插入前脚本
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("插入前脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String INSERTING_SCRIPT { get; set; }


        /// <summary>
        /// 更新后脚本
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("更新后脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String UPDATED_SCRIPT { get; set; }


        /// <summary>
        /// 更新前脚本
        /// </summary>
        [DBField]
        [DisplayName("更新前脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String UPDATING_SCRIPT { get; set; }


        /// <summary>
        /// 删除后脚本
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("删除后脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String DELETED_SCRIPT { get; set; }


        /// <summary>
        /// 删除前脚本
        /// </summary>
        [DBField]
        [DisplayName("删除前脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String DELETING_SCRIPT { get; set; }


        /// <summary>
        /// 查询后脚本
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("查询后脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String SELECTED_SCRIPT { get; set; }


        /// <summary>
        /// 查询前脚本
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("查询前脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String SELECTING_SCRIPT { get; set; }

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