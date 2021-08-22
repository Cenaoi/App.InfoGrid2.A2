using App.BizCommon;
using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.BOM
{
    public partial class BOMTree : WidgetControl, IView
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



            LModel lm102 = decipher.GetModelByPk("UT_281", utID);


            LModel lm196 = new LModel("UT_196");
            
            lm196["ROW_DATE_CREATE"] = DateTime.Now;

            TreeNode cnode = new TreeNode();

            if (id == "root") 
            {
                lm196["BIZ_PARENT_ID"] = 0;
                cnode.ParentId = "root";
                

            }
            else 
            {
                lm196["BIZ_PARENT_ID"] = id;
                cnode.ParentId = id;
               
            }
            lm196["UT_102_ID"] = utID;
            lm196["COL_20"] = lm102["COL_13"];

            decipher.InsertModel(lm196);

            
            cnode.Text = "新建节点";
            cnode.NodeType = "default";
            cnode.Value = lm196.Get<string>("ROW_IDENTITY_ID");


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

            string id =  node.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (id == "root") 
            {
                int RID = WebUtil.QueryInt("id");

                LightModelFilter lmFilter = new LightModelFilter("UT_196");
                lmFilter.And("UT_102_ID", RID);
                lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                decipher.UpdateProps(lmFilter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });
                this.TreePanel1.Refresh();

                return;

            }

            int subID = int.Parse(id);


            List<int> ids = new List<int>();

            ids.Add(subID);



            RecursiveID(subID, ids);



            LightModelFilter lmFilter2 = new LightModelFilter("UT_196");
            lmFilter2.And("ROW_IDENTITY_ID", ids.ToArray(), HWQ.Entity.Filter.Logic.In);
            lmFilter2.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            decipher.UpdateProps(lmFilter2, new object[]{"ROW_SID",-3,"ROW_DATE_DELETE",DateTime.Now});


            //e.Cancel = true;



        }

        /// <summary>
        /// 递归拿到所有子节点的ID
        /// </summary>
        void RecursiveID(int id,List<int> ids) 
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_196");
            lmFilter.And("ROW_SID",0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_PARENT_ID", id);

            List<LModel> lmList196 = decipher.GetModelList(lmFilter);

            foreach(var item in lmList196)
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

            if(node == null){return;}

            string id  = node.Value;

            if (id == "root") { return; }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm196 = decipher.GetModelByPk("UT_196",id);



            EntityFormEngine efe = new EntityFormEngine();
            efe.Model = "UT_196";
            efe.SetData(this.FormLayout1, lm196);


        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData() 
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            //	UT_102	吸塑指令单-订单明细
            LModel lm102 = decipher.GetModelByPk("UT_281",id);


            LightModelFilter lmFilter = new LightModelFilter("UT_196"); 
            lmFilter.And("UT_102_ID",id);
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

            //	UT_196	生产管理BOM配方表-构件明细
            List<LModel> lmList196 = decipher.GetModelList(lmFilter);


            TreeNode rootNode = new TreeNode(lm102["COL_3"] + "  " + lm102["COL_4"], "root");
                rootNode.ParentId = "0";
                rootNode.NodeType = "default";
                rootNode.StatusID = 0;
                rootNode.Expand();
                this.TreePanel1.Add(rootNode);


            foreach (var item in lmList196) 
            {
                //上级ID
                int  PID  = item.Get<int>("BIZ_PARENT_ID");

                //节点名称
                string nodeName = string.Format("<b>{0}</b>  {1}  使用量：{2}  损耗率%：{3}",
                    item["COL_7"], item["COL_8"], item["COL_11"], item["COL_12"]);
                TreeNode tNode = new TreeNode(nodeName, item["ROW_IDENTITY_ID"]);
                
                tNode.NodeType = "default";
                tNode.StatusID = 0;

                if(PID == 0)
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
        /// bbaocun
        /// </summary>
        public void btnSave() 
        {

            TreeNode node = this.TreePanel1.NodeSelected;

            if (node == null) { return; }

            string id = node.Value;

            if (id == "root") { return; }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm196 = decipher.GetModelByPk("UT_196", id);

            lm196.SetTakeChange(true);


            EntityFormEngine efe = new EntityFormEngine();
            efe.Model = "UT_196";
            efe.EditData(this.FormLayout1, lm196);

            decipher.UpdateModel(lm196, true);


            InitData();


            Toast.Show("保存成功了！");

        }

        /// <summary>
        /// 映射 数据过来
        /// </summary>
        public void MapData(int id) 
        {

            TreeNode node = this.TreePanel1.NodeSelected;

            string ids = node.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm083 = decipher.GetModelByPk("UT_083", id);

            if (ids == "root") 
            {
                MessageBox.Alert("根节点不能映射数据！");   
                return;
            }

            LModel lm196 = decipher.GetModelByPk("UT_196", ids);

            MapMgr.MapData(78, lm083, lm196);

            decipher.UpdateModel(lm196, true);

            EntityFormEngine efe = new EntityFormEngine();
            efe.Model = "UT_196";
            efe.SetData(this.FormLayout1, lm196);

        }


    }
}