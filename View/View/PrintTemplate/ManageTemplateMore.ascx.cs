using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.PrintTemplate
{
    public partial class ManageTemplateMore  : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 页面ID
        /// </summary>
        public int m_pageID;

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
            this.store1.Deleting += new ObjectCancelEventHandler(store1_Deleting);

            if (!IsPostBack)
            {
                InitData();


                this.store1.DataBind();


            }
        }


        /// <summary>
        /// 初始化数据，看看有没有默认模板
        /// </summary>
        void InitData()
        {




            EcUserState user = EcContext.Current.User;

            //判断是否是设计师
            if (user.Roles.Exist(IG2Param.Role.BUILDER))
            {
                var table_remaks = table1.Columns.FindByDataField("REMARKS");

                table_remaks.Visible = true;

            }



            DbDecipher decipher = ModelAction.OpenDecipher();


            int id = WebUtil.QueryInt("id");


            ViewSet m_ViewSet = ViewSet.Select(decipher, id);



            string sql = String.Format("select count(BIZ_PRINT_TEMPLATE_ID) from BIZ_PRINT_TEMPLATE where ROW_SID >=0 and TEMPLATE_NAME = '默认模板' and PAGE_ID = {0} and PAGE_TEXT='{1}'", id, m_ViewSet.View.DISPLAY);

            int count = decipher.ExecuteScalar<int>(sql);

            if (count > 0)
            {
                return;
            }




            try
            {



                BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
                {
                    MAIN_TABLE_ID = 0,
                    PAGE_ID = id,
                    PAGE_TEXT = m_ViewSet.View.DISPLAY,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_DATE_UPDATE = DateTime.Now,
                    ROW_SID = 0,
                    SUB_TABLE_NAME = "UT_001",
                    MAIN_TABLE_NAME = "UT_001",
                    TABLE_NUMBER = 0,
                    TEMPLATE_TYPE = "EXCEL",
                    SUB_F_FIELD = "ROW_IDENTITY_ID",
                    TEMPLATE_NAME = "默认模板"
                };


                string name = string.Format(
                    "{0}_{1}_{2}_{3}_{4}.xls",
                    m_ViewSet.View.DISPLAY,
                    id,
                    bpt.BIZ_PRINT_TEMPLATE_ID,
                    "UT_001",
                    "UT_001"
                    );


                WebFileInfo wFile = new WebFileInfo("/PrintTemplate", name);
                wFile.CreateDir();
                
                WriteExcel(wFile.PhysicalPath, m_ViewSet);

                bpt.TEMPLATE_URL = wFile.RelativePath;


                decipher.InsertModel(bpt);




            }
            catch (Exception ex)
            {
                throw new Exception("生成默认模板时出错了！", ex);
            }
        }


        /// <summary>
        /// 删除模板数据顺便也把模板文件给删除了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void store1_Deleting(object sender, ObjectCancelEventArgs e)
        {
            try
            {
                LModel lm = (LModel)e.Object;

                string url = lm["TEMPLATE_URL"].ToString();

                string path = Server.MapPath(url);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

            }
            catch (Exception ex)
            {
                log.Error("删除模板文件出错了！", ex);
            }
        }


        /// <summary>
        /// 显示上传界面
        /// </summary>
        public void InputTemplate()
        {

            BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
            {
                MAIN_TABLE_ID = 0,
                PAGE_ID = m_pageID,
                PAGE_TEXT = "关联表",
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = -1,
                SUB_TABLE_NAME = "UT_001",
                MAIN_TABLE_NAME = "UT_001",
                TABLE_NUMBER = 0,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = "ROW_IDENTITY_ID"
            };

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                decipher.InsertModel(bpt);

                string url = "/App/InfoGrid2/View/PrintTemplate/TemplateUpload.aspx?id=" + bpt.BIZ_PRINT_TEMPLATE_ID;

                EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowUrl('{0}')", url);


            }
            catch (Exception ex)
            {
                log.Error("插入新模板数据出错了！", ex);
                MessageBox.Alert("插入新模板数据出错了！");
            }



        }

        /// <summary>
        /// 更新模板数据
        /// </summary>
        /// <param name="id"></param>
        public void UpdateData(string id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                BIZ_PRINT_TEMPLATE bpt = decipher.SelectModelByPk<BIZ_PRINT_TEMPLATE>(id);

                if (bpt == null)
                {
                    MessageBox.Alert("找不到上传模板数据！");
                    return;
                }

                bpt.ROW_SID = 0;

                decipher.UpdateModelProps(bpt, "ROW_SID");


                this.store1.DataBind();


            }
            catch (Exception ex)
            {
                log.Error("找不到上传模板数据！", ex);
                MessageBox.Alert("找不到上传模板数据！");

            }

        }

        /// <summary>
        /// 下载模板
        /// </summary>
        public void DowTemplate()
        {
            string id = this.store1.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择要下载的模板！");
                return;
            }

            DataRecord dr = this.store1.GetDataCurrent();

            string url = dr.Fields["TEMPLATE_URL"].Value;


            EasyClick.Web.Mini.MiniHelper.EvalFormat("DonwloadShow('{0}','{1}')", "下载Excle模板", url);

        }



        /// <summary>
        /// 生成Excel模板
        /// </summary>
        /// <param name="path">生成模板路径</param>
        /// <param name="tsItem">主表信息</param>
        /// <param name="tsSubItem">子表信息</param>
        public void WriteExcel(string path, ViewSet m_ViewSet)
        {

            int visibleSubCount = m_ViewSet.Fields.Count;   //显示的列


            try
            {
                IWorkbook workbook = new HSSFWorkbook();


                ISheet isheet = workbook.CreateSheet("sheet1");

                isheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, visibleSubCount - 1));


                ICellStyle ics = workbook.CreateCellStyle();



                IFont ifont = workbook.CreateFont();

                isheet.PrintSetup.Scale = 100;
                isheet.PrintSetup.PaperSize = (short)PaperSize.A4;
                isheet.PrintSetup.UsePage = true;

                isheet.PrintSetup.FitHeight = 0;
                isheet.PrintSetup.FitWidth = 1;
                isheet.FitToPage = false;


                SetExcelHead(ics, isheet, ifont);



                SetExcelData( m_ViewSet, isheet, workbook);




                for (int i = 0; i < visibleSubCount; i++)
                {

                    isheet.SetColumnWidth(i++, m_CellWidth);
                }


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
        private void SetExcelData(ViewSet m_ViewSet, ISheet isheet, IWorkbook workbook)
        {

            ///这是设置第二张工作表的数据的
            ISheet isheet1 = workbook.CreateSheet("sheet2");

            IRow ir = isheet1.CreateRow(0);
            ICell ic = ir.CreateCell(1);

            ic.SetCellValue("子表开始位置");
            ic = ir.CreateCell(0);
            ic.SetCellValue(3);

            ir = isheet1.CreateRow(1);
            ic = ir.CreateCell(1);
            ic.SetCellValue("子表结束位置");
            ic = ir.CreateCell(0);
            ic.SetCellValue(3);

            ir = isheet1.CreateRow(3);
            ic = ir.CreateCell(1);
            ic.SetCellValue("页宽");

            ir = isheet1.CreateRow(4);
            ic = ir.CreateCell(1);
            ic.SetCellValue("页高");

          
            SetExcelSubItem(isheet,m_ViewSet, 1, workbook);
            


        }

      
        /// <summary>
        /// 把子表数据写入到Excel中去
        /// </summary>
        /// <param name="i">行数</param>
        /// <param name="isheet">工作薄</param>
        /// <param name="tsSubItem">子表数据</param>
        /// <param name="workbook">Excel文件</param>
        private void SetExcelSubItem(ISheet isheet, ViewSet m_ViewSet, int i, IWorkbook workbook)
        {

            IRow ir = isheet.CreateRow(i);
            IRow ir1 = isheet.CreateRow(i + 1);



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

            foreach (var  lm in m_ViewSet.Fields)
            {
              
                ICell ic = ir.CreateCell(j);
                ic.SetCellValue(lm.FIELD_TEXT);
                ic.CellStyle = ics;


                ic = ir1.CreateCell(j);
                ic.SetCellValue("{$T." +lm.TABLE_NAME+"_"+ lm.FIELD_NAME + "}");
                ic.CellStyle = ics1;

                j++;
            }

        }


    }
}