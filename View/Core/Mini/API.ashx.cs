using EC5.IO;
using EC5.SystemBoard;
using EC5.SystemBoard.EcReflection;
using EC5.Utility.Web;
using EC5.Web.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Core.Mini
{
    /// <summary>
    /// API1 的摘要说明
    /// </summary>
    public class API : IHttpHandler, IRequiresSessionState
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


            string typePath = "App" + bllPath.Replace("/", ".");

            Type curType = Type.GetType(typePath);

            MethodInfo mi = null;

            object bllObject = null;

            if (curType != null)
            {
                bllObject = Activator.CreateInstance(curType);

                mi = AjaxHandlerManager.GetMethod(curType, action);
            }
            else
            {

                EcTypeInfo ecBLL = SysBoardManager.GetBllType(bllPath);

                if (ecBLL == null)
                {
                    return HttpResult.Error($"业务对象'{bllPath}'不存在.");
                }

                bllObject = Activator.CreateInstance(curType);

                mi = AjaxHandlerManager.GetMethod(curType, action);
            }

            if (mi == null)
            {
                return HttpResult.Error($"业务函数 '{action}' 不存在.");
            }


            HttpResult hResult;

            try
            {
                hResult = mi.Invoke(bllObject, null) as HttpResult;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    log.Error($"执行函数错误: {typePath}, action={action}", ex.InnerException);
                    hResult = HttpResult.Error("执行函数错误.", ex.Message);
                }
                else
                {
                    log.Error($"执行函数错误: {typePath}, action={action}", ex);
                    hResult = HttpResult.Error("执行函数错误.", ex.Message);
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