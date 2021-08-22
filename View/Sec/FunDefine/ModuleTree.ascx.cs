using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using EasyClick.BizWeb.UI;
using HWQ.Entity.Filter;
using EC5.Utility;
using EasyClick.Web.Mini;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.FunDefine
{
    public partial class ModuleTree : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        TreeViewAction<SEC_FUN_DEF, int> m_TreeAction;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            m_TreeAction = new TreeViewAction<SEC_FUN_DEF, int>(this.TreeView1);
            m_TreeAction.NodeTypeChange("2", "page");

            m_TreeAction.FilterSearch("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            m_TreeAction.FilterSearch("FUN_TYPE_ID", new int[] { 0, 2 }, Logic.In);


            if (!this.IsPostBack)
            {
                m_TreeAction.LoadNodes();
            }
        }


        /// <summary>
        /// 创建节点目录
        /// </summary>
        public void CreateNodeForDir()
        {
            int nodeId = StringUtil.ToInt(this.TreeView1.FocusValue);

            EcView.SetClosedClientScript("New_FormClosed(sender,e);");
            EcView.showDialog("ModuleNew.aspx?type=module&pid=" + nodeId, "创建目录",400,300);

        }

        /// <summary>
        /// 重新加载节点
        /// </summary>
        /// <param name="nodeId"></param>
        public void LoadNode(int nodeId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_FUN_DEF m = decipher.SelectModelByPk<SEC_FUN_DEF>(nodeId);

            TreeNode node = new TreeNode(m.TEXT,m.SEC_FUN_DEF_ID);

            if (m.FUN_TYPE_ID == 2)
            {
                node.NodeType = "page";
            }

            string parentID = this.TreeView1.ClientID + "_" + m.PARENT_ID;

            this.TreeView1.AddNode(parentID, node);
            this.TreeView1.Refresh(parentID);
        }


        public void CreateNodeForModule()
        {
            int nodeId = StringUtil.ToInt(this.TreeView1.FocusValue);

            EcView.SetClosedClientScript("New_FormClosed(sender,e);");
            EcView.showDialog("ModuleNew.aspx?type=fun&pid=" + nodeId, "创建模块", 400, 300);
        }


        public void RenameNode()
        {
            int nodeId = StringUtil.ToInt(this.TreeView1.FocusValue);

            EcView.SetClosedClientScript("Edit_FormClosed(sender,e);");

            EcView.showDialog("ModuleEdit.aspx?id=" + nodeId, "修改", 400, 300);
        }

        /// <summary>
        /// 刷新节点文本
        /// </summary>
        /// <param name="nodeId"></param>
        public void ResetNodeText(int nodeId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_FUN_DEF m = decipher.SelectModelByPk<SEC_FUN_DEF>(nodeId);


            string nodeIdStr = this.TreeView1.ClientID + "_" + m.SEC_FUN_DEF_ID;

            this.TreeView1.ResetNodeText(nodeIdStr, m.TEXT);
        }


        public void DeleteNode()
        {
            int nodeId = StringUtil.ToInt(this.TreeView1.FocusValue);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_FUN_DEF m = decipher.SelectModelByPk<SEC_FUN_DEF>(nodeId);

            if (decipher.ExistsModels<SEC_FUN_DEF>("PARENT_ID={0} AND FUN_TYPE_ID <> 4", nodeId))
            {
                MiniHelper.Alert("存在子节点,无法删除.");
                return;
            }

            try
            {
                m.ROW_DATE_DELETE = DateTime.Now;
                m.ROW_STATUS_ID = -3;

                decipher.UpdateModelProps(m, "ROW_DATE_DELETE", "ROW_STATUS_ID");


                decipher.UpdateProps<SEC_FUN_DEF>(string.Format("PARENT_ID={0}", nodeId), 
                    new object[] { "ROW_DATE_DELETE",DateTime.Now, "ROW_STATUS_ID",-3 });

                string pIdStr = this.TreeView1.ClientID + "_" + m.PARENT_ID;
                string nodeIdStr = this.TreeView1.ClientID + "_" + nodeId;

                this.TreeView1.RemoveNode(nodeIdStr);
                this.TreeView1.Refresh(pIdStr);


            }
            catch (Exception ex)
            {
                log.Error(ex);

                MiniHelper.Alert("删除失败");
            }
        }

    }
}