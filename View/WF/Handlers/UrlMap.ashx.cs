using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Model.WeChat;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
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
    /// UrlMap 的摘要说明
    /// </summary>
    public class UrlMap : IHttpHandler,IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {

            string view_code = WebUtil.QueryTrim("view_code");

            //界面上的GUID
            Guid view_guid = WebUtil.QueryGuid("v_id");

            //传过来的ID
            int id = WebUtil.QueryInt("id");


            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

            NameValueCollection nv = new NameValueCollection();

            nv["view_guid"] = view_guid.ToString();

            string result = string.Empty;

            using (WebClient wc = new WebClient())
            {

                byte[] json = wc.UploadValues(url + "/GetOpenid.ashx", "POST", nv);

                result = Encoding.UTF8.GetString(json);

                log.Info("从微信公共平台获取的openid：" + result);

            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            HttpResult sm_result = HttpResult.ParseJson(result);

            BizFilter filter = new BizFilter(typeof(WX_ACCOUNT));
            filter.And("W_OPENID", sm_result.data["openid"]);

            WX_ACCOUNT lm002 = decipher.SelectToOneModel<WX_ACCOUNT>(filter);

            if (lm002 == null)
            {
                throw new Exception("没有登录");

            }


            if(StringUtil.IsBlank(lm002.LOGIN_NAME) || StringUtil.IsBlank(lm002.LOGIN_PASS))
            {
                throw new Exception("登录账号或者密码为空");
            }


            LoginMgr login_mrg = new LoginMgr();

            bool flag = login_mrg.Login(lm002.LOGIN_NAME, lm002.LOGIN_PASS);

            login_mrg.GetUserByLoginName(lm002.LOGIN_NAME);

            //准备跳转的地址
            string re_url = "/App/InfoGrid2/WF/View/Home.aspx";

            //这是 月度报销  
            if (view_code == "reim")
            {

                re_url = "/App/InfoGrid2/WF/View/Reim/ShowReimbur.aspx?reim_id="+id;
                 

            }

            if(view_code == "report")
            {

                re_url = "/App/InfoGrid2/WF/View/Report/ReportFlow.aspx?report_id=" + id;

            }


            context.Response.Redirect(re_url);


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