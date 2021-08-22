using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 上下文菜单
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ContextMenu:MenuItemCollection
    {

    }

    public class MenuItemCollection:List<MenuItem>
    {

    }

    /// <summary>
    /// 菜单项目
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 显示
        /// </summary>
        public string Text { get; set; }

        public string Icon { get; set; }

        public string IconClass { get; set; }


        /// <summary>
        /// 服务器命令
        /// </summary>
        public string Command { get; set; }


        public string OnClick { get; set; }

    }

    
}
