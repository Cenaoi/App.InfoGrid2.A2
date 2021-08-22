using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 打印文件的列表
    /// </summary>
    [DBTable("BIZ_PRINT_FILE")]
    [Description("打印文件的列表")]
    [Serializable]
    public class BIZ_PRINT_FILE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_PRINT_FILE_ID { get; set; }


        /// <summary>
        /// 文件URL
        /// </summary>
        [DBField]
        [DisplayName("文件URL")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String FILE_URL { get; set; }

        /// <summary>
        /// 打印机代码
        /// </summary>
        [DBField]
        public string PRINT_CODE { get; set; }



        /// <summary>
        /// 打印机名
        /// </summary>
        [DBField]
        [DisplayName("打印机名")]
        [LMValidate(maxLen = 100)]
        [UIField(visible = true)]
        public String PRINT_NAME { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
        [DisplayName("创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 状态类型  0--未下载状态，2--已下载 1--异常  4--打印完成
        /// </summary>
        [DBField]
        [DisplayName("状态类型")]
        [UIField(visible = true)]
        public int ROW_SID { get; set; }


        [DBField,DefaultValue("(NewID())")]
        public Guid FILE_GUID { get; set; }

        /// <summary>
        /// 打印结束的时间
        /// </summary>
        [DBField]
        public DateTime? TIME_PRINT_END { get;set;}
    }
}
