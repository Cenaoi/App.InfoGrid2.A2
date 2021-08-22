using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Sec.User
{
    public partial class SelectUser : WidgetControl, IView
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
                store1.DataBind();
            }
        }


        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        public void GoSuccess()
        {


            DataRecordCollection drc = table1.CheckedRows;


            if(drc.Count == 0)
            {
                MessageBox.Alert("请至少选择一条记录！");
                return;
            }



            List<string> user_ids = new List<string>();

            foreach(DataRecord dr in drc)
            {

                user_ids.Add(dr.Id);

            }


            //弹出窗口关闭事件
            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:\""+StringUtil.Join(",",user_ids.ToArray(),0)+"\"});");



        }

    }
}