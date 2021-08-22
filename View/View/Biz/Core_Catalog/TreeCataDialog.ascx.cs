using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.Core_Catalog
{
    public partial class TreeCataDialog : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            int rootId = WebUtil.QueryInt("pid", -1) ;

            if (rootId <= 0)
            {
                return;
            }

            BIZ_CATALOG pCata = decipher.SelectModelByPk<BIZ_CATALOG>(rootId);


            List<BIZ_CATALOG> rootCatalogs = decipher.SelectModels<BIZ_CATALOG>(
                LOrder.By("PARENT_ID"),
                "ROW_SID >= 0 AND PARENT_ID = {0}", rootId);

            TreeNode rootNode = new TreeNode(pCata.CATA_TEXT, pCata.BIZ_CATALOG_ID);
            rootNode.ParentId = "0";
            rootNode.NodeType = "CATA";
            rootNode.StatusID = 0;
            rootNode.Expand();

            this.TreePanel1.Add(rootNode);


            foreach (BIZ_CATALOG cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.CATA_TEXT, cata.BIZ_CATALOG_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "CATA";
                tNode.StatusID = 0;

                this.TreePanel1.Add(tNode);
            }

            this.TreePanel1.Refresh();
        }


        protected void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                int parentId = StringUtil.ToInt(node.Value);


                List<BIZ_CATALOG> models = decipher.SelectModels<BIZ_CATALOG>(
                    "ROW_SID >= 0 AND PARENT_ID={0}", parentId);

                foreach (BIZ_CATALOG cata in models)
                {
                    TreeNode node2 = new TreeNode(cata.CATA_TEXT, cata.BIZ_CATALOG_ID);

                    node2.ParentId = cata.PARENT_ID.ToString(); ;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();


                node.ChildLoaded = true;
                node.Expand();
            }


        }


        public void GoSubmit()
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_CATALOG pCata = decipher.SelectModelByPk<BIZ_CATALOG>(id);

            string rowJson = HWQ.Entity.ModelConvert.ToJson(pCata);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', row:" + rowJson + "});");
        }
    }
}