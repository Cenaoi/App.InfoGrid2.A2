using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 下拉选择编辑框
    /// </summary>
    [Description("下拉选择编辑框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    public class EditorSelect2Cell : EditorTextCell
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
                writer.WriteLine("In('mi.EditorSelect2Cell', function(){");
            }

            writer.WriteLine("var {0}_EditorCell = new Mini.ui.EditorSelect2Cell({{", columnID);
            if (this.ImeMode != EditorImeMode.Auto)
            {
                writer.WriteLine("imeMode:'{0}',", this.ImeMode.ToString().ToLower());
            }
            writer.WriteLine("dataStoreStatusId:'{0}'", owner.GetClientID() + "_DSStatus");
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
