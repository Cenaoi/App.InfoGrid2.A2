using App.BizCommon;
using App.InfoGrid2.Model.Mall;
using App.InfoGrid2.Model.WeChat;
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

namespace App.InfoGrid2.Mall.Handlers
{
    /// <summary>
    /// 微信支付后回调处理类
    /// </summary>
    public class WxCallback : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        public void ProcessRequest(HttpContext context)
        {



            string ipaddress = context.Request.UserHostAddress;

            log.Debug("访问者的IP:" + ipaddress);


            CallbackResult cbr = null;


            try
            {

                StreamReader reader = new StreamReader(context.Request.InputStream);

                string xmlStr = reader.ReadToEnd();

                log.Debug("获取post流里面的数据：" + xmlStr);

                byte[] array = Encoding.UTF8.GetBytes(xmlStr);

                MemoryStream stream = new MemoryStream(array);

                XmlSerializer xmlSearializer = new XmlSerializer(typeof(CallbackResult), new XmlRootAttribute("xml"));

                cbr = (CallbackResult)xmlSearializer.Deserialize(stream);

            }
            catch (Exception ex)
            {
                log.Error("解析微信那边传过来的数据出错了！", ex);
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(MALL_ORDER));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            //微信支付流水号
            lmFilter.And("ORDER_NO", cbr.out_trade_no);

            MALL_ORDER order = decipher.SelectToOneModel<MALL_ORDER>(lmFilter);

            if (order == null)
            {

                context.Response.Write("FAIL");
                log.Error("找不到订单数据");

                return;
            }

            if (cbr.result_code == "SUCCESS")
            {

                DateTime tiem_edn;

                //2016 07 01 162216
                DateTime.TryParseExact(cbr.time_end, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out tiem_edn);

                order.BIZ_SID = 2;
                order.PAY_DATE = tiem_edn;
                order.ROW_DATE_UPDATE = DateTime.Now;
                order.W_P_ORDER_NO = cbr.transaction_id;

                decipher.UpdateModelProps(order, "PAY_SID", "PAY_DATE", "ROW_DATE_UPDATE", "W_P_ORDER_NO");



            }
            else if (cbr.result_code == "FAIL")
            {

                log.Error("哦噢，微信支付失败了！");


            }
            else
            {
                context.Response.Write("FAIL");

                return;
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