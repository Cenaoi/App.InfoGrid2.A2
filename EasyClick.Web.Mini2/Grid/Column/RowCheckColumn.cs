using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini2
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RowCheckColumn : BoundField
    {
        public RowCheckColumn()
        {

            base.MiType = "rowcheckcolumn";
            this.ItemAlign = Mini.CellAlign.Center;
            this.HeaderAlign = Mini.CellAlign.Center;
            this.Width = 40;
        }



    }
}
