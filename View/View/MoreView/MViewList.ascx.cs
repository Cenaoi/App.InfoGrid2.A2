using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.LightModels;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewList : WidgetControl, IView
    {
        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
                this.store1.DataBind();
            }
        }

       
        /// <summary>
        /// 跳转到编辑窗口
        /// </summary>
        public void GoEdit() 
        {
            string id = this.store1.CurDataId;

            string url = string.Format("MViewStepEdit2.aspx?id={0}", id);

            MiniPager.Redirect(url);

        }

    }
}