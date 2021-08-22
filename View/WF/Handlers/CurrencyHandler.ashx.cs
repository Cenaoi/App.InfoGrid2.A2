using App.BizCommon;
using EC5.IO;
using EC5.Utility.Web;
using EC5.Web.WebAPI;
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
    /// CurrencyHandler 的摘要说明
    /// </summary>
    public class CurrencyHandler : AjaxHandler, IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);

            //如果不执行上面所有函数
            if (!this.IsExecedAjaxMethod)
            {

            }
        }


        /// <summary>
        /// 获取微信js配置对象信息
        /// </summary>
        /// <returns></returns>
        [Ajax("GET_WX_JS_CONFIG")]
        HttpResult GetWxJsConfig()
        {


            string url_1 = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

            bool debug = WebUtil.FormBool("debug");


            NameValueCollection nv = new NameValueCollection();

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

            nv["key"] = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "MTC", "在微信公共API中的关键字");

            nv["url"] = url_1;

            nv["debug"] = debug.ToString();

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



    }
}