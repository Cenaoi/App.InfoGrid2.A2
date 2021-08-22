using System;
using System.Collections.Generic;
using System.Web;
using App.BizCommon;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.JsonModel;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace EC5.IG2.Core.UI
{
    /// <summary>
    /// 界面表格列的构造工厂
    /// </summary>
    public class M2TableFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool m_IsPostBack = false;


        public bool IsPostBack
        {
            get { return m_IsPostBack; }
            set { m_IsPostBack = value; }
        }


        /// <summary>
        /// 界面表格列的构造工厂(构造函数)
        /// </summary>
        /// <param name="isPostBack">是否 Post 提交</param>
        public M2TableFactory(bool isPostBack)
        {
            m_IsPostBack = isPostBack;
        }

        public void CreateTableColumns(Table tableUi, Store storeUi, TableSet tableSet)
        {
            CreateTableColumns(tableUi, storeUi, tableSet, true);
        }

        /// <summary>
        /// 创建表格列
        /// </summary>
        /// <param name="tableUi"></param>
        /// <param name="storeUi"></param>
        /// <param name="tableSet"></param>
        /// <param name="autoInsertFilter">自动插入过滤条件</param>
        public void CreateTableColumns(Table tableUi,Store storeUi, TableSet tableSet, bool autoInsertFilter)
        {
            Table dataGrid = tableUi;

            tableUi.SummaryVisible = tableSet.Table.SUMMARY_VISIBLE;


            TableColumnCollection cols = dataGrid.Columns;

            RowNumberer rowNumCol = new RowNumberer();
            cols.Add(rowNumCol);

            RowCheckColumn rowCheckCol = new RowCheckColumn();
            cols.Add(rowCheckCol);


            #region 功能

            if (tableSet.Table.VISIBLE_BTN_EDIT)
            {
                ActionColumn actCol = new ActionColumn();
                actCol.HeaderText = "功能";
                actCol.Width = 80;
                
                actCol.Items.Add(new ActionItem()
                {
                    DisplayMode= DisplayMode.Text,
                    Text = "编辑",
                    Handler = "form_EditShow"
                });


                cols.Add(actCol);
            }

            #endregion


            IG2_TABLE tab = tableSet.Table;

            bool isMoveView = (tab.TABLE_TYPE_ID == "MORE_VIEW");

            bool isView = (tab.TABLE_TYPE_ID == "VIEW");


            List<IG2_TABLE_COL> tabCols = new List<IG2_TABLE_COL>();

            foreach (IG2_TABLE_COL item in tableSet.Cols)
            {
                if (autoInsertFilter && isView && !StringUtil.IsBlank( item.FILTER_LOGIC,item.FILTER_VALUE))
                {
                    Param pn = new Param(item.DB_FIELD, item.FILTER_VALUE);
                    pn.Logic = item.FILTER_LOGIC;
                    storeUi.FilterParams.Add(pn);
                }


                if (item.SEC_LEVEL > 6)
                {
                    continue;
                }

                if (item.ROW_SID == -3 || !item.IS_VISIBLE || !item.IS_LIST_VISIBLE)
                {
                    continue;
                }

                tabCols.Add(item);

            }

            List<GroupColumn> groups = new List<GroupColumn>();   //分组索引

            GroupColumn curGroupCol ;

            foreach (IG2_TABLE_COL item in tabCols)
            {
                BoundField col = CreateGridCol(item, isMoveView);

                curGroupCol = GetGroupColumn(item,cols, groups);

                if (curGroupCol != null)
                {
                    curGroupCol.Columns.Add(col);
                }
                else
                {
                    cols.Add(col);
                }
            }


            //创建合计栏目
            CreateSummary(tableUi, storeUi, tableSet, isMoveView);
                
        }

        /// <summary>
        /// 获取组列
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cols"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        private GroupColumn GetGroupColumn(IG2_TABLE_COL item,TableColumnCollection cols,  List<GroupColumn> groups)
        {
            if (item == null) { throw new ArgumentNullException("item"); }
            if (cols == null) { throw new ArgumentNullException("cols"); }
            if (groups == null) { throw new ArgumentNullException("groups"); }

            if (StringUtil.IsBlank(item.GROUP_ID))
            {
                return null;
            }

            string[] groupStr = StringUtil.Split(item.GROUP_ID, "|");

            if (groupStr.Length == 0)
            {
                return null;
            }

            GroupColumn curGroupCol = null;


            for (int i = 0; i < groupStr.Length; i++)
            {
                string text = groupStr[i].Trim() ;

                if (i < groups.Count)
                {
                    if (groups[i].HeaderText == text)
                    {
                        curGroupCol = groups[i];

                        continue;
                    }
                    else
                    {
                        groups.RemoveRange(i, groups.Count - i);
                    }
                }


                GroupColumn gc = new GroupColumn(text);

                groups.Add(gc);

                if (i == 0)
                {
                    cols.Add(gc);
                }
                else
                {
                    curGroupCol.Columns.Add(gc);
                }

                curGroupCol = gc;
            }

            return curGroupCol;
        }





        /// <summary>
        /// 创建合计栏目
        /// </summary>
        private void CreateSummary(Table tableUi, Store storeUi, TableSet tableSet, bool isMoveView)
        {
            if (!tableUi.SummaryVisible)
            {
                return;
            }

            foreach (IG2_TABLE_COL tCol in tableSet.Cols)
            {
                if (StringUtil.IsBlank(tCol.SUMMARY_TYPE))
                {
                    continue;
                }

                SummaryType sType = EnumUtil.Parse<SummaryType>(tCol.SUMMARY_TYPE, true);

                SummaryField sumField;

                if (isMoveView && !StringUtil.IsBlank(tCol.VIEW_FIELD_SRC))
                {
                    sumField = storeUi.SummaryFields.Add(tCol.VIEW_FIELD_SRC, tCol.DB_FIELD, sType);
                }
                else
                {
                    sumField = storeUi.SummaryFields.Add(tCol.DB_FIELD, sType);
                }

                if (!StringUtil.IsBlank(tCol.SUMMARY_FILTER))
                {
                    TSqlWhereParam tSqlWhereParam = new TSqlWhereParam();
                    tSqlWhereParam.Where = tCol.SUMMARY_FILTER;

                    sumField.Filter.Add(tSqlWhereParam);
                }
            }




        }



        /// <summary>
        /// 创建表格列
        /// </summary>
        /// <param name="item"></param>
        private BoundField CreateGridCol(IG2_TABLE_COL item, bool isMoveView)
        {
            BoundField baseCol = null;

            item.ACT_MODE = item.ACT_MODE.ToUpper();

            

            //固定值,一般采用下拉框.
            if (item.ACT_MODE == "FIXED")
            {
                SelectColumn col = new SelectColumn();
                //col.TriggerMode = TriggerMode.None;

                col.TriggerMode = (item.V_TRIGGER_MODE == "USERINPUT") ? TriggerMode.UserInput: TriggerMode.None ;

                //if (!m_IsPostBack)
                //{
                ProFixed(item, col);
                //}

                baseCol = col;
            }
            else if (item.ACT_MODE == "TABLE")
            {
                //如果是下拉框,
                if ("SelectColumn".Equals(item.V_LIST_MODE_ID, StringComparison.OrdinalIgnoreCase))
                {
                    //SelectColumn col = new SelectColumn();
                    //col.TriggerMode = (item.V_TRIGGER_MODE == "NONE") ? TriggerMode.None : TriggerMode.UserInput;

                    //ProTableCol(item, col);

                    SelectBaseColumn col = new SelectBaseColumn();
                    col.TriggerMode = (item.V_TRIGGER_MODE == "USERINPUT") ? TriggerMode.UserInput : TriggerMode.None;

                    //if (!m_IsPostBack)
                    //{
                    ProTableCol(item, col);
                    //}

                    baseCol = col;
                }
                else
                {
                    TriggerColumn col = new TriggerColumn();
                    col.ButtonType = TriggerButtonType.More;
                    col.OnButtonClick = "showDialgoForTable(this)";

                    col.TriggerMode = (item.V_TRIGGER_MODE == "USERINPUT") ? TriggerMode.UserInput : TriggerMode.None;


                    if (!m_IsPostBack)
                    {
                        ProTableCol(item, col);
                    }

                    baseCol = col;
                }

            }
            else
            {
                switch (item.DB_TYPE.ToLower())
                {
                    case "datetime":
                        {
                            DateTimeColumn col = new DateTimeColumn();
                            baseCol = col;
                        }
                        break;
                    case "date":
                        {
                            DateColumn col = new DateColumn();
                            baseCol = col;
                        }
                        break;
                    case "boolean":
                        {
                            CheckColumn col = new CheckColumn();

                            baseCol = col;
                        }
                        break;
                    case "int":
                        {
                            NumColumn col = new NumColumn();
                            col.Format = "0";
                            baseCol = col;
                        }
                        break;
                    case "currency":
                        {
                            NumColumn col = new NumColumn();
                            col.Format = GetFormat(item.FORMAT, item.DB_DOT);
                            col.NotDisplayValue = "0";

                            baseCol = col;
                        }
                        break;
                    case "decimal":
                        {
                            NumColumn col = new NumColumn();
                            col.Format = GetFormat(item.FORMAT, item.DB_DOT);
                            col.NotDisplayValue = "0";

                            baseCol = col;
                        }
                        break;
                    case "string":
                        {
                            string dpType = item.DISPLAY_TYPE.ToLower();

                            if ("textarea" == dpType)
                            {
                                TextAreaColumn ta = new TextAreaColumn();
                                baseCol = ta;
                            }
                            else if("more_file_upload" == dpType)
                            {
                                MoreFileColumn mfc = new MoreFileColumn();
                                mfc.View = MoreFileView.List;
                                baseCol = mfc;
                            }
                            else if ("more_image_upload" == dpType)
                            {
                                MoreFileColumn mfc = new MoreFileColumn();
                                mfc.View = MoreFileView.Large;
                                baseCol = mfc;
                            }
                            else
                            {
                                BoundField col = new BoundField();
                                baseCol = col;
                            }
                            break;
                        }
                    default:
                        {
                            BoundField col = new BoundField();
                            baseCol = col;
                        }
                        break;
                }

            }

            baseCol.DataField = item.DB_FIELD;
            baseCol.HeaderText = StringUtil.NoBlank(item.DISPLAY, item.F_NAME);
            baseCol.EditorMode = item.IS_READONLY ? EditorMode.None : EditorMode.Auto;

            
            baseCol.DataFormatString = item.FORMAT;

            baseCol.ItemAlign = EnumUtil.Parse<EasyClick.Web.Mini.CellAlign>(item.ANGLE, EasyClick.Web.Mini.CellAlign.Left);

            baseCol.Width = (item.DISPLAY_LEN > 0 ? item.DISPLAY_LEN : 120);

            baseCol.SummaryType = item.SUMMARY_TYPE;

            baseCol.Required = item.IS_BIZ_MANDATORY;

            baseCol.Placeholder = item.FORM_PLACEHOLER;


            #region  字段展示样式...

            string ruleCode = item.DISPLAY_RULE_CODE;

            if (!StringUtil.IsBlank(ruleCode))
            {
                if (StringUtil.StartsWith(ruleCode, "RE:"))
                {
                    string ruleName = ruleCode.Substring(3);

                    baseCol.Renderer = string.Format("Mini2.DisplayRule.getRule('{0}')",ruleName);
                }
            }


            #endregion

            if (isMoveView && !StringUtil.IsBlank(item.VIEW_FIELD_SRC))
            {
                baseCol.SortExpression = item.VIEW_FIELD_SRC;
            }

            return baseCol;
        }

        /// <summary>
        /// 获取格式化
        /// </summary>
        /// <param name="dot"></param>
        /// <returns></returns>
        private string GetFormat(string format, int dot)
        {
            if (!string.IsNullOrEmpty(format))
            {
                return format;
            }

            if (dot == 0)
            {
                return "0";
            }

            string result =  "0,000.".PadRight(6 + dot,'0');

            return result;
        }

        private void ProFixed(IG2_TABLE_COL item, SelectColumn col)
        {
            string[] listItems = StringUtil.Split(item.ACT_FIXED_ITEMS, "\n", ";", "$$$");

            foreach (string li in listItems)
            {
                if (li.Trim().Length == 0)
                {
                    continue;
                }


                int n = li.IndexOf("=");

                string v;
                string t;

                if (n > 0)
                {
                    v = li.Substring(0, n);
                    t = li.Substring(n + 1);
                }
                else
                {
                    t = v = li;
                }

                col.Items.Add(v, t);
            }
        }

        /// <summary>
        /// 处理弹出或下拉表格数据的
        /// </summary>
        /// <param name="item"></param>
        /// <param name="col"></param>
        private void ProTableCol(IG2_TABLE_COL item, SelectBaseColumn col)
        {
            if (string.IsNullOrEmpty(item.ACT_TABLE_ITEMS))
            {
                return;
            }

            try
            {
                ActTableItem ati = Newtonsoft.Json.JsonConvert.DeserializeObject<ActTableItem>(item.ACT_TABLE_ITEMS);

                DbDecipher decipher = ModelAction.OpenDecipher();

                TableSet tSet = TableSet.Select(decipher, ati.view_id);

                IG2_TABLE tab = tSet.Table;

                if (StringUtil.IsBlank(tab.ENUM_TEXT_FIELD) ||
                    StringUtil.IsBlank(tab.ENUM_VALUE_FIELD))
                {
                    return;
                }

                col.ItemValueField = tab.ENUM_VALUE_FIELD;
                col.ItemDisplayField = tab.ENUM_TEXT_FIELD;

                string tableName = ati.table_name.ToUpper();

                LightModelFilter filter = new LightModelFilter(tableName);
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


                #region 权限-结构过滤

                IG2_TABLE srcTab = tab;   //最原始的数据表

                if (srcTab.TABLE_TYPE_ID != "TABLE")
                {
                    srcTab = App.InfoGrid2.Bll.TableMgr.GetTableForName(srcTab.TABLE_NAME);
                }

                if (srcTab.SEC_STRUCT_ENABLED)
                {
                    UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

                    if (uSec != null)
                    {
                        filter.And("BIZ_CATA_CODE", uSec.ArrCatalogCode, Logic.In);
                    }
                }

                #endregion


                filter.Locks.Add(LockType.NoLock);

                LModelList<LModel> models = decipher.GetModelList(filter);

                List<string> fields = new List<string>();

                List<MapItem> mapItems = new List<MapItem>();

                foreach (IG2_TABLE_COL tCol in tSet.Cols)
                {
                    if (StringUtil.IsBlank(tCol.EVENT_AFTER_FIELD_ID))
                    {
                        continue;
                    }

                    fields.Add(tCol.DB_FIELD);

                    mapItems.Add(new MapItem(tCol.DB_FIELD, tCol.EVENT_AFTER_FIELD_ID));
                }

                if (!fields.Contains(tab.ENUM_VALUE_FIELD)) { fields.Add(tab.ENUM_VALUE_FIELD); }
                if (!fields.Contains(tab.ENUM_TEXT_FIELD)) { fields.Add(tab.ENUM_TEXT_FIELD); }

                foreach (LModel model in models)
                {
                    ListItemBase itemBase = new ListItemBase();

                    foreach (string field in fields)
                    {
                        object value = model[field];

                        string valueStr = (value != null) ? value.ToString() : string.Empty;

                        itemBase.SetAttribute(field, valueStr);
                    }

                    col.Items.Add(itemBase);
                }


                foreach (MapItem mapItem in mapItems)
                {
                    col.MapItems.Add(mapItem);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private void ProTableCol(IG2_TABLE_COL item, SelectColumn col)
        {
            if (string.IsNullOrEmpty(item.ACT_TABLE_ITEMS))
            {
                return;
            }

            try
            {
                ActTableItem ati = Newtonsoft.Json.JsonConvert.DeserializeObject<ActTableItem>(item.ACT_TABLE_ITEMS);

                DbDecipher decipher = ModelAction.OpenDecipher();

                TableSet tSet = TableSet.Select(decipher, ati.view_id);

                IG2_TABLE tab = tSet.Table;

                if (StringUtil.IsBlank(tab.ENUM_TEXT_FIELD) || StringUtil.IsBlank(tab.ENUM_VALUE_FIELD))
                {
                    return;
                }

                LightModelFilter filter = new LightModelFilter(ati.table_name.ToUpper());
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                #region 权限-结构过滤

                IG2_TABLE srcTab = tab;   //最原始的数据表

                if (srcTab.TABLE_TYPE_ID != "TABLE")
                {
                    srcTab = App.InfoGrid2.Bll.TableMgr.GetTableForName(srcTab.TABLE_NAME);
                }

                if (srcTab.SEC_STRUCT_ENABLED)
                {
                    UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

                    if (uSec != null)
                    {
                        filter.And("BIZ_CATA_CODE", uSec.ArrCatalogCode, Logic.In);
                    }
                }

                #endregion


                filter.Locks.Add(LockType.NoLock);

                LModelList<LModel> models = decipher.GetModelList(filter);

                foreach (LModel model in models)
                {
                    string value = model[tab.ENUM_VALUE_FIELD].ToString();
                    string text = model[tab.ENUM_TEXT_FIELD].ToString();

                    col.Items.Add(value, text);
                }


               
                FullItems(col, tSet);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        /// <summary>
        /// 填充映射条目
        /// </summary>
        private void FullItems(SelectColumn col,TableSet tSet)
        {
            foreach (IG2_TABLE_COL tCol in tSet.Cols)
            {
                if (StringUtil.IsBlank(tCol.EVENT_AFTER_FIELD_ID))
                {
                    continue;
                }

                col.MapItems.Add(new MapItem(tCol.DB_FIELD, tCol.EVENT_AFTER_FIELD_ID));
            }

        }


        private void ProTableCol(IG2_TABLE_COL item, TriggerColumn col)
        {
            if (string.IsNullOrEmpty(item.ACT_TABLE_ITEMS))
            {
                return;
            }

            col.Tag = item.ACT_TABLE_ITEMS;


            if (col.TriggerMode == TriggerMode.UserInput)
            {
                ActTableItem ati = Newtonsoft.Json.JsonConvert.DeserializeObject<ActTableItem>(item.ACT_TABLE_ITEMS);


                DbDecipher decipher = ModelAction.OpenDecipher();

                TableSet tSet = TableSet.Select(decipher, ati.view_id);

                IG2_TABLE tab = tSet.Table;

                string targetField ="";

                foreach (IG2_TABLE_COL tCol in tSet.Cols)
                {
                    if (StringUtil.IsBlank(tCol.EVENT_AFTER_FIELD_ID))
                    {
                        continue;
                    }

                    if (tCol.EVENT_AFTER_FIELD_ID == item.DB_FIELD)
                    {
                        targetField = tCol.DB_FIELD;
                    }

                    col.MapItems.Add(new MapItem(tCol.DB_FIELD, tCol.EVENT_AFTER_FIELD_ID));
                }

                col.OnMaping = string.Format("biz_mapping(this,'VIEW',{0},'{1}')", ati.view_id, targetField);
            }

        }




    }
}