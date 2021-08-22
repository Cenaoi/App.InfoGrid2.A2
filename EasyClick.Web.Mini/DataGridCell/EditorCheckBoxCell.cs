using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.IO;
using System.ComponentModel;
using System.Web;

namespace EasyClick.Web.Mini
{
    [Description("复选框")]
    public class EditorCheckBoxCell: EditorTextCell 
    {
        string m_Name;

        string m_ID;

        [System.ComponentModel.DefaultValue("")]
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

        CheckBoxHeaderMode m_HeaderMode = CheckBoxHeaderMode.Common;

        [DefaultValue(CheckBoxHeaderMode.SelectAll)]
        public CheckBoxHeaderMode HeaderMode
        {
            get { return m_HeaderMode; }
            set { m_HeaderMode = value; }
        }

        public EditorCheckBoxCell()
        {
            this.Width = 16;
            m_AllowInput = true;
        }


        private new string OnClientTextChanged
        {
            get { return base.OnClientTextChanged; }
            set { base.OnClientTextChanged = value; }
        }


        public override void RenderClientScript(HtmlTextWriter writer)
        {
            DataGridView owner = (DataGridView)this.Owner;

            string columnID = string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID;

            //string colId = string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID;
            string colId = this.Owner.ID + "_" + columnID;

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In('mi.EditorCheckBoxCell', function(){");
            }

            writer.WriteLine("var {0}_EditorCell = new Mini.ui.EditorCheckBoxCell({{", columnID);
            writer.WriteLine("dataStoreStatusId:'{0}'", owner.GetClientID() + "_DSStatus");
            writer.WriteLine("});");

            writer.WriteLine("{0}_EditorCell.setGridView( {1} );", columnID, owner.GetClientID());
            //writer.WriteLine("{0}_EditorCell.setDataStore({1}.getDataStore());", columnID, owner.GetClientID());
            //writer.WriteLine("$('#{0} tbody td[ColumnID={1}]').mousedown(function () {{", owner.GetClientID(), columnID);
            //writer.WriteLine("    {0}_EditorCell.show(this);", columnID);
            //writer.WriteLine("});");

            writer.WriteLine("if({0}.setColumnEditor){{", owner.GetClientID());
            writer.WriteLine("    {0}.setColumnEditor('{1}', {1}_EditorCell);", owner.GetClientID(), columnID);
            writer.WriteLine("}");

            writer.WriteLine("window.{0}_EditorCell = {1}_EditorCell;", colId, columnID);

            if (jsMode == "InJs")
            {
                writer.WriteLine("});");
            }
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            DataGridView gridView = this.Owner as DataGridView;

            

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

            if (cellType == MiniDataControlCellType.Header)
            {
                if (m_HeaderMode == CheckBoxHeaderMode.Common)
                {
                    this.CreateHeaderHtmlTemplate(writer, MiniDataControlCellType.Header, MiniDataControlRowState.Normal);
                }
                else
                {
                    if (this.ItemAlign != CellAlign.Left)
                    {
                        writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                    }

                    writer.AddAttribute("nowrap", "nowrap");
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);

                    writer.AddAttribute("type", "checkbox");
                    writer.AddAttribute("onclick", "if($(this).attr('checked') == undefined){$(':checkbox[name=" + conName + "]').removeAttr('checked');}else{$(':checkbox[name=" + conName + "]').attr('checked','checked');};");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();


                    writer.RenderEndTag();
                }
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                //if (this.Required)
                //{
                //    writer.AddAttribute("class", "Required");
                //}


                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                if (this.ReadOnly || (gridView != null && gridView.ReadOnly))
                {
                    writer.Write("{#if $T." + this.DataField + "=='True'}");
                    writer.Write("√");
                    writer.Write("{#/if}");
                }
                else
                {
                    string colId = string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID;
                    colId = this.Owner.ID + "_" + colId;

                    writer.AddAttribute("type", "checkbox");

                    writer.AddAttribute("value", GetDataBindExp(this.DataField));
                    writer.AddAttribute("DBField", this.DataField);
                    writer.AddAttribute("ColumnID", string.IsNullOrEmpty(this.ID) ? this.DataField : this.ID);

                    writer.AddAttribute("onclick", string.Format("{0}_EditorCell.click(this)", colId));

                    writer.AddAttribute("{#if $T." + this.DataField + "=='True'}checked{#/if}", "{#if $T." + this.DataField + "=='True'}checked{#/if}");

                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
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
