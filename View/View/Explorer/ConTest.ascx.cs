using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.Explorer
{
    public partial class ConTest : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.Store1.DataBind();
            }
        }

        public void GoSubmit()
        {

            //string vv = this.dateRange1.StartValue;

            EasyClick.Web.Mini.MiniHelper.Alert("xxxxxx");

        //    LuaInterface.Lua lua = new LuaInterface.Lua();

        //    string code = "(23 * 3) + 1 + (2 * 3)";

        //    object[] luaResult = lua.DoString("return " + code);

        //    EasyClick.Web.Mini.MiniHelper.Alert(this.checkBox1.Checked.ToString() + "   " + this.textBox1.Value + "    " + luaResult[0].ToString());
        }
    }
}