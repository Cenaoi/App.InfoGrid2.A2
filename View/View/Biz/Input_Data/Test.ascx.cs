using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Input_Data
{
    public partial class Test : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void btnTest() 
        {
            string json = "{dialog_id:196, map_id:13,parent_id_field:'COL_1',l_table:'', r_table:''}";

            string abc = Base64Util.ToString(json, Base64Mode.Http);

            string url = string.Format("/App/InfoGrid2/View/Biz/Input_Data/EditInputData.aspx?rule={0}&parent_table={1}", abc, "UT_062");

            MiniPager.Redirect(url);

        }

    }
}