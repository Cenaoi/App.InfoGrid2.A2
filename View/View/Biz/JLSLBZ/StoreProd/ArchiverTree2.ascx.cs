using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.Biz.JLSLBZ.StoreProd
{
    public partial class ArchiverTree2 : WidgetControl, IView
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
            this.TreePanel1.Creating += new EventHandler(TreePanel1_Creating);
            this.TreePanel1.Removeing += TreePanel1_Removeing;
            if (!IsPostBack)
            {
                InitData();
            }
        }

        
        void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (node.Value == "ROOT")
            {
                return;
            }


            int menuID = WebUtil.QueryInt("menu_id",218);


            string id = node.Value.Substring(2);

            LModel lm = decipher.GetModelByPk("UT_087", id);

            StringBuilder sb = new StringBuilder();

            List<string> ids = new List<string>();

            GetID(id, ids);

            int i = 0;



            foreach (var item in ids)
            {
                if (i++ > 0) { sb.Append(","); }

                sb.Append(item);
            }


            MiniPager.Redirect("iform1", string.Format("/app/infogrid2/view/onetable/tablepreview.aspx?id=608&menu_id={1}&StoreID={0}", sb.ToString(),menuID));


        }

        /// <summary>
        /// 获取下面的子节点的所有仓库编码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sb"></param>
        void GetID(string id, List<string> sb)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModelByPk("UT_087", id);

            if (!string.IsNullOrEmpty(lm.Get<string>("COL_1")))
            {
                sb.Add(lm.Get<string>("COL_1"));
            }

            LightModelFilter lmFilter = new LightModelFilter("UT_087");
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("COL_9", id);
            List<LModel> lmList = decipher.GetModelList(lmFilter);

            foreach (var item in lmList)
            {
                GetID(item.Get<string>("ROW_IDENTITY_ID"), sb);
            }

        }





        private void InitData()
        {



            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter fileter = new LightModelFilter("UT_087");
            fileter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            List<LModel> nodeList = decipher.GetModelList(fileter);

            int RootID = 0;

            foreach (var item in nodeList)
            {
                if (item.Get<int>("COL_9") == 0)
                {
                    RootID = item.Get<int>("ROW_IDENTITY_ID");
                    TreeNode rootNode = new TreeNode();
                    rootNode.Text = string.Format("{0}({1})", item.Get<string>("COL_2"), item.Get<string>("COL_1"));
                    rootNode.Value = "ROOT";
                    rootNode.ParentId = "0";
                    rootNode.Expand();

                    this.TreePanel1.Add(rootNode);
                    continue;
                }

            }

            foreach (var item in nodeList)
            {
                TreeNode node1 = new TreeNode();
                node1.Text = string.Format("{0}({1})", item.Get<string>("COL_2"), item.Get<string>("COL_1"));
                node1.Value = "OP" + item.Get<int>("ROW_IDENTITY_ID");

                if (item.Get<int>("COL_9") == RootID)
                {
                    node1.ParentId = "ROOT";
                }
                else
                {
                    node1.ParentId = "OP" + item.Get<int>("COL_9");
                }
                node1.Expand();

                this.TreePanel1.Add(node1);
            }
        }


        void TreePanel1_Creating(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TreeNode parent = this.TreePanel1.NodeSelected;

            LModel lm = new LModel("UT_087");
            lm["COL_2"] = "新建目录";
            lm["COL_9"] = parent.Value.Substring(2);

            try
            {
                decipher.InsertModel(lm);
            }
            catch (Exception ex)
            {
                log.Error("插入节点数据失败了！");
                MessageBox.Alert("创建节点失败！");
                return;
            }
            TreeNode node = new TreeNode();
            node.ParentId = parent.Value;
            node.Text = "新建目录";
            node.Value = "OP" + lm.Get<int>("ROW_IDENTITY_ID");

            this.TreePanel1.Add(node);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(node.Value);
        }

        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = this.TreePanel1.NodeSelected;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = int.Parse(node.Value.Substring(2));

            LightModelFilter filterCata = new LightModelFilter("UT_087");
            filterCata.And("COL_9", id);

            if (decipher.ExistsModels(filterCata))
            {
                MessageBox.Alert("删除失败, 必须先删除目录下的‘工作表’和‘子目录’.");

                return;
            }

            LModel cata = decipher.GetModelByPk("UT_087", id);

            cata["ROW_SID"] = -3;
            cata["ROW_DATE_DELETE"] = DateTime.Now;

            decipher.UpdateModelProps(cata, "ROW_SID", "ROW_DATE_DELETE");


        }

    }
}