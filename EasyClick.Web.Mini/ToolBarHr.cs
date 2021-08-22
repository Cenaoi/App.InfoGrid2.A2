
namespace EasyClick.Web.Mini
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
    }


}
