using App.BizCommon;
using App.InfoGrid2.Bll.Sec;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Login
{
    public partial class Index3 : EC5.SystemBoard.Web.UI.Page, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            this.Context.Items["ResponseJS"] = true;
            this.Response.ExpiresAbsolute = DateTime.Now.AddDays(-20);

            base.OnInit(e);
        }

        public EC5.SystemBoard.EcUserState EcUser
        {
            get
            {
                EcUserState user = EC5.SystemBoard.EcContext.Current.User;
                return user;
            }
        }

        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <returns></returns>
        public string GetCompanyName()
        {
            return App.InfoGrid2.Bll.BizCompanyMgr.GetName();

        }


        protected void Page_Load(object sender, EventArgs e)
        {

            string action = WebUtil.Form("action");

            if (action == "Submit")
            {
                GoSubmit();
            }

        }

        public string m_LoginName;
        public string m_LoginPass;

        public string AlertMsg;

        public void GoSubmit()
        {

            string loginName = WebUtil.FormTrim("username");
            string password = WebUtil.FormTrim("password");

            m_LoginName = loginName;


            if (!ValidateUtil.SqlInput(loginName) || !ValidateUtil.SqlInput(password))
            {
                AlertMsg = "alert('账号密码有误');";
                return;
            }



            SecFunMgr.Clear();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_003");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("AGENT_LOGIN_NAME", loginName);
            filter.And("AGENT_LOGIN_PASS", password);

            LModel lm003 = decipher.GetModel(filter);

            if(lm003 == null)
            {

                AlertMsg = "alert('账号或密码出错了！。')";
                return;

            }


            EcContext ec_context = EcContext.Current;

            EcUserState userState = ec_context.User;

            userState.Clear();

            userState.ExpandPropertys.Clear();

            userState.Identity = lm003.Get<int>("ROW_IDENTITY_ID");
            userState.LoginID = loginName;
            userState.LoginName = lm003.Get<string>("AGENT_NAME");

            userState.Roles.Clear();
            userState.Roles.Add(IG2Param.Role.ADMIN);

            userState.ExpandPropertys["AGENT_CODE"] = lm003.Get<string>("AGENT_CODE");
            userState.ExpandPropertys["USER_CODE"] = "001";



            try
            {

                //Response.Redirect("/app/BizExplorer/View/v2013.aspx?_rum=" + Guid.NewGuid(),false);
                Response.Redirect($"/app/InfoGrid2/View/Explorer/Main_v2016.aspx?_rum={RandomUtil.Next():00000000}", false);
            }
            catch (Exception ex)
            {
                log.Error("登陆获取用户信息的时候错误。", ex);

                AlertMsg = "alert('登陆失败，请联系设计人员。')";
                return;
            }

        }
    }
}