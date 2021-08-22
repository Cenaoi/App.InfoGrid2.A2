using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Mall.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Model.WeChat;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Mall.Handlers
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            string action = WebUtil.FormTrim("action");

            // 这是同意跨域获取数据
            context.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:8080");
            context.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            HttpResult result = null;
            try
            {

                switch (action)
                {

                    case "WELCOME":
                        result = Welcome();
                        break;
                    case "LOGIN_BTN":
                        result = LoginBtn();
                        break;
                    case "IS_LOGIN":
                        break;
                    default:
                        result = HttpResult.Error("哦噢，没有这个接口哦");
                        break;

                }

            }catch(Exception ex)
            {
                log.Error(ex);
                result = HttpResult.Error(ex.Message);
            }
            context.Response.Write(result.ToJson());
        }

        /// <summary>
        /// 欢迎界面的事件
        /// </summary>
        /// <returns></returns>
        HttpResult Welcome()
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            //界面上的GUID
            Guid view_guid = WebUtil.FormGuid("v_id");

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

            var key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "QLM", "在微信公共API中的关键字");


            NameValueCollection nv = new NameValueCollection();

            nv["view_guid"] = view_guid.ToString();

            nv["key"] = key;

            string result = string.Empty;

            using (WebClient wc = new WebClient())
            {

                byte[] json = wc.UploadValues(url + "/GetUserByVId.ashx", "POST", nv);

                result = Encoding.UTF8.GetString(json);

                log.Info("从微信公共平台获取的openid：" + result);

            }

            HttpResult sm_result = HttpResult.ParseJson(result);

            if (!sm_result.success)
            {
                return sm_result;

            }



            BizFilter filter = new BizFilter(typeof(WX_ACCOUNT));
            filter.And("W_OPENID", sm_result.data["openid"]);

            WX_ACCOUNT account = decipher.SelectToOneModel<WX_ACCOUNT>(filter);

            if (account == null)
            {

                SModel data = sm_result.Get<SModel>("data");


                account = new WX_ACCOUNT();
                account["PK_W_CODE"] = BillIdentityMgr.NewCodeForDay("WECHAT_CODE", "W");

                account["ROW_DATE_CREATE"] = account["ROW_DATE_UPDATE"] = DateTime.Now;
                account["W_OPENID"] = sm_result.data["openid"];


                if (data.Get<int>("subscribe") == 1)
                {
                    account["W_ADDRESS"] = sm_result.data["country"] + sm_result.data["province"] + sm_result["city"];
                    account["SEX"] = sm_result.data["sex"] == 2 ? "女" : "男";
                    account["HEAD_IMG_URL"] = sm_result.data["headimgurl"];
                    account["W_NICKNAME"] = sm_result.data["nickname"];
                    account["IS_ENABLE"] = true;

                }

                decipher.InsertModel(account);

            }


            BizFilter filterLogin = new BizFilter("UT_071");

            filterLogin.And("LOGIN_NAME", account.LOGIN_NAME);
            filterLogin.And("LOGIN_PASS", account.LOGIN_PASS);


            LModel lm071 = decipher.GetModel(filterLogin);

            if(lm071 == null)
            {
                HttpResult  http_result = HttpResult.Error("登录名或密码错误");
                http_result.data = new { user_id = account.WX_ACCOUNT_ID };

                return http_result;
            }


            EcUserState user = EcContext.Current.User;

            user.Clear();
            user.ExpandPropertys.Clear();

            user.Identity = account.WX_ACCOUNT_ID;

            user.LoginID = account.PK_W_CODE;
            user.LoginName = account.W_NICKNAME;

            user.IsVirtual = false;

            user.ExpandPropertys["CLIENT_ID"] = lm071.GetPk().ToString();

            user.Roles.Clear();

            user.Roles.Add("WX");



            DataTable dt = new DataTable();

                


            return HttpResult.SuccessMsg("自动登录成功了");


        }


        /// <summary>
        /// 登录按钮点击事件
        /// </summary>
        /// <returns></returns>
        HttpResult LoginBtn()
        {


            string login_name = WebUtil.FormTrim("login_name");

            string login_pass = WebUtil.FormTrim("login_pass");

            int id = WebUtil.FormInt("id");


            //检查是否为空
            if (string.IsNullOrWhiteSpace(login_name) || string.IsNullOrWhiteSpace(login_pass))
            {

                return HttpResult.Error("登录账号或者登录密码不能为空");

            }


            log.Info($"login_name={login_name},login_pass={login_pass}");



            DbDecipher decipher = ModelAction.OpenDecipher();


            BizFilter filterLogin = new BizFilter("UT_071");

            filterLogin.And("LOGIN_NAME", login_name);
            filterLogin.And("LOGIN_PASS", login_pass);

            LModel lm071 = decipher.GetModel(filterLogin);


            if(lm071 == null)
            {
                return HttpResult.Error("登录账号或者登录密码不正确！");
            }





            WX_ACCOUNT account = decipher.SelectModelByPk<WX_ACCOUNT>(id);

            account.LOGIN_NAME = login_name;
            account.LOGIN_PASS = login_pass;
            account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(account, "LOGIN_NAME", "LOGIN_PASS", "ROW_DATE_UPDATE");


            EcUserState user = EcContext.Current.User;

            user.Clear();
            user.ExpandPropertys.Clear();

            user.Identity = account.WX_ACCOUNT_ID;

            user.LoginID = account.PK_W_CODE;
            user.LoginName = account.W_NICKNAME;

            user.IsVirtual = false;

            user.ExpandPropertys["CLIENT_ID"] = lm071.GetPk().ToString();

            user.Roles.Clear();

            user.Roles.Add("WX");




            return HttpResult.SuccessMsg("登录成功了");



        }


        ///// <summary>
        ///// 判断是否已经登录了
        ///// </summary>
        ///// <returns></returns>
        //HttpResult IsLogin()
        //{

        //    if (BusHelper.IsLogin())
        //    {
        //        return HttpResult.SuccessMsg("ok");
        //    }

        //    var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

        //    var key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "QLM", "在微信公共API中的关键字");

        //    var me_domain = GlobelParam.GetValue("ME_DOMAIN", "http://mall.51.yq-ic.com", "自己的域名");

        //    var wx_appid = GlobelParam.GetValue("WX_APPID", "wxde17583f7c1d1359", "公众号的唯一标识");




        //    var red_uri = "";


        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("https://open.weixin.qq.com/connect/oauth2/authorize?");
        //    sb.Append($"appid={wx_appid}&");
        //    sb.Append($"redirect_uri=");

        //    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?";





        //}


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}