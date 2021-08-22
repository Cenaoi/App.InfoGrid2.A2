using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
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
    /// 登录用的函数类
    /// </summary>
    public class LoginHandler : IHttpHandler,IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            string action = WebUtil.FormTrimUpper("action");


            HttpResult result = null;


            try
            {

                switch (action)
                {

                    case "AUTO_LOGIN":
                        result = AutoLogin();
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
        /// 自动登录 根据界面上的GUID来登录
        /// </summary>
        /// <returns></returns>
        HttpResult AutoLogin()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (user.Roles.Exist("WX"))
            {



                WX_ACCOUNT old_account = decipher.SelectModelByPk<WX_ACCOUNT>(user.Identity);

                if (old_account != null)
                {

                    return HttpResult.SuccessMsg("自动登录成功了！");

                }

            }


            //界面上的GUID
            Guid view_guid = WebUtil.FormGuid("v_id");

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://yq.gzbeih.com/API", "微信公共API地址");

            NameValueCollection nv = new NameValueCollection();

            nv["view_guid"] = view_guid.ToString();

            string result = string.Empty;

            using (WebClient wc = new WebClient())
            {

                byte[] json = wc.UploadValues(url + "/GetOpenid.ashx", "POST", nv);

                result = Encoding.UTF8.GetString(json);

                log.Info("从微信公共平台获取的openid：" + result);

            }

            HttpResult sm_result = HttpResult.ParseJson(result);

            if (!sm_result.success)
            {
                return HttpResult.Error("");

            }


     


            BizFilter filter = new BizFilter(typeof(WX_ACCOUNT));
            filter.And("W_OPENID", sm_result.data["openid"]);


            WX_ACCOUNT account = decipher.SelectToOneModel<WX_ACCOUNT>(filter);


            if (account == null)
            {

                throw new Exception("找不到微信用户数据！");

            }


            EcUserState userState = EcContext.Current.User;

            //判断登录的和微信传上来的是否是同一个用户
            if (userState.LoginID != account.PK_W_CODE)
            {
                userState.Clear();
                userState.ExpandPropertys.Clear();

                userState.Identity = account.WX_ACCOUNT_ID;
                userState.LoginID = account.PK_W_CODE;
                userState.LoginName = account.W_NICKNAME;


                userState.IsVirtual = false;

                userState.Roles.Clear();
                userState.Roles.Add("WX");

            }

           
            return HttpResult.SuccessMsg("自动登录成功了！");

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