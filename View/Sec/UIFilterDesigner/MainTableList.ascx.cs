using System;
using System.Collections.Generic;
using System.Xml;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json.Linq;

namespace App.InfoGrid2.Sec.UIFilterDesigner
{
    public partial class MainTableList : WidgetControl, IView
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
        void InitData() 
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
                tNode.Tag = "MENU|" + cata.BIZ_C_MENU_ID;

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
                List<BIZ_C_MENU> models = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} AND ROW_SID >= 0", id);

                foreach (BIZ_C_MENU model in models)
                {
                    TreeNode node2 = new TreeNode(model.NAME, model.BIZ_C_MENU_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    node2.Tag = "MENU|" + model.BIZ_C_MENU_ID;

                    this.TreePanel1.Add(node2);
                }


                node.Expand();

                ///创建表格节点
                CreateTableNode(node);


                node.ChildLoaded = true;

            }


            this.TreePanel1.Refresh();




            this.TreePanel1.OpenNode(node.Value);





            if (node.Value != "ROOT")
            {


                string[] ids = node.Tag.Split('|');


                if (ids[0] == "VIEW")
                {


                    string menuID = ids[1];     ///菜单ID
                    string pageID = ids[2];     ///窗体ID
                    string tabID = ids[3];      ///复杂表 区域ID
                    string tabName = ids[4];    ///复杂表 区域类型
                    string fieldName = ids[5];  ///内部列名
                    string tableID = ids[6];    ///弹出窗口ID


                    MiniPager.Redirect("iform1",
                       string.Format("UserList.aspx?menuId={0}&pageId={1}&tableId={2}&tabId={3}&tabName={4}&fieldName={5}", menuID, pageID, tableID, tabID, tabName, fieldName));


                }
                else if (ids[0] == "MENU")
                {
                    id = int.Parse(node.Value);

                    BIZ_C_MENU cata = decipher.SelectModelByPk<BIZ_C_MENU>(id);



                    MiniPager.Redirect("iform1",
                        string.Format("/App/InfoGrid2/Sec/UIFilter/UITableSetup.aspx?menuId={0}&pageTypeId={1}", id, cata.SEC_PAGE_TYPE_ID));
                }



                
            }


        }



        /// <summary>
        /// 创建表格节点
        /// </summary>
        private void CreateTableNode(TreeNode node)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_C_MENU menu = decipher.SelectModelByPk<BIZ_C_MENU>(node.Value);

            if (menu.SEC_PAGE_TYPE_ID == "TABLE")
            {

                CreateTableOne(menu.SEC_PAGE_ID, node, "", "", menu);



            }
            else if (menu.SEC_PAGE_TYPE_ID == "PAGE")
            {

                LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
                filter.And("IG2_TABLE_ID", menu.SEC_PAGE_ID);
                filter.And("TABLE_TYPE_ID", "PAGE");
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.Fields = new string[] { "PAGE_TEMPLATE" };

                string pageTemplate = decipher.ExecuteScalar<string>(filter);

                if (StringUtil.IsBlank(pageTemplate))
                {
                    return;
                }

                List<PageTable> pTables = new List<PageTable>();

                ParseTemplate(pageTemplate, pTables);

                foreach (PageTable pTable in pTables)
                {
                    CreateTableOne(pTable.TableId, node, pTable.ID, pTable.EcType, menu);
                }


            }

        }


        private void ParseTemplate(string pageTemplate, List<PageTable> pTables)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pageTemplate);

            XmlNode root = doc.DocumentElement;
            XmlNode xBody = root.SelectSingleNode("body");


            ParseTemplateNode(xBody, pTables);

        }


        private void ParseTemplateNode(XmlNode xParent, List<PageTable> pTables)
        {
            foreach (XmlNode xNode in xParent.ChildNodes)
            {
                string ecType = XmlUtil.GetAttrValue(xNode, "ec-type");
                string ecMainView = XmlUtil.GetAttrValue(xNode, "ec-main-view");
                string ecMainName = XmlUtil.GetAttrValue(xNode, "ec-main-name");
                string id = XmlUtil.GetAttrValue(xNode, "id");

                if (!StringUtil.IsBlank(ecMainView) && !StringUtil.IsBlank(ecMainName))
                {
                    PageTable pt = new PageTable();
                    pt.EcType = ecType;
                    pt.ID = id;
                    pt.TableName = ecMainName;
                    pt.TableId = StringUtil.ToInt(ecMainView);

                    pTables.Add(pt);
                }

                ParseTemplateNode(xNode, pTables);
            }

        }

        /// <summary>
        /// 创建一张表格的节点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="node"></param>
        void CreateTableOne(int id, TreeNode node, string tabID, string tabName, BIZ_C_MENU menu)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);

            TreeNode nodeTA = new TreeNode(it.DISPLAY + "(" + tabID + ")", "TA_" + id);

            nodeTA.ParentId = node.Value;
            nodeTA.StatusID = 0;
            nodeTA.Tag = "TABLE|" + id;

            this.TreePanel1.Add(nodeTA);

            nodeTA.ChildLoaded = true;


            LightModelFilter lmFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            lmFilter.And("ACT_MODE", "TABLE");
            lmFilter.And("V_LIST_MODE_ID", new string[] { "TriggerColumn", "SelectColumn" }, Logic.In);
            lmFilter.And("IG2_TABLE_ID", id);

            List<IG2_TABLE_COL> itcList = decipher.SelectModels<IG2_TABLE_COL>(lmFilter);

            ///这是弹出界面的表ID
            int view_id;
            ///这是弹出界面的类型
            string tableType;
            foreach (var item in itcList)
            {

                if (string.IsNullOrEmpty(item.ACT_TABLE_ITEMS))
                {
                    continue;
                }

                try
                {

                    JObject jo = JObject.Parse(item.ACT_TABLE_ITEMS);

                    view_id = jo.Value<int>("view_id");

                    tableType = jo.Value<string>("type_id");
                }
                catch (Exception ex)
                {

                    log.Error("解析json数据出错了！", ex);

                    TreeNode nodeError = new TreeNode("解析错误！", "Error_" + item.IG2_TABLE_COL_ID);

                    nodeError.ParentId = nodeTA.Value;
                    nodeError.StatusID = 0;
                    nodeError.Tag = "ERROR|" + item.IG2_TABLE_COL_ID;
                    this.TreePanel1.Add(nodeError);
                    this.TreePanel1.Refresh();

                    nodeError.ChildLoaded = true;

                    continue;
                }

                LightModelFilter lmFView = new LightModelFilter(typeof(IG2_TABLE));
                lmFView.And("IG2_TABLE_ID", view_id);
                lmFView.And("TABLE_TYPE_ID", tableType);

                IG2_TABLE itView = decipher.SelectToOneModel<IG2_TABLE>(lmFView);

                if (itView == null)
                {
                    continue;
                }

                TreeNode nodeShow = null;

                if (item.V_LIST_MODE_ID == "SelectColumn")
                {
                    nodeShow = new TreeNode(item.DISPLAY + "_" + itView.DISPLAY + "(下拉)", nodeTA.Value + "_VIEW_" + view_id);
                }
                else
                {


                    nodeShow = new TreeNode(item.DISPLAY + "_" + itView.DISPLAY + "(弹窗)", nodeTA.Value + "_VIEW_" + view_id);
                }
                nodeShow.ParentId = nodeTA.Value;
                nodeShow.StatusID = 0;
                nodeShow.Tag = string.Concat("VIEW|",              ///这是类型
                    menu.BIZ_C_MENU_ID,                            ///菜单ID
                    "|", menu.SEC_PAGE_ID,                         ///界面ID     
                    "|", tabID,                                    /// 区域ID
                    "|", tabName,                                   ///区域类型
                    "|", item.DB_FIELD,                            ///内部列名
                    "|", view_id);                                 ///弹出窗口ID
                this.TreePanel1.Add(nodeShow);

                nodeShow.ChildLoaded = true;

            }
        }



    }


    class PageTable
    {
        /// <summary>
        /// 
        /// </summary>
        public string EcType { get; set; }

        public int TableId { get; set; }

        public string TableName { get; set; }

        public string ID { get; set; }
    }

}