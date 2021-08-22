
using System.Text;
using EC5.Utility;
namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 工具栏-分隔符
    /// </summary>
    public class ToolBarHr : ToolBarItem
    {
        protected internal override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<div class=\"hr\" ");

            if (!string.IsNullOrEmpty(this.ID))
            {
                writer.Write("id=\"{0}\" ", this.ID);
            }

            if (this.Align == ToolBarItemAlign.Right)
            {
                writer.Write("style=\"float:{0};\" ", this.Align.ToString().ToLower());
            }

            writer.Write("></div>");
        }

        public override string GetConfigJS()
        {
            StringBuilder sb = new StringBuilder("{ type:'hr'");

            if (!this.Visible)
            {
                sb.Append(", visible:false");
            }

            sb.Append("}");

            return sb.ToString() ;
        }

    }


}
