using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.OneFlowBuilder
{
    public partial class FlowNodeSetup : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                storeMain1.DataBind();

                store1.DataBind();
                store2.DataBind();
            }
        }

        public void GoShowSelectUser()
        {


            Window win = new Window();

            win.ContentPath = "/App/InfoGrid2/Sec/User/SelectUser.aspx";


            win.WindowClosed += Win_WindowClosed;

            win.State = WindowState.Max;

            win.ShowDialog();







        }

        public void Win_WindowClosed(object sender, string data)
        {

            SModel sm = SModel.ParseJson(data);

            if(sm.Get<string>("result") != "ok")
            {
                return;
            }


            string[] user_ids = StringUtil.Split(sm.Get<string>("ids"), ",");


            if(user_ids.Length == 0)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            int node_id = WebUtil.QueryInt("def_node_id");

            int def_id = WebUtil.QueryInt("def_id");


            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            FLOW_DEF_NODE node = decipher.SelectModelByPk<FLOW_DEF_NODE>(node_id);

            




            foreach (string user_id in user_ids)
            {
                SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user_id);

                LightModelFilter lmFilter = new LightModelFilter("FLOW_DEF_NODE_PARTY");
                lmFilter.And("ROW_SID",0,Logic.GreaterThanOrEqual);
                lmFilter.And("P_USER_CODE",account.BIZ_USER_CODE);
                lmFilter.And("NODE_ID", node_id);
                lmFilter.And("DEF_ID", def_id);

                //判断是否已经存在了
                bool flag = decipher.ExistsModels(lmFilter);

                //存在就不新增了
                if (flag)
                {
                    continue;
                }

                LModel lm = new LModel("FLOW_DEF_NODE_PARTY");

                lm["NODE_ID"] = node_id;
                lm["DEF_ID"] = def_id;
                lm["PARTY_TYPE"] = "USER";
                lm["DEF_CODE"] = def.DEF_CODE;
                lm["NODE_CODE"] = node.NODE_CODE;



                lm["P_USER_CODE"] = account.BIZ_USER_CODE;
                lm["P_USER_TEXT"] = account.TRUE_NAME;

                decipher.InsertModel(lm);


            }


            store1.Refresh();

            Toast.Show("添加参与活动者成功了！");



        }





        /// <summary>
        /// 获取抄送人员
        /// </summary>
        public void GoShowSelectCopyUser()
        {
            Window win = new Window();

            win.ContentPath = "/App/InfoGrid2/Sec/User/SelectUser.aspx";


            win.WindowClosed += CopyPartyWin_WindowClosed;

            win.State = WindowState.Max;

            win.ShowDialog();

        }

        public void CopyPartyWin_WindowClosed(object sender, string data)
        {

            SModel sm = SModel.ParseJson(data);

            if (sm.Get<string>("result") != "ok")
            {
                return;
            }


            string[] user_ids = StringUtil.Split(sm.Get<string>("ids"), ",");


            if (user_ids.Length == 0)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            int node_id = WebUtil.QueryInt("def_node_id");

            int def_id = WebUtil.QueryInt("def_id");


            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            FLOW_DEF_NODE node = decipher.SelectModelByPk<FLOW_DEF_NODE>(node_id);



            foreach (string user_id in user_ids)
            {
                SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user_id);

                LightModelFilter lmFilter = new LightModelFilter("FLOW_DEF_NODE_COPY_PARTY");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("DEF_ID", def_id);
                lmFilter.And("NODE_ID", node_id);
                lmFilter.And("P_USER_CODE", account.BIZ_USER_CODE);

                //判断是否已经存在了
                bool flag = decipher.ExistsModels(lmFilter);

                //存在就不新增了
                if (flag)
                {
                    continue;
                }

                LModel lm = new LModel("FLOW_DEF_NODE_COPY_PARTY");

                lm["NODE_ID"] = node_id;
                lm["DEF_ID"] = def_id;
                lm["PARTY_TYPE"] = "USER";
                lm["DEF_CODE"] = def.DEF_CODE;
                lm["NODE_CODE"] = node.NODE_CODE;

                lm["P_USER_CODE"] = account.BIZ_USER_CODE;
                lm["P_USER_TEXT"] = account.TRUE_NAME;

                decipher.InsertModel(lm);

            }


            store2.Refresh();

            Toast.Show("添加参与活动者成功了！");



        }
    }
}