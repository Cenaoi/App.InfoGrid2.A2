using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OnePage
{
    public partial class AreaStepNew1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitData();
                //this.store1.DataBind();
            }
        }


        public void GoNext()
        {
            string tableIdStr = this.store1.CurDataId;

            int tableId = StringUtil.ToInt(tableIdStr);
            Guid opGuid = WebUtil.QueryGuid("TMP_GUID", Guid.NewGuid());
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                DeleteTmpTable_ForGuid(opGuid);

                TableSet tSet = TableSet.Select(decipher, tableId);

                TmpTableSet ttSet = tSet.ToTmpTableSet();

                ttSet.Insert(decipher, opGuid, session);

                string url = string.Format("AreaStepNew2.aspx?page_id={0}&area_id={1}&area_type_id={2}&tmp_guid={3}",
                    pageId, areaId, areaTypeId, opGuid);

                MiniPager.Redirect(url);

            }
            catch (Exception ex)
            {
                log.Error("创建临时表失败",ex);
            }
        }

        /// <summary>
        /// 删除临时表数据
        /// </summary>
        /// <param name="guid"></param>
        private int DeleteTmpTable_ForGuid(Guid guid)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter tabFilter = new LightModelFilter(typeof(IG2_TMP_TABLE));
            tabFilter.And("TMP_GUID", guid);

            LightModelFilter colFilter = new LightModelFilter(typeof(IG2_TMP_TABLECOL));
            colFilter.And("TMP_GUID", guid);


            int n = decipher.DeleteModels(tabFilter);
            n += decipher.DeleteModels(colFilter);

            return n;
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