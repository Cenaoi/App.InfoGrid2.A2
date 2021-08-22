using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Model.WeChat;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using EC5.Web.WebAPI;
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

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// 升级版本的登录处理类
    /// </summary>
    public class LoginV1 : AjaxHandler, IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoginV1()
        {
            this.ActionMedhod = AjaxRequestMethod.Form;
            this.ActionChange = AjaxActionChange.UpperCase;
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);

            if (this.IsExecedAjaxMethod)
            {
                return;
            }


            string action = WebUtil.FormTrimUpper("action");

            HttpResult result = HttpResult.Error("哦噢,,没有这个接口哦！action=" + action);

            HttpResponse response = context.Response;
            response.ContentType = "text/plain";
            response.Clear();
            response.Write(result);
        }


        /// <summary>
        /// 自动登录 根据界面上的GUID来登录
        /// </summary>
        /// <returns></returns>
        [Ajax("AUTO_LOGIN")]
        public HttpResult AutoLogin()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            //界面上的GUID
            Guid view_guid = WebUtil.FormGuid("v_id");

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

            var key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "MTC", "在微信公共API中的关键字");


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

            WX_ACCOUNT lm002 = decipher.SelectToOneModel<WX_ACCOUNT>(filter);

            if (lm002 == null)
            {

                SModel data = sm_result.Get<SModel>("data");


                lm002 = new WX_ACCOUNT();
                lm002["PK_W_CODE"] = BillIdentityMgr.NewCodeForDay("WECHAT_CODE", "W");

                lm002["ROW_DATE_CREATE"] = lm002["ROW_DATE_UPDATE"] = DateTime.Now;
                lm002["W_OPENID"] = sm_result.data["openid"];


                if (data.Get<int>("subscribe") == 1)
                {
                    lm002["W_ADDRESS"] = sm_result.data["country"] + sm_result.data["province"] + sm_result["city"];
                    lm002["SEX"] = sm_result.data["sex"] == "2" ? "女" : "男";
                    lm002["HEAD_IMG_URL"] = sm_result.data["headimgurl"];
                    lm002["W_NICKNAME"] = sm_result.data["nickname"];
                    
                    lm002.IS_ENABLE = true;
                }

                decipher.InsertModel(lm002);

                return HttpResult.Error("没有登录");

            }


            if(string.IsNullOrWhiteSpace(lm002.LOGIN_NAME) || string.IsNullOrWhiteSpace(lm002.LOGIN_PASS))
            {
                return HttpResult.Error("登录名或密码为空");
            }
            
            LoginMgr login_mrg = new LoginMgr();


            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            lmFilter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("LOGIN_NAME", lm002.LOGIN_NAME);
            lmFilter.And("LOGIN_PASS", lm002.LOGIN_PASS);


            SEC_LOGIN_ACCOUNT account = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(lmFilter);


            if (account == null)
            {

                return HttpResult.Error("登录名或密码不正确！");

            }



            login_mrg.GetUserByLoginName(lm002.LOGIN_NAME);


            account.W_CODE = lm002.PK_W_CODE;
            account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(account, "W_CODE", "ROW_DATE_UPDATE");

               
            return HttpResult.SuccessMsg("自动登录成功了！");

        }

        /// <summary>
        /// 登录界面的登录函数
        /// </summary>
        /// <returns></returns>
        [Ajax("LOGIN")]
        public HttpResult Login()
        {

            string login_name = WebUtil.Form("login_name");

            string login_pass = WebUtil.Form("login_pass");

            Guid v_id = WebUtil.FormGuid("v_id");

            if (string.IsNullOrWhiteSpace(login_name))
            {
                return HttpResult.Error("登录名称不能为空！");
            }


            if (string.IsNullOrWhiteSpace(login_pass))
            {

                return HttpResult.Error("登录密码不能为空！");

            }


            LoginMgr login_mrg = new LoginMgr();

            bool flag = login_mrg.Login(login_name, login_pass);

            if (!flag)
            {

                return HttpResult.Error("登录名或密码不正确！");

            }

            login_mrg.GetUserByLoginName(login_name);

            EcUserState user = EcContext.Current.User;

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

            NameValueCollection nv = new NameValueCollection();
      
            nv["view_guid"] = v_id.ToString();

            string result = string.Empty;

            using (WebClient wc = new WebClient())
            {

                byte[] json = wc.UploadValues(url + "/GetOpenid.ashx", "POST", nv);

                result = Encoding.UTF8.GetString(json);

                log.Info("从微信公共平台获取的openid：" + result);

            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            HttpResult sm_result = HttpResult.ParseJson(result);

            //sm_result.data["openid"] = "ojfl5wn0RWBujLovoABKS0-BrWLk"; 测试用的

            BizFilter filter = new BizFilter(typeof(WX_ACCOUNT));
            filter.And("W_OPENID", sm_result.data["openid"]);

            WX_ACCOUNT wx = decipher.SelectToOneModel<WX_ACCOUNT>(filter);

            if(wx == null)
            {
                return HttpResult.Error("没有关注");
            }

            SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user.Identity);

            account.W_CODE = wx.PK_W_CODE;
            account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(account, "W_CODE", "ROW_DATE_UPDATE");


            wx.LOGIN_NAME = login_name;
            wx.LOGIN_PASS = login_pass;
            wx.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(wx, "LOGIN_NAME", "LOGIN_PASS", "ROW_DATE_UPDATE");




            return HttpResult.SuccessMsg("登录成功了！");

        }

        /// <summary>
        /// 注销账号按钮事件
        /// </summary>
        /// <returns></returns>
        [Ajax("LOGOUT")]
        public HttpResult Logout()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user.Identity);

            BizFilter filter = new BizFilter(typeof(WX_ACCOUNT));
            filter.And("PK_W_CODE", account.W_CODE);


            WX_ACCOUNT wx_account = decipher.SelectToOneModel<WX_ACCOUNT>(filter);

            wx_account.LOGIN_NAME = "";
            wx_account.LOGIN_PASS = "";
            wx_account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(wx_account, "LOGIN_NAME", "LOGIN_PASS", "ROW_DATE_UPDATE");

            account.W_CODE = "";
            account.ROW_DATE_UPDATE = DateTime.Now;


            decipher.UpdateModelProps(account, "W_CODE", "ROW_DATE_UPDATE");


            user.Clear();
            user.Roles.Clear();

            return HttpResult.SuccessMsg("注销账号成功");




        }



    }
}