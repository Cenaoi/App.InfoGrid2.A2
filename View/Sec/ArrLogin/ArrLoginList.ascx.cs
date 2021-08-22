using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model.SecModels;
using System.Text;

namespace App.InfoGrid2.Sec.ArrLogin
{
    public partial class ArrLoginList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";


            base.OnInit(e);
            
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            OnBiz_InitUI();

            OnBiz_InitData();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void OnBiz_InitUI()
        {

        }

        /// <summary>
        /// 初始化界面数据
        /// </summary>
        private void OnBiz_InitData()
        {
            if (this.IsPostBack)
            {
                return;
            }



            SelectColumn col = this.table1.Columns.FindByDataField("REF_CODE") as SelectColumn;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.Fields = new string[] { "BIZ_USER_CODE", "TRUE_NAME" };

            List<LModel> models = decipher.GetModelList(filter);

            col.Items.Add(new ListItem() { TextEx = "--空--" });

            foreach (LModel model in models)
            {
                col.Items.Add(model.Get<string>("BIZ_USER_CODE"), model.Get<string>("TRUE_NAME"));
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }

            
        }

    }
}