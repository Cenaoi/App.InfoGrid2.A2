using App.BizCommon;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.BOM
{
    public partial class BOM091Tree : WidgetControl, IView
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
            this.TreePanel1.Creating += TreePanel1_Creating;

            if (!IsPostBack)
            {
                InitData();
            }

            
            




        }


        void TreePanel1_Creating(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            string id = node.Value;

            int utID = WebUtil.QueryInt("id");




            DbDecipher decipher = ModelAction.OpenDecipher();



            LModel lm091 = decipher.GetModelByPk("UT_091", utID);


            LModel lm219 = new LModel("UT_219");

            lm219["ROW_DATE_CREATE"] = DateTime.Now;

            TreeNode cnode = new TreeNode();

            if (id == "root")
            {
                lm219["BIZ_PARENT_ID"] = 0;
                cnode.ParentId = "root";


            }
            else
            {
                lm219["BIZ_PARENT_ID"] = id;
                cnode.ParentId = id;

            }
            lm219["UT_102_ID"] = utID;
            lm219["COL_20"] = lm091["COL_12"];

            decipher.InsertModel(lm219);


            cnode.Text = "新建节点";
            cnode.NodeType = "default";
            cnode.Value = lm219.Get<string>("ROW_IDENTITY_ID");


            this.TreePanel1.Add(cnode);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(node.Value);



        }



        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = e.Node;

            string id = node.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (id == "root")
            {
                int RID = WebUtil.QueryInt("id");

                LightModelFilter lmFilter = new LightModelFilter("UT_219");
                lmFilter.And("UT_091_ID", RID);
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                decipher.UpdateProps(lmFilter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });
                this.TreePanel1.Refresh();

                return;

            }

            int subID = int.Parse(id);


            List<int> ids = new List<int>();

            ids.Add(subID);



            RecursiveID(subID, ids);



            LightModelFilter lmFilter2 = new LightModelFilter("UT_219");
            lmFilter2.And("ROW_IDENTITY_ID", ids.ToArray(), Logic.In);
            lmFilter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            decipher.UpdateProps(lmFilter2, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });


            //e.Cancel = true;



        }

        /// <summary>
        /// 递归拿到所有子节点的ID
        /// </summary>
        void RecursiveID(int id, List<int> ids)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_219");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_PARENT_ID", id);

            List<LModel> lmList219 = decipher.GetModelList(lmFilter);

            foreach (var item in lmList219)
            {
                int PID = item.Get<int>("ROW_IDENTITY_ID");

                ids.Add(PID);

                RecursiveID(PID, ids);
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

            if (node == null) { return; }

            string id = node.Value;

            if (id == "root") { return; }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm219 = decipher.GetModelByPk("UT_219", id);



            EntityFormEngine efe = new EntityFormEngine();
            efe.Model = "UT_219";
            efe.SetData(this.FormLayout1, lm219);


        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            //	UT_102	吸塑指令单-订单明细
            LModel lm091 = decipher.GetModelByPk("UT_091", id);


            LightModelFilter lmFilter = new LightModelFilter("UT_219");
            lmFilter.And("UT_091_ID", id);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            //	UT_196	生产管理BOM配方表-构件明细
            List<LModel> lmList196 = decipher.GetModelList(lmFilter);


            TreeNode rootNode = new TreeNode(lm091["COL_3"] + "  " + lm091["COL_4"], "root");
            rootNode.ParentId = "0";
            rootNode.NodeType = "default";
            rootNode.StatusID = 0;
            rootNode.Expand();
            this.TreePanel1.Add(rootNode);


            foreach (var item in lmList196)
            {
                //上级ID
                int PID = item.Get<int>("BIZ_PARENT_ID");

                //节点名称
                string nodeName = string.Format("<b>{0}</b>  {1}  使用量：{2}  损耗率%：{3}",
                    item["COL_7"], item["COL_8"], item["COL_11"], item["COL_12"]);
                TreeNode tNode = new TreeNode(nodeName, item["ROW_IDENTITY_ID"]);

                tNode.NodeType = "default";
                tNode.StatusID = 0;

                if (PID == 0)
                {
                    tNode.ParentId = "root";
                }
                else
                {
                    tNode.ParentId = PID.ToString();
                }


                tNode.Expand();
                this.TreePanel1.Add(tNode);

            }
        }


        /// <summary>
        /// 保存按钮事件
        /// </summary>
        public void btnSave()
        {

            TreeNode node = this.TreePanel1.NodeSelected;

            if (node == null) { return; }

            string id = node.Value;

            if (id == "root") { return; }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm219 = decipher.GetModelByPk("UT_219", id);

            lm219.SetTakeChange(true);


            EntityFormEngine efe = new EntityFormEngine();
            efe.Model = "UT_219";
            efe.EditData(this.FormLayout1, lm219);

            decipher.UpdateModel(lm219, true);

            Toast.Show("保存成功了！");

        }

    }
}