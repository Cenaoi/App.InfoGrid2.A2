using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
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

namespace App.InfoGrid2.Sec.StructDefine
{
    public partial class TreeStruceDialog : WidgetControl, IView
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
            this.TreePanel1.Selected += TreePanel1_Selected;
            this.TreePanel1.Creating += TreePanel1_Creating;
            this.TreePanel1.Renaming += TreePanel1_Renaming;
            this.TreePanel1.Removeing += TreePanel1_Removeing;

            if (!this.IsPostBack)
            {
                InitData();
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {
            TreeNode node = e.Node;

            int id = StringUtil.ToInt(node.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();


            bool b = decipher.ExistsModels<SEC_STRUCT>("PARENT_ID={0} and ROW_SID >=0",id);

            if (b) 
            {
                e.Cancel = true;
                Toast.Show("有子节点，不能删除！");
                return;
            }


            SEC_STRUCT ss = decipher.SelectModelByPk<SEC_STRUCT>(id);

            ss.ROW_SID = -3;
            ss.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(ss, "ROW_SID", "ROW_DATE_DELETE");


        }

        /// <summary>
        /// 重命名节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Renaming(object sender, TreeNodeRenameEventArgs e)
        {
            TreeNode node = e.Node;

            int id = StringUtil.ToInt( node.Value);


            DbDecipher decipher = ModelAction.OpenDecipher();

            //拿到父节点的数据
            SEC_STRUCT ss = decipher.SelectModelByPk<SEC_STRUCT>(id);

            ss.STRUCE_TEXT = node.Text;

            decipher.UpdateModelProps(ss, "STRUCE_TEXT");


        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Creating(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);


            DbDecipher decipher = ModelAction.OpenDecipher();

            //拿到父节点的数据
            SEC_STRUCT parent = decipher.SelectModelByPk<SEC_STRUCT>(id);



            SEC_STRUCT ss = new SEC_STRUCT() 
            {
             PARENT_ID = id,
              ROW_DATE_CREATE = DateTime.Now,
            };

            //父节点的子节点数量加一
            parent.CHILD_IDENTITY++;

            ss.STRUCE_CODE = parent.STRUCE_CODE + parent.CHILD_IDENTITY.ToString("00");
            ss.STRUCE_TEXT = ss.STRUCE_CODE + "_结构";


            //新增节点到数据库中
            decipher.InsertModel(ss);

            decipher.UpdateModelProps(parent, "CHILD_IDENTITY");


            TreeNode ChileNode = new TreeNode();
            ChileNode.ParentId = id.ToString();
            ChileNode.Text = ss.STRUCE_TEXT;

            node.Value = ss.SEC_STRUCT_ID.ToString();

            this.TreePanel1.Add(ChileNode);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(ChileNode.Value);



        }

        void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                List<SEC_STRUCT> models = decipher.SelectModels<SEC_STRUCT>(LOrder.By("SEC_STRUCT_ID "), "PARENT_ID={0} AND ROW_SID >= 0", id);

                foreach (SEC_STRUCT model in models)
                {
                    TreeNode node2 = new TreeNode(model.STRUCE_TEXT, model.SEC_STRUCT_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();

                node.ChildLoaded = true;
                node.Expand();

            }

            this.TreePanel1.OpenNode(node.Value);

        }
        

        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            int rootId = 100;

            
            

            SEC_STRUCT pCata = decipher.SelectModelByPk<SEC_STRUCT>(rootId);

            if (pCata == null)
            {
                pCata = new SEC_STRUCT();
                pCata.SEC_STRUCT_ID = rootId;

                decipher.IdentityStop();

                decipher.InsertModel(pCata);

                decipher.IdentityRecover();
            }


            string struct_code = WebUtil.Query("struct_code");

            if (StringUtil.StartsWith(struct_code, "_"))
            {
                struct_code = struct_code.Substring(1);
            }
            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_STRUCT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("STRUCE_CODE", struct_code);
            SEC_STRUCT ssInfo = decipher.SelectToOneModel<SEC_STRUCT>(lmFilter);

            if (ssInfo == null)
            {
                return;
            }



            rootId = ssInfo.SEC_STRUCT_ID;


            List<SEC_STRUCT> rootCatalogs = decipher.SelectModels<SEC_STRUCT>(LOrder.By("PARENT_ID"), "ROW_SID >= 0 AND PARENT_ID = {0}", rootId);



            TreeNode rootNode = new TreeNode(ssInfo.STRUCE_TEXT, rootId);
            rootNode.ParentId = "0";
            rootNode.NodeType = "CATA";
            rootNode.StatusID = 0;
            rootNode.Expand();

            this.TreePanel1.Add(rootNode);


            foreach (SEC_STRUCT cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.STRUCE_TEXT, cata.SEC_STRUCT_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "CATA";
                tNode.StatusID = 0;

                this.TreePanel1.Add(tNode);
            }

            this.TreePanel1.Refresh();
        }

        public void GoSubmit()
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);
            
            DbDecipher decipher = ModelAction.OpenDecipher();
           
            SEC_STRUCT pCata = decipher.SelectModelByPk<SEC_STRUCT>(id);

            string rowJson = HWQ.Entity.ModelConvert.ToJson(pCata);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', row:" + rowJson + "});");
        }
    }
}