using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.Web.UI;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 工具栏
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [Description("工具栏")]
    public class ToolBarGroup : ToolBarItem
    {
        ToolBarItemCollection m_Items;


        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public ToolBarItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ToolBarItemCollection();
                }
                return m_Items;
            }
        }

        protected internal override void Render(System.Web.UI.HtmlTextWriter writer)
        {


            writer.Write("<span");

            if (this.Align == ToolBarItemAlign.Right)
            {
                writer.Write(" style='float:right;'");
            }

            writer.Write(">");


            foreach (ToolBarItem item in this.Items)
            {
                if (!item.Visible)
                {
                    continue;
                }

                item.Render(writer);
            }

            writer.Write("</span>");
        }

        SecControlCollection m_SecControls;

        /// <summary>
        /// 权限组件的集合
        /// </summary>
        public override SecControlCollection SecControls
        {
            get
            {
                if (m_SecControls == null)
                {
                    m_SecControls = new SecControlCollection();

                    foreach (ToolBarItem item in this.Items)
                    {
                        m_SecControls.Add(item);
                    }
                }

                return m_SecControls;
            }
        }

    }


}
