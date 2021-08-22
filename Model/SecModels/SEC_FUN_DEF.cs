
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 系统模块权限定义
    /// </summary>
    [DBTable("SEC_FUN_DEF")]
    [Description("系统模块权限定义")]
    [Serializable]
    public class SEC_FUN_DEF : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_FUN_DEF_ID { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DBField]
        [DisplayName("父ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        [DBField]
        [DisplayName("节点类型")]
        [UIField(visible = true)]
        public Int32 FUN_TYPE_ID { get; set; }


        /// <summary>
        /// 代码
        /// </summary>
        [DBField]
        [DisplayName("代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CODE { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        [DBField]
        [DisplayName("名称")]
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

        /// <summary>
        /// 排序的字段
        /// </summary>
        [DBField]
        public decimal SEQ { get; set; }



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