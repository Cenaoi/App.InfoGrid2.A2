using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// Table 表格的数字列
    /// </summary>
    [Description("Table 表格的数字列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RowNumberer : BoundField
    {
        /// <summary>
        /// 行的数字
        /// </summary>
        public RowNumberer()
        {
            this.Width = 40;
            this.ItemAlign = Mini.CellAlign.Center;
            this.Resizable = false;
            this.EditorMode = Mini2.EditorMode.None;
            base.MiType = "rownumberer";

        }
        

    }
}
