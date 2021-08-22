
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 客户端小程序
    /// </summary>
    [DBTable("BIZ_PRINT_CLIENT")]
    [Description("客户端小程序")]
    [Serializable]
    public class BIZ_PRINT_CLIENT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_PRINT_CLIENT_ID { get; set; }


        /// <summary>
        /// 业务状态ID.
        /// </summary>
        [DBField]
        [DisplayName("业务状态ID")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [DBField]
        [DisplayName("是否在线")]
        public bool IS_LINE { get; set; }


        /// <summary>
        /// 客户端 GUID
        /// </summary>
        [DBField]
        [DisplayName("客户端 GUID")]
        [UIField(visible = true)]
        public Guid PCLIENT_GUID { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }



        /// <summary>
        /// 最后连接时间
        /// </summary>
        [DBField]
        public DateTime LAST_LINE_TIME { get; set; }

        #region 数据记录常规信息

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
        [DBField,DefaultValue("(GetDate())")]
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

        #endregion

    }

}