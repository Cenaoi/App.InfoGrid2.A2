using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.Login
{
    public partial class Index2 : EC5.SystemBoard.Web.UI.Page, IView
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
    }
}