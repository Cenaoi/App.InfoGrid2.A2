using App.BizCommon;
using EC5.IO;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Wxhb.Handlers
{
    /// <summary>
    ///  消息处理函数
    /// </summary>
    public class MsgHandler : IHttpHandler, IRequiresSessionState
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

                    case "GET_ADVE_HISTORY_MSGS":
                        result = GetAdveHistoryMsgs();
                        break;
                    default:
                        result = HttpResult.Error("哦噢，写错了！");
                        break;

                }


            }
            catch (Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error(ex.Message);

            }

            

            context.Response.Write(result.ToJson());

        }


        /// <summary>
        /// 根据编码 获取广告下面的历史消息
        /// </summary>
        /// <returns></returns>
        HttpResult GetAdveHistoryMsgs()
        {

            string adve_code = WebUtil.FormTrim("adve_code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_008");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_ADVE_CODE", adve_code);

            SModelList sm_data = decipher.GetSModelList(lmFilter);

            return HttpResult.Success(sm_data);
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