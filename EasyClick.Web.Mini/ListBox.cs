using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 列表框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items")]
    [DefaultProperty("Value")]
    [Description("列表框")]
    public class ListBox : DropDownList
    {
        public ListBox()
            : base()
        {

        }

        /// <summary>
        /// 多选
        /// </summary>
        bool m_Multiple = false;

        /// <summary>
        /// 多选
        /// </summary>
        [DefaultValue(false)]
        public bool Multiple
        {
            get { return m_Multiple; }
            set { m_Multiple = value; }
        }

        protected override void RenderHtml(HtmlTextWriter writer)
        {
            if (m_Multiple)
            {
                this.SetAttribute("multiple", "multiple");
            }

            base.RenderHtml(writer);
        }

    }
}
