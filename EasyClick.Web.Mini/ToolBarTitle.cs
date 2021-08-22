

namespace EasyClick.Web.Mini
{
    public class ToolBarTitle : ToolBarItem
    {
        string m_Text = "标题";

        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        protected internal override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<span class='titleR' ><span class='titleL'>");

            writer.Write(m_Text);
            
            writer.Write("</span></span>");
        }
    }
}
