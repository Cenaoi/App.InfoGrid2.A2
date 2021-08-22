using App.BizCommon;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 对账单的打印列表
    /// </summary>
    public partial class PrintTemplateDZD : WidgetControl, IView
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
                this.store1.DataBind();
                this.store2.DataBind();
            }
        }


        /// <summary>
        /// 打印Excel模板
        /// </summary>
        public void btnPrint()
        {



            string PrintID = this.store1.CurDataId;
            string TemplateID = this.store2.CurDataId;

            if (string.IsNullOrEmpty(PrintID))
            {
                MessageBox.Alert("请选择打印机！");
                return;
            }

            if (string.IsNullOrEmpty(TemplateID))
            {
                MessageBox.Alert("请选择Excel模板！");
                return;
            }




            DataRecord printDr = this.store1.GetDataCurrent();
            DataRecord templateDr = this.store2.GetDataCurrent();

            string url = templateDr.Fields["TEMPLATE_URL"].Value;

            string pathUrl = string.Empty;

            try
            {

                pathUrl = CreateExcelData(url);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("打印出错了！" + ex.Message);
                return;
            }

            try
            {

                BIZ_PRINT_FILE bpf = new BIZ_PRINT_FILE()
                {
                    FILE_URL = pathUrl,
                    PRINT_CODE = printDr.Fields["PRINT_CODE"].Value,
                    PRINT_NAME = printDr.Fields["PRINT_TEXT"].Value,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_SID = 0
                };

                DbDecipher decipher = ModelAction.OpenDecipher();

                decipher.InsertModel(bpf);


                EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'yes'});");

            }
            catch (Exception ex)
            {
                log.Error("插入打印数据出错了！", ex);
                MessageBox.Alert("打印出错了！");
            }


        }


        /// <summary>
        /// 生成打印Excel文件
        /// </summary>
        public string CreateExcelData(string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Alert("请选择模板！");
                return url;
            }

            string path = Server.MapPath(url);

            if (!File.Exists(path))
            {
                throw new Exception("模板文件不存在。");


            }
            DbDecipher decipher = ModelAction.OpenDecipher();

            DataSet ds = new DataSet();

            try
            {


                LModelElement modelElem = new LModelElement();
                modelElem.Fields.Add("NAME", LMFieldDBTypes.String);
                modelElem.Fields.Add("CODE", LMFieldDBTypes.String);
                modelElem.Fields.Add("BEG_TIME", LMFieldDBTypes.String);
                modelElem.Fields.Add("END_TIME", LMFieldDBTypes.String);
                modelElem.Fields.Add("RECEIVABLES", LMFieldDBTypes.Decimal);

                ds.Head = new LModel(modelElem);
                ds.Head["NAME"] = WebUtil.Query("name");
                ds.Head["CODE"] = WebUtil.Query("code");
                ds.Head["BEG_TIME"] = WebUtil.Query("begTime");
                ds.Head["END_TIME"] = WebUtil.Query("endTime");
                ds.Head["RECEIVABLES"] = 0;

                LightModelFilter lmFilter = new LightModelFilter("FINANCIAL_STATISTICS");
                lmFilter.And("SESSION_ID", this.Session.SessionID);

                ds.Items = decipher.GetModelList(lmFilter);


                if (ds.Head == null && ds.Items.Count > 0)
                {
                    ds.Head = ds.Items[0];
                }


            }
            catch (Exception ex)
            {

                throw new Exception("查询数据出错了！", ex);

            }

            try
            {



                NOPIHandlerEX handler = new NOPIHandlerEX();

                SheetPro sp = handler.ReadExcel(path);

                handler.InsertSubData(sp, ds);


                //文件名为当前时间时分秒都有
                string fileName = BillIdentityMgr.NewCodeForDay("PRINT", "P", 4) + ".xls";
                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", fileName);
                wFile.CreateDir();
                

                //保存Excel文件在服务器中
                handler.WriteExcel(sp, wFile.PhysicalPath);
                handler.Dispose();
                
                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

        }

        /// <summary>
        /// 导出 Excel 数据
        /// </summary>
        public void InputOut()
        {

            string PrintID = this.store1.CurDataId;
            string TemplateID = this.store2.CurDataId;

            if (string.IsNullOrEmpty(PrintID))
            {
                MessageBox.Alert("请选择打印机！");
                return;
            }

            if (string.IsNullOrEmpty(TemplateID))
            {
                MessageBox.Alert("请选择Excel模板！");
                return;
            }

            DataRecord templateDr = this.store2.GetDataCurrent();

            string url = templateDr.Fields["TEMPLATE_URL"].Value;


            string pathUrl = CreateExcelData(url);

            EasyClick.Web.Mini.MiniHelper.Eval($"window.open('{pathUrl}')");


        }

    }
}