using System;
using System.Text;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace EasyClick.BizWeb.UI
{
    /// <summary>
    /// Excel 文件辅助类
    /// </summary>
    /// <typeparam name="ModelT"></typeparam>
    public class ExcelAction<ModelT> where ModelT : class
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Excel 文件辅助类
        /// </summary>
        public ExcelAction()
        {
        }

        /// <summary>
        /// Excel 文件辅助类
        /// </summary>
        /// <param name="grid"></param>
        public ExcelAction(EasyClick.Web.Mini.DataGrid grid)
        {
            m_DataGridView = grid;
        }

        /// <summary>
        /// Excel 文件辅助类
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="models"></param>
        public ExcelAction(DataGrid grid, LModelList<ModelT> models)
        {
            m_DataGridView = grid;
            m_Models = models;
        }

        /// <summary>
        /// Excel 文件辅助类
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="models"></param>
        public ExcelAction(DataGridView grid, ModelT[] models)
        {
            m_DataGridView = grid;

            m_Models = new LModelList<ModelT>();
            m_Models.AddRange(models);
        }

        DataGrid m_DataGridView;

        LModelList<ModelT> m_Models;

        string m_WorksheetName = "新建文档";

        public string WoksheetName
        {
            get { return m_WorksheetName; }
            set { m_WorksheetName = value; }
        }

        public DataGrid DataGridView
        {
            get { return m_DataGridView; }
            set { m_DataGridView = value; }
        }


        public LModelList<ModelT> Models
        {
            get { return m_Models; }
            set { m_Models = value; }
        }


        class ExcelColumn
        {
            public BoundField Field;

            public int Index;
        }

        private ExcelColumn[] GetExcelColumns()
        {
            List<ExcelColumn> cols = new List<ExcelColumn>();

            for (int i = 0; i < m_DataGridView.Columns.Count; i++)
            {
                EasyClick.Web.Mini.BoundField boundField = m_DataGridView.Columns[i];

                if (!boundField.ToExcel)
                {
                    continue;
                }

                if (!boundField.Visible)
                {
                    continue;
                }

                if (i == 1 && boundField is EasyClick.Web.Mini.CheckBoxField)
                {
                    if (((EasyClick.Web.Mini.CheckBoxField)boundField).HeaderMode == CheckBoxHeaderMode.SelectAll)
                    {
                        continue;
                    }
                }

                ExcelColumn col = new ExcelColumn();

                col.Field = boundField;
                col.Index = i;

                cols.Add(col);
            }

            return cols.ToArray();
        }

        /// <summary>
        /// 输出 Excel 的 Html 格式
        /// </summary>
        /// <returns></returns>
        public string ToHtml()
        {
            LModelList<ModelT> models = m_Models;

            StringBuilder sb = new StringBuilder();

            ExcelColumn[] excelCols = GetExcelColumns();

            #region Head

            sb.AppendLine("<html xmlns:x='urn:schemas-microsoft-com:office:excel'>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <!--[if gte mso 9]><xml>");
            sb.AppendLine("  <x:ExcelWorkbook>");
            sb.AppendLine("    <x:ExcelWorksheets>");
            sb.AppendLine("      <x:ExcelWorksheet>");
            sb.AppendLine("        <x:Name>" + m_WorksheetName + "</x:Name>");
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

            sb.AppendLine("<table border='1'>");

            //sb.AppendFormat("<tr><td style='font-size:20px;' colspan='6'>{0}</td></tr>", 0);

            #region 构造标题

            sb.Append("<tr>");

            foreach (ExcelColumn col in excelCols)
            {
                BoundField boundField = col.Field;

                if (string.IsNullOrEmpty(boundField.DataField))
                {
                    continue;
                }

                sb.AppendFormat("<th nowrap='nowrap'>{0}</th>\r\n", boundField.HeaderText);
            }

            sb.Append("</tr>");

            #endregion

            #region 构造数据内容


            foreach (ModelT m in models)
            {
                sb.Append("<tr>");

                foreach (ExcelColumn col in excelCols)
                {
                    BoundField boundField = col.Field;

                    if (string.IsNullOrEmpty(boundField.DataField))
                    {
                        continue;
                    }

                    if (boundField is EditorNumberCell || boundField is NumberBoxField)
                    {
                        sb.Append("<td nowrap=\"nowrap\">");
                    }
                    else
                    {
                        sb.Append("<td nowrap=\"nowrap\" style=\"vnd.ms-excel.numberformat:@\">");
                         
                    }

                    if (!string.IsNullOrEmpty(boundField.DataFormatString))
                    {
                        sb.AppendFormat(boundField.DataFormatString, LightModel.GetFieldValue(m, boundField.DataField));
                    }
                    else if (boundField.DataField.Contains("{$"))
                    {
                        //临时针对 {$T.字段名} 的解决方法

                        string lightForamt = boundField.DataField.Replace("$T.", "");

                        string lightText = LightModel.Format(lightForamt, m);

                        sb.Append(lightText);
                    }
                    else
                    {
                        sb.Append(LightModel.GetFieldValue(m, boundField.DataField));
                    }

                    sb.Append("</td>");
                }
                
                sb.Append("</tr>");

            }

            #endregion


            sb.AppendLine("</table>");
            sb.AppendLine("</body>");

            sb.Append("</html>");

            return sb.ToString();
        }


        public void Save(string filename)
        {
            RenderDataTableToExcel(m_Models, filename);
        }

        private void RenderDataTableToExcel(LModelList<ModelT> list, string fileName)
        {
            MemoryStream ms = RenderDataTableToExcel(list) as MemoryStream;

            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            ms.Close();


            data = null;
            ms = null;
            fs = null;
        }

        private Stream RenderDataTableToExcel(LModelList<ModelT> list)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);

            //ICellStyle dateStyle = workbook.CreateCellStyle();
            //IDataFormat format = workbook.CreateDataFormat();
            //dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            // handling header.
            ExcelColumn[] excelCols = GetExcelColumns();
            int colIndex = 0;

            foreach (ExcelColumn col in excelCols)
            {
                BoundField boundField = col.Field;

                if (string.IsNullOrEmpty(boundField.DataField))
                {
                    continue;
                }

                ICell cell = headerRow.CreateCell(colIndex);
                
                cell.SetCellValue(boundField.HeaderText);

                colIndex++;
            }
            
            // handling value.
            int rowIndex = 1;
            
            foreach (ModelT m in list)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                colIndex = 0;

                foreach (ExcelColumn col in excelCols)
                {
                    BoundField boundField = col.Field;

                    
                    if (string.IsNullOrEmpty(boundField.DataField))
                    {
                        continue;
                    }

                    object valueObj = LightModel.GetFieldValue(m, boundField.DataField);

                    string valueStr=null;

                    if (!string.IsNullOrEmpty(boundField.DataFormatString))
                    {
                        valueStr=String.Format(boundField.DataFormatString, valueObj);
                    }
                    else if (boundField.DataField.Contains("{$"))
                    {
                        //临时针对 {$T.字段名} 的解决方法

                        string lightForamt = boundField.DataField.Replace("$T.", "");

                        valueStr = LightModel.Format(lightForamt, m);
                    }
                    else
                    {
                        valueStr = valueObj.ToString();
                    }

                    ICell cell = dataRow.CreateCell(colIndex);

                    cell.SetCellValue(valueStr);
                    
                    colIndex++;
                }
                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }
    }
}
