using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.ReportForms.UI
{
    /// <summary>
    /// 字段标签集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RFieldTagCollection : List<ReportFieldGroup>
    {

    }
}
