using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 样式规则
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class StyleRuleCollection:List<StyleRuleItem>
    {

    }
}
