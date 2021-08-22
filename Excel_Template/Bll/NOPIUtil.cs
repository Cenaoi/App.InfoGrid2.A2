using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace App.InfoGrid2.Excel_Template.Bll
{
    /// <summary>
    /// Excel助手
    /// </summary>
    public class NOPIUtil
    {

        /// <summary>
        /// 这是系统字体缓冲
        /// </summary>
        SortedDictionary<string, Font> m_font = new SortedDictionary<string, Font>();

        /// <summary>
        /// 字体缓冲
        /// </summary>
        SortedDictionary<string, HSSFFont> m_FontBuffer = new SortedDictionary<string, HSSFFont>();

        /// <summary>
        /// 单元格样式缓冲
        /// </summary>
        SortedDictionary<string, ICellStyle> m_StyleBuffer = new SortedDictionary<string, ICellStyle>();

        /// <summary>
        /// 绘制工具
        /// </summary>
        public Graphics m_gh;

        Bitmap m_TmpBmp;

        /// <summary>
        /// 行高
        /// </summary>
        short m_RowHegiht = 500;

        /// <summary>
        /// 列宽
        /// </summary>
        int m_CellWidth = 5000;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 拿到单元格的值
        /// </summary>
        /// <returns></returns>
        public string GetCellValue(ICell icell)
        {
            if (icell.CellType == CellType.Blank)
            {
                return "";
            }

            try
            {
                if (icell.CellType == CellType.Numeric)
                {
                    ICellStyle style = icell.CellStyle;

                    int i = style.DataFormat;

                    string fs = style.GetDataFormatString();

                    //把日期格式中的中文去掉，就能判断是否是日期了
                    fs = Regex.Replace(fs, "\"|\'|年|月|日|时|分|秒|毫秒|微秒", string.Empty);

                    if (NPOI.SS.UserModel.DateUtil.IsADateFormat(i, fs))
                    {
                        DateTime dt = icell.DateCellValue;
                        return dt.ToString();
                    }

                    return icell.NumericCellValue.ToString()?.Trim();

                }
            }
            catch (Exception ex)
            {
                log.Error("拿单元格的日期类型出错了！", ex);
            }

            return icell.ToString()?.Trim();
        }
        /// <summary>
        /// 拿到单元格的类型
        /// </summary>
        /// <param name="icell">单元格对象</param>
        /// <returns>如果是日期类型就返回 Date ,如果是数字类型就返回 Double,否则就返回 String。</returns>
        public V1.CellTypeName GetCellType(ICell icell)
        {


            if (icell.CellType == CellType.Blank)
            {
                return V1.CellTypeName.Blank;
            }

            try
            {
                if (icell.CellType == CellType.Numeric)
                {

                    ICellStyle style = icell.CellStyle;

                    int i = style.DataFormat;

                    string fs = style.GetDataFormatString();

                    //把日期格式中的中文去掉，就能判断是否是日期了
                    fs = Regex.Replace(fs, "\"|\'|年|月|日|时|分|秒|毫秒|微秒", string.Empty);

                    if (NPOI.SS.UserModel.DateUtil.IsADateFormat(i, fs))
                    {

                        return V1.CellTypeName.Date;
                    }

                    return V1.CellTypeName.Double;
                }
            }
            catch (Exception ex)
            {
                log.Error("拿单元格的日期类型出错了！", ex);
            }

            return V1.CellTypeName.String;
        }

        /// <summary>
        /// 拿到数据格式的名称，如果单元格的值为空就返回""
        /// </summary>
        /// <param name="icell">单元格对象</param>
        /// <returns>数据格式名称，如果单元格的值为空就返回""</returns>
        public string GetDataFormatString(ICell icell)
        {
            if (icell.CellType == CellType.Blank)
            {
                return "";
            }

            return icell.CellStyle.GetDataFormatString();

        }


        /// <summary>
        /// 设置 sheet 第一行 第一列值
        /// </summary>
        /// <param name="ics">单元格样式</param>
        /// <param name="isheet">工作表1</param>
        /// <param name="ifont">字体设置</param>
        /// <param name="cell_value">单元格值 默认值 公司名称</param>
        void SetExcelHead(ICellStyle ics, ISheet isheet, IFont ifont, string cell_value = "公司名称")
        {

            //左右居中
            ics.Alignment = HorizontalAlignment.CenterSelection;
            //垂直居中
            ics.VerticalAlignment = VerticalAlignment.Center;
            //字体大小
            ifont.FontHeightInPoints = 25;
            //字体名称
            ifont.FontName = "宋体";

            ics.SetFont(ifont);


            //第一行
            IRow ir = isheet.CreateRow(0);
            //行高
            ir.Height = m_RowHegiht;

            //第一行第一个单元格
            ICell ic = ir.CreateCell(0);
            ic.SetCellValue(cell_value);
            ic.CellStyle = ics;


        }


        /// <summary>
        /// 设置列的默认宽度 根据传进来的长度
        /// </summary>
        /// <param name="sheet">sheet对象</param>
        /// <param name="num">列数量</param>
        public void SetCellWidth(ISheet sheet, int num)
        {

            for (int i = 0; i < num; i++)
            {

                sheet.SetColumnWidth(i, m_CellWidth);

            }
        }


        /// <summary>
        /// 拿到列的数量
        /// </summary>
        /// <param name="sheet">sheet对象</param>
        /// <returns></returns>
        public int GetCellNum(ISheet sheet)
        {
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            //最大列数
            int column_num = 0;


            for (int i = 0; i < rowCount; i++)
            {

                ///拿到每一行的对象
                IRow row = sheet.GetRow(i);
                ///判断行是否有数据
                if (row == null)
                {
                    continue;
                }

                column_num = Math.Max(column_num, row.LastCellNum);
            }

            return column_num;
        }


        /// <summary>
        /// 获取每一列的宽度
        /// </summary>
        /// <param name="ts">模板的第一页参数对象</param>
        /// <param name="sheet">模板第一页对象</param>
        public void GetColumnWidth(TempSheet ts, HSSFSheet sheet)
        {
            NOPIHandler nopi = new NOPIHandler();

            ts.ColumnNum = GetCellNum(sheet);

            for (int i = 0; i < ts.ColumnNum; i++)
            {
                var c_width = sheet.GetColumnWidth(i);

                if (c_width == 2048)
                {
                    //这个是特殊处理的   因为这个拿的值好像不对，不是默认值 8.43 字符宽度      下面的值是这样算的，所有列 （8.43 * 7 + 5）/ 7 * 256 
                    c_width = 2340;
                }

                ts.ColumnWidth.Add(i, c_width);

                int c_width_px = Convert.ToInt32(Math.Round(sheet.GetColumnWidthInPixels(i)));

                ts.ColumnWidthPx.Add(i, c_width_px);

             }


        }


        /// <summary>
        /// 拿到单元格的样式
        /// </summary>
        /// <param name="cp">自定义单元格对象</param>
        /// <param name="icell">Excel单元格对象</param>
        /// <param name="workbook">整个Excel对象</param>
        public void GetCellStyle(V1.CellPro cp, ICell icell, IWorkbook workbook)
        {
            try
            {
                ICellStyle ics = icell.CellStyle;

                //下边框
                cp.BorderBottomName = ics.BorderBottom;
                cp.BorderBottomColor = ics.BottomBorderColor;
                //上边框
                cp.BorderTopName = ics.BorderTop;
                cp.BorderTopColor = ics.TopBorderColor;
                //左边框
                cp.BorderLeftName = ics.BorderLeft;
                cp.BorderLeftColor = ics.LeftBorderColor;
                //右边框
                cp.BorderRightName = ics.BorderRight;
                cp.BorderRightColor = ics.RightBorderColor;
                //左右对齐方式
                cp.Alignment = ics.Alignment;
                //垂直对齐方式
                cp.VerticalAlignment = ics.VerticalAlignment;

                //填充背景色
                cp.FillBackgroundColor = ics.FillBackgroundColor;
                //填充前景色
                cp.FillForegroundColor = ics.FillForegroundColor;

                //填充模式
                cp.FillPattern = ics.FillPattern;

                //是否缩放
                cp.ShrinkToFit = ics.ShrinkToFit;

                //是否换行
                cp.WrapText = ics.WrapText;


                IFont ifont = ics.GetFont(workbook);
                //字体颜色
                cp.FontColor = ifont.Color;
                //字体名字
                cp.FontName = ifont.FontName;
                //字体大小
                cp.FontSize = ifont.FontHeightInPoints;
                //字体粗细
                cp.FontBlod = ifont.Boldweight;
                //斜体
                cp.IsItalic = ifont.IsItalic;

            }
            catch (Exception ex)
            {
                throw new Exception("拿单元格的属性发生错误！", ex);
            }

        }



        /// <summary>
        /// 设置打印属性的
        /// </summary>
        /// <param name="sheet">工作薄</param>
        /// <param name="ts">模板sheet参数对象</param>
        public void SetPrintPor(ISheet sheet, TempSheet ts)
        {
            IPrintSetup ips = sheet.PrintSetup;
            ips.FitHeight = ts.pp.FitHeight;
            ips.FitWidth = ts.pp.FitWidth;
            ips.Draft = ts.pp.IsDraft;
            ips.Landscape = ts.pp.Landscape;
            ips.LeftToRight = ts.pp.LeftToRight;
            ips.NoColor = ts.pp.NoColors;
            ips.PageStart = ts.pp.PageStart;
            ips.PaperSize = ts.pp.PaperSize;
            ips.Scale = ts.pp.Scale;
            ips.UsePage = ts.pp.UsePage;

        }

        /// <summary>
        /// 设置页面边距
        /// </summary>
        /// <param name="sheet">工作薄</param>
        /// <param name="ts">模板sheet参数对象</param>
        public void SetSheetMargin(ISheet sheet, TempSheet ts)
        {
            sheet.SetMargin(MarginType.BottomMargin, ts.SheetMargin.BottomMargin);
            //sheet.SetMargin(MarginType.FooterMargin, sp.sheetMargin.FooterMargin);
            //sheet.SetMargin(MarginType.HeaderMargin, sp.sheetMargin.HeaderMargin);
            sheet.SetMargin(MarginType.LeftMargin, ts.SheetMargin.LeftMargin);
            sheet.SetMargin(MarginType.RightMargin, ts.SheetMargin.RightMargin);
            sheet.SetMargin(MarginType.TopMargin, ts.SheetMargin.TopMargin);
        }

        /// <summary>
        /// 设置图片到Excel中去
        /// </summary>
        /// <param name="workbook">Excel对象</param>
        /// <param name="sheet">工作簿</param>
        /// <param name="ts">模板sheet参数对象</param>
        public void SetImg(IWorkbook workbook, ISheet sheet, TempSheet ts)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

            for (int i = 0; i < ts.Pictures.Count; i++)
            {

                HSSFPictureData hpd = ts.Pictures[i];
                HSSFClientAnchor hca = ts.Hcas[i];
                int pictureIdx = workbook.AddPicture(hpd.Data, PictureType.JPEG);
 
                HSSFClientAnchor anchor = new HSSFClientAnchor(hca.Dx1, hca.Dy1, hca.Dx2, hca.Dy2, hca.Col1, hca.Row1, hca.Col2, hca.Row2);
                //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50
                HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                //这是原图显示
                // pict.Resize();

            }


            foreach(var item in ts.DefPictures)
            {
                int pictureIdx = workbook.AddPicture(item.Bytes, PictureType.JPEG);
                HSSFClientAnchor anchor = new HSSFClientAnchor(item.Dx1, item.Dy1, item.Dx2, item.Dy2, item.Col1, item.Row1, item.Col2, item.Row2);
                //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50
                HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);


            }



        }


        /// <summary>
        /// 根据模板的第二页参数对象来创建第二页数据
        /// </summary>
        /// <param name="workbook">整个Excel对象</param>
        /// <param name="tp">第二个sheet参数对象</param>
        public void CreateSheet_2(IWorkbook workbook, TempParam tp)
        {
            //这是设置第二张工作表的数据的
            ISheet isheet1 = workbook.CreateSheet("sheet2");

            IRow ir = isheet1.CreateRow(0);
            ICell ic = ir.CreateCell(1);

            ic.SetCellValue("子表开始位置");
            ic = ir.CreateCell(0);
            //在外面显示要多加一行
            ic.SetCellValue(tp.FirstRowIndex + 1);

            ir = isheet1.CreateRow(1);
            ic = ir.CreateCell(1);
            ic.SetCellValue("子表结束位置");
            ic = ir.CreateCell(0);
            //在外面显示要多加一行
            ic.SetCellValue(tp.LastRowIndex + 1);

            ir = isheet1.CreateRow(3);
            ic = ir.CreateCell(1);
            ic.SetCellValue("页宽");
            ic = ir.CreateCell(0);
            ic.SetCellValue(tp.Width);


            ir = isheet1.CreateRow(4);
            ic = ir.CreateCell(1);
            ic.SetCellValue("页高");
            ic = ir.CreateCell(0);
            ic.SetCellValue(tp.Height);


            ir = isheet1.CreateRow(6);
            ic = ir.CreateCell(1);
            ic.SetCellValue("合计");
            ic = ir.CreateCell(0);
            ic.SetCellValue(tp.IsTotal);


            ir = isheet1.CreateRow(8);
            ic = ir.CreateCell(1);
            ic.SetCellValue("数据区类型");
            ic = ir.CreateCell(0);
            ic.SetCellValue(tp.DataAreaType.ToString());


            ir = isheet1.CreateRow(10);
            ic = ir.CreateCell(1);
            ic.SetCellValue("自定义JSON数据");

            ic = ir.CreateCell(0);

            if (tp.Sm == null)
            {
                tp.Sm = new SModel();
            }

            ic.SetCellValue(tp.Sm.ToJson());




            ir = isheet1.CreateRow(12);
            ic = ir.CreateCell(1);
            ic.SetCellValue("合计JSON数据");

            ic = ir.CreateCell(0);

            if (tp.TotalSm == null)
            {
                tp.TotalSm = new SModel();
            }

            ic.SetCellValue(tp.TotalSm.ToJson());


        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="ic">单元格对象</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="vlaue">值</param>
        public void SetCellValue(ICell ic, V1.CellTypeName typeName, string vlaue)
        {
            if (typeName == V1.CellTypeName.Date)
            {
                DateTime dt = DateTime.Parse(vlaue);
                ic.SetCellValue(dt);
            }
            else if (typeName == V1.CellTypeName.Double)
            {
                double d = double.Parse(vlaue);
                ic.SetCellValue(d);

            }
            else
            {
                ic.SetCellValue(vlaue);
            }

        }


        /// <summary>
        /// 设置单元格的属性
        /// </summary>
        /// <param name="ic">单元格对象</param>
        /// <param name="cp">自定义单元对象</param>
        /// <param name="workbook">整个Excel对象</param>
        public void SetCellStyl(ICell ic, V1.CellPro cp, IWorkbook workbook)
        {

            try
            {


                ICellStyle ics;

                string styleKey = string.Concat(
                    cp.BorderTopName, "|",
                    cp.BorderTopColor, "|",

                    cp.BorderLeftColor, "|",
                    cp.BorderLeftName, "|",

                    cp.BorderBottomColor, "|",
                    cp.BorderBottomName, "|",

                    cp.BorderRightColor, "|",
                    cp.BorderRightName, "|",

                    cp.Alignment, "|",
                    cp.VerticalAlignment, "|",
                    cp.WrapText, "|",
                    cp.ShrinkToFit, "|",
                    
                    cp.FillBackgroundColor, "|",
                    cp.FillForegroundColor, "|");




                if (!m_StyleBuffer.TryGetValue(styleKey, out ics))
                {

                    ics = workbook.CreateCellStyle();


                    ///对齐方式
                    ics.Alignment = cp.Alignment;
                    ///垂直对齐方式
                    ics.VerticalAlignment = cp.VerticalAlignment;
                    ///下边框类型和颜色
                    ics.BorderBottom = cp.BorderBottomName;
                    ics.BottomBorderColor = cp.BorderBottomColor;
                    ///上边框类型和颜色
                    ics.BorderTop = cp.BorderTopName;
                    ics.TopBorderColor = cp.BorderTopColor;
                    ///左边框类型和颜色
                    ics.BorderLeft = cp.BorderLeftName;
                    ics.LeftBorderColor = cp.BorderLeftColor;
                    ///右边框类型和颜色
                    ics.BorderRight = cp.BorderRightName;
                    ics.RightBorderColor = cp.BorderRightColor;

                    ///背景填充色
                    ics.FillBackgroundColor = cp.FillBackgroundColor;
                    ///填充前景色
                    ics.FillForegroundColor = cp.FillForegroundColor;
                    ///填充模式
                    ics.FillPattern = cp.FillPattern;
                    ///是否缩放
                    ics.ShrinkToFit = cp.ShrinkToFit;


                    //是否换行
                    ics.WrapText = cp.WrapText;

                    m_StyleBuffer.Add(styleKey, ics);

                }


                if (!string.IsNullOrWhiteSpace(cp.FontName))
                {


                    string fontKey = string.Concat(cp.FontName, "|", cp.FontSize, "|", cp.FontColor, "|", cp.FontBlod);

                    HSSFFont ifont;

                    if (!m_FontBuffer.TryGetValue(fontKey, out ifont))
                    {

                        ifont = workbook.CreateFont() as HSSFFont;

                        ///设置字体名称
                        ifont.FontName = cp.FontName;
                        ///设置字体颜色
                        ifont.Color = cp.FontColor;
                        ///设置字体大小
                        ifont.FontHeightInPoints = cp.FontSize;
                        ///设置字体粗细
                        ifont.Boldweight = cp.FontBlod;
                        ///斜体
                        ifont.IsItalic = cp.IsItalic;

                        m_FontBuffer.Add(fontKey, ifont);

                    }



                    ics.SetFont(ifont);

                }
                ic.CellStyle = ics;


            }
            catch (Exception ex)
            {
                throw new Exception("设置属性错误", ex);
            }

        }


        /// <summary>
        ///  创建模板  没有主表对象就生成单表模板
        /// </summary>
        /// <param name="path">生成模板路径</param>
        /// <param name="tsItem">子表信息</param>
        /// <param name="tsHead">主表信息 主表默认为空，就生成单表模板</param>
        public void CreateSingleTableTemp(string path, TableSet tsItem, TableSet tsHead = null)
        {

            int visibleSubCount = 0;   //显示的列

            foreach (IG2_TABLE_COL tCol in tsItem.Cols)
            {
                if (!tCol.IS_VISIBLE || !tCol.IS_LIST_VISIBLE)
                {
                    continue;
                }

                visibleSubCount++;
            }

            try
            {
                IWorkbook workbook = new HSSFWorkbook();


                ISheet isheet = workbook.CreateSheet("sheet1");


                ICellStyle ics = workbook.CreateCellStyle();

                IFont ifont = workbook.CreateFont();

                isheet.PrintSetup.Scale = 100;
                isheet.PrintSetup.PaperSize = (short)PaperSize.A4;
                isheet.PrintSetup.UsePage = true;

                isheet.PrintSetup.FitHeight = 0;
                isheet.PrintSetup.FitWidth = 1;
                isheet.FitToPage = false;


                if (tsHead != null)
                {
                    isheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, visibleSubCount - 1));
                    isheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, visibleSubCount - 1));

                    SetExcelHead(ics, isheet, ifont);
                }


                //代表行数
                int i = 0;

                if (tsHead != null)
                {
                    SetExcelItem(isheet, tsHead, out i, workbook);
                }


                TempParam tp = new TempParam();

                tp.FirstRowIndex = i + 1;
                tp.LastRowIndex = i + 1;

                CreateSheet_2(workbook, tp);

                if (tsItem != null)
                {
                    SetExcelSubItem(isheet, tsItem, i, workbook);
                }

                //设置整列的宽度  里面有默认宽度
                SetCellWidth(isheet, tsItem.Cols.Count);

                //新建一个Excel文件
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                //把数据流写进Excel中
                workbook.Write(file);

                file.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("生成Excel模板出错！", ex);
            }



        }


        /// <summary>
        /// 把主表信息写入到Excel中去
        /// </summary>
        /// <param name="isheet">工作薄</param>
        /// <param name="tsItem">主表信息</param>
        /// <param name="i">行数</param>
        /// <param name="workbook">Excel对象</param>
        void SetExcelItem(ISheet isheet, TableSet tsItem, out int i, IWorkbook workbook)
        {
            //从第二行开始写入主表数据
            i = 1;

            ICellStyle ics = workbook.CreateCellStyle();
            //左右居中
            ics.Alignment = HorizontalAlignment.CenterSelection;
            //垂直居中
            ics.VerticalAlignment = VerticalAlignment.Center;
            IFont ifont = workbook.CreateFont();
            //字体大小
            ifont.FontHeightInPoints = 25;
            ifont.FontName = "宋体";

            ics.SetFont(ifont);

            //表名
            IRow ir = isheet.CreateRow(i);
            //行高
            ir.Height = m_RowHegiht;
            ICell ic = ir.CreateCell(0);
            ic.SetCellValue(tsItem.Table.DISPLAY);
            ic.CellStyle = ics;

            List<IG2_TABLE_COL> colList = tsItem.Cols;


            int j = 0;

            i = 2;

            foreach (IG2_TABLE_COL col in colList)
            {
                if (!col.IS_VISIBLE || !col.IS_LIST_VISIBLE)
                {
                    continue;
                }

                //一行放两列
                if (j % 2 == 0)
                {
                    //主表字段行
                    ir = isheet.CreateRow(i);
                    ir.Height = m_RowHegiht;
                    ic = ir.CreateCell(0);
                    ic.SetCellValue(col.DISPLAY + ": " + "{$T." + col.DB_FIELD + "}");

                    i++;

                }
                else
                {
                    ic = ir.CreateCell(4);
                    ic.SetCellValue(col.DISPLAY + ": " + "{$T." + col.DB_FIELD + "}");
                }

                j++;
            }

        }


        /// <summary>
        /// 把子表数据写入到Excel中去
        /// </summary>
        /// <param name="i">行数</param>
        /// <param name="isheet">工作薄</param>
        /// <param name="tsSubItem">子表数据</param>
        /// <param name="workbook">Excel文件</param>
        void SetExcelSubItem(ISheet isheet, TableSet tsSubItem, int i, IWorkbook workbook)
        {

            IRow ir = isheet.CreateRow(i);
            IRow ir1 = isheet.CreateRow(i + 1);

            List<IG2_TABLE_COL> colList = tsSubItem.Cols;

            if (colList == null)
            {
                return;
            }



            ICellStyle ics = workbook.CreateCellStyle();
            IFont ifont = workbook.CreateFont();

            //字体大小
            ifont.FontHeightInPoints = 12;
            ifont.FontName = "新宋体";
            //粗体
            ifont.Boldweight = (short)FontBoldWeight.Bold;

            //左右居中
            ics.Alignment = HorizontalAlignment.Center;
            //顺直居中
            ics.VerticalAlignment = VerticalAlignment.Center;
            //下边框
            ics.BorderBottom = BorderStyle.Medium;
            //左边框
            ics.BorderLeft = BorderStyle.Medium;
            //右边框
            ics.BorderRight = BorderStyle.Medium;
            //上边框
            ics.BorderTop = BorderStyle.Medium;

            ics.SetFont(ifont);



            ICellStyle ics1 = workbook.CreateCellStyle();
            IFont ifont1 = workbook.CreateFont();

            //字体大小
            ifont1.FontHeightInPoints = 16;
            //字体名称
            ifont1.FontName = "宋体";

            //左对齐
            ics1.Alignment = HorizontalAlignment.Left;
            //垂直居中
            ics1.VerticalAlignment = VerticalAlignment.Center;
            //下边框
            ics1.BorderBottom = BorderStyle.Medium;
            //左边框
            ics1.BorderLeft = BorderStyle.Medium;
            //右边框
            ics1.BorderRight = BorderStyle.Medium;
            //上边框
            ics1.BorderTop = BorderStyle.Medium;

            ics1.SetFont(ifont1);


            //行高
            ir.Height = m_RowHegiht;
            //行高
            ir1.Height = m_RowHegiht;

            int j = 0;

            foreach (IG2_TABLE_COL col in colList)
            {
                if (!col.IS_VISIBLE || !col.IS_LIST_VISIBLE)
                {
                    continue;
                }

                ICell ic = ir.CreateCell(j);
                ic.SetCellValue(col.DISPLAY);
                ic.CellStyle = ics;


                ic = ir1.CreateCell(j);
                ic.SetCellValue("{$T." + col.DB_FIELD + "}");
                ic.CellStyle = ics1;

                j++;
            }



        }

        /// <summary>
        /// 动态计算行高度
        /// </summary>
        /// <param name="sp">整个模板sheet对象</param>
        public void GetDynHegiht(SheetParam sp)
        {
            //获取自动换行的字体高度
            foreach (var row in sp.Rows)
            {
                foreach (var cell in row)
                {
                    //不是自动换行的就不处理
                    if (!cell.WrapText) { continue; }

                    if (m_gh == null)
                    {
                        m_TmpBmp = new Bitmap(2, 2);
                        m_gh = Graphics.FromImage(m_TmpBmp);
                    }

                    Font myF;

                    string fontKey = string.Concat(cell.FontName, "|", cell.FontSize);

                    if (!m_font.TryGetValue(fontKey, out myF))
                    {
                        myF = new Font(cell.FontName, cell.FontSize);

                        m_font.Add(fontKey, myF);
                    }


                    //单元格宽
                    int CellWidth = cell.WidthPx;

                    if (cell.SpanWidthPx != 0)
                    {
                        CellWidth = cell.SpanWidthPx;
                    }


                    SizeF sf = m_gh.MeasureString(cell.Value, myF, CellWidth);

                    cell.DynHeight = sf.Height;

                }
            }

            //设置行高为最高的列高度
            foreach (var row in sp.Rows)
            {
                float height = 0;

                foreach (var cell in row)
                {
                    height = Math.Max(height, cell.DynHeight);

                }

                if (height == 0) { continue; }

                //换算单位 从像素换到磅  
                float sum = height * 0.75f + 5;

                if (sum < row.RowHeight) { continue; }

                row.RowHeight = sum;

            }

        }

        /// <summary>
        /// 读取模板的第二页自定义参数  放到一个字典里面
        /// </summary>
        /// <param name="sheet">sheet对象</param>
        public TempParam GetSheet2(HSSFSheet sheet)
        {

            TempParam tp = new TempParam();

            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = 0; i <= rowCount; i++)
            {

                //拿到每一行的对象
                IRow row = sheet.GetRow(i);

                if (row == null)
                {
                    continue;
                }

                string key = GetCellValue(row.GetCell(1));

                string value = GetCellValue(row.GetCell(0));

                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                tp.ParamDict.Add(key, value);


            }


            #region 是否有合计

            if (tp.ParamDict.ContainsKey("合计"))
            {
                string value = tp.ParamDict["合计"];


                tp.IsTotal = StringUtil.ToBool(value);

            }



            if (tp.ParamDict.ContainsKey("合计JSON数据"))
            {
                string json_1 = tp.GetParam("合计JSON数据");

                try
                {

                    if (!string.IsNullOrWhiteSpace(json_1))
                    {
                        tp.TotalSm = SModel.ParseJson(json_1);
                    }


                }
                catch (Exception ex)
                {

                    log.Error("序列化合计JSON数据出错了！", ex);

                }


            }


            #endregion

            #region 获取到页宽 和 页高

            if (tp.ParamDict.ContainsKey("页宽"))
            {

                string value = tp.ParamDict["页宽"];

                tp.Width = StringUtil.ToInt(value);

            }


            if (tp.ParamDict.ContainsKey("页高"))
            {

                string value = tp.ParamDict["页高"];

                tp.Height = StringUtil.ToInt(value);

            }


            #endregion


            #region 获取到数据区开始行索引 和结束行索引

            if (tp.ParamDict.ContainsKey("子表开始位置"))
            {

                string value = tp.ParamDict["子表开始位置"];

                tp.FirstRowIndex = StringUtil.ToInt32(value);

                if (tp.FirstRowIndex > 0)
                {
                    //变成实际的行索引
                    tp.FirstRowIndex--;
                }

            }


            if (tp.ParamDict.ContainsKey("子表结束位置"))
            {

                string value = tp.ParamDict["子表结束位置"];

                tp.LastRowIndex = StringUtil.ToInt32(value);

                if (tp.LastRowIndex > 0)
                {
                    //变成实际的行索引
                    tp.LastRowIndex--;
                }


            }


            #endregion

            #region 获取数据类型


            if (tp.ParamDict.ContainsKey("数据区类型"))
            {

                string value = tp.ParamDict["数据区类型"]?.Trim();

                if (value.Equals("COLUMN_REPEAT", StringComparison.CurrentCultureIgnoreCase))
                {
                    tp.DataAreaType = DataAreaType.COLUMN_REPEAT;
                }
                if (value.Equals("MORE_SUB_TABLE", StringComparison.CurrentCultureIgnoreCase))
                {
                    tp.DataAreaType = DataAreaType.MORE_SUB_TABLE;
                }
                if(value.Equals("ROW_MERGE_IMG", StringComparison.CurrentCultureIgnoreCase))
                {
                    tp.DataAreaType = DataAreaType.ROW_MERGE_IMG;
                }

            }


            #endregion

            #region  获取自定义JSON数据

            if (tp.ParamDict.ContainsKey("自定义JSON数据"))
            {

                string json = tp.GetParam("自定义JSON数据");

                try
                {

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        tp.Sm = SModel.ParseJson(json);
                    }


                }
                catch (Exception ex)
                {

                    log.Error("序列化自定义JSON数据出错了！", ex);

                }

            }

            #endregion



            return tp;

        }

        /// <summary>
        /// 获取图片对象和位置
        /// </summary>
        /// <param name="ts">模板的第一页参数对象</param>
        /// <param name="workbook">整个Excel对象</param>
        /// <param name="sheet">模板第一页对象</param>
        public void GetImg(TempSheet ts, IWorkbook workbook, HSSFSheet sheet)
        {

            //读取图片的
            IList pictures = workbook.GetAllPictures();

            foreach (HSSFPictureData pid in pictures)
            {
                //拿到图片集合
                ts.Pictures.Add(pid);
            }


            HSSFShapeContainer hp = (HSSFShapeContainer)sheet.DrawingPatriarch;

            //拿到图片位置
            if (hp != null)
            {
                foreach (var pic in hp.Children)
                {
                    //只有这个才是图片
                    if(pic is HSSFPicture)
                    {
                        HSSFClientAnchor hca = (HSSFClientAnchor)pic.Anchor;
                        ts.Hcas.Add(hca);
                    }
                }
            }

        }

        /// <summary>
        /// 获取合并单元格信息
        /// </summary>
        /// <param name="ts">模板的第一页参数对象</param>
        /// <param name="sheet">模板第一页对象</param>
        public void GetSpanCellAndRow(TempSheet ts, HSSFSheet sheet)
        {


            V1.RowPro rp = new V1.RowPro();

            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                V1.CellPro cp = new V1.CellPro();
                CellRangeAddress rAdd = sheet.GetMergedRegion(i);
                cp.SpanFirstCell = rAdd.FirstColumn;
                cp.SpanLastCell = rAdd.LastColumn;
                cp.SpanFirstRow = rAdd.FirstRow;
                cp.SpanLastRow = rAdd.LastRow;

                rp.Add(cp);

            }

            ts.SpanCellAndRow = rp;

        }

        /// <summary>
        /// 拿到单元格的宽和高，X轴和Y轴
        /// </summary>
        /// <param name="ts">模板的第一页参数对象</param>
        /// <param name="rowProList">所有行数据集合</param>
        public void GetCellWidthAndHeight(TempSheet ts, V1.RowProCollection rowProList)
        {

            try
            {

                int rowCount = rowProList.Count;

                float y1 = 0;
                float x1 = 0;

                for (int i = 0; i < rowCount; i++)
                {
                    x1 = 0;

                    V1.RowPro rp = rowProList[i];

                    int rpCount = rp.Count;

                    for (int j = 0; j < rpCount; j++)
                    {
                        V1.CellPro cp = rp[j];
                        cp.Width = ts.ColumnWidth[j] / 48.7619f;

                        //这是直接获取单元格列宽度像素
                        cp.WidthPx = ts.ColumnWidthPx[j];

                        cp.Height = rp.RowHeight;
                        cp.X = x1;
                        cp.Y = y1;

                        x1 += cp.Width;

                    }

                    y1 += rp.RowHeight;

                }

                foreach (V1.CellPro cp in ts.SpanCellAndRow)
                {
                    int sfc = cp.SpanFirstCell;
                    int sfr = cp.SpanFirstRow;
                    int slc = cp.SpanLastCell;
                    int slr = cp.SpanLastRow;


                    V1.CellPro item = rowProList[sfr][sfc];

                    item.SpanFirstCell = sfc;
                    item.SpanFirstRow = sfr;
                    item.SpanLastCell = slc;
                    item.SpanLastRow = slr;

                    for (int i = sfc; i <= slc; i++)
                    {
                        var row = rowProList[sfr];

                        if (row.Count < slc - 1)
                        {
                            continue;
                        }

                        item.SpanWidth += rowProList[sfr][i].Width;
                        item.SpanWidthPx += rowProList[sfr][i].WidthPx;
                    }


                    for (int i = sfr; i <= slr; i++)
                    {
                        item.SpanHeight += rowProList[i][sfc].Height;
                    }


                    for (int j = sfr; j <= slr; j++)
                    {

                        for (int i = sfc; i <= slc; i++)
                        {

                            var row = rowProList[j];

                            if (row.Count < slc - 1)
                            {
                                continue;
                            }

                            V1.CellPro cp1 = rowProList[j][i];

                            cp1.Display = false;
                        }

                    }

                    item.Display = true;


                }


            }
            catch (Exception ex)
            {
                throw new Exception("拿单元格相对位置出错了！", ex);
            }


        }

        /// <summary>
        /// 获取工作薄的打印属性
        /// </summary>
        /// <param name="sheet">模板第一页对象</param>
        /// <param name="pp">打印属性对象</param>
        public void GetPrintPro(ISheet sheet, PrintPro pp)
        {
            HSSFPrintSetup hps = sheet.PrintSetup as HSSFPrintSetup;
            IPrintSetup ips = sheet.PrintSetup;
            pp.FitHeight = ips.FitHeight;
            pp.FitWidth = ips.FitWidth;
            pp.IsDraft = ips.Draft;
            pp.Landscape = ips.Landscape;
            pp.LeftToRight = ips.LeftToRight;
            pp.NoColors = ips.NoColor;
            pp.PageStart = ips.PageStart;
            pp.PaperSize = ips.PaperSize;
            pp.Scale = ips.Scale;
            pp.UsePage = ips.UsePage;
        }

        /// <summary>
        /// 拿页面边距
        /// </summary>
        /// <param name="sheet">模板第一页对象</param>
        /// <param name="sheetMargin">页面间距对象</param>
        public void GetSheetMarginPro(ISheet sheet, V1.SheetMargin sheetMargin)
        {
            sheetMargin.BottomMargin = sheet.GetMargin(MarginType.BottomMargin);

            // sp.sheetMargin.FooterMargin = sheet.GetMargin(MarginType.FooterMargin);

            // sp.sheetMargin.HeaderMargin = sheet.GetMargin(MarginType.HeaderMargin);

            sheetMargin.LeftMargin = sheet.GetMargin(MarginType.LeftMargin);
            sheetMargin.RightMargin = sheet.GetMargin(MarginType.RightMargin);
            sheetMargin.TopMargin = sheet.GetMargin(MarginType.TopMargin);


        }

        /// <summary>
        /// 处理头部模板或底部模板
        /// </summary>
        /// <param name="rows">头部或底部所有Excel行集合</param>
        /// <param name="head">主表数据</param>
        public V1.RowProCollection HandlerHeaderAndFooterTemp(V1.RowProCollection rows, LModel head, SheetParam sp = null)
        {
            try
            {


                V1.RowProCollection newRows = new V1.RowProCollection();

                foreach (V1.RowPro rp in rows)
                {
                    V1.RowPro newRp = rp.Clone();

                    newRows.Add(newRp);

                    foreach (V1.CellPro cp in newRp)
                    {
                        JTemplate jt = new JTemplate();

                        jt.ModelList = null;

                        jt.SrcText = cp.Value;
                        jt.Model = head;
                        cp.Value = jt.Exec();

                        if (sp != null)
                        {
                            ReadImg(cp, sp);
                        }
                    }
                }


                newRows.SpanCellAndRow = rows.SpanCellAndRow;
                newRows.hcaList = rows.hcaList;
                newRows.Height = rows.Height;
                newRows.picturesList = rows.picturesList;


                return newRows;

            }
            catch (Exception ex)
            {

                log.Error("处理头部或底部模板出错了！", ex);
                throw ex;

            }


        }

        /// <summary>
        /// 拷贝数据区模板集合
        /// </summary>
        /// <param name="item">数据区模板集合</param>
        /// <returns></returns>
        public V1.RowProCollection CopyTemp(V1.RowProCollection item)
        {

            try
            {

                V1.RowProCollection rpList = new V1.RowProCollection();

                foreach (V1.RowPro rp in item)
                {
                    rpList.Add(rp.Clone());
                }

                rpList.SpanCellAndRow = item.SpanCellAndRow;
                rpList.hcaList = item.hcaList;
                rpList.picturesList = item.picturesList;

                return rpList;

            }
            catch (Exception ex)
            {
                throw new Exception("拷贝中间数据区模板发生错误！", ex);
            }

        }

        /// <summary>
        /// 设置Excel的合并单元格 和 列宽度
        /// </summary>
        /// <param name="sheet">NOPI 生成的sheet对象</param>
        /// <param name="sp">整个模板sheet对象</param>
        public void CreateSpanCellAndCellWidth(ISheet sheet, SheetParam sp)
        {
            //合并单元格
            foreach (V1.CellPro cp in sp.TempSheet.SpanCellAndRow)
            {

                sheet.AddMergedRegion(new CellRangeAddress(cp.SpanFirstRow, cp.SpanLastRow, cp.SpanFirstCell, cp.SpanLastCell));

            }
            //设置整列的宽度
            for (int i = 0; i < sp.TempSheet.ColumnNum; i++)
            {
                    sheet.SetColumnWidth(i, sp.TempSheet.ColumnWidth[i]);
            }



        }

        /// <summary>
        /// 根据整个sheet数据对象来创建Excel文件
        /// </summary>
        /// <param name="sp">整个sheet数据对象</param>
        /// <param name="file_path">Excel文件路径</param>
        public void CreateExcel(SheetParam sp, string file_path)
        {
            IWorkbook workbook = new HSSFWorkbook();

            //创建表名为keyBooks的表
            ISheet sheet = workbook.CreateSheet("sheet1");


            //这里要处理内容了
            SetRowPro(sp, sheet, workbook);

            // 设置Excel的合并单元格 和 列宽度
            CreateSpanCellAndCellWidth(sheet, sp);

            //false ---- 选择缩放比例  这个不知道是干嘛的
            sheet.FitToPage = false;

            //设置打印属性的
            SetPrintPor(sheet, sp.TempSheet);

            //设置页面外边距
            SetSheetMargin(sheet, sp.TempSheet);

            //设置图片到Excel中去
            SetImg(workbook, sheet, sp.TempSheet);

            //创建Excel第二页参数
            CreateSheet_2(workbook, sp.TempParam);


            //新建一个Excel文件
            using (FileStream file = new FileStream(file_path, FileMode.OpenOrCreate))
            {
                try
                {
                    //把数据流写进Excel中
                    workbook.Write(file);
                }
                catch(Exception ex)
                {
                    log.Debug("字体数量:" + m_FontBuffer.Count);

                    log.Debug("样式数量："+ m_StyleBuffer.Count);


                    foreach (var item in m_FontBuffer)
                    {
                        log.Debug("--" + item.Key);
                    }

                    throw ex;
                }
            }

             
        }

        /// <summary>
        /// 设置所有行的数据
        /// </summary>
        /// <param name="sp">整个模板sheet对象</param>
        /// <param name="sheet">工作薄</param>
        /// <param name="workbook">整个Excel对象</param>
        void SetRowPro(SheetParam sp, ISheet sheet, IWorkbook workbook)
        {


            for (int i = 0; i < sp.Rows.Count; i++)
            {
                V1.RowPro rp = sp.Rows[i];
                //新建一行
                IRow row = sheet.CreateRow(i);
                //行高
                row.HeightInPoints = rp.RowHeight;

                //这是空行数据
                if (rp.Count == 0)
                {
                    row.Height = 225;
                    for (int k = 0; k < sp.TempSheet.ColumnNum; k++)
                    {
                        ICell ic = row.CreateCell(k);
                        ic.SetCellValue("");
                    }

                    continue;

                }

                for (int j = 0; j < rp.Count; j++)
                {
                    V1.CellPro cp = rp[j];
                    ICell ic = row.CreateCell(cp.ColsIndex);
                    SetCellValue(ic, cp.CellType, cp.Value);

                    SetCellStyl(ic, cp, workbook);
                    if (!string.IsNullOrEmpty(cp.FormatName))
                    {
                        IDataFormat df = workbook.CreateDataFormat();
                        //设置数据格式
                        ic.CellStyle.DataFormat = df.GetFormat(cp.FormatName);
                    }

                }

            }


        }


        /// <summary>
        /// 把整个数据集合弄成一个分页数据的
        /// </summary>
        /// <param name="items">数据集合</param>
        /// <param name="sp">sheet对象</param>
        /// <returns></returns>
        public Dictionary<int, List<LModel>> CreateExcel_CreatePageData(List<LModel> items, SheetParam sp)
        {

            Dictionary<int, List<LModel>> PageData = new Dictionary<int, List<LModel>>();

            float numHeight = 0;
            int i = 1;

            List<LModel> lms = new List<LModel>();

            TempParam tp = sp.TempParam;



            try
            {

                PageData.Add(i, lms);


                foreach (LModel item in items)
                {

                    numHeight += sp.DataAreaTemp.Height;

                    if (numHeight + sp.TotalTemp.RowHeight > sp.TempSheet.ContentHeight)
                    {
                        i++;

                        lms = new List<LModel>();

                        PageData.Add(i, lms);

                        numHeight = sp.DataAreaTemp.Height;
                    }

                    lms.Add(item);

                }

                return PageData;

            }
            catch (Exception ex)
            {
                log.Error("把数据集合分页出错了！", ex);

                throw ex;

            }
        }


        /// <summary>
        /// 根据数据来创建合并单元格信息
        /// </summary>
        /// <param name="SpanCellAndRow"></param>
        /// <param name="tp"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public V1.RowPro GetDataAreaSpanCellAndRow(V1.RowPro SpanCellAndRow, TempParam tp,int i)
        {
            int num = tp.LastRowIndex - tp.FirstRowIndex + 1;

            V1.RowPro rp = new V1.RowPro();

            foreach (V1.CellPro cp in SpanCellAndRow)
            {
                V1.CellPro cp1 = new V1.CellPro();
                cp1.SpanFirstRow = cp.SpanFirstRow + num * i;
                cp1.SpanLastRow = cp.SpanLastRow + num * i;
                cp1.SpanFirstCell = cp.SpanFirstCell;
                cp1.SpanLastCell = cp.SpanLastCell;
                rp.Add(cp1);
            }


            return rp;

        }


        /// <summary>
        /// 把单元格里面的图片路径读取出来
        /// </summary>
        /// <param name="cp">单元格对象</param>
        /// <param name="num">数据区行合计</param>
        /// <param name="i">子表数据循环索引</param>
        /// <param name="sp">sheet参数对象</param>
        public void ReadImg(V1.CellPro cp,int num,int i, SheetParam sp)
        {
            //这是证明是图片来的 
            if (!cp.Value.StartsWith("img:")) 
            {
                return;
            }

            string imgVal = cp.Value.Substring(4);

            SModelList sms = BusHelper.ParseImgField(imgVal);

            cp.Value = "";

            if (sms.Count == 0)
            {
                return;
            }


            string imgUrl = sms[0].Get<string>("url");

            string imgPath = HttpContext.Current.Server.MapPath(imgUrl);

            using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
            {
                Image image = Image.FromStream(fs);

                BinaryReader r = new BinaryReader(fs);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                DefPictures dp = new DefPictures
                {
                    Bytes = r.ReadBytes((int)r.BaseStream.Length),
                    Dx1 = 0,
                    Dx2 = 1023,
                    Dy1 = 0,
                    Dy2 = 255,
                    Col1 = cp.ColsIndex,
                    Col2 = cp.ColsIndex,
                    Row1 = cp.RowIndex + num * i,
                    Row2 = cp.RowIndex + num * i,
                    Width = image.Width,
                    Height = image.Height
                };


                sp.TempSheet.DefPictures.Add(dp);

              
            }

          
        }


        /// <summary>
        /// 把单元格里面的图片路径读取出来
        /// </summary>
        /// <param name="cp">单元格对象</param>
        /// <param name="num">数据区行合计</param>
        /// <param name="i">子表数据循环索引</param>
        /// <param name="sp">sheet参数对象</param>
        public void ReadImg(V1.CellPro cp, SheetParam sp)
        {
            //这是证明是图片来的 
            if (!cp.Value.StartsWith("img:"))
            {
                return;
            }

            //图片地址
            string imgVal = cp.Value.Substring(4);

            #region  图片地址前面有0的问题  

            imgVal = imgVal.Replace("0\n/UserFile", "/UserFile");

            #endregion

            SModelList sms = BusHelper.ParseImgField(imgVal);

            cp.Value = "";

            if (sms.Count == 0)
            {
                return;
            }

            string imgUrl = sms[0].Get<string>("url");

            string imgPath = HttpContext.Current.Server.MapPath(imgUrl);

            using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
            {
                Image image = Image.FromStream(fs);

                BinaryReader r = new BinaryReader(fs);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                DefPictures dp = new DefPictures
                {
                    Bytes = r.ReadBytes((int)r.BaseStream.Length),
                    Dx1 = 0,
                    Dx2 = 1023,
                    Dy1 = 0,
                    Dy2 = 255,
                    Col1 = cp.SpanFirstCell,
                    Col2 = cp.SpanLastCell,
                    Row1 = cp.SpanFirstRow,
                    Row2 = cp.SpanLastRow,
                    Width = image.Width,
                    Height = image.Height
                };

                sp.TempSheet.DefPictures.Add(dp);
            }


        }

    }
}
