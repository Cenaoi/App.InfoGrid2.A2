using App.BizCommon;
using App.InfoGrid2.JF.WeChat.Model;
using App.InfoGrid2.Wxhb.Bll;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml.Serialization;

namespace App.InfoGrid2.Wxhb.Handlers
{
    /// <summary>
    /// 微信付款回调通知处理函数
    /// </summary>
    public class WxNotifyHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string ipaddress = context.Request.UserHostAddress;

            log.Debug("访问者的IP:" + ipaddress);


            CallbackResult cbr = null;


            try
            {

                StreamReader reader = new StreamReader(context.Request.InputStream);

                string xmlStr = reader.ReadToEnd();

                log.Debug("获取post流里面的数据：" + xmlStr);

                cbr = WeChatUtil.GetResultObj<CallbackResult>(xmlStr);

                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter lmFilter = new LightModelFilter("UT_001");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("COL_3", cbr.out_trade_no);
                lmFilter.And("BIZ_SID", 0);

                LModel lm001 = decipher.GetModel(lmFilter);

                if (lm001 == null)
                {

                    log.Error("微信支付成功回调回来找不到这个订单！");

                    context.Response.Write("SUCCESS");

                    return;


                }

                lm001["BIZ_SID"] = 2;
                lm001["COL_5"] = cbr.transaction_id;
                lm001["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.UpdateModelProps(lm001, "BIZ_SID", "COL_5", "ROW_DATE_UPDATE");



            }
            catch (Exception ex)
            {
                log.Error("解析微信那边传过来的数据出错了！", ex);
            }

            context.Response.Write("SUCCESS");

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