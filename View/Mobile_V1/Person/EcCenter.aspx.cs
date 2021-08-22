using EC5.SystemBoard;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Mobile_V1.Person
{
    public partial class EcCenter : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public string GetUserObj()
        {

            EcUserState user = EcContext.Current.User;

            SModel sm = new SModel();
           
            if (!user.Roles.Exist("VIP"))
            {
                  sm["id"] = 0;

                 return sm.ToJson();

            }






            return user.Identity.ToString();

        }


        


    }
}