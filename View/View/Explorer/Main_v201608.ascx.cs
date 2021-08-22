using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using System;


namespace App.InfoGrid2.View.Explorer
{
    public partial class Main_v201608 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

            InitWelcome();
        }

        private void InitWelcome()
        {
            string url = GlobelParam.GetValue("WELCOME_URL", "/App/InfoGrid2/View/Explorer/WelcomeJB2.aspx", "欢迎页面");

            EasyClick.Web.Mini2.Tab tab = new EasyClick.Web.Mini2.Tab();
            tab.ID = "welcome1";
            tab.Closable = false;
            tab.Text = " 欢 迎! ";
            tab.Url = url;
            tab.IFrame = true;

            this.layout1.Items.Add(tab);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {

            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }


        public EC5.SystemBoard.EcUserState EcUser
        {
            get
            {
                EcUserState user = EC5.SystemBoard.EcContext.Current.User;

                return user;
            }
        }

        /// <summary>
        /// 获取欢迎页面
        /// </summary>
        /// <returns></returns>
        public string GetWelcomeUrl()
        {
            //App/InfoGrid2/View/Explorer/WelcomeJB.aspx

            string url = GlobelParam.GetValue("WELCOME_URL", "/App/InfoGrid2/View/Explorer/WelcomeJB2.aspx", "欢迎页面地址");

            return url;
        }


        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <returns></returns>
        public string GetCompanyName()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_C_COMPANY item = decipher.SelectToOneModel<BIZ_C_COMPANY>("ROW_SID >= 0");

            if (item != null)
            {
                return item.SHORT_NAME;
            }

            return "EasyClick 软件开发公司";

        }

    }
}