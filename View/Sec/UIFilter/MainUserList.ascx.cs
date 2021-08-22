using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using EasyClick.Web.Mini2;
using EC5.Utility;

namespace App.InfoGrid2.Sec.UIFilter
{
    public partial class MainUserList : WidgetControl, IView
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
                InitData();
            }
        }



        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<BIZ_C_MENU> rootCatalogs = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} and ROW_SID >=0", 0);

            //TreeNode rootNode = new TreeNode("编辑菜单", "ROOT");
            //rootNode.ParentId = "0";
            //rootNode.NodeType = "default";
            //rootNode.StatusID = 0;
            //rootNode.Expand();

            //this.TreePanel1.Add(rootNode);

            

            foreach (BIZ_C_MENU cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.NAME, cata.BIZ_C_MENU_ID);
                tNode.ParentId = "0";
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
            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                List<BIZ_C_MENU> models = decipher.SelectModels<BIZ_C_MENU>(
                    LOrder.By("SEQ ,BIZ_C_MENU_ID "),
                    "PARENT_ID={0} AND ROW_SID >= 0", id);

                foreach (BIZ_C_MENU model in models)
                {
                    TreeNode node2 = new TreeNode(model.NAME, model.BIZ_C_MENU_ID);

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
                id = int.Parse(node.Value);

                BIZ_C_MENU cata = decipher.SelectModelByPk<BIZ_C_MENU>(id);



                MiniPager.Redirect("iform1",
                    string.Format("UITableSetup.aspx?menuId={0}&pageTypeId={1}", id, cata.SEC_PAGE_TYPE_ID));
            }


        }


        private void ShowTableCol(string userCode, string pageId)
        {

        }
    }
}