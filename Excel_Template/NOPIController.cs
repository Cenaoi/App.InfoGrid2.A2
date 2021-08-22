using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace App.InfoGrid2.Excel_Template
{


    /// <summary>
    /// 这个是有合计，小计，啥的，反正这个是最后用的
    /// </summary>
    public class NOPIController
    {

        ///// <summary>
        ///// 读取模板数据
        ///// </summary>
        ///// <param name="path">模板文件的物理路径</param>
        ///// <returns>模板数据</returns>
        //public SheetPro ReadOneSheet(string path)
        //{

        //    if (File.Exists(path) == false)
        //    {
        //        throw new Exception("模板路径不存在！");
        //    }

           


        //    SheetPro sp = new SheetPro();

        //    IWorkbook workbook;

        //    using (FileStream fs = new FileStream(path, FileMode.Open))
        //    {

        //        //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
        //        workbook = new HSSFWorkbook(fs);

        //    }



        //    #region ///读取图片



        //    ///测试读取图片的
        //    IList pictures = workbook.GetAllPictures();
        //    foreach (HSSFPictureData pid in pictures)
        //    {
        //        ///拿到图片集合
        //        sp.picturesList.Add(pid);
        //    }

        //    //获取excel的第一个sheet
        //    NPOI.HSSF.UserModel.HSSFSheet sheet = workbook.GetSheet(m_TemplateShare) as NPOI.HSSF.UserModel.HSSFSheet;



        //    HSSFPatriarch hp = (HSSFPatriarch)sheet.DrawingPatriarch;

        //    if (hp != null)
        //    {
        //        foreach (HSSFPicture pic in hp.Children)
        //        {
        //            HSSFClientAnchor hca = (HSSFClientAnchor)pic.Anchor;
        //            sp.hcaList.Add(hca);
        //        }
        //    }



        //    #endregion
        //    GetCellNum(sheet, sp);
        //    GetRoowAllData(sheet, workbook, sp);

        //    ///这是要计算工作表里面的所有计算公式
        //    //sheet.ForceFormulaRecalculation = true;

        //    int regNum = sheet.NumMergedRegions;

        //    RowPro rp = new RowPro();

        //    for (int i = 0; i < regNum; i++)
        //    {
        //        CellPro cp = new CellPro();
        //        CellRangeAddress rAdd = sheet.GetMergedRegion(i);
        //        cp.SpanFirstCell = rAdd.FirstColumn;
        //        cp.SpanLastCell = rAdd.LastColumn;
        //        cp.SpanFirstRow = rAdd.FirstRow;
        //        cp.SpanLastRow = rAdd.LastRow;

        //        rp.cellPro.Add(cp);

        //    }
        //    sp.SpanCellAndRow = rp;


        //    ///拿打印属性
        //    GetPrintPor(sheet, sp);


        //    ///拿页面边距
        //    GetSheetMargin(sheet, sp);


        //    ///拿到整列的宽度
        //    for (int i = 0; i < sp.ColumnNum; i++)
        //    {
        //        sp.ColumnWidth.Add(i, sheet.GetColumnWidth(i));
        //    }


        //    try
        //    {
        //        ///获取第二张表的信息
        //        sheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.GetSheet(m_ParamShare);

        //        sp.subFirstRow = GetSubFirstAndLast(sheet, 0, workbook);
        //        sp.subLastRow = GetSubFirstAndLast(sheet, 1, workbook);
        //        sp.Width = GetNum(sheet, 3, workbook);
        //        sp.Height = GetNum(sheet, 4, workbook);


        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("第二张Sheet不存在", ex);
        //        sp.subFirstRow = 0;
        //        sp.subLastRow = 0;

        //    }


        //    GetCellWidthAndHeight(sp);



        //    sp.Header = GetTitle(sp);
        //    sp.DataArea = GetData(sp);
        //    sp.Bottom = GetBottom(sp);

        //    sp.Header.ProSize();
        //    sp.DataArea.ProSize();
        //    sp.Bottom.ProSize();


        //    if (sp.Height != 0)
        //    {
        //        sp.PageHeight = sp.Height * 3.9370079d;
        //    }

        //    if (sp.Width != 0)
        //    {
        //        sp.PageWidth = sp.Width * 3.9370079d;
        //    }


        //    return sp;



        //}

    }
}
