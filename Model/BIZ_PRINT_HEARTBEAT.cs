
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 打印机心跳
    /// </summary>
    [DBTable("BIZ_PRINT_HEARTBEAT")]
    [Description("打印机心跳")]
    [Serializable]
    public class BIZ_PRINT_HEARTBEAT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_PRINT_HEARTBEAT_ID { get; set; }

        /// <summary>
        /// 客户端GUID
        /// </summary>
        [DBField]
        [DisplayName("客户端GUID")]
        [UIField(visible = true)]
        public Guid PCLIENT_GUID { get; set; }


        /// <summary>
        /// 打印机编码
        /// </summary>
        [DBField]
        [DisplayName("打印机编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PRINT_CODE { get; set; }


        /// <summary>
        /// 客户IP
        /// </summary>
        [DBField]
        [DisplayName("客户IP")]
        [LMValidate(maxLen = 16)]
        [UIField(visible = true)]
        public String C_IP { get; set; }


        /// <summary>
        /// 电脑主机名称
        /// </summary>
        [DBField]
        [DisplayName("电脑主机名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COMPUTER_NAME { get; set; }


        /// <summary>
        /// 打印机驱动名
        /// </summary>
        [DBField]
        [DisplayName("打印机驱动名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DRIVER_NAME { get; set; }


        /// <summary>
        /// 客户端备注
        /// </summary>
        [DBField]
        [DisplayName("客户端备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }

        /// <summary>
        /// 记录-状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录-状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录-创建时间
        /// </summary>
        [DBField, DefaultValue("(GetDate())")]
        [DisplayName("记录-创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录-更新时间
        /// </summary>
        [DBField,DefaultValue("(GetDate())")]
        [DisplayName("记录-更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录-删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录-删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }

}