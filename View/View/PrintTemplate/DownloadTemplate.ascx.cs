using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Excel_Template;
using HWQ.Entity.Filter;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using EC5.Utility;
using App.InfoGrid2.Excel_Template.V1;

namespace App.InfoGrid2.View.PrintTemplate
{
    public partial class DownloadTemplate : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ///页面传过来的属性

        /// <summary>
        /// 页面ID
        /// </summary>
        public int m_pageID;
        /// <summary>
        /// 页面名称
        /// </summary>
        public string m_pageText;

        /// <summary>
        /// 主表tableID
        /// </summary>
        public int m_mainTableID;

        /// <summary>
        /// 子表tableID
        /// </summary>
        public int m_subTableID;

        /// <summary>
        /// 主表本身ID
        /// </summary>
        public  int m_mainID;
        /// <summary>
        /// 主表主键
        /// </summary>
        public string m_mainPK ;
        /// <summary>
        /// 主表名称
        /// </summary>
        public string m_mainTabel;
        /// <summary>
        /// 子表名称
        /// </summary>
        public string m_subTable;
        /// <summary>
        /// 子表对应主表列
        /// </summary>
        public string m_fFiled;

        #endregion

        /// <summary>
        /// 行高
        /// </summary>
        short m_RowHegiht = 500;

        /// <summary>
        /// 列宽
        /// </summary>
        int m_CellWidth = 5000;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            m_mainID = WebUtil.QueryInt("mainID");
            m_mainTableID = WebUtil.QueryInt("mainTableID");
            m_pageID = WebUtil.QueryInt("pageID");
            m_subTableID = WebUtil.QueryInt("subTableID");

            if (m_subTableID == 0 || m_pageID == 0 || m_mainID == 0 || m_mainTableID== 0)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


            m_fFiled = WebUtil.Query("fFiled");
            m_mainPK = WebUtil.Query("mainPK");
            m_mainTabel = WebUtil.Query("mainTable");
            m_pageText = WebUtil.Query("pageText");
            m_subTable = WebUtil.Query("subTable");

            if (string.IsNullOrEmpty(m_fFiled) || string.IsNullOrEmpty(m_mainPK) || string.IsNullOrEmpty(m_mainTabel) || string.IsNullOrEmpty(m_subTable))
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }





            if(!IsPostBack)
            {
                InitData();
            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData() 
        {
            
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {

                List<BIZ_PRINT_TEMPLATE> bptList = decipher.SelectModels<BIZ_PRINT_TEMPLATE>("PAGE_ID={0} and ROW_SID >=0", m_pageID);


                foreach (BIZ_PRINT_TEMPLATE item in bptList)
                {
                    this.cbxTemplate.Items.Add(item.TEMPLATE_URL, item.TEMPLATE_NAME);
                }

                if (bptList.Count > 0)
                {
                    this.cbxTemplate.Value = bptList[0].TEMPLATE_URL;
                    return;
                }

                CreateDefaultTemplate();


            }
            catch (Exception ex) 
            {
                log.Error("初始化数据失败了！",ex);
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


        }



        /// <summary>
        /// 获取子表的信息
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public IG2_TABLE GetSubTable(int tableId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter tableFilter = new LightModelFilter(typeof(IG2_TABLE));
            tableFilter.And("IG2_TABLE_ID", tableId);
            tableFilter.Locks.Add(LockType.NoLock);

            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(tableFilter);

            return table;
        }



        /// <summary>
        /// 导出Excel文件
        /// 修改于 2018-11-16
        /// 小渔夫
        /// </summary>
        public void InputOut() 
        {

            string url = this.cbxTemplate.Value;

            if(string.IsNullOrEmpty(url))
            {
                MessageBox.Alert("请选择模板！");
                return;
            }


            EasyClick.Web.Mini2.ScriptManager.Eval("ownerWindow.close({result:'ok',url:'"+url+"'});");


            
            //DataSet ds = new DataSet();

            //try
            //{

            //    DbDecipher decipher = ModelAction.OpenDecipher();

            //    LightModelFilter mainFilter = new LightModelFilter(m_mainTabel);
            //    mainFilter.And(m_mainPK, m_mainID);
            //    mainFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);



            //    ///拿到主表数据
            //    ds.Head = decipher.GetModel(mainFilter);

            //    IG2_TABLE subtable = GetSubTable(m_subTableID);

            //    LightModelFilter subFilter = new LightModelFilter(m_subTable);
            //    subFilter.And(m_fFiled, m_mainID);
            //    subFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            //    //有没有排序
            //    if (!string.IsNullOrWhiteSpace(subtable.SORT_TEXT))
            //    {
            //        subFilter.TSqlOrderBy = subtable.SORT_TEXT;
            //    }

            //    ///拿到子表数据
            //    ds.Items = decipher.GetModelList(subFilter);

            //}
            //catch (Exception ex) 
            //{
            //    log.Error("查询数据出错了！",ex);
            //    MessageBox.Alert("查询数据出错了！");
            //    return;
            //}

            //try
            //{
            //    WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
            //    wFile.CreateDir();

            //    string srcPath = Server.MapPath(url);


            //    SheetParam sp = TemplateUtilV1.ReadTemp(srcPath);

            //    //保存Excel文件在服务器中
            //    TemplateUtilV1.CreateExcel(sp, ds, wFile.PhysicalPath);

            //    DownloadWindow.Show( wFile.Filename, wFile.RelativePath);

            //}
            //catch (Exception ex) 
            //{
            //    log.Error("导出Excel文件出错了！",ex);
            //    MessageBox.Alert("导出Excel文件出错了！");
            //}

        }


        /// <summary>
        /// 创建模板文件
        /// </summary>
        private void CreateDefaultTemplate() 
        {
           
            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet ts ;
            TableSet tsSub; ;

            try
            {

                ///主表列信息
                ts = TableSet.SelectSID_0_5(decipher, m_mainTableID);



                ///子表列信息
                tsSub = TableSet.SelectSID_0_5(decipher, m_subTableID);

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

                decipher.InsertModel(bpt);

                string name = string.Format(
                    "{0}_{1}_{2}_{3}_{4}.xls",
                    m_pageText,
                    m_pageID,
                    bpt.BIZ_PRINT_TEMPLATE_ID,
                    m_mainTabel,
                    m_subTable
                    );


                WebFileInfo wFile = new WebFileInfo("/PrintTemplate", name);
                wFile.CreateDir();

                WriteExcel(wFile.PhysicalPath , ts, tsSub);

                bpt.TEMPLATE_URL = wFile.RelativePath;

                decipher.UpdateModelProps(bpt, "TEMPLATE_URL");

                this.cbxTemplate.Items.Add(bpt.TEMPLATE_URL,bpt.TEMPLATE_NAME);
                this.cbxTemplate.Value = bpt.TEMPLATE_URL;
            }
            catch (Exception ex) 
            {
                throw new Exception("生成默认模板时出错了！",ex);
            }

        }


        /// <summary>
        /// 跳转到管理页面
        /// </summary>
        public void GoEdit() 
        {
            string url = string.Format("/App/InfoGrid2/View/PrintTemplate/ManageTemplate.aspx?mainID={0}&pageID={1}&fFiled={2}&mainTableID={3}&subTableID={4}&mainPK={5}&mainTable={6}&subTable={7}&pageText={8}",
                m_mainID,
                m_pageID,
                m_fFiled,
                m_mainTableID,
                m_subTableID,
                m_mainPK,
                m_mainTabel,
                m_subTable,
                m_pageText
                );

            MiniPager.Redirect(url);

        }




        /// <summary>
        /// 生成Excel模板
        /// </summary>
        /// <param name="path">生成模板路径</param>
        /// <param name="tsItem">主表信息</param>
        /// <param name="tsSubItem">子表信息</param>
        public void WriteExcel(string path, TableSet tsItem, TableSet tsSubItem)
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

                isheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, visibleSubCount - 1));
                isheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, visibleSubCount - 1));


                ICellStyle ics = workbook.CreateCellStyle();



                IFont ifont = workbook.CreateFont();

                isheet.PrintSetup.Scale = 100;
                isheet.PrintSetup.PaperSize = (short)PaperSize.A4;
                isheet.PrintSetup.UsePage = true;

                isheet.PrintSetup.FitHeight = 0;
                isheet.PrintSetup.FitWidth = 1;
                isheet.FitToPage = false;


                SetExcelHead(ics, isheet, ifont);



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
        /// <param name="isheet">字体设置</param>
        private void SetExcelHead(ICellStyle ics, ISheet isheet, IFont ifont)
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
        private void SetExcelData(TableSet tsItem, TableSet tsSubItem, ISheet isheet, IWorkbook workbook)
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
        private void SetExcelItem(ISheet isheet, TableSet tsItem, out int i, IWorkbook workbook)
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
        private void SetExcelSubItem(ISheet isheet, TableSet tsSubItem, int i, IWorkbook workbook)
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