using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.IO;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TextareaField : BoundField
    {
        int m_CellHeight = 60;

        public TextareaField()
        {
            m_AllowInput = true;
        }

        public int CellHeight
        {
            get { return m_CellHeight; }
            set { m_CellHeight = value; }
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            if (this.Width > 0)
            {
                writer.AddAttribute("width", this.Width + "px");
            }

            if (cellType == MiniDataControlCellType.Header)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(this.HeaderText);
                writer.RenderEndTag();
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.Write("<textarea style=\"width:{1}px;height{2}px;\" readonly=\"readonly\">{0}</textarea>",
                    GetDataBindExp(this.DataField),
                    this.Width, 
                    this.CellHeight);

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
