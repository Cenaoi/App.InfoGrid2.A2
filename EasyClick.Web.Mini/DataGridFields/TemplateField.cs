using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Security.Permissions;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 模板字段
    /// </summary>
    [Description("模板字段")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TemplateField : BoundField
    {
        /// <summary>
        /// 数据记录模板
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public string ItemTemplate { get; set; }

        /// <summary>
        /// 插入记录模板
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public string InsertItemTemplate { get; set; }

        /// <summary>
        /// 编辑记录模板
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public string EditItemTemplate { get; set; }

        /// <summary>
        /// 标题模板
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        public string HeaderTemplate { get; set; }


        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (cellType == MiniDataControlCellType.Header)
            {
                TextWriter tw = new StringWriter();

                HtmlTextWriter writer = new HtmlTextWriter(tw);
                if (this.Width > 0)
                {
                    writer.AddAttribute("width", this.Width + "px");
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Th);



                if (!string.IsNullOrEmpty(this.HeaderTemplate))
                {
                    //return string.Format("<th>{0}</th>", this.HeaderTemplate);

                    writer.Write(this.HeaderTemplate);
                }
                else
                {
                    writer.Write(this.HeaderText);

                }
                
                writer.RenderEndTag();

                string txt = tw.ToString();

                writer.Dispose();
                tw.Dispose();

                return txt;

                //return string.Format("<th>{0}</th>", this.HeaderText);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<td ");
                if (this.ItemAlign != CellAlign.Left)
                {
                    sb.AppendFormat("align=\"{0}\" ", this.ItemAlign.ToString().ToLower());
                }
                sb.Append(">");

                sb.Append(this.ItemTemplate);


                sb.Append("</td>");

                return sb.ToString();
            }
        }
    }
}
