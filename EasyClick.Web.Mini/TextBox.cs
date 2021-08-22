using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    [Description("文本框")]
    [DefaultProperty("Value")]
    [ToolboxData("<{0}:TextBox runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class TextBox : MiniHtmlBase
    {
        /// <summary>
        /// 文本框
        /// </summary>
        public TextBox()
        {
            this.Type = "text";
        }
    }
}
