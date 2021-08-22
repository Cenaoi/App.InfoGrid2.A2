using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class ShowFieldAllEdit : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            this.store1.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(store1_CurrentChanged);

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }


        /// <summary>
        /// 数据表焦点改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            this.store2.DataBind();
        }

        /// <summary>
        /// 确定选择字段
        /// </summary>
        public void SelectField()
        {
            if (this.table2.CheckedRows.Count == 0)
            {
                MiniHelper.Alert("没有选择数据表！");
                return;
            }


            List<string> idList = new List<string>();

            DataRecordCollection drc = this.table2.CheckedRows;

            foreach (DataRecord dr in drc)
            {
                idList.Add(dr.Id);
            }


            string ids = string.Join(",", idList.ToArray());



            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:'" + ids + "'});");

        }

    }
}