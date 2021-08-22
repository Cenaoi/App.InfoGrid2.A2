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
    /// 下拉框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    public class DropDownListField : BoundField,IMiniInputField
    {
        string m_Name;

        string m_ID;

        public DropDownListField()
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

        ListItemCollection m_Items;

        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public virtual ListItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ListItemCollection();
                }

                return m_Items;
            }

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
                this.CreateHeaderHtmlTemplate(writer, cellType, MiniDataControlRowState.Normal);
                //writer.RenderBeginTag(HtmlTextWriterTag.Th);
                //writer.Write(this.HeaderText);
                //writer.RenderEndTag();
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

                //writer.AddAttribute("type", "checkbox");

                writer.AddAttribute("name", conName);

                writer.AddAttribute("value", GetDataBindExp(this.DataField) );

                writer.RenderBeginTag(HtmlTextWriterTag.Select);

                StringBuilder sb = new StringBuilder();

                foreach (ListItem item in this.Items)
                {
                    sb.Append("<option ");
                    sb.AppendFormat("value=\"{0}\" ", item.Value);

                    sb.Append("{#if ");
                    sb.AppendFormat("$T.{0}=='{1}'", this.DataField, item.Value);
                    sb.Append("}selected='selected'{#/if}");
                    

                    sb.Append(">");
                    sb.Append(item.Text);
                    sb.Append("</option>");

                    //writer.AddAttribute("value", item.Value);
                    //writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    //writer.Write(item.Text);
                    //writer.RenderEndTag();
                }
                writer.Write(sb.ToString());



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

        public override string DebugModeCreateHtml(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<td>");

            sb.Append("<select >");
            if (this.Items.Count > 0)
            {
                ListItem item = this.Items[0];
                sb.Append("<option ");
                sb.AppendFormat("value=\"{0}\" ", item.Value);

                sb.Append("{#if ");
                sb.AppendFormat("$T.{0}=='{1}'", this.DataField, item.Value);
                sb.Append("} selected='selected' {/#if}");

                sb.Append(">");
                sb.Append(item.Text);
                sb.Append("</option>");

            }
            sb.Append("</select>");

            sb.Append("</td>");

            return sb.ToString();
        }
    }
}
