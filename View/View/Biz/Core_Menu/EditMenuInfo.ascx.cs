using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.Core_Menu
{
    public partial class EditMenuInfo : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        /// 保存事件
        /// </summary>
        public void btnSave() 
        {
            try
            {

                int n = this.store1.SaveAll();

                Toast.Show("保存成功！");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("保存失败");
            }

        }



    }
}