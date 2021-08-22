using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 新行
    /// </summary>
    public class NewLine:Control
    {
        /// <summary>
        /// (构造函数) 
        /// </summary>
        public NewLine()
        {

        }

        /// <summary>
        /// 线尺寸
        /// </summary>
        int m_BorderWidth = 0;

        string m_Text;

        public new string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 线宽度
        /// </summary>
        [DefaultValue(0)]
        public int BorderWidth { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div class=\"mi-newline ");

            if(m_BorderWidth > 0)
            {
                writer.Write("mi-newline-border ");
            }

            writer.Write("\">");

            if (!string.IsNullOrEmpty(this.m_Text))
            {
                writer.Write("<div class=\"mi-newline-text\">");
                writer.Write(this.m_Text);
                writer.Write("</div>");
            }

            writer.Write("</div>");
        }

    }
}
