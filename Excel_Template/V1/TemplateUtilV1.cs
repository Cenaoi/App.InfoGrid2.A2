using App.InfoGrid2.Excel_Template.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 模板助手
    /// </summary>
    public class TemplateUtilV1
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        ///  创建模板  没有主表对象就生成单表模板
        /// </summary>
        /// <param name="path">生成模板路径</param>
        /// <param name="tsItem">子表信息</param>
        /// <param name="tsHead">主表信息 主表默认为空，就生成单表模板</param>
        public static void CreateSingleTableTemp(string path, TableSet tsItem, TableSet tsHead = null)
        {

            NOPIUtil nopi = new NOPIUtil();

            nopi.CreateSingleTableTemp(path, tsItem, tsHead);

        }


        /// <summary>
        /// 读取模板的数据
        /// </summary>
        /// <param name="sheet_param">sheet参数对象</param>
        /// <param name="temp_path">模板绝对路径</param>
        public static SheetParam ReadTemp(string temp_path)
        {


            //判断文件是否存在
            if (!File.Exists(temp_path))
            {
                throw new Exception("模板文件不存在！");
            }

            NOPIUtil nopi = new NOPIUtil();

            try
            {

                IWorkbook workbook;

                using (FileStream fs = new FileStream(temp_path, FileMode.Open))
                {
                    //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                    workbook = new HSSFWorkbook(fs);

                }

                //获取excel的第一个sheet
                HSSFSheet sheet = workbook.GetSheet("sheet1") as HSSFSheet;

                SheetParam sp = new SheetParam();

                try
                {
                    //获取第二张表的信息
                    HSSFSheet sheet2 = (HSSFSheet)workbook.GetSheet("sheet2");

                    TempParam tp = nopi.GetSheet2(sheet2);

                    sp.TempParam = tp;

                }
                catch (Exception ex)
                {
                    log.Error("获取第二页自定义参数出错了！", ex);
                    throw ex;
                }



                TempSheet ts = new TempSheet();

                sp.TempSheet = ts;

                //获取图片数据
                nopi.GetImg(ts, workbook, sheet);
                //获取每一列的宽度
                nopi.GetColumnWidth(ts, sheet);
                //获取合并单元格信息
                nopi.GetSpanCellAndRow(ts, sheet);

                //拿打印属性
                nopi.GetPrintPro(sheet, ts.pp);

                //拿页面边距

                nopi.GetSheetMarginPro(sheet, ts.SheetMargin);

                //获取所有行数据
                ReadTemp_GetRows(sheet, workbook, sp.Rows, ts.ColumnNum, nopi);


                //获取单元格的宽和高 和 X 和 Y
                nopi.GetCellWidthAndHeight(ts, sp.Rows);

                //获取头部数据 根据第二页自定义数据区开始行
                sp.HeaderTemp = ReadTemp_GetHeader(sp.Rows, ts, sp.TempParam.FirstRowIndex);

                //拿到中间数据区部分 根据第二页自定义数据区开始和结束行
                sp.DataAreaTemp = ReadTemp_GetDataArea(sp.Rows, ts, sp.TempParam.FirstRowIndex, sp.TempParam.LastRowIndex);

                //获取小计行模板
                sp.TotalTemp = ReadTemp_GetTotalTemp(sp.Rows, ts, sp.TempParam);


                //拿到底部 根据第二页自定义数据区结束行
                sp.FooterTemp = ReadTemp_GetFooter(sp.Rows, ts,sp.TempParam);

                //计算三块区域的高度 包括外边距
                ReadTemp_GetHeight(sp);

                return sp;

            }
            catch (Exception ex)
            {
                log.Error("读取Excel模板出错了！", ex);

                throw new Exception("读取Excel模板出错！", ex);

            }

        }


        /// <summary>
        /// 拿到所有行的数据
        /// </summary>
        static void ReadTemp_GetRows(ISheet sheet, IWorkbook workbook, RowProCollection rows, int column_num, NOPIUtil nopi)
        {
            try
            {

                //最后一列的标号  即总的行数
                int rowCount = sheet.LastRowNum;

                for (int i = 0; i <= rowCount; i++)
                {

                    //拿到每一行的对象
                    IRow row = sheet.GetRow(i);
                    //判断行是否有数据
                    if (row == null)
                    {
                        RowPro rp1 = new RowPro();
                        rows.Add(rp1);

                        continue;
                    }

                    RowPro rp = new RowPro();

                    rp.RowHeight = row.HeightInPoints;
                    rp.RowIndex = i;


                    ReadTemp_GetRows_GetCells(row, i, workbook, rp, column_num, nopi);

                    rows.Add(rp);
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
        static void ReadTemp_GetRows_GetCells(IRow row, int i, IWorkbook workbook, RowPro rp, int columnNum, NOPIUtil nopi)
        {
            try
            {
                for (int j = 0; j < columnNum; j++)
                {
                    //拿到每一个单元格的对象
                    HSSFCell icell = row.GetCell(j) as HSSFCell;


                    //判断单元格是否有数据
                    if (icell == null)
                    {
                        CellPro cp1 = new CellPro();
                        cp1.Value = "";
                        cp1.FontName = "新宋体";
                        cp1.FontSize = 12;
                        cp1.ColsIndex = j;
                        cp1.RowIndex = i;
                        rp.Add(cp1);
                        continue;
                    }


                    CellPro cp = new CellPro();
                    //拿到单元格的值
                    cp.Value = nopi.GetCellValue(icell);
                    //拿到数据格式名称
                    cp.FormatName = nopi.GetDataFormatString(icell);
                    //这是拿到数据格式的数字
                    //cp.DataFormat = icell.CellStyle.DataFormat;

                    //拿到单元格值类型
                    cp.CellType = nopi.GetCellType(icell);
                    //拿到单元格所在的列位置
                    cp.ColsIndex = j;
                    //拿到单元格所在的行位置
                    cp.RowIndex = i;

                    //获取单元格样式
                    nopi.GetCellStyle(cp, icell, workbook);

                    rp.Add(cp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("拿到所有单元格的数据", ex);
            }
        }


        /// <summary>
        /// 获取头部数据 根据第二页自定义数据区开始行
        /// </summary>
        /// <param name="rows">所有行集合</param>
        /// <param name="ts">模板第一页属性对象</param>
        /// <param name="first_row_index">数据区开始行</param>
        /// <returns></returns>
        static RowProCollection ReadTemp_GetHeader(RowProCollection rows, TempSheet ts, int first_row_index)
        {


            RowProCollection spTitel = new RowProCollection();

            if (first_row_index == 0)
            {
                return spTitel;
            }

            for (int i = 0; i < first_row_index; i++)
            {
                spTitel.Add(rows[i]);
            }

            foreach (CellPro cp in ts.SpanCellAndRow)
            {
                if (cp.SpanLastRow >= first_row_index) { continue; }

                spTitel.SpanCellAndRow.Add(cp);

            }


            for (int i = 0; i < ts.Hcas.Count; i++)
            {
                HSSFClientAnchor hca = ts.Hcas[i];
                HSSFPictureData hpd = ts.Pictures[i];
                if (hca.Row1 >= first_row_index) { continue; }

                spTitel.hcaList.Add(hca);
                spTitel.picturesList.Add(hpd);

            }

            spTitel.ProSize();

            return spTitel;

        }

        /// <summary>
        /// 拿到中间数据区部分  根据第二页自定义数据区开始和结束行
        /// </summary>
        /// <param name="rows">所有行集合</param>
        /// <param name="ts">模板第一页属性对象</param>
        /// <param name="first_row_index">数据区开始行</param>
        /// <param name="last_row_index">数据区结束行</param>
        /// <returns></returns>
        static RowProCollection ReadTemp_GetDataArea(RowProCollection rows, TempSheet ts, int first_row_index, int last_row_index)
        {
            RowProCollection spData = new RowProCollection();

            if (first_row_index == 0 && last_row_index == 0)
            {
                spData.AddRange(rows);
                return spData;
            }

            for (int i = first_row_index; i <= last_row_index; i++)
            {
                spData.Add(rows[i]);
            }

            foreach (CellPro cp in ts.SpanCellAndRow)
            {
                if (cp.SpanFirstRow < first_row_index || cp.SpanFirstRow > last_row_index) { continue; }

                spData.SpanCellAndRow.Add(cp);


            }

            for (int i = 0; i < ts.Hcas.Count; i++)
            {
                HSSFClientAnchor hca = ts.Hcas[i];
                HSSFPictureData hpd = ts.Pictures[i];
                if (hca.Row1 < first_row_index || hca.Row2 > last_row_index) { continue; }

                spData.hcaList.Add(hca);
                spData.picturesList.Add(hpd);

            }


            spData.ProSize();

            return spData;


        }

        /// <summary>
        /// 拿到底部 根据第二页自定义数据区结束行
        /// </summary>
        /// <param name="rows">所有行集合</param>
        /// <param name="ts">模板第一页属性对象</param>
        /// <param name="tp">模板第二页属性对象</param>
        /// <returns></returns>
        static RowProCollection ReadTemp_GetFooter(RowProCollection rows, TempSheet ts,TempParam tp)
        {
            RowProCollection spBot = new RowProCollection();

            if (tp.LastRowIndex == 0)
            {
                return spBot;
            }


            int last_row_index_v1 = tp.LastRowIndex;

            //如果有合计，就再加一行索引
            if (tp.IsTotal)
            {
                last_row_index_v1++;
            }



            for (int i = last_row_index_v1 + 1; i < rows.Count; i++)
            {
                spBot.Add(rows[i]);
            }

            foreach (CellPro cp in ts.SpanCellAndRow)
            {
                if (cp.SpanFirstRow <= last_row_index_v1) { continue; }
                spBot.SpanCellAndRow.Add(cp);

            }

            for (int i = 0; i < ts.Hcas.Count; i++)
            {
                HSSFClientAnchor hca = ts.Hcas[i];
                HSSFPictureData hpd = ts.Pictures[i];
                if (hca.Row1 <= last_row_index_v1) { continue; }

                spBot.hcaList.Add(hca);
                spBot.picturesList.Add(hpd);

            }

            spBot.ProSize();

            return spBot;
        }


        /// <summary>
        /// 拿到小计模板 根据第二页自定义数据区结束行
        /// </summary>
        /// <param name="rows">所有行集合</param>
        /// <param name="ts">模板第一页属性对象</param>
        /// <param name="tp">模板第二页属性对象</param>
        /// <returns></returns>
        static RowPro ReadTemp_GetTotalTemp(RowProCollection rows, TempSheet ts, TempParam tp)
        {
            RowPro spBot = new RowPro();

            //没有合计就不用拿模板了
            if (!tp.IsTotal)
            {
                return spBot;
            }


            if (tp.LastRowIndex == 0)
            {
                return spBot;
            }

            
            //如果有合计功能，最后一行索引就加一
            int last_row_index_v1 = tp.LastRowIndex + 1;

            spBot = rows[last_row_index_v1];


            foreach (CellPro cp in ts.SpanCellAndRow)
            {
                if (cp.SpanFirstRow < last_row_index_v1 || cp.SpanFirstRow > last_row_index_v1) { continue; }

                spBot.Add(cp);

            }


            return spBot;


        }


        /// <summary>
        /// 计算三块区域的高度 包括外边距  
        /// </summary>
        /// <param name="sp">sheet参数</param>
        static void ReadTemp_GetHeight(SheetParam sp)
        {

            TempSheet ts = sp.TempSheet;

            TempParam tp = sp.TempParam;


            if (tp.Height != 0 && tp.Width != 0)
            {
                ts.PaperSize = new System.Drawing.Printing.PaperSize("自定义纸张大小",(int) tp.PageWidth,(int) tp.PageHeight);
            }
            else
            {


                PrintDocument printDoc = new PrintDocument();

                PageSettings ps = printDoc.DefaultPageSettings;

                foreach (System.Drawing.Printing.PaperSize item in ps.PrinterSettings.PaperSizes)
                {

                    if (item.RawKind == ts.pp.PaperSize)
                    {
                        ts.PaperSize = item;
                    }
                }

                //都找不到就默认一个A4纸张
                if (ts.PaperSize == null)
                {

                    ts.PaperSize = new System.Drawing.Printing.PaperSize("A4", 210, 297);

                }

            }



            //页脚边距  这是转成 磅单位
            float bottomMargin = (float)(ts.SheetMargin.BottomMargin * 10 * 2.83);
            //页眉边距 这是转成磅 单位
            float topMargin = (float)(ts.SheetMargin.TopMargin * 10 * 2.83);


            ts.HeaderHeight = sp.HeaderTemp.Height + topMargin;

            ts.FooterHeight = sp.FooterTemp.Height + bottomMargin;

            //这是中间剩多少高度
            ts.ContentHeight = ts.PaperSize.Height * 0.72f - ts.HeaderHeight - ts.FooterHeight;
            //纸张大小 - 页脚边距 - 页眉边距 - 头部高度 - 底部高度  == 中间数据区高度

        }

        /// <summary>
        /// 创建Excle文件
        /// </summary>
        /// <param name="sp">>整个模板sheet对象</param>
        /// <param name="ds">数据集</param>
        /// <param name="file_path">Excel文件路径</param>
        public static void CreateExcel(SheetParam sp, DataSet ds, string file_path)
        {

            //这是处理那种列重复的
            if (sp.TempParam.DataAreaType == DataAreaType.COLUMN_REPEAT)
            {

                ColumnRepeatTemp crt = new ColumnRepeatTemp();

                crt.CreateExcel(sp, ds, file_path);

            }
            //正常模板
            if (sp.TempParam.DataAreaType == DataAreaType.NONE)
            {

                NoneTemp nt = new NoneTemp();
                nt.CreateExcel(sp, ds, file_path);

            }

            //这是行合并有图片的，就是某一列的值相同行，都要合并在一起
            if (sp.TempParam.DataAreaType == DataAreaType.ROW_MERGE_IMG)
            {

                RowMergeImgTemp rmit = new RowMergeImgTemp();
                rmit.CreateExcel(sp, ds, file_path);

            }

        }

        /// <summary>
        /// 创建多子表Excel文件
        /// </summary>
        /// <param name="sp">>整个模板sheet对象</param>
        /// <param name="ds">多子表数据集</param>
        /// <param name="file_path">Excel文件路径</param>
        public static void CreateExcel(SheetParam sp, MoreSubTableDataSet ds, string file_path)
        {


            //这是一个主表和子表集合的对象
            DataSet dsV1 = new DataSet();
            dsV1.Head = ds.Head;
            dsV1.Items = ds.OneItems;

            //多子表模板
            if (sp.TempParam.DataAreaType == DataAreaType.MORE_SUB_TABLE)
            {

                MoreSubTableTemp mstt = new MoreSubTableTemp();
                mstt.CreateExcel(sp, ds, file_path);

            }


            //这是处理那种列重复的
            if (sp.TempParam.DataAreaType == DataAreaType.COLUMN_REPEAT)
            {

                ColumnRepeatTemp crt = new ColumnRepeatTemp();

                crt.CreateExcel(sp, dsV1, file_path);

            }
            //正常模板
            if (sp.TempParam.DataAreaType == DataAreaType.NONE)
            {

                NoneTemp nt = new NoneTemp();
                nt.CreateExcel(sp, dsV1, file_path);

            }

            //这是行合并有图片的，就是某一列的值相同行，都要合并在一起
            if (sp.TempParam.DataAreaType == DataAreaType.ROW_MERGE_IMG)
            {

                RowMergeImgTemp rmit = new RowMergeImgTemp();
                rmit.CreateExcel(sp, dsV1, file_path);

            }

        }
    }
}
