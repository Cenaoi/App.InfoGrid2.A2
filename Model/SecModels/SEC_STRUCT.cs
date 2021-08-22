using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限-系统结构
    /// </summary>
    [DBTable("SEC_STRUCT")]
    [Description("权限-系统结构")]
    [Serializable]
    public class SEC_STRUCT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_STRUCT_ID { get; set; }

        /// <summary>
        /// 上级节点ID
        /// </summary>
        [DBField]
        [DisplayName("上级节点ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }


        /// <summary>
        /// 结构编码
        /// </summary>
        [DBField]
        [DisplayName("结构编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STRUCE_CODE { get; set; }


        /// <summary>
        /// 结构名称
        /// </summary>
        [DBField]
        [DisplayName("结构名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String STRUCE_TEXT { get; set; }


        /// <summary>
        /// 子数值
        /// </summary>
        [DBField]
        public int CHILD_IDENTITY { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String REMARK { get; set; }



        #region 记录状态

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

        #endregion

    }

}
