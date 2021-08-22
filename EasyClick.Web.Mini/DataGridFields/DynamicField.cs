using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.IO;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// 动态状态字段
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [Description("动态状态字段")]
    public class DynamicField : BoundField
    {
        public DynamicField()
        {

            m_AllowInput = false;
        }


        DynamicItemCollection m_Items;

        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public virtual DynamicItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new DynamicItemCollection();
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
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {

                StringBuilder sb = new StringBuilder();
                
                if (m_Items.Count > 0)
                {

                    DynamicItem item = m_Items[0];
                    item.m_Owner = this;

                    sb.Append("{#if ");
                    item.RenderLogin(sb);
                    sb.Append("}");
                    item.Render(sb);

                    for (int i = 1; i < m_Items.Count; i++)
                    {
                        item = m_Items[i];
                        item.m_Owner = this;

                        sb.Append("{#elseif ");
                        item.RenderLogin(sb);
                        sb.Append("}");
                        item.Render(sb);
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
