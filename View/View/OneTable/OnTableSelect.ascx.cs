using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;

namespace App.InfoGrid2.View.OneTable
{
    public partial class OnTableSelect : WidgetControl, IView
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

            base.OnLoad(e);
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

            int cataId = StringUtil.ToInt32(node.Value);

            TableRefresh(cataId);

        }



        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<IG2_CATALOG> rootCatalogs = decipher.SelectModels<IG2_CATALOG>("ROW_SID >= 0 AND PARENT_ID={0} AND DEFAULT_TABLE_TYPE='TABLE'", 101);

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


        /// <summary>
        /// 右边表列表,刷新
        /// </summary>
        private void TableRefresh(int cataId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("IG2_CATALOG_ID", cataId);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.Fields = StringUtil.Split("IG2_TABLE_ID,TABLE_NAME,DISPLAY,IS_BIG_TITLE_VISIBLE,REMARK,ROW_DATE_CREATE", ",");

            List<LModel> tables = decipher.GetModelList(filter);

            this.store1.RemoveAll();
            this.store1.AddRange(tables);
        }

        public void GoOk_Click()
        {
            SModelList tables = new SModelList();

            foreach (var item in this.table1.CheckedRows)
            {
                SModel tm = new SModel();
                tm["IG2_TABLE_ID"] = item.Id;
                tm["TABLE_NAME"] = item.Fields["TABLE_NAME"].Value;
                tm["TABLE_DISPLAY"] = item.Fields["DISPLAY"].Value;

                tables.Add(tm);
            }

            SModel json = new SModel();
            json["result"] = "ok";
            json["data"] = tables;

            string jsonStr = json.ToJson();

            ScriptManager.Eval("ownerWindow.close(" + jsonStr + ")");
        }
    }
}