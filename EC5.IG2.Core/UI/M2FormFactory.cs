using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model;
using System.Web.UI;
using EC5.Utility;
using App.InfoGrid2.Model.JsonModel;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Bll.Sec;

namespace EC5.IG2.Core.UI
{
    public class M2FormFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public bool IsPostBack { get; set; }


        public M2FormFactory(bool isPostBack)
        {
            this.IsPostBack = isPostBack;
        }

        public void CreateFormControls(FormLayout formUi, TableSet tableSet)
        {

            IG2_TABLE tab = tableSet.Table;

            foreach (IG2_TABLE_COL item in tableSet.Cols)
            {
                if (item.SEC_LEVEL > 6)
                {
                    continue;
                }

                if (item.ROW_SID == -3 || !item.IS_VISIBLE || !item.IS_LIST_VISIBLE)
                {
                    continue;
                }


                if (item.FORM_IS_NEWLINE)
                {
                    Literal newLine = new Literal("<div class=\"mi-newline\" style=\"width:96%;height:1px;\" ></div>");


                    formUi.Controls.Add(newLine);
                }


                FieldBase field = CreateFormControl(formUi, item,tab);

                formUi.Controls.Add(field);
            }

        }


        /// <summary>
        /// 创建查询子控件
        /// </summary>
        /// <param name="formUi"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private FieldBase CreateFormControl(FormLayout formUi, IG2_TABLE_COL item,IG2_TABLE table)
        {
            FieldBase con = null;

            string dbType = item.DB_TYPE.ToLower();


            if (item.ACT_MODE == "FIXED")
            {
                ComboBox combo = new ComboBox();
                combo.TriggerMode = TriggerMode.None;

                if (!this.IsPostBack)
                {
                    ProFixed(item, combo);
                }


                con = combo;
            }
            else if (item.ACT_MODE == "TABLE")
            {
                //如果是下拉框,
                if ("SelectColumn".Equals(item.V_LIST_MODE_ID, StringComparison.OrdinalIgnoreCase))
                {
                    ComboBoxBase col = new ComboBoxBase();
                    col.TriggerMode = (item.V_TRIGGER_MODE == "NONE") ? TriggerMode.None : TriggerMode.UserInput;

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
                    col.OnButtonClick = "showDialgoForTable(this)";

                    //if (!this.IsPostBack)
                    //{
                        ProTableCol(item, col);
                    //}

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
                        {
                            NumberBox tb = new NumberBox();

                            con = tb;
                        }
                        break;
                    case "decimal":
                        {
                            NumberBox tb = new NumberBox();

                            con = tb;
                        }
                        break;
                    case "datetime":
                        {
                            DatePicker drp = new DatePicker();
                            //drp.SetAttribute("ColSpan", "3");
                            con = drp;
                        }
                        break;
                    case "boolean":
                        {
                            CheckBox ddl = new CheckBox();
                            //ddl.Items.Add("true");
                            //ddl.Items.Add("false");

                            con = ddl;
                        }
                        break;
                    case "string":
                        {
                            string dpType = item.DISPLAY_TYPE.ToLower();

                            if ("textarea" == dpType)
                            {
                                Textarea ta = new Textarea();
                                con = ta;
                            }
                            else if("html_edit" == dpType)
                            {
                                HtmlEditor he = new HtmlEditor();
                                con = he;
                            }
                            else if("file_upload" == dpType)
                            {
                                FileUpload fu = new FileUpload();
                                //fu.FileUrl = "";
                                con = fu;
                            }
                            else if("more_file_upload" == dpType)
                            {
                                int row_pk = Utility.Web.WebUtil.QueryInt("row_pk");

                                MoreFileUpload mfu = new MoreFileUpload();

                                mfu.DisplayMode = FileUploadDisplayMode.None;

                                UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormHandle.aspx");
                                uri.Append("method", "image_upload");
                                uri.Append("table", table.TABLE_NAME);
                                uri.Append("field", item.DB_FIELD);
                                uri.Append("item_id", row_pk);

                                mfu.FileUrl = uri.ToString();

                                con = mfu;
                            }
                            else if("more_image_upload" == dpType)
                            {
                                int row_pk = Utility.Web.WebUtil.QueryInt("row_pk");

                                MoreFileUpload mfu = new MoreFileUpload();

                                mfu.DisplayMode = FileUploadDisplayMode.Preview;

                                UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormHandle.aspx");
                                uri.Append("method", "image_upload");
                                uri.Append("table", table.TABLE_NAME);
                                uri.Append("field", item.DB_FIELD);
                                uri.Append("item_id", row_pk);

                                mfu.FileUrl = uri.ToString();

                                con = mfu;
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                con = tb;
                            }
                            break;
                        }
                    default:
                        {
                            TextBox tb = new TextBox();
                            con = tb;
                        }
                        break;
                }
            }


            IAttributeAccessor attr = con;

            string headerText = StringUtil.NoBlank(item.DISPLAY, item.F_NAME);


            con.ID = formUi.ID + "_" + item.DB_FIELD;

            con.FieldLabel = headerText;

            con.DataField = item.DB_FIELD;

            con.ReadOnly = item.IS_READONLY;

            if (item.DISPLAY_LEN > 0)
            {
                con.MinWidth = item.DISPLAY_LEN + "px";
            }

            if (item.DISPLAY_HEIGHT > 0)
            {
                con.Height = item.DISPLAY_HEIGHT + "px";
            }

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

                KeyValueString kv = KeyValueString.Parse(li, "=");

                if (kv.Value == null)
                {
                    kv.Value = kv.Key;
                }

                col.Items.Add(kv.Key, kv.Value);
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

                string tableName = ati.table_name.ToUpper();

                LightModelFilter filter = new LightModelFilter(tableName);
                filter.Locks.Add(LockType.NoLock);
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

                        string valueStr = value?.ToString();
                        // (value != null) ? value.ToString() : string.Empty;

                        itemBase.SetAttribute(field, valueStr);
                    }

                    col.Items.Add(itemBase);
                }


                //暂时不支持映射
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
                    }

                    col.MapItems.Add(new MapItem(tCol.DB_FIELD, tCol.EVENT_AFTER_FIELD_ID));
                }

                col.OnMaping = string.Format("biz_mapping(this,'VIEW',{0},'{1}')", ati.view_id, targetField);
            }

        }

    }
}