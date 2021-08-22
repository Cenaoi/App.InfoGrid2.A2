

using System.ComponentModel;
using System.Text;
using EC5.Utility;
namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 工具栏-标题
    /// </summary>
    public class ToolBarTitle : ToolBarItem
    {
        /// <summary>
        /// (构造函数)工具栏-标题
        /// </summary>
        public ToolBarTitle()
        {

        }

        /// <summary>
        /// (构造函数)工具栏-标题
        /// </summary>
        /// <param name="text">标题文字</param>
        public ToolBarTitle(string text)
        {
            this.Text = text;
        }

        string m_Text = "标题";

        /// <summary>
        /// 标题文字
        /// </summary>
        [Description("标题文字")]
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

        public override string GetConfigJS()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("type:'{0}'", "title");
            sb.AppendFormat(",text:'{0}'", this.Text);

            sb.AppendFormat(",dock:'{0}'", this.Align.ToString().ToLower());

            if (!this.Visible)
            {
                sb.Append(", visible:false");
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
