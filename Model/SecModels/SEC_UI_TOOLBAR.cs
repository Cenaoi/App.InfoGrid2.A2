using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{

    /// <summary>
    /// 权限-工具栏
    /// </summary>
    [DBTable("SEC_UI_TOOLBAR")]
    [Description("权限-工具栏")]
    [Serializable]
    public class SEC_UI_TOOLBAR : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_TOOLBAR_ID { get; set; }

        /// <summary>
        /// GUID
        /// </summary>
        [DBField]
        [Description("GUID")]
        [UIField(visible = true)]
        public Guid TOOLBAR_UID { get; set; }

        /// <summary>
        /// 对应的表ID,或试图ID
        /// </summary>
        [DBField]
        [Description("对应的表ID,或试图ID")]
        [UIField(visible = true)]
        public Int32 TABLE_ID { get; set; }


        /// <summary>
        /// 工具栏描述
        /// </summary>
        [DBField]
        [Description("工具栏描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DISPLAY { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [Description("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [Description("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [Description("记录更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除时间
        /// </summary>
        [DBField]
        [Description("记录删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

        /// <summary>
        /// 记录用户自定义排序
        /// </summary>
        [DBField]
        [Description("记录用户自定义排序")]
        [UIField(visible = true)]
        public Decimal ROW_USER_SEQ { get; set; }


        /// <summary>
        /// 记录创建角色代码
        /// </summary>
        [DBField]
        [Description("记录创建角色代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ROLE_CODE { get; set; }


        /// <summary>
        /// 记录创建公司代码
        /// </summary>
        [DBField]
        [Description("记录创建公司代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_COMP_CODE { get; set; }


        /// <summary>
        /// 记录创建部门代码
        /// </summary>
        [DBField]
        [Description("记录创建部门代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ORG_CODE { get; set; }


        /// <summary>
        /// 记录创建人员代码
        /// </summary>
        [DBField]
        [Description("记录创建人员代码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_USER_CODE { get; set; }

        /// <summary>
        /// 对应的权限界面ID
        /// </summary>
        [DBField]
        [Description("对应的权限界面ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_ID { get; set; }
        
         /// <summary>
        /// 对应的权限界面的表ID
        /// </summary>
        [DBField]
        [Description("对应的权限界面的表ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_TABLE_ID { get; set; }
        



    }
}
