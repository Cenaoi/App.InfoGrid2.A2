using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 指定 Literal 控件中的内容如何呈现。
    /// </summary>
    public enum LiteralMode
    {
        /// <summary>
        /// 移除文本控件中不受支持的标记语言元素。如果文本控件在支持 HTML 或 XHTML 的浏览器上呈现，则不修改控件的内容。
        /// </summary>
        Transform = 0,

        /// <summary>
        /// 不修改文本控件的内容。
        /// </summary>
        PassThrough = 1,

        /// <summary>
        /// 对文本控件的内容进行 HTML 编码。
        /// </summary>
        Encode = 2
    }


    /// <summary>
    /// 在网页上保留显示静态文本的位置。
    /// </summary>
    [Description("在网页上保留显示静态文本的位置。")]
    public class Literal : System.Web.UI.Control
    {


        /// <summary>
        /// (构造函数) 在网页上保留显示静态文本的位置
        /// </summary>
        public Literal()
        {

        }

        /// <summary>
        /// (构造函数) 在网页上保留显示静态文本的位置
        /// </summary>
        /// <param name="text"></param>
        public Literal(string text)
        {
            this.Text = text;
        }


        /// <summary>
        /// 获取或设置在 Literal 控件中显示的标题。
        /// </summary>
        [Description("获取或设置在 Literal 控件中显示的标题。")]
        public string Text { get; set; }

        /// <summary>
        /// 呈现模式
        /// </summary>
        [DefaultValue(LiteralMode.Transform)]
        [Description("")]
        public LiteralMode Mode { get; set; } = LiteralMode.Transform;


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }

            if (this.Mode == LiteralMode.Transform)
            {
                writer.Write(this.Text);
            }
            else if(this.Mode == LiteralMode.Encode)
            {
                string html = System.Web.HttpContext.Current.Server.HtmlEncode(this.Text);

                writer.Write(html);
            }
            
        }
    }
}
