using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EasyClick.Web.ReportForms.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace EasyClick.Web.ReportForms.Export
{
    /// <summary>
    /// 导出 Excel 文件
    /// </summary>
    public class ExcelExport
    {
        /// <summary>
        /// 保存到 Excel 文件
        /// </summary>
        /// <param name="fileName">保存文件路径</param>
        /// <param name="cr">报表文件定义</param>
        public void Save(string fileName,CrossReport cr)
        {
            RenderDataTableToExcel(fileName, cr);
        }

        private void RenderDataTableToExcel(string fileName, CrossReport cr)
        {
            MemoryStream ms = RenderDataTableToExcel(cr) as MemoryStream;
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }

        const string STYLE_TITLE = "TITLE"; //大标题
        const string STYLE_DATA = "DATA";   //数据区
        const string STYLE_DATA_TOTAL = "DATA_TOTAL";
        const string STYLE_DATA_TOTAL_ALL = "DATA_TOTAL_ALL";

        const string STYLE_ROW_HEAD = "ROW_HEAD";
        const string STYLE_COLUMN_HEAD = "COLUMN_HEAD";


        #region 样式

        /// <summary>
        /// 单元格样式
        /// </summary>
        SortedList<string, HSSFCellStyle> m_Styles;

        private HSSFCellStyle GetStyle(HSSFWorkbook workbook, string styleName)
        {

            HSSFCellStyle ics = null;

            if(m_Styles == null)
            {
                m_Styles = new SortedList<string, HSSFCellStyle>();

                HSSFCellStyle title = CreateStyle_Title(workbook);
                HSSFCellStyle columnHead = CreateStyle_ColumnHead(workbook);
                HSSFCellStyle rowHead = CreateStyle_RowHead(workbook);

                HSSFCellStyle data = CreateStyle_Data(workbook);
                HSSFCellStyle dataTotal = CreateStyle_DataTotal(workbook);
                HSSFCellStyle dataTotalAll = CreateStyle_DataTotalAll(workbook);

                m_Styles.Add(STYLE_TITLE, title);
                m_Styles.Add(STYLE_COLUMN_HEAD, columnHead);
                m_Styles.Add(STYLE_ROW_HEAD, rowHead);
                m_Styles.Add(STYLE_DATA, data);
                m_Styles.Add(STYLE_DATA_TOTAL, dataTotal);
                m_Styles.Add(STYLE_DATA_TOTAL_ALL, dataTotalAll);
            }

            styleName = styleName.ToUpper();

            if (m_Styles.TryGetValue(styleName, out ics))
            {
                return ics;
            }

            return ics;
            
        }

        /// <summary>
        /// 数据区域样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private HSSFCellStyle CreateStyle_Title(HSSFWorkbook workbook)
        {
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();

            style.VerticalAlignment = VerticalAlignment.Center;

            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 16;
            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }

        /// <summary>
        /// 数据区域样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private HSSFCellStyle CreateStyle_Data(HSSFWorkbook workbook)
        {
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;

            return style;
        }

        /// <summary>
        /// 数据区小计样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private HSSFCellStyle CreateStyle_DataTotal(HSSFWorkbook workbook)
        {
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;



            HSSFFont font = (HSSFFont)workbook.CreateFont();
            //font.FontHeightInPoints = 12;

            font.Boldweight = (short)FontBoldWeight.Bold;

            style.SetFont(font);

            return style;
        }

        /// <summary>
        /// 数据区总计样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private HSSFCellStyle CreateStyle_DataTotalAll(HSSFWorkbook workbook)
        {
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;

            return style;
        }



        /// <summary>
        /// 行标题样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private HSSFCellStyle CreateStyle_RowHead(HSSFWorkbook workbook)
        {
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = false;

            return style;
        }

        /// <summary>
        /// 列标题的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private HSSFCellStyle CreateStyle_ColumnHead(HSSFWorkbook workbook)
        {
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();

            
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            

            HSSFFont font = (HSSFFont)workbook.CreateFont();
            //font.FontHeightInPoints = 12;

            font.Boldweight = (short)FontBoldWeight.Bold;
            
            style.SetFont(font);

            return style;
        }

        #endregion




        private Stream RenderDataTableToExcel(CrossReport cr)
        {
            int rowOffset = 0;  //行位移
            int colIndex = 0;
            int colSpan = 0;
            int rowSpan = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            HSSFRow headerRow;
            
            RTable table = cr.ToRTable();

            HSSFCellStyle columnHeadStyle = GetStyle(workbook, STYLE_COLUMN_HEAD);


            //创建标题
            HSSFRow titleEX = (HSSFRow)sheet.CreateRow(rowOffset++);
            titleEX.Height = 1000;
            ICell titleCellEx = titleEX.CreateCell(0);
            titleCellEx.SetCellValue(cr.Title);
            titleCellEx.CellStyle = GetStyle(workbook, STYLE_TITLE);


            // handling header.
            foreach (RRow row in table.Head)
            {
                headerRow = (HSSFRow)sheet.CreateRow(rowOffset++);
                colIndex = 0;

                headerRow.Height = 500;

                foreach (RCell cell in row)
                {
                    if (cell.IsMergeChild)
                    {
                        colIndex++;
                        continue;
                    }

                    ICell icell = headerRow.CreateCell(colIndex++);
                    icell.CellStyle = columnHeadStyle;

                    if (cell.Value != null )
                    {
                        icell.SetCellValue(cell.Value.ToString());
                    }

                    if (cell.ColSpan > 1 || cell.RowSpan > 1)
                    {
                        colSpan = cell.ColSpan;
                        rowSpan = cell.RowSpan;
                        MergedRegion(sheet, rowOffset - 1, rowOffset + rowSpan - 2, colIndex - 1, colIndex + colSpan - 2);
                    }
                    

                }
            }

            int columnCount = 0;

            // handling value.
            foreach (RRow row in table.Body)
            {
                columnCount = Math.Max(columnCount, colIndex);

                colIndex = 0;
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowOffset++);

                dataRow.Height = 400;

                foreach (RCell cell in row)
                {
                    if (cell.IsMergeChild)
                    {
                        colIndex++;
                        continue;
                    }

                    HSSFCell icell = (HSSFCell)dataRow.CreateCell(colIndex++);


                    if (cell.ColSpan > 1 || cell.RowSpan > 1)
                    {
                        colSpan = cell.ColSpan;
                        rowSpan = cell.RowSpan;
                        MergedRegion(sheet, rowOffset - 1, rowOffset + rowSpan - 2, colIndex - 1, colIndex + colSpan - 2);
                    }
                    
                    if(cell.Value != null)
                    {
                        if (IsNumber(cell.Value))
                        {
                            double valueDob = Convert.ToDouble(cell.Value);

                            if (valueDob != 0)
                            {
                                icell.SetCellValue(valueDob);
                            }
                        }
                        else
                        {
                            icell.SetCellValue(cell.Value.ToString());
                        }
                    }

                    if (colIndex <= table.RowHeaderCount)
                    {
                        icell.CellStyle = GetStyle(workbook, STYLE_ROW_HEAD);
                    }
                    else
                    {
                        if(cell.TreeNode != null && cell.TreeNode.NodeType == CrossHeadTreeNodeTypes.Total)
                        {
                            icell.CellStyle = GetStyle(workbook, STYLE_DATA_TOTAL);
                        }
                        else
                        {
                            icell.CellStyle = GetStyle(workbook, STYLE_DATA);
                        }
                    }

                }
            }

            for (int i = 0; i < table.RowHeaderCount; i++)
            {
                sheet.SetColumnWidth(i, 7000);
            }

            for (int i = table.RowHeaderCount; i < columnCount; i++)
            {
                sheet.SetColumnWidth(i, 3000);
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }





        private bool IsNumber(object value)
        {
            Type vType = value.GetType();

            bool isNum = false;

            if (vType == typeof(decimal) ||
                vType == typeof(Int16) ||
                vType == typeof(Int32) ||
                vType == typeof(Int64) ||

                vType == typeof(UInt16) ||
                vType == typeof(UInt32) ||
                vType == typeof(UInt64) ||

                vType == typeof(Double) ||
                vType == typeof(Single) ||
                vType == typeof(byte))

            {
                isNum = true;
            }

            return isNum;
        }

        /// <summary>
        /// 合并区域
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowstart"></param>
        /// <param name="rowend"></param>
        /// <param name="colstart"></param>
        /// <param name="colend"></param>
        private void MergedRegion(HSSFSheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }

        

    }
}
