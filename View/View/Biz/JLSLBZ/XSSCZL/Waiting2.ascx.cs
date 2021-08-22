using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.JLSLBZ.XSSCZL
{
    public partial class Waiting2 : WidgetControl, IView
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
        /// 关闭窗口
        /// </summary>
        public void CloseWin()
        {
            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok'});");
        }

    }
}