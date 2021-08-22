using App.BizCommon;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using App.InfoGrid2.Model.JsonModel;
using App.InfoGrid2.Model.Hairdressing;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// VipInfoHandler 的摘要说明
    /// </summary>
    public class VipInfoHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {


          
            context.Response.ContentType = "text/plain";

            string action = WebUtil.FormTrimUpper("action");

            try {

                switch (action)
                {

                    case "GET_VIP_INFO":
                        GetVipInfo(context);
                        break;
                    case "SAVE_VIP_INFO":
                        SaveVipInfo(context);
                        break;


                }

            }catch(Exception ex)
            {

                log.Error(ex);

                context.Response.Write("哦噢，出错了！");

            }

            context.Response.End();

        }

        /// <summary>
        /// 获取vip账号信息
        /// </summary>
        /// <param name="context"></param>
        void GetVipInfo(HttpContext context)
        {
           

            EcUserState user = EcContext.Current.User;

            if (!user.Roles.Exist("VIP"))
            {
                context.Response.Write("不是VIP角色！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();


            try
            {


                SModel  lm  = decipher.GetSModel($"select * from VIP_ACCOUNT where ROW_IDENTITY_ID = {user.Identity}");

                if(lm == null)
                {
                    context.Response.Write("找不到vip用户信息了！");
                    return;
                }

                context.Response.Write(lm.ToJson());

            }
            catch (Exception ex)
            {

                log.Error("获取VIP账号信息出错了！", ex);
                return;

            }

        }

        /// <summary>
        /// 保存用户信息函数
        /// </summary>
        /// <param name="context">上下文</param>
        void SaveVipInfo(HttpContext context)
        {
            EcUserState user = EcContext.Current.User;

            if (!user.Roles.Exist("VIP"))
            {
                context.Response.Write("不是VIP角色！");
                return;
            }



            string data = WebUtil.Form("data");



            DbDecipher decipher = ModelAction.OpenDecipher();

            try {


                VIP_ACCOUNT_VM vip_info = JsonConvert.DeserializeObject<VIP_ACCOUNT_VM>(data);


                decipher.UpdateModel(vip_info);

                SModel sm = new SModel();
                sm["result"] = "ok";

                context.Response.Write(sm.ToJson());

            }catch(Exception ex)
            {
                log.Error(ex);

                context.Response.Write("保存数据出错了！");

            }
            finally
            {

                decipher.Dispose();

            }
            

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