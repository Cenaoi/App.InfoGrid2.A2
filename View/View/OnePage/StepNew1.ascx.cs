using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using App.BizCommon;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OnePage
{
    public partial class StepNew1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        public void GoNext()
        {
            if (StringUtil.IsBlank(TB1.Value))
            {
                this.TB1.MarkInvalid("请填写“复杂表名称”！");
                return;
            }

            int catalog_id = WebUtil.QueryInt("catalog_id", 103);

            string page_type = this.RG1.Value;

            if(page_type == "ONE_FORM")
            {

            }
            else if(page_type == "ONE_PAGE")
            {

            }
            else
            {
                EasyClick.Web.Mini2.MessageBox.Alert("格式错误..");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE table = new IG2_TABLE();

            table.TABLE_TYPE_ID = "PAGE";

            if (page_type != "ONE_PAGE")
            {
                table.TABLE_SUB_TYPE_ID = page_type;
            }

            table.IG2_CATALOG_ID = catalog_id;
            table.DISPLAY = this.TB1.Value;

            try
            {

                decipher.InsertModel(table);
                
                string url = $"/App/InfoGrid2/View/OneBuilder/PageBuilder.aspx?id={table.IG2_TABLE_ID}&page_type={page_type}";

                MiniPager.Redirect(url);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                EasyClick.Web.Mini.MiniHelper.Alert("创建复杂表失败." + ex.Message);
            }
        }
    }
}