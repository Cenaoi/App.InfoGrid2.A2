using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace EC5.DbCascade.Model
{
/// <summary>
    /// 数据表关联操作
    /// </summary>
    [DBTable("IG2_ACTION")]
    [Description("数据表关联操作")]
    [Serializable]
    public class IG2_ACTION : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_ACTION_ID { get; set; }


        /// <summary>
        /// 左-动作代码
        /// </summary>
        [DBField]
        [DisplayName("左-动作代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String L_ACT_CODE { get; set; }

        /// <summary>
        /// 更新数据的新条件。
        /// 如果更新的记录不存在，就创建一条记录出来.
        /// NONE-不处理， A-新建
        /// </summary>
        [DBField]
        public string L_NOT_EXIST_THEN { get; set; }


        /// <summary>
        /// 右-动作代码
        /// </summary>
        [DBField]
        [DisplayName("右-动作代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String R_ACT_CODE { get; set; }

        /// <summary>
        /// 左-表ID
        /// </summary>
        [DBField]
        [DisplayName("左-表ID")]
        [UIField(visible = true)]
        public Int32 L_TABLE_ID { get; set; }


        /// <summary>
        /// 左-数据表名
        /// </summary>
        [DBField]
        [DisplayName("左-数据表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_TABLE { get; set; }


        /// <summary>
        /// 左-表描述
        /// </summary>
        [DBField]
        [DisplayName("左-表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_TABLE_TEXT { get; set; }

        /// <summary>
        /// 左-表GUID
        /// </summary>
        [DBField]
        [DisplayName("左-表GUID")]
        [UIField(visible = true)]
        public Guid L_TABLE_UID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 R_TABLE_ID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_TABLE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_TABLE_TEXT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Guid R_TABLE_UID { get; set; }


        /// <summary>
        /// 左-列名
        /// </summary>
        [DBField]
        [DisplayName("左-列名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_COL { get; set; }


        /// <summary>
        /// 左-列描述
        /// </summary>
        [DBField]
        [DisplayName("左-列描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_COL_TEXT { get; set; }


        /// <summary>
        /// 右-列名
        /// </summary>
        [DBField]
        [DisplayName("右-列名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_COL { get; set; }


        /// <summary>
        /// 右-列描述
        /// </summary>
        [DBField]
        [DisplayName("右-列描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_COL_TEXT { get; set; }


        /// <summary>
        /// 值模式
        /// </summary>
        [DBField]
        [DisplayName("值模式")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String L_VALUE_MODE { get; set; }


        /// <summary>
        /// 固定值
        /// </summary>
        [DBField]
        [DisplayName("固定值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_VALUE_FIXED { get; set; }


        /// <summary>
        /// 函数
        /// </summary>
        [DBField]
        [DisplayName("函数")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String L_VALUE_FUN { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String R_VALUE_MODE { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_VALUE_FIXED { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String R_VALUE_FUN { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        public string REMARK { get; set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        [DBField]
        public string LINK_TYPE_ID { get; set; }

        /// <summary>
        /// 是否过滤子集合
        /// </summary>
        [DBField]
        public bool L_IS_SUB_FILTER { get; set; }


        /// <summary>
        /// 是否过滤子集合
        /// </summary>
        [DBField]
        public bool R_IS_SUB_FILTER { get; set; }


        /// <summary>
        /// 激活
        /// </summary>
        [DBField]
        public bool ENABLED { get; set; }

        /// <summary>
        /// 监听，与或格式 ..AND , OR
        /// </summary>
        [DBField]
        public string LISTEN_AO { get; set; }

        /// <summary>
        /// 执行的SEQ, 数值越大, 越早执行.
        /// </summary>
        [DBField]
        public int EXEC_SEQ { get; set; }


        #region 联动执行脚本

        /// <summary>
        /// 激活联动执行脚本
        /// </summary>
        [DBField]
        public bool ACT_NEW_ENABLED { get; set; }

        /// <summary>
        /// 新建操作后,执行的脚本
        /// </summary>
        [DBField]
        public string ACT_NEW_SCODE { get; set; }

        /// <summary>
        /// 激活更新执行脚本
        /// </summary>
        [DBField]
        public bool ACT_UPDATE_ENABLED { get; set; }

        /// <summary>
        /// 更新操作后,执行的脚本
        /// </summary>
        [DBField]
        public string ACT_UPDATE_SCODE { get; set; }


        /// <summary>
        /// 执行继续触发联动
        /// </summary>
        [DBField]
        public bool AUTO_CONTINUE { get; set; }


        #endregion

        #region 行常规字段


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
        [DBField,DefaultValue("(GETDATE())")]
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

        #endregion


    }
}
