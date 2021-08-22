using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 显示模式
    /// </summary>
    [Description("显示模式")]
    public enum DisplayMode
    {
        /// <summary>
        /// 自动模式
        /// </summary>
        Auto,
        /// <summary>
        /// 图标模式
        /// </summary>
        Icon,
        /// <summary>
        /// 图标+文本模式
        /// </summary>
        IconText,
        /// <summary>
        /// 文本模式
        /// </summary>
        Text
    }



    /// <summary>
    /// 动作按钮
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionItem
    {
        string m_Icon;

        string m_Tooltip;

        string m_Text;

        string m_Handler;

        string m_Href;

        /// <summary>
        /// 目标
        /// </summary>
        string m_Target;

        string m_TargetText;

        string m_Click;

        string m_ClickHandler;

        /// <summary>
        /// 超链接
        /// </summary>
        public string Href
        {
            get { return m_Href; }
            set { m_Href = value; }
        }

        /// <summary>
        /// 目标对象
        /// </summary>
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }


        /// <summary>
        /// 目标框架的标题
        /// </summary>
        public string TargetText
        {
            get { return m_TargetText; }
            set { m_TargetText = value; }
        }


        public string Click
        {
            get { return m_Click; }
            set { m_Click = value; }
        }

        public string ClickHandler
        {
            get { return m_ClickHandler; }
            set { m_ClickHandler = value; }
        }

        DisplayMode m_DisplayMode = DisplayMode.Auto;

        /// <summary>
        /// 显示模式
        /// </summary>
        [Description("显示模式")]
        [DefaultValue(DisplayMode.Auto)]
        public DisplayMode DisplayMode
        {
            get { return m_DisplayMode; }
            set { m_DisplayMode = value; }
        }


        /// <summary>
        /// 图标路径
        /// </summary>
        [Description("图标模式")]
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        [Description("提示信息")]
        public string Tooltip
        {
            get { return m_Tooltip; }
            set { m_Tooltip = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 处理函数
        /// </summary>
        [Description("处理函数")]
        public string Handler
        {
            get { return m_Handler; }
            set { m_Handler = value; }
        }

        /// <summary>
        /// 服务器命令
        /// </summary>
        [Description("服务器命令")]
        public string Command { get; set; }

        /// <summary>
        /// 服务器命令参数
        /// </summary>
        [Description("服务器命令参数")]
        public string CommandParam { get; set; }


    }
}
