using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    [Description("文本编辑框和按钮")]
    public class EditorTextButtonCell : EditorTextCell
    {
        string m_OnButtonClick;

        /// <summary>
        /// 按钮事件
        /// </summary>
        [Description("按钮事件")]
        [DefaultValue("")]
        public string OnButtonClick
        {
            get { return m_OnButtonClick; }
            set { m_OnButtonClick = value; }
        }

        public override void RenderClientScript(System.Web.UI.HtmlTextWriter writer)
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
                writer.WriteLine("In('mi.EditorTextButtonCell', function(){");
            }

            writer.WriteLine("var {0}_EditorCell = new Mini.ui.EditorTextButtonCell({{", columnID);
            if (this.ImeMode != EditorImeMode.Auto)
            {
                writer.WriteLine("imeMode:'{0}',", this.ImeMode.ToString().ToLower());
            }
            writer.WriteLine("dataStoreStatusId:'{0}'", owner.GetClientID() + "_DSStatus");
            writer.WriteLine("});");

            if (!string.IsNullOrEmpty(m_OnButtonClick))
            {
                writer.WriteLine("{0}_EditorCell.buttonClick(function(sender,e){{ {1} }});", columnID, m_OnButtonClick);
            }

            writer.WriteLine("{0}_EditorCell.setGridView( {1} );", columnID, owner.GetClientID());
            //writer.WriteLine("{0}_EditorCell.setDataStore({1}.getDataStore());", columnID, owner.GetClientID());

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
