using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Security.Permissions;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CheckColumn : BoundField
    {
        public CheckColumn()
        {

            base.MiType = "checkcolumn";
            this.ItemAlign = Mini.CellAlign.Center;
            this.Width = 40;
        }

        public CheckColumn(string dataField)
        {

            base.MiType = "checkcolumn";
            this.ItemAlign = Mini.CellAlign.Center;
            this.Width = 40;


            this.DataField = dataField;
        }

        public CheckColumn(string dataField, string headerText)
        {
            base.MiType = "checkcolumn";
            this.ItemAlign = Mini.CellAlign.Center;
            this.Width = 40;

            this.DataField = dataField;
            this.HeaderText = headerText;
        }


    }
}
