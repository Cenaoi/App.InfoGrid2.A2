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
    public partial class ShowReimDeta : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 获取报销明细数据集合
        /// </summary>
        /// <returns></returns>
        public string GetReimDetasObj()
        {

            string fee_code = WebUtil.QueryTrim("fee_code");

            int id = WebUtil.QueryInt("id");

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_347");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0, Logic.GreaterThan);
            lmFilter.And("COL_24", id);


            //电话费（UT_347 COL_15 = 10103）
            //汽车费（UT_347 COL_15 = 10104）
            //招待费（UT_347 COL_15 = 10105）
            //差旅费（UT_347 COL_15 = 10106）

            //费用类型不为空就要过滤
            if (!string.IsNullOrWhiteSpace(fee_code))
            {
                lmFilter.And("COL_15", fee_code);
            }



            lmFilter.Fields = new string[] { "ROW_IDENTITY_ID", "COL_8", "COL_17", "COL_40", "COL_27", "COL_22", "COL_23", "COL_12", "COL_13", "COL_14", "COL_15", "COL_28", "COL_29" };

            SModelList sm_reim_detas = decipher.GetSModelList(lmFilter);


            foreach (var sm in sm_reim_detas)
            {

                int row_id = sm.Get<int>("ROW_IDENTITY_ID");

                sm["COL_8"] = BusHelper.FormatDate(sm.Get<string>("COL_8"));

                SModel imgs = new SModel();

                imgs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_22"));
                imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_347&tag_code=reim_deta_img&row_id=" + row_id;
                imgs["delete_img_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
                imgs["row_id"] = row_id;
                imgs["table_name"] = "UT_347";
                imgs["tag_code"] = "reim_deta_img";
                imgs["btn_id"] = "uploader_img_" + row_id;
                imgs["field_text"] = "COL_22";
                imgs["read_only"] = true;


                sm["imgs"] = imgs;


                SModel annexs = new SModel();

                annexs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_23"));
                annexs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_347&tag_code=reim_deta_annex&row_id=" + row_id;
                annexs["delete_img_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
                annexs["btn_id"] = "uploader_annex_" + row_id;
                annexs["row_id"] = row_id;
                annexs["table_name"] = "UT_347";
                annexs["tag_code"] = "reim_deta_annex";
                annexs["field_text"] = "COL_23";
                annexs["read_only"] = true;

                sm["annexs"] = annexs;
            }

            return sm_reim_detas.ToJson();

        }

    }
}