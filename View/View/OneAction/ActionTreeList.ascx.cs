using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using EC5.Utility;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using System.Data;
using System.Text;
using EC5.DbCascade.Model;
using System.Collections.Specialized;
using App.InfoGrid2.Bll;

namespace App.InfoGrid2.View.OneAction
{
    public partial class ActionTreeList: WidgetControl, IView
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


            if (node.Value.LastIndexOf("ITEM_")>0)
            {
                int n = node.Value.LastIndexOf("ITEM_");
                string idStr = node.Value.Substring(n + 5);

                int id = int.Parse(idStr);

                MiniPager.Redirect("iform1",
                    string.Format("/App/InfoGrid2/View/OneAction/ActionStepEdit2.aspx?id={0}", id));
            }

        }

        NameValueCollection m_ActCodes = new NameValueCollection()
            {
                {"INSERT","新建"},
                {"UPDATE","更新"},
                {"DELETE","删除"},
                {"ALL","全部"},
            };

        private void InitData()
        {

            string tableR = WebUtil.Query("table_right");

            IG2_TABLE table = TableMgr.GetTableForName(tableR);

            if (table == null)
            {
                return;
            }

            int depth = 0;   //深度


            TreeNode rootNode = new TreeNode();
            rootNode.Text = string.Format("{0} - {1}", table.TABLE_NAME, table.DISPLAY);
            rootNode.Value = "ROOT";
            rootNode.ParentId = "0";

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_ACTION));
            filter.And("R_TABLE", tableR);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            LModelList<IG2_ACTION> actList = decipher.SelectModels<IG2_ACTION>(filter);

            LModelGroup<IG2_ACTION,string> actGroups = actList.ToGroup<string>("R_ACT_CODE");

            rootNode.Expand();

            this.TreePanel1.Add(rootNode);

            int n = 0;

            foreach (string actCode in actGroups.Keys)
            {
                LModelList<IG2_ACTION> actGroup = actGroups[actCode];

                TreeNode node = new TreeNode();
                node.Text = m_ActCodes[actCode] + "-操作";
                node.Value = "OP_" + n++ ;

                node.ParentId = "ROOT";

                node.Expand();


                this.TreePanel1.Add(node);

                foreach (IG2_ACTION act in actGroup)
                {
                    TreeNode actNode = new TreeNode();
                    actNode.Text = string.Format("{0}-{1}:{2} - {3}",act.IG2_ACTION_ID, m_ActCodes[act.L_ACT_CODE], act.L_TABLE, act.L_TABLE_TEXT);
                    actNode.Value = node.Value + "_ITEM_" + act.IG2_ACTION_ID;
                   
                    actNode.ParentId = node.Value;

                    this.TreePanel1.Add(actNode);

                    List<int> idLine = new List<int>(act.IG2_ACTION_ID);    //联动的链。防止出现死循环

                    FullChinds(++depth,idLine, actNode.Value, act.L_ACT_CODE, act.L_TABLE);
                }

            }

        }


        private void FullChinds(int depth,List<int> idLine,string parentId, string rightActCode, string rightTable)
        {


            if (depth > 9)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_ACTION));
            filter.And("R_TABLE", rightTable);
            filter.And("R_ACT_CODE", new string[]{"ALL",rightActCode}, Logic.In);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            LModelList<IG2_ACTION> actList = decipher.SelectModels<IG2_ACTION>(filter);

            foreach (IG2_ACTION act in actList)
            {
                if (idLine.Contains(act.IG2_ACTION_ID))
                {
                    TreeNode actNode = new TreeNode();
                    actNode.Text = string.Format("【循环】 {0}-{1}:{2} - {3}", act.IG2_ACTION_ID, m_ActCodes[act.L_ACT_CODE], act.L_TABLE, act.L_TABLE_TEXT);
                    actNode.Value = parentId + "_ITEM_" + act.IG2_ACTION_ID;
                    actNode.ParentId = parentId;

                    this.TreePanel1.Add(actNode);

                    continue;
                }
                else
                {
                    TreeNode actNode = new TreeNode();
                    actNode.Text = string.Format("{0}-{1}:{2} - {3}",act.IG2_ACTION_ID, m_ActCodes[act.L_ACT_CODE], act.L_TABLE, act.L_TABLE_TEXT);
                    actNode.Value = parentId + "_ITEM_" + act.IG2_ACTION_ID;
                    actNode.ParentId = parentId;

                    this.TreePanel1.Add(actNode);

                    idLine.Add(act.IG2_ACTION_ID);

                    FullChinds(++depth, idLine, actNode.Value, act.L_ACT_CODE, act.L_TABLE);
                }
            }


        }

    }
}