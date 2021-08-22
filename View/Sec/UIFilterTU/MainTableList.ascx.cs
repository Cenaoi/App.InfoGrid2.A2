using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using App.BizCommon;
using HWQ.Entity.Filter;
using EC5.Utility;
using App.InfoGrid2.Model.SecModels;
using HWQ.Entity.LightModels;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using App.InfoGrid2.Model.XmlModel;

namespace App.InfoGrid2.Sec.UIFilterTU
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
        public void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<BIZ_C_MENU> rootCatalogs = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} and ROW_SID >=0", 0);

            foreach (BIZ_C_MENU cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.NAME, cata.BIZ_C_MENU_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "default";
                tNode.Tag = "MENU|" + cata.BIZ_C_MENU_ID;
                tNode.StatusID = 0;
                tNode.Expand();

                this.TreePanel1.Add(tNode);
            }

            rootCatalogs = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} and ROW_SID >=0", 100);

            foreach (BIZ_C_MENU cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.NAME, cata.BIZ_C_MENU_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "default";
                tNode.Tag =  "MENU|" + cata.BIZ_C_MENU_ID;
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

            int treeNodeValue = StringUtil.ToInt(node.Value, 0);

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded && treeNodeValue > 0)
            {
                List<BIZ_C_MENU> models = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} AND ROW_SID >= 0", node.Value);



                foreach (BIZ_C_MENU model in models)
                {
                    TreeNode node2 = new TreeNode(model.NAME, model.BIZ_C_MENU_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    node2.Tag = "MENU|"+ model.BIZ_C_MENU_ID;

                    this.TreePanel1.Add(node2);
                }

                node.Expand();

                //创建表格节点
                CreateTableNode(node);


                node.ChildLoaded = true;

            }


            this.TreePanel1.Refresh();

             
            this.TreePanel1.OpenNode(node.Value);

            if (node.Value != "ROOT")
            {
                string[] ids = node.Tag.Split('|');

                string role = ids[0];


                if (role == "VIEW")
                {
                    string menuID = ids[1];     //菜单ID
                    string pageID = ids[2];     //窗体ID
                    string tabID = ids[3];      //复杂表 区域ID
                    string tabName = ids[4];    //复杂表 区域类型
                    string fieldName = ids[5];  //内部列名
                    string tableID = ids[6];    //弹出窗口ID
                    string viewType = ids[7];   //页面类型


                    MiniPager.Redirect("iform1",
                       $"UserList2.aspx?menuId={menuID}&pageId={pageID}&tableId={tableID}&tabId={tabID}&tabName={tabName}&fieldName={fieldName}&ViewType={viewType}");


                }
                else if(role == "MENU")
                {
                    int id = int.Parse(node.Value);

                    BIZ_C_MENU cata = decipher.SelectModelByPk<BIZ_C_MENU>(id);
                                        
                    MiniPager.Redirect("iform1",
                        $"UserList.aspx?menuId={id}&pageTypeId={cata.SEC_PAGE_TYPE_ID}");
                }
                else if(role == "FORM")
                {
                    int id = int.Parse(ids[1]);

                    int formId = int.Parse(ids[2]); //表单 ID

                    //BIZ_C_MENU cata = decipher.SelectModelByPk<BIZ_C_MENU>(id);

                    MiniPager.Redirect("iform1",
                        $"UserList.aspx?menuId={id}&form_id={formId}&pageTypeId=PAGE&page_sub_type_id=ONE_FORM");
                }
                else if(role == "TABLE")
                {
                    string menuID = ids[1];     //菜单ID
                    string pageID = ids[2];     //窗体ID
                    //string tabID = "";// ids[3];      //复杂表 区域ID
                    //string tabName = "";// ids[4];    //复杂表 区域类型
                    //string fieldName = "";// ids[5];  //内部列名
                    //string tableID = "";// ids[6];    //弹出窗口ID
                    //string viewType = ids[7];   //页面类型


                    MiniPager.Redirect("iform1",
                        $"UserList.aspx?menuId={menuID}&form_id={pageID}&pageTypeId=PAGE&page_sub_type_id=TABLE_FORM");

                    //MiniPager.Redirect("iform1",
                    //   $"UserList.aspx?menuId={menuID}&pageId={pageID}&tableId={tableID}&tabId={tabID}&tabName={tabName}&fieldName={fieldName}&ViewType={viewType}");

                }
                else
                {
                    Console.WriteLine("role = {0}", role);
                }

               
            }


        }


        private void ShowTableCol(string userCode, string pageId)
        {

        }


        /// <summary>
        /// 创建表格节点
        /// </summary>
        private void CreateTableNode(TreeNode node) 
        {

            DbDecipher decipher = ModelAction.OpenDecipher();  

            BIZ_C_MENU menu = decipher.SelectModelByPk<BIZ_C_MENU>(node.Value);

            CustomView info = null;

            if (!string.IsNullOrEmpty(menu.EXPAND_CFG))
            {
                try
                {
                    string c_ui = EC5.SystemBoard.SysBoardManager.CurrentApp.AppSettings["custom_ui"];

                    string url = c_ui+"/"+ menu.EXPAND_CFG;

                    //xml来源可能是外部文件，也可能是从其他系统获得
                    string paths = HttpContext.Current.Server.MapPath(url);
                    if (!File.Exists(paths))
                    {
                        return;
                    }
                    FileStream file = new FileStream(paths, FileMode.Open, FileAccess.Read);
                    XmlSerializer xmlSearializer = new XmlSerializer(typeof(CustomView));
                    info = (CustomView)xmlSearializer.Deserialize(file);
                    file.Close();
                    file.Dispose();



                }
                catch (Exception ex)
                {
                    throw new Exception("反系列化xml文件出错了！", ex);
                }

            }

           



            if (menu.SEC_PAGE_TYPE_ID == "TABLE")
            {

                CreateTableOne(menu.SEC_PAGE_ID, node,"","",menu,0,0);



            }
            else if (menu.SEC_PAGE_TYPE_ID == "PAGE")
            {
               
                LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("IG2_TABLE_ID", menu.SEC_PAGE_ID);
                filter.And("TABLE_TYPE_ID", "PAGE");
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
                    CreateTableOne(pTable.TableId, node,pTable.ID,pTable.EcType,menu,0,0);
                }

            }



            

            if ((menu.SEC_PAGE_TYPE_ID == "TABLE" || menu.SEC_PAGE_TYPE_ID == "PAGE") && info  != null)
            {
                TreeNode nodeTA = new TreeNode("自定义界面", "TA_" + menu.BIZ_C_MENU_ID);

                nodeTA.ParentId = node.Value;
                nodeTA.StatusID = 0;
                nodeTA.Tag = "TABLE|" + menu.BIZ_C_MENU_ID;

                this.TreePanel1.Add(nodeTA);

                nodeTA.ChildLoaded = true;

                RecursiveCustomView(nodeTA, menu, info.structEpand.structEpand);
            }
            else if (menu.SEC_PAGE_TYPE_ID == "FIXED_PAGE" && info != null)
            {
                RecursiveCustomView(node, menu, info.viewStruct.viewStruct);
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
        /// 处理表单
        /// </summary>
        /// <param name="newPageId">如果这个值不为0, 篡改 menu.SEC_PAGE_ID 的值,</param>
        private void CreateFormOne(int pageId, TreeNode node, BIZ_C_MENU menu,int pageItemlevel, int newPageId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("IG2_TABLE_ID", pageId);
            filter.And("TABLE_TYPE_ID", "PAGE");
            filter.And("TABLE_SUB_TYPE_ID", "ONE_FORM");

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
                CreateTableOne(pTable.TableId, node, pTable.ID, pTable.EcType, menu, pageItemlevel, newPageId);
            }
        }





        /// <summary>
        /// 创建一张表格的节点
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="tabID">区域ID</param>
        /// <param name="tabName">区域类型</param>
        /// <param name="menu"></param>
        /// <param name="parentNode"></param>
        private void CreateTableFormOne(int pageId, TreeNode parentNode, string tabID, string tabName, BIZ_C_MENU menu, int pageItemlevel, int newPageId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(pageId);

            if (it == null)
            {
                throw new Exception($"数据表不存在。IG2_TABLE.IG2_TABLE_ID={pageId}, tabName={tabName} 没有这记录。");
            }

            TreeNode nodeTA = new TreeNode($"{it.DISPLAY}(单表表单)", $"TA_{menu.BIZ_C_MENU_ID}_{pageId}");

            nodeTA.ParentId = parentNode.Value;
            nodeTA.StatusID = 0;
            nodeTA.Tag = $"TABLE|{menu.BIZ_C_MENU_ID}|{pageId}";

            this.TreePanel1.Add(nodeTA);

            nodeTA.ChildLoaded = true;


            LightModelFilter lmFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            lmFilter.AddFilter("ROW_SID >= 0");
            lmFilter.And("IG2_TABLE_ID", pageId);
            //lmFilter.And("ACT_MODE", "TABLE");
            //lmFilter.And("V_LIST_MODE_ID", new string[] { "TriggerColumn", "SelectColumn" }, Logic.In);


            List<IG2_TABLE_COL> itcList = decipher.SelectModels<IG2_TABLE_COL>(lmFilter);


            int view_id; //这是弹出界面的表ID


            string tableType;  //这是弹出界面的类型

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
                lmFView.AddFilter("ROW_SID >= 0");
                lmFView.And("IG2_TABLE_ID", view_id);
                lmFView.And("TABLE_TYPE_ID", tableType);

                IG2_TABLE itView = decipher.SelectToOneModel<IG2_TABLE>(lmFView);

                if (itView == null)
                {
                    continue;
                }


                int secPageId = menu.SEC_PAGE_ID;

                if (newPageId > 0)
                {
                    secPageId = newPageId;
                }


                TreeNode nodeShow = null;

                if (item.V_LIST_MODE_ID == "SelectColumn")
                {
                    nodeShow = new TreeNode(item.DISPLAY + "_" + itView.DISPLAY + "(下拉)" + "---" + secPageId, nodeTA.Value + "_VIEW_" + view_id);
                }
                else
                {
                    nodeShow = new TreeNode(item.DISPLAY + "_" + itView.DISPLAY + "(弹窗)" + "---" + secPageId, nodeTA.Value + "_VIEW_" + view_id);
                }

                nodeShow.ParentId = nodeTA.Value;
                nodeShow.StatusID = 0;

                //nodeShow.Tag = string.Concat(
                //    "VIEW|",               // 这是类型
                //    menu.BIZ_C_MENU_ID,    // 菜单ID
                //    "|", menu.SEC_PAGE_ID, // 界面ID     
                //    "|", tabID,            // 区域ID
                //    "|",tabName,           // 区域类型
                //    "|", item.DB_FIELD,    // 内部列名
                //    "|", view_id,          // 弹出窗口ID
                //    "|", "DIALOG");        // 页面类型

                nodeShow.Tag = string.Concat(
                    "VIEW|",               // 这是类型
                    menu.BIZ_C_MENU_ID,    // 菜单ID
                    "|", secPageId,        // 界面ID     
                    "|", tabID,            // 区域ID
                    "|", tabName,          // 区域类型
                    "|", item.DB_FIELD,    // 内部列名
                    "|", view_id,          // 弹出窗口ID
                    "|", "DIALOG");        // 页面类型

                this.TreePanel1.Add(nodeShow);

                nodeShow.ChildLoaded = true;

            }




            #region 处理表单


            if ((it.FORM_EDIT_TYPE == "ONE_FORM" || it.FORM_NEW_TYPE == "ONE_FORM") &&
               (it.FORM_EDIT_PAGEID > 0 || it.FORM_NEW_PAGEID > 0))
            {

                if (it.FORM_NEW_PAGEID == it.FORM_EDIT_PAGEID)
                {
                    if (pageItemlevel == 0)
                    {

                        TreeNode formNode = new TreeNode($"{it.DISPLAY}(表单)", $"TA_{it.FORM_NEW_PAGEID}");
                        formNode.ParentId = parentNode.Value;
                        formNode.StatusID = 1;
                        formNode.Tag = $"FORM|{menu.BIZ_C_MENU_ID}|{it.FORM_NEW_PAGEID}";

                        this.TreePanel1.Add(formNode);

                        newPageId = it.FORM_EDIT_PAGEID;

                        CreateFormOne(it.FORM_EDIT_PAGEID, formNode, menu, pageItemlevel + 1, newPageId);

                    }

                }


            }
            else if ((it.FORM_EDIT_TYPE == "TABLE_FORM" || it.FORM_NEW_TYPE == "TABLE_FORM") &&
               (it.FORM_EDIT_PAGEID > 0 || it.FORM_NEW_PAGEID > 0))
            {

                if (it.FORM_NEW_PAGEID == it.FORM_EDIT_PAGEID)
                {
                    if (pageItemlevel == 0)
                    {
                        newPageId = it.FORM_EDIT_PAGEID;

                        //TreeNode formNode = new TreeNode($"{it.DISPLAY}(单表表单)", $"TA_{menu.BIZ_C_MENU_ID}_{newPageId}");
                        //formNode.ParentId = parentNode.Value;
                        //formNode.StatusID = 1;
                        //formNode.Tag = $"TABLE|{menu.BIZ_C_MENU_ID}|{newPageId}";

                        //this.TreePanel1.Add(formNode);


                        CreateTableFormOne(newPageId, parentNode, "", "", menu, pageItemlevel + 1, 0);

                        //CreateFormOne(it.FORM_EDIT_PAGEID, formNode, menu, pageItemlevel + 1, newPageId);

                    }

                }

            }


            #endregion

        }



        /// <summary>
        /// 创建一张表格的节点
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="tabID">区域ID</param>
        /// <param name="tabName">区域类型</param>
        /// <param name="menu"></param>
        /// <param name="parentNode"></param>
        private void CreateTableOne(int pageId,TreeNode parentNode,string tabID,string tabName,BIZ_C_MENU menu, int pageItemlevel, int newPageId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();  

            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(pageId);

            if (it == null)
            {
                throw new Exception($"数据表不存在。IG2_TABLE.IG2_TABLE_ID={pageId}, tabName={tabName} 没有这记录。");
            }

            TreeNode nodeTA = new TreeNode($"{it.DISPLAY}({tabID})", $"TA_{pageId}");

            nodeTA.ParentId = parentNode.Value;
            nodeTA.StatusID = 0;
            nodeTA.Tag = $"TABLE|{menu.BIZ_C_MENU_ID}|{pageId}";

            this.TreePanel1.Add(nodeTA);

            nodeTA.ChildLoaded = true;


            LightModelFilter lmFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            lmFilter.AddFilter("ROW_SID >= 0");
            lmFilter.And("IG2_TABLE_ID", pageId);
            lmFilter.And("ACT_MODE", "TABLE");
            lmFilter.And("V_LIST_MODE_ID", new string[]{"TriggerColumn","SelectColumn"}, Logic.In);
           

            List<IG2_TABLE_COL> itcList = decipher.SelectModels<IG2_TABLE_COL>(lmFilter);

           
            int view_id; //这是弹出界面的表ID
          

            string tableType;  //这是弹出界面的类型

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
                    nodeError.Tag = "ERROR|"+item.IG2_TABLE_COL_ID;
                    this.TreePanel1.Add(nodeError);
                    this.TreePanel1.Refresh();

                    nodeError.ChildLoaded = true;

                    continue;
                }

                LightModelFilter lmFView = new LightModelFilter(typeof(IG2_TABLE));
                lmFView.AddFilter("ROW_SID >= 0");
                lmFView.And("IG2_TABLE_ID", view_id);
                lmFView.And("TABLE_TYPE_ID", tableType);

                IG2_TABLE itView = decipher.SelectToOneModel<IG2_TABLE>(lmFView);

                if (itView == null)
                {
                    continue;
                }


                int secPageId = menu.SEC_PAGE_ID;

                if (newPageId > 0)
                {
                    secPageId = newPageId;
                }


                TreeNode nodeShow  = null;

                if (item.V_LIST_MODE_ID == "SelectColumn")
                {
                    nodeShow = new TreeNode(item.DISPLAY + "_" + itView.DISPLAY + "(下拉)" + "---" + secPageId, nodeTA.Value + "_VIEW_" + view_id );
                }
                else
                {
                    nodeShow = new TreeNode(item.DISPLAY + "_" + itView.DISPLAY + "(弹窗)" + "---" + secPageId, nodeTA.Value + "_VIEW_" + view_id);
                }

                nodeShow.ParentId = nodeTA.Value;
                nodeShow.StatusID = 0;

                //nodeShow.Tag = string.Concat(
                //    "VIEW|",               // 这是类型
                //    menu.BIZ_C_MENU_ID,    // 菜单ID
                //    "|", menu.SEC_PAGE_ID, // 界面ID     
                //    "|", tabID,            // 区域ID
                //    "|",tabName,           // 区域类型
                //    "|", item.DB_FIELD,    // 内部列名
                //    "|", view_id,          // 弹出窗口ID
                //    "|", "DIALOG");        // 页面类型

                nodeShow.Tag = string.Concat(
                    "VIEW|",               // 这是类型
                    menu.BIZ_C_MENU_ID,    // 菜单ID
                    "|", secPageId,        // 界面ID     
                    "|", tabID,            // 区域ID
                    "|", tabName,          // 区域类型
                    "|", item.DB_FIELD,    // 内部列名
                    "|", view_id,          // 弹出窗口ID
                    "|", "DIALOG");        // 页面类型

                this.TreePanel1.Add(nodeShow);

                nodeShow.ChildLoaded = true;

            }




            #region 处理表单


            if ((it.FORM_EDIT_TYPE == "ONE_FORM" || it.FORM_NEW_TYPE == "ONE_FORM") &&
               (it.FORM_EDIT_PAGEID > 0 || it.FORM_NEW_PAGEID > 0))
            {

                if (it.FORM_NEW_PAGEID == it.FORM_EDIT_PAGEID)
                {
                    if (pageItemlevel == 0)
                    {

                        TreeNode formNode = new TreeNode($"{it.DISPLAY}(表单)", $"TA_{it.FORM_NEW_PAGEID}");
                        formNode.ParentId = parentNode.Value;
                        formNode.StatusID = 1;
                        formNode.Tag = $"FORM|{menu.BIZ_C_MENU_ID}|{it.FORM_NEW_PAGEID}";

                        this.TreePanel1.Add(formNode);

                        newPageId = it.FORM_EDIT_PAGEID;

                        CreateFormOne(it.FORM_EDIT_PAGEID, formNode, menu, pageItemlevel + 1, newPageId);

                    }

                }


            }
            else if ((it.FORM_EDIT_TYPE == "TABLE_FORM" || it.FORM_NEW_TYPE == "TABLE_FORM") &&
               (it.FORM_EDIT_PAGEID > 0 || it.FORM_NEW_PAGEID > 0))
            {

                if (it.FORM_NEW_PAGEID == it.FORM_EDIT_PAGEID)
                {
                    if (pageItemlevel == 0)
                    {
                        newPageId = it.FORM_EDIT_PAGEID;

                        //TreeNode formNode = new TreeNode($"{it.DISPLAY}(单表表单)", $"TA_{menu.BIZ_C_MENU_ID}_{newPageId}");
                        //formNode.ParentId = parentNode.Value;
                        //formNode.StatusID = 1;
                        //formNode.Tag = $"TABLE|{menu.BIZ_C_MENU_ID}|{newPageId}";

                        //this.TreePanel1.Add(formNode);


                        CreateTableFormOne(newPageId, parentNode, "", "", menu,pageItemlevel + 1, 0);

                        //CreateFormOne(it.FORM_EDIT_PAGEID, formNode, menu, pageItemlevel + 1, newPageId);

                    }

                }

            }


            #endregion

        }



        /// <summary>
        /// 递归创建页面节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="menu"></param>
        /// <param name="vsList"></param>
        void RecursiveCustomView(TreeNode node, BIZ_C_MENU menu, List<ViewStruct> vsList) 
        {
            foreach (ViewStruct vs in vsList)
            {
                TreeNode nodeShow = new TreeNode(vs.text, node.Value + "_VIEW_" + vs.id);

                nodeShow.ParentId = node.Value;
                nodeShow.StatusID = 0;

                if (vs.isPanel)
                {
                    nodeShow.Tag = string.Concat("TA|",            ///这是类型
                    menu.BIZ_C_MENU_ID,                            ///菜单ID
                    "|", menu.SEC_PAGE_ID,                         ///界面ID     
                    "|", vs.id,                                    /// 区域ID
                    "|", vs.displayMode,                           ///区域类型
                    "|", "",                                       ///内部列名
                    "|", vs.tableId,                               ///弹出窗口ID
                    "|", "");                                      ///页面类型  
                }
                else
                {

                    nodeShow.Tag = string.Concat("VIEW|",              ///这是类型
                        menu.BIZ_C_MENU_ID,                            ///菜单ID
                        "|", menu.SEC_PAGE_ID,                         ///界面ID     
                        "|", vs.id,                                    /// 区域ID
                        "|", vs.displayMode,                           ///区域类型
                        "|", "",                                       ///内部列名
                        "|", vs.tableId,                               ///弹出窗口ID
                        "|","");                                       ///页面类型                         

                }
                this.TreePanel1.Add(nodeShow);


               
               RecursiveCustomView(nodeShow, menu, vs.viewStruct);
                


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