using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using System;

namespace App.InfoGrid2.View.Explorer
{
    public partial class ResetLogin : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void GoSubmit()
        {
            string loginName = this.loginNameTB.Value;
            string password = this.passTB.Value;





            if (!ValidateUtil.SqlInput(loginName) || !ValidateUtil.SqlInput(password))
            {
                MessageBox.Alert("账号密码错误.");
                return;
            }



            SecFunMgr.Clear();

            LoginMgr mgr = new LoginMgr();

            bool login = mgr.Login(loginName, password);

            if (!login)
            {
                MessageBox.Alert("账号密码有误");
                return;
            }


            try
            {
                bool loginGet = mgr.GetUserByLoginName(loginName);

                if (!loginGet)
                {
                    MessageBox.Alert("账号已在别处登录，请保管好自己的用户密码！");
                    return;
                }

                Toast.Show("登录成功.");
                ScriptManager.Eval("ownerWindow.close({success:true})");
            }
            catch (Exception ex)
            {
                MessageBox.Alert("登陆失败，请联系设计人员.");

                log.Error("登陆获取用户信息的时候错误。", ex);

                return;
            }

        }

        public void GoCancel()
        {
            ScriptManager.Eval("ownerWindow.close({success:false})");
        }
    }
}