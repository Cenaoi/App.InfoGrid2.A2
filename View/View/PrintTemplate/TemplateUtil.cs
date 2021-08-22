using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Decipher.LightDecipher;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 打印模板助手类
    /// </summary>
    public class TemplateUtil
    {
        /// <summary>
        /// 行高
        /// </summary>
        static short m_RowHegiht = 500;

        /// <summary>
        /// 列宽
        /// </summary>
        static int m_CellWidth = 5000;


        /// <summary>
        /// 生成 上下表结构的 默认模板
        /// </summary>
        /// <param name="m_mainTableID">主表ID</param>
        /// <param name="m_subTableID">子表ID</param>
        /// <param name="m_pageID">页面ID</param>
        /// <param name="m_pageText">页面名称</param>
        /// <param name="m_subTable">子表名称</param>
        /// <param name="m_mainTabel">主表名称</param>
        /// <param name="m_fFiled">子表中存放主表ID的字段</param>
        public static void CreateDefaultTemplateByOneTable(
            int m_mainTableID,
            int m_subTableID,
            int m_pageID,
            string m_pageText,
            string m_subTable,
            string m_mainTabel,
            string m_fFiled)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            
            ///主表列信息
            TableSet ts = TableSet.SelectSID_0_5(decipher, m_mainTableID);
                        
            ///子表列信息
            TableSet tsSub = TableSet.SelectSID_0_5(decipher, m_subTableID);

            BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
            {
                MAIN_TABLE_ID = m_mainTableID,
                PAGE_ID = m_pageID,
                PAGE_TEXT = m_pageText,
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = 0,
                SUB_TABLE_NAME = m_subTable,
                MAIN_TABLE_NAME = m_mainTabel,
                TABLE_NUMBER = 1,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = m_fFiled,
                TEMPLATE_NAME = "默认模板"
            };

            string name = string.Format(
                "{0}_{1}_{2}_{3}_{4}.xls",
                m_pageText,
                m_pageID,
                bpt.BIZ_PRINT_TEMPLATE_ID,
                m_mainTabel,
                m_subTable
                );

            string path = HttpContext.Current.Server.MapPath("/PrintTemplate");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                WriteExcel(path + "\\" + name, tsSub,ts);

                bpt.TEMPLATE_URL = "/PrintTemplate/" + name;

                decipher.InsertModel(bpt);



            }
            catch (Exception ex)
            {
                throw new Exception("生成默认模板时出错了！", ex);
            }

        }


        /// <summary>
        /// 生成 单表 结构 的 默认模板
        /// </summary>
        /// <param name="m_mainTableID">主表ID</param>
        /// <param name="m_pageID">页面ID</param>
        /// <param name="m_mainTable">主表名称</param>
        public static void CreateDefaultTemplateBySingleTable(int m_mainTableID,int m_pageID,string m_mainTable)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();
            
            //主表列信息
            TableSet ts = TableSet.SelectSID_0_5(decipher, m_pageID);

            


            BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
            {
                MAIN_TABLE_ID = m_mainTableID,
                PAGE_ID = m_pageID,
                PAGE_TEXT = ts.Table.TABLE_NAME,
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = 0,
                SUB_TABLE_NAME = "子表名称",
                MAIN_TABLE_NAME = m_mainTable,
                TABLE_NUMBER = 1,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = "ROW_IDENTITY_ID",
                TEMPLATE_NAME = "默认模板"
            };


            string name = string.Format(
                    "{0}_{1}_{2}_{3}_{4}.xls",
                    ts.Table.TABLE_NAME,
                    m_pageID,
                    bpt.BIZ_PRINT_TEMPLATE_ID,
                    "主表名称",
                    "子表名称"
                    );

            string path = HttpContext.Current.Server.MapPath("/PrintTemplate");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try {


                WriteExcel(path + "\\" + name, ts);

                bpt.TEMPLATE_URL = "/PrintTemplate/" + name;

                decipher.InsertModel(bpt);

            }catch(Exception ex)
            {
                throw new Exception("生成单表默认模板出错了！",ex);
            }
        }



        /// <summary>
        /// 生成Excel模板  上下表结构的
        /// </summary>
        /// <param name="path">生成模板路径</param>
        /// <param name="tsSubItem">子表信息</param>
        /// <param name="tsItem">主表信息</param>
        static void WriteExcel(string path, TableSet tsSubItem,TableSet tsItem = null)
        {

            int visibleSubCount = 0;   //显示的列

            foreach (IG2_TABLE_COL tCol in tsSubItem.Cols)
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

                if (tsItem != null)
                {

                    isheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, visibleSubCount - 1));
                    isheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, visibleSubCount - 1));

                }

                ICellStyle ics = workbook.CreateCellStyle();



                IFont ifont = workbook.CreateFont();

                isheet.PrintSetup.Scale = 100;
                isheet.PrintSetup.PaperSize = (short)PaperSize.A4;
                isheet.PrintSetup.UsePage = true;

                isheet.PrintSetup.FitHeight = 0;
                isheet.PrintSetup.FitWidth = 1;
                isheet.FitToPage = false;


                if (tsItem != null)
                {

                    SetExcelHead(ics, isheet, ifont);
                }


                SetExcelData(tsItem, tsSubItem, isheet, workbook);



                ///设置整列的宽度
                int i = 0;
                foreach (IG2_TABLE_COL tCol in tsSubItem.Cols)
                {

                    isheet.SetColumnWidth(i++, m_CellWidth);
                }


                ///新建一个Excel文件
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                ///把数据流写进Excel中
                workbook.Write(file);

                file.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("生成Excel模板出错！", ex);
            }


        }


        /// <summary>
        /// 设置Excel文件的头部
        /// </summary>
        /// <param name="ics">单元格样式</param>
        /// <param name="isheet">工作表1</param>
        /// <param name="ifont">字体设置</param>
        static void SetExcelHead(ICellStyle ics, ISheet isheet, IFont ifont)
        {

            ///左右居中
            ics.Alignment = HorizontalAlignment.CenterSelection;
            ///垂直居中
            ics.VerticalAlignment = VerticalAlignment.Center;
            ///字体大小
            ifont.FontHeightInPoints = 25;
            ///字体名称
            ifont.FontName = "宋体";

            ics.SetFont(ifont);


            ///第一行
            IRow ir = isheet.CreateRow(0);
            ///行高
            ir.Height = m_RowHegiht;

            ///第一行第一个单元格
            ICell ic = ir.CreateCell(0);
            ic.SetCellValue("公司名称");
            ic.CellStyle = ics;


        }

        /// <summary>
        /// 设置Excel数据
        /// </summary>
        /// <param name="tsItem">主表信息</param>
        /// <param name="tsSubItem">子表信息</param>
        /// <param name="isheet">工作薄</param>
        /// <param name="workbook">Excel文件</param>
        static void SetExcelData(TableSet tsItem, TableSet tsSubItem, ISheet isheet, IWorkbook workbook)
        {
            ///代表行数
            int i = 0;

            if (tsItem != null)
            {
                SetExcelItem(isheet, tsItem, out i, workbook);
            }

            ///这是设置第二张工作表的数据的
            ISheet isheet1 = workbook.CreateSheet("sheet2");

            IRow ir = isheet1.CreateRow(0);
            ICell ic = ir.CreateCell(1);

            ic.SetCellValue("子表开始位置");
            ic = ir.CreateCell(0);
            ic.SetCellValue(i + 2);

            ir = isheet1.CreateRow(1);
            ic = ir.CreateCell(1);
            ic.SetCellValue("子表结束位置");
            ic = ir.CreateCell(0);
            ic.SetCellValue(i + 2);

            ir = isheet1.CreateRow(3);
            ic = ir.CreateCell(1);
            ic.SetCellValue("页宽");

            ir = isheet1.CreateRow(4);
            ic = ir.CreateCell(1);
            ic.SetCellValue("页高");


            ir = isheet1.CreateRow(6);
            ic = ir.CreateCell(1);
            ic.SetCellValue("合计");




            if (tsSubItem != null)
            {
                SetExcelSubItem(isheet, tsSubItem, i, workbook);
            }


        }

        /// <summary>
        /// 把主表信息写入到Excel中去
        /// </summary>
        /// <param name="isheet">工作薄</param>
        /// <param name="tsItem">主表信息</param>
        /// <param name="i">行数</param>
        static void SetExcelItem(ISheet isheet, TableSet tsItem, out int i, IWorkbook workbook)
        {
            ///从第二行开始写入主表数据
            i = 1;

            ICellStyle ics = workbook.CreateCellStyle();
            ///左右居中
            ics.Alignment = HorizontalAlignment.CenterSelection;
            ///垂直居中
            ics.VerticalAlignment = VerticalAlignment.Center;
            IFont ifont = workbook.CreateFont();
            ///字体大小
            ifont.FontHeightInPoints = 25;
            ifont.FontName = "宋体";

            ics.SetFont(ifont);

            ///表名
            IRow ir = isheet.CreateRow(i);
            ///行高
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

                ///一行放两列
                if (j % 2 == 0)
                {
                    ///主表字段行
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
        static void SetExcelSubItem(ISheet isheet, TableSet tsSubItem, int i, IWorkbook workbook)
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

            ///字体大小
            ifont.FontHeightInPoints = 12;
            ifont.FontName = "新宋体";
            ///粗体
            ifont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            ///左右居中
            ics.Alignment = HorizontalAlignment.Center;
            ///顺直居中
            ics.VerticalAlignment = VerticalAlignment.Center;
            ///下边框
            ics.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            ///左边框
            ics.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            ///右边框
            ics.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            ///上边框
            ics.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;

            ics.SetFont(ifont);



            ICellStyle ics1 = workbook.CreateCellStyle();
            IFont ifont1 = workbook.CreateFont();

            ///字体大小
            ifont1.FontHeightInPoints = 16;
            ///字体名称
            ifont1.FontName = "宋体";

            ///左对齐
            ics1.Alignment = HorizontalAlignment.Left;
            ///垂直居中
            ics1.VerticalAlignment = VerticalAlignment.Center;
            ///下边框
            ics1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            ///左边框
            ics1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            ///右边框
            ics1.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            ///上边框
            ics1.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;

            ics1.SetFont(ifont1);



            ///行高
            ir.Height = m_RowHegiht;
            ///行高
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



    }
}