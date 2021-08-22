using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.IO;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 图像列表
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ImageField : BoundField
    {

        string m_ImageUrl;

        int m_ImageWidth = 0;
        int m_ImageHeight = 0;

        /// <summary>
        /// 图像 Url
        /// </summary>
        [DefaultValue("")]
        public string ImageUrl
        {
            get { return m_ImageUrl; }
            set { m_ImageUrl = value; }
        }

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
                DataGrid grid = m_Owner as DataGrid;

                if (!string.IsNullOrEmpty(this.HeaderTooltip))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, this.HeaderTooltip);
                }

                writer.AddAttribute("nowrap", "nowrap");
                writer.RenderBeginTag(HtmlTextWriterTag.Th);

                if ((grid != null && !grid.AllowSorting) || this.SortMode == Mini.SortMode.None)
                {
                    writer.Write(this.HeaderText);
                }
                else if (this.SortMode == Mini.SortMode.Default)
                {
                    if (grid == null || string.IsNullOrEmpty(this.SortExpression))
                    {
                        writer.Write(this.HeaderText);
                    }
                    else
                    {
                        writer.AddAttribute("href", "#" + this.SortExpression);
                        writer.AddAttribute("onclick", string.Format("{0}.sort(this,'{1}',{2})", grid.GetClientID(), this.SortExpression, this.Index));

                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(this.HeaderText);
                        writer.RenderEndTag();
                    }
                }
                else if (this.SortMode == Mini.SortMode.User)
                {

                }

                writer.RenderEndTag();

                return tw.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<td ");
                
                if (this.ItemAlign != CellAlign.Left)
                {
                    sb.AppendFormat("align=\"{0}\" ", this.ItemAlign.ToString().ToLower());
                }


                sb.Append(">");

                sb.AppendFormat("<img src=\"{0}\" ", m_ImageUrl);

                if (m_ImageWidth > 0) { sb.AppendFormat("width=\"{0}px\" ", m_ImageWidth); }
                if (m_ImageHeight > 0) { sb.AppendFormat("height=\"{0}px\" ", m_ImageHeight); }

                sb.Append("border=\"0\" />");

                sb.Append("</td>");

                return sb.ToString();
            }
        }
    }
}
