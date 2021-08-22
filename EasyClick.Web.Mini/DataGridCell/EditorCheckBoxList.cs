using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 复选框
    /// </summary>
    [Description("复选框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    public class EditorCheckBoxList : BoundField
    {
        ListItemCollection m_Items;



        string m_ID;

        [System.ComponentModel.DefaultValue("")]
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }



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
                CreateHeaderHtmlTemplate(writer, MiniDataControlCellType.Header, rowState);
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {


                writer.AddAttribute("ColumnID", this.DataField);
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                if (m_HtmlAttrs != null)
                {
                    foreach (MiniHtmlAttr attr in m_HtmlAttrs.Values)
                    {
                        writer.AddAttribute(attr.Key, attr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(this.Tooltip))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, this.Tooltip);
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                //if (string.IsNullOrEmpty(m_NotDisplayValue))
                //{
                //    writer.Write(GetDataBindExp(this.DataField));
                //}
                //else
                //{
                //    writer.Write("{{#if $T.{0} == '{1}' }}&nbsp;{{#else}}{{$T.{0}}}{{#/if}}", this.DataField, m_NotDisplayValue);
                //}

                
                string fieldGuid;

                if(!string.IsNullOrEmpty(m_ID))
                {
                    fieldGuid = Owner.ClientID + "_" + m_ID;
                }
                else
                {
                    fieldGuid =  Owner.ClientID + "_" + this.DataField;
                }



                writer.AddAttribute("DBField", this.DataField);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.WriteLine();

                string checkedText = "checked=\"checked\" ";

                foreach (ListItem item in this.Items)
                {
                    //<input type="checkbox" checked="checked" id="ROLE_IDS$ADMIN" /><label for="ROLE_IDS$ADMIN">管理员</label>

                    writer.RenderBeginTag(HtmlTextWriterTag.Label);


                    string colId = string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID;
                    colId = this.Owner.ID + "_" + colId;

                    string itemId = colId + "$" + item.Value;

                    writer.AddAttribute("type", "checkbox");

                    writer.AddAttribute("value", item.Value );
                    writer.AddAttribute("DBField", this.DataField);
                    writer.AddAttribute("ColumnID",itemId);

                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();

                    writer.Write( item.Text );

                    writer.RenderEndTag();

                    writer.Write("&nbsp;");
                    writer.WriteLine();

                }


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
