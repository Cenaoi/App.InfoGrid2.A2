using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model;
using EC5.Utility;
using App.InfoGrid2.Model.JsonModel;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EC5.SystemBoard;
using App.InfoGrid2.Model.SecModels;
using System.Text;
using HWQ.Entity.Filter;

namespace EC5.IG2.Core.UI
{

    /// <summary>
    /// 权限 UI 的表格类型
    /// </summary>
    public enum SecUiItemType
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL,
        /// <summary>
        /// 页面
        /// </summary>
        Page,
        /// <summary>
        /// 复杂页区域
        /// </summary>
        Area,
        /// <summary>
        /// 弹出窗体
        /// </summary>
        Dialog
    }

    /// <summary>
    /// 权限数据
    /// </summary>
    public class M2SecurityUiFactory
    {
        SEC_UI m_SecUI;

        /// <summary>
        /// 源表定义。
        /// </summary>
        TableSet m_SrcTableSet;


        public SEC_UI SecUI
        {
            get { return m_SecUI; }
        }


        /// <summary>
        /// 初始化 UI
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="pageTypeId"></param>
        /// <param name="pageSubTypeId">表单子类型: ONE_FORM</param>
        /// <param name="srcTableSet">源表定义。</param>
        public void InitSecUI(int pageId, string pageTypeId, string pageSubTypeId, int menuId, TableSet srcTableSet)
        {
            m_SrcTableSet = srcTableSet;

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SEC_MODE_ID", 2);
            filter.And("SEC_USER_CODE", userCode);
            filter.And("UI_PAGE_ID", pageId);
            filter.And("UI_TYPE_ID", pageTypeId);
            filter.And("UI_SUB_TYPE_ID", pageSubTypeId);
            filter.And("MENU_ID", menuId);

            filter.TSqlOrderBy = "SEC_UI_ID desc";

            m_SecUI = decipher.SelectToOneModel<SEC_UI>(filter);

        }

        /// <summary>
        /// 初始化 UI
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="pageTypeId"></param>
        /// <param name="srcTableSet">源表定义。</param>
        public void InitSecUI(int pageId,string pageTypeId,int menuId,TableSet srcTableSet)
        {
            m_SrcTableSet = srcTableSet;

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI));
            filter.And("SEC_MODE_ID", 2);
            filter.And("SEC_USER_CODE", userCode);
            filter.And("UI_PAGE_ID", pageId);
            filter.And("UI_TYPE_ID", pageTypeId);
            filter.And("MENU_ID", menuId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            filter.TSqlOrderBy = "SEC_UI_ID desc";

            m_SecUI = decipher.SelectToOneModel<SEC_UI>(filter);

        }




        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="displayModeId"></param>
        /// <param name="secTag">安全标记。</param>
        /// <param name="pageTypeId"></param>
        public void Filter_For_TableSet(string displayModeId, string secTag, string pageAreaId, TableSet tabSet)
        {
            if (m_SecUI == null)
            {
                return;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            if (m_SecUI.UI_TYPE_ID == "PAGE")
            {
                filter.And("DISPALY_MODE_ID", displayModeId);
                filter.And("PAGE_AREA_ID", pageAreaId);
            }

            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return;
            }



            List<SEC_UI_TABLECOL> suCols = decipher.SelectModels<SEC_UI_TABLECOL>(
                "SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1} AND ROW_SID >=0",
                m_SecUI.SEC_UI_ID, suTable.SEC_UI_TABLE_ID);




            foreach (SEC_UI_TABLECOL suCol in suCols)
            {
                int srcIndex = 0;

                IG2_TABLE_COL field = tabSet.Find(suCol.DB_FIELD,out srcIndex);

                if (field != null)
                {
                    field.IS_VISIBLE = suCol.IS_VISIBLE && suCol.IS_LIST_VISIBLE && suCol.IS_LIST_VISIBLE_B;
                }

            }
        }


        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="displayModeId"></param>
        /// <param name="secTag">安全标记。</param>
        /// <param name="pageTypeId"></param>
        public void Filter( string displayModeId,string secTag, string pageAreaId, Toolbar uiToolbar, Table uiTable,Store uiStore)
        {
            if (m_SecUI == null)
            {
                return;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            if (m_SecUI.UI_TYPE_ID == "PAGE")
            {
                filter.And("DISPALY_MODE_ID", displayModeId);
                filter.And("PAGE_AREA_ID", pageAreaId);
            }

            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return;
            }

            //处理工具栏权限
            Filter_Toolbar(suTable.SEC_UI_TABLE_ID, uiToolbar);

            //处理表格权限
            Filter_TableCols(suTable,uiTable,uiStore); 


        }


        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="displayModeId"></param>
        /// <param name="secTag">安全标记。</param>
        public void FilterToolbar(string displayModeId, string pageAreaId, Toolbar uiToolbar)
        {
            if (m_SecUI == null)
            {
                return;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            if (m_SecUI.UI_TYPE_ID == "PAGE")
            {
                filter.And("DISPALY_MODE_ID", displayModeId);
                filter.And("PAGE_AREA_ID", pageAreaId);
            }

            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return;
            }

            //处理工具栏权限
            Filter_Toolbar(suTable.SEC_UI_TABLE_ID, uiToolbar);
            


        }

        public void Filter_Toolbar(int secUiTableId, Toolbar uiToolbar)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_UI_TOOLBAR suToolbar = decipher.SelectToOneModel<SEC_UI_TOOLBAR>(
                "ROW_SID >=0 AND SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1}",
                m_SecUI.SEC_UI_ID, secUiTableId);


            List<SEC_UI_TOOLBAR_ITEM> suItems = decipher.SelectModels<SEC_UI_TOOLBAR_ITEM>(
                "ROW_SID>=0 AND  SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1}",
                m_SecUI.SEC_UI_ID, secUiTableId);

            ToolBarItem tItem = null;

            foreach (var item in suItems)
            {
                string id ="toolbarItem" + item.IG2_TOOLBAR_ITEM_ID;

                tItem = uiToolbar.Items.Find(id);

                if (tItem == null)
                {
                    continue;
                }

                tItem.Visible = (item.VISIBLE  && item.VISIBLE_B);
            }


        }

        /// <summary>
        /// 处理表格 UI 权限
        /// </summary>
        /// <param name="suTable"></param>
        /// <param name="uiTable"></param>
        /// <param name="uiStore"></param>
        private void Filter_TableCols(SEC_UI_TABLE suTable,Table uiTable,Store uiStore)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            List<SEC_UI_TABLECOL> suCols = decipher.SelectModels<SEC_UI_TABLECOL>(
                "SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1} AND ROW_SID >=0",
                m_SecUI.SEC_UI_ID, suTable.SEC_UI_TABLE_ID);


            if (!StringUtil.IsBlank(suTable.LOCKED_RULE))
            {
                uiStore.LockedRule = suTable.LOCKED_RULE;
            }

            //默认是开启，如果全局是默认关闭，那么在这里就可以启动
            if (suTable.VALID_MSG_ENABLED != uiTable.RowStyleEnabled)
            {
                uiTable.RowStyleEnabled = suTable.VALID_MSG_ENABLED;
            }


            TableColumnCollection cols = uiTable.Columns;

            foreach (SEC_UI_TABLECOL suCol in suCols)
            {
                BoundField field = cols.FindByDataField(suCol.DB_FIELD);

                if (field != null)
                {
                    field.Visible = suCol.IS_VISIBLE && suCol.IS_LIST_VISIBLE && suCol.IS_LIST_VISIBLE_B;
                    field.EditorMode = (suCol.IS_READONLY || suCol.IS_READONLY_B) ? EditorMode.None : EditorMode.Auto;
                }

                //如果第一个过滤条件存在，才采用过滤
                if (!string.IsNullOrEmpty(suCol.FILTER_1))
                {
                    string tWhere = GetFieldFilter(suCol);

                    TSqlWhereParam tsParam = new TSqlWhereParam();
                    tsParam.Where = tWhere;

                    uiStore.FilterParams.Add(tsParam);
                }

                if (!string.IsNullOrEmpty(suCol.FILTER_1_B))
                {
                    string tWhere = GetFieldFilterB(suCol);

                    TSqlWhereParam tsParam = new TSqlWhereParam();
                    tsParam.Where = tWhere;

                    uiStore.FilterParams.Add(tsParam);
                }

            }
        }




        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="secTag"></param>
        /// <param name="pageTypeId"></param>
        public void FilterForItem(int tableId,string col,string itemType, string displayModeId, string secTag, 
            string pageAreaId, Table uiTable, Store uiStore)
        {
            if (m_SecUI == null)
            {
                return;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            filter.And("SEC_ITEM_TYPE", itemType);
            filter.And("DIALOG_FIELD", col);
            filter.And("DIALOG_TABLE_ID", tableId);
            

            //if (m_SecUI.UI_TYPE_ID == "PAGE")
            //{
            //    filter.And("DISPALY_MODE_ID", displayModeId);
            //    filter.And("PAGE_AREA_ID", pageAreaId);
            //}

            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return;
            }

            List<SEC_UI_TABLECOL> suCols = decipher.SelectModels<SEC_UI_TABLECOL>(
                "SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1} AND ROW_SID >=0",
                m_SecUI.SEC_UI_ID, suTable.SEC_UI_TABLE_ID);


            if (!StringUtil.IsBlank(suTable.LOCKED_RULE))
            {
                uiStore.LockedRule = suTable.LOCKED_RULE;
            }

            //默认是开启，如果全局是默认关闭，那么在这里就可以启动
            if (suTable.VALID_MSG_ENABLED != uiTable.RowStyleEnabled)
            {
                uiTable.RowStyleEnabled = suTable.VALID_MSG_ENABLED;
            }


            TableColumnCollection cols = uiTable.Columns;

            foreach (SEC_UI_TABLECOL suCol in suCols)
            {
                BoundField field = cols.FindByDataField(suCol.DB_FIELD);

                if (field != null)
                {
                    field.Visible = suCol.IS_VISIBLE && suCol.IS_LIST_VISIBLE ;
                    field.EditorMode = (suCol.IS_READONLY || suCol.IS_READONLY_B) ? EditorMode.None : EditorMode.Auto;
                }

                //如果第一个过滤条件存在，才采用过滤
                if (!string.IsNullOrEmpty(suCol.FILTER_1))
                {
                    string tWhere = GetFieldFilter(suCol);

                    TSqlWhereParam tsParam = new TSqlWhereParam();
                    tsParam.Where = tWhere;

                    uiStore.FilterParams.Add(tsParam);
                }

                if (!string.IsNullOrEmpty(suCol.FILTER_1_B))
                {
                    string tWhere = GetFieldFilterB(suCol);

                    TSqlWhereParam tsParam = new TSqlWhereParam();
                    tsParam.Where = tWhere;

                    uiStore.FilterParams.Add(tsParam);
                }

            }


        }


        /// <summary>
        /// 过滤查询表单
        /// </summary>
        /// <param name="displayModeId"></param>
        /// <param name="secTag"></param>
        /// <param name="pageAreaId"></param>
        /// <param name="uiTable"></param>
        /// <param name="uiStore"></param>
        public void FilterForItemForm(int tableId, string col, string itemType, string displayModeId, string secTag, string pageAreaId, FormLayout uiForm, Store uiStore)
        {
            if (m_SecUI == null)
            {
                return;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            filter.And("SEC_ITEM_TYPE", itemType);
            filter.And("DIALOG_FIELD", col);
            filter.And("DIALOG_TABLE_ID", tableId);


            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return;
            }

            List<SEC_UI_TABLECOL> suCols = decipher.SelectModels<SEC_UI_TABLECOL>(
                "SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1} AND ROW_SID >=0",
                m_SecUI.SEC_UI_ID, suTable.SEC_UI_TABLE_ID);


            if (!StringUtil.IsBlank(suTable.LOCKED_RULE))
            {
                uiStore.LockedRule = suTable.LOCKED_RULE;
            }


            foreach (SEC_UI_TABLECOL suCol in suCols)
            {
                System.Web.UI.Control field = uiForm.FindControl("Search" + suCol.DB_FIELD);

                if (field == null)
                {
                    continue;
                }

                bool isVisibe = suCol.IS_VISIBLE && suCol.IS_SEARCH_VISIBLE;

                if (!isVisibe)
                {
                    uiForm.Controls.Remove(field);
                }
                else
                {
                    field.Visible = true;
                }
            }


        }





        /// <summary>
        /// 过滤 TableSet 里面的字段
        /// </summary>
        /// <param name="displayModeId"></param>
        /// <param name="secTag"></param>
        /// <param name="pageAreaId"></param>
        /// <param name="uiTable"></param>
        /// <param name="uiStore"></param>
        public TableSet FilterTableSet(string displayModeId, string secTag, string pageAreaId, TableSet tSet)
        {
            if (tSet == null) { throw new ArgumentNullException("tSet"); }

            if (m_SecUI == null)
            {
                return null;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return null;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);

            
            if (m_SecUI.UI_TYPE_ID == "PAGE")
            {
                if (m_SecUI.UI_SUB_TYPE_ID == "TABLE_FORM")
                {

                }
                else
                {
                    filter.And("DISPALY_MODE_ID", displayModeId);
                    filter.And("PAGE_AREA_ID", pageAreaId);
                }
            }

            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return null;
            }

            List<SEC_UI_TABLECOL> suCols = decipher.SelectModels<SEC_UI_TABLECOL>(
                "SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1} AND ROW_SID >=0",
                m_SecUI.SEC_UI_ID, suTable.SEC_UI_TABLE_ID);


            TableSet newTSet = new TableSet();
            newTSet.Cols = null;// new List<IG2_TABLE_COL>();
            newTSet.Table = tSet.Table.Clone<IG2_TABLE>();

            IG2_TABLE newTab = newTSet.Table;

            newTab.LOCKED_RULE = StringUtil.NoBlank(newTab.LOCKED_RULE, suTable.LOCKED_RULE);
            newTab.LOCKED_FIELD = StringUtil.NoBlank(newTab.LOCKED_FIELD, suTable.LOCKED_FIELD);
            newTab.LOCKED_MODE_ID = StringUtil.NoBlank(newTab.LOCKED_MODE_ID, suTable.LOCKED_MODE_ID);


            SortedList<int, IG2_TABLE_COL> dictTCols = new SortedList<int, IG2_TABLE_COL>();
            
            foreach (SEC_UI_TABLECOL suCol in suCols)
            {
                int srcIndex = 0;
                IG2_TABLE_COL col = tSet.Find(suCol.DB_FIELD,out srcIndex);

                if (col == null) { continue; }

                if (suCol.IS_VISIBLE && suCol.IS_SEARCH_VISIBLE && suCol.IS_VISIBLE_B)
                {
                    IG2_TABLE_COL newCol = col.Clone<IG2_TABLE_COL>();
                    newCol.IS_VISIBLE = true;
                    newCol.IS_LIST_VISIBLE = true;
                    
                    newCol.IS_READONLY = (suCol.IS_READONLY || suCol.IS_READONLY_B);

                    dictTCols[srcIndex] = newCol;

                    //newTSet.Cols.Add(newCol);
                }

            }

            newTSet.Cols = new List<IG2_TABLE_COL>( dictTCols.Values);



            return newTSet;

        }




        /// <summary>
        /// 过滤查询表单
        /// </summary>
        /// <param name="displayModeId"></param>
        /// <param name="secTag"></param>
        /// <param name="pageAreaId"></param>
        /// <param name="uiTable"></param>
        /// <param name="uiStore"></param>
        public void FilterForForm(string displayModeId, string secTag, string pageAreaId, FormLayout uiForm, Store uiStore)
        {
            if (m_SecUI == null)
            {
                return;
            }

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            string userCode = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI_TABLE));
            filter.And("SEC_UI_ID", m_SecUI.SEC_UI_ID);
            filter.And("PAGE_ID", m_SecUI.UI_PAGE_ID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            if (m_SecUI.UI_TYPE_ID == "PAGE")
            {
                filter.And("DISPALY_MODE_ID", displayModeId);
                filter.And("PAGE_AREA_ID", pageAreaId);
            }

            SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(filter);

            if (suTable == null)
            {
                return;
            }

            List<SEC_UI_TABLECOL> suCols = decipher.SelectModels<SEC_UI_TABLECOL>(
                "SEC_UI_ID={0} AND SEC_UI_TABLE_ID = {1} AND ROW_SID >=0",
                m_SecUI.SEC_UI_ID, suTable.SEC_UI_TABLE_ID);


            if (!StringUtil.IsBlank(suTable.LOCKED_RULE))
            {
                uiStore.LockedRule = suTable.LOCKED_RULE;
            }


            foreach (SEC_UI_TABLECOL suCol in suCols)
            {
                System.Web.UI.Control field = uiForm.FindControl("Search" + suCol.DB_FIELD);

                if (field == null)
                {
                    continue;
                }

                bool isVisibe = suCol.IS_VISIBLE && suCol.IS_SEARCH_VISIBLE && suCol.IS_VISIBLE_B;

                if (!isVisibe)
                {
                    uiForm.Controls.Remove(field);
                }
                else
                {
                    field.Visible = true;
                }
            }


        }

        /// <summary>
        /// 过滤参数替换
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string FilterReplace(string filter)
        {
            EcContext context = EcContext.Current;
            EcUserState user = context.User;
            string userCode = user.ExpandPropertys["USER_CODE"];

            string newFilter =  filter.Replace("(GetBizUserCode())", $"'{userCode}'");

            return newFilter;
        }

        /// <summary>
        /// 二级过滤
        /// </summary>
        /// <param name="vField"></param>
        /// <returns></returns>
        private string GetFieldFilterB(SEC_UI_TABLECOL vField)
        {

            StringBuilder sb = new StringBuilder();

            int count = 1;

            string tf;

            if (StringUtil.IsBlank(vField.DB_TABLE))
            {
                tf = vField.DB_FIELD;
            }
            else
            {
                tf = string.Concat(vField.DB_TABLE, ".", vField.DB_FIELD);
            }

            sb.AppendFormat("({0} {1})", tf, FilterReplace(vField.FILTER_1_B));

            if (!StringUtil.IsBlank(vField.FILTER_2_B))
            {
                sb.AppendFormat("OR ({0} {1})", tf, FilterReplace( vField.FILTER_2_B));

                count++;
            }

            if (!StringUtil.IsBlank(vField.FILTER_3_B))
            {
                sb.AppendFormat("OR ({0} {1})", tf, FilterReplace(vField.FILTER_3_B));

                count++;
            }

            string tWhere;

            if (count > 1)
            {
                tWhere = "(" + sb.ToString() + ")";
            }
            else
            {
                tWhere = sb.ToString();
            }

            return tWhere;
        }

        /// <summary>
        /// 一级过滤
        /// </summary>
        /// <param name="vField"></param>
        /// <returns></returns>
        private string GetFieldFilter(SEC_UI_TABLECOL vField)
        {

            StringBuilder sb = new StringBuilder();

            int count = 1;

            string tf = null;

            IG2_TABLE table = m_SrcTableSet != null ? m_SrcTableSet.Table : null;


            string typeId = (table != null) ? table.TABLE_TYPE_ID : string.Empty;

            if (table != null && typeId == "MORE_VIEW")
            {
                int srcIndex = 0;
                var srcField = m_SrcTableSet.Find(vField.DB_FIELD,out srcIndex);

                if (srcField == null)
                {
                    throw new Exception(string.Format("视图字段不存在。{0}.{1}", vField.DB_FIELD));
                }
                
                tf = srcField.VIEW_FIELD_SRC;

            }
            else
            {
                if (StringUtil.IsBlank(vField.DB_TABLE))
                {
                    tf = vField.DB_FIELD;
                }
                else
                {
                    tf = string.Concat(vField.DB_TABLE, ".", vField.DB_FIELD);
                }
            }

            sb.AppendFormat("({0} {1})", tf, FilterReplace( vField.FILTER_1));

            if (!StringUtil.IsBlank(vField.FILTER_2))
            {
                sb.AppendFormat("OR ({0} {1})", tf, FilterReplace(vField.FILTER_2));

                count++;
            }

            if (!StringUtil.IsBlank(vField.FILTER_3))
            {
                sb.AppendFormat("OR ({0} {1})", tf, FilterReplace(vField.FILTER_3));

                count++;
            }

            string tWhere;

            if (count > 1)
            {
                tWhere = "(" + sb.ToString() + ")";
            }
            else
            {
                tWhere = sb.ToString();
            }

            return tWhere;
        }



    }
}