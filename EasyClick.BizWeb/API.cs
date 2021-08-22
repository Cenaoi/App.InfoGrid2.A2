using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using EC5.IO;
using EC5.SystemBoard;
using EC5.SystemBoard.EcReflection;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EC5.Web.WebAPI;

namespace EasyClick.BizWeb
{
    /// <summary>
    /// API1 的摘要说明
    /// </summary>
    public class API :IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpResult hResult;

            try
            {
                hResult = ExecFunction();
            }
            catch (Exception ex)
            {
                log.Error("执行 WebApi 错误,", ex);

                hResult = HttpResult.Error(ex.Message);
            }

            if (hResult == null)
            {
                hResult = HttpResult.Error("那个混蛋, 没有给我反馈数据??");
            }

            context.Response.Clear();
            context.Response.Write(hResult.ToJson());
        }


        /// <summary>
        /// 执行函数
        /// </summary>
        /// <returns></returns>
        private HttpResult ExecFunction()
        {

            string bllPath = WebUtil.QueryTrim("__path");// actStr.Substring(0, n);

            string action = WebUtil.QueryTrim("__action");  // actStr.Substring(n + 1);



            EcTypeInfo ecBLL = SysBoardManager.GetBllType(bllPath);

            if (ecBLL == null)
            {
                return HttpResult.Error($"业务对象'{bllPath}'不存在.");
            }
            
            IBllAction bllObject = Activator.CreateInstance(ecBLL.Src) as IBllAction;

            MethodInfo mi = AjaxHandlerManager.GetMethod(bllObject, action);

            if (mi == null)
            {
                return HttpResult.Error($"业务函数'{action}'不存在.");
            }


            HttpResult hResult;

            try
            {
                hResult = mi.Invoke(bllObject, new object[0]) as HttpResult;
            }
            catch (Exception ex)
            {
                Exception inEx = ex.InnerException;

                if (inEx != null)
                {
                    hResult = HttpResult.Error("执行函数错误." + inEx.Message);

                    log.Error(inEx);
                }
                else
                {
                    hResult = HttpResult.Error("执行函数错误." + ex.Message);

                    log.Error(ex);
                }
            }

            return hResult;
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
