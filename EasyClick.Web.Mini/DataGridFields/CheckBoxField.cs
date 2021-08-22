using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    public enum CheckBoxHeaderMode
    {
        Common,
        SelectAll
    }

    /// <summary>
    /// 复选框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CheckBoxField : BoundField, IMiniInputField 
    {
        string m_Name;
        string m_ID;
        CheckBoxHeaderMode m_HeaderMode = CheckBoxHeaderMode.SelectAll;

        public CheckBoxField()
        {
            this.ItemAlign = CellAlign.Center;
            this.Width = 16;
            m_AllowInput = true;
        }

        [DefaultValue(CellAlign.Center)]
        public override CellAlign ItemAlign
        {
            get
            {
                return base.ItemAlign;
            }
            set
            {
                base.ItemAlign = value;
            }
        }

        [DefaultValue("")]
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


        [DefaultValue(CheckBoxHeaderMode.SelectAll)]
        public CheckBoxHeaderMode HeaderMode
        {
            get { return m_HeaderMode; }
            set { m_HeaderMode = value; }
        }

        [DefaultValue("")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        static Random m_Random;

        static CheckBoxField()
        {
            Random ro = new Random(10);
            long tick = DateTime.Now.Ticks;
            m_Random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        int m_ColGuid = 0; 

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (m_ColGuid == 0)
            {
                m_ColGuid = m_Random.Next();
            }

            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            string conName;
            if (string.IsNullOrEmpty(m_Name) && !string.IsNullOrEmpty(m_ID))
            {
                conName = string.Format("{0}_{{$P.guid}}${1}", Owner.ClientID, m_ID);
            }
            else
            {
                conName = m_Name;
            }

            if (this.Width > 0)
            {
                writer.AddAttribute("width", this.Width + "px");
            }

            if (!this.Visible)
            {
                writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none");
            }

            string colGuid = "C" + m_ColGuid;

            if (cellType == MiniDataControlCellType.Header)
            {
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Th);

                if (m_HeaderMode == CheckBoxHeaderMode.Common)
                {
                    writer.Write(this.HeaderText);
                }
                else
                {
                    writer.AddAttribute("type", "checkbox");
                    writer.AddAttribute("onclick", "if($(this).attr('checked') == undefined){$(':checkbox[" + colGuid + "][name=" + conName + "]').removeAttr('checked');}else{$(':checkbox[" + colGuid + "][name=" + conName + "]').attr('checked','checked');};");
                    
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {


                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                if (m_HeaderMode == CheckBoxHeaderMode.SelectAll)
                {
                    writer.AddAttribute("checkMode", "true");
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.AddAttribute("type", "checkbox");

                writer.AddAttribute("name", conName);

                writer.AddAttribute("value", GetDataBindExp(this.DataField) );

                writer.AddAttribute(colGuid, "");

                writer.RenderBeginTag(HtmlTextWriterTag.Input);
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
