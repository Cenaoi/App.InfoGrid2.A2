using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.Core_Catalog
{
    public partial class EditTree : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.TreePanel1.Removeing += TreePanel1_Removeing;
            this.TreePanel1.Renaming += TreePanel1_Renaming;
            this.TreePanel1.Creating += TreePanel1_Creating;


            if (!IsPostBack) 
            {
                InitData();
            }
        }

        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = this.TreePanel1.NodeSelected;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = StringUtil.ToInt(node.Value);

            BIZ_CATALOG cata = decipher.SelectModelByPk<BIZ_CATALOG>(id);

            if (cata == null)
            {
                MessageBox.Alert("不能删除根节点");
                e.Cancel = true;
                return;
            }

            if (cata.SEC_LEVEL > 0)
            {
                MessageBox.Alert("您的权限不够，无法删除。");
                e.Cancel = true;
                return;
            }


            LightModelFilter filterCata = new LightModelFilter(typeof(BIZ_CATALOG));
            filterCata.And("PARENT_ID", id);
            filterCata.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            if (decipher.ExistsModels(filterCata))
            {
                e.Cancel = true;
                MessageBox.Alert("删除失败, 必须先删除目录下的‘工作表’和‘子目录’.");

                return;
            }



            cata.ROW_SID = -3;
            cata.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(cata, "ROW_SID", "ROW_DATE_DELETE");
        }

        void TreePanel1_Renaming(object sender, TreeNodeRenameEventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_CATALOG model = decipher.SelectModelByPk<BIZ_CATALOG>(StringUtil.ToInt(e.Node.Value));

            if (model.SEC_LEVEL > 0)
            {
                MessageBox.Alert("您的权限不够，无法重命名。");
                e.Cancel = true;
                return;
            }


            model.CATA_TEXT = e.Text;

            decipher.UpdateModelProps(model, "CATA_TEXT");
        }



        void TreePanel1_Creating(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TreeNode parent = this.TreePanel1.NodeSelected;

            int pId = StringUtil.ToInt(parent.Value);

            BIZ_CATALOG pCata = decipher.SelectModelByPk<BIZ_CATALOG>(pId);

            BIZ_CATALOG cata = new BIZ_CATALOG();


            cata.CATA_CODE = BizCatalogMgr.NewCode(pCata);
            cata.CATA_TEXT = "新建类别" + cata.CATA_CODE;
            cata.SEQ = 9999;
            cata.VISIBLE = true;
            cata.SEC_STRUCT_CODE = pCata?.SEC_STRUCT_CODE;

            EcUserState user = EcContext.Current.User;

            string comp_code = user.ExpandPropertys["OP_COMP_CODE"];

            cata.PARENT_ID = pId;


            decipher.InsertModel(cata);


            TreeNode node = new TreeNode();
            node.ParentId = parent.Value;
            node.Text = cata.CATA_TEXT;
            node.NodeType = "default";
            node.Value = cata.BIZ_CATALOG_ID.ToString();


            this.TreePanel1.Add(node);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(node.Value);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string cataIdentity = WebUtil.Query("cataIdentity");

            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_CATALOG));
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("CATA_IDENTITY",cataIdentity);
            lmFilter.And("CATA_TYPE_CODE","USER_ROOT");


            BIZ_CATALOG rootCata = decipher.SelectToOneModel<BIZ_CATALOG>(lmFilter);

            TreeNode rNode = new TreeNode(rootCata.CATA_TEXT, rootCata.BIZ_CATALOG_ID);
            rNode.ParentId = "0"; //rootCata.PARENT_ID.ToString();
            rNode.NodeType = "ROOT";
            rNode.StatusID = 0;
            rNode.Expand();

            this.TreePanel1.Add(rNode);


            List<BIZ_CATALOG> rootCatalogs = decipher.SelectModels<BIZ_CATALOG>(
                LOrder.By("SEQ ,BIZ_CATALOG_ID "),
                "ROW_SID >=0 AND PARENT_ID={0}", rootCata.BIZ_CATALOG_ID);

            foreach (BIZ_CATALOG cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.CATA_TEXT, cata.BIZ_CATALOG_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "default";
                tNode.StatusID = 0;

                this.TreePanel1.Add(tNode);
            }


        }

        /// <summary>
        /// 节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreePanel1_Selected(object sender, EventArgs e)
        {

            string struct_code = WebUtil.Query("struct_code");

            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                List<BIZ_CATALOG> models = decipher.SelectModels<BIZ_CATALOG>(LOrder.By("SEQ ,BIZ_CATALOG_ID "), "PARENT_ID={0} AND ROW_SID >= 0", id);

                foreach (BIZ_CATALOG model in models)
                {
                    TreeNode node2 = new TreeNode(model.CATA_TEXT, model.BIZ_CATALOG_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();

                node.ChildLoaded = true;
                node.Expand();

            }

            this.TreePanel1.OpenNode(node.Value);

            if (node.Value == "ROOT")
            {
                return;
            }

            BIZ_CATALOG cata = decipher.SelectModelByPk<BIZ_CATALOG>(id);


                MiniPager.Redirect("iform1",
                    string.Format("/App/InfoGrid2/View/Biz/Core_Catalog/CataEdit2.aspx?id={0}&struct_code=_{1}", id, struct_code));
        }



    }
}