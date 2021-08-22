using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.InputExcel
{
    public partial class StepManyNew1 : WidgetControl, IView
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
            string name = this.tbxName.Value;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Alert("请输入规则名！");
                return;
            }

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_IMPORT_RULE rule = new IG2_IMPORT_RULE();

                rule.RULE_NAME = name;
                rule.ROW_DATE_CREATE = DateTime.Now;
                rule.ROW_DATE_UPDATE = DateTime.Now;
                rule.TABLE_COUNT = 2;
                decipher.InsertModel(rule);


                MiniPager.Redirect("StepManyNew2.aspx?id=" + rule.IG2_IMPORT_RULE_ID);


            }
            catch (Exception ex)
            {
                log.Error("插入数据失败！", ex);
            }


        }
    }
}