using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    public class T1992_0_Page : ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {


        }

        protected override void OnLoad()
        {

            Toolbar tbar = this.FindControl("toolbarT1996") as Toolbar;

            if (tbar != null)
            {

                ToolBarButton btn = null;

                foreach (var item in tbar.Items)
                {
                    if(item.ID== "toolbarItem3311")
                    {
                        btn = item as ToolBarButton;
                        break;
                    }
                }


                if (btn != null)
                {                    
                    btn.OnBeforeClick = GetMethodJs("ShowGif");
                }


            }


            base.OnLoad();

        }


        public void ShowGif()
        {
            Window win = new Window();
            string img_src = "/res/生产指令单运算.gif";
            win.ContentPath = $"/App/InfoGrid2/View/Biz/JLSLBZ/XSSCZL/CurrencyWaiting.aspx?img_src={img_src}";
            win.WindowClosed += Win_WindowClosed;
            win.Mode = true;
            
            win.Show();
        }


        public void Win_WindowClosed(object sender, string data)
        {
            MainStore.Refresh();
            
        }

    }
}