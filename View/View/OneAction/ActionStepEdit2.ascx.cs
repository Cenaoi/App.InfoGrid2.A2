using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2.Data;
using EasyClick.Web.Mini2;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.OneAction
{
    public partial class ActionStepEdit2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.DataBind();

                //try {
                //    this.DataBind();
                //}
                //catch(Exception ex)
                //{
                //    log.Error("数据绑定异常， ", ex);
                //}
            }
        }

        /// <summary>
        /// 应用到系统
        /// </summary>
        public void GoApply()
        {
            try
            {
                DbCascadeLoader dbCCLoader = new DbCascadeLoader();
                dbCCLoader.InitDbcc();

                Toast.Show("更新成功.");
            }
            catch (Exception ex)
            {
                log.Error("应用到系统错误。", ex);
                MessageBox.Alert("更新失败!");
            }
        }


        /// <summary>
        /// 初始化事务
        /// </summary>
        private void InitDbcc()
        {

            log.Info("准备初始化，数据表联动业务...");

            try
            {
                DbCascadeLoader dbCCLoader = new DbCascadeLoader();
                dbCCLoader.InitDbcc();
            }
            catch (Exception ex)
            {
                log.Error("初始化业务数据库事务失败.", ex);
            }


            log.Info("完成初始化，数据表联动业务.");
        }



    }
}