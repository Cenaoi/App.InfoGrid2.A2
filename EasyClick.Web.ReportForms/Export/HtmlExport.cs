using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.ReportForms.Data;
using System.IO;
using System.Web.UI;
using EasyClick.Web.ReportForms.UI;
using CrossReportDef = EasyClick.Web.ReportForms.CrossReport;

namespace EasyClick.Web.ReportForms.Export
{
    public class HtmlExport
    {

        public void Save(string path, string reportTitle, CrossReportDef cr, RFieldTagCollection rowTags)
        {
            string excel = GetExcel(reportTitle, cr, rowTags);

            File.WriteAllText(path, excel);
        }

        /// <summary>
        /// 获取 Excel 数据
        /// </summary>
        /// <returns></returns>
        private string GetExcel(string reportTitle, CrossReportDef cr, RFieldTagCollection rowTags)
        {
            StringBuilder sb = new StringBuilder();

            #region Head

            sb.AppendLine("<html xmlns:x='urn:schemas-microsoft-com:office:excel'>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <!--[if gte mso 9]><xml>");
            sb.AppendLine("  <x:ExcelWorkbook>");
            sb.AppendLine("    <x:ExcelWorksheets>");
            sb.AppendLine("      <x:ExcelWorksheet>");
            sb.AppendLine("        <x:Name>" + reportTitle + "</x:Name>");
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

            string table = Render(cr, rowTags);

            sb.Append(table);

            sb.AppendLine("</body>");

            sb.Append("</html>");

            return sb.ToString();
        }

        /// <summary>
        /// 显示
        /// </summary>
        private string Render(CrossReportDef cr,RFieldTagCollection rowTags)
        {
            RTable rTable = cr.ToRTable();

            string html = ToHtml(rTable, rowTags);

            return html;
        }

        private string ToHtml(RTable table, RFieldTagCollection rowTags)
        {
            StringWriter sw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(sw);

            writer.AddAttribute("border", "1");
            writer.AddAttribute("cellspacing", "0");
            writer.AddAttribute("cellpadding", "4");

            writer.AddAttribute("class", "table1");

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            ToHtml_Head(writer, table);
            ToHtml_Body(writer, table, rowTags);

            writer.RenderEndTag();

            return sw.ToString();
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

        private void ToHtml_Body(HtmlTextWriter writer, RTable table, RFieldTagCollection rowTags)
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

                    //writer.AddStyleAttribute("width", "40px");

                    if (cell.ColSpan > 1)
                    {
                        writer.AddAttribute("colspan", cell.ColSpan.ToString());
                    }

                    if (cell.RowSpan > 1)
                    {
                        writer.AddAttribute("rowspan", cell.RowSpan.ToString());
                    }


                    if (n < rowTags.Count)
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
    }
}
