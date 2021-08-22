using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.Manager
{
    public partial class TreePageManager : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                List<IG2_CATALOG> models = decipher.SelectModels<IG2_CATALOG>("PARENT_ID={0} AND ROW_SID >= 0", node.Value);

                foreach (IG2_CATALOG model in models)
                {
                    TreeNode node2 = new TreeNode(model.TEXT, model.IG2_CATALOG_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }
                
                this.TreePanel1.Refresh();

                node.ChildLoaded = true;
                node.Expand();

            }

            this.TreePanel1.OpenNode(node.Value);

            if(node.Value != "ROOT")
            {
                int id = int.Parse(node.Value);

                IG2_CATALOG cata = decipher.SelectModelByPk<IG2_CATALOG>(id);

                MiniPager.Redirect("iform1",
                    string.Format("/App/InfoGrid2/View/Manager/TableList.aspx?tableTypeId={0}&catalog_id={1}" , cata.DEFAULT_TABLE_TYPE, id));
            }


        }

        protected override void OnLoad(EventArgs e)
        {

            this.TreePanel1.Creating += new EventHandler(TreePanel1_Creating);
            this.TreePanel1.Removeing += new TreeNodeCancelEventHander(TreePanel1_Removeing);
            this.TreePanel1.Renaming += new TreeNodeRenameEventHander(TreePanel1_Renaming);

            if (!this.IsPostBack)
            {
                InitData();
            }

        }


        void TreePanel1_Renaming(object sender, TreeNodeRenameEventArgs e)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_CATALOG model = decipher.SelectModelByPk<IG2_CATALOG>(StringUtil.ToInt( e.Node.Value));

            model.TEXT = e.Text;

            decipher.UpdateModelProps(model, "TEXT");

        }


        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = this.TreePanel1.NodeSelected;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = StringUtil.ToInt(node.Value);

            LightModelFilter filterCata = new LightModelFilter(typeof(IG2_CATALOG));
            filterCata.And("PARENT_ID", id);

            if (decipher.ExistsModels(filterCata))
            {
                MessageBox.Alert("删除失败, 必须先删除目录下的‘工作表’和‘子目录’.");

                return;
            }


            LightModelFilter filterTab = new LightModelFilter(typeof(IG2_TABLE));
            filterTab.And("IG2_CATALOG_ID", id);


            if (decipher.ExistsModels(filterTab))
            {
                MessageBox.Alert("删除失败, 必须先删除目录下的‘工作表’和‘子目录’.");
                return;
            }


            IG2_CATALOG cata = decipher.SelectModelByPk<IG2_CATALOG>(id);

            cata.ROW_SID = -3;
            cata.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(cata, "ROW_SID", "ROW_DATE_DELETE");


        }

        void TreePanel1_Creating(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            TreeNode parent = this.TreePanel1.NodeSelected;

            IG2_CATALOG pCata = decipher.SelectModelByPk<IG2_CATALOG>(StringUtil.ToInt(parent.Value)); 

            IG2_CATALOG cata = new IG2_CATALOG();
            cata.TEXT = "新建目录";
            cata.PARENT_ID = StringUtil.ToInt(parent.Value);
            cata.DEFAULT_TABLE_TYPE = pCata.DEFAULT_TABLE_TYPE;
            

            decipher.InsertModel(cata);


            TreeNode node = new TreeNode();
            node.ParentId = parent.Value;
            node.Text = cata.TEXT;
            node.NodeType = "table";
            node.Value = cata.IG2_CATALOG_ID.ToString();

            this.TreePanel1.Add(node);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(node.Value);
        }



        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<IG2_CATALOG> rootCatalogs = decipher.SelectModels<IG2_CATALOG>("PARENT_ID={0}", 0);

            TreeNode rootNode = new TreeNode("开发系统","ROOT");
            rootNode.ParentId = "0";
            rootNode.NodeType = "CATA";
            rootNode.StatusID = 0;
            rootNode.Expand();

            this.TreePanel1.Add(rootNode);


            foreach (IG2_CATALOG cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.TEXT, cata.IG2_CATALOG_ID);
                tNode.ParentId = "ROOT";
                tNode.NodeType = "CATA";
                tNode.StatusID = 0;

                this.TreePanel1.Add(tNode);
            }

            //TreeNode moneViewNode = new TreeNode("关联视图表", "ROOT_VIEW");
            //moneViewNode.ParentId = "ROOT";
            //moneViewNode.NodeType = "CATA";
            //moneViewNode.StatusID = 0;

            //this.TreePanel1.Add(moneViewNode);
        }
    }
}