using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;


namespace App.InfoGrid2.View.Biz.Rosin.Transform
{
    /// <summary>
    /// 转移历史界面
    /// </summary>
    public partial class TransformList : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                store1.DataBind();
            }
        }
    }
}