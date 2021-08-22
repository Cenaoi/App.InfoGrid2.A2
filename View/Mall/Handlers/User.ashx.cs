using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Mall.Handlers
{
    /// <summary>
    /// User 的摘要说明
    /// </summary>
    public class User : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            string action = WebUtil.FormTrim("action");

            HttpResult result = null;

            try
            {

                switch (action)
                {

                    case "GET_WX_USER":
                        result = GetWxUser();
                        break;
                    default:
                        result = HttpResult.Error("哦噢，没有这个接口哦");
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
        /// 获取微信用户对象
        /// </summary>
        /// <returns></returns>
        HttpResult GetWxUser()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;


            WX_ACCOUNT account = decipher.SelectModelByPk<WX_ACCOUNT>(user.Identity);


            return HttpResult.Success(account);



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