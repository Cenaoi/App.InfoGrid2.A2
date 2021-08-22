using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 树下拉框
    /// </summary>
    [Description("树下拉框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.UI.ParseChildren(true, "Items"), System.Web.UI.PersistChildren(false)]
    public class TreeColumn
    {

        public string IdField { get; set; }
        public string ParentField { get; set; }

        public string ValueField { get; set; }
        public string DisplayField { get; set; }

        public string RootValue { get; set; }

        /// <summary>
        /// 实体名称,或表名
        /// </summary>
        public string Model { get; set; }

        public string SortText { get; set; }
        public string SortField { get; set; }
    }
}
