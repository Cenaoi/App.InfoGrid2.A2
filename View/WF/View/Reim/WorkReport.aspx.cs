using App.BizCommon;
using App.InfoGrid2.WF.Bll;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.WF.View.Reim
{
    public partial class WorkReport : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 获取工作报告数据
        /// </summary>
        /// <returns></returns>
        public string GetWorkReportsArray()
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            LightModelFilter lmFilter = new LightModelFilter("UT_351");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.And("COL_24", id);

            lmFilter.Fields = new string[] { "ROW_IDENTITY_ID", "COL_26", "COL_27" , "COL_22" , "COL_23"};

            SModelList sm_reim_detas = decipher.GetSModelList(lmFilter);


            //如果报销明细没有数据，就默认添加一条
            if (sm_reim_detas.Count == 0)
            {

                LModel lm351 = new LModel("UT_351");

                lm351["ROW_SID"] = 0;
                lm351["ROW_DATE_CREATE"] = lm351["ROW_DATE_UPDATE"] = lm351["BIZ_CREATE_DATE"] = lm351["BIZ_UPDATE_DATE"] = DateTime.Now;
                lm351["ROW_AUTHOR_USER_CODE"] = user_code;
                lm351["BIZ_CREATE_USER_CODE"] = user_code;
                lm351["BIZ_CREATE_USER_TEXT"] = user.LoginName;
                lm351["COL_24"] = id;

                decipher.InsertModel(lm351);

                SModel sm = new SModel();

                lm351.CopyTo(sm);

                sm_reim_detas.Add(sm);

            }

            foreach (var sm in sm_reim_detas)
            {

                int row_id = sm.Get<int>("ROW_IDENTITY_ID");

                SModel imgs = new SModel();

                imgs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_22"));
                imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_351&tag_code=work_report_img&row_id=" + row_id;
                imgs["delete_img_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
                imgs["row_id"] = row_id;
                imgs["table_name"] = "UT_351";
                imgs["tag_code"] = "work_report_img";
                imgs["btn_id"] = "uploader_img_" + row_id;
                imgs["field_text"] = "COL_22";


                sm["imgs"] = imgs;


                SModel annexs = new SModel();

                annexs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_23"));
                annexs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_351&tag_code=work_report_annex&row_id=" + row_id;
                annexs["delete_annex_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
                annexs["btn_id"] = "uploader_annex_" + row_id;
                annexs["row_id"] = row_id;
                annexs["table_name"] = "UT_351";
                annexs["tag_code"] = "work_report_annex";
                annexs["field_text"] = "COL_23";


                sm["annexs"] = annexs;

            }

            return sm_reim_detas.ToJson();


        }

    }
}