using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;

namespace EasyClick.Web.Mini
{
    [Description("日期选择框")]
    public class EditorDateCell : EditorTextCell
    {
        public EditorDateCell()
        {
            this.Width = 70;
            this.DataFormatString = "{0:yyyy-MM-dd}";
        }

        [DefaultValue("{0:yyyy-MM-dd}")]
        public override string DataFormatString
        {
            get
            {
                return base.DataFormatString;
            }
            set
            {
                base.DataFormatString = value;
            }
        }

        [DefaultValue(70)]
        public override int Width
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

        public override void RenderClientScript(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.ReadOnly)
            {
                return;
            }

            if (Owner is DataGridView && ((DataGridView)Owner).ReadOnly)
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
                writer.WriteLine("In('mi.EditorDateCell', function(){");
            }

            writer.WriteLine("var {0}_EditorCell = new Mini.ui.EditorDateCell({{", columnID);
            if (this.ImeMode != EditorImeMode.Auto)
            {
                writer.WriteLine("imeMode:'{0}',", this.ImeMode.ToString().ToLower());
            }
            writer.WriteLine("dataStoreStatusId:'{0}'", owner.GetClientID() + "_DSStatus");
            writer.WriteLine("});");

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
