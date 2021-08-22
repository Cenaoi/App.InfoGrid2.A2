using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 脚本区域
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class Script:Control
    {
        public Script()
        {
        }



        protected override void Render(HtmlTextWriter writer)
        {




        }
    }
}
