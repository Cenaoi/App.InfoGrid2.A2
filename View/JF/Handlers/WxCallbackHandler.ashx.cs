using App.BizCommon;
using App.InfoGrid2.JF.WeChat.Model;
using App.InfoGrid2.Model.JF;
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

namespace App.InfoGrid2.JF.Handlers
{
    /// <summary>
    /// 微信支付回调处理类
    /// </summary>
    public class WxCallbackHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        public void ProcessRequest(HttpContext context)
        {



            string ipaddress = context.Request.UserHostAddress;

            log.Debug("访问者的IP:" + ipaddress);


            Wx();



        }

        /// <summary>
        /// 微信支付完成后的回调函数
        /// </summary>
        void Wx()
        {

            HttpContext context = HttpContext.Current;


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

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_ORDER));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            //微信支付流水号
            lmFilter.And("W_P_SERIAL_CODE", cbr.out_trade_no);
            lmFilter.And("W_OPENID", cbr.openid);

            ES_ORDER order = decipher.SelectToOneModel<ES_ORDER>(lmFilter);

            if (order == null)
            {

                context.Response.Write("FAIL");

                return;
            }

            if (cbr.result_code == "SUCCESS")
            {
                try
                {

                    log.Debug("微信支付成功了！");

                    DateTime tiem_edn;

                    //2016 07 01 162216
                    DateTime.TryParseExact(cbr.time_end, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out tiem_edn);

                    order.PAY_SID = 2;
                    order.PAY_DATE = tiem_edn;
                    order.ROW_DATE_UPDATE = DateTime.Now;
                    order.W_P_ORDER_NO = cbr.transaction_id;

                    decipher.UpdateModelProps(order, "PAY_SID", "PAY_DATE", "ROW_DATE_UPDATE", "W_P_ORDER_NO");


                }
                catch (Exception ex)
                {
                    log.Error(ex);

                }


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