using System;
using System.Collections.Generic;
using System.Web;
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
    public partial class SelectTreeCatalog : WidgetControl, IView
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
                InitData();
            }

        }

        protected void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                List<IG2_CATALOG> models = decipher.SelectModels<IG2_CATALOG>("ROW_SID >= 0 AND PARENT_ID={0}", node.Value);

                foreach (IG2_CATALOG model in models)
                {
                    TreeNode node2 = new TreeNode(model.TEXT, model.IG2_CATALOG_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();


                node.ChildLoaded = true;

                this.TreePanel1.OpenNode(node.Value);
            }

        }



        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<IG2_CATALOG> rootCatalogs = decipher.SelectModels<IG2_CATALOG>("ROW_SID >= 0 AND PARENT_ID={0}", 0);

            TreeNode rootNode = new TreeNode("开发系统", "ROOT");
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


        }


        public void GoSubmit()
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            if (node == null)
            {
                return;
            }

            object bob = new
            {
                result = "ok",
                node_id = node.Value
            };


            ScriptManager.Eval("ownerWindow.close({result:'ok', node_id:" + node.Value + "} );");

        }


    }
}