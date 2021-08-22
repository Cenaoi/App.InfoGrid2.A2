using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 打印机设置
    /// </summary>
    [DBTable("BIZ_PRINT")]
    [Description("打印机设置")]
    [Serializable]
    public class BIZ_PRINT : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_PRINT_ID { get; set; }

        /// <summary>
        /// 业务状态：0-不在线，2-在线
        /// </summary>
        [DBField]
        [DisplayName("是否在线")]
        public bool IS_LINE { get; set; }

        /// <summary>
        /// 客户端Guid
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        public int BIZ_SID { get; set; }

        /// <summary>
        /// 打印机编码
        /// </summary>
        [DBField]
        [DisplayName("打印机编码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PRINT_CODE { get; set; }


        /// <summary>
        /// 打印机名称
        /// </summary>
        [DBField]
        [DisplayName("打印机名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PRINT_TEXT { get; set; }


        /// <summary>
        /// 打印机 URL
        /// </summary>
        [DBField]
        [DisplayName("打印机 URL")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PRINT_URL { get; set; }


        /// <summary>
        /// 最后连接时间
        /// </summary>
        [DBField]
        public DateTime LAST_LINE_TIME { get; set; }



        #region 数据记录常规信息

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
        [DBField,DefaultValue("(GetDate())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField,DefaultValue("(GetDate())")]
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
        /// 记录行的样式
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("记录行的样式")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String ROW_STYLE_JSON { get; set; }


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


        /// <summary>
        /// 结构权限编码
        /// </summary>
        [DBField]
        [LMValidate(maxLen = 20)]
        public string ROW_STRUCE_CODE { get; set; }


    }
}
