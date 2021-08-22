using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini;
using EC5.SystemBoard;

namespace App.InfoGrid2.Sec
{
    public partial class Permit : WidgetControl, IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Submit()
        {
            string key = PermitKeyTB.Value;

            string adminKey = EC5.SystemBoard.SysBoardManager.CurrentApp.AppSettings["ADMIN_KEY"];
            
            if (key != adminKey )
            {
                PermitKeyTB.Value = string.Empty;
                MiniHelper.Alert("授权号错误,");
                return;
            }

            EcUserState user = EcContext.Current.User;

            if (!user.Roles.Exist("ADMIN"))
            {
                user.Roles.Add("ADMIN");
            }

            MiniHelper.Alert("授权成功.");

            EcView.close();

        }
    }
}