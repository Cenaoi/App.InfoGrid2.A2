using App.BizCommon;
using App.InfoGrid2.Model.CMS;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace App.InfoGrid2.View.CMS
{
    public partial class CmsView : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


            InitData();
        }

        public CMS_ITEM T { get; set; }


        private void InitData()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            CMS_ITEM cm = decipher.SelectModelByPk<CMS_ITEM>(id);

            this.T = cm;

        }




    }
}