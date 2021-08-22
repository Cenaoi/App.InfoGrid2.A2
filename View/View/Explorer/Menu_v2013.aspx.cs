using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.Explorer
{
    public partial class Menu_v2013 : EC5.SystemBoard.Web.UI.Page, IView
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

    }
}