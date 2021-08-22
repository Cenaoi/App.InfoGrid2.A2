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
    /// 隐藏列
    /// </summary>
    [DisplayName("隐藏列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HideField : BoundField, IMiniInputField
    {
        [DefaultValue(false)]
        public override bool Visible
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        [Description("宽度.像素 px单位")]
        [DefaultValue(0)]
        private new int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        string m_Name;

        string m_ID;

        public HideField()
        {
            m_AllowInput = true;
        }

        [DefaultValue("")]
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        [DefaultValue("")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {


            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            writer.AddStyleAttribute("display", "none");

            if (cellType == MiniDataControlCellType.Header)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(this.HeaderText);
                writer.RenderEndTag();
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

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("<input type=\"hidden\" name=\"{1}\" value=\"{0}\" />", GetDataBindExp( this.DataField), conName);
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
