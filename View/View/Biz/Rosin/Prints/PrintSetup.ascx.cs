using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Rosin.Prints
{
    public partial class PrintSetup : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

            if (!this.IsPostBack)
            {
                //OnInitData();


                int row_count = WebUtil.QueryInt("row_count");

                this.range1.EndValue = row_count.ToString() ;
            }
        }


        /// <summary>
        /// 开始打印
        /// </summary>
        public void GoPrint()
        {
            int row_id = WebUtil.QueryInt("row_id");
            int row_count = WebUtil.QueryInt("row_count");

            string pMode = this.modeRG1.Value;

            int startI = this.range1.StartInt??0;
            int endI = this.range1.EndInt??0;


            Window win = new Window("打印条码");
            win.ContentPath = $"/App/InfoGrid2/view/biz/rosin/prints/FormPrint.aspx?row_id={row_id}&p_mode={this.modeRG1.Value}&p_start={startI}&p_end={endI}";

            win.ShowDialog();


        }
    }
}