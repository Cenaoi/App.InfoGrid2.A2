using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Login
{
    public partial class LoginHandle : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = WebUtil.Form("action").ToUpper();

            string result = string.Empty;

            switch (action)
            {
                case "LOGIN":

                    result = GoSubmit();

                    break;
            }

            Response.Clear();
            Response.Write(result);

            Response.End();
        }


        public string m_LoginName;
        public string m_LoginPass;

        public string AlertMsg;

        private string GoSubmit()
        {

            string loginName = WebUtil.Form("LoginName");
            string password = WebUtil.Form("LoginPass");

            m_LoginName = loginName;


            if (!ValidateUtil.SqlInput(loginName) || !ValidateUtil.SqlInput(password))
            {
                SModel sm = new SModel();
                sm["result"] = "error";
                sm["msg"] = "账号名称错误!";
                sm["data"] = null;
                
                return sm.ToJson();
            }



            SecFunMgr.Clear();

            LoginMgr mgr = new LoginMgr();

            bool login = mgr.Login(loginName, password);

            if (!login)
            {
                SModel sm = new SModel();
                sm["result"] = "error";
                sm["msg"] = "账号名称错误!";
                sm["data"] = null;

                return sm.ToJson();
            }


            try
            {
                bool loginGet = mgr.GetUserByLoginName(loginName);

                if (!loginGet)
                {
                    SModel fail = new SModel();
                    fail["result"] = "error";
                    fail["msg"] = "账号已在别处登录，请保管好自己的用户密码！";
                    fail["data"] = null;

                    return fail.ToJson();
                }

                //Response.Redirect("/app/BizExplorer/View/v2013.aspx?_rum=" + Guid.NewGuid(),false);
                string url = $"/app/InfoGrid2/View/Explorer/Main_v201608.aspx?_rum={Guid.NewGuid()}";

                SModel sm = new SModel();
                sm["result"] = "ok";
                sm["msg"] = "";
                sm["data"] = url;

                return sm.ToJson();
            }
            catch (Exception ex)
            {
                log.Error("登陆获取用户信息的时候错误。", ex);
                
                SModel sm = new SModel();
                sm["result"] = "error";
                sm["msg"] = "登陆失败，请联系设计人员!";
                sm["data"] = null;

                return sm.ToJson();
            }

        }

    }
}