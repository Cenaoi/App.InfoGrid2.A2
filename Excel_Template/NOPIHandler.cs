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

namespace App.InfoGrid2.Excel_Template
{
    public class NOPIHandler
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
        /// 读取Excel文件
        /// </summary>
        /// <param name="path">Excel模板的路径</param>
        public SheetPro ReadExcel(string path)
        {
            ///判断文件是否存在
            if(!File.Exists(path))
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
                ISheet sheet = workbook.GetSheet(m_TemplateShare);

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

                GetRoowAllData(sheet, workbook, sp.rowProList);

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


                ///拿到整列的宽度
                for (int i = 0; i <= sheet.GetRow(0).LastCellNum;i++ )
                {
                    sp.ColumnWidth.Add(i,sheet.GetColumnWidth(i));
                    sp.ColumnNum = i;
                }
                ///获取第二张表的信息
                sheet = workbook.GetSheet(m_ParamShare);

                sp.subFirstRow = GetSubFirstAndLast(sheet, 0, workbook);
                sp.subLastRow = GetSubFirstAndLast(sheet, 1, workbook);

                return sp;

            }
            catch(Exception ex)
            {

                throw new Exception("读取Excel模板出错！",ex);
                
            }
        }

        /// <summary>
        /// 设置打印属性的
        /// </summary>
        /// <param name="sheet">工作薄</param>
        /// <param name="sp">工作薄类</param>
        public void SetPrintPor(ISheet sheet,SheetPro sp)
        {
             IPrintSetup ips = sheet.PrintSetup;
             ips.FitHeight = sp.pp.FitHeight;
             ips.FitWidth = sp.pp.FitWidth;
             ips.Draft = sp.pp.IsDraft;
             ips.Landscape = sp.pp.Landscape;
             ips.LeftToRight = sp.pp.LeftToRight ;
             ips.NoColor = sp.pp.NoColors ;
             ips.PageStart = sp.pp.PageStart;
             ips.PaperSize = sp.pp.PaperSize;
             ips.Scale = sp.pp.Scale;
             ips.UsePage = sp.pp.UsePage ;
        }

        /// <summary>
        /// 获取工作薄的打印属性
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sp"></param>
        public void GetPrintPor(ISheet sheet, SheetPro sp) 
        {
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
        private void GetRoowAllData(ISheet sheet, IWorkbook workbook, List<RowPro> rowList) 
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
                        continue;
                    }
                    RowPro rp = new RowPro();

                    rp.RowHeight = row.Height;
                    
                    

                    GetCellAllData(row,i,workbook,rp);

                    rowList.Add(rp);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("拿到行数据发生错误！", ex);
               
            }
        }
        /// <summary>
        /// 拿到所有单元格的数据
        /// </summary>
        private void GetCellAllData(IRow row, int i, IWorkbook workbook, RowPro rp) 
        {
            try
            {
                for (int j = 0; j <= row.LastCellNum; j++)
                {
                    ///拿到每一个单元格的对象
                    ICell icell = row.GetCell(j);
                    ///判断单元格是否有数据
                    if (icell == null)
                    {
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
                    GetCellStyle(cp,icell,workbook);
                    rp.cellPro.Add(cp);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("拿到所有单元格的数据",ex);
            }
        }


        /// <summary>
        /// 拿到单元格的值
        /// </summary>
        /// <returns></returns>
        private string GetCellValue(ICell icell) 
        {


            if(icell.CellType == CellType.Blank)
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
                log.Error("拿单元格的日期类型出错了！",ex);
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
        private void GetCellStyle(CellPro cp, ICell icell,IWorkbook workbook) 
        {
            try
            {
                ICellStyle ics = icell.CellStyle;

                ///下边框
                cp.BorderBottomName = ics.BorderBottom;
                cp.BorderBottomColor = ics.BottomBorderColor;
                ///上边框
                cp.BorderTopName= ics.BorderTop;
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
                cp.FillBackgroundColor =  ics.FillBackgroundColor;
                ///填充前景色
                cp.FillForegroundColor = ics.FillForegroundColor;

                ///填充模式
                cp.FillPattern = ics.FillPattern;



                IFont ifont = ics.GetFont(workbook);
                ///字体颜色
                cp.FontColor = ifont.Color;
                ///字体名字
                cp.FontName = ifont.FontName;
                ///字体大小
                cp.FontSize = ifont.FontHeightInPoints;
                ///字体粗细
                cp.FontBlod = ifont.Boldweight;

            }
            catch(Exception ex)
            {
                throw new Exception("拿单元格的属性发生错误！",ex);
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
                return index;
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

                IWorkbook workbook = new HSSFWorkbook();

                ///创建表名为keyBooks的表
                ISheet sheet = workbook.CreateSheet("sheet1");

                SetRowPro(sp, sheet, workbook);
                /////合并单元格
                foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                {

                    sheet.AddMergedRegion(new CellRangeAddress(cp.SpanFirstRow, cp.SpanLastRow, cp.SpanFirstCell, cp.SpanLastCell));

                }
                ///设置整列的宽度
                for (int i = 0; i <= sp.ColumnNum;i++ )
                {
                    sheet.SetColumnWidth(i, sp.ColumnWidth[i]);
                }

                ///设置打印属性
                SetPrintPor(sheet, sp);

                #region  ///插入图片到Excel去

                for (int i = 0; i < sp.picturesList.Count; i++ )
                {

                    HSSFPictureData hpd = sp.picturesList[i];
                    HSSFClientAnchor hca = sp.hcaList[i];
                    int pictureIdx = workbook.AddPicture(hpd.Data, NPOI.SS.UserModel.PictureType.JPEG);
                    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    HSSFClientAnchor anchor = new HSSFClientAnchor(hca.Dx1, hca.Dy1, hca.Dx2, hca.Dy2, hca.Col1, hca.Row1, hca.Col2, hca.Row2);
                    //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50
                    HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                    pict.Resize();

                }


                #endregion

                


                ///新建一个Excel文件
                FileStream file = new FileStream(path, FileMode.OpenOrCreate);

                ///把数据流写进Excel中
                workbook.Write(file);

                file.Close();

            }
            catch(Exception ex) 
            {
                throw new Exception("导出Excel文件出错了！",ex);
            }
        }


        /// <summary>
        /// 设置行属性
        /// </summary>
        private void SetRowPro(SheetPro sp, ISheet sheet, IWorkbook workbook) 
        {
            try
            {
                for (int i = 0; i < sp.rowProList.Count; i++)
                {
                    RowPro rp = sp.rowProList[i];

                    ///新建一行
                    IRow row = sheet.CreateRow(i);
                    ///行高
                    row.HeightInPoints = rp.RowHeight;
                    List<CellPro> cpList = rp.cellPro;

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
                throw new Exception("拿行数据出错了！",ex);
            }
        }


        /// <summary>
        /// 设置单元格的属性
        /// </summary>
        private void SetCellStyl(ICell ic, CellPro cp,IWorkbook workbook) 
        {

            try
            {
                

                ICellStyle ics = workbook.CreateCellStyle();
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

              


                IFont ifont = workbook.CreateFont();
                ///设置字体名称
                ifont.FontName = cp.FontName;
                ///设置字体颜色
                ifont.Color = cp.FontColor;
                ///设置字体大小
                ifont.FontHeightInPoints = cp.FontSize;
                ///设置字体粗细
                ifont.Boldweight = cp.FontBlod;

                

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
        private void SetCellValue(ICell ic,CellTypeName typeName,string vlaue) 
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
        /// <param name="sp"></param>
        /// <param name="mf"></param>
        /// <returns></returns>
        public SheetPro InsertSubData(SheetPro sp,DataSet mf) 
        {
            LModel lmHead =  mf.Head;
            ///在集合里面子表开始的下标
            int subFirstRow = sp.subFirstRow - 1;
            ///在集合里面子表结束的下标
            int  subLastRow = sp.subLastRow - 1;

            if(sp == null && sp.rowProList.Count == 0)
            {
                return null;
            }

            int count = mf.Items.Count;

            #region ///把图片的位置设置成相对位置


            foreach(HSSFClientAnchor hca in sp.hcaList)
            {
                if (hca.Row1 > subLastRow)
                {
                    hca.Row1 += (count -(subLastRow - subFirstRow) - 1);
                    hca.Row2 += (count - (subLastRow - subFirstRow) - 1);
                }
            }

            #endregion
            try
            {

                ///保证下标不越界
                subFirstRow = (subFirstRow >= sp.rowProList.Count) ? sp.rowProList.Count : subFirstRow;
                subLastRow = (subLastRow >= sp.rowProList.Count) ? sp.rowProList.Count : subLastRow;

                UpdateSpanCell(sp.SpanCellAndRow, mf, subFirstRow, subLastRow);

                for (int i = 0; i < subFirstRow; i++)
                {
                    RowPro rp = sp.rowProList[i];

                    foreach (CellPro cp in rp.cellPro)
                    {
                       
                        JTemplate jt = new JTemplate();

                        jt.SrcText = cp.Value;
                        jt.Model = lmHead;
                        cp.Value = jt.Exec();

                        
                    }

                }



                foreach (LModel item in mf.Items)
                {
                    List<RowPro> rpList = CopyData(sp, subFirstRow, subLastRow);

                    LModelElement modelElem = item.GetModelElement();

                    LModelFieldElement fieldElem = null;

                    foreach (RowPro rp1 in rpList)
                    {
                        foreach (CellPro cp in rp1.cellPro)
                        {

                           
                            bool isNum = false;

                            ///这是判断是否为数字类型
                            if (cp.Value.IndexOf("{") == 0 && cp.Value.IndexOf("-") < 0)
                            {
                                //切割字符串。。提取模板  {$T.COL_1}  的字段名 COL_1
                                string vl = cp.Value.Substring(4, cp.Value.Length - 5);

                                if (modelElem.TryGetField(vl, out fieldElem))
                                {
                                    object value = item[vl];

                                    if (value != null  && fieldElem.IsNumber)
                                    {
                                            decimal cc = Convert.ToDecimal(value);
                                            cp.Value = cc.ToString();
                                            cp.CellType = CellTypeName.Double;
                                    
                                    }
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
                                throw new Exception(string.Format("字段“{0}”异常！",cp.Value), ex);
                            }
                           
                        }

                        sp.rowProList.Insert(subFirstRow, rp1);
                        subFirstRow++;
                        subLastRow++;
                    }

                }
                ///把子表模板数据给移除掉
                sp.rowProList.RemoveRange(subFirstRow, (subLastRow - subFirstRow)+1);

                for (int i = subLastRow; i <= sp.rowProList.Count; i++)
                {
                    RowPro rp = sp.rowProList[i];

                    foreach (CellPro cp in rp.cellPro)
                    {

                        JTemplate jt = new JTemplate();

                        jt.SrcText = cp.Value;
                        jt.Model = lmHead;
                        cp.Value = jt.Exec();
                    }

                }

                return sp;
            }
            catch (Exception ex) 
            {
                log.Error("插入子表数据出错了！",ex);
                return sp;
            }

        }
        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <returns></returns>
        private List<RowPro> CopyData(SheetPro sp, int subFirstRow, int subLastRow) 
        {
            try
            {

                List<RowPro> rpList = new List<RowPro>();

                for (int i = subFirstRow; i <= subLastRow; i++)
                {
                    RowPro rp = sp.rowProList[i];

                    RowPro rp1 = new RowPro();

                    foreach (CellPro cp in rp.cellPro)
                    {
                        CellPro cp1 = new CellPro();

                        cp1.Alignment = cp.Alignment;
                        cp1.BorderBottomColor = cp.BorderBottomColor;
                        cp1.BorderBottomName = cp.BorderBottomName;
                        cp1.BorderLeftColor = cp.BorderLeftColor;
                        cp1.BorderLeftName = cp.BorderLeftName;
                        cp1.BorderRightColor = cp.BorderRightColor;
                        cp1.BorderRightName = cp.BorderRightName;
                        cp1.BorderTopColor = cp.BorderTopColor;
                        cp1.BorderTopName = cp.BorderTopName;
                        cp1.ColsIndex = cp.ColsIndex;
                        cp1.FontBlod = cp.FontBlod;
                        cp1.FontColor = cp.FontColor;
                        cp1.FontName = cp.FontName;
                        cp1.FontSize = cp.FontSize;
                        cp1.RowIndex = cp.RowIndex;
                        cp1.SpanFirstCell = cp.SpanFirstCell;
                        cp1.SpanFirstRow = cp.SpanFirstRow;
                        cp1.SpanLastCell = cp.SpanLastCell;
                        cp1.SpanLastRow = cp.SpanLastRow;
                        cp1.Value = cp.Value;
                        cp1.VerticalAlignment = cp.VerticalAlignment;

                        rp1.cellPro.Add(cp1);

                    }

                    rp1.RowHeight = rp.RowHeight;

                    rpList.Add(rp1);

                }

                return rpList;

            }
            catch (Exception ex) 
            {
                throw new Exception("这是拷贝数据发生错误！",ex);
            }
            
        }

        /// <summary>
        /// 更新合并单元格数据
        /// </summary>
        private void UpdateSpanCell(RowPro rp, DataSet mf, int subFirstRow, int subLastRow) 
        {

            if(rp == null)
            {
                return;
            }

            
            ///头部合并集合
            List<CellPro> cpHead = GetSpanCell(rp.cellPro, subFirstRow, subLastRow, 0);
            ///数据部合并集合
            List<CellPro> cpSubData = GetSpanCell(rp.cellPro, subFirstRow, subLastRow, 1);
            ///尾部合并集合
            List<CellPro> cpFoot = GetSpanCell(rp.cellPro, subFirstRow, subLastRow, 2);

            cpSubData = UpdateSubSpanCell(cpSubData, mf.Items.Count, (subLastRow - subFirstRow) + 1);

            rp.cellPro.Clear();

            foreach(CellPro cp in cpHead)
            {
                rp.cellPro.Add(cp);

            }

            int i = subFirstRow;
            
            foreach (CellPro cp in cpSubData)
            {
                  cp.SpanFirstRow += i;
                  cp.SpanLastRow += i;
                  rp.cellPro.Add(cp);
            }

            i = subFirstRow + ((subLastRow - subFirstRow) + 1) * mf.Items.Count;

            foreach (CellPro cp in cpFoot)
            {
                cp.SpanFirstRow += i;
                cp.SpanLastRow += i;
                rp.cellPro.Add(cp);
            }


            



        }
        /// <summary>
        /// 拿到头或数据或尾部的合并数据集合
        /// </summary>
        /// <param name="cpList">所有的合并数据集合</param>
        /// <param name="subFirstRow">数据开始行</param>
        /// <param name="subLastRow">数据结束行</param>
        /// <param name="mark">0--标识头部，1--标识数据部，2--标识尾部 </param>
        /// <returns></returns>
        private List<CellPro> GetSpanCell(List<CellPro> cpList, int subFirstRow, int subLastRow, int mark) 
        {
            List<CellPro> newCpList = new List<CellPro>();

            try
            {
                if (mark == 0)
                {

                    foreach (CellPro cp in cpList)
                    {
                        if (cp.SpanFirstRow >= subFirstRow) { continue; }

                        CellPro cp1 = new CellPro();
                        cp1.SpanFirstRow = cp.SpanFirstRow;
                        cp1.SpanLastRow = cp.SpanLastRow;
                        cp1.SpanFirstCell = cp.SpanFirstCell;
                        cp1.SpanLastCell = cp.SpanLastCell;

                        newCpList.Add(cp1);

                    }

                }
                else if (mark == 1)
                {

                    foreach (CellPro cp in cpList)
                    {
                        if (cp.SpanFirstRow < subFirstRow || cp.SpanFirstRow > subLastRow) { continue; }

                        CellPro cp1 = new CellPro();
                        cp1.SpanFirstRow = cp.SpanFirstRow - subFirstRow ;
                        cp1.SpanLastRow = cp.SpanLastRow - subFirstRow ;
                        cp1.SpanFirstCell = cp.SpanFirstCell;
                        cp1.SpanLastCell = cp.SpanLastCell;

                        newCpList.Add(cp1);

                    }


                }
                else if (mark == 2)
                {
                    foreach (CellPro cp in cpList)
                    {
                        if (cp.SpanFirstRow <= subLastRow) { continue; }

                        CellPro cp1 = new CellPro();
                        cp1.SpanFirstRow = cp.SpanFirstRow - subLastRow -1;
                        cp1.SpanLastRow = cp.SpanLastRow - subLastRow - 1;
                        cp1.SpanFirstCell = cp.SpanFirstCell;
                        cp1.SpanLastCell = cp.SpanLastCell;

                        newCpList.Add(cp1);

                    }
                  
                }
                
            }
            catch (Exception ex) 
            {
                throw new Exception("拿到头或数据或尾部的合并数据集合", ex);
            }
            return newCpList;
        }
        /// <summary>
        /// 更新数据部合并集合
        /// </summary>
        /// <param name="cpSubData">数据部合并集合</param>
        /// <param name="number">子表数据数量</param>
        private List<CellPro> UpdateSubSpanCell(List<CellPro> cpSubData, int number,int num) 
        {
            List<CellPro> newCpList = new List<CellPro>();
            try
            {
                for (int i = 0; i < number; i++)
                {
                    for (int j = 0; j < cpSubData.Count; j++ )
                    {
                        CellPro cp = cpSubData[j];
                        CellPro cp1 = new CellPro();

                        cp1.SpanFirstRow = cp.SpanFirstRow + num * i;
                        cp1.SpanLastRow = cp.SpanLastRow + num * i;
                        cp1.SpanFirstCell = cp.SpanFirstCell;
                        cp1.SpanLastCell = cp.SpanLastCell;
                        newCpList.Add(cp1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("更新数据部合并集合", ex);
            }

            return newCpList;

        }


    }

   
}