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
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TextBoxField : BoundField,IMiniInputField
    {

        string m_Name;
        string m_ID;

        public TextBoxField()
        {
            m_AllowInput = true;
        }

        [DefaultValue("")]
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
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
                //writer.RenderBeginTag(HtmlTextWriterTag.Th);
                //writer.Write(this.HeaderText);
                //writer.RenderEndTag();

                this.CreateHeaderHtmlTemplate(writer, cellType, MiniDataControlRowState.Normal);
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                string conName;
                if (string.IsNullOrEmpty(m_Name) && !string.IsNullOrEmpty(m_ID))
                {
                    conName = string.Format("{0}_{{$P.guid}}${1}", Owner.ClientID, m_ID);
                }
                else
                {
                    conName = m_Name;
                }
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("<input type='text' style=\"width:{0}px;\" name=\"{2}\" value=\"{1}\" ", this.Width, GetDataBindExp(this.DataField), conName);

                foreach (MiniHtmlAttr attr in this.m_HtmlAttrs)
                {
                    writer.Write("{0}=\"{1}\" ", attr.Key, attr.Value);
                }

                writer.Write("/>");

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
