using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini.Utility;
using System.Reflection;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 轻量级表格模板
    /// </summary>
    [DefaultProperty("ItemTemplate")]
    [ParseChildren(true, "ItemTemplate")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:TableTemplate  runat=\"server\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n\t<ItemTemplate>\r\n\t</ItemTemplate>\r\n</{0}:TableTemplate>")]
    public class TableTemplate : Template
    {
        public TableTemplate()
        {
            
        }

        int m_ColCount = 4;

        /// <summary>
        /// 列数量
        /// </summary>
        public int ColCount
        {
            get { return m_ColCount; }
            set { m_ColCount = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            

            if (this.DesignMode )
            {
                writer.Write(this.ItemTemplate);

                return;
            }

            if (this.EcDesignMode)
            {
                Render_DesignMode(writer);

                return;
            }


            writer.Write("<script type='text/html' id='{0}_TemplateCode'>", GetClientID());
            writer.Write(this.ItemTemplate);
            writer.Write("</script>");

            writer.Write("<table border='0' cellspacing='0' cellpadding='0' id='{0}'",GetClientID());

            foreach (string key in m_HtmlAttrs.Keys)
            {
                if (key == "id" || key == "type" || key == "name" || key == "value")
                {
                    continue;
                }

                
                //if (MiniConfiguration.ServerAttrTags != null)
                //{
                //    string[] serTags = MiniConfiguration.ServerAttrTags;

                //    bool exist = false;

                //    for (int i = 0; i < serTags.Length; i++)
                //    {
                //        if (key.Equals( serTags[i], StringComparison.OrdinalIgnoreCase))
                //        {
                //            exist = true;
                //            break;
                //        }
                //    }

                //    if (exist)
                //    {
                //        continue;
                //    }
                //}

                MiniHtmlAttr attr = m_HtmlAttrs[key];

                writer.Write(" {0}=\"{1}\"", attr.Key, attr.Value);
            }

            writer.Write(">");
            writer.Write("<tbody></tbody>");
            writer.Write("</table>");

            writer.WriteLine("<script type=\"text/javascript\">");

            writer.WriteLine("var {0} = null;", GetClientID());

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.TableTemplate',function () {");
            }
            else
            {
                writer.WriteLine("$(document).ready(function () {");
            }

            writer.WriteLine("      {0} = new Mini.ui.TableTemplate({{id:'{0}', colCount:{1} }});", GetClientID(),m_ColCount);

            //writer.Write("{0}.addItem({{'name':'核武器','age':23 }});\r\n",this.ClientID);

            string[] fs = null;

            if (this.FilterField)
            {
                fs = GetFieldNames(this.ItemTemplate);
            }

            if (this.Items.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("try { ");
                sb.AppendLine();

                sb.AppendFormat("{0}.addItemRange([", GetClientID());

                sb.AppendLine();
                sb.AppendLine(GetItemJson(this.Items[0], this.DataFormats, fs));

                for (int i = 1; i < this.Items.Count; i++)
                {
                    sb.Append(",").AppendLine();
                    sb.Append(GetItemJson(this.Items[i], this.DataFormats, fs));
                }

                sb.AppendLine("]);");

                sb.AppendLine();
                sb.AppendLine("} catch(ex) { alert(ex.Message); }");
                sb.AppendLine();

                writer.WriteLine(sb.ToString());
            }

            writer.WriteLine("});");



            writer.WriteLine("</script>");
        }


    }
}
