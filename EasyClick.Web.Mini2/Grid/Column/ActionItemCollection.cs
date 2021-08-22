using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 按钮集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionItemCollection : List<ActionItem>
    {

    }
}
