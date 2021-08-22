using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Sec.M2User
{
    public partial class UserSecSetup1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.CurrentChanged += store1_CurrentChanged;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        void store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            if (e.SrcRecord == null)
            {
                return;
            }

            int user_id = StringUtil.ToInt( e.SrcRecord.Id);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_LOGIN_ACCOUNT model = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user_id);


            string url = string.Format("UserTreeMenuSetup1.aspx?user_id={0}&login_code={1}", user_id, model.BIZ_USER_CODE);

            MiniPager.Redirect("iform1",url );

        }


    }
}