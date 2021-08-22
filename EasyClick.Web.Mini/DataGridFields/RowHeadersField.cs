using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    public class RowHeadersField:BoundField
    {
        public RowHeadersField()
        {
            this.Width = 28;
            this.HeaderText = "&nbsp;";
            this.ItemAlign = CellAlign.Center;
            this.ToExcel = false;
        }


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
            DataGridView grid = this.Owner as DataGridView;

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

                string className = "RowHeader";

                if (grid != null)
                {
                    className += " {#if $T." + grid.LockedKey + ".toLowerCase() == 'true'}locked{#/if}";
                }

                writer.AddAttribute("class", className);

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                if(!string.IsNullOrEmpty(this.DataField) && this.DataField.StartsWith("{") && this.DataField.EndsWith("}") )
                {
                    writer.Write(this.DataField);
                }
                else if (!string.IsNullOrEmpty(this.DataField))
                {
                    //writer.Write(string.Format("{{$T.{0}}}", this.DataField));
                    writer.Write(GetDataBindExp(this.DataField));
                }
                else
                {
                    writer.Write("{$P.guid}");
                }

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
