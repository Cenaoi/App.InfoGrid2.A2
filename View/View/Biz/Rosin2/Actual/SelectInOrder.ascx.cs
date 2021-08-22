using App.BizCommon;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
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
using System.Web.UI.WebControls;
using System.Text;
using EC5.AppDomainPlugin;

namespace App.InfoGrid2.View.Biz.Rosin2.Actual
{   
    /// <summary>
    /// 选择入库单数据明细
    /// </summary>
    public partial class SelectInOrder : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TriggerBox1.ButtonClickCallback += TriggerBox1_ButtonClickCallback;
            TriggerBox3.ButtonClickCallback += TriggerBox3_ButtonClickCallback;


            if (!IsPostBack)
            {

                string client_code = WebUtil.Query("client_code");

                string client_text = WebUtil.Query("client_text");


                TriggerBox1.Value = client_code;

                TextBox1.Value = client_text;


                store1.FilterParams.Add("CLIENT_CODE", client_code);

                store1.DataBind();
      
            }
        }

        public void TriggerBox3_ButtonClickCallback(object sender, string data)
        {
            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={39}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed1; ;

            win.ShowDialog();
        }

        public void Win_WindowClosed1(object sender, string data)
        {
            DynSModel e = DynSModel.ParseJson(data);

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;


            TriggerBox3.Value = row["PROD_CODE"].ToString();

            TextBox2.Value = row["PROD_TEXT"].ToString();



        }

        public void Win_WindowClosed(object sender, string data)
        {
            DynSModel e = DynSModel.ParseJson(data);

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            TriggerBox1.Value = row["CLIENT_CODE"].ToString();
            TextBox1.Value = row["CLIENT_TEXT"].ToString();




        }

        public void TriggerBox1_ButtonClickCallback(object sender, string data)
        {
           

            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={17}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed;

            win.ShowDialog();

        }

        public void GoSuccess()
        {



            DataRecordCollection drc = this.table1.CheckedRows;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (drc.Count == 0)
            {
                MessageBox.Alert("请至少选择一条记录！");
                return;
            }



            StringBuilder sb = new StringBuilder();






            int i = 0;
            foreach(DataRecord dr in drc)
            {
                if(i++> 0) { sb.Append(","); };


                sb.Append(dr.Id);


            }


            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close('{\"result\":\"ok\",\"ids\":\""+sb.ToString()+"\"}')");


        }

    }
}