using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.IO;

namespace App.InfoGrid2.View.Biz.Financial
{
    public partial class FormKHTJ : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.headLab.Value = "<span class='page-head' >销售出货汇总报表</span>";

                DbDecipher decipher = ModelAction.OpenDecipher();

                decipher.DeleteModels<FINANCIAL_STATISTICS>("SESSION_ID='{0}'", this.Session.SessionID);
            }
        }

        /// <summary>
        /// 查询财务信息
        /// </summary>
        public void btnSelect()
        {
            try
            {


              


                int id = WebUtil.FormInt("HID");

                

                DbDecipher decipher = ModelAction.OpenDecipher();


                decipher.DeleteModels<FINANCIAL_STATISTICS>("SESSION_ID='{0}'", this.Session.SessionID);


                LightModelFilter lmFilter096 = new LightModelFilter("UT_096");

                if (id != 0)
                {
                    lmFilter096.And("COL_10", id);
                }

                DateTime begTime = StringUtil.ToDateTime(this.DateRangePicker1.StartValue, EC5.Utility.DateUtil.StartByMonth());
                DateTime endTime = StringUtil.ToDateTime(this.DateRangePicker1.EndValue, EC5.Utility.DateUtil.EndByMonth());


                if (!string.IsNullOrEmpty(this.DateRangePicker1.EndValue))
                {
                    endTime = EC5.Utility.DateUtil.EndDay(this.DateRangePicker1.EndValue);
                }

                lmFilter096.And("COL_3",begTime, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                lmFilter096.And("COL_3",endTime, HWQ.Entity.Filter.Logic.LessThanOrEqual);
                lmFilter096.And("ROW_SID",0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);



                List<LModel> lm096 = decipher.GetModelList(lmFilter096);

                List<FINANCIAL_STATISTICS> fsList = new List<FINANCIAL_STATISTICS>();


                lm096.ForEach(m =>
                {
                    var item = fsList.Find(f => f.CUSTOMER_ID == m.Get<int>("COL_10"));
                    if (item != null)
                    {
                        item.F_PRICE += m.Get<decimal>("COL_8");
                        item.F_NUMBER += m.Get<decimal>("COL_12");
                    }
                    else
                    {
                        FINANCIAL_STATISTICS fs = new FINANCIAL_STATISTICS()
                        {
                            CUSTOMER_ID = m.Get<int>("COL_10"),
                            REMARKS = m.Get<string>("COL_4"),
                            F_NO = m.Get<string>("COL_11"),
                            F_PRICE = m.Get<decimal>("COL_8"),
                            F_NUMBER = m.Get<decimal>("COL_12"),
                            DATE_TIME = DateTime.Now,
                            CREATE_DATE_TIME = DateTime.Now,
                            SESSION_ID = this.Session.SessionID

                        };

                        fsList.Add(fs);
                    }

                });


                

                decipher.InsertModels<FINANCIAL_STATISTICS>(fsList);

               

            }
            catch (Exception ex)
            {
                log.Error("出错了！", ex);
                MessageBox.Alert("出错了！");
                return;
            }
            this.store1.DataBind();


        }

     
        /// <summary>
        /// 这是清空查询条件
        /// </summary>
        public void btnClear()
        {
            this.DateRangePicker1.EndValue = "";
            this.DateRangePicker1.StartValue = "";
            this.tbName.Value = "";
            EasyClick.Web.Mini.MiniHelper.Eval(" $('#HID').val('0'); ");

        }


        /// <summary>
        /// 导出Excel文件
        /// </summary>
        public void ExportEXCEL()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FINANCIAL_STATISTICS));
            lmFilter.And("SESSION_ID", this.Session.SessionID);

            List<FINANCIAL_STATISTICS> fsList = decipher.SelectModels<FINANCIAL_STATISTICS>(lmFilter);

            if (fsList.Count == 0)
            {
                MessageBox.Alert("没有要导出的数据，请点查询按钮，重新查找数据！");
                return;
            }



            IWorkbook workbook = new HSSFWorkbook();

            ///创建表名为keyBooks的表
            ISheet sheet = workbook.CreateSheet("销售出货汇总报表");

            sheet.AddMergedRegion(new CellRangeAddress(0,0,0,3));

            IRow irHeade = sheet.CreateRow(0);

            ICell icHeade = irHeade.CreateCell(0);

            ICellStyle ics = workbook.CreateCellStyle();

            ics.Alignment = HorizontalAlignment.Center;
            ics.VerticalAlignment = VerticalAlignment.Center;


            IFont ifont =  workbook.CreateFont();

            ///字体大小
            ifont.FontHeightInPoints = 25;
            ///字体名称
            ifont.FontName = "宋体";

            //行高
            irHeade.Height = 500;

            ics.SetFont(ifont);

            icHeade.CellStyle = ics;

            icHeade.SetCellValue("销售出货汇总报表");


            IRow irTitle = sheet.CreateRow(1);




            //行高
            irTitle.Height = 500;

            ICell ic0 = irTitle.CreateCell(0);
            ICell ic1 = irTitle.CreateCell(1);
            ICell ic2 = irTitle.CreateCell(2);
            ICell ic3 = irTitle.CreateCell(3);


            ICellStyle icsTitle = workbook.CreateCellStyle();
            IFont ifontTitle = workbook.CreateFont();

            ///字体大小
            ifontTitle.FontHeightInPoints = 12;
            ifontTitle.FontName = "新宋体";
            ///粗体
            ifontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            ///左右居中
            icsTitle.Alignment = HorizontalAlignment.Center;
            ///顺直居中
            icsTitle.VerticalAlignment = VerticalAlignment.Center;
            ///下边框
            icsTitle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            ///左边框
            icsTitle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            ///右边框
            icsTitle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            ///上边框
            icsTitle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;

            icsTitle.SetFont(ifont);


            ic0.CellStyle = icsTitle;
            ic1.CellStyle = icsTitle;
            ic2.CellStyle = icsTitle;
            ic3.CellStyle = icsTitle;

            ic0.SetCellValue("编码");
            ic1.SetCellValue("客户名称");
            ic2.SetCellValue("数量");
            ic3.SetCellValue("金额");




            ICellStyle icsNew = workbook.CreateCellStyle();
            IFont ifontNew = workbook.CreateFont();

            ///字体大小
            ifontNew.FontHeightInPoints = 16;
            ///字体名称
            ifontNew.FontName = "宋体";

            ///左对齐
            icsNew.Alignment = HorizontalAlignment.Left;
            ///垂直居中
            icsNew.VerticalAlignment = VerticalAlignment.Center;
            ///下边框
            icsNew.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            ///左边框
            icsNew.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            ///右边框
            icsNew.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            ///上边框
            icsNew.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;

            icsNew.SetFont(ifontNew);



            for (int i = 0; i < fsList.Count; i++)
            {
                FINANCIAL_STATISTICS item = fsList[i];


                IRow irNew = sheet.CreateRow(2 + i);

                ICell icNew0 = irNew.CreateCell(0);

                ICell icNew1 = irNew.CreateCell(1);
                ICell icNew2 = irNew.CreateCell(2);
                ICell icNew3 = irNew.CreateCell(3);


                icNew0.CellStyle = icsNew;
                icNew0.SetCellValue(item.F_NO);

                icNew1.CellStyle = icsNew;
                icNew1.SetCellValue(item.REMARKS);

                icNew2.CellStyle = icsNew;
                icNew2.SetCellValue(item.F_NUMBER.ToString());

                icNew3.CellStyle = icsNew;
                icNew3.SetCellValue(item.F_PRICE.ToString("0.##"));

            }



            WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));

            FileUtil.AutoCreateDir(wFile.PhysicalDir);
            
            //新建一个Excel文件
            FileStream file = new FileStream(wFile.PhysicalPath, FileMode.OpenOrCreate);

            //把数据流写进Excel中
            workbook.Write(file);

            file.Close();

            EasyClick.Web.Mini.MiniHelper.EvalFormat("window.open('{0}')", wFile.RelativePath);

            

        }

        /// <summary>
        ///  小渔夫加的 管理模板按钮事件
        /// </summary>
        public void ManageTemplate()
        {

            //那个ID是随便给的一个唯一的ID
            string urlStr = "/App/InfoGrid2/View/PrintTemplate/ManageTemplateDZD.aspx?id=1000005";

            Window win = new Window("模板管理");
            win.ContentPath = urlStr;
            win.State = WindowState.Max;
            win.ShowDialog();
        }


        /// <summary>
        /// 小渔夫加的  打印按钮事件
        /// </summary>
        public void btnPrint()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.FormInt("HID");

            string name = this.tbName.Value;


            LModel lm071 = decipher.GetModelByPk("UT_071", id);

            if (lm071 == null)
            {
                MessageBox.Alert("请选择客户才能打印！");
                return;
            }

            //客户编码
            string code = lm071.Get<string>("COL_1");

            DateTime begTime = StringUtil.ToDateTime(this.DateRangePicker1.StartValue, EC5.Utility.DateUtil.StartByMonth());
            DateTime endTime = StringUtil.ToDateTime(this.DateRangePicker1.EndValue, EC5.Utility.DateUtil.EndByMonth());

            if (!string.IsNullOrEmpty(this.DateRangePicker1.EndValue))
            {
                endTime = EC5.Utility.DateUtil.EndDay(this.DateRangePicker1.EndValue);
            }


            //那个ID是随便给的一个唯一的ID
            string urlStr = $"/App/InfoGrid2/View/PrintTemplate/PrintTemplateDZD.aspx?id=1000005&name={name}&code={code}&begTime={begTime.ToString("yyyy - MM - dd")}&endTime={endTime.ToString("yyyy - MM - dd")}"; 

            Window win = new Window("打印");
            win.ContentPath = urlStr;
            win.State = WindowState.Max;
            win.ShowDialog();


        }



    }
}