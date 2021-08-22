using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.AC3
{
    /// <summary>
    /// 监听的表
    /// </summary>
    [DBTable("AC3_LISTEN_TABLE")]
    [Description("监听的表")]
    [Serializable]
    public class AC3_LISTEN_TABLE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 AC3_LISTEN_TABLE_ID { get; set; }


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
        public String PK_LISTEN_CODE { get; set; }


        /// <summary>
        /// 数据操作, all=全部, insert=插入; update=更新; delete=删除;
        /// </summary>
        [DBField]
        public string DB_METHOD { get; set; }


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
        [DBField(StringEncoding.UTF8,true)]
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
        /// 检测值变化的脚本那类型 XML, CS=C#代码,json
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("检测值变化的脚本那类型 XML, CS=C#代码,json")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String VCHANGE_SCRIPT_TYPE { get; set; }


        /// <summary>
        /// 检测值变化的脚本
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("检测值变化的脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String VCHANGE_SCRIPT_XML { get; set; }


        /// <summary>
        /// 检测值变化的脚本
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("检测值变化的脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String VCHANGE_SCRIPT_CS { get; set; }


        /// <summary>
        /// 检测值变化的脚本
        /// </summary>
        [DBField(StringEncoding.UTF8,true)]
        [DisplayName("检测值变化的脚本")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String VCHANGE_SCRIPT_JSON { get; set; }




        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
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
