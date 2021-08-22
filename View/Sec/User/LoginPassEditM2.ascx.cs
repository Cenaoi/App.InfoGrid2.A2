using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.View;
using EasyClick.Web.Mini2;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Sec.User
{
    public partial class LoginPassEditM2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                InitData();
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData() 
        {

            DbDecipher decipher = ModelAction.OpenDecipher();  

            EcUserState user = EcContext.Current.User;

            //用户表ID
            int id = user.Identity;


            SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(id);

            if (account == null)
            {
                Error404.Send("提示", "请登录。");
                return;
            }

        }


        /// <summary>
        /// 保存新密码
        /// </summary>
        public void btnSave() 
        {


            string passNew1 = this.tbxPassNew1.Value;
            string passNew2 = this.tbxpassNew2.Value;
            string passOld = this.tbxPassOld.Value;

            if (passNew1 != passNew2) 
            {
                Toast.Show("两次输入的密码不一致！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;

            //用户表ID
            int id = user.Identity;


            SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(id);


            if (account.LOGIN_PASS != passOld) 
            {
                MessageBox.Alert("原密码输入错误！");
                return;
            }


            account.LOGIN_PASS = passNew2;


            decipher.UpdateModelProps(account, "LOGIN_PASS");


            this.tbxPassOld.Value = "";
            this.tbxPassNew1.Value = "";
            this.tbxpassNew2.Value = "";


            Toast.Show("更改密码成功了！");

        }

    }
}