using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
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

namespace App.InfoGrid2.Wxhb.Handlers
{
    /// <summary>
    /// 处理关于地图方面的函数
    /// </summary>
    public class MapHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpResult result = null;


            EcUserState userState = EcContext.Current.User;

            if (!userState.Roles.Exist("WX"))
            {
                context.Response.Write(HttpResult.Error("哈哈哈，写错了！"));
                return;
            }

            try
            {

                string action = WebUtil.FormTrimUpper("action");

                switch (action)
                {

                    case "EDIT_SHOP_ADDRES":
                        result = EditShopAddres();
                        break;
                    case "EDIT_ADVE_SHOP_ADDRES":
                        result = EditAdveShopAddres();
                        break;
                    case "NEW_ADVE_HB_ADDRES":
                        result = NewAdveHbAddres();
                        break;
                    case "EDIT_ADVE_HB_ADDRES":
                        result = EditAdveHbAddres();
                        break;
                    case "GET_ALL_HB_ADDRESS":
                        result = GetAllHbAddress();
                        break;
                    default:
                        result = HttpResult.Error("哦噢，写错了吧！");
                        break;
                }

            }
            catch (Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error(ex.Message);

            }


            context.Response.Write(result);
            
        }

        
        /// <summary>
        /// 编辑商户的店铺位置
        /// </summary>
        /// <returns></returns>
        HttpResult EditShopAddres()
        {

            decimal lat = WebUtil.FormDecimal("lat");
            decimal lon = WebUtil.FormDecimal("lon");
            string address = WebUtil.FormTrim("address");



            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }


            wx_account.COL_25 = lat;
            wx_account.COL_24 = lon;
            wx_account.COL_27 = address;
            wx_account.ROW_DATE_UPDATE = DateTime.Now;


            decipher.UpdateModelProps(wx_account, "COL_24", "COL_25", "COL_27", "ROW_DATE_UPDATE");


            return HttpResult.SuccessMsg("保存店铺地址成功了！");
            

        }

        /// <summary>
        /// 编辑广告店铺地址
        /// </summary>
        /// <returns></returns>
        HttpResult EditAdveShopAddres()
        {
            decimal lat = WebUtil.FormDecimal("lat");
            decimal lon = WebUtil.FormDecimal("lon");
            string address = WebUtil.FormTrim("address");
            int row_id = WebUtil.FormInt("row_id");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel lm002 = decipher.GetModelByPk("UT_002", row_id);
            lm002["COL_5"] = lat;
            lm002["COL_4"] = lon;
            lm002["COL_6"] = address;
            lm002["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm002, "COL_5", "COL_4", "COL_6", "ROW_DATE_UPDATE");

            return HttpResult.SuccessMsg("保存广告店铺地址成功了！");
        }

        /// <summary>
        /// 新增一个广告下面的红包地址
        /// </summary>
        /// <returns></returns>
        HttpResult NewAdveHbAddres()
        {

            decimal lat = WebUtil.FormDecimal("lat");
            decimal lon = WebUtil.FormDecimal("lon");
            string address = WebUtil.FormTrim("address");
            int row_id = WebUtil.FormInt("row_id");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel lm002 = decipher.GetModelByPk("UT_002", row_id);

            EcUserState userState = EcContext.Current.User;

            LModel lm010 = new LModel("UT_010");
            lm010["PK_A_HB_CODE"] = BillIdentityMgr.NewCodeForDay("A_HB_CODE", "A", 6);
            lm010["FK_ADVE_CODE"] = lm002["PK_ADVE_CODE"];
            lm010["FK_W_CODE"] = userState.LoginID;
            lm010["W_NICKNAME"] = userState.LoginName;
            lm010["HB_LONGITUDE"] = lon;
            lm010["HB_LATITUDE"] = lat;
            lm010["HB_ADDRESS"] = address;

            decipher.InsertModel(lm010);

            return HttpResult.SuccessMsg("新增一个红包地址成功了！");
            
        }


        /// <summary>
        /// 编辑 广告下面的红包地址
        /// </summary>
        /// <returns></returns>
        HttpResult EditAdveHbAddres()
        {

            decimal lat = WebUtil.FormDecimal("lat");
            decimal lon = WebUtil.FormDecimal("lon");
            string address = WebUtil.FormTrim("address");
            int row_id = WebUtil.FormInt("row_id");


            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel lm010 = decipher.GetModelByPk("UT_010", row_id);

            lm010["HB_LONGITUDE"] = lon;
            lm010["HB_LATITUDE"] = lat;
            lm010["HB_ADDRESS"] = address;
            lm010["ROW_DATE_UPDATE"] = DateTime.Now;


            decipher.UpdateModelProps(lm010, "HB_LONGITUDE", "HB_LATITUDE", "HB_ADDRESS", "ROW_DATE_UPDATE");


            return HttpResult.SuccessMsg("编辑红包地址成功了！");


        }

        /// <summary>
        /// 获取红包地址集合
        /// </summary>
        /// <returns></returns>
        HttpResult GetAllHbAddress()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter("UT_010");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 2);


            SModelList sm_data = decipher.GetSModelList(lmFilter);

            return HttpResult.Success(sm_data);

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