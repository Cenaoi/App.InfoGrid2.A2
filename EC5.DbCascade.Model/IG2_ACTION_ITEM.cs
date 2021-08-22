using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace EC5.DbCascade.Model
{
/// <summary>
    /// 
    /// </summary>
    [DBTable("IG2_ACTION_ITEM")]
    [Description("")]
    [Serializable]
    public class IG2_ACTION_ITEM : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_ITEM_ID { get; set; }

        /// <summary>
        /// 关联表ID
        /// </summary>
        [DBField]
        [DisplayName("关联表ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_ID { get; set; }


        /// <summary>
        /// 条目类型.FILTER-过滤;SET-赋值
        /// </summary>
        [DBField]
        [DisplayName("条目类型.FILTER-过滤;SET-赋值")]
        [LMValidate(maxLen = 15)]
        [UIField(visible = true)]
        public String ITEM_TYPE_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_TABLE { get; set; }

        /// <summary>
        /// 左表描述
        /// </summary>
        [DBField]
        public string L_TABLE_TEXT { get; set; }



        /// <summary>
        /// 字段名称
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_COL { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [DBField]
        public string L_COL_TEXT { get; set; }




        /// <summary>
        /// 值统计函数
        /// </summary>
        [DBField]
        public string R_VALUE_TOTAL_FUN { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_LOGIC { get; set; }


        /// <summary>
        /// 值模式
        /// </summary>
        [DBField,DefaultValue("TABLE")]
        [DisplayName("值模式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_VALUE_MODE { get; set; }


        /// <summary>
        /// 固定值
        /// </summary>
        [DBField]
        [DisplayName("固定值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_VALUE_FIXED { get; set; }


        /// <summary>
        /// 函数值
        /// </summary>
        [DBField]
        [DisplayName("函数值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_VALUE_FUN { get; set; }


        /// <summary>
        /// 链接类型
        /// </summary>
        [DBField]
        public string LINK_TYPE_ID { get; set; }

        /// <summary>
        /// （值的表模式）表名称
        /// </summary>
        [DBField]
        public string R_VALUE_TABLE { get; set; }

        /// <summary>
        /// 数据表名描述
        /// </summary>
        [DBField]
        public string R_VALUE_TABLE_TEXT { get; set; }


        /// <summary>
        /// （值的表模式）表字段名称
        /// </summary>
        [DBField]
        public string R_VALUE_COL { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [DBField]
        public string R_VALUE_COL_TEXT { get; set; }

        /// <summary>
        /// 自定义公式
        /// </summary>
        [DBField]
        public string R_VALUE_USER_FUNC { get; set; }
        

        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        public string REMARK { get; set; }

        /// <summary>
        /// 更新与新建，分别赋值。。默认是 E-更新
        /// A-新建，E-更新， A-E - 如果不存在就新建
        /// </summary>
        [DBField]
        public string L_ACT_CODE { get; set; }

        #region 常规字段

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
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
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

        #endregion
    }

}
