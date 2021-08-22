using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using EasyClick.BizWeb2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewStepEdit3 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);

        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                IntiDataUI();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.DataBind();
            }

        }


        private void IntiDataUI()
        {

            SelectColumn tCol = this.table3.Columns.FindByDataField("JOIN_TABLE_NAME") as SelectColumn;
            SelectColumn fCol = this.table3.Columns.FindByDataField("JOIN_DB_FIELD") as SelectColumn;


            DbDecipher decipher = ModelAction.OpenDecipher();


            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TMP_VIEW_TABLE));
            filter.And("TMP_GUID", uid);
            filter.And("TMP_SESSION_ID", sessionId);
            filter.Fields = new string[] { "TABLE_NAME", "TABLE_TEXT" };

            LModelList<LModel> models = decipher.GetModelList(filter);


            tCol.Items.Add(new ListItem() { TextEx = "--空--" });

            foreach (LModel model in models)
            {
                tCol.Items.Add((string)model["TABLE_NAME"], (string)model["TABLE_TEXT"]);
            }



            SelectColumn mainTNameCol = this.table1.Columns.FindByDataField("MAIN_TABLE_NAME") as SelectColumn;
            mainTNameCol.Items.Add(new ListItem() { TextEx = "--空--" });

            foreach (LModel model in models)
            {
                mainTNameCol.Items.Add((string)model["TABLE_NAME"], (string)model["TABLE_TEXT"]);
            }

        }

       
        /// <summary>
        /// 这是弹出选择字段窗口
        /// </summary>
        public void SelectField()
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");

            string url = string.Format("ShowFieldAllEdit.aspx?id={0}&uid={1}", id, uid);

            EasyClick.Web.Mini.MiniHelper.EvalFormat("SelectField('{0}')", url);

        }




        


        /// <summary>
        /// 插入字段
        /// </summary>
        public void InsertField(string fieldIdStr)
        {
            HttpContext cont = HttpContext.Current;


            DbDecipher decipher = ModelAction.OpenDecipher();

            string[] fieldIds = StringUtil.Split(fieldIdStr, ",");

            
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            List<string> localFs = TmpViewMgr.GetFieldNames(uid, sessionId);



            try
            {

                List<IG2_TABLE_COL> colList = decipher.SelectModelsIn<IG2_TABLE_COL>("IG2_TABLE_COL_ID", fieldIds);

                SortedList<int, IG2_TABLE> tabDict = new SortedList<int, IG2_TABLE>();

                IG2_TABLE table;


                foreach (IG2_TABLE_COL col in colList)
                {
                    if (!tabDict.TryGetValue(col.IG2_TABLE_ID, out table))
                    {
                        table = decipher.SelectModelByPk<IG2_TABLE>(col.IG2_TABLE_ID);
                        tabDict.Add(col.IG2_TABLE_ID, table);
                    }

                    IG2_TMP_VIEW_FIELD field = new IG2_TMP_VIEW_FIELD();

                    field.IG2_VIEW_ID = id;
                    field.TMP_SESSION_ID = sessionId;
                    field.TMP_GUID = uid;
                    field.TMP_OP_ID = "A";

                    field.TABLE_ID = table.IG2_TABLE_ID;
                    field.TABLE_NAME = table.TABLE_NAME;    // col.TABLE_NAME;
                    field.TABLE_TEXT = table.DISPLAY;

                    field.FIELD_NAME = col.DB_FIELD;
                    field.FIELD_TEXT = col.F_NAME;

                    field.FIELD_VISIBLE = true;
                    field.ROW_USER_SEQ = 999999;

                    if (localFs.Contains(field.TABLE_NAME + "_" + field.FIELD_NAME))
                    {
                        field.ALIAS = TmpViewMgr.GetExprName(localFs, field.TABLE_NAME + "_" + field.FIELD_NAME);

                        localFs.Add(field.ALIAS);
                    }


                    decipher.InsertModel(field);
                }

                this.store2.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("添加字段错误.",ex);

                MessageBox.Alert("添加字段错误.");
            }
        }


        /// <summary>
        /// 返回上一步
        /// </summary>
        public void GoBack()
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");

            string url = string.Format("MViewStepEdit2.aspx?id={0}&uid={1}", id,uid);
            MiniPager.Redirect(url);
        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");

            string url = string.Format("MViewStepEdit4.aspx?id={0}&uid={1}", id,uid);
            MiniPager.Redirect(url);
        }


        


        /// <summary>
        /// 点击完成  
        /// </summary>
        public void GoLast()
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");




            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();


                TmpViewSet tvSet = TmpViewSet.Select(decipher, uid, Session.SessionID);




                #region 处理关联字段名重复的问题

                List<string> exprList = new List<string>();
                List<IG2_TMP_VIEW_FIELD> exprFs = new List<IG2_TMP_VIEW_FIELD>();   //异常的字段，先记录起来

                foreach (IG2_TMP_VIEW_FIELD tmpVField in tvSet.Fields)
                {
                    string field;

                    if (!StringUtil.IsBlank(tmpVField.ALIAS))
                    {
                        field = tmpVField.ALIAS;
                    }
                    else
                    {
                        field = tmpVField.TABLE_NAME + "_" + tmpVField.FIELD_NAME;
                    }

                    if (exprList.Contains(field))
                    {
                        exprFs.Add(tmpVField);
                        continue;
                    }

                    exprList.Add(field);

                }

                foreach (IG2_TMP_VIEW_FIELD tmpVField in exprFs)
                {
                    string field;

                    if (!StringUtil.IsBlank(tmpVField.ALIAS))
                    {
                        field = tmpVField.ALIAS;
                    }
                    else
                    {
                        field = tmpVField.TABLE_NAME + "_" + tmpVField.FIELD_NAME;
                    }

                    if (exprList.Contains(field))
                    {
                        field = TmpViewMgr.GetExprName(exprList, "");

                        tmpVField.ALIAS = field;
                        tmpVField.TMP_OP_ID = "E";

                        exprList.Add(field);
                    }

                }

                #endregion



                IG2_VIEW view = decipher.SelectModelByPk<IG2_VIEW>(id);

                if (view == null)
                {
                    MessageBox.Alert("保存出错！");
                    return;
                }


                UpdataData(tvSet, view);

                tvSet.Delete(decipher,uid, this.Session.SessionID);

                MiniPager.Redirect(string.Format("MoreViewPreview.aspx?id={0}", id));
            }
            catch (Exception ex)
            {
                log.Error("保存出错",ex);

                MessageBox.Alert("保存出错！");
            }
        }

        /// <summary>
        /// 导入数据到真正的表中
        /// </summary>
        public void UpdataData(TmpViewSet tmpViewSet, IG2_VIEW view)
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");

            ViewSet vSet = tmpViewSet.ToViewSet();
            vSet.SetID(view.IG2_VIEW_ID);

            IG2_TMP_VIEW tmpView = tmpViewSet.View;



            LightModelFilter tabFilter = new LightModelFilter(typeof(IG2_VIEW_TABLE));
            tabFilter.And("IG2_VIEW_ID", id);
            tabFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);



            vSet.View.CopyTo(view);




            DbDecipher decipher = ModelAction.OpenDecipher();

            ViewFieldOpSet opSet = GetOpSet(decipher, tmpViewSet.Fields);
            



            try
            {
                TableSet tSet = vSet.ToTableSet();


                //decipher.BeginTransaction();

                decipher.UpdateModel(view);

                //把正式表中的数据删除掉，然后重新插入
                decipher.UpdateProps(tabFilter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });
                decipher.InsertModels<IG2_VIEW_TABLE>(vSet.Tables);

                //执行，更新关联视图表
                opSet.Exec(decipher);

                //更新 "工作表".


                LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
                filter.And("IG2_TABLE_ID", tSet.Table.IG2_TABLE_ID);
                filter.Fields = new string[] { "TABLE_UID" };

                Guid tableUid = decipher.ExecuteScalar<Guid>(filter);

                TableColOpSet tableColOpSet = ConvertTColOSet(opSet,view.IG2_VIEW_ID, view.VIEW_NAME);


                int delCount = 0;

                foreach (IG2_TABLE_COL tmpCol in tableColOpSet.deleteFs)
                {
                    LightModelFilter delFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
                    delFilter.And("TABLE_UID", tableUid);
                    delFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    delFilter.And("DB_FIELD", tmpCol.DB_FIELD);

                    delCount += decipher.UpdateProps(delFilter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });

                }


                foreach (IG2_TABLE_COL tmpCol in tableColOpSet.updateFs)
                {
                    LightModelFilter editFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
                    editFilter.And("TABLE_UID", tableUid);
                    editFilter.And("DB_FIELD", tmpCol.DB_FIELD);
                    editFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                    int dbDot = 0;
                    int dbLen = 0;

                    switch (tmpCol.DB_FIELD.ToLower())
                    {
                        case "currency":
                            dbDot = 2;
                            dbLen = 18;
                            break;
                        case "decimal":
                            dbDot = 6;
                            dbLen = 18;
                            break;
                        case "string":
                            dbDot = 0;
                            dbLen = 50;
                            break;
                        case "boolean":
                            dbDot = 0;
                            dbLen = 0;
                            break;
                    }

                    decipher.UpdateProps(editFilter, new object[] { 
                        "F_NAME", StringUtil.NoBlank( tmpCol.F_NAME,""),
                        "DB_TYPE", StringUtil.NoBlank( tmpCol.DB_TYPE,"") ,
                        "DB_DOT", dbDot,
                        "DB_LEN", dbLen
                    });

                }


                if (tableColOpSet.insertFs.Count > 0)
                {
                    LightModelFilter newTableFilter = new LightModelFilter(typeof(IG2_TABLE));
                    newTableFilter.And("TABLE_UID", tableUid);
                    newTableFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    newTableFilter.Fields = new string[] { "IG2_TABLE_ID", "TABLE_UID" };

                    LModelList<LModel> tables = decipher.GetModelList(newTableFilter);



                    foreach (LModel tab in tables)
                    {
                        Guid tUid = tab.Get<Guid>("TABLE_UID");
                        int tabId = tab.Get<int>("IG2_TABLE_ID");

                        foreach (IG2_TABLE_COL tmpCol in tableColOpSet.insertFs)
                        {
                            IG2_TABLE_COL tCol = new IG2_TABLE_COL();
                            tmpCol.CopyTo(tCol, true);
                            tCol.IG2_TABLE_ID = tabId;
                            tCol.TABLE_UID = tUid;
                            tCol.TABLE_NAME = tmpCol.TABLE_NAME;


                            decipher.InsertModel(tCol);
                        }
                    }
                }


                //decipher.TransactionCommit();





            }
            catch (Exception ex)
            {
                //decipher.TransactionRollback();

                log.Error("IG2_VIEW 临时表导入正式表失败。", ex);

                throw new Exception("IG2_VIEW 临时表导入正式表失败。", ex); ;

            }

        }





        private ViewFieldOpSet GetOpSet(DbDecipher decipher, List<IG2_TMP_VIEW_FIELD> tmpViewFields)
        {
            ViewFieldOpSet opSet = new ViewFieldOpSet();

            foreach (IG2_TMP_VIEW_FIELD tmpVField in tmpViewFields)
            {
                int vFieldId = tmpVField.IG2_VIEW_FIELD_ID;

                //这条已经被删除的记录,没有在正式表存在过.
                if (vFieldId == 0 && tmpVField.ROW_SID == -3)
                {
                    continue;
                }

                if (vFieldId == 0)
                {
                    IG2_VIEW_FIELD vField = new IG2_VIEW_FIELD();
                    tmpVField.CopyTo(vField, true);

                    opSet.insertFs.Add(vField);
                }
                else if (vFieldId > 0 && tmpVField.ROW_SID == -3)
                {
                    IG2_VIEW_FIELD vField = decipher.SelectModelByPk<IG2_VIEW_FIELD>(vFieldId);

                    opSet.deleteFs.Add(vField);

                    //decipher.UpdateProps<IG2_VIEW_FIELD>(string.Format("IG2_VIEW_FIELD={0}", tmpVField.IG2_VIEW_FIELD_ID),
                    //    new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });


                }
                else if (vFieldId > 0 && tmpVField.TMP_OP_ID == "E")
                {
                    IG2_VIEW_FIELD vField = new IG2_VIEW_FIELD();
                    tmpVField.CopyTo(vField, true);

                    opSet.updateFs.Add(vField);
                }

            }

            return opSet;
        }

        private TableColOpSet ConvertTColOSet(ViewFieldOpSet viewFieldOpSet,int tableId, string tableName)
        {
            TableColOpSet tcOpSet = new TableColOpSet();

            foreach (IG2_VIEW_FIELD vField in viewFieldOpSet.deleteFs)
            {
                IG2_TABLE_COL col = new IG2_TABLE_COL();
                col.IG2_TABLE_ID = vField.TABLE_ID;
                col.DB_FIELD =  StringUtil.NoBlank(vField.ALIAS, vField.TABLE_NAME + "_" + vField.FIELD_NAME) ;
                col.DISPLAY = vField.FIELD_TEXT;

                col.TABLE_NAME = tableName;
                col.IG2_TABLE_ID = tableId;

                tcOpSet.deleteFs.Add(col);
            }


            foreach (IG2_VIEW_FIELD vField in viewFieldOpSet.insertFs)
            {
                IG2_TABLE_COL col = new IG2_TABLE_COL();
                col.IG2_TABLE_ID = vField.TABLE_ID;

                col.VIEW_FIELD_SRC = string.Concat( vField.TABLE_NAME , "." , vField.FIELD_NAME);
                col.DB_FIELD = StringUtil.NoBlank(vField.ALIAS, vField.TABLE_NAME + "_" + vField.FIELD_NAME);

                col.F_NAME = vField.FIELD_TEXT;
                col.DISPLAY = vField.FIELD_TEXT;

                col.TABLE_NAME = tableName;
                col.IG2_TABLE_ID = tableId;

                col.IS_VISIBLE = true;
                col.IS_LIST_VISIBLE = true;
                col.IS_SEARCH_VISIBLE = true;

                tcOpSet.insertFs.Add(col);
            }


            foreach (IG2_VIEW_FIELD vField in viewFieldOpSet.updateFs)
            {
                IG2_TABLE_COL col = new IG2_TABLE_COL();
                col.IG2_TABLE_ID = vField.TABLE_ID;
                col.DB_FIELD = StringUtil.NoBlank(vField.ALIAS, string.Concat(vField.TABLE_NAME, "_", vField.FIELD_NAME));
                col.DISPLAY = vField.FIELD_TEXT;

                col.TABLE_NAME = tableName;
                col.IG2_TABLE_ID = tableId;


                tcOpSet.updateFs.Add(col);
            }


            return tcOpSet;
        }

        /// <summary>
        /// 修复字段按钮事件  小渔夫添加的 
        /// </summary>
        public void RepairField()
        {


            int id = WebUtil.QueryInt("id");

          


            DbDecipher decipher = ModelAction.OpenDecipher();


            IG2_VIEW iView = decipher.SelectModelByPk<IG2_VIEW>(id);

            if (iView == null)
            {
                MessageBox.Alert("好像出错了喔！");
                return;
            }


        


            List<IG2_VIEW_FIELD> fieldList = decipher.SelectModels<IG2_VIEW_FIELD>("IG2_VIEW_ID = {0} and ROW_SID >=0",id);

            List<IG2_TABLE_COL> colInsertList = new List<IG2_TABLE_COL>();


            foreach (var item in fieldList)
            {

                IG2_TABLE_COL col = new IG2_TABLE_COL();
                col.IG2_TABLE_ID = item.TABLE_ID;

                col.VIEW_FIELD_SRC = string.Concat(item.TABLE_NAME, ".", item.FIELD_NAME);
                col.DB_FIELD = StringUtil.NoBlank(item.ALIAS, item.TABLE_NAME + "_" + item.FIELD_NAME);

                col.F_NAME = item.FIELD_TEXT;
                col.DISPLAY = item.FIELD_TEXT;

                col.TABLE_NAME = iView.VIEW_NAME;
                col.IG2_TABLE_ID = id;
                col.IS_VISIBLE = true;
                col.IS_LIST_VISIBLE = true;
                col.IS_SEARCH_VISIBLE = true;


                colInsertList.Add(col);


            }

            string sql = string.Format("UPDATE IG2_TABLE_COL SET ROW_SID = -3,ROW_DATE_DELETE = '{0}' WHERE ROW_SID >=0 and IG2_TABLE_ID = {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);



            try
            {


                decipher.BeginTransaction();

                decipher.ExecuteNonQuery(sql);

                decipher.InsertModels<IG2_TABLE_COL>(colInsertList);

                decipher.TransactionCommit();

                MessageBox.Alert("修复成功了！");


            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("修复字段出错了！", ex);

                MessageBox.Alert("修复失败了！");

            }

           
        }




        class TableColOpSet
        {
            /// <summary>
            /// 新建的记录
            /// </summary>
            public List<IG2_TABLE_COL> insertFs = new List<IG2_TABLE_COL>();


            /// <summary>
            /// 更新记录
            /// </summary>
            public List<IG2_TABLE_COL> updateFs = new List<IG2_TABLE_COL>();

            /// <summary>
            /// 删除记录
            /// </summary>
            public List<IG2_TABLE_COL> deleteFs = new List<IG2_TABLE_COL>();



        }

        class ViewFieldOpSet
        {

            /// <summary>
            /// 新建的记录
            /// </summary>
            public List<IG2_VIEW_FIELD> insertFs = new List<IG2_VIEW_FIELD>();

            /// <summary>
            /// 更新记录
            /// </summary>
            public List<IG2_VIEW_FIELD> updateFs = new List<IG2_VIEW_FIELD>();

            /// <summary>
            /// 删除记录
            /// </summary>
            public List<IG2_VIEW_FIELD> deleteFs = new List<IG2_VIEW_FIELD>();


            /// <summary>
            /// 执行上面3个集合的操作
            /// </summary>
            /// <param name="decipher"></param>
            public void Exec(DbDecipher decipher)
            {
                foreach (IG2_VIEW_FIELD field in insertFs)
                {
                    decipher.InsertModel(field);
                }

                foreach (IG2_VIEW_FIELD field in updateFs)
                {
                    decipher.UpdateModel(field);
                }

                foreach (IG2_VIEW_FIELD field in deleteFs)
                {
                    field.ROW_SID = -3;
                    field.ROW_DATE_DELETE = DateTime.Now;

                    decipher.UpdateModelProps(field, "ROW_SID", "ROW_DATE_DELETE");
                }
            }

        }



    }
}