using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 过滤
    /// </summary>
    [DBTable("BIZ_FILTER")]
    [Description("过滤")]
    [Serializable]
    public class BIZ_FILTER : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible= true)]
        public Int32 BIZ_FILTER_ID { get; set; }


        /// <summary>
        /// 页类型：单页，复杂页，查询页，导入页
        /// </summary>
        [DBField]
        [DisplayName("页类型：单页，复杂页，查询页，导入页")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PAGE_TYPE_ID { get; set; }

        /// <summary>
        /// 页ID，对应 IG2_TABLE 表的主键
        /// </summary>
        [DBField]
        [DisplayName("页ID，对应 IG2_TABLE 表的主键")]
        [UIField(visible = true)]
        public Int32 PAGE_ID { get; set; }


        /// <summary>
        /// 代码
        /// </summary>
        [DBField]
        [DisplayName("代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_CODE { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [DBField]
        [DisplayName("描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_TEXT { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        [DBField]
        [DisplayName("激活")]
        [UIField(visible = true)]
        public Boolean IS_ENABLED { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

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
        [DBField,DefaultValue("(GETDATE())")]
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
        /// 记录用户自定义排序
        /// </summary>
        [DBField]
        [DisplayName("记录用户自定义排序")]
        [UIField(visible = true)]
        public Decimal ROW_USER_SEQ { get; set; }


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

    }
}
