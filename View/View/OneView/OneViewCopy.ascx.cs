using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using App.InfoGrid2.Model.DataSet;

namespace App.InfoGrid2.View.OneView
{
    public partial class OneViewCopy : WidgetControl, IView
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
                OnDataInit();
            }
        }

        private void OnDataInit()
        {
            int srcViewId = WebUtil.QueryInt("src_view_id");    //源视图字段


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet vSet = TableSet.Select(decipher, srcViewId);

            this.TB1.Value = vSet.Table.DISPLAY;
        }

        public void GoNext()
        {
            if (StringUtil.IsBlank(TB1.Value))
            {
                this.TB1.MarkInvalid("请填写工作表名");
                return;
            }

            int srcViewId = WebUtil.QueryInt("src_view_id");    //源视图字段


            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                TableSet vSet = TableSet.Select(decipher, srcViewId);

                vSet.Table.DISPLAY = this.TB1.Value;

                vSet.Insert(decipher);


                Toast.Show("创建成功!");
            }
            catch(Exception ex)
            {
                Toast.Show("拷贝失败.");

                log.Error("拷贝单视图表失败!", ex);
            }
        }







    }
}