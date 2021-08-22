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
    public partial class TreeStruceCheckDialog : WidgetControl, IView
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

            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        private void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();
            

            int id = WebUtil.QueryInt("id");
            
            //拿到用户对象
            SEC_LOGIN_ACCOUNT sla = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(id);
            
            if (sla == null) { return; }

            //一个编码集合
            string[] codeList = StringUtil.Split(sla.REF_ARR_STRUCT_CODE);




            int rootId = 100;

            SEC_STRUCT pCata = decipher.SelectModelByPk<SEC_STRUCT>(rootId);

            if (pCata == null)
            {
                pCata = new SEC_STRUCT();
                pCata.SEC_STRUCT_ID = 100;

                decipher.IdentityStop();

                decipher.InsertModel(pCata);

                decipher.IdentityRecover();
            }



            List<SEC_STRUCT> rootCatalogs = decipher.SelectModels<SEC_STRUCT>(LOrder.By("PARENT_ID"), "ROW_SID >= 0 AND SEC_STRUCT_ID <> {0}", rootId);

            TreeNode rootNode = new TreeNode("根结构", rootId);
            rootNode.ParentId = "0";
            rootNode.NodeType = "CATA";
            rootNode.StatusID = 0;
            rootNode.Expand();
            

            this.TreePanel1.Add(rootNode);

            List<string> checkNodes = new List<string>();   //选中的节点ID

            foreach (SEC_STRUCT cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.STRUCE_TEXT, cata.SEC_STRUCT_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "CATA";
                tNode.StatusID = 0;

                //看看是否已经有记录了
                bool exist = ArrayUtil.Exist(codeList,cata.STRUCE_CODE);

                if (exist)
                {
                    checkNodes.Add(cata.SEC_STRUCT_ID.ToString());
                    //tNode.Checked = true;
                }

                this.TreePanel1.Add(tNode);
            }

            this.TreePanel1.CheckNodes(checkNodes.ToArray());

            //this.TreePanel1.Refresh();
        }

        public void GoSubmit()
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            string[] ids = this.TreePanel1.GetCheckeds();


            if(ids.Length ==0 )
            {
                Toast.Show("请选择结构！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

         

            LModelList<SEC_STRUCT> rootCatalogs = decipher.SelectModelsIn<SEC_STRUCT>("SEC_STRUCT_ID",ids);

            string idStr = string.Empty;

            for (int i = 0; i < rootCatalogs.Count; i++)
            {
                if (i > 0) { idStr += ","; }

                idStr += rootCatalogs[i].STRUCE_CODE;

            }

           

            int id = WebUtil.QueryInt("id");

            decipher.UpdateModelByPk<SEC_LOGIN_ACCOUNT>(id, new object[] { 
                "REF_ARR_STRUCT_CODE", idStr, 
                "ROW_DATE_UPDATE", DateTime.Now });

           EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok'});");
        }
    }
}