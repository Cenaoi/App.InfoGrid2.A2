using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.OneValid
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


        /// <summary>
        /// 跳转到正式界面
        /// </summary>
        public void btnTest() 
        {
            string json = string.Format("{{remote:'{0}',date:true,number:true,digits:true,rangelength:[{1},{2}],messages:{{remote:'{3}',date:'{4}',number:'{5}',digits:'{6}',rangelength:'{7}'}}}}",
                   "哈哈哈",
                   1,
                   2,
                   "哈哈哈",
                   "哈哈哈",
                   "哈哈哈",
                   "哈哈哈",
                   "哈哈哈"
                   );

            string abc = Base64Util.ToString(json, Base64Mode.Http);

            Convert.FromBase64String(null);

            MiniPager.Redirect("/App/InfoGrid2/View/OneValid/ValidParamSetup.aspx?rule="+abc);


        }


       

    }
}