using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 上传文件列表
    /// </summary>
    [DBTable("BIZ_FILE")]
    [Description("上传文件列表")]
    [Serializable]
    public class BIZ_FILE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_FILE_ID { get; set; }


        /// <summary>
        /// 表格名称
        /// </summary>
        [DBField]
        [DisplayName("表格名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }

        /// <summary>
        /// 表格ID
        /// </summary>
        [DBField]
        [DisplayName("表格ID")]
        [UIField(visible = true)]
        public Int32 TABLE_ID { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        public string FIELD_NAME { get; set; }


        /// <summary>
        /// 行ID
        /// </summary>
        [DBField]
        [DisplayName("行ID")]
        [UIField(visible = true)]
        public Int32 ROW_ID { get; set; }


        /// <summary>
        /// 行编码
        /// </summary>
        [DBField]
        [DisplayName("行编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ROW_CODE { get; set; }


        /// <summary>
        /// 文件名称
        /// </summary>
        [DBField]
        [DisplayName("文件名称")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String FILE_NAME { get; set; }


        /// <summary>
        /// 文件路径
        /// </summary>
        [DBField]
        [DisplayName("文件路径")]
        [LMValidate(maxLen = 500)]
        [UIField(visible = true)]
        public String FILE_PATH { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [DBField]
        [DisplayName("文件大小")]
        [UIField(visible = true)]
        public Int32 FILE_SIZE { get; set; }


        /// <summary>
        /// 文件后缀名
        /// </summary>
        [DBField]
        [DisplayName("文件后缀名")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FILE_EX { get; set; }


        /// <summary>
        /// 表格类型
        /// </summary>
        [DBField]
        [DisplayName("表格类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TABLE_TYPE { get; set; }


        /// <summary>
        /// 标记编码
        /// </summary>
        [DBField]
        [DisplayName("标记编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TAG_CODE { get; set; }

        /// <summary>
        /// 类型编码
        /// </summary>
        [DBField]
        [DisplayName("类型编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public string TYPE_CODE { get; set; }

        
        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [DisplayName("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }


        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
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
        /// 记录创建角色代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建角色代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ROLE_CODE { get; set; }


        /// <summary>
        /// 记录创建公司代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建公司代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_COMP_CODE { get; set; }


        /// <summary>
        /// 记录创建部门代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建部门代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ORG_CODE { get; set; }


        /// <summary>
        /// 记录创建人员代码
        /// </summary>
        [DBField]
        [DisplayName("记录创建人员代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_USER_CODE { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        public String REMARKS { get; set; }




    }
}
