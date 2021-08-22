using App.BizCommon;
using App.InfoGrid2.Bll.Sec;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using System;

namespace App.InfoGrid2.Login
{
    public partial class Builder : EC5.SystemBoard.Web.UI.Page, IView
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



            if (action == "GoSubmit")
            {
                GoSubmit();
            }
            else
            {
                bool BUILDER_INPUT_ENABLE = GlobelParam.GetValue("BUILDER_INPUT_ENABLE", true, "免密码输入");

                if (BUILDER_INPUT_ENABLE)
                {
                    m_LoginName = GlobelParam.GetValue("BUILDER_ID", "builder");
                    m_LoginPass = GlobelParam.GetValue("BUILDER_PASS", "13242300623");
                }
            }



        }

        public string AlertMsg;

        public string m_LoginName;
        public string m_LoginPass;

        private void GoSubmit()
        {

            string loginName = WebUtil.FormTrim("LoginName");
            string password = WebUtil.FormTrim("LoginPass");

            m_LoginName = loginName;
            m_LoginPass = password;

            if (!ValidateUtil.RangeLength(loginName, new decimal[] { 4, 30 }) ||
                !ValidateUtil.RangeLength(loginName, new decimal[] { 4, 30 }))
            {
                AlertMsg = "alert('账号密码有误');";
                return;
            }

            if (!ValidateUtil.SqlInput(loginName) || !ValidateUtil.SqlInput(password))
            {
                AlertMsg = "alert('账号密码有误');";
                return;
            }

            bool login = Login(loginName, password);

            if (!login)
            {
                AlertMsg = "alert('账号密码有误')";
                return;
            }

            SecFunMgr.Clear();

            EcUserState user = EC5.SystemBoard.EcContext.Current.User;
            user.Clear();
            user.ExpandPropertys.Clear();
            
            
            user.Roles.Add(IG2Param.Role.BUILDER);

            user.LoginID = "Builder";
            user.LoginName = "设计师";
            
            user.IsVirtual = false;
            user.Identity = 99999999;
           
            


            try
            {

                //Response.Redirect("/app/BizExplorer/View/v2013.aspx?_rum=" + EC5.Utility.RandomUtil.Next().ToString("00000000"),false);

                Response.Redirect($"/app/InfoGrid2/View/Explorer/Main_v2016.aspx?_rum={RandomUtil.Next():00000000}", false);
            }
            catch (Exception ex)
            {
                log.Error("登陆获取用户信息的时候错误。", ex);

                AlertMsg = "alert('登陆失败，请联系设计人员。')";
                return;
            }
        }

        private bool Login(string loginName, string loginPass)
        {

            string builderName = GlobelParam.GetValue("BUILDER_ID", "builder");
            string builderPass = GlobelParam.GetValue("BUILDER_PASS", "13242300623");

            if (loginName == builderName && loginPass == builderPass)
            {
                return true;
            }

            return false;
        }

    }
}