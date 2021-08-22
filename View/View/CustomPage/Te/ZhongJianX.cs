using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.CustomPage.Te
{
    public class ZhongJianX :ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {

            string btn1_code = App.BizCommon.GlobelParam.GetValue("BTN1_CODE", "toolbarItem3018");
            string btn2_code = App.BizCommon.GlobelParam.GetValue("BTN2_CODE", "toolbarItem3019");


            Toolbar toolbar = this.MainToolbar;


            ToolBarButton btn1 = toolbar.Items[btn1_code] as ToolBarButton;

            if (btn1 != null)
            {
                btn1.OnClick = GetMethodJs("btn1_click", "你好");
            }


            ToolBarButton btn2 = toolbar.Items[btn2_code] as ToolBarButton;

            if (btn2 != null)
            {
                btn2.OnClick = GetMethodJs("btn2_click", "你好");
            }

        }

        
        public void btn1_click(string name)
        {

            string btn1_url = App.BizCommon.GlobelParam.GetValue("BTN1_URL", "toolbarItem3018");

            Window win = new Window("抽奖模式 1");
            win.ContentPath = btn1_url + "?_rum=" + EC5.Utility.RandomUtil.Next(1000000);

            win.State = WindowState.Max;

            win.ShowDialog();

        }

        public void btn2_click(string name)
        {
            string btn2_url = App.BizCommon.GlobelParam.GetValue("BTN2_URL", "toolbarItem3018");

            Window win = new Window("抽奖模式 2");

            win.ContentPath = btn2_url + "?_rum=" + EC5.Utility.RandomUtil.Next(1000000);
            win.State = WindowState.Max;

            win.ShowDialog();


        }


    }
}