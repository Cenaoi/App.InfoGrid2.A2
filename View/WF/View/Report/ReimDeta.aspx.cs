using App.BizCommon;
using App.InfoGrid2.WF.Bll;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.WF.View.Report
{
    public partial class ReimDeta : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 获取周报告详细信息
        /// </summary>
        /// <returns></returns>
        public string GetReportObj()
        {

            int id = WebUtil.QueryInt("id");

            //显示类型
            string show_type = WebUtil.QueryTrimUpper("show_type");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm371 = decipher.GetModelByPk("UT_371", id);

            SModel sm = new SModel();


            lm371.CopyTo(sm);





            int row_id = lm371.Get<int>("ROW_IDENTITY_ID");

            sm["COL_1"] = BusHelper.FormatDate(lm371.Get<string>("COL_1"));

            SModel imgs = new SModel();

            imgs["data"] = BusHelper.GetFilesByField(lm371.Get<string>("COL_49"));
            imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_371&tag_code=report_img&row_id=" + row_id;
            imgs["delete_img_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
            imgs["row_id"] = row_id;
            imgs["table_name"] = "UT_371";
            imgs["tag_code"] = "report_img";
            imgs["btn_id"] = "uploader_img_" + row_id;
            imgs["field_text"] = "COL_49";
            if(show_type == "SHOW")
            {
                imgs["read_only"] = true;
            }


            sm["imgs"] = imgs;


            SModel annexs = new SModel();

            annexs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_50"));
            annexs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_371&tag_code=report_annex&row_id=" + row_id;
            annexs["delete_annex_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
            annexs["btn_id"] = "uploader_annex_" + row_id;
            annexs["row_id"] = row_id;
            annexs["table_name"] = "UT_371";
            annexs["tag_code"] = "report_annex";
            annexs["field_text"] = "COL_50";
            if (show_type == "SHOW")
            {
                annexs["read_only"] = true;
            }

            sm["annexs"] = annexs;



            return sm.ToJson();
            

        }



    }
}