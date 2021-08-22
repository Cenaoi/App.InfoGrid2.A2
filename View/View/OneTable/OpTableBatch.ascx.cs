using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.AppDomainPlugin;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using HWQ.Entity.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace App.InfoGrid2.View.OneTable
{
    public partial class OpTableBatch : WidgetControl, IView
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
                this.storeMain.DataBind();
                this.storeNewCol.DataBind();
                this.storeTable.DataBind();
            }
        }

        public void GoTableSelect()
        {
            Window win = new Window("选择表");
            win.ContentPath = "/App/InfoGrid2/View/OneTable/OnTableSelect.aspx";

            win.StartPosition = WindowStartPosition.CenterScreen;
            win.State = WindowState.Max;
            win.WindowClosed += Win_WindowClosed;
            win.ShowDialog();
        }

        public void Win_WindowClosed(object sender, string data)
        {
            SModel json = SModel.ParseJson(data);

            DynSModel ds = new DynSModel(json);

            if(ds["result"] != "ok")
            {
                return;
            }

            SModelList dataList = ds["data"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("id");

            List<IG2_BATCH_TO_TABLE> newBtts = new List<IG2_BATCH_TO_TABLE>();

            foreach (SModel sm in dataList)
            {
                int tableId = Convert.ToInt32(sm["IG2_TABLE_ID"]);
                string table = (string)sm["TABLE_NAME"];
                string display = (string)sm["TABLE_DISPLAY"];

                LightModelFilter filter = new LightModelFilter(typeof(IG2_BATCH_TO_TABLE));
                filter.AddFilter("ROW_SID >= 0");
                filter.And("BATCH_TABLE_OPERATE_ID", id);

                filter.And("TABLE_NAME", table);

                if (decipher.ExistsModels(filter))
                {
                    continue;
                }


                IG2_BATCH_TO_TABLE btt = new IG2_BATCH_TO_TABLE();
                btt.BATCH_TABLE_OPERATE_ID = id;
                btt.TABLE_ID = tableId;
                btt.TABLE_NAME = table;
                btt.TABLE_DISPLAY = display;

                newBtts.Add(btt);

            }

            decipher.InsertModels(newBtts);

            this.storeTable.AddRange(newBtts);

        }


        /// <summary>
        /// 监测字段
        /// </summary>
        public void GoCheckField()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            var btCols = decipher.SelectModels<IG2_BATCH_TO_COL>($"ROW_SID >= 0 and BATCH_TABLE_OPERATE_ID={id}");


            int[] ids = this.tableTables.CheckedRows.GetIds<int>();

            LightModelFilter btFilter = new LightModelFilter(typeof(IG2_BATCH_TO_TABLE));
            btFilter.AddFilter("ROW_SID >= 0");
            btFilter.And("IG2_BATCH_TO_TABLE_ID", ids, Logic.In);

            var tables = decipher.SelectModels<IG2_BATCH_TO_TABLE>(btFilter);
            

            //var btColsDict = btCols.ToSortedList<string>("DB_FIELD");

            foreach (var tab in tables)
            {
                var tableCols = decipher.SelectModels<IG2_TABLE_COL>($"ROW_SID >= 0 and IG2_TABLE_ID={tab.TABLE_ID}");

                var tableColDict = tableCols.ToSortedList<string>("DB_FIELD");


                List<string> queFields = new List<string>();    //缺少的字段


                foreach (var btCol in btCols)
                {
                    if (!tableColDict.ContainsKey(btCol.DB_FIELD))
                    {
                        queFields.Add(btCol.DB_FIELD);
                    }
                }


                StringBuilder sb;

                if (queFields.Count > 0)
                {
                    sb = new StringBuilder($"缺少{queFields.Count}个字段:");

                    int n = 0;
                    foreach (var queField in queFields)
                    {
                        if (n++ > 0) { sb.Append("; "); }
                        sb.Append(queField);
                    }
                }
                else
                {
                    sb = new StringBuilder("没有缺");
                }


                tab.STATE_TEXT = sb.ToString();

                decipher.UpdateModelProps(tab, "STATE_TEXT");

            }

            Toast.Show("监测完成, 点击刷新!");

        }

        /// <summary>
        /// 批量添加
        /// </summary>
        public void GoBatchAdd()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            DatabaseBuilder db = decipher.DatabaseBuilder;

            var btCols = decipher.SelectModels<IG2_BATCH_TO_COL>($"ROW_SID >= 0 and BATCH_TABLE_OPERATE_ID={id}");


            int[] ids = this.tableTables.CheckedRows.GetIds<int>();

            LightModelFilter btFilter = new LightModelFilter(typeof(IG2_BATCH_TO_TABLE));
            btFilter.AddFilter("ROW_SID >= 0");
            btFilter.And("IG2_BATCH_TO_TABLE_ID", ids, Logic.In);

            var tables = decipher.SelectModels<IG2_BATCH_TO_TABLE>(btFilter);

            var btColsDict = btCols.ToSortedList<string>("DB_FIELD");

            foreach (var tab in tables)
            {
                IG2_TABLE srcTable = decipher.SelectModelByPk<IG2_TABLE>(tab.TABLE_ID);

                var tableCols = decipher.SelectModels<IG2_TABLE_COL>($"ROW_SID >= 0 and IG2_TABLE_ID={tab.TABLE_ID}");
                var tableColDict = tableCols.ToSortedList<string>("DB_FIELD");

                List<string> queFields = new List<string>();    //缺少的字段


                foreach (var btCol in btCols)
                {
                    if (!tableColDict.ContainsKey(btCol.DB_FIELD))
                    {
                        queFields.Add(btCol.DB_FIELD);
                    }
                }

                /*******************
                 * 操作
                 * 1.创建临时表;
                 * 2.拷贝数据;
                 * 3.改变单表结构
                 * 4.改变视图表结构;
                 *******************/

                //创建新添加的字段
                List<IG2_TABLE_COL> newCols =  Pre1_AddFields(btColsDict, queFields,tab);

                if(newCols.Count == 0)
                {
                    continue;
                }

                //临时表的字段
                List<IG2_TABLE_COL> tmpCols = new List<IG2_TABLE_COL>(tableCols);
                tmpCols.AddRange(newCols);

                foreach (var item in tableCols)
                {
                    if (item.DB_TYPE == "decimal")
                    {
                        item.DB_LEN = 18;
                    }
                }


                string tmpTableName;
                int rowCount;
                DataTable srcDataList;

                XmlModelElem tmpModelElem = null;

                //创建临时表
                tmpTableName = CreateTempTable(srcTable, tmpCols,out tmpModelElem);

                LModelDna.BeginEdit();
                LModelElement newModelElem = ModelConvert.ToModelElem(tmpModelElem);
                LModelDna.EndEdit();

                //srcDataList = decipher.GetDataTable($"select * from {tab.TABLE_NAME}");

                LModelList<LModel> srcModels = decipher.GetModelList(tab.TABLE_NAME, string.Empty);


                LModelList<LModel> newModels = new LModelList<LModel>(newModelElem);

                foreach (var m in srcModels)
                {
                    LModel newM = new LModel(newModelElem);

                    m.CopyTo(newM, true);

                    newModels.Add(newM);
                }


                //DataColumnCollection cols = srcDataList.Columns;

                ////检查一遍数据,并重新赋值
                //foreach (DataRow row in srcDataList.Rows)
                //{
                //    foreach (XmlFieldElem xField in tmpModelElem.Fields)
                //    {
                //        if (!cols.Contains(xField.DBField))
                //        {
                //            continue;
                //        }
                        
                //        object srcV = row[xField.DBField];

                //        if (!DBNull.Value.Equals(srcV))
                //        {
                //            continue;
                //        }

                //        if (!xField.Mandatory)
                //        {
                //            continue;
                //        }


                //        srcV = null;
                        
                //        srcV = ModelConvert.ChangeType(srcV, xField.DBType, true);

                //        row[xField.DBField] = srcV;

                //    }
                //}

                try
                {
                    DataTable dt = newModels.ToDataTable();

                    rowCount = decipher.InsertTable(tmpTableName, dt, DbDecipherInsertType.Batch);

                    //decipher.InsertModels((IList<LModel>)newModels, DbDecipherInsertType.Batch);
                }
                catch(Exception ex)
                {
                    throw new Exception("拷贝数据错误," + tab.TABLE_NAME, ex);
                }


                LightModelFilter filterTab = new LightModelFilter(typeof(IG2_TABLE));
                filterTab.AddFilter("ROW_SID >= 0");
                filterTab.And("TABLE_UID", srcTable.TABLE_UID);

                LModelList<IG2_TABLE> srcTables = decipher.SelectModels<IG2_TABLE>(filterTab);

                decipher.BeginTransaction();

                try
                {
                    foreach (IG2_TABLE srcTab2 in srcTables)
                    {
                        foreach (var newCol in newCols)
                        {
                            newCol.IG2_TABLE_ID = srcTab2.IG2_TABLE_ID;
                            newCol.TABLE_UID = srcTab2.TABLE_UID;

                            newCol.TABLE_NAME = srcTab2.TABLE_NAME;
                            newCol.IS_VISIBLE = false;

                            decipher.InsertModel(newCol);
                        }
                    }

                    db.DropTable(tab.TABLE_NAME);
                    db.RenameTable(tmpTableName, tab.TABLE_NAME);

                    decipher.TransactionCommit();

                    Bll.TableBufferMgr.Remove(srcTable.IG2_TABLE_ID);
                    EC5.IG2.Core.UI.M2VFieldHelper.VModels.Remove(srcTable.IG2_TABLE_ID);
                }
                catch(Exception ex)
                {
                    decipher.TransactionRollback();

                    throw new Exception("增加新字段错误." + srcTable.TABLE_NAME, ex);
                }



            }

            Toast.Show("更新完成!");
        }


        /// <summary>
        /// 创建临时表
        /// </summary>
        /// <param name="tmpTab"></param>
        /// <param name="tmpCols"></param>
        private string CreateTempTable(IG2_TABLE tmpTab, List<IG2_TABLE_COL> tmpCols, out XmlModelElem xModelElememt)
        {
            string tmpTableName = tmpTab.TABLE_NAME + "_tmp";

            XmlModelElem xModelElem = new XmlModelElem(tmpTableName);



            foreach (var tmpCol in tmpCols)
            {
                XmlFieldElem xFieldElem = new XmlFieldElem(tmpCol.DB_FIELD);
                xFieldElem.Caption = xFieldElem.Description = tmpCol.DISPLAY;
                xFieldElem.DBType = ModelConvert.ToDbType(tmpCol.DB_TYPE);

                if (xFieldElem.DBType == LMFieldDBTypes.Decimal)
                {
                    xFieldElem.MaxLen = 18;
                }
                else
                {
                    xFieldElem.MaxLen = tmpCol.DB_LEN;
                }

                xFieldElem.DecimalDigits = tmpCol.DB_DOT;
                xFieldElem.Mandatory = tmpCol.IS_MANDATORY;
                xFieldElem.IsRemark = tmpCol.IS_REMARK;

                xFieldElem.DefaultValue = tmpCol.DEFAULT_VALUE;

                xModelElem.Fields.Add(xFieldElem);

                if(tmpCol.DB_FIELD == tmpTab.ID_FIELD)
                {
                    xFieldElem.IsKey = true;
                }
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            DatabaseBuilder db = decipher.DatabaseBuilder;

            //如果临时表存在,就直接删除
            if (db.ExistTable(tmpTableName))
            {
                db.DropTable(tmpTableName);
            }


            LModelDna.BeginEdit();

            db.CreateTable(xModelElem);

            LModelDna.EndEdit();

            xModelElememt = xModelElem;

            return tmpTableName;
        }


        private List<IG2_TABLE_COL> Pre1_AddFields(SortedList<string, IG2_BATCH_TO_COL> btCols, List<string> queFields,IG2_BATCH_TO_TABLE btTab)
        {
            List<IG2_TABLE_COL> newCols = new List<IG2_TABLE_COL>();

            foreach (var queField in queFields)
            {
                IG2_BATCH_TO_COL btCol = btCols[queField];


                IG2_TABLE_COL col = new IG2_TABLE_COL();

                col.DB_FIELD = btCol.DB_FIELD;
                col.DB_TYPE = btCol.DB_TYPE;
                col.DISPLAY = col.F_NAME = btCol.F_NAME;
                col.DB_LEN = btCol.DB_LEN;
                col.DB_DOT = btCol.DB_DOT;

                col.SEC_LEVEL = btCol.SEC_LEVEL;

                col.IS_MANDATORY = btCol.IS_MANDATORY;

                col.IS_VISIBLE = false;

                newCols.Add(col);
            }
            

            return newCols;
        }



    }
}