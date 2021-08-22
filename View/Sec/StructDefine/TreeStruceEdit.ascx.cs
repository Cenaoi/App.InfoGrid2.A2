using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.Sec.StructDefine
{
    public partial class TreeStruceEdit : WidgetControl, IView
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
                int parentId = StringUtil.ToInt(node.Value);


                List<SEC_STRUCT> models = decipher.SelectModels<SEC_STRUCT>("PARENT_ID={0} AND ROW_SID >= 0", parentId);

                foreach (SEC_STRUCT model in models)
                {
                    TreeNode node2 = new TreeNode(model.STRUCE_TEXT, model.SEC_STRUCT_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();

                node.ChildLoaded = true;
                node.Expand();

            }

            this.TreePanel1.OpenNode(node.Value);


            if (node.Value != "ROOT")
            {
                int id = int.Parse(node.Value);

                SEC_STRUCT cata = decipher.SelectModelByPk<SEC_STRUCT>(id);

                MiniPager.Redirect("iform1",
                    string.Format("/App/InfoGrid2/Sec/StructDefine/TreeNodeStruceEdit.aspx?id={0}", id));
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

            SEC_STRUCT model = decipher.SelectModelByPk<SEC_STRUCT>(StringUtil.ToInt(e.Node.Value));

            model.STRUCE_TEXT = e.Text;

            decipher.UpdateModelProps(model, "STRUCE_TEXT");

        }


        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = this.TreePanel1.NodeSelected;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = StringUtil.ToInt(node.Value);

            if (id <= 100)
            {
                e.Cancel = true;
                Toast.Show("这结点不能删除.");
                return;
            }

            LightModelFilter filterCata = new LightModelFilter(typeof(SEC_STRUCT));
            filterCata.And("PARENT_ID", id);
            filterCata.And("ROW_SID",0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

            if (decipher.ExistsModels(filterCata))
            {
                e.Cancel = true;
                Toast.Show("删除失败, 必须先删除目录下的‘结构节点’.");

                return;
            }




            SEC_STRUCT cata = decipher.SelectModelByPk<SEC_STRUCT>(id);

            cata.ROW_SID = -3;
            cata.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(cata, "ROW_SID", "ROW_DATE_DELETE");


        }

        static string m_Codes = "123456789ABCDEFGHIJKLNMOPQRSTUVWXYZ";

        void TreePanel1_Creating(object sender, EventArgs e)
        {



            DbDecipher decipher = ModelAction.OpenDecipher();


            TreeNode parent = this.TreePanel1.NodeSelected;

            int parentId = StringUtil.ToInt(parent.Value);


            SEC_STRUCT pCata = decipher.SelectModelByPk<SEC_STRUCT>(parentId);

            if (pCata == null)
            {
                pCata = new SEC_STRUCT();
                pCata.SEC_STRUCT_ID = 100;

                decipher.IdentityStop();
                
                decipher.InsertModel(pCata);

                decipher.IdentityRecover();
            }
            

            pCata.CHILD_IDENTITY++;


            decipher.UpdateModelProps(pCata, "CHILD_IDENTITY");



            SEC_STRUCT cata = new SEC_STRUCT();

            cata.STRUCE_CODE = pCata.STRUCE_CODE + pCata.CHILD_IDENTITY.ToString("00");
            cata.STRUCE_TEXT = cata.STRUCE_CODE + "_结构";

            cata.PARENT_ID = StringUtil.ToInt(parent.Value);



            decipher.InsertModel(cata);


            TreeNode node = new TreeNode();
            node.ParentId = parent.Value;
            node.Text = cata.STRUCE_TEXT;
            
            node.Value = cata.SEC_STRUCT_ID.ToString();

            this.TreePanel1.Add(node);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(node.Value);
        }



        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            int rootId = 100;

            SEC_STRUCT pCata = decipher.SelectModelByPk<SEC_STRUCT>(rootId);

            if (pCata == null)
            {
                pCata = new SEC_STRUCT();
                pCata.SEC_STRUCT_ID = 100;

                decipher.IdentityStop();

                decipher.InsertModel(pCata);

                decipher.IdentityRecover();
            }



            List<SEC_STRUCT> rootCatalogs = decipher.SelectModels<SEC_STRUCT>("PARENT_ID={0}", rootId);

            TreeNode rootNode = new TreeNode("根结构", rootId);
            rootNode.ParentId = "0";
            rootNode.NodeType = "CATA";
            rootNode.StatusID = 0;
            rootNode.Expand();

            this.TreePanel1.Add(rootNode);


            foreach (SEC_STRUCT cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.STRUCE_TEXT, cata.SEC_STRUCT_ID);
                tNode.ParentId = rootId.ToString();
                tNode.NodeType = "CATA";
                tNode.StatusID = 0;

                this.TreePanel1.Add(tNode);
            }

            this.TreePanel1.Refresh();

            //TreeNode moneViewNode = new TreeNode("关联视图表", "ROOT_VIEW");
            //moneViewNode.ParentId = "ROOT";
            //moneViewNode.NodeType = "CATA";
            //moneViewNode.StatusID = 0;

            //this.TreePanel1.Add(moneViewNode);
        }
    }
}