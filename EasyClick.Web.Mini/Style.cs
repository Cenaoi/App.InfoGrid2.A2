using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    
    [ToolboxItem(false)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class Style : Component
    {
        [NotifyParentProperty(true), DefaultValue("")]
        public string CssClass { get; set; }
    }
}
