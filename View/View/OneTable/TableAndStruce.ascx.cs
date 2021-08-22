using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5;
using EC5.BizLogger;
using EC5.DbCascade;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.IG2.Core.UI;
using EC5.IG2.Plugin;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace App.InfoGrid2.View.OneTable
{
    public partial class TableAndStruce : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TableSet m_TableSet;

        LModelElement m_ModelElem = null;


        /// <summary>
        /// 表名
        /// </summary>
        string m_TableName;

        /// <summary>
        /// 
        /// </summary>
        int m_TableId = 0;

        IG2_TABLE m_Table;

        string m_SecTag;

        int m_MenuId = 0;


        Store m_MainStore;

        Table m_MainTable;

        Toolbar m_MainToolbar;

        int m_ParentCataId = -1;

        public int ParentCataId
        {
            get { return m_ParentCataId; }
        }

        #region 业务类别权限

        private void InitData()
        {
            if (!m_Table.BIZ_CATALOG_ENABLED)
            {
                return;
            }


            string cataIdentity = StringUtil.NoBlank( m_Table.BIZ_CATALOG_IDENTITY, m_Table.TABLE_NAME);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("CATA_IDENTITY", cataIdentity);
            filter.And("CATA_TYPE_CODE", "USER_ROOT");

            BIZ_CATALOG pCata = decipher.SelectToOneModel<BIZ_CATALOG>(filter);

            if (pCata == null)
            {
                BIZ_CATALOG rootCata = decipher.SelectModelByPk<BIZ_CATALOG>(100);

                pCata = new BIZ_CATALOG();
                pCata.CATA_TEXT = StringUtil.NoBlank(m_Table.DISPLAY, m_Table.TABLE_NAME) + "类别";
                pCata.REMARK = m_Table.TABLE_NAME + " 数据表";
                pCata.CATA_IDENTITY = cataIdentity;
                pCata.CATA_TYPE_CODE = "USER_ROOT";
                pCata.CATA_CODE = BizCatalogMgr.NewCode(rootCata);
                pCata.VISIBLE = true;
                pCata.PARENT_ID = 100;
                
                decipher.InsertModel(pCata);
            }

            m_ParentCataId = pCata.BIZ_CATALOG_ID;

            List<BIZ_CATALOG> rootCatalogs = decipher.SelectModels<BIZ_CATALOG>("ROW_SID >=0 AND PARENT_ID={0}", pCata.BIZ_CATALOG_ID);

            TreeNode rootNode = new TreeNode(pCata.CATA_TEXT, pCata.BIZ_CATALOG_ID);
            rootNode.ParentId = "0";
            rootNode.NodeType = "table";
            rootNode.StatusID = 0;
            rootNode.Tag = "99";
            rootNode.Expand();

            this.TreePanel1.Add(rootNode);


            TreeNode noRootNode = new TreeNode("未归类", "NO_CATA");
            noRootNode.ParentId = "0";
            noRootNode.NodeType = "CATA";
            noRootNode.StatusID = 0;
            noRootNode.ChildLoaded = true;
            noRootNode.Tag = "99";

            this.TreePanel1.Add(noRootNode);


            foreach (BIZ_CATALOG cata in rootCatalogs)
            {
                TreeNode tNode = new TreeNode(cata.CATA_TEXT, cata.BIZ_CATALOG_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "CATA";
                tNode.StatusID = 0;
                tNode.Tag = cata.SEC_LEVEL.ToString();

                this.TreePanel1.Add(tNode);
            }


        }

        protected void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                int parentId = StringUtil.ToInt(node.Value);


                List<BIZ_CATALOG> models = decipher.SelectModels<BIZ_CATALOG>("PARENT_ID={0} AND ROW_SID >= 0", parentId);

                foreach (BIZ_CATALOG cata in models)
                {
                    TreeNode node2 = new TreeNode(cata.CATA_TEXT, cata.BIZ_CATALOG_ID);

                    node2.ParentId = cata.PARENT_ID.ToString(); ;
                    node2.StatusID = 0;
                    node2.Tag = cata.SEC_LEVEL.ToString();

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();

                node.ChildLoaded = true;
                node.Expand();

            }

            this.TreePanel1.OpenNode(node.Value);


            if (node.Value != "ROOT")
            {
                //int id = int.Parse(node.Value);

                //SEC_STRUCT cata = decipher.SelectModelByPk<SEC_STRUCT>(id);

                //MiniPager.Redirect("iform1",
                //    string.Format("/App/InfoGrid2/Sec/StructDefine/TreeNodeStruceEdit.aspx?id={0}", id));
            }

            
            this.store1.CurPage = 0;
            this.store1.Refresh();
        }



        #endregion

        /// <summary>
        /// 移动记录的结构权限
        /// </summary>
        public void GoMoveCata(string struceCode)
        {
            if (StringUtil.IsBlank(struceCode))
            {
                MessageBox.Alert("转移异常。");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelElement modelElem = LightModel.GetLModelElement(this.store1.Model);

            if (!modelElem.Fields.ContainsField("BIZ_CATA_CODE"))
            {
                MessageBox.Alert("此记录没有 “BIZ_CATA_CODE” 字段,无法做结构权限。");
                return;
            }

            int n = 0;

            foreach (var item in this.table1.CheckedRows)
            {
                int id = StringUtil.ToInt(item.Id);

                n += decipher.UpdateModelByPk(this.store1.Model, id, new object[] { "BIZ_CATA_CODE", struceCode });

            }

            this.store1.Refresh();
        }



        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }


        protected override void OnInitCustomControls(EventArgs e)
        {

            m_MainStore = this.store1;
            m_MainTable = this.table1;
            m_MainToolbar = this.Toolbar1;

            this.store1.Updating += store1_Updating;
            this.store1.Filtering += store1_Filtering;
            this.store1.Inserting += store1_Inserting;
            this.store1.Inserted += Store1_Inserted;
            this.store1.PageLoaded += store1_PageLoaded;



            DbDecipher decipher = ModelAction.OpenDecipher();

            m_TableId = WebUtil.QueryInt("id");

            m_SecTag = WebUtil.Query("sec_tag");
            m_MenuId = WebUtil.QueryInt("menu_id");

            m_TableSet = TableSet.SelectByPk(decipher, m_TableId);

            if (m_TableSet == null)
            {
                throw new Exception(string.Format("TableSet 对象不存在,TableId={0}", m_TableId));
            }

            m_Table = m_TableSet.Table;
            m_TableName = m_Table.TABLE_NAME;

            m_ModelElem = TableMgr.GetModelElem(m_TableSet);


            this.FormEditType = m_Table.FORM_EDIT_TYPE;
            this.FormEditPageID = m_Table.FORM_EDIT_PAGEID;

            string alias_title = WebUtil.Query("alias_title");

            HeadPanel.Visible = m_Table.IS_BIG_TITLE_VISIBLE;
            this.headLab.Value = "<span class='page-head' >" + StringUtil.NoBlank(alias_title, m_Table.DISPLAY) + "</span>";

            this.viewport1.MarginTop = m_Table.IS_BIG_TITLE_VISIBLE ? 40 : 0;

            //this.tableNameTB2.Text = m_Table.DISPLAY;

            if (m_ModelElem == null)
            {
                EasyClick.Web.Mini.MiniHelper.Alert("数据表不存在");
                return;
            }


            //初始化数据仓库
            IntiStoreAttrs();


            //创建界面UI
            CreateControl(m_TableSet);


            M2SecurityUiFactory secUiFactory = new M2SecurityUiFactory();
            secUiFactory.InitSecUI(m_TableId, "TABLE", m_MenuId, m_TableSet);
            secUiFactory.Filter("", m_SecTag, "", this.Toolbar1, this.table1, this.store1);

            secUiFactory.FilterForForm("", m_SecTag, "", this.searchForm, this.store1);

            




            m_UserControls.AddRange(new List<Control>()
            {
                this.searchForm,this.Toolbar1,this.store1,this.table1
            });


            try
            {
                InitCustomPage(m_Table.SERVER_CLASS);
            }
            catch (Exception ex)
            {
                log.Error("加载自定义类错误:" + m_Table.SERVER_CLASS, ex);
            }


            try
            {
                if (m_CustomPage != null)
                {
                    m_CustomPage.OnProInit();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("调用自定义类 OnInit 错误", ex);
            }


            //TreeBind();
        }

        private void Store1_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            if (m_Table.FORM_NEW_TYPE == "ONE_FORM")
            {
                LModel model = e.Object as LModel;

                object pk = model.GetPk();
                int pageId = m_Table.FORM_NEW_PAGEID;



                string url = $"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={pk}&pageId={pageId}&edit_pageid={m_Table.FORM_EDIT_PAGEID}";

                string title = "";

                title = StringUtil.NoBlank(m_Table.FORM_NEW_ALIAS_TITLE, m_Table.DISPLAY);


                if (!StringUtil.IsBlank(title))
                {
                    url += "&alias_title=" + System.Uri.EscapeUriString(title);
                }

                url += "&form_edit_pageID=" + m_Table.FORM_EDIT_PAGEID;

                EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','" + title + "-新建');");
            }

        }

        /// <summary>
        /// 绑定树控件的新增，删除，修改事件
        /// </summary>
        private void TreeBind()
        {

            this.TreePanel1.Creating += TreePanel1_Creating;
            this.TreePanel1.Removeing += TreePanel1_Removeing;
            this.TreePanel1.Renaming += TreePanel1_Renaming;
        }

        /// <summary>
        /// 重命名出节点名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Renaming(object sender, TreeNodeRenameEventArgs e)
        {
            TreeNode node = e.Node;

            if (node == null || node.Value == "root")
            {
                return;
            }

            try
            {

                int id = int.Parse(node.Value);

                DbDecipher decipher = ModelAction.OpenDecipher();

                string sql = string.Format("UPDATE BIZ_CATALOG SET CATA_TEXT = '{0}' WHERE  BIZ_CATALOG_ID = {1}", node.Text, id);

                decipher.ExecuteNonQuery(sql);



            }
            catch (Exception ex)
            {

                log.Error("更新树节点出错了！",ex);
            }


        }

        /// <summary>
        /// 删除节点信息，假删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Removeing(object sender, TreeNodeCancelEventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = StringUtil.ToInt(node.Value);

            BIZ_CATALOG cata = decipher.SelectModelByPk<BIZ_CATALOG>(id);

            if (cata == null)
            {
                MessageBox.Alert("不能删除根节点");
                e.Cancel = true;
                return;
            }

            if (cata.SEC_LEVEL > 0)
            {
                MessageBox.Alert("您的权限不够，无法删除。");
                e.Cancel = true;
                return;
            }


            LightModelFilter filterCata = new LightModelFilter(typeof(BIZ_CATALOG));
            filterCata.And("PARENT_ID", id);
            filterCata.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            if (decipher.ExistsModels(filterCata))
            {
                e.Cancel = true;
                MessageBox.Alert("删除失败, 必须先删除目录下的‘工作表’和‘子目录’.");

                return;
            }



            cata.ROW_SID = -3;
            cata.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(cata, "ROW_SID", "ROW_DATE_DELETE");
        }

        /// <summary>
        ///创建一个新节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreePanel1_Creating(object sender, EventArgs e)
        {
            TreeNode parent = this.TreePanel1.NodeSelected;

            int id = 0;

            if (parent == null)
            {
                return;
            }

            if (parent.Value == "root")
            {
                id = 100;
            }
            else
            {


                id = StringUtil.ToInt(parent.Value);
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_CATALOG pCata = decipher.SelectModelByPk<BIZ_CATALOG>(id);

            BIZ_CATALOG cata = new BIZ_CATALOG();


            cata.CATA_CODE = BizCatalogMgr.NewCode(pCata);
            cata.CATA_TEXT = "新建类别" + cata.CATA_CODE;
            cata.SEQ = 9999;
            cata.VISIBLE = true;

            cata.PARENT_ID = id;


            decipher.InsertModel(cata);


            TreeNode node = new TreeNode();
            node.ParentId = parent.Value;
            node.Text = cata.CATA_TEXT;
            node.NodeType = "default";
            node.Value = cata.BIZ_CATALOG_ID.ToString();


            this.TreePanel1.Add(node);

            this.TreePanel1.Refresh();

            this.TreePanel1.Edit(node.Value);
        }

        void store1_Updating(object sender, ObjectCancelEventArgs e)
        {
            BizHelper.FullForUpdate(e.Object as LModel);
        }

        void store1_PageLoaded(object sender, ObjectListEventArgs e)
        {
            M2VFieldHelper.Full(e.ObjectList,m_TableId);
        }

        void store1_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;
            TreeNode node = this.TreePanel1.NodeSelected;

            if (model != null && node != null)
            {
                int cataId = StringUtil.ToInt(node.Value);

                LModelElement modelElem = model.GetModelElement();

                LModelFieldElement fieldElem = null;

                if (modelElem.TryGetField("BIZ_CATA_CODE", out fieldElem))
                {
                    string cataCode = BizCatalogMgr.GetCode(cataId);

                    model[fieldElem] = cataCode;
                }
            }
        }

        void store1_Filtering(object sender, ObjectCancelEventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            if(node == null)
            {
                return;
            }

            LModelElement modelElem = LightModel.GetLModelElement(this.store1.Model);

            if (!modelElem.Fields.ContainsField("BIZ_CATA_CODE"))
            {
                return;
            }

            if (node.Value == "NO_CATA")
            {
                //this.store1.FilterParams.Add(new Param("BIZ_CATA_CODE", "null") { Logic = "is" });
                this.store1.FilterParams.Add(new EasyClick.BizWeb2.TSqlWhereParam("(BIZ_CATA_CODE is null or BIZ_CATA_CODE = '')"));
            }
            else
            {




                int cataId = StringUtil.ToInt(node.Value);

                string[] cataCodes = BizCatalogMgr.GetChildCodeAll(cataId);

                Param pm = new Param("BIZ_CATA_CODE");
                pm.SetInnerValue(cataCodes);
                pm.Logic = "in";

                this.store1.FilterParams.Add(pm);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //this.TreePanel1.Creating += TreePanel1_Creating;
            //this.TreePanel1.Renaming += TreePanel1_Renaming;
            //this.TreePanel1.Removeing += TreePanel1_Removeing;


            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        ExPage m_CustomPage;

        List<Control> m_UserControls = new List<Control>();

        /// <summary>
        /// 初始化自定义服务器代码
        /// </summary>
        private void InitCustomPage(string serClass)
        {
            if (StringUtil.IsBlank(serClass))
            {
                return;
            }


            serClass = serClass.Trim();

            Type customPageT = Type.GetType(serClass);

            if (customPageT == null)
            {
                log.ErrorFormat("自定义类“{0}”没有找到", serClass);
                return;
            }

            m_CustomPage = Activator.CreateInstance(customPageT) as ExPage;

            if (m_CustomPage == null)
            {
                log.ErrorFormat("自定义类“{0}”没法实例化。", serClass);
                return;
            }

            m_CustomPage.SetDefaultValue(this, this.searchForm, this.store1, this.Toolbar1, this.table1);
            m_CustomPage.UserControls = m_UserControls;
            m_CustomPage.ID = "ExPage";

            viewport1.Controls.Add(m_CustomPage);
        }


        /// <summary>
        /// 初始化数据库仓库的属性
        /// </summary>
        private void IntiStoreAttrs()
        {
            LModelElement modelElem = LightModel.GetLModelElement(m_TableName);

            this.store1.Model = m_TableName;
            this.store1.IdField = m_Table.ID_FIELD;
            this.store1.SortField = m_Table.USER_SEQ_FIELD;
            this.store1.SortText = m_Table.SORT_TEXT;

            this.store1.StringFields = GetFieldsArray(m_TableSet);

            this.store1.DeleteQuery.Add(new ControlParam(m_Table.ID_FIELD, this.table1.ID, "CheckedRows"));


            if (modelElem.HasField("BIZ_SID"))
            {
                this.store1.DeleteQuery.Add(new EasyClick.Web.Mini2.TSqlWhereParam(string.Format(
                    "(BIZ_SID is null OR BIZ_SID <= {0})", m_Table.DELETE_BY_BIZ_SID)));
            }


            #region 设置创建行的默认值

            foreach (IG2_TABLE_COL col in m_TableSet.Cols)
            {
                if (col.ROW_SID == -3 || StringUtil.IsBlank(col.DEFAULT_VALUE) || col.SEC_LEVEL > 6)
                {
                    continue;
                }

                Param pa = new Param(col.DB_FIELD, col.DEFAULT_VALUE);

                this.store1.InsertParams.Add(pa);
            }

            #endregion


            #region 删除进回收站模式

            store1.DeleteRecycle = m_Table.DELETE_RECYCLE;

            if (m_Table.DELETE_RECYCLE)
            {
                store1.FilterParams.Add(new Param("ROW_SID", "-1", DbType.Int32) { Logic = ">=" }); ;

                Param drP1 = new Param("ROW_SID", "-3", System.Data.DbType.Int32);
                ServerParam drP2 = new ServerParam("ROW_DATE_DELETE", "TIME_NOW");

                store1.DeleteRecycleParams.Add(drP1);
                store1.DeleteRecycleParams.Add(drP2);
            }

            #endregion


            if (this.IsBuilder())
            {
                return;
            }


            #region 权限-结构过滤

            IG2_TABLE srcTab=  m_Table;   //最原始的数据表

            if (m_Table.TABLE_TYPE_ID != "TABLE")
            {
                srcTab = TableMgr.GetTableForName(m_Table.TABLE_NAME);
            }

            if (srcTab.SEC_STRUCT_ENABLED)
            {
                UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

                if (uSec != null)
                {
                    Param cataPs = new Param("BIZ_CATA_CODE");
                    cataPs.SetInnerValue(uSec.ArrCatalogCode);
                    cataPs.Logic = "in";

                    this.store1.FilterParams.Add(cataPs);
                }
            }

            #endregion

            M2SecurityDataFactory secDataFactory = new M2SecurityDataFactory();
            secDataFactory.BindStore(this.store1);


            //M2ValidateFactory validFactory = new M2ValidateFactory();
            //validFactory.BindStore(this.store1);

            ////单元格公式
            //LCodeFactory lcFactory = new LCodeFactory();
            //lcFactory.BindStore(this.store1);
            
            ////简单流程
            //LCodeValueFactory lcvFactiry = new LCodeValueFactory();
            //lcvFactiry.BindStore(this.store1);
            

            //DbCascadeFactory dbccFactory = new DbCascadeFactory();
            //dbccFactory.ExecEnd += dbccFactory_ExecEnd;
            //dbccFactory.BindStore(this.store1);

            DbCascadeRule.Bind(this.store1);

        }

        void dbccFactory_ExecEnd(object sender, DbCascadeEventArges e)
        {
            LogStepMgr.Insert(e.Steps[0], e.OpText, e.Remark);
        }


        /// <summary>
        /// 输出到打印机
        /// </summary>
        public void ToPrint()
        {
            string plugClass = "EC5.IG2.Plugin.Custom.PrintPlugin";
            string plugMethod = "PrintExcel";

            Type plugT = PluginManager.Get(plugClass);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + plugClass);
                return;
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
                return;
            }


            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = "{}";

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = this.store1;
                inter.SrcTable = this.table1;

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }

        }



        /// <summary>
        /// 改变 BIZ_SID 字段.Biz 业务状态
        /// </summary>
        /// <param name="paramStr">提交的 json 参数</param>
        public void ChangeBizSID(string paramStr)
        {
            string[] sp = StringUtil.Split(paramStr, ",");


            Table tab = m_MainTable;
            Store store = m_MainStore;

            if (sp.Length == 4)
            {
                string tableId = sp[2];
                string storeId = sp[3];

                tab = FindControl(this.viewport1, tableId) as Table;
                store = FindControl(this.viewport1, storeId) as Store;

                paramStr = $"{sp[0]},{sp[1]}";
            }


            try
            {
                ResultBase result = BizHelper.ChangeBizSID(paramStr, m_MainTable, m_MainStore);
                
                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 改变某个字段的值
        /// </summary>
        /// <param name="paramStr"></param>
        public void ChangeField(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeField(paramStr, m_MainTable, m_MainStore);

                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }
        }




        /// <summary>
        /// 导出 Excel 
        /// </summary>
        public void ToExcel()
        {
            string plugClass = "EC5.IG2.Plugin.Custom.InputOutExcelPlugin";
            string plugMethod = "InputOut";

            Type plugT = PluginManager.Get(plugClass);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + plugClass);
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
            }


            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = "{}";

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = this.store1;
                inter.SrcTable = this.table1;

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }

        }

        private string GetFieldsArray(TableSet tableSet)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();

            foreach (IG2_TABLE_COL col in tableSet.Cols)
            {

                if (col.ROW_SID == -3 && col.DB_FIELD != m_Table.ID_FIELD)
                {
                    continue;
                }

                if (i++ > 0)
                {
                    sb.Append(",");
                }

                sb.Append(col.DB_FIELD);

            }

            return sb.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (m_CustomPage != null)
                {
                    m_CustomPage.OnProLoad();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("调用自定义类 OnInit 错误", ex);
            }

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }



        /// <summary>
        /// 创建界面控件
        /// </summary>
        private void CreateControl(TableSet tableSet)
        {
            bool isPostBack = this.IsPostBack;

            DbDecipher decipher = ModelAction.OpenDecipher();

            ToolbarSet toolbarSet = new ToolbarSet();
            toolbarSet.SelectForTable(decipher, tableSet.Table.IG2_TABLE_ID);


            M2ToolbarFactory toolbarFactory = new M2ToolbarFactory();
            toolbarFactory.TableId = m_MainTable.ID;
            toolbarFactory.StoreId = m_MainStore.ID;
            toolbarFactory.CreateItems(this.Toolbar1, toolbarSet);


            M2TableFactory tableFactory = new M2TableFactory(isPostBack);
            tableFactory.CreateTableColumns(this.table1, this.store1, tableSet);


            M2SearchFormFactory searchFactory = new M2SearchFormFactory(isPostBack, this.store1.ID);
            searchFactory.CreateControls(this.searchForm, tableSet);
        }




        /// <summary>
        /// 修改列.
        /// 需要存到临时表,然后才开始修改
        /// </summary>
        public void StepEdit2()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tableSet = TableSet.Select(decipher, m_TableId);


            Guid tmpId = Guid.NewGuid();

            IList<IG2_TMP_TABLECOL> tmpCols = new List<IG2_TMP_TABLECOL>();

            IG2_TMP_TABLE tmpTable = new IG2_TMP_TABLE();
            tableSet.Table.CopyTo(tmpTable, true);

            tmpTable.TMP_GUID = tmpId;
            tmpTable.TMP_OP_ID = "";
            tmpTable.TMP_SESSION_ID = this.Session.SessionID;

            foreach (IG2_TABLE_COL col in tableSet.Cols)
            {
                if (col.ROW_SID == -3)
                {
                    continue;
                }

                IG2_TMP_TABLECOL tmpCol = new IG2_TMP_TABLECOL();
                col.CopyTo(tmpCol, true);

                tmpCol.TMP_GUID = tmpId;
                tmpCol.TMP_OP_ID = "";
                tmpCol.TMP_SESSION_ID = this.Session.SessionID;

                tmpCols.Add(tmpCol);
            }



            try
            {
                decipher.InsertModel(tmpTable);
                decipher.InsertModels(tmpCols);


                MiniPager.Redirect(string.Format("StepEdit2.aspx?id={0}&tmp_id={1}", m_TableId, tmpId));
            }
            catch (Exception ex)
            {
                log.Error(ex);

                MessageBox.Alert("无法修改。");
            }
        }

        public void StepEdit3()
        {
            MiniPager.Redirect("StepEdit3.aspx?id=" + m_TableId);
        }

        public void StepEdit4()
        {
            MiniPager.Redirect("StepEdit4.aspx?id=" + m_TableId);
        }

        public void Search()
        {
            this.store1.LoadPage(0);

        }

        /// <summary>
        /// 工具栏设置
        /// </summary>
        public void ToolbarSetup()
        {

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("TABLE_ID", m_TableId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "IG2_TOOLBAR_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            int toolbarId = decipher.ExecuteScalar<int>(filter);


            string urlStr;

            if (toolbarId > 0)
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/SetToolbar.aspx?id={0}&table_id={1}", toolbarId, m_TableId);
            }
            else
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/ToolbarStepNew1.aspx?table_id={0}", m_TableId);
            }

            MiniPager.Redirect(urlStr);
        }


        /// <summary>
        /// 联动设置
        /// </summary>
        public void GoActList()
        {
            string src_url = string.Format("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id={0}", m_TableId);
            string urlCode = Base64Util.ToString(src_url, Base64Mode.Http);


            string urlStr = string.Format("/app/infogrid2/view/OneAction/ActList.aspx?l_table={0}&src_url={1}", m_TableName, urlCode);

            MiniPager.Redirect(urlStr);

        }

        /// <summary>
        /// 验证设置
        /// </summary>
        public void GoValidSetup()
        {
            string src_url = string.Format("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id={0}", m_TableId);
            string urlCode = Base64Util.ToString(src_url, Base64Mode.Http);

            string urlStr = string.Format("/App/InfoGrid2/View/OneValid/ValidStepEdit2.aspx?id={0}&src_url={1}", m_TableId, urlCode);

            MiniPager.Redirect(urlStr);
        }

        /// <summary>
        /// 弹出编辑目录树形结构
        /// </summary>
        public void EditTree() 
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE lm = decipher.SelectModelByPk<IG2_TABLE>(id);

            string tableName = StringUtil.NoBlank(lm.BIZ_CATALOG_IDENTITY,lm.TABLE_NAME);


            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_CATALOG));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("CATA_IDENTITY", tableName);
            lmFilter.And("CATA_TYPE_CODE", "USER_ROOT");

            BIZ_CATALOG bc = decipher.SelectToOneModel<BIZ_CATALOG>(lmFilter);


            Window win = new Window("编辑界面");
            win.State = WindowState.Max;
            win.ContentPath =string.Format("/App/InfoGrid2/View/Biz/Core_Catalog/EditTree.aspx?cataIdentity={0}&struct_code={1}",tableName,bc.SEC_STRUCT_CODE) ;
            
            win.ShowDialog();



        }


        public void RefreshTree()
        {
            this.TreePanel1.Clear();

            InitData();

            this.TreePanel1.Refresh();
        }


        /// <summary>
        /// 获取展示规则
        /// </summary>
        /// <returns></returns>
        public string GetDisplayRule()
        {
            return App.InfoGrid2.Bll.DisplayRuleMgr.GetJScript();
        }

        #region 插件



        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="id"></param>
        public void ExecPlugin(string cmdParam)
        {
            string[] ps = cmdParam.Split(';');

            if (ps.Length < 4)
            {
                MessageBox.Alert("命令已经失效..");
                return;
            }

            int toolbarItemId = StringUtil.ToInt(ps[0]);
            string storeId = ps[1];
            string tableId = ps[2];
            string searchId = ps[3];

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TOOLBAR_ITEM tItem = decipher.SelectModelByPk<IG2_TOOLBAR_ITEM>(toolbarItemId);

            if (tItem == null)
            {
                MessageBox.Alert("命令已经失效.");
                return;
            }

            if (tItem.EVENT_MODE_ID != "PLUG")
            {
                MessageBox.Alert("非插件命令。");
                return;
            }

            if (StringUtil.IsBlank(tItem.PLUG_CLASS))
            {
                MessageBox.Alert("插件设置有错误:没有插件路径名.");
                return;
            }

            if (StringUtil.IsBlank(tItem.PLUG_METHOD))
            {
                MessageBox.Alert("插件设置有错误:没有插件函数名.");
                return;
            }


            Store storeUi;
            Table tableUi;


            storeUi = this.store1;// this.FindControl(storeId) as Store;

            //if (storeUi == null)
            //{
            //    MessageBox.Alert("数据仓库 " + storeId + " 不存在.");
            //    return;
            //}

            tableUi = this.table1;// this.FindControl(tableId) as Table;

            //if (tableUi == null)
            //{
            //    MessageBox.Alert("表格 " + tableId + " 不存在.");
            //    return;
            //}


            Type plugT = PluginManager.Get(tItem.PLUG_CLASS);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + tItem.PLUG_CLASS);
            }

            MethodInfo mi = plugT.GetMethod(tItem.PLUG_METHOD);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + tItem.PLUG_CLASS + ", " + tItem.PLUG_METHOD);
            }



            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = tItem.PLUG_CLASS;
                inter.Method = tItem.PLUG_METHOD;
                inter.Params = tItem.PLUG_PARAMS;

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = storeUi;
                inter.SrcTable = tableUi;

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }

        }

        /// <summary>
        /// 插件步骤第二步
        /// </summary>
        /// <param name="cmdParam"></param>
        public void ExecPlugin_NextStep(string plug, string user_ps)
        {
            string plugJson = Base64Util.FromString(plug, Base64Mode.Http);

            JObject ppm;

            string plugClass;
            string plugMethod;
            string src_url;

            string plugParam;

            string srcStoreId;

            string srcTableId;

            try
            {
                ppm = (JObject)JsonConvert.DeserializeObject(plugJson);

                plugClass = ppm.Value<string>("plug_class");
                plugMethod = ppm.Value<string>("plug_method");
                src_url = ppm.Value<string>("src_url");

                plugParam = ppm.Value<string>("ps");

                srcStoreId = ppm.Value<string>("src_store_id");
                srcTableId = ppm.Value<string>("src_table_id");

            }
            catch (Exception ex)
            {
                log.Error("解析命令参数出错", ex);
                MessageBox.Alert("解析命令参数出错.");
                return;
            }




            Store storeUi;
            Table tableUi;


            storeUi = this.FindControl(srcStoreId) as Store;

            if (storeUi == null)
            {
                MessageBox.Alert("数据仓库 " + srcStoreId + " 不存在.");
                return;
            }

            tableUi = this.FindControl(srcTableId) as Table;

            if (tableUi == null)
            {
                MessageBox.Alert("表格 " + srcTableId + " 不存在.");
                return;
            }




            Type plugT = PluginManager.Get(plugClass);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + plugClass);
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
            }



            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = plugParam;

                inter.SrcUrl = src_url;

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = storeUi;
                inter.SrcTable = tableUi;

                inter.UserParams = user_ps;

                mi.Invoke(inter, null);

            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }


        }

        #endregion



        /// <summary>
        /// 修改的表单类型 ONE_FORM | TABLE_FORM
        /// </summary>
        public string FormEditType { get; set; } = "ONE_FORM";


        /// <summary>
        /// 表单页的 ID
        /// </summary>
        public int FormEditPageID { get; set; }

        /// <summary>
        /// 弹出编辑表单的别名
        /// </summary>
        public string FromEditAliasTitle { get; set; }
    }
}