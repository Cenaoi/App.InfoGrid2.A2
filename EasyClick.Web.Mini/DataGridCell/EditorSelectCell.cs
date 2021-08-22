using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 下拉框
    /// </summary>
    [Description("下拉选择框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    public class EditorSelectCell : EditorTextCell
    {

        ListItemCollection m_Items;

        string m_OnSelectedChanged;

        /// <summary>
        /// 按钮事件
        /// </summary>
        [Description("项目发生变化的事件")]
        [DefaultValue("")]
        public string OnSelectedChanged
        {
            get { return m_OnSelectedChanged; }
            set { m_OnSelectedChanged = value; }
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

        private string GetItemJson(ListItem item)
        {
            if(string.IsNullOrEmpty(item.Value))
            {
                return string.Format("{{ value:\"{0}\", text: \"{0}\" }}",item.Text.Replace("\"","\\\""));
            }
            else
            {
                return string.Format("{{ value:\"{0}\", text: \"{1}\" }}",
                    item.Value.Replace("\"","\\\""),
                    item.Text.Replace("\"", "\\\""));
            }
        }

        public override string DebugModeCreateHtml(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (cellType == MiniDataControlCellType.DataCell)
            {
                return string.Format("<td>{{$T.{0}}}</td>", this.DataField);
            }
            else
            {
                return base.DebugModeCreateHtml(cellType, rowState);
            }
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            DataGridView owner = (DataGridView)this.Owner;

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
                this.CreateHeaderHtmlTemplate(writer, MiniDataControlCellType.Header, MiniDataControlRowState.Normal);
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                if (ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", ItemAlign.ToString().ToLower());
                }

                writer.AddAttribute("DBField", this.DataField);
                writer.AddAttribute("ColumnID", string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID);


                string isEditor = "editor ";
                string isReadOnly = (this.ReadOnly ? "readonly " : "");

                writer.AddAttribute("class", isEditor + isReadOnly);


                writer.RenderBeginTag(HtmlTextWriterTag.Td);


                StringBuilder sb = new StringBuilder();

                if (this.Items.Count > 0)
                {
                    ListItem item = this.Items[0];

                    sb.Append("{#if ").AppendFormat("$T.{0}=='{1}'", this.DataField, item.Value).Append("}");
                    sb.Append(item.Text);

                    for (int i = 1; i < this.Items.Count; i++)
                    {
                        item = this.Items[i];
                        sb.Append("{#elseif ").AppendFormat("$T.{0}=='{1}'", this.DataField, item.Value).Append("}");
                        sb.Append(item.Text);
                    }

                    sb.Append("{#else}{$T.").Append(this.DataField).Append("}{#/if}");
                }

                writer.Write(sb.ToString());
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

        public override void RenderClientScript(HtmlTextWriter writer)
        {
            if (this.ReadOnly)
            {
                return;
            }


            DataGridView owner = (DataGridView)this.Owner;
            string columnID = string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID;

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In('mi.EditorSelectCell', function(){");
            }

            writer.WriteLine("var {0}_EditorCell = new Mini.ui.EditorSelectCell({{", columnID);
            writer.WriteLine("  dataStoreStatusId:'{0}_DSStatus'", owner.GetClientID() );
            writer.WriteLine("});");

            writer.WriteLine("{0}_EditorCell.setGridView( {1} );", columnID, owner.GetClientID());

            if (!string.IsNullOrEmpty(m_OnSelectedChanged))
            {
                writer.WriteLine("{0}_EditorCell.selectedChanged(function(sender,e){{ {1} }});", columnID, m_OnSelectedChanged);
            }

            writer.WriteLine("{0}_EditorCell.setItems([",columnID);

            if (this.Items.Count > 0)
            {
                writer.Write(GetItemJson(Items[0]));
             
                for (int i = 1; i < this.Items.Count; i++)
                {
                    writer.Write(",");
                    writer.Write(GetItemJson(Items[i]));
                }
            }


            
            writer.Write("]);");


            writer.WriteLine("$('#{0} tbody td[ColumnID={1}]').mousedown(function () {{", owner.GetClientID(), columnID);
            writer.WriteLine("    {0}_EditorCell.show(this);", columnID);
            writer.WriteLine("});");

            writer.WriteLine("if({0}.setColumnEditor){{", owner.GetClientID());
            writer.WriteLine("    {0}.setColumnEditor('{1}', {1}_EditorCell);", owner.GetClientID(), columnID);
            writer.WriteLine("}");

            if (jsMode == "InJs")
            {
                writer.WriteLine("});");
            }

        }

    }
}
