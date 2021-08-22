using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    public class EmptyField:BoundField
    {
        public EmptyField()
        {
            this.ToExcel = false;
        }


        [EditorBrowsable( EditorBrowsableState.Never)]
        [Browsable(false)]
        [DefaultValue(false)]
        public override bool ToExcel
        {
            get
            {
                return base.ToExcel;
            }
            set
            {
                base.ToExcel = value;
            }
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            if (!this.Visible)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }

            if (cellType == MiniDataControlCellType.Header)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write("&nbsp;");
                writer.RenderEndTag();
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("&nbsp;");
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
