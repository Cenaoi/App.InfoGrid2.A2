using EasyClick.Web.Mini2;
using EC5.AppDomainPlugin;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Rosin.Transform_V1
{
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

            TriggerBox1.ButtonClickCallback += TriggerBox1_ButtonClickCallback;

            TriggerBox2.ButtonClickCallback += TriggerBox2_ButtonClickCallback;

            if (!IsPostBack)
            {

                store1.DataBind();

            }

        }

        public void TriggerBox2_ButtonClickCallback(object sender, string data)
        {
            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={17}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed1;

            win.ShowDialog();

        }

        public void Win_WindowClosed1(object sender, string data)
        {
            DynSModel e = DynSModel.ParseJson(data);

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            TriggerBox2.Value = row.Get<string>("CLIENT_TEXT");
            TextBox2.Value = row.Get<string>("CLIENT_CODE");


        }

        public void TriggerBox1_ButtonClickCallback(object sender, string data)
        {

            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={17}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed;

            win.ShowDialog();


        }

        public void Win_WindowClosed(object sender, string data)
        {

            DynSModel e = DynSModel.ParseJson(data);

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            TriggerBox1.Value = row.Get<string>("CLIENT_TEXT");
            TextBox1.Value = row.Get<string>("CLIENT_CODE");

             

        }
    }
}