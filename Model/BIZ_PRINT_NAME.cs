using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 打印机信息设置
    /// </summary>
    [DBTable("BIZ_PRINT_NAME")]
    [Description("打印机信息设置")]
    [Serializable]
    public class BIZ_PRINT_NAME : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField,DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_PRINT_NAME_ID { get; set; }

        /// <summary>
        /// 客户端Guid
        /// </summary>
        [DBField]
        [DisplayName("客户端Guid")]
        [UIField(visible = true)]
        public Guid PCLIENT_GUID { get; set; }


        /// <summary>
        /// 打印机名称
        /// </summary>
        [DBField]
        [DisplayName("打印机名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PRINT_NAME { get; set; }


        /// <summary>
        /// 打印机编码
        /// </summary>
        [DBField]
        [DisplayName("打印机编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PRINT_CODE { get; set; }


        /// <summary>
        /// 打印机驱动名称
        /// </summary>
        [DBField]
        [DisplayName("打印机驱动名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PRINT_DRIVE_NAME { get; set; }

        /// <summary>
        /// 是否在线,0--不在线,1--在线
        /// </summary>
        [DBField]
        [DisplayName("是否在线,0--不在线,1--在线")]
        [UIField(visible = true)]
        public Boolean IS_LINE { get; set; }


        /// <summary>
        /// 业务状态ID.
        /// </summary>
        [DBField]
        [DisplayName("业务状态ID")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

    }
}
