using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 业务-分类目录
    /// </summary>
    [DBTable("BIZ_CATALOG")]
    [Description("业务-分类目录")]
    [Serializable]
    public class BIZ_CATALOG : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_CATALOG_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }


        /// <summary>
        /// 目录类型
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("目录类型")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CATA_TYPE_CODE { get; set; }


        /// <summary>
        /// 目录代码
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("目录代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CATA_CODE { get; set; }

        /// <summary>
        /// 目录唯一ID，给程序使用的。
        /// </summary>
        [DBField]
        public string CATA_IDENTITY { get; set; }

        /// <summary>
        /// 权限等级
        /// </summary>
        [DBField]
        public int SEC_LEVEL { get; set; }

        /// <summary>
        ///  可视
        /// </summary>
        [DBField]
        public bool VISIBLE { get; set; }


        /// <summary>
        /// 目录名称
        /// </summary>
        [DBField]
        [DisplayName("目录名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CATA_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        /// <summary>
        /// 权限结构
        /// </summary>
        [DBField(StringEncoding.ASCII)]
        [DisplayName("权限结构")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_STRUCT_CODE { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DBField]
        public decimal SEQ { get; set; }

        /// <summary>
        /// 子节点的 ID
        /// </summary>
        [DBField]
        public int CHILD_IDENTITY { get; set; }

        /// <summary>
        /// 扩展类型
        /// </summary>
        [DBField]
        public string EX_TYPE_CODE { get; set; }


        /// <summary>
        /// 扩展表名
        /// </summary>
        [DBField]
        public string EX_TABLE { get; set; }

        /// <summary>
        /// 记录创建的公司代码
        /// </summary>
        [DBField]
        public string ROW_AUTHOR_COMP_CODE { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建日期
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建日期")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新日期
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录更新日期")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除日期
        /// </summary>
        [DBField]
        [DisplayName("记录删除日期")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }
}
