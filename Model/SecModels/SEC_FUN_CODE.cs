
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 内置代码
    /// </summary>
    [DBTable("SEC_FUN_CODE")]
    [Description("内置代码")]
    [Serializable]
    public class SEC_FUN_CODE : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField,DBIdentity,DBKey]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 SEC_FUN_CODE_ID { get; set; }


        /// <summary>
        /// 代码
        /// </summary>
        [DBField]
        [DisplayName("代码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CODE { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [DBField]
        [DisplayName("描述")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TEXT { get; set; }


        /// <summary>
        /// 代码类型.VIEW,UPDATE,PRINT
        /// </summary>
        [DBField]
        [DisplayName("代码类型.VIEW,UPDATE,PRINT")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String CODE_TYPE_ID { get; set; }



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