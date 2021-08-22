using App.BizCommon;
using App.InfoGrid2.WF.Bll;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// ReimburDetaHandle 的摘要说明
    /// </summary>
    public class ReimburDetaHandle : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpResult result = null;

            try
            {
                string action = WebUtil.FormTrimUpper("action");

                switch (action)
                {

                    case "INIT_DATA_REIMBUR_DETA":
                        result = InitDataReimburDeta();
                        break;
                }
            }
            catch (Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error("哦噢，出错了！");

            }

            context.Response.Write(result);
        }



        HttpResult InitDataReimburDeta()
        {
            int id = WebUtil.FormInt("r_id");

            SModel result = new SModel();

            SModelList reim_detas = GetReimDetasObj(id);

            decimal money_total = 0;

            foreach (var item in reim_detas)
            {
                money_total += item.GetDecimal("COL_17");
            }

            result["money_total"] = money_total;
            result["reim_detas"] = reim_detas;
            result["reim_types"] = GetReimTypeArray();

            return HttpResult.Success(result);
        }


        /// <summary>
        /// 获取报销明细数据集合
        /// </summary>
        /// <returns></returns>
        public SModelList GetReimDetasObj(int id)
        {

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_347");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.And("COL_24", id);

            lmFilter.Fields = new string[] { "ROW_IDENTITY_ID", "COL_8", "COL_17", "COL_40", "COL_27", "COL_22", "COL_23", "COL_12", "COL_13", "COL_14", "COL_15", "COL_28", "COL_29" };

            SModelList sm_reim_detas = decipher.GetSModelList(lmFilter);

            //如果报销明细没有数据，就默认添加一条
            if (sm_reim_detas.Count == 0)
            {

                LModel lm347 = new LModel("UT_347");

                lm347["ROW_SID"] = 0;
                lm347["ROW_DATE_CREATE"] = lm347["ROW_DATE_UPDATE"] = lm347["BIZ_CREATE_DATE"] = lm347["BIZ_UPDATE_DATE"] = DateTime.Now;
                lm347["COL_24"] = id;
                lm347["ROW_AUTHOR_USER_CODE"] = user_code;
                lm347["BIZ_CREATE_USER_CODE"] = user_code;
                lm347["BIZ_CREATE_USER_TEXT"] = user.LoginName;

                decipher.InsertModel(lm347);

                SModel sm = new SModel();

                lm347.CopyTo(sm);

                sm_reim_detas.Add(sm);

            }

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


                sm["imgs"] = imgs;


                SModel annexs = new SModel();

                annexs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_23"));
                annexs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_347&tag_code=reim_deta_annex&row_id=" + row_id;
                annexs["delete_annex_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
                annexs["btn_id"] = "uploader_annex_" + row_id;
                annexs["row_id"] = row_id;
                annexs["table_name"] = "UT_347";
                annexs["tag_code"] = "reim_deta_annex";
                annexs["field_text"] = "COL_23";

                sm["annexs"] = annexs;
            }

            return sm_reim_detas;

        }



        /// <summary>
        /// 获取费用类型数据
        /// </summary>
        /// <returns></returns>
        public SModelList GetReimTypeArray()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_273");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 2);
            lmFilter.Fields = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "COL_6" };

            SModelList sm_reim_types = decipher.GetSModelList(lmFilter);

            return sm_reim_types;

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}