using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 自动填充框
    /// </summary>
    [Description("自动填充框")]
    [DefaultProperty("Value")]
    [ToolboxData("<{0}:AutoCompleteBox  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class AutoCompleteBox : TextBox
    {
        bool m_VisibleButton = true;

        /// <summary>
        /// 显示按钮
        /// </summary>
        [DefaultValue(true)]
        public bool VisibleButton
        {
            get { return m_VisibleButton; }
            set { m_VisibleButton = value; }
        }

        /// <summary>
        /// 远程地址
        /// </summary>
        string m_DataRemotePath;

        /// <summary>
        /// 远程数据地址
        /// </summary>
        public string DataRemotePath
        {
            get { return m_DataRemotePath; }
            set { m_DataRemotePath = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                base.Render(writer);

                writer.Write("<button >...</button>");

                return;
            }

            base.Render(writer);

            if (m_VisibleButton)
            {
                writer.Write("<input type='button' value='…' onclick='MadeIn_Show()' />");
            }

            DateTime minDate = DateTime.Today;

            writer.WriteLine("<script type=\"text/javascript\">");
            writer.WriteLine("<!--");

            writer.WriteLine("$(document).ready(function() {");
            writer.WriteLine("  $('#{0}').autocomplete({{", GetClientID() );



            writer.WriteLine("});");

            writer.WriteLine("-->");
            writer.WriteLine("</script>");


        }
    }
}
