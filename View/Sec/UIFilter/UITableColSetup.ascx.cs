using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model.SecModels;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;

namespace App.InfoGrid2.Sec.UIFilter
{
    public partial class UITableColSetup : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";


            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            this.InitData();

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                
                this.DataBind();
            }
        }

        private void InitData()
        {
            int secUiId = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID",secUiId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            //注意: 下面这行代码注释掉, 不明白当年为什么加下面这行代码...有恢复回来
            filter.And("SEC_ITEM_TYPE", "DIALOG", Logic.Inequality);   

            List<SEC_UI_TABLE> suTables = decipher.SelectModels<SEC_UI_TABLE>(filter);

            foreach (SEC_UI_TABLE uiTable in suTables)
            {               

                Tab tab = new Tab();
                tab.Text = uiTable.TABLE_TEXT;
                tab.Scroll = ScrollBars.None;
                tab.ID = "TAB_" + uiTable.SEC_UI_TABLE_ID;

                tab.IsDelayRender = true;


                #region 顶部表格

                Store topStore = new Store();
                topStore.ID = string.Format("TopStore{0}", uiTable.SEC_UI_TABLE_ID);
                topStore.Model = "SEC_UI_TABLE";
                topStore.IdField = "SEC_UI_TABLE_ID";
                topStore.PageSize = 0;

                topStore.FilterParams.Add("SEC_UI_ID", secUiId);
                topStore.FilterParams.Add("SEC_UI_TABLE_ID", uiTable.SEC_UI_TABLE_ID);


                Table topTable = new Table();
                topTable.StoreID = topStore.ID;
                topTable.Store = topStore;
                topTable.ID = string.Format("TopTable{0}", uiTable.SEC_UI_TABLE_ID);

                topTable.Dock = DockStyle.Top;
                topTable.PagerVisible = false;
                topTable.Height = "60";

                topTable.IsDelayRender = true;


                topTable.Columns.AddRange(new List<BoundField>(){
                    new RowNumberer(),
                    new RowCheckColumn(),
                    new BoundField("TABLE_NAME","表名"){ EditorMode= EditorMode.None},
                    new BoundField("DISPALY_MODE_ID","类型"){ EditorMode= EditorMode.None},
                    new BoundField("TABLE_TEXT","表描述"){},
                    new BoundField("LOCKED_RULE","锁行规则 JS"){ Width=400},
                    new CheckColumn("VALID_MSG_ENABLED","验证提示")
                });

                StoreSet.Controls.Add(topStore);

                tab.Controls.Add(topTable);

                #endregion



                #region 中间工具栏表格

                Store storeTool = new Store();
                storeTool.ID = string.Format("StoreTool{0}", uiTable.SEC_UI_TABLE_ID);
                storeTool.Model = "SEC_UI_TOOLBAR_ITEM";
                storeTool.IdField = "SEC_UI_TOOLBAR_ITEM_ID";
                storeTool.PageSize = 0;

                storeTool.FilterParams.Add("SEC_UI_ID", secUiId);
                storeTool.FilterParams.Add("SEC_UI_TABLE_ID", uiTable.SEC_UI_TABLE_ID);
                storeTool.FilterParams.Add(new Param("ROW_SID", 0) { Logic = ">=" });



                Table tableTool = new Table();
                tableTool.StoreID = storeTool.ID;
                tableTool.Store = storeTool;
                tableTool.ID = string.Format("TableTool{0}", uiTable.SEC_UI_TABLE_ID);

                tableTool.Dock = DockStyle.Top;
                tableTool.PagerVisible = false;
                tableTool.Height = "200";

                tableTool.IsDelayRender = true;

                tableTool.Columns.AddRange(new List<BoundField>(){
                    new RowNumberer(),
                    new RowCheckColumn(),
                    new BoundField("ITEM_ID","按钮名称"){ EditorMode= EditorMode.None},
                    new BoundField("ITEM_TEXT","按钮描述"){ EditorMode= EditorMode.None},
                    new CheckColumn("VISIBLE","可视-设计师"),
                    new CheckColumn("VISIBLE_B","可视-管理员"),
                });

                StoreSet.Controls.Add(storeTool);

                tab.Controls.Add(tableTool);

                #endregion



                #region 底部表格

                Store store = new Store();
                store.ID = string.Format("Store{0}", uiTable.SEC_UI_TABLE_ID);
                store.Model = "SEC_UI_TABLECOL";
                store.IdField = "SEC_UI_TABLECOL_ID";
                store.PageSize = 0;

                store.FilterParams.Add("SEC_UI_ID", secUiId);
                store.FilterParams.Add("SEC_UI_TABLE_ID", uiTable.SEC_UI_TABLE_ID);
                store.FilterParams.Add(new Param("ROW_SID", 0) { Logic = ">=" });



                Table table = new Table();
                table.StoreID = store.ID;
                table.Store = store;
                table.ID = string.Format("Table{0}", uiTable.SEC_UI_TABLE_ID);

                table.Dock = DockStyle.Full;
                table.PagerVisible = false;

                table.IsDelayRender = true;


                SelectColumn sortTypeCol = new SelectColumn("SORT_TYPE", "排序类型");
                sortTypeCol.TriggerMode = TriggerMode.None;
                sortTypeCol.Width = 80;
                sortTypeCol.Items.Add(new ListItem() { TextEx = "--空--" });
                sortTypeCol.Items.Add("ASC", "降序");
                sortTypeCol.Items.Add("DESC", "升序");

                SelectColumn sortTypeColB = new SelectColumn("SORT_TYPE_B", "排序类型");
                sortTypeColB.TriggerMode = TriggerMode.None;
                sortTypeColB.Width = 80;
                sortTypeColB.Items.Add(new ListItem() { TextEx = "--空--" });
                sortTypeColB.Items.Add("ASC", "降序");
                sortTypeColB.Items.Add("DESC", "升序");


                GroupColumn groupColsForBuilder = new GroupColumn("设计师");
                groupColsForBuilder.Columns.AddRange(new List<BoundField>() {

                    new CheckColumn("IS_READONLY","只读"),
                    new CheckColumn("IS_VISIBLE","可视"),
                    new CheckColumn("IS_LIST_VISIBLE","表格可视"),
                    new CheckColumn("IS_SEARCH_VISIBLE","查询可视"),
                    sortTypeCol,
                    new NumColumn("SORT_ORDER","排序顺序"){ NotDisplayValue="0"},
                    new BoundField("FILTER_1","过滤1"),
                    new BoundField("FILTER_2","过滤2"),
                    new BoundField("FILTER_3","过滤3")
                });

                GroupColumn groupColsForAdmin = new GroupColumn("管理员");
                groupColsForAdmin.Columns.AddRange(new List<BoundField>() {

                    new CheckColumn("IS_READONLY_B","只读"),
                    new CheckColumn("IS_VISIBLE_B","可视"),
                    new CheckColumn("IS_LIST_VISIBLE_B","表格可视"),
                    sortTypeColB,
                    new NumColumn("SORT_ORDER_B","排序顺序"){ NotDisplayValue="0"},
                    new BoundField("FILTER_1_B","过滤1"),
                    new BoundField("FILTER_2_B","过滤2"),
                    new BoundField("FILTER_3_B","过滤3")
                });

                table.Columns.AddRange(new List<BoundField>(){
                    new RowNumberer(),
                    new RowCheckColumn(),
                    new BoundField("DB_FIELD","字段名"){ EditorMode= EditorMode.None},
                    new BoundField("FIELD_TEXT","字段描述"){ EditorMode= EditorMode.None},
                    groupColsForBuilder,

                    groupColsForAdmin
                });

                StoreSet.Controls.Add(store);

                tab.Controls.Add(table);

                #endregion


                this.tabPanel1.Items.Add(tab);

            }


        }
    }
}