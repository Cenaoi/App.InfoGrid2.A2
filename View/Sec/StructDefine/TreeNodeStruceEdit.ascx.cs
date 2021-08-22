using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.Sec.StructDefine
{
    public partial class TreeNodeStruceEdit : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //App.InfoGrid2.View.Error404.Send("错误信息");

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        public void btnSave()
        {
            int id = WebUtil.QueryInt("id");

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                SEC_STRUCT model = decipher.SelectModelByPk<SEC_STRUCT>(id);

                //model.STRUCE_CODE = tbCode.Value;
                model.STRUCE_TEXT = tbText.Value;
                model.ROW_DATE_UPDATE = DateTime.Now;

                decipher.UpdateModelProps(model,  "STRUCE_TEXT", "ROW_DATE_UPDATE");

                Toast.Show("保存成功。");
            }
            catch (Exception ex)
            {
                log.Error("保存失败", ex);

                MessageBox.Alert("保存失败");
            }
            
        }
    }
}