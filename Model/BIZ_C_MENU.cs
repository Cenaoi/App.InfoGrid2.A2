using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 业务-核心-菜单
    /// </summary>
    [DBTable("BIZ_C_MENU")]
    [Description("业务-核心-菜单")]
    [Serializable]
    public class BIZ_C_MENU : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 BIZ_C_MENU_ID { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        [DBField]
        [DisplayName("父级菜单ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }


        /// <summary>
        /// 语言.CN-中文，EN-英文
        /// </summary>
        [DBField]
        [DisplayName("语言.CN-中文，EN-英文")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String DISPLAY_LANGUAGE { get; set; }



        /// <summary>
        /// 菜单类型.
        /// 
        /// </summary>
        [DBField]
        [DisplayName("菜单类型。0-目录,2-模块,4-方法")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String MENU_TYPE_ID { get; set; }


        /// <summary>
        /// 唯一值-定义特殊值，给特定程序使用
        /// </summary>
        [DBField]
        [DisplayName("唯一值-定义特殊值，给特定程序使用")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String MENU_IDENTIFIER { get; set; }


        /// <summary>
        /// 菜单名称
        /// </summary>
        [DBField]
        [DisplayName("菜单名称")]
        [LMValidate(maxLen = 40)]
        [UIField(visible = true)]
        public String NAME { get; set; }


        /// <summary>
        /// 提示信息
        /// </summary>
        [DBField]
        [DisplayName("提示信息")]
        [LMValidate(maxLen = 120)]
        [UIField(visible = true)]
        public String DESCRIPTION { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [DBField]
        [DisplayName("权限ID")]
        [UIField(visible = true)]
        public Int32 SEC_FUN_ID { get; set; }


        /// <summary>
        /// 页面地址
        /// </summary>
        [DBField]
        [DisplayName("页面地址")]
        [LMValidate(maxLen = 255)]
        [UIField(visible = true)]
        public String URI { get; set; }


        /// <summary>
        /// 特殊参数
        /// </summary>
        [DBField]
        [DisplayName("特殊参数")]
        [LMValidate(maxLen = 255)]
        [UIField(visible = true)]
        public String PARAMS { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DBField]
        [DisplayName("排序")]
        [UIField(visible = true)]
        public Decimal SEQ { get; set; }


        /// <summary>
        /// 图标地址
        /// </summary>
        [DBField]
        [DisplayName("图标地址")]
        [LMValidate(maxLen = 255)]
        [UIField(visible = true)]
        public String ICO { get; set; }

        /// <summary>
        /// 激活-或无效状态
        /// </summary>
        [DBField]
        [DisplayName("激活-或无效状态")]
        [UIField(visible = true)]
        public Boolean SYS_ENABLE { get; set; }

        /// <summary>
        /// 打开窗体数 - 默认是1 - 能一次打开窗体的最大数量
        /// </summary>
        [DBField]
        [DisplayName("打开窗体数 - 默认是1 - 能一次打开窗体的最大数量")]
        [UIField(visible = true)]
        public Int32 MAX_COUNT { get; set; }

        /// <summary>
        /// 深度（C）
        /// </summary>
        [DBField]
        [DisplayName("深度（C）")]
        [UIField(visible = true)]
        public Int32 DEPTH { get; set; }


        #region 配合权限使用

        /// <summary>
        /// 0-目录,2-模块,4-方法
        /// </summary>
        [DBField]
        public int SEC_FUN_TYPE_ID { get; set; }

        /// <summary>
        /// 页面类型。TABLE-单表模式，PAGE-复杂表模式（多表模式），
        /// </summary>
        [DBField]
        [DisplayName("页面类型。TABLE-单表模式，PAGE-复杂表模式（多表模式），")]
        public string SEC_PAGE_TYPE_ID { get; set; }


        /// <summary>
        /// 表ID
        /// </summary>
        [DBField]
        public int SEC_PAGE_ID { get;set;}

        #endregion



        /// <summary>
        /// 所有子节点的集合（C）
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("所有子节点的集合（C）")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String ARR_CHILD_ID { get; set; }


        /// <summary>
        /// 所有父节点的集合（C）
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("所有父节点的集合（C）")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String PARENT_PARTH { get; set; }

        /// <summary>
        /// 记录状态（C）
        /// </summary>
        [DBField]
        [DisplayName("记录状态（C）")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间（C）
        /// </summary>
        [DBField]
        [DisplayName("记录创建时间（C）")]
        [DefaultValue("(GetDate())")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间（C）
        /// </summary>
        [DBField]
        [DisplayName("记录更新时间（C）")]
        [DefaultValue("(GetDate())")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除时间（C）
        /// </summary>
        [DBField]
        [DisplayName("记录删除时间（C）")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

        /// <summary>
        /// 页面标识
        /// </summary>
        [DBField]
        public string SEC_PAGE_TAG { get; set; }

        /// <summary>
        /// 界面标题别名
        /// </summary>
        [DBField]
        public string ALIAS_TITLE { get; set; }

        /// <summary>
        /// 菜单激活
        /// </summary>
        [DBField]
        [DefaultValue(true)]
        public bool MENU_ENABLED { get; set; }

        /// <summary>
        /// 扩展配置信息
        /// </summary>
        [DBField]
        [DisplayName("扩展配置信息")]
        public string EXPAND_CFG { get; set; }

        /// <summary>
        /// 所属菜单ID。配合首页版面用的
        /// </summary>
        [DBField]
        public int OWNER_MENU_ID { get; set; }


        #region 图标

        /// <summary>
        /// 自动产生的图标文字
        /// </summary>
        [DBField]
        public string ICON_CHAT { get; set; }

        /// <summary>
        /// 图标字的颜色
        /// </summary>
        [DBField]
        public string ICON_COLOR { get; set; }

        /// <summary>
        /// 图标背景颜色
        /// </summary>
        [DBField]
        public string ICON_BG_COLOR { get; set; }


        #endregion

    }
}
