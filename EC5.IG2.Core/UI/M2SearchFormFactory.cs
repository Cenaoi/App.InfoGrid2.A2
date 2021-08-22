using System;
using System.Collections.Generic;
using System.Web;
using App.InfoGrid2.Model.DataSet;
using System.Web.UI;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using EC5.Utility;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.Model.JsonModel;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace EC5.IG2.Core.UI
{
    public class M2SearchFormFactory
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 数据仓库ID
        /// </summary>
        public string StoreID { get; set; }

        bool m_IsPostBack = false;

        public bool IsPostBack
        {
            get { return m_IsPostBack; }
            set { m_IsPostBack = value; }
        }

        public M2SearchFormFactory(bool isPostBack,string storeId)
        {
            m_IsPostBack = isPostBack;
            this.StoreID = storeId;
        }



        /// <summary>
        /// 定义查询框
        /// </summary>
        /// <param name="searchForm"></param>
        /// <param name="tableSet"></param>
        public void CreateControls(FormLayout searchForm, TableSet tableSet)
        {
            searchForm.Visible = false;

            CreateControls(searchForm, tableSet.Cols);                      
        }

        /// <summary>
        /// 定义查询框
        /// </summary>
        /// <param name="searchForm"></param>
        /// <param name="tabCols"></param>
        public void CreateControls(FormLayout searchForm, List<IG2_TABLE_COL> tabCols)
        {
            ControlCollection cons = searchForm.Controls;

            SortedList<string, Control> dictCons = new SortedList<string, Control>();


            List<IG2_TABLE_COL> displayCols = new List<IG2_TABLE_COL>();

            //先找出要显示的字段名
            foreach (IG2_TABLE_COL item in tabCols)
            {
                if (item.SEC_LEVEL > 6)
                {
                    continue;
                }

                if (item.ROW_SID == -3 || !item.IS_VISIBLE || !item.IS_SEARCH_VISIBLE)
                {
                    continue;
                }

                displayCols.Add(item);
            }

            foreach (IG2_TABLE_COL item in displayCols)
            {


                Control con = CreateSearchControl(item);

                if (con == null)
                {
                    continue;
                }


                string newId = "Search" + item.DB_FIELD;

                for (int i = 0; i < 999; i++)
                {
                    if (dictCons.ContainsKey(newId))
                    {
                        newId = "Search" + item.DB_FIELD + "_N" + i;
                    }
                    else
                    {
                        break;
                    }
                }

                con.ID = newId;

                //if (con is IDelayRender)
                //{
                //    ((IDelayRender)con).IsDelayRender = true;
                //}

                cons.Add(con);
            }

            SearchButtonGroup searcbBtns = new SearchButtonGroup();
            searcbBtns.ID = "SearchBtnGroup1";

            cons.Add(searcbBtns);

        }


        /// <summary>
        /// 创建查询子控件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private FieldBase CreateSearchControl(IG2_TABLE_COL item)
        {
            FieldBase con = null;

            string dbType = item.DB_TYPE.ToLower();

            if (item.ACT_MODE == "FIXED")
            {
                ComboBox combo = new ComboBox();
                combo.TriggerMode = TriggerMode.None;

                if (!m_IsPostBack)
                {
                    ProFixed(item, combo);
                }


                con = combo;
            }
            else if (item.ACT_MODE == "TABLE")
            {
                //如果是下拉框,
                //if ("SelectColumn".Equals(item.V_LIST_MODE_ID, StringComparison.OrdinalIgnoreCase))
                //{
                //    ComboBoxBase col = new ComboBoxBase();
                //    col.TriggerMode = (item.V_TRIGGER_MODE == "NONE") ? TriggerMode.None : TriggerMode.UserInput;

                //    if (!m_IsPostBack)
                //    {
                //        ProTableCol(item, col);
                //    }

                //    con = col;
                //}
                //else
                //{
                //    TextBox txtBox = new TextBox();
                //    txtBox.DataLogic = "like";

                //    con = txtBox;
                //}

                //如果是下拉框,
                if ("SelectColumn".Equals(item.V_LIST_MODE_ID, StringComparison.OrdinalIgnoreCase))
                {
                    ComboBoxBase col = new ComboBoxBase();
                    col.TriggerMode = (item.V_TRIGGER_MODE == "NONE") ? TriggerMode.None : TriggerMode.UserInput;

                    if (item.DB_TYPE == "string")
                    {
                        col.DataLogic = "like";
                    }

                    if (!this.IsPostBack)
                    {
                        ProTableCol(item, col);
                    }

                    con = col;
                }
                else if ("TriggerColumn".Equals(item.V_LIST_MODE_ID, StringComparison.OrdinalIgnoreCase))
                {
                    TriggerBox col = new TriggerBox();
                    col.TriggerMode = (item.V_TRIGGER_MODE == "NONE") ? TriggerMode.None : TriggerMode.UserInput;
                    col.ButtonType = TriggerButtonType.More;
                    col.OnButtonClick = "if(showSelectWinForTable ) { showSelectWinForTable(this); } else { showDialgoForTable(this); }";

                    if (item.DB_TYPE == "string")
                    {
                        col.DataLogic = "like";
                    }

                    if (!this.IsPostBack)
                    {
                        ProTableCol(item, col);
                    }

                    con = col;
                }
                else
                {
                    TextBox txtBox = new TextBox();
                    txtBox.DataLogic = "like";

                    con = txtBox;
                }

            }
            else
            {
                switch (dbType)
                {
                    case "int":
                    case "currency":
                    case "decimal":
                        {
                            NumRangeBox tb = new NumRangeBox();

                            con = tb;
                        }
                        break;
                    case "date":
                    case "datetime":
                        {
                            DateRangePicker drp = new DateRangePicker();
                            drp.SetAttribute("ColSpan", "2");
                            con = drp;
                        }
                        break;
                    case "bool":
                    case "boolean":
                        {
                            ComboBox ddl = new ComboBox();
                            ddl.Items.Add("true");
                            ddl.Items.Add("false");

                            con = ddl;
                        }
                        break;
                    case "string":
                    default:
                        {
                            TextBox tb = new TextBox();
                            con = tb;
                            con.DataLogic = "like";
                        }
                        break;
                }
            }

            if (con == null)
            {
                return null;
            }

            con.FieldLabel = StringUtil.NoBlank(item.DISPLAY, item.F_NAME);

            con.DataField = StringUtil.NoBlank(item.VIEW_FIELD_SRC, item.DB_FIELD); 

            return con;
        }




        private void ProFixed(IG2_TABLE_COL item, ComboBox col)
        {
            string[] listItems = StringUtil.Split(item.ACT_FIXED_ITEMS, "\n", ";", "$$$");

            foreach (string li in listItems)
            {
                if (li.Trim().Length == 0)
                {
                    continue;
                }

                if (li.Contains("{{") && li.Contains("}}"))
                {
                    int n1 = li.IndexOf("{{");
                    int n2 = li.IndexOf("}}", n1 + 2);

                    string code = li.Substring(n1 + 2, n2 - n1 - 2);

                    string otherText = li.Substring(n2 + 2);

                    int eqIndex = otherText.IndexOf("=");

                    string text = otherText.Substring(eqIndex + 1).Trim();

                    col.Items.Add(code, text);

                }
                else
                {
                    KeyValueString kv = KeyValueString.Parse(li);

                    col.Items.Add(kv.Key, kv.Value);
                    
                }
            }
        }


        private void ProTableCol(IG2_TABLE_COL item, ComboBoxBase col)
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

                col.ValueField = tab.ENUM_VALUE_FIELD;
                col.DisplayField = tab.ENUM_TEXT_FIELD;

                LightModelFilter filter = new LightModelFilter(ati.table_name.ToUpper());
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
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

                        string valueStr;

                        if (value == null)
                        {
                            valueStr = string.Empty;
                        }
                        else
                        {
                            valueStr = value.ToString();
                        }

                        itemBase.SetAttribute(field, valueStr);
                    }

                    col.Items.Add(itemBase);
                }


                //暂时不支持映射
                //foreach (MapItem mapItem in mapItems)
                //{
                //    col.MapItems.Add(mapItem);
                //}


            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private void ProTableCol(IG2_TABLE_COL item, TriggerBox col)
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

                string targetField = "";

                foreach (IG2_TABLE_COL tCol in tSet.Cols)
                {
                    if (StringUtil.IsBlank(tCol.EVENT_AFTER_FIELD_ID))
                    {
                        continue;
                    }

                    if (tCol.EVENT_AFTER_FIELD_ID == item.DB_FIELD)
                    {
                        targetField = tCol.DB_FIELD;

                        break;
                    }


                    //col.MapItems.Add(new MapItem(tCol.DB_FIELD, tCol.EVENT_AFTER_FIELD_ID));
                    
                }

                col.OnMaping = string.Format("biz_mapping(this,'VIEW',{0},'{1}')", ati.view_id, targetField);
            }

        }

    }

}