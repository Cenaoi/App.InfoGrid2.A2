using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.Core_Menu
{
    public partial class EditMenuTree : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            this.TreePanel1.Creating += new EventHandler(TreePanel1_Creating);
            this.TreePanel1.Renaming += new TreeNodeRenameEventHander(TreePanel1_Renaming);
            this.TreePanel1.Removeing += new TreeNodeCancelEventHander(TreePanel1_Removeing);

            this.TreePanel1.NodeMoved += new TreeNodeMoveEventHander(TreePanel1_NodeMoved);

            if(!IsPostBack)
            {
                InitData();
            }
        }

        void TreePanel1_NodeMoved(object sender, TreeNodeMoveEventArgs e)
        {

            int id = StringUtil.ToInt(e.NodeId);

            int pId = 0;

            if (id == pId)
            {
                MessageBox.Alert("节点转移失败.");
                return;
            }

            if (e.ParentId != "ROOT")
            {
                pId = StringUtil.ToInt(e.ParentId);
            }

            DbDecipher decipher = ModelAction.OpenDecipher();



            BIZ_C_MENU curModel = decipher.SelectModelByPk<BIZ_C_MENU>(id);

            #region 这个是要移动到快捷首页里面的

            //原来的跟节点ID
            int SourceId = RecursiveGetRootId(decipher, id);

            //目标跟节点ID
            int TargetId = RecursiveGetRootId(decipher, pId);


            //如果 原来的更节点  和目标跟节点不同  并且目标根节点 等于 70  的话就创建节点
            if (TargetId != SourceId && TargetId == 70)
            {
                CreateMenu(curModel, pId, e.Position);

                return;
            }

            #endregion

  
            SortByPosition(e.Position, pId, id,curModel);




        }

    


        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {

            TreeNode node = this.TreePanel1.NodeSelected;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = StringUtil.ToInt(node.Value);

            LightModelFilter filterCata = new LightModelFilter(typeof(BIZ_C_MENU));
            filterCata.And("PARENT_ID", id);
            filterCata.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            if (decipher.ExistsModels(filterCata))
            {
                e.Cancel = true;
                MessageBox.Alert("删除失败, 必须先删除目录下的‘工作表’和‘子目录’.");

                return;
            }

            BIZ_C_MENU cata = decipher.SelectModelByPk<BIZ_C_MENU>(id);
            if(cata == null)
            {
                MessageBox.Alert("不能删除根节点");
                e.Cancel = true;
                return;
            }

            cata.ROW_SID = -3;
            cata.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(cata, "ROW_SID", "ROW_DATE_DELETE");    
        }

        void TreePanel1_Renaming(object sender, TreeNodeRenameEventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_C_MENU model = decipher.SelectModelByPk<BIZ_C_MENU>(StringUtil.ToInt(e.Node.Value));

            model.NAME = e.Text;

            decipher.UpdateModelProps(model, "NAME");
        }

        void TreePanel1_Creating(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            TreeNode parent = this.TreePanel1.NodeSelected;

            BIZ_C_MENU cata = new BIZ_C_MENU();
            cata.MENU_ENABLED = true;

            cata.NAME = "新建菜单";
            cata.SEQ = 9999;

            if (parent.Value == "ROOT")
            {
                cata.PARENT_ID = 0;
            }
            else 
            {
                cata.PARENT_ID = StringUtil.ToInt(parent.Value);
            }
            


            decipher.InsertModel(cata);


            TreeNode node = new TreeNode();
            node.ParentId = parent.Value;
            node.Text = cata.NAME;
            node.NodeType = "default";
            node.Value = cata.BIZ_C_MENU_ID.ToString();
            

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

            //List<BIZ_C_MENU> rootCatalogs = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} and ROW_SID >=0", 0);


            //foreach (BIZ_C_MENU cata in rootCatalogs)
            //{
            //    TreeNode tNode = new TreeNode(cata.NAME, cata.BIZ_C_MENU_ID);
            //    tNode.ParentId = cata.PARENT_ID.ToString();
            //    tNode.NodeType = "default";
            //    tNode.StatusID = 0;
            //    tNode.Expand();

            //    this.TreePanel1.Add(tNode);
            //}

            List<BIZ_C_MENU> rootCatalogs = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "ROW_SID >=0 ");

            foreach (BIZ_C_MENU cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.NAME, cata.BIZ_C_MENU_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "default";
                tNode.StatusID = 0;

                if (cata.BIZ_C_MENU_ID == 100)
                {
                    tNode.Expand();
                }

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
                List<BIZ_C_MENU> models = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "),"PARENT_ID={0} AND ROW_SID >= 0", id);

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
                    string.Format("/App/InfoGrid2/View/Biz/Core_Menu/EditMenuInfo.aspx?id={0}", id));
            }


        }


        /// <summary>
        /// 递归获取根节点
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="pId">父键</param>
        int RecursiveGetRootId(DbDecipher decipher,int pId)
        {
            BIZ_C_MENU curModel = decipher.SelectModelByPk<BIZ_C_MENU>(pId);


            if (curModel.PARENT_ID == 0 )
            {
                return pId;
            }

            return RecursiveGetRootId(decipher, curModel.PARENT_ID);
        }


        /// <summary>
        /// 递归获取是否在于面板下面
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="pId">目标父键</param>
        BIZ_C_MENU RecursiveGetPID(DbDecipher decipher, int pId)
        {
            BIZ_C_MENU curModel = decipher.SelectModelByPk<BIZ_C_MENU>(pId);


            if (curModel == null)
            {
                return null;
            }


            if (curModel.MENU_TYPE_ID == "QUICK" || curModel.MENU_TYPE_ID == "LIST")
            {
                return curModel;
            }

            
        
            return RecursiveGetPID(decipher, curModel.PARENT_ID);
           
            

         
        }


        /// <summary>
        /// 创建菜单节点
        /// </summary>
        /// <param name="curModel">数据源节点</param>
        /// <param name="pId">父ID</param>
        /// <param name="position">排序索引</param>
        void CreateMenu(BIZ_C_MENU curModel, int pId,int position)
        {
             DbDecipher decipher = ModelAction.OpenDecipher();

            //判断这个目标跟节点是否是快捷面板 或 列表面板
            BIZ_C_MENU PanModel = RecursiveGetPID(decipher, pId);

            //如果是移动到面板下面 就 只 创建 一个节点，否则 创建三个 菜单节点
            if (PanModel != null && (PanModel.MENU_TYPE_ID == "LIST" || PanModel.MENU_TYPE_ID == "QUICK"))
            {
                BIZ_C_MENU bcm1 = new BIZ_C_MENU();

                curModel.CopyTo(bcm1);

                bcm1.PARENT_ID = pId;

                bcm1.ROW_DATE_CREATE = DateTime.Now;
                bcm1.ROW_DATE_UPDATE = DateTime.Now;

                decipher.InsertModel(bcm1);


                SortByPosition(position, pId, bcm1.BIZ_C_MENU_ID,bcm1);


                TreeNode node1 = new TreeNode();
                node1.ParentId = pId.ToString();
                node1.Text = bcm1.NAME;
                node1.ChildLoaded = false;
                node1.NodeType = "default";
                node1.Value = bcm1.BIZ_C_MENU_ID.ToString();


                this.TreePanel1.Add(node1);

                this.TreePanel1.Refresh();

                return;
            }

       
            BIZ_C_MENU ParentModel = decipher.SelectModelByPk<BIZ_C_MENU>(pId);


            BIZ_C_MENU bcm = new BIZ_C_MENU();

            curModel.CopyTo(bcm);

            bcm.PARENT_ID = pId;

            bcm.ROW_DATE_CREATE = DateTime.Now;
            bcm.ROW_DATE_UPDATE = DateTime.Now;

            decipher.InsertModel(bcm);


            SortByPosition(position, pId, bcm.BIZ_C_MENU_ID,bcm);


            TreeNode node = new TreeNode();
            node.ParentId = pId.ToString();
            node.Text = bcm.NAME;
            node.ChildLoaded = false;
            node.NodeType = "default";
            node.Value = bcm.BIZ_C_MENU_ID.ToString();


            this.TreePanel1.Add(node);



            //这是快捷面板
            BIZ_C_MENU bcm2 = new BIZ_C_MENU();

            curModel.CopyTo(bcm2);

            bcm2.PARENT_ID = bcm.BIZ_C_MENU_ID;
            bcm2.MENU_TYPE_ID = "QUICK";
            bcm2.NAME = "快捷面板";

            bcm2.ROW_DATE_CREATE = DateTime.Now;
            bcm2.ROW_DATE_UPDATE = DateTime.Now;

            decipher.InsertModel(bcm2);

            //这是列表面板
            BIZ_C_MENU bcm3 = new BIZ_C_MENU();

            curModel.CopyTo(bcm2);

            bcm3.PARENT_ID = bcm.BIZ_C_MENU_ID;
            bcm3.MENU_TYPE_ID = "LIST";
            bcm3.NAME = "列表面板";
            bcm3.MENU_ENABLED = true;

            bcm3.ROW_DATE_CREATE = DateTime.Now;
            bcm3.ROW_DATE_UPDATE = DateTime.Now;

            decipher.InsertModel(bcm3);

            this.TreePanel1.Refresh();
        }

        /// <summary>
        /// 根据移动的位置排序
        /// </summary>
        /// <param name="position">排序索引</param>
        /// <param name="pId">父ID</param>
        /// <param name="id">自身ID</param>
        /// <param name="curModel">当前节点</param>
        void SortByPosition(int position, int pId, int id, BIZ_C_MENU curModel)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            LModelList<BIZ_C_MENU> models;

            if (curModel.PARENT_ID == pId)
            {
                models = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ASC,BIZ_C_MENU_ID ASC"), "PARENT_ID = {0} AND BIZ_C_MENU_ID <> {1} AND ROW_SID >=0", pId, id);
            }
            else
            {
                models = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ASC,BIZ_C_MENU_ID ASC"), "PARENT_ID = {0} AND ROW_SID >=0", pId);
            }

            models.Insert(position, curModel);

            int i = 0;


            foreach (BIZ_C_MENU item in models)
            {
                item.SEQ = i++;

                if (curModel == item)
                {
                    item.PARENT_ID = pId;
                    decipher.UpdateModelProps(item, "PARENT_ID", "SEQ");
                }
                else
                {
                    decipher.UpdateModelProps(item, "SEQ");
                }
            }
        }


    }
}