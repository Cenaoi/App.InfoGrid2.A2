using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using System.ComponentModel;
using EasyClick.Web.ReportForms.Data;

using CrossReportDef = EasyClick.Web.ReportForms.CrossReport;
using System.Collections;
using System.IO;
using EC5.Utility;

namespace EasyClick.Web.ReportForms.UI
{
    /// <summary>
    /// 交叉报表
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class CrossReport : Control, IAttributeAccessor
    {
        bool m_ShowColTotal = true;
        bool m_ShowRowTotal = true;

        RFieldTagCollection m_RowTags = new RFieldTagCollection();

        RFieldTagCollection m_ColTags = new RFieldTagCollection();

        RFieldTagCollection m_DataTags = new RFieldTagCollection();


        IList m_DataList = null;
        
        /// <summary>
        /// 显示列的统计
        /// </summary>
        [Description("显示列的统计")]
        [DefaultValue(true)]
        public bool ShowColTotal
        {
            get { return m_ShowColTotal; }
            set { m_ShowColTotal = value; }
        }

        /// <summary>
        /// 显示行的统计
        /// </summary>
        [Description("显示行的统计")]
        [DefaultValue(true)]
        public bool ShowRowTotal
        {
            get { return m_ShowRowTotal; }
            set { m_ShowRowTotal = value; }
        }

        /// <summary>
        /// 数据条目集合
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public RFieldTagCollection RowTags
        {
            get { return m_RowTags; }
        }

        /// <summary>
        /// 数据条目集合
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public RFieldTagCollection ColTags
        {
            get { return m_ColTags; }
        }

        /// <summary>
        /// 数据条目集合
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public RFieldTagCollection DataTags
        {
            get { return m_DataTags; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        [Browsable(false)]
        public IList DataSource
        {
            get { return m_DataList; }
            set { m_DataList = value; }
        }

        /// <summary>
        /// 报表标题
        /// </summary>
        string m_ReportTitle = "报表标题";

        /// <summary>
        /// 报表标题
        /// </summary>
        [Description("报表标题")]
        public string ReportTitle
        {
            get { return m_ReportTitle; }
            set { m_ReportTitle = value; }
        }

        /// <summary>
        /// 保存到 Excel 文件
        /// </summary>
        /// <param name="path"></param>
        public void SaveToExcel(string path)
        {
            string excel = GetExcel();

            File.WriteAllText(path, excel);
        }


        /// <summary>
        /// 获取 Excel 数据
        /// </summary>
        /// <returns></returns>
        public string GetExcel()
        {
            StringBuilder sb = new StringBuilder();


            #region Head

            sb.AppendLine("<html xmlns:x='urn:schemas-microsoft-com:office:excel'>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <!--[if gte mso 9]><xml>");
            sb.AppendLine("  <x:ExcelWorkbook>");
            sb.AppendLine("    <x:ExcelWorksheets>");
            sb.AppendLine("      <x:ExcelWorksheet>");
            sb.AppendLine("        <x:Name>" + m_ReportTitle + "</x:Name>");
            sb.AppendLine("        <x:WorksheetOptions>");
            sb.AppendLine("          <x:Print>");
            sb.AppendLine("            <x:ValidPrinterInfo />");
            sb.AppendLine("          </x:Print>");
            sb.AppendLine("        </x:WorksheetOptions>");
            sb.AppendLine("      </x:ExcelWorksheet>");
            sb.AppendLine("    </x:ExcelWorksheets>");
            sb.AppendLine("  </x:ExcelWorkbook>");
            sb.AppendLine("  </xml>");
            sb.AppendLine("  <![endif]-->");
            sb.AppendLine("</head>");

            #endregion

            sb.AppendLine("<body>");

            string table = Render();

            sb.Append(table);

            sb.AppendLine("</body>");

            sb.Append("</html>");

            return sb.ToString();
        }         

        /// <summary>
        /// 显示
        /// </summary>
        public string Render()
        {

            CrossReportDef cr = new CrossReportDef();

            cr.EnabledColTotal = m_ShowColTotal;
            cr.EnabledRowTotal = m_ShowRowTotal;

            for (int i = 0; i < this.ColTags.Count; i++)
            {
                ReportFieldGroup fieldGroup = this.ColTags[i];
                
                foreach (ReportField field in fieldGroup.Items)
                {
                    ReportItem rItem = ToReportItem(field);

                    cr.ColGroupTags.Add(rItem);
                }
            }


            for (int i = 0; i < this.RowTags.Count; i++)
            {
                ReportFieldGroup fieldGroup = this.RowTags[i];
                
                foreach (ReportField field in fieldGroup.Items)
                {
                    ReportItem rItem = ToReportItem(field);

                    cr.RowGroupTags.Add(rItem);
                }
            }


            for (int i = 0; i < this.DataTags.Count; i++)
            {
                ReportFieldGroup fieldGroup = this.DataTags[i];

                foreach (ReportField field in fieldGroup.Items)
                {
                    ReportItem rItem = ToReportItem(field);

                    cr.DataGroupTags.Add(rItem);
                }
            }


            cr.SetDataSource(m_DataList);

            RTable rTable = cr.ToRTable();

            string html = ToHtml(rTable);


            if (!MiniScriptManager.ClientScript.ReadOnly)
            {
                string txt =  html.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n").Replace("\t", @"\t");

                MiniScript.Add("$(\"#{0}\").html(\"{1}\")", this.ClientID, txt);
            }

            return html;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "200px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("交叉报表");
                writer.RenderEndTag();
                return;
            }

            bool scriptReadOnly = MiniScriptManager.ClientScript.ReadOnly;

            MiniScriptManager.ClientScript.ReadOnly = true;

            string html = Render();

            MiniScriptManager.ClientScript.ReadOnly = scriptReadOnly;

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

            writer.RenderBeginTag( HtmlTextWriterTag.Div);

            writer.Write(html);

            writer.RenderEndTag();
        }

        private ReportItem ToReportItem(ReportField field)
        {
            ReportItem item = new ReportItem();
            item.CellClassName = field.CellClassName;
            item.Code = field.Code;
            item.DBField = field.DBField;
            item.DBValue = field.DBValue;
            item.EnabledTotal = field.EnabledTotal;
            item.Format = field.Format;

            item.OrderType = field.OrderType;

            item.FormatType = field.FormatType;

            item.FunName = field.FunName;
            item.HeadClassName = field.HeadClassName;
            item.Style = field.Style;
            item.Title = field.Title;

            item.ValueMode = field.ValueMode;

            item.Width = field.Width;

            foreach (RFieldValue fv in field.FixedValues)
            {
                item.FixedValues.Add(fv.Value, fv.Text, fv.Operator); 
            }


            return item;
        }


        private void ToHtml_Head(HtmlTextWriter writer, RTable table)
        {

            writer.RenderBeginTag(HtmlTextWriterTag.Thead);

            foreach (RRow row in table.Head)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                foreach (RCell cell in row)
                {
                    if (cell.IsMergeChild)
                    {
                        continue;

                        //writer.AddStyleAttribute("display", "none");
                    }

                    if (cell.ColSpan > 1)
                    {
                        writer.AddAttribute("colspan", cell.ColSpan.ToString());
                    }

                    if (cell.RowSpan > 1)
                    {
                        writer.AddAttribute("rowspan", cell.RowSpan.ToString());
                    }



                    writer.RenderBeginTag(HtmlTextWriterTag.Th);

                    if (cell.Value == null || cell.Value.ToString() == "0")
                    {
                        writer.Write("&nbsp;");
                    }
                    else
                    {
                        writer.Write(cell.Value);
                    }

                    writer.RenderEndTag();
                }

                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }


        private void ToHtml_Body(HtmlTextWriter writer, RTable table)
        {

            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

            int n = 0;

            foreach (RRow row in table.Body)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                n = -1;

                foreach (RCell cell in row)
                {
                    n++;

                    if (cell.IsMergeChild)
                    {
                        //writer.AddStyleAttribute("display", "none");
                        continue;
                    }

                    writer.AddStyleAttribute("width", "40px");

                    if (cell.ColSpan > 1)
                    {
                        writer.AddAttribute("colspan", cell.ColSpan.ToString());
                    }

                    if (cell.RowSpan > 1)
                    {
                        writer.AddAttribute("rowspan", cell.RowSpan.ToString());
                    }


                    if (n < m_RowTags.Count)
                    {
                        writer.AddStyleAttribute("font-weight", "bold");
                        writer.AddStyleAttribute("text-align", "center");
                    }
                    else
                    {
                        writer.AddStyleAttribute("text-align", "right");
                    }
                    

                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    if (cell.Value == null || cell.Value.ToString() == "0")
                    {
                        writer.Write("&nbsp;");
                    }
                    else
                    {
                        writer.Write(cell.Value);
                    }

                    writer.RenderEndTag();

                }

                writer.RenderEndTag();

            }

            writer.RenderEndTag();
        }


        private string ToHtml(RTable table)
        {
            

            //StringBuilder sb = new StringBuilder();

            StringWriter sw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(sw);


            writer.AddAttribute("border", "1");
            writer.AddAttribute("cellspacing", "0");
            writer.AddAttribute("cellpadding", "4");
            
            writer.AddAttribute("class", "table1");


            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            ToHtml_Head(writer, table);
            ToHtml_Body(writer, table);


            writer.RenderEndTag();

            return sw.ToString();
        }

        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion

    }


}
