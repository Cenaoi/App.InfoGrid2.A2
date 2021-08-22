using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.IG2.Core;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;

namespace App.InfoGrid2.View.Biz.Core_Code
{
    public partial class CodeMgr : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        public void GoApplyReset()
        {
            try
            {
                BizCodeLoader loader = new BizCodeLoader();
                loader.Load();

                Toast.Show("重新加载成功");
            }
            catch (Exception ex)
            {
                log.Error("加载编码定义失败", ex);


                MessageBox.Alert("重新加载失败");
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        public void GoTest()
        {
           DataRecord dr =  store1.GetDataCurrent();

            var t_code = dr.Get("T_CODE");

            if(t_code == null)
            {
                MessageBox.Alert("没有找到组代码，请重新应用一下！");
                return;
            }

            string newId = EC5.BizCoder.BizCodeMgr.NewCode(t_code.ToString());

            MessageBox.Alert("新单号：" + newId);
        }
    }
}