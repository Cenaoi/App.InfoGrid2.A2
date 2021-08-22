using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewStepNew1 : WidgetControl, IView
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
        /// 下一步
        /// </summary>
        public void GoNext() 
        {
            int cataId = WebUtil.QueryInt("catalog_id", 104);

            ///拿到视图名称
            string name = this.tbxViewName.Value;

            if(string.IsNullOrEmpty(name))
            {
                this.tbxViewName.MarkInvalid("视图名称不能为空");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                IG2_TMP_VIEW view = new IG2_TMP_VIEW();            

                view.DISPLAY = name;

                view.TMP_GUID = Guid.NewGuid();
                view.TMP_SESSION_ID = this.Session.SessionID;

                decipher.InsertModel(view);

                string url = string.Format("MViewStepNew2.aspx?uid={0}&catalog_id={1}", view.TMP_GUID, cataId);

                MiniPager.Redirect(url);

            }
            catch (Exception ex) 
            {
                log.Error(ex);

                MessageBox.Alert("执行错误:" + ex.Message);
            }
        }

    }
}