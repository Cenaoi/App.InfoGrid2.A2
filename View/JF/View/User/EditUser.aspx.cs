using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.JF.View.User
{
    public partial class EditUser : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!BusHelper.AutoLogin())
                {

                    Response.Redirect("/JF/WeChat/Index.ashx");


                }

            }

        }


        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        public string GetUserObj()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_W_ACCOUNT account = decipher.SelectModelByPk<ES_W_ACCOUNT>(user.Identity);

            if (account == null)
            {

                Response.Redirect("/JF/WeChat/Index.ashx");

            }


            SModel sm = new SModel();

            sm["w_nickname"] = account.W_NICKNAME;
            sm["sex"] = account.SEX;

            return sm.ToJson();

        }

    }
}