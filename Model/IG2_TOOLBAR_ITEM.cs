using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 工具栏条目
    /// </summary>
    [DBTable("IG2_TOOLBAR_ITEM")]
    [Description("工具栏条目")]
    [Serializable]
    public class IG2_TOOLBAR_ITEM : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_TOOLBAR_ITEM_ID { get; set; }

        /// <summary>
        /// 工具栏ID
        /// </summary>
        [DBField]
        [DisplayName("工具栏ID")]
        [UIField(visible = true)]
        public Guid TOOLBAR_UID { get; set; }

        /// <summary>
        /// 表格 ID
        /// </summary>
        [DBField]
        public int TABLE_ID { get; set; }

        /// <summary>
        /// 条目ID,配合 EVENT_MODE_ID = SYS 系统字段使用
        /// </summary>
        [DBField]
        public string ITEM_ID { get; set; }

        /// <summary>
        /// 工具栏GUID
        /// </summary>
        [DBField]
        [DisplayName("工具栏GUID")]
        [UIField(visible = true)]
        public Int32 IG2_TOOLBAR_ID { get; set; }


        /// <summary>
        /// 项目类型.BTN-按钮,HR-分隔符, MENU-小菜单
        /// </summary>
        [DBField]
        [DisplayName("项目类型.BTN-按钮,HR-分隔符, MENU-小菜单")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ITEM_TYPE_ID { get; set; }

        /// <summary>
        /// 父节点,备用..子容器模式使用
        /// </summary>
        [DBField]
        [DisplayName("父节点,备用..子容器模式使用")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }


        /// <summary>
        /// 按钮名称
        /// </summary>
        [DBField]
        [DisplayName("按钮名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ITEM_TEXT { get; set; }


        /// <summary>
        /// 点击按钮前,提示信息
        /// </summary>
        [DBField]
        [DisplayName("点击按钮前,提示信息")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String ASK_MSG { get; set; }


        /// <summary>
        /// 显示模式.DEFAULT-默认模式 , ICON_TEXT-按钮图标模式, ICON-图标模式, TEXT-文字模式
        /// </summary>
        [DBField]
        [DisplayName("显示模式.DEFAULT-默认模式 , ICON_TEXT-按钮图标模式, ICON-图标模式, TEXT-文字模式")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DISPLAY_MODE_ID { get; set; }


        /// <summary>
        /// 图标ID.0-没有指定图标.
        /// </summary>
        [DBField]
        [DisplayName("图标ID.0-没有指定图标.")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ICON_ID { get; set; }


        /// <summary>
        /// 图标文件路径
        /// </summary>
        [DBField]
        [DisplayName("图标文件路径")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ICON_PATH { get; set; }


        /// <summary>
        /// 左右对齐. LEFT-左对齐,RIGHT-右对齐
        /// </summary>
        [DBField]
        [DisplayName("左右对齐. LEFT-左对齐,RIGHT-右对齐")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String ALIGN { get; set; }


        /// <summary>
        /// 提示信息
        /// </summary>
        [DBField]
        [DisplayName("提示信息")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TOOLTIP { get; set; }


        /// <summary>
        /// 点击触发的事件模式. DEFAULT-默认, COMMAND-服务器命令, PLUG-插件命令..SYS-内部常规命令(增删改查..)
        /// </summary>
        [DBField]
        [DisplayName("点击触发的事件模式. DEFAULT-默认, COMMAND-服务器命令, PLUG-插件命令..SYS-内部常规命令(增删改查..)")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String EVENT_MODE_ID { get; set; }


        /// <summary>
        /// 用户自定义.额外脚本.(备用)
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("用户自定义.额外脚本.(备用)")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String USER_JS { get; set; }


        /// <summary>
        /// 点击前,触发的脚本
        /// </summary>
        [DBField]
        [DisplayName("点击前,触发的脚本")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String ON_BEFORE_CLICK { get; set; }


        /// <summary>
        /// 点击事件脚本
        /// </summary>
        [DBField]
        [DisplayName("点击事件脚本")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String ON_CLICK { get; set; }


        /// <summary>
        /// 服务器命令名称
        /// </summary>
        [DBField]
        [DisplayName("服务器命令名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COMMAND { get; set; }


        /// <summary>
        /// 服务器命令参数
        /// </summary>
        [DBField]
        [DisplayName("服务器命令参数")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COMMAND_PARAMS { get; set; }


        /// <summary>
        /// 插件-类全名
        /// </summary>
        [DBField]
        [DisplayName("插件-类全名")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String PLUG_CLASS { get; set; }


        /// <summary>
        /// 插件-函数名
        /// </summary>
        [DBField]
        [DisplayName("插件-函数名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PLUG_METHOD { get; set; }


        /// <summary>
        /// 插件参数
        /// </summary>
        [DBField]
        [DisplayName("插件参数")]
        public string PLUG_PARAMS { get; set; }



        /// <summary>
        /// 可视
        /// </summary>
        [DBField]
        public bool VISIBLE { get; set; }


        /// <summary>
        /// 激活
        /// </summary>
        [DBField]
        public bool ENABLED { get; set; }


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

    }

}
