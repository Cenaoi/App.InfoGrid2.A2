using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// Head
    /// </summary>
    [Description("Head")]
    [DefaultProperty("Title")]
    [ToolboxData("<{0}:Head runat=\"server\" >\n    <Title>标题</Title>\n</{0}:Head>\n")]
    [ParseChildren(true),PersistChildren(false)]
    public class Head:Control
    {
        string m_Title;

        /// <summary>
        /// 标题
        /// </summary>
        [DefaultValue("")]
        [Description("标题")]
        [PersistenceMode(PersistenceMode.InnerProperty)] 
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<script>");
            writer.Write("$(document).ready(function(){ document.title = \"");
            writer.Write(m_Title);
            writer.Write("\"; });");
            writer.Write("</script>");
        }
    }
}
