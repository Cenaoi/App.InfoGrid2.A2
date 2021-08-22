using App.BizCommon;
using App.InfoGrid2.Handlers;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.JF.Handlers
{
    /// <summary>
    /// 用户处理类
    /// </summary>
    public class UserHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";



            string action = WebUtil.FormTrimUpper("action");


            try
            {

                switch (action)
                {

                    //加入购物车按钮点击事件
                    case "EDIT_USER":
                        EditUser();
                        break;
                 
                    default:
                        ResponseHelper.Result_error("写错了吧！");
                        break;

                }


            }
            catch (Exception ex)
            {

                log.Debug(ex);

                ResponseHelper.Result_error("哦噢，出错了喔！");
            }




        }

        /// <summary>
        /// 编辑用户保存函数
        /// </summary>
        void EditUser()
        {

            string json_data = WebUtil.Form("json_data");

            SModel sm = SModel.ParseJson(json_data);

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_W_ACCOUNT account = decipher.SelectModelByPk<ES_W_ACCOUNT>(user.Identity);

            account.SetTakeChange(true);

            account.W_NICKNAME = sm["w_nickname"].ToString();
            account.SEX = sm["sex"].ToString();
            account.CONTACTER_NAME = sm["contacter_name"].ToString();
            account.CONTACTER_TEL = sm["contacter_tel"].ToString();
            account.ADDRESS_T2 = sm["address_t2"].ToString();

            account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModel(account, true);


            ResponseHelper.Result_ok("保存成功了！");


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