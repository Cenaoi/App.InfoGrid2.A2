using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.IO;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 超链接
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [IODescription("超链接")]
    public class HyperLinkField : BoundField
    {
        string m_NavigateUrl;
        string m_Target;
        string m_Text;
        string m_ImageUrl;
        string m_OnClientClick;

        [DefaultValue("")]
        public string NavigateUrl
        {
            get { return m_NavigateUrl; }
            set { m_NavigateUrl = value; }
        }

        [DefaultValue("")]
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        [DefaultValue("")]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        [DefaultValue("")]
        public string ImageUrl
        {
            get { return m_ImageUrl; }
            set { m_ImageUrl = value; }
        }

        int m_ImageWidth = 0;
        int m_ImageHeight = 0;

        [DefaultValue(0)]
        public int ImageWidth
        {
            get { return m_ImageWidth; }
            set { m_ImageWidth = value; }
        }

        [DefaultValue(0)]
        public int ImageHeight
        {
            get { return m_ImageHeight; }
            set { m_ImageHeight = value; }
        }

        [DefaultValue("")]
        public string OnClientClick
        {
            get { return m_OnClientClick; }
            set { m_OnClientClick = value; }
        }

        [DefaultValue(0)]
        private new string DataField
        {
            get { return base.DataField; }
            set { base.DataField = value; }
        }

        [DefaultValue(0)]
        public string DataTextField { get; set; }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            if (this.Width > 0)
            {
                writer.AddAttribute("width", this.Width + "px");
            }

            if (!this.Visible)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }


            if (cellType == MiniDataControlCellType.Header)
            {
                this.CreateHeaderHtmlTemplate(writer, cellType, MiniDataControlRowState.Normal);
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                writer.AddAttribute("nowrap", "nowrap");

                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.AddAttribute("href", GetDataBindExp(m_NavigateUrl));

                if (!string.IsNullOrEmpty(m_OnClientClick))
                {
                    writer.AddAttribute("onclick", m_OnClientClick);
                }

                if (!string.IsNullOrEmpty(m_Target))
                {
                    writer.AddAttribute("target", m_Target);
                }

                writer.RenderBeginTag(HtmlTextWriterTag.A);

                if (!string.IsNullOrEmpty(m_ImageUrl))
                {
                    writer.Write("<img src=\"{0}\" border=\"0\" ",m_ImageUrl);

                    if (m_ImageWidth != 0) { writer.Write("width=\"{0}px\" ", m_ImageWidth); }
                    if (m_ImageHeight != 0) { writer.Write("height=\"{0}px\" ", m_ImageHeight); }

                    writer.Write("/>");
                }
                else
                {
                    if (string.IsNullOrEmpty(DataTextField))
                    {
                        writer.Write(this.Text);
                    }
                    else
                    {
                        writer.Write(GetDataBindExp(this.DataTextField));
                    }
                }

                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("&nbsp;");
                writer.RenderEndTag();
            }

            string txt = tw.ToString();

            writer.Dispose();
            tw.Dispose();

            return txt;

        }
    }
}
