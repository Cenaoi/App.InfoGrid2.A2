using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.OneMap
{
    public partial class ShowTableAll : WidgetControl, IView
    {
        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
                //this.store1.DataBind();
                
            }
        }


        /// <summary>
        /// 选择表
        /// </summary>
        public void SelectTable()
        {


            if (this.table1.CheckedRows.Count <= 0)
            {
                EasyClick.Web.Mini.MiniHelper.Alert("请选择工作表！");
                return;
            }


            DataRecordCollection drc = this.table1.CheckedRows;

            string id = drc[0].Id;

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',id:'" + id + "'});");
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

            if (node.Value != "ROOT")
            {
                int id = int.Parse(node.Value);

                IG2_CATALOG cata = decipher.SelectModelByPk<IG2_CATALOG>(id);

                List<IG2_TABLE> tableList = decipher.SelectModels<IG2_TABLE>("IG2_CATALOG_ID={0} and TABLE_TYPE_ID='{1}' and ROW_SID >=0", id, cata.DEFAULT_TABLE_TYPE);

                this.store1.RemoveAll();
                this.store1.AddRange(tableList);
                //this.store1.Refresh();

            }


        }



        protected override void OnLoad(EventArgs e)
        {

            if (!this.IsPostBack)
            {
                InitData();
            }

        }



        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<IG2_CATALOG> rootCatalogs = decipher.SelectModels<IG2_CATALOG>("PARENT_ID={0}", 0);

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

            //TreeNode moneViewNode = new TreeNode("关联视图表", "ROOT_VIEW");
            //moneViewNode.ParentId = "ROOT";
            //moneViewNode.NodeType = "CATA";
            //moneViewNode.StatusID = 0;

            //this.TreePanel1.Add(moneViewNode);
        }



    }
}