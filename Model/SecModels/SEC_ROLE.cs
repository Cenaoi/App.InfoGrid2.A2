
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限角色
    /// </summary>
    [DBTable("SEC_ROLE")]
    [Description("权限角色")]
    [Serializable]
    public class SEC_ROLE : LightModel
    {


        /// <summary>
        /// 角色代码
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [DisplayName("角色代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public int SEC_ROLE_ID { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        [DBField]
        [DisplayName("角色代码")]
        public string ROLE_CODE { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [DBField]
        [DisplayName("角色名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TEXT { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [DBField]
        [DisplayName("描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DESCRIPTION { get; set; }



        #region 数据库记录信息

        /// <summary>
        /// 记录状态
        /// </summary>
        [DBField]
        [DisplayName("记录状态")]
        [UIField(visible = true)]
        public Int32 ROW_STATUS_ID { get; set; }

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

        #endregion


    }

}