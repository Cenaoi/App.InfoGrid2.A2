using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.ComponentModel;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 单选项
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    public class RadioListField : BoundField, IMiniInputField
    {
        public RadioListField()
        {

            m_AllowInput = false;
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

        public override string DebugModeCreateHtml(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (cellType == MiniDataControlCellType.DataCell)
            {
                return string.Format("<td>{0}</td>", this.DataField);
            }

            return base.DebugModeCreateHtml(cellType, rowState);
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

            if (this.ItemAlign != CellAlign.Left)
            {
                writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
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

                StringBuilder sb = new StringBuilder();
                
                if (m_Items.Count > 0)
                {

                    ListItem item = m_Items[0];

                    sb.Append("{#if ");
                    sb.AppendFormat("$T.{0} == '{1}'", this.DataField, item.Value);
                    sb.Append("}");
                    sb.Append(item.Text);

                    for (int i = 1; i < m_Items.Count; i++)
                    {
                        item = m_Items[i];

                        sb.Append("{#elseif ");
                        sb.AppendFormat("$T.{0} == '{1}'", this.DataField, item.Value);
                        sb.Append("}");
                        sb.Append(item.Text);
                    }

                    sb.Append("{#/if}");
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(sb.ToString());
                writer.RenderEndTag();

            }

            return tw.ToString();
        }



        #region IMiniInputField 成员

        public string ID
        {
            get
            {
                return string.Empty;
            }
        }

        public string Name
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
