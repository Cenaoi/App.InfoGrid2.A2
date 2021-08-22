using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 工具栏-版面
    /// </summary>
    [Description("工具栏-版面")]
    [ParseChildren(true, "Controls")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ToolBarPanel : ToolBarItem
    {

        Control m_Control;

        /// <summary>
        /// 子控件集合
        /// </summary>
        [Description("子控件集合")]
        public ControlCollection Controls
        {
            get
            {
                if (m_Control == null)
                {
                    m_Control = new Control();
                }

                return m_Control.Controls ;
            }
        }

        /// <summary>
        /// 将服务器组件输出为 Html
        /// </summary>
        /// <param name="writer"></param>
        protected internal override void Render(HtmlTextWriter writer)
        {
            if (m_Control == null)
            {
                return;
            }

            writer.Write("<span");
            
            if (this.Align == ToolBarItemAlign.Right)
            {
                writer.Write(" style='float:right;'");
            }

            writer.Write(">");

            m_Control.RenderControl(writer);

            writer.Write("</span>");
        }


    }
}
