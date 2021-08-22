using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Bll.Sec;


namespace App.InfoGrid2.Login
{
    public partial class Index  : EC5.SystemBoard.Web.UI.Page, IView
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

            LoginMgr mgr = new LoginMgr();

            bool login = mgr.Login(loginName, password);

            if (!login)
            {
                AlertMsg =  "alert('账号密码有误')";
                return;
            }


            try
            {
                mgr.GetUserByLoginName(loginName);

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