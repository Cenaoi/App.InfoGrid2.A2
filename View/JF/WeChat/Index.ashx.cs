using App.BizCommon;
using App.InfoGrid2.Model.JF;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.JF.WeChat
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : IHttpHandler,IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {


            try
            {
                InitData(context);

            }catch(Exception ex)
            {
                log.Error(ex);
            }
            
            
        }



        void InitData(HttpContext context)
        {


            #region  获取用户Cookie编码


            HttpCookie cookie = context.Request.Cookies["USER_GUID"];

            if (cookie != null)
            {



                Guid cookie_code;
                //判断Cookie传上来的Guid是否是真的Guid
                if (Guid.TryParse(cookie.Value, out cookie_code))
                {


                    DbDecipher decipher = ModelAction.OpenDecipher();

                    LightModelFilter lmFilter = new LightModelFilter(typeof(ES_W_ACCOUNT));
                    lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    lmFilter.And("W_GUID", cookie_code);
                    lmFilter.And("GUID_LIMIT_DATE", DateTime.Now, Logic.GreaterThan);

                    ES_W_ACCOUNT account = decipher.SelectToOneModel<ES_W_ACCOUNT>(lmFilter);

                    if (account != null)
                    {

                        account.GUID_LIMIT_DATE = DateTime.Now.AddDays(3);
                        account.W_GUID = Guid.NewGuid();
                        account.ROW_DATE_UPDATE = DateTime.Now;
                        decipher.UpdateModelProps(account, "GUID_LIMIT_DATE", "W_GUID", "ROW_DATE_UPDATE");

                        log.Debug($"用Cooike里面的Guid【{account.W_GUID}】直接登录的！");

                        context.Response.Redirect("/JF/View/Welcome.aspx?Guid=" + account.W_GUID.ToString());

                        return;

                    }

                }



            }






            #endregion


            //拿简单数据
            string domain_name = BizCommon.GlobelParam.GetValue<string>("HF_DOMAIN_NAME", "wechat.gzbeih.com", "话费域名加端口");

            //公众号唯一标示
            string appid = "wxfa3fa4c793ea10cc";

            //微信授权完成或失败调用的回调页面路径
            string redirectUrl = HttpContext.Current.Server.UrlEncode($"http://{domain_name}/WeChat/WxLoginHandler.ashx");


            //上级编码
            string parent_code = WebUtil.QueryTrim("parent_code");

            string state = $"JF|{parent_code}";

            string url = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={appid}&redirect_uri={redirectUrl}&response_type=code&scope=snsapi_userinfo&state={state}#wechat_redirect";

            log.Debug("微信授权登录地址：" + url);

            log.Debug("回调路径：" + $"http://{domain_name}/WeChat/WxLoginHandler.ashx");



            context.Response.Redirect(url,true);


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