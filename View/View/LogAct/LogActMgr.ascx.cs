using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EC5.BizLogger.Model;
using EC5.Utility;

namespace App.InfoGrid2.View.LogAct
{
    public partial class LogActMgr : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {

            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.TreePanel1.Selected += TreePanel1_Selected;

            if (!this.IsPostBack)
            {
                this.InitData();
            }
        }

        void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            DbDecipher decipher = ModelAction.OpenDecipher();



            if (node.Value == "ROOT")
            {
                return;
            }

            string id = node.Value.Substring(2);   

            MiniPager.Redirect("iform1",
                      string.Format("/App/InfoGrid2/View/LogAct/LogActText.aspx?id={0}", id));

        }

        NameValueCollection m_ActCodes = new NameValueCollection()
            {
                {"INSERT","新建"},
                {"UPDATE","更新"},
                {"DELETE","删除"},
                {"ALL","全部"},
                {"NONE","阻止"}
            };
      
        private void InitData() 
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LOG_ACT logAct = decipher.SelectModelByPk<LOG_ACT>(id);
            LModelList<LOG_ACT_OP> opList = decipher.SelectModels<LOG_ACT_OP>("LOG_ACT_ID={0}",id);

            if (logAct == null || opList.Count == 0)
            {
                Response.Redirect("http://baidu.com");
                return;
            }
            TreeNode rootNode = new TreeNode();
            rootNode.Text = logAct.OP_TEXT;
            rootNode.Value = "ROOT";
            rootNode.ParentId = "0";

            rootNode.Expand();

            this.TreePanel1.Add(rootNode);


            foreach (LOG_ACT_OP item in opList)
            {
                TreeNode node1 = new TreeNode();
                node1.Text = string.Format("{0}:{1}-({2}){3}", 
                    m_ActCodes[item.ACT_CODE], item.C_TABLE, item.ACTION_ID, item.C_DISPLAY);

                if (!StringUtil.IsBlank(item.RESULT_MESSAGE))
                {
                    node1.Text += "(" + item.RESULT_MESSAGE + ")";
                }

                node1.Value = "OP" + item.LOG_ACT_OP_ID;

                if (item.PARENT_ID == 0)
                {
                    node1.ParentId = "ROOT";
                }
                else 
                {
                    node1.ParentId = "OP" + item.PARENT_ID;
                }
                node1.Expand();

                this.TreePanel1.Add(node1);

            }

        }


    }
}