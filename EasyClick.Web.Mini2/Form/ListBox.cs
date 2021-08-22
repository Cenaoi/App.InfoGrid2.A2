using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 列表控件
    /// </summary>
    /// <summary>
    /// 复选框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [DefaultProperty("Value")]
    public class ListBox:CheckboxGroup
    {
        public ListBox()
        {
            this.InReady = "Mini2.ui.form.field.ListBox";

            this.JsNamespace = "Mini2.ui.form.field.ListBox";
        }


    }
}
