using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;


namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 生产指令单用的
    /// </summary>
    public partial class SelectPrintByZLD : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        public void btnPrint()
        {
            DataRecord printDr = this.store1.GetDataCurrent();


            if (printDr == null)
            {
                MessageBox.Alert("请选择打印机！");
                return;
            }

            string code = printDr.Fields["PRINT_CODE"].Value;

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Alert("打印机没有相应的代码！");
                return;
            }

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',id:'" + code + "'});");


        }
    }
}