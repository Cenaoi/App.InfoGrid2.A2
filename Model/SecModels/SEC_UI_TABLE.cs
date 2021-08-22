using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 权限-界面表
    /// </summary>
    [DBTable("SEC_UI_TABLE")]
    [Description("权限-界面表")]
    [Serializable]
    public class SEC_UI_TABLE : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_TABLE_ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DBField]
        [DisplayName("上级ID")]
        [UIField(visible = true)]
        public Int32 SEC_UI_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("表的 GUID")]
        [UIField(visible = true)]
        public Guid TABLE_UID { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 表描述
        /// </summary>
        [DBField]
        [DisplayName("表描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_TEXT { get; set; }


        /// <summary>
        /// 控件ID
        /// </summary>
        [DBField]
        [DisplayName("控件ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String CONTROL_ID { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARK { get; set; }


        #region 记录公共字段

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
        [DBField, DefaultValue("(GetDate())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField, DefaultValue("(GetDate())")]
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
        /// 显示模式
        /// </summary>
        [DBField]
        public string DISPALY_MODE_ID { get; set; }
        
        /// <summary>
        /// 页面区域模式
        /// </summary>
        [DBField]
        public string PAGE_AREA_ID { get; set; }

        [DBField]
        public int PAGE_ID { get; set; }


        /// <summary>
        /// 锁行模式
        /// </summary>
        [DBField]
        public string LOCKED_MODE_ID { get; set; }

        /// <summary>
        /// 锁行的字段名
        /// </summary>
        [DBField]
        public string LOCKED_FIELD { get; set; }

        /// <summary>
        /// 锁行的规则。
        /// 暂时支持 JS 代码
        /// </summary>
        [DBField]
        public string LOCKED_RULE { get; set; }


        /// <summary>
        /// 显示验证消息
        /// </summary>
        [DBField]
        [Description("显示验证消息")]
        public bool VALID_MSG_ENABLED { get; set; }




        /// <summary>
        /// 弹出的字段名
        /// </summary>
        [DBField]
        [Description("弹出的字段名")]
        public String DIALOG_FIELD { get; set; }

        /// <summary>
        /// 弹出界面表ID
        /// </summary>
        [DBField]
        [Description("弹出界面表ID")]
        public Int32 DIALOG_TABLE_ID { get; set; }

        /// <summary>
        /// 页面类型， PAGE--表|AREA --区域|DIALOG--弹出窗口
        /// </summary>
        [DBField]
        [Description("页面类型， PAGE--表|AREA --区域|DIALOG--弹出窗口 ")]
        public String SEC_ITEM_TYPE { get; set; }


    }

}
