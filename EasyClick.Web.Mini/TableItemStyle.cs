using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TableItemStyle:Style
    {
    }
}
