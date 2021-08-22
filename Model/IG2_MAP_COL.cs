using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 列对应列
    /// </summary>
    [DBTable("IG2_MAP_COL")]
    [Description("列对应列")]
    [Serializable]
    public class IG2_MAP_COL : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_MAP_COL_ID { get; set; }

        /// <summary>
        /// 表和表导入数据ID
        /// </summary>
        [DBField]
        [DisplayName("表和表导入数据ID")]
        [UIField(visible = true)]
        public Int32 IG2_MAP_ID { get; set; }


        /// <summary>
        /// 目标列
        /// </summary>
        [DBField]
        [DisplayName("目标列")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_COL { get; set; }


        /// <summary>
        /// 目标列显示名称
        /// </summary>
        [DBField]
        [DisplayName("目标列显示名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_COL_TEXT { get; set; }



        /// <summary>
        /// 逻辑语句
        /// </summary>
        [DBField]
        [DisplayName("值模式")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String LONIN { get; set; }


        /// <summary>
        /// 值模式
        /// </summary>
        [DBField]
        [DisplayName("值模式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String VALUE_MODE { get; set; }



        /// <summary>
        /// 数据列
        /// </summary>
        [DBField]
        [DisplayName("数据列")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_COL { get; set; }


        /// <summary>
        /// 数据列显示名称
        /// </summary>
        [DBField]
        [DisplayName("数据列显示名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_COL_TEXT { get; set; }


        /// <summary>
        /// 固定值
        /// </summary>
        [DBField]
        [DisplayName("固定值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_FIXED { get; set; }


        /// <summary>
        /// 函数
        /// </summary>
        [DBField]
        [DisplayName("函数")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_FUN { get; set; }

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
