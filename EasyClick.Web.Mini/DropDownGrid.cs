using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 可编辑的下拉表格
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items")]
    [DefaultProperty("Value")]
    [Description("可编辑的下拉表格")]
    public class DropDownGrid : MiniHtmlListBase
    {
        /// <summary>
        /// 可编辑的下拉表格
        /// </summary>
        public DropDownGrid()
        {
            this.HtmlTag = System.Web.UI.HtmlTextWriterTag.Select;

        }

        protected virtual void Render_Script(HtmlTextWriter writer)
        {
            string width = "150px";

            writer.WriteLine("<script>");
            writer.WriteLine("var {0} = new Mini.ui.DropDownGrid({{id:'{0}',width:'{1}' }});", this.GetClientID(), width);
            writer.WriteLine("</script>");
        }

        string m_SetValueScript = "$('#{0}').val('{1}')";

        [DefaultValue("$('#{0}').val('{1}')")]
        public string SetValueScript
        {
            get { return m_SetValueScript; }
            set { m_SetValueScript = value; }
        }

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (!this.DesignMode && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    if (value == null)
                    {
                        MiniScript.Add(m_SetValueScript, GetClientID(), string.Empty);
                    }
                    else
                    {
                        string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(value);

                        MiniScript.Add(m_SetValueScript, GetClientID(), txt);
                    }
                }

                base.Value = value;
            }
        }

        protected override void RenderItems(HtmlTextWriter writer)
        {

            if (this.ShowEmptyItem)
            {


                if (this.EmptyItemValue == this.Value)
                {
                    writer.Write("<div value=\"{0}\">{1}</div>", this.EmptyItemValue, "");
                }
                else
                {
                    writer.Write("<div value=\"{0}\">{1}</div>", this.EmptyItemValue, "");
                }
            }

            foreach (ListItem item in this.Items)
            {
                if (string.IsNullOrEmpty(item.Value) && !string.IsNullOrEmpty(item.Text))
                {
                    item.Value = item.Text;
                }
                else if (!string.IsNullOrEmpty(item.Value) && string.IsNullOrEmpty(item.Text))
                {
                    item.Text = item.Value;
                }



                if (item.Value == this.Value)
                {
                    writer.Write("<div value=\"{0}\" selected=\"selected\"><b>{1}</b></div>", item.Value, item.Text);
                }
                else
                {
                    writer.Write("<div value=\"{0}\">{1}</div>", item.Value, item.Text);
                }

            }
        }

        protected override void RenderHtml(HtmlTextWriter writer)
        {

            writer.Write("<table border='0' cellpadding='0' cellspacing='0' class='Mini_DropDownText' id='{0}' >", this.GetClientID());
            {
                writer.Write("<tr>");
                {
                    writer.Write("<td><input type=\"text\" name=\"{0}\" class='input' value=\"{1}\" /></td>", this.GetClientID(), this.Value);
                    writer.Write("<td style='width:16px;'>");
                    {
                        writer.Write("<div class=\"select\">▼</div>");

                        //列表
                        writer.Write("<div id=\"{0}_dropDownList\" style=\"display:none;\" class=\"Mini_DropDownPanel\">", this.GetClientID());

                        //RenderItems(writer);

                        writer.Write("</div>");

                    }
                    writer.Write("</td>");
                }
                writer.Write("</tr>");
            }
            writer.Write("</table>");

            //Render_Script(writer);
        }
    }
}
