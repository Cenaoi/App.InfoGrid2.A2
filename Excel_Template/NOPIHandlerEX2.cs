using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using HWQ.Entity.LightModels;
using NPOI.HSSF.Model;
using System.Collections;
using NPOI.HSSF.Record;
using System.Drawing;

namespace App.InfoGrid2.Excel_Template
{
   public class NOPIHandlerEX2
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 模板页
        /// </summary>
        string m_TemplateShare = "Sheet1";

        /// <summary>
        /// 参数页
        /// </summary>
        string m_ParamShare = "Sheet2";

        /// <summary>
        /// 绘制工具
        /// </summary>
        public Graphics m_gh;

        Bitmap m_TmpBmp;


        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="path">Excel模板的路径</param>
        public SheetPro ReadExcel(string path)
        {
            ///判断文件是否存在
            if (!File.Exists(path))
            {
                throw new Exception("模板文件不存在！");
            }

            try
            {

                SheetPro sp = new SheetPro();
                IWorkbook workbook;

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {

                    //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                    workbook = new HSSFWorkbook(fs);

                }



                #region ///读取图片



                ///测试读取图片的
                IList pictures = workbook.GetAllPictures();
                foreach (HSSFPictureData pid in pictures)
                {
                    ///拿到图片集合
                    sp.picturesList.Add(pid);
                }

                //获取excel的第一个sheet
                NPOI.HSSF.UserModel.HSSFSheet sheet = workbook.GetSheet(m_TemplateShare) as NPOI.HSSF.UserModel.HSSFSheet;



                HSSFPatriarch hp = (HSSFPatriarch)sheet.DrawingPatriarch;

                if (hp != null)
                {
                    foreach (HSSFPicture pic in hp.Children)
                    {
                        HSSFClientAnchor hca = (HSSFClientAnchor)pic.Anchor;
                        sp.hcaList.Add(hca);
                    }
                }



                #endregion
                GetCellNum(sheet, sp);
                GetRoowAllData(sheet, workbook, sp);

                ///这是要计算工作表里面的所有计算公式
                //sheet.ForceFormulaRecalculation = true;

                int regNum = sheet.NumMergedRegions;

                RowPro rp = new RowPro();

                for (int i = 0; i < regNum; i++)
                {
                    CellPro cp = new CellPro();
                    CellRangeAddress rAdd = sheet.GetMergedRegion(i);
                    cp.SpanFirstCell = rAdd.FirstColumn;
                    cp.SpanLastCell = rAdd.LastColumn;
                    cp.SpanFirstRow = rAdd.FirstRow;
                    cp.SpanLastRow = rAdd.LastRow;

                    rp.cellPro.Add(cp);

                }
                sp.SpanCellAndRow = rp;


                ///拿打印属性
                GetPrintPor(sheet, sp);


                ///拿页面边距
                GetSheetMargin(sheet, sp);


                ///拿到整列的宽度
                for (int i = 0; i < sp.ColumnNum; i++)
                {
                    sp.ColumnWidth.Add(i, sheet.GetColumnWidth(i));
                }


                try
                {
                    ///获取第二张表的信息
                    sheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.GetSheet(m_ParamShare);

                    sp.subFirstRow = GetSubFirstAndLast(sheet, 0, workbook);
                    sp.subLastRow = GetSubFirstAndLast(sheet, 1, workbook);
                    sp.Width = GetNum(sheet, 3, workbook);
                    sp.Height = GetNum(sheet, 4, workbook);


                }
                catch (Exception ex)
                {
                    log.Error("第二张Sheet不存在", ex);
                    sp.subFirstRow = 0;
                    sp.subLastRow = 0;

                }


                GetCellWidthAndHeight(sp);



                sp.Header = GetTitle(sp);
                sp.DataArea = GetData(sp);
                sp.Bottom = GetBottom(sp);

                sp.Header.ProSize();
                sp.DataArea.ProSize();
                sp.Bottom.ProSize();


                if (sp.Height != 0)
                {
                    sp.PageHeight = sp.Height * 3.9370079d;
                }

                if (sp.Width != 0)
                {
                    sp.PageWidth = sp.Width * 3.9370079d;
                }


                return sp;



            }
            catch (Exception ex)
            {


                throw new Exception("读取Excel模板出错！", ex);

            }
        }

        /// <summary>
        /// 拿到子表的自定义页高和宽
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="index">行数，从0开始</param>
        /// <returns>第一列的值</returns>
        private int GetNum(ISheet sheet, int index, IWorkbook workbook)
        {
            IRow row = sheet.GetRow(index);

            if (row == null)
            {
                return 0;
            }

            if (row.GetCell(0) == null)
            {
                return 0;
            }

            string value = GetCellValue(row.GetCell(0));


            if (int.TryParse(value, out index))
            {

                if (index == 0)
                {
                    return 0;
                }

                return index;
            }

            return 0;
        }



        /// <summary>
        /// 拿到列的数量
        /// </summary>
        /// <returns></returns>
        private void GetCellNum(ISheet sheet, SheetPro sp)
        {
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = 0; i < rowCount; i++)
            {

                ///拿到每一行的对象
                IRow row = sheet.GetRow(i);
                ///判断行是否有数据
                if (row == null)
                {
                    continue;
                }

                sp.ColumnNum = Math.Max(sp.ColumnNum, row.LastCellNum);
            }
        }


        /// <summary>
        /// 拿到单元格的宽和高，X轴和Y轴
        /// </summary>
        private void GetCellWidthAndHeight(SheetPro sp)
        {
            try
            {

                int rowCount = sp.rowProList.Count;

                float y1 = 0;
                float x1 = 0;

                for (int i = 0; i < rowCount; i++)
                {
                    x1 = 0;

                    RowPro rp = sp.rowProList[i];

                    int rpCount = rp.cellPro.Count;

                    for (int j = 0; j < rpCount; j++)
                    {
                        CellPro cp = rp.cellPro[j];
                        cp.Width = (float)(sp.ColumnWidth[j]) / 48.7619f;
                        cp.Height = rp.RowHeight;
                        cp.X = x1;
                        cp.Y = y1;

                        x1 += cp.Width;

                    }

                    y1 += rp.RowHeight;

                }

                foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                {
                    int sfc = cp.SpanFirstCell;
                    int sfr = cp.SpanFirstRow;
                    int slc = cp.SpanLastCell;
                    int slr = cp.SpanLastRow;


                    CellPro item = sp.rowProList[sfr].cellPro[sfc];

                    item.SpanFirstCell = sfc;
                    item.SpanFirstRow = sfr;
                    item.SpanLastCell = slc;
                    item.SpanLastRow = slr;

                    for (int i = sfc; i <= slc; i++)
                    {
                        item.SpanWidth += sp.rowProList[sfr].cellPro[i].Width;
                    }


                    for (int i = sfr; i <= slr; i++)
                    {
                        item.SpanHeight += sp.rowProList[i].cellPro[sfc].Height;
                    }


                    for (int j = sfr; j <= slr; j++)
                    {

                        for (int i = sfc; i <= slc; i++)
                        {
                            CellPro cp1 = sp.rowProList[j].cellPro[i];

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
        /// 拿页面边距
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sp"></param>
        private void GetSheetMargin(ISheet sheet, SheetPro sp)
        {
            sp.sheetMargin.BottomMargin = sheet.GetMargin(MarginType.BottomMargin);



            // sp.sheetMargin.FooterMargin = sheet.GetMargin(MarginType.FooterMargin);

            // sp.sheetMargin.HeaderMargin = sheet.GetMargin(MarginType.HeaderMargin);
            sp.sheetMargin.LeftMargin = sheet.GetMargin(MarginType.LeftMargin);
            sp.sheetMargin.RightMargin = sheet.GetMargin(MarginType.RightMargin);
            sp.sheetMargin.TopMargin = sheet.GetMargin(MarginType.TopMargin);
        }

        /// <summary>
        /// 设置页面边距
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sp"></param>
        private void SetSheetMargin(ISheet sheet, SheetPro sp)
        {
            sheet.SetMargin(MarginType.BottomMargin, sp.sheetMargin.BottomMargin);
            //sheet.SetMargin(MarginType.FooterMargin, sp.sheetMargin.FooterMargin);
            //sheet.SetMargin(MarginType.HeaderMargin, sp.sheetMargin.HeaderMargin);
            sheet.SetMargin(MarginType.LeftMargin, sp.sheetMargin.LeftMargin);
            sheet.SetMargin(MarginType.RightMargin, sp.sheetMargin.RightMargin);
            sheet.SetMargin(MarginType.TopMargin, sp.sheetMargin.TopMargin);
        }

        /// <summary>
        /// 拿到头一部分
        /// </summary>
        /// <returns></returns>
        private RowProCollection GetTitle(SheetPro sp)
        {

            int subFirstRow = sp.subFirstRow;

            int subLastRow = sp.subLastRow;

            RowProCollection spTitel = new RowProCollection();

            if (sp.subFirstRow == 0 && sp.subLastRow == 0)
            {
                return spTitel;
            }

            for (int i = 0; i < subFirstRow; i++)
            {
                spTitel.Add(sp.rowProList[i]);
            }

            foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
            {
                if (cp.SpanFirstRow >= subFirstRow) { continue; }

                spTitel.SpanCellAndRow.cellPro.Add(cp);

            }


            for (int i = 0; i < sp.hcaList.Count; i++)
            {
                HSSFClientAnchor hca = sp.hcaList[i];
                HSSFPictureData hpd = sp.picturesList[i];
                if (hca.Row1 >= subFirstRow) { continue; }

                spTitel.hcaList.Add(hca);
                spTitel.picturesList.Add(hpd);

            }

            return spTitel;

        }

        /// <summary>
        /// 拿到中间数据部分
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private RowProCollection GetData(SheetPro sp)
        {
            RowProCollection spData = new RowProCollection();

            if (sp.subFirstRow == 0 && sp.subLastRow == 0)
            {
                spData.AddRange(sp.rowProList);
                return spData;
            }

            for (int i = sp.subFirstRow; i <= sp.subLastRow; i++)
            {
                spData.Add(sp.rowProList[i]);
            }

            foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
            {
                if (cp.SpanFirstRow < sp.subFirstRow || cp.SpanFirstRow > sp.subLastRow) { continue; }

                spData.SpanCellAndRow.cellPro.Add(cp);


            }




            for (int i = 0; i < sp.hcaList.Count; i++)
            {
                HSSFClientAnchor hca = sp.hcaList[i];
                HSSFPictureData hpd = sp.picturesList[i];
                if (hca.Row1 < sp.subFirstRow || hca.Row2 > sp.subLastRow) { continue; }

                spData.hcaList.Add(hca);
                spData.picturesList.Add(hpd);

            }


            return spData;



        }


        /// <summary>
        /// 拿到底部
        /// </summary>
        /// <returns></returns>
        private RowProCollection GetBottom(SheetPro sp)
        {
            RowProCollection spBot = new RowProCollection();
            if (sp.subFirstRow == 0 && sp.subLastRow == 0)
            {
                return spBot;
            }


            for (int i = sp.subLastRow + 1; i < sp.rowProList.Count; i++)
            {
                spBot.Add(sp.rowProList[i]);
            }

            foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
            {
                if (cp.SpanFirstRow <= sp.subLastRow) { continue; }
                spBot.SpanCellAndRow.cellPro.Add(cp);

            }

            for (int i = 0; i < sp.hcaList.Count; i++)
            {
                HSSFClientAnchor hca = sp.hcaList[i];
                HSSFPictureData hpd = sp.picturesList[i];
                if (hca.Row1 <= sp.subLastRow) { continue; }

                spBot.hcaList.Add(hca);
                spBot.picturesList.Add(hpd);

            }

            return spBot;
        }


        /// <summary>
        /// 设置打印属性的
        /// </summary>
        /// <param name="sheet">工作薄</param>
        /// <param name="sp">工作薄类</param>
        private void SetPrintPor(ISheet sheet, SheetPro sp)
        {
            IPrintSetup ips = sheet.PrintSetup;
            ips.FitHeight = sp.pp.FitHeight;
            ips.FitWidth = sp.pp.FitWidth;
            ips.Draft = sp.pp.IsDraft;
            ips.Landscape = sp.pp.Landscape;
            ips.LeftToRight = sp.pp.LeftToRight;
            ips.NoColor = sp.pp.NoColors;
            ips.PageStart = sp.pp.PageStart;
            ips.PaperSize = sp.pp.PaperSize;
            ips.Scale = sp.pp.Scale;
            ips.UsePage = sp.pp.UsePage;

        }

        /// <summary>
        /// 获取工作薄的打印属性
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sp"></param>
        private void GetPrintPor(ISheet sheet, SheetPro sp)
        {

            HSSFPrintSetup hps = sheet.PrintSetup as HSSFPrintSetup;



            IPrintSetup ips = sheet.PrintSetup;

            PrintPro pp = new PrintPro();

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


            sp.pp = pp;

        }



        /// <summary>
        /// 拿到所有行的数据
        /// </summary>
        private void GetRoowAllData(ISheet sheet, IWorkbook workbook, SheetPro sp)
        {
            try
            {
                //最后一列的标号  即总的行数
                int rowCount = sheet.LastRowNum;

                for (int i = 0; i <= rowCount; i++)
                {

                    ///拿到每一行的对象
                    IRow row = sheet.GetRow(i);
                    ///判断行是否有数据
                    if (row == null)
                    {
                        RowPro rp1 = new RowPro();
                        sp.rowProList.Add(rp1);

                        continue;
                    }




                    RowPro rp = new RowPro();

                    rp.RowHeight = row.HeightInPoints;



                    GetCellAllData(row, i, workbook, rp, sp.ColumnNum);

                    sp.rowProList.Add(rp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("拿到行数据发生错误！", ex);

            }
        }
        /// <summary>
        /// 拿到所有单元格的数据
        /// </summary>
        private void GetCellAllData(IRow row, int i, IWorkbook workbook, RowPro rp, int columnNum)
        {
            try
            {
                for (int j = 0; j < columnNum; j++)
                {
                    ///拿到每一个单元格的对象
                    NPOI.HSSF.UserModel.HSSFCell icell = row.GetCell(j) as NPOI.HSSF.UserModel.HSSFCell;


                    ///判断单元格是否有数据
                    if (icell == null)
                    {
                        CellPro cp1 = new CellPro();
                        cp1.Value = "";
                        cp1.FontName = "新宋体";
                        cp1.FontSize = 12;
                        cp1.ColsIndex = j;
                        cp1.RowIndex = i;
                        rp.cellPro.Add(cp1);
                        continue;
                    }


                    CellPro cp = new CellPro();
                    ///拿到单元格的值
                    cp.Value = GetCellValue(icell);
                    ///拿到数据格式名称
                    cp.FormatName = GetDataFormatString(icell);
                    ///这是拿到数据格式的数字
                    //cp.DataFormat = icell.CellStyle.DataFormat;

                    ///拿到单元格值类型
                    cp.CellType = GetCellType(icell);
                    ///拿到单元格所在的列位置
                    cp.ColsIndex = j;
                    ///拿到单元格所在的行位置
                    cp.RowIndex = i;
                    GetCellStyle(cp, icell, workbook);
                    rp.cellPro.Add(cp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("拿到所有单元格的数据", ex);
            }
        }


        /// <summary>
        /// 拿到单元格的值
        /// </summary>
        /// <returns></returns>
        private string GetCellValue(ICell icell)
        {


            if (icell.CellType == CellType.Blank)
            {
                return "";
            }

            try
            {
                if (icell.CellType == CellType.Numeric)
                {

                    string name = icell.CellStyle.GetDataFormatString();
                    if (name.EndsWith("@"))
                    {
                        DateTime dt = icell.DateCellValue;
                        return dt.ToString();

                    }

                    return icell.NumericCellValue.ToString();

                }
            }
            catch (Exception ex)
            {
                log.Error("拿单元格的日期类型出错了！", ex);
            }

            return icell.ToString();
        }
        /// <summary>
        /// 拿到单元格的类型
        /// </summary>
        /// <param name="icell">单元格对象</param>
        /// <returns>如果是日期类型就返回 Date ,如果是数字类型就返回 Double,否则就返回 String。</returns>
        private CellTypeName GetCellType(ICell icell)
        {


            if (icell.CellType == CellType.Blank)
            {
                return CellTypeName.Blank;
            }

            try
            {
                if (icell.CellType == CellType.Numeric)
                {

                    string name = icell.CellStyle.GetDataFormatString();
                    if (name.EndsWith("@"))
                    {
                        return CellTypeName.Date;
                    }



                    return CellTypeName.Double;
                }
            }
            catch (Exception ex)
            {
                log.Error("拿单元格的日期类型出错了！", ex);
            }

            return CellTypeName.String;
        }

        /// <summary>
        /// 拿到数据格式的名称，如果单元格的值为空就返回""
        /// </summary>
        /// <param name="icell">单元格对象</param>
        /// <returns>数据格式名称，如果单元格的值为空就返回""</returns>
        private string GetDataFormatString(ICell icell)
        {
            if (icell.CellType == CellType.Blank)
            {
                return "";
            }

            return icell.CellStyle.GetDataFormatString();

        }


        /// <summary>
        /// 拿到单元格的样式
        /// </summary>
        private void GetCellStyle(CellPro cp, ICell icell, IWorkbook workbook)
        {
            try
            {
                ICellStyle ics = icell.CellStyle;

                ///下边框
                cp.BorderBottomName = ics.BorderBottom;
                cp.BorderBottomColor = ics.BottomBorderColor;
                ///上边框
                cp.BorderTopName = ics.BorderTop;
                cp.BorderTopColor = ics.TopBorderColor;
                ///左边框
                cp.BorderLeftName = ics.BorderLeft;
                cp.BorderLeftColor = ics.LeftBorderColor;
                ///右边框
                cp.BorderRightName = ics.BorderRight;
                cp.BorderRightColor = ics.RightBorderColor;
                ///左右对齐方式
                cp.Alignment = ics.Alignment;
                ///垂直对齐方式
                cp.VerticalAlignment = ics.VerticalAlignment;

                ///填充背景色
                cp.FillBackgroundColor = ics.FillBackgroundColor;
                ///填充前景色
                cp.FillForegroundColor = ics.FillForegroundColor;

                ///填充模式
                cp.FillPattern = ics.FillPattern;

                ///是否缩放
                cp.ShrinkToFit = ics.ShrinkToFit;

                //是否换行
                cp.WrapText = ics.WrapText;


                IFont ifont = ics.GetFont(workbook);
                ///字体颜色
                cp.FontColor = ifont.Color;
                ///字体名字
                cp.FontName = ifont.FontName;
                ///字体大小
                cp.FontSize = ifont.FontHeightInPoints;
                ///字体粗细
                cp.FontBlod = ifont.Boldweight;
                ///斜体
                cp.IsItalic = ifont.IsItalic;

            }
            catch (Exception ex)
            {
                throw new Exception("拿单元格的属性发生错误！", ex);
            }

        }


        /// <summary>
        /// 拿到子表的开始位置和结束位置
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="index">行数，从0开始</param>
        /// <returns>第一列的值</returns>
        private int GetSubFirstAndLast(ISheet sheet, int index, IWorkbook workbook)
        {
            IRow row = sheet.GetRow(index);

            if (row == null)
            {
                return 0;
            }

            if (row.GetCell(0) == null)
            {
                return 0;
            }

            string value = GetCellValue(row.GetCell(0));


            if (int.TryParse(value, out index))
            {

                if (index == 0)
                {
                    return 0;
                }

                return index - 1;
            }

            return 0;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sp">数据内容</param>
        /// <param name="path">导出路径，绝对物理路径</param>
        public void WriteExcel(SheetPro sp, string path)
        {
            if (sp == null)
            {
                return;
            }

            try
            {


                GetDynHegiht(sp);


                IWorkbook workbook = new HSSFWorkbook();

                ///创建表名为keyBooks的表
                ISheet sheet = workbook.CreateSheet("sheet1");
                sp.subFirstRow = sp.Header.Count + 1;
                sp.subLastRow = sp.subFirstRow + sp.DataArea.Count - 1;
                SetRowPro(sp, sheet, workbook);
                /////合并单元格
                foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                {

                    sheet.AddMergedRegion(new CellRangeAddress(cp.SpanFirstRow, cp.SpanLastRow, cp.SpanFirstCell, cp.SpanLastCell));

                }
                ///设置整列的宽度
                for (int i = 0; i < sp.ColumnNum; i++)
                {
                    sheet.SetColumnWidth(i, sp.ColumnWidth[i]);
                }

                ///false ---- 选择缩放比例
                sheet.FitToPage = false;
                ///打印标题行
                //workbook.SetRepeatingRowsAndColumns(0,-1,-1, 0, sp.subFirstRow - 2);






                ///设置打印属性
                SetPrintPor(sheet, sp);
                ///设置页面边距
                SetSheetMargin(sheet, sp);

                #region  ///插入图片到Excel去

                for (int i = 0; i < sp.picturesList.Count; i++)
                {

                    HSSFPictureData hpd = sp.picturesList[i];
                    HSSFClientAnchor hca = sp.hcaList[i];
                    int pictureIdx = workbook.AddPicture(hpd.Data, NPOI.SS.UserModel.PictureType.JPEG);
                    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    HSSFClientAnchor anchor = new HSSFClientAnchor(hca.Dx1, hca.Dy1, hca.Dx2, hca.Dy2, hca.Col1, hca.Row1, hca.Col2, hca.Row2);
                    //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50
                    HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                    ///这是原图显示
                    // pict.Resize();

                }


                #endregion


                ///这是设置第二张工作表的数据的
                ISheet isheet1 = workbook.CreateSheet("sheet2");

                IRow ir = isheet1.CreateRow(0);
                ICell ic = ir.CreateCell(1);

                ic.SetCellValue("子表开始位置");
                ic = ir.CreateCell(0);
                ic.SetCellValue(sp.subFirstRow);

                ir = isheet1.CreateRow(1);
                ic = ir.CreateCell(1);
                ic.SetCellValue("子表结束位置");
                ic = ir.CreateCell(0);
                ic.SetCellValue(sp.subLastRow);


                ir = isheet1.CreateRow(3);
                ic = ir.CreateCell(1);
                ic.SetCellValue("页宽");
                ic = ir.CreateCell(0);
                ic.SetCellValue(sp.Width);

                ir = isheet1.CreateRow(4);
                ic = ir.CreateCell(1);
                ic.SetCellValue("页高");
                ic = ir.CreateCell(0);
                ic.SetCellValue(sp.Height);

                ///新建一个Excel文件
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                ///把数据流写进Excel中
                workbook.Write(file);

                file.Close();



            }
            catch (Exception ex)
            {
                throw new Exception("导出Excel文件出错了！", ex);
            }
        }


        /// <summary>
        /// 设置行属性
        /// </summary>
        private void SetRowPro(SheetPro sp, ISheet sheet, IWorkbook workbook)
        {
            try
            {

                sp.rowProList.Clear();
                sp.rowProList.AddRange(sp.Header);
                sp.rowProList.AddRange(sp.DataArea);
                sp.rowProList.AddRange(sp.Bottom);


                for (int i = 0; i < sp.rowProList.Count; i++)
                {
                    RowPro rp = sp.rowProList[i];

                    ///新建一行
                    IRow row = sheet.CreateRow(i);
                    ///行高
                    row.HeightInPoints = rp.RowHeight;
                    List<CellPro> cpList = rp.cellPro;

                    ///这是空行数据
                    if (cpList.Count == 0)
                    {
                        row.Height = 225;
                        for (int k = 0; k < sp.ColumnNum; k++)
                        {
                            ICell ic = row.CreateCell(k);
                            ic.SetCellValue("");
                        }

                        continue;

                    }

                    for (int j = 0; j < cpList.Count; j++)
                    {
                        CellPro cp = cpList[j];
                        ICell ic = row.CreateCell(cp.ColsIndex);
                        SetCellValue(ic, cp.CellType, cp.Value);
                        SetCellStyl(ic, cp, workbook);
                        IDataFormat df = workbook.CreateDataFormat();
                        if (!string.IsNullOrEmpty(cp.FormatName))
                        {
                            ///设置数据格式
                            ic.CellStyle.DataFormat = df.GetFormat(cp.FormatName);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("拿行数据出错了！", ex);
            }
        }

        /// <summary>
        /// 字体缓冲
        /// </summary>
        SortedDictionary<string, NPOI.HSSF.UserModel.HSSFFont> m_FontBuffer = new SortedDictionary<string, NPOI.HSSF.UserModel.HSSFFont>();

        /// <summary>
        /// 单元格样式缓冲
        /// </summary>
        SortedDictionary<string, ICellStyle> m_StyleBuffer = new SortedDictionary<string, ICellStyle>();


        /// <summary>
        /// 设置单元格的属性
        /// </summary>
        private void SetCellStyl(ICell ic, CellPro cp, IWorkbook workbook)
        {

            try
            {


                ICellStyle ics;

                string styleKey = string.Concat(
                    cp.BorderTopName, "|",
                    cp.BorderTopColor, "|",
                    cp.BorderBottomColor, "|",
                    cp.BorderBottomName, "|",
                    cp.BorderLeftColor, "|",
                    cp.BorderLeftName, "|",
                    cp.BorderRightColor, "|",
                    cp.BorderRightName, "|",
                    cp.Alignment, "|",
                    cp.VerticalAlignment, "|",
                    cp.WrapText, "|",
                    cp.ShrinkToFit, "|",
                    cp.IsItalic, "|",
                    cp.FontSize, "|",
                    cp.FillBackgroundColor, "|",
                    cp.FillForegroundColor, "|",
                    cp.ShrinkToFit, "|",
                    cp.FontBlod, "|",
                    cp.FontName, "|",
                    cp.FontColor);




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

                string fontKey = string.Concat(cp.FontName, "|", cp.FontSize, "|", cp.FontColor, "|", cp.FontBlod);

                NPOI.HSSF.UserModel.HSSFFont ifont;

                if (!m_FontBuffer.TryGetValue(fontKey, out ifont))
                {

                    ifont = workbook.CreateFont() as NPOI.HSSF.UserModel.HSSFFont;

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


                ic.CellStyle = ics;


            }
            catch (Exception ex)
            {
                throw new Exception("设置属性错误", ex);
            }

        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="ic">单元格对象</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="vlaue">值</param>
        private void SetCellValue(ICell ic, CellTypeName typeName, string vlaue)
        {
            if (typeName == CellTypeName.Date)
            {
                DateTime dt = DateTime.Parse(vlaue);
                ic.SetCellValue(dt);
            }
            else if (typeName == CellTypeName.Double)
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
        /// 把子表数据插入到指定位置
        /// </summary>
        /// <param name="sp">整个工作簿</param>
        /// <param name="m_dic">数据组</param>
        /// <param name="lm">主表数据</param>
        /// <returns></returns>
        public void InsertSubData(SheetPro sp, Dictionary<string, List<DataGroup>> m_dic, LModel lm)
        {


            if (sp == null && m_dic == null)
            {
                throw new Exception("数据不能为空！");
            }


            int num = 0;

            try
            {

                ///这是处理头部
                RowProCollection header = sp.Header;
                num = (sp.subLastRow - sp.subFirstRow) + 1;
                //sp.subLastRow = mf.Items.Count * num + sp.subLastRow - 1;
                GetDataBaseValue(header, lm);

        

                ///这是处理中间数据区
                RowProCollection dateArea = sp.DataArea;
                dateArea.SpanCellAndRow.cellPro = UpdateSubSpanCell(dateArea.SpanCellAndRow.cellPro,sp.subFirstRow,m_dic);
                ///更新模板数据
                UpdateSubData(m_dic, sp);
                ///把子表模板数据给移除掉
                dateArea.RemoveRange(0, num);

                ///这是处理尾部
                RowProCollection bottom = sp.Bottom;

                ///这是图片的位置变化
                foreach (HSSFClientAnchor hca in bottom.hcaList)
                {
                    hca.Row1 += dateArea.Count - num;
                    hca.Row2 += dateArea.Count - num;

                }
                ///这是合并单元格变化
                foreach (CellPro cp in bottom.SpanCellAndRow.cellPro)
         

                {
                    cp.SpanFirstRow += dateArea.Count - num ;
                    cp.SpanLastRow += dateArea.Count - num;
                }

                GetDataBaseValue(bottom, lm);




                sp.rowProList.Clear();
                sp.rowProList.AddRange(sp.Header);
                sp.rowProList.AddRange(sp.DataArea);
                sp.rowProList.AddRange(sp.Bottom);

                sp.SpanCellAndRow.cellPro.Clear();
                sp.SpanCellAndRow.cellPro.AddRange(sp.Header.SpanCellAndRow.cellPro);
                sp.SpanCellAndRow.cellPro.AddRange(sp.DataArea.SpanCellAndRow.cellPro);
                sp.SpanCellAndRow.cellPro.AddRange(sp.Bottom.SpanCellAndRow.cellPro);

                sp.picturesList.Clear();
                sp.picturesList.AddRange(sp.Header.picturesList);
                sp.picturesList.AddRange(sp.DataArea.picturesList);
                sp.picturesList.AddRange(sp.Bottom.picturesList);

        
                sp.hcaList.Clear();

                sp.hcaList.AddRange(sp.Header.hcaList);
                sp.hcaList.AddRange(sp.DataArea.hcaList);
                

                sp.hcaList.AddRange(sp.Bottom.hcaList);


            }
            catch (Exception ex)
            {
                log.Error("插入子表数据出错了！", ex);
              

                throw new Exception("插入子表数据出错了！", ex);
            }

        }

        /// <summary>
        /// 从数据集合中拿数据
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="mf">数据集合</param>
        private void GetDataBaseValue(RowProCollection item, LModel lm)
        {
            foreach 

            (RowPro rp in item)
            {
                foreach (CellPro cp in rp.cellPro)
                {
                    JTemplate jt = new JTemplate();

                    

                    jt.SrcText = cp.Value;
                    jt.Model = lm;
                    cp.Value = jt.Exec();
                }

            }
        }


        private void UpdateSubData_CellPro(LModel item, RowPro rp1,int rowIndex)
        {
            LModelElement modelElem = item.GetModelElement();

            LModelFieldElement fieldElem;



            foreach (CellPro cp in rp1.cellPro)
            {
                bool isNum = false;

                ///这是判断是否为数字类型
                if(cp.Value.StartsWith("{") && !cp.Value.Contains("-"))
                {
                    string vl = cp.Value.Substring(4, cp.Value.Length - 5);



                    if (modelElem.TryGetField(vl, out fieldElem))
                    {
                        object value = item[vl];

                        if (fieldElem.IsNumber && value != null)
                        {
                            decimal cc = Convert.ToDecimal(value);
                            cp.Value = cc.ToString();


                            cp.CellType = CellTypeName.Double;
                        }

                    }
                }//用来自定义参数类型的{$P.}
                else if (cp.Value.StartsWith("{$P."))
                {
                    string paramName = cp.Value.Substring(4, cp.Value.Length - 5);

                    if ("ROW_INDEX".Equals(paramName, StringComparison.OrdinalIgnoreCase))
                    {
                        cp.Value = rowIndex.ToString();

                        cp.CellType = CellTypeName.Double;
                    }


                }



                try
                {


                    if (!isNum)
                    {
                        JTemplate jt = new JTemplate();

                        jt.SrcText = cp.Value;


                        jt.Model = item;
                        cp.Value = jt.Exec();
                    }
                }
                catch (Exception ex)
                {


                    throw new Exception(string.Format("字段“{0}”异常！", cp.Value), ex);
                }
            }
        }

        private void UpdateSubData_Model(RowProCollection sp, List<RowPro> rpList, LModel item,int i)
        {
            foreach (RowPro rp1 in rpList)
            {
                UpdateSubData_CellPro(item, rp1,i);

                sp.Add(rp1);
            }

        }

        /// <summary>
        /// 更新模板数据
        /// </summary>
        /// <param name="mf">数据集合</param>
        /// <param name="sp">模板数据</param>
        private void UpdateSubData(Dictionary<string, List<DataGroup>> m_dic, SheetPro sp)
        {



            foreach (var item in m_dic)
            {
                if (item.Value == null || item.Value.Count == 0) { continue; }

                for (int i = 0; i < item.Value.Count; i++)
                {


                    List<RowPro> rpList = CopyData(sp.DataArea, i * 2, i * 2 + 1);
                    sp.DataArea.AddRange(rpList);


                    DataGroup dg = item.Value[i];
                    if (dg == null) { continue; }

                    //序号用的
                    int j = 0;

                    foreach (var lm in dg)
                    {
                        rpList = CopyData(sp.DataArea, i * 2 + 1, i * 2 + 2);

                        UpdateSubData_Model(sp.DataArea, rpList, lm,++j);


                    }


                }

            }



        }


        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <returns></returns>
        private List<RowPro> CopyData(RowProCollection item, int begin, int end)
        {
            try
            {

                List<RowPro> rpList = new List<RowPro>();

                for (int i = begin; i < end; i++)
                {
                    RowPro rp = item[i];

                    RowPro rp1 = new RowPro();

                    foreach (CellPro cp in rp.cellPro)
                    {

                    
                        rp1.cellPro.Add(cp.Clone());

   

                 }

                    rp1.RowHeight = rp.RowHeight;

                    rpList.Add(rp1);

                }

                return rpList;

            }
  

          catch (Exception ex)
            {
                throw new Exception("这是拷贝数据发生错误！", ex);
            }

        }

        /// <summary>
        /// 更新数据部合并集合
        /// </summary>
        /// <param name="cpSubData">数据部合并集合</param>
        /// <param name="subFirstRow">子表数据开始位置</param>
        /// <param name="m_dic">数据集</param>
        private CellProCollection UpdateSubSpanCell(CellProCollection cpSubData,int subFirstRow,Dictionary<string,List<DataGroup>> m_dic)
        {
            CellProCollection newCpList = new CellProCollection();
            try
            {
         

       
                ///这是索引
                int num = subFirstRow;

                foreach (var item in m_dic)
                {

                    if (item.Value == null || item.Value.Count == 0) { continue; }
                    for (int i = 0; i < item.Value.Count; i++)
                    {

                        List<CellPro> newSpanRow = new List<CellPro>();
                        ///拿到数据标题合并区
                        var spanRow = cpSubData.FindAll(c => c.SpanFirstRow == subFirstRow  + i * 2);

                        foreach (var sr in spanRow) 
                        {
                            CellPro cp = new CellPro();
                            cp.SpanFirstRow = num;
                            cp.SpanFirstCell = sr.SpanFirstCell;
                            cp.SpanLastCell = sr.SpanLastCell;
                            cp.SpanLastRow = num + (sr.SpanLastRow - sr.SpanFirstRow);

                            newSpanRow.Add(cp);
                            
                            
                        }

                        newCpList.AddRange(newSpanRow);

                        num++;

                        DataGroup dg = item.Value[i];

                        if (dg == null)
                        {
                            continue;
     

                   }
                       
                        for (int j = 0; j < dg.Count; j++)
                        {
                            newSpanRow = new List<CellPro>();
                            ///拿到数据合并区
                           var spanRow1 = cpSubData.FindAll(c => c.SpanFirstRow == subFirstRow + 1 + i * 2);

                            foreach (var sr in spanRow1) 
                            {
                                CellPro cp = new CellPro();
         

                                cp.SpanFirstRow = num;
                                cp.SpanFirstCell = sr.SpanFirstCell;
                                cp.SpanLastCell = sr.SpanLastCell;
                                cp.SpanLastRow = num + (sr.SpanLastRow - sr.SpanFirstRow);

                                newSpanRow.Add(cp);
        

                    }

                            newCpList.AddRange(newSpanRow);

                            num++;
                        }

                    }

     

           }



            }
            catch (Exception ex)
            {
                throw new Exception("更新数据部合并集合", ex);
            }

            return newCpList;

        }


        /// <summary>
        /// 这是系统字体缓冲
        /// </summary>
        SortedDictionary<string, Font> m_font = new SortedDictionary<string, Font>();


        /// <summary>
        /// 获取自动换行列的动态高度
        /// </summary>
        private void GetDynHegiht(SheetPro sheet)
        {

            float x34 = 3.0f / 4.0f;

            //获取自动换行的字体高度
            foreach (var row in sheet.rowProList)
            {
                foreach (var cell in row.cellPro)
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
                    int CellWidth = 0;

                    if (cell.SpanWidth == 0)
                    {
                        CellWidth = (int)(cell.Width / x34) - 8;
                    }
                    else
                    {
                        CellWidth = (int)(cell.SpanWidth / x34) - 8;
                    }



                    SizeF sf = m_gh.MeasureString(cell.Value, myF, CellWidth);

                    cell.DynHeight = sf.Height;

                }
            }




            //设置行高为最高的列高度
            foreach (var row in sheet.rowProList)
            {
                float height = 0;

                foreach (var cell in row.cellPro)
                {
                    height = Math.Max(height, cell.DynHeight);

                }

                if (height == 0) { continue; }

                //换算单位 从像素换到磅
                float sum = height * x34;

                if (sum < row.RowHeight) { continue; }

                row.RowHeight = height;

            }


        }



        public void Dispose()
        {
            if (m_gh != null)
            {
                m_gh.Dispose();
            }

            if (m_TmpBmp != null)
            {
                m_TmpBmp.Dispose();
            }

            foreach (var item in m_font.Values)
            {
                item.Dispose();
            }

            m_font.Clear();
            m_font = null;

            GC.SuppressFinalize(this);
        }


    }
}
