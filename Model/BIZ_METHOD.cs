using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 业务函数
    /// </summary>
    [DBTable("BIZ_METHOD")]
    [Description("业务函数")]
    [Serializable]
    public class BIZ_METHOD : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_METHOD_ID { get; set; }


        /// <summary>
        /// 目录
        /// </summary>
        [DBField]
        [DisplayName("目录")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATA_TEXT { get; set; }


        /// <summary>
        /// 分类
        /// </summary>
        [DBField]
        [DisplayName("分类")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATE_TEXT { get; set; }


        /// <summary>
        /// 函数编码
        /// </summary>
        [DBField]
        [DisplayName("函数编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String METHOD_CODE { get; set; }


        /// <summary>
        /// 返回类型, int, string, table
        /// </summary>
        [DBField]
        [DisplayName("返回类型, int, string, table")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String RETURN_TYPE { get; set; }


        /// <summary>
        /// 执行类型 TSQL, CODE
        /// </summary>
        [DBField]
        [DisplayName("执行类型 TSQL, CODE")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EXEC_TYPE { get; set; }


        /// <summary>
        /// T-SQL 语句
        /// </summary>
        [DBField]
        [DisplayName("T-SQL 语句")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String TSQL { get; set; }


        /// <summary>
        /// 脚本代码
        /// </summary>
        [DBField]
        [DisplayName("脚本代码")]
        [LMValidate(maxLen = 0)]
        [UIField(visible = true)]
        public String ACTION_CODE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

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
    }
}
