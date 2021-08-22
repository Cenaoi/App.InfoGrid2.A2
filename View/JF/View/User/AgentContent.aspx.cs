using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.JF.View.User
{
    public partial class AgentContent : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (!BusHelper.AutoLogin())
                {

                    Response.Redirect("/JF/WeChat/Index.ashx");


                }

            }


        }


        /// <summary>
        /// 获取代理用户数据  现在只拿一级的
        /// </summary>
        /// <returns></returns>
        public string GetAgentUsersObj()
        {
            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_W_ACCOUNT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("PARENT_CODE", w_code);

            List<ES_W_ACCOUNT> agent_users = decipher.SelectModels<ES_W_ACCOUNT>(lmFilter);


            SModelList sm_agent_users = new SModelList();


            foreach (ES_W_ACCOUNT agent_user in agent_users)
            {

                SModel sm = new SModel();

                sm["name"] = agent_user.W_NICKNAME;


                sm_agent_users.Add(sm);

            }


            return sm_agent_users.ToJson();


        }


    }
}