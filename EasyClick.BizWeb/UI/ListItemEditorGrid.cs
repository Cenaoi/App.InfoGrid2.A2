using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using EasyClick.Web.Mini;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.BizWeb.UI
{
    /// <summary>
    /// 属性编辑表格
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class ListItemEditorGrid : DataGridView
    {

        public ListItemEditorGrid()
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            this.SetAttribute("class", "table1");
            this.SetAttribute("cellspacing", "0");
            this.SetAttribute("rules", "all");

            this.FilterField = false;
            this.PagerVisible = false;
            this.ScrollBars = ScrollBars.Both;
            this.Height = "150px";
            this.DataKeys = "$P.guid";
            this.ReadOnly = false;
            this.EmptyDataText = "没有相关记录!";

            writer.AddStyleAttribute("width", "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                {
                    writer.AddAttribute("valign", "top");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    {

                        base.Render(writer);

                    }
                    writer.RenderEndTag();
                }
                {
                    writer.AddStyleAttribute("width", "100px");
                    writer.AddAttribute("valign", "top");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    {
                        writer.WriteLine("<button type='button' style='width:80px;' onclick='{0}.newRow()'>添加行</button><br />", this.GetClientID());
                        writer.WriteLine("<button type='button' style='width:80px;' onclick='{0}.removeFocusRow()'>删除行</button><br />", this.GetClientID());

                        writer.WriteLine("<button type='button' style='width:80px;' onclick=''>上移</button><br />");
                        writer.WriteLine("<button type='button' style='width:80px;' onclick=''>下移</button>");
                    }
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();


            //base.Render(writer);

        }


    }
}
