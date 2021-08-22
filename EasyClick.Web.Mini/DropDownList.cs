using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 下拉框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items")]
    [DefaultProperty("Value")]
    [Description("下拉框")]
    public class DropDownList:MiniHtmlListBase
    {
        /// <summary>
        /// 下拉框
        /// </summary>
        public DropDownList()
        {
            this.HtmlTag = System.Web.UI.HtmlTextWriterTag.Select;

        }

    }
}
