using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using App.InfoGrid2.Wxhb.Bll;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Wxhb.Handlers
{
    /// <summary>
    /// UserHandler 的摘要说明
    /// </summary>
    public class UserHandler : IHttpHandler, IRequiresSessionState
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

                    case "GET_USER":
                        result = GetUser();
                        break;
                    case "GET_QRCODE":
                        result = GetQrcode();
                        break;
                    case "GET_WX_JS_CONFIG":
                        result = GetWxJsConfig();
                        break;
                    case "GET_RECOMMEND_USERS":
                        result = GetRecommendUsers();
                        break;
                    case "NEW_FOREVER_QRCODE_APPLY":
                        result = NewForeverQrcodeApply();
                        break;
                    case "GET_FOREVER_QRCODE_APPLY":
                        result = GetForeverQrcodeApply();
                        break;
                    case "SUBMIT_FOREVER_QRCODE_APPLY":
                        result = SubmitForeverQrcodeApply();
                        break;
                    case "SUBMIT_WITHDRAWALS":
                        result = SubmitWithdrawals();
                        break;
                    case "GET_WITHDS":
                        result = GetWithds();
                        break;
                    case "GET_BROWSES":
                        result = GetBrowses();
                        break;
                    case "BROWSE_DELETE":
                        result = BrowseDelete();
                        break;
                    case "BROWSE_COLLECTION":
                        result = BrowseCollection();
                        break;
                    default:
                        result = HttpResult.Error("哦噢，写错了吧！");
                        break;
                }

            }catch(Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error(ex.Message);

            }


            context.Response.Write(result);

        }




        /// <summary>
        /// 获取微信js配置对象信息
        /// </summary>
        /// <returns></returns>
        HttpResult GetWxJsConfig()
        {


            string url_1 = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

            log.Info("前端访问的地址："+url_1);
           
            NameValueCollection nv = new NameValueCollection();

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://yq.gzbeih.com/API", "微信公共API地址");

            nv["key"] = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "WXHB", "在微信公共API中的关键字");

            nv["url"] = url_1;

            nv["debug"] = "false";

            using (WebClient wc = new WebClient())
            {
                //这是提交键值对类型的
                byte[] json = wc.UploadValues(url + "/WxJsConfig.ashx", "POST", nv);

                string result = Encoding.Default.GetString(json);

                log.Info(result);





                HttpResult sm_result = HttpResult.ParseJson(result);



                return sm_result;

            }

        }

        


        /// <summary>
        /// 获取微信用户个人专属二维码
        /// </summary>
        /// <returns></returns>
        HttpResult GetQrcode()
        {
            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }


            if(!string.IsNullOrWhiteSpace(wx_account.COL_17) && wx_account.COL_18 > DateTime.Now)
            {
                return HttpResult.Success(new { col_17=wx_account.COL_17,col_18 = wx_account.COL_18.ToString("yyyy-MM-dd")});
            }


            NameValueCollection nv = new NameValueCollection();

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://yq.gzbeih.com/API", "微信公共API地址");


            nv["key"] = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "WXHB", "在微信公共API中的关键字");

            nv["user_id"] =wx_account.WX_ACCOUNT_ID.ToString();

            //看看是否是临时二维码还是永久二维码 
            nv["action"] = "TEMP_QRCODE";

            HttpResult sm_result = null;

            using (WebClient wc = new WebClient())
            {
                //这是提交键值对类型的
                byte[] json = wc.UploadValues(url + "/QrCode.ashx", "POST", nv);

                string result = Encoding.Default.GetString(json);

                sm_result = HttpResult.ParseJson(result);

            }

            if (!sm_result.success)
            {
                return sm_result;

            }

            SModel sm_data = sm_result.data;


            //二维码图片地址
            string qrcode_url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + sm_data.Get<string>("ticket");

            //二维码有效期为30天
            DateTime qrcode_data = DateTime.Now.AddSeconds(sm_data.Get<int>("expire_seconds"));


            wx_account.COL_17 = qrcode_url;
            wx_account.COL_18 = qrcode_data;
            wx_account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(wx_account, "COL_17", "COL_18", "ROW_DATE_UPDATE");

            return HttpResult.Success(new { col_17 = qrcode_url, col_18 = qrcode_data.ToString("yyyy-MM-dd") });

        }


        /// <summary>
        /// 获取微信用户数据
        /// </summary>
        /// <returns></returns>
        HttpResult GetUser()
        {

            EcUserState userState = EcContext.Current.User;



            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }


            SModel sm_data = new SModel();
            sm_data["user_text"] = wx_account.W_NICKNAME;
            sm_data["head_img_url"] = wx_account.HEAD_IMG_URL;
            sm_data["longitude"] = wx_account.W_LONGITUDE;
            sm_data["latitude"] = wx_account.W_LATITUDE;
            sm_data["col_23"] = wx_account.COL_23;
            sm_data["col_20"] = wx_account.COL_20;
            sm_data["col_21"] = wx_account.COL_21;
            sm_data["col_19"] = wx_account.COL_19;
            sm_data["col_1"] = wx_account.COL_1;
            sm_data["col_2"] = wx_account.COL_2;
            sm_data["col_13"] = wx_account.COL_13;
            sm_data["col_16"] = wx_account.COL_16;
            sm_data["col_4"] = wx_account.COL_4;
            sm_data["col_15"] = wx_account.COL_15;
            sm_data["col_27"] = wx_account.COL_27;
            sm_data["col_24"] = wx_account.COL_24;
            sm_data["col_25"] = wx_account.COL_25;
            
            return HttpResult.Success(sm_data);

        }

        /// <summary>
        /// 获取推荐用户集合
        /// </summary>
        /// <returns></returns>
        HttpResult GetRecommendUsers()
        {

            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(WX_ACCOUNT));
            lmFilter.And("ROW_SID",0,Logic.GreaterThanOrEqual);
            lmFilter.And("PARENT_W_CODE_1", userState.LoginID);
            lmFilter.Fields = new string[] { "W_NICKNAME", "HEAD_IMG_URL"};

            SModelList sm_data = decipher.GetSModelList(lmFilter);

            return HttpResult.Success(sm_data);

        }

        /// <summary>
        /// 新建一条永久二维码申请记录
        /// </summary>
        /// <returns></returns>
        HttpResult NewForeverQrcodeApply()
        {



            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }

            if (wx_account.IS_FOREVER_QRCODE)
            {
                return HttpResult.Error("已经是永久二维码了！");
            }


            LightModelFilter lmFilter = new LightModelFilter("UT_006");
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", wx_account.PK_W_CODE);

            log.Info("登录微信的自定义编码："+wx_account.PK_W_CODE);


            LModel lm006 = decipher.GetModel(lmFilter);

            //没有就新增一条记录到数据中
            if (lm006 == null)
            {

                lm006 = new LModel("UT_006");

                lm006["BIZ_SID"] = 0;
                lm006["ROW_DATE_CREATE"] = lm006["ROW_DATE_UPDATE"] = DateTime.Now;
                lm006["FK_W_CODE"] = wx_account.PK_W_CODE;
                lm006["W_NICKNAME"] = wx_account.W_NICKNAME;
                lm006["PK_FQRCODE_CODE"] = BillIdentityMgr.NewCodeForDay("FQRCODE_CODE", "F", 6);

                decipher.InsertModel(lm006);
                
            }
            
            if(lm006.Get<int>("BIZ_SID") == 999)
            {
                return HttpResult.Error("已经是永久二维码了！");
            }

            return HttpResult.Success(new { id=lm006.GetPk()});


        }

        /// <summary>
        /// 根据ID获取永久二维码申请记录
        /// </summary>
        /// <returns></returns>
        HttpResult GetForeverQrcodeApply()
        {

            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState userState = EcContext.Current.User;

            LightModelFilter lmFilter = new LightModelFilter("UT_006");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", userState.LoginID);
            lmFilter.And("ROW_IDENTITY_ID",id);

            SModel sm_006 = decipher.GetSModel(lmFilter);

            if(sm_006 == null)
            {
                return HttpResult.Error("哦噢，找不到数据哦！");
            }

            SModel imgs = new SModel();

            imgs["data"] = BusHelper.GetFilesByField(sm_006.Get<string>("ID_IMG"));
            imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_006&tag_code=forever_qrcode_img&row_id=" + sm_006.Get<int>("ROW_IDENTITY_ID");
            imgs["delete_img_url"] = "/Wxhb/Handlers/UploaderFileHandle.ashx";
            imgs["row_id"] = sm_006.Get<int>("ROW_IDENTITY_ID");
            imgs["table_name"] = "UT_006";
            imgs["tag_code"] = "forever_qrcode_img";
            imgs["btn_id"] = "uploader_img_" + sm_006.Get<int>("ROW_IDENTITY_ID");
            imgs["field_text"] = "ID_IMG";

            sm_006["imgs"] = imgs;

            return HttpResult.Success(sm_006);

        }

        /// <summary>
        /// 提交永久二维码申请
        /// </summary>
        /// <returns></returns>
        HttpResult SubmitForeverQrcodeApply()
        {

            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState userState = EcContext.Current.User;

            LightModelFilter lmFilter = new LightModelFilter("UT_006");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", userState.LoginID);
            lmFilter.And("ROW_IDENTITY_ID", id);

            LModel sm_006 = decipher.GetModel(lmFilter);

            if (sm_006 == null)
            {
                return HttpResult.Error("哦噢，找不到数据哦！");
            }


            if(sm_006.Get<int>("BIZ_SID") > 0)
            {
                return HttpResult.Error("在申请中...");
            }


            sm_006["BIZ_SID"] = 2;
            sm_006["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(sm_006, "BIZ_SID", "ROW_DATE_UPDATE");

            return HttpResult.Success("提交成功！");
            
            
        }


        /// <summary>
        /// 提交提现记录
        /// </summary>
        /// <returns></returns>
        HttpResult SubmitWithdrawals()
        {


            decimal withd_money = WebUtil.FormDecimal("withd_money");

            if (withd_money < 1)
            {

                return HttpResult.Error("提现金额不能小于1元");
            }

            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }


            if(wx_account.COL_16 < withd_money)
            {

                return HttpResult.Error("提现金额不能大于当前账户余额");

            }


            //余额
            decimal bana = wx_account.COL_16 - withd_money;

            LModel lm007 = new LModel("UT_007");

            lm007["FK_W_CODE"] = wx_account.PK_W_CODE;
            lm007["W_NICKNAME"] = wx_account.W_NICKNAME;
            lm007["WITHD_MONEY"] = withd_money;
            lm007["CUR_BALANCE"] = wx_account.COL_16 - withd_money;
            lm007["PK_WITHD_CODE"] = BillIdentityMgr.NewCodeForDay("WITHD_CODE", "W", 6);
            lm007["BIZ_SID"] = 0;

            wx_account.COL_16 = bana;
            wx_account.ROW_DATE_UPDATE = DateTime.Now;


            decipher.BeginTransaction();

           
            try
            {
                decipher.InsertModel(lm007);
                decipher.UpdateModelProps(wx_account, "COL_16", "ROW_DATE_UPDATE");

                decipher.TransactionCommit();

                return HttpResult.Success("提现申请成功！");

            }catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

                throw ex;

            }
            

        }

        /// <summary>
        /// 获取提现集合数据
        /// </summary>
        /// <returns></returns>
        HttpResult GetWithds()
        {


            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }

            LightModelFilter lmFilter = new LightModelFilter("UT_007");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", wx_account.PK_W_CODE);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";
            

            List<LModel> lm_007s = decipher.GetModelList(lmFilter);

            SModelList sm_data = new SModelList();


            foreach(LModel lm_007 in lm_007s)
            {
                SModel sm = new SModel();

                int biz_sid = lm_007.Get<int>("BIZ_SID");

                sm["create_date"] = lm_007["ROW_DATE_CREATE"];

                sm["title"] = "提现金额：" + lm_007.Get<decimal>("WITHD_MONEY").ToString("0.##") +" 元 ";

                if(biz_sid == 0)
                {
                    sm["text"] = "申请中";
                }

                if(biz_sid == 2)
                {
                    sm["text"] = "审核通过";
                }

                if(biz_sid == 1)
                {
                    sm["text"] = "审核不通过";
                }

                sm_data.Add(sm);

            }

            return HttpResult.Success(sm_data);

        }

        /// <summary>
        /// 获取自己浏览过的广告记录
        /// </summary>
        /// <returns></returns>
        HttpResult GetBrowses()
        {

            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("找不到用户数据！");
            }


            LightModelFilter lmFilter = new LightModelFilter("UT_009");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", wx_account.PK_W_CODE);
            lmFilter.TSqlOrderBy = "IS_TOP desc,ROW_DATE_CREATE desc";


            SModelList sm_009s = decipher.GetSModelList(lmFilter);


            return HttpResult.Success(sm_009s);

        }

        /// <summary>
        /// 删除浏览历史记录
        /// </summary>
        /// <returns></returns>
        HttpResult BrowseDelete()
        {

            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_009");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("ROW_IDENTITY_ID", id);

            decipher.UpdateProps(lmFilter, new object[] { "ROW_SID",-3, "ROW_DATE_DELETE", DateTime.Now});

            return HttpResult.Success("删除成功了！");

        }

        /// <summary>
        /// 浏览历史记录收藏事件
        /// </summary>
        /// <returns></returns>
        HttpResult BrowseCollection()
        {

            int id = WebUtil.FormInt("id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm009 = decipher.GetModelByPk("UT_009", id);


            lm009["IS_TOP"] = lm009.Get<bool>("IS_TOP") ? false : true;
            lm009["ROW_DATE_UPDATE"] = DateTime.Now;


            decipher.UpdateModelProps(lm009, "IS_TOP", "ROW_DATE_UPDATE");

            return HttpResult.Success("收藏浏览历史成功了！");
            

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