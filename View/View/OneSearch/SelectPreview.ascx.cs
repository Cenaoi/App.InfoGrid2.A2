using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.View.OneTable;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.IG2.Core.UI;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace App.InfoGrid2.View.OneSearch
{
    public partial class SelectPreview : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        M2SecurityUiFactory m_SecUiFty;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";


            base.OnInit(e);

        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            InitUI();
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        TableSet m_tSet;

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




        /// <summary>
        /// 获取 T-SQL 的 Where 子语句
        /// </summary>
        /// <param name="filter2"></param>
        /// <returns></returns>
        private string GetTSqlWhere_ForFilter2(string filter2)
        {
            if (StringUtil.IsBlank(filter2))
            {
                return string.Empty;
            }

            JArray items = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(filter2);

            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (JObject item in items)
            {
                string field = item.Value<string>("field");
                string logic = item.Value<string>("logic");
                string value = item.Value<string>("value");

                if (i++ > 0) { sb.Append(" AND "); }

                sb.AppendFormat("({0} {1} '{2}')", field, logic, value);
            }


            return sb.ToString();
        }



        private void InitUI()
        {
            int viewId = WebUtil.QueryInt("viewId");    //当前查询窗体的 id

            string filter2 = WebUtil.QueryBase64("filter2");  //二次过滤参数

            filter2 = GetTSqlWhere_ForFilter2(filter2);

            int owner_page_id = WebUtil.QueryInt("owner_page_id");  //所属弹出的主界面ID

            string owner_table = WebUtil.Query("owner_table");//对应记录的表名
            int owner_row_id = WebUtil.QueryInt("owner_row_id");    //所属对应弹出的那条记录的主键值(如果是子表,就是子表记录的主键值)
            int menuId = WebUtil.QueryInt("menu_Id");   //菜单ID

            string uiTypeId = WebUtil.Query("ui_Type_id");      
            
            string dialog_col = WebUtil.Query("dialog_col");    //被弹出的列


            int mainRowId = WebUtil.QueryInt("main_row_id");    //主表的记录行 id
            string mainTable = WebUtil.Query("main_table");     //主表的表名
            LModel mainModel = null;   //如果有多表，则是主表。如果是单表，就是当前记录。

            LModel ownerModel = null;   //当前对应的记录实体.


            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!StringUtil.IsBlank(mainTable) && mainRowId > 0)
            {
                mainModel = decipher.GetModelByPk(mainTable, mainRowId);
            }

            if(mainModel !=null && mainTable == owner_table && mainRowId == owner_row_id)
            {
                ownerModel = mainModel;
            }

            if(ownerModel == null && !StringUtil.IsBlank(owner_table) && owner_row_id > 0)
            {
                ownerModel = decipher.GetModelByPk(owner_table, owner_row_id);
            }



            m_tSet = TableSet.Select(decipher, viewId);

            //初始化元素,怕没有实体元素.
            TableMgr.GetModelElem(m_tSet);


            IG2_TABLE table = m_tSet.Table;

            this.store1.Model = table.TABLE_NAME;
            this.store1.IdField = table.ID_FIELD;

            this.table1.CheckedMode = table.SINGLE_SELECTION ? CheckedMode.Single : CheckedMode.Multi;
            this.table1.AutoRowCheck = (this.table1.CheckedMode == CheckedMode.Single);
            this.table1.CellDbclick += Table1_CellDbclick;

            IG2_TABLE srcTab ;   //最原始的数据表

            if(table.TABLE_TYPE_ID == "MORE_VIEW")
            {
                srcTab = table;
            }
            else if (table.TABLE_TYPE_ID != "TABLE")
            {
                srcTab = TableMgr.GetTableForName(table.TABLE_NAME);
            }
            else
            {
                srcTab = table;
            }


            UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

            if (srcTab != null && srcTab.SEC_STRUCT_ENABLED && uSec != null)
            {
                Param cataPs = new Param("BIZ_CATA_CODE");
                cataPs.SetInnerValue(uSec.ArrCatalogCode);
                cataPs.Logic = "in";

                this.store1.FilterParams.Add(cataPs);

            }


            if (table.TABLE_TYPE_ID == "MORE_VIEW")
            {
                ProMoreView(viewId, dialog_col, m_tSet, mainModel, uSec, filter2, owner_page_id, uiTypeId, menuId);
            }
            else
            {
                ProTableOrView(viewId, dialog_col, mainModel, ownerModel, uSec, filter2, owner_page_id, uiTypeId, menuId);
            }
        }

        /// <summary>
        /// 关联表
        /// </summary>
        private void ProMoreView(int viewId, string dialog_col, TableSet tSet, LModel mainModel, UserSecritySet uSec, string filter2,
            int owner_page_id, string uiTypeId, int menuId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            List<string> fields = new List<string>();

            ViewSet viewSet = ViewSet.Select(decipher, viewId);

            StoreTSqlQuerty tSql = this.store1.TSqlQuery;

            tSql.Enabeld = true;
            tSql.Select = ViewMgr.GetTSqlSelect(viewSet, ref fields);
            tSql.Form = ViewMgr.GetTSqlForm(viewSet);
            tSql.Where = ViewMgr.GetTSqlWhere(viewSet);
            tSql.OrderBy = ViewMgr.GetTSqlOrder(viewSet, fields);

            if(!StringUtil.IsBlank(tSql.Where) && !StringUtil.IsBlank(filter2))
            {
                tSql.Where += " AND ";
            }


            tSql.Where += filter2;


            M2SearchFormFactory serachFactory = new M2SearchFormFactory(this.IsPostBack, this.store1.ID);
            serachFactory.CreateControls(this.searchForm, m_tSet);

            M2TableFactory tableFactory = new M2TableFactory(this.IsPostBack);
            tableFactory.CreateTableColumns(this.table1, this.store1, m_tSet);


        }

        /// <summary>
        /// 单表 或 单表视图
        /// </summary>
        /// <param name="ownerModel">所属对应的当前这条点击记录的实体</param>
        private void ProTableOrView(int viewId, string dialog_col,LModel mainModel,LModel ownerModel, UserSecritySet uSec,string filter2,
            int owner_page_id,string uiTypeId,int menuId)
        {
            ProFilterTSet(mainModel,ownerModel, uSec);
            
            //二次过滤
            if (!StringUtil.IsBlank(filter2))
            {
                TSqlWhereParam filterWhere2 = new TSqlWhereParam();
                filterWhere2.Name = "COL_X";   //COL_X 是一个临时随便的名称，没有任何意义。
                filterWhere2.Where = filter2;

                this.store1.SelectQuery.Add(filterWhere2);
            }


            M2SearchFormFactory serachFactory = new M2SearchFormFactory(this.IsPostBack, this.store1.ID);
            serachFactory.CreateControls(this.searchForm, m_tSet);

            M2TableFactory tableFactory = new M2TableFactory(this.IsPostBack);
            tableFactory.CreateTableColumns(this.table1, this.store1, m_tSet, false);


            m_SecUiFty = new M2SecurityUiFactory();
            m_SecUiFty.InitSecUI(owner_page_id, uiTypeId, menuId, m_tSet);

            m_SecUiFty.FilterForItem(viewId, dialog_col, "DIALOG", "", "", "", this.table1, this.store1);

            m_SecUiFty.FilterForItemForm(viewId, dialog_col, "DIALOG", "", "", "", this.searchForm, this.store1);


        }


        /// <summary>
        /// 处理过滤条件
        /// </summary>
        /// <param name="mainModel">主表实体</param>
        /// <param name="ownerModel">当前记录的实体</param>
        /// <param name="uSec"></param>
        private void ProFilterTSet(LModel mainModel,LModel ownerModel, UserSecritySet uSec)
        {
            string ownerName = "$owner.";
            string mainName = "$main.";


            foreach (IG2_TABLE_COL col in m_tSet.Cols)
            {
                if (!StringUtil.IsBlank(col.FILTER_LOGIC) ||
                    !StringUtil.IsBlank(col.FILTER_VALUE))
                {
                    if (StringUtil.StartsWith(col.FILTER_VALUE, ownerName))
                    {
                        string field = col.FILTER_VALUE.Substring(ownerName.Length);

                        if(ownerModel == null)
                        {
                            throw new Exception("没有当前记录可供查询, 过滤条件: " + col.FILTER_VALUE);
                        }

                        object val = ownerModel[field];

                        Param p = new Param(col.DB_FIELD);
                        p.Logic = col.FILTER_LOGIC;
                        p.SetInnerValue(val);

                        this.store1.SelectQuery.Add(p);
                    }
                    else if (StringUtil.StartsWith(col.FILTER_VALUE, mainName))
                    {
                        string field = col.FILTER_VALUE.Substring(mainName.Length);

                        if (mainModel == null)
                        {
                            throw new Exception("没有当前记录可供查询, 过滤条件: " + col.FILTER_VALUE);
                        }

                        object val = mainModel[field];

                        Param p = new Param(col.DB_FIELD);
                        p.Logic = col.FILTER_LOGIC;
                        p.SetInnerValue(val);

                        this.store1.SelectQuery.Add(p);
                    }
                    else
                    {

                        Param p = new Param(col.DB_FIELD, col.FILTER_VALUE);
                        p.Logic = col.FILTER_LOGIC;

                        this.store1.SelectQuery.Add(p);
                    }
                }
                else if (!StringUtil.IsBlank(col.FILTER_TSQL_WHERE) && mainModel != null)
                {
                    TSqlWhereParam p = new TSqlWhereParam();
                    p.Name = col.DB_FIELD;

                    if (StringUtil.StartsWith(col.FILTER_TSQL_WHERE, "="))
                    {

                        EC5.LCodeEngine.LcFieldRule lcFRule = new EC5.LCodeEngine.LcFieldRule();

                        lcFRule.Field = col.DB_FIELD;
                        lcFRule.Code = col.FILTER_TSQL_WHERE;
                        lcFRule.CodeParse();

                        lcFRule.SetParam("Main", mainModel);

                        string resultValue = lcFRule.Exec(mainModel).ToString();

                        p.Where = resultValue;
                    }
                    else
                    {
                        p.Where = col.FILTER_TSQL_WHERE;
                    }

                    this.store1.SelectQuery.Add(p);
                }

                M2SecurityDataFactory.BindCatalogForCol(col, uSec, this.store1);
            }
        }



        private void Table1_CellDbclick(object sender, EventArgs e)
        {
            GoSubmit();
        }

        public void GoSubmit()
        {
            DataRecord record = this.store1.GetDataCurrent();

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (m_tSet.Table.TABLE_TYPE_ID == "MORE_VIEW")
            {
                string rowJson = "{}";
                
                if (this.table1.CheckedMode == CheckedMode.Multi && this.table1.CheckedRows.Count > 1)
                {
                    rowJson = this.table1.CheckedRows.ToJson();
                }
                else
                {
                    if (record != null)
                    {
                        rowJson = record.Fields.ToJson();
                    }
                }
                
                string mapJson = GetMapJson(m_tSet);

                ScriptManager.Eval("ownerWindow.close({result:\"ok\", row:" + rowJson + ", map:" + mapJson + " } );");
            }
            else
            {
                SModel msg = new SModel();
                msg["result"] = "ok";



                if (this.table1.CheckedMode == CheckedMode.Multi && this.table1.CheckedRows.Count > 1)
                {
                    msg["is_multi"] = true;

                    int[] ids = this.table1.CheckedRows.GetIds<int>();

                    IG2_TABLE table = m_tSet.Table;

                    LightModelFilter filter = new LightModelFilter(table.TABLE_NAME);
                    filter.AddFilter("ROW_SID >= 0");
                    filter.And(table.ID_FIELD, record.Id);

                    LModelList<LModel> models = decipher.GetModelList(filter);

                    ids = ArrayUtil.Remove<int>(int.Parse(record.Id), ids);

                    msg["rows"] = models;

                    msg["new_ids"] = ids;
                    msg["select_table"] = table.TABLE_NAME;
                    msg["map_id"] = table.IG2_TABLE_ID;
                }
                else
                {
                    if (record != null)
                    {
                        LModel model = decipher.GetModelByPk(m_tSet.Table.TABLE_NAME, record.Id);
                        msg["row"] = model;
                    }
                }

                msg["map"] = GetMaps(m_tSet);

                
                string jsonStr = msg.ToJson();

                ScriptManager.Eval("ownerWindow.close(" + jsonStr + ");");
            }

        }

        private SModelList GetMaps(TableSet tSet)
        {
            SModelList mapList = new SModelList();

            foreach (IG2_TABLE_COL item in m_tSet.Cols)
            {
                if (StringUtil.IsBlank(item.EVENT_AFTER_FIELD_ID))
                {
                    continue;
                }

                SModel map = new SModel();
                map["src"] = item.DB_FIELD;
                map["to"] = item.EVENT_AFTER_FIELD_ID;

                mapList.Add(map);
            }

            return mapList;
        }

        private string GetMapJson(TableSet tSet)
        {
            return GetMaps(tSet).ToJson();
        }


        public void StepEdit()
        {
            int id = WebUtil.QueryInt("viewId");


            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE model = decipher.SelectModelByPk<IG2_TABLE>(id);


            Window win = new Window("设置");
            win.State = WindowState.Max;
            win.ContentPath = string.Format("/App/InfoGrid2/View/OneSearch/StepEdit2.aspx?view_id={0}&owner_table_id={1}",
                id, model.VIEW_OWNER_TABLE_ID);

            win.ShowDialog();
        }



        /// <summary>
        /// 获取展示规则
        /// </summary>
        /// <returns></returns>
        public string GetDisplayRule()
        {
            return App.InfoGrid2.Bll.DisplayRuleMgr.GetJScript();
        }



    }
}