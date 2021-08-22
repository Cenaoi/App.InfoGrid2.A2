using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using EC5.Utility.Web;
using EasyClick.BizWeb2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using App.InfoGrid2.Bll;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewStepNew3 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

            if (!this.IsPostBack)
            {
                IntiDataUI();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.IsPostBack)
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
            filter.Fields = new string[] {"TABLE_NAME","TABLE_TEXT" };

            LModelList<LModel> models = decipher.GetModelList(filter);


            tCol.Items.Add(new ListItem() { TextEx = "--空--" });

            foreach (LModel model in models)
            {
                tCol.Items.Add((string)model["TABLE_NAME"], (string)model["TABLE_TEXT"]);
            }



            //////////////

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
            Guid uid = WebUtil.QueryGuid("uid");
            string url = string.Format("ShowFieldAll2.aspx?uid={0}", uid);

            EasyClick.Web.Mini.MiniHelper.EvalFormat("SelectField('{0}')",url);

        
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="localFs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetExprName(List<string> localFs, string name)
        {
            if (!localFs.Contains(name))
            {
                return name;
            }

            string exName = string.Empty;

            for (int i = 0; i < 999; i++)
            {
                exName = "Expr" + i;

                if (!localFs.Contains(exName))
                {
                    break;
                }
            }

            return exName;

        }


        /// <summary>
        /// 插入字段
        /// </summary>
        public void InsertField(string ids)
        {
            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            DbDecipher decipher = ModelAction.OpenDecipher();

            int[] idsInt = StringUtil.ToIntList(ids);


            List<string> localFs = TmpViewMgr.GetFieldNames(uid, sessionId);


            

            try
            {

                List<IG2_TABLE_COL> colList = decipher.SelectModelsIn<IG2_TABLE_COL>("IG2_TABLE_COL_ID", idsInt);

                SortedList<int, IG2_TABLE> tabDict = new SortedList<int, IG2_TABLE>();

                IG2_TABLE table;

                foreach (IG2_TABLE_COL col in colList)
                {

                    if (!tabDict.TryGetValue(col.IG2_TABLE_ID,out table))
                    {
                        table = decipher.SelectModelByPk<IG2_TABLE>(col.IG2_TABLE_ID);
                        tabDict.Add(col.IG2_TABLE_ID, table);
                    }


                    IG2_TMP_VIEW_FIELD field = new IG2_TMP_VIEW_FIELD();

                    field.TMP_GUID = uid;
                    field.TMP_SESSION_ID = sessionId;
                    

                    field.TABLE_ID = table.IG2_TABLE_ID;
                    field.TABLE_TEXT = table.DISPLAY;
                    field.TABLE_NAME = table.TABLE_NAME;


                    field.FIELD_TEXT = col.F_NAME;
                    field.FIELD_NAME = col.DB_FIELD;

                    field.FIELD_VISIBLE = true;
                    field.ROW_USER_SEQ = 999999;

                    if (localFs.Contains(field.TABLE_NAME + "_" + field.FIELD_NAME))
                    {
                        field.ALIAS = TmpViewMgr.GetExprName(localFs, field.TABLE_NAME + "_" + field.FIELD_NAME);

                        localFs.Add(field.ALIAS);
                    }


                    decipher.InsertModel(field);

                    this.store2.Add(field);
                }


            }
            catch (Exception ex) 
            {
                log.Error("添加新字段失败.",ex);

                MessageBox.Alert("添加新字段失败!");
            }
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        public void GoBack()
        {
            Guid uid = WebUtil.QueryGuid("uid");

            string url = string.Format("MViewStepNew2.aspx?uid={0}", uid);
            MiniPager.Redirect(url);
        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            Guid uid = WebUtil.QueryGuid("uid");

            string url = string.Format("MViewStepNew4.aspx?uid={0}", uid);
            MiniPager.Redirect(url);
        }


        /// <summary>
        /// 点击完成  
        /// </summary>
        public void GoLast()
        {

            int cataId = WebUtil.QueryInt("catalog_id", 104);

            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;



            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                TmpViewSet tvSet = TmpViewSet.Select(decipher, uid, sessionId);


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


                if (tvSet.View == null)
                {
                    MessageBox.Alert("保存出错！");
                    return;
                }

                ViewSet viewSet = InputData(tvSet);

                MiniPager.Redirect(string.Format("MoreViewPreview.aspx?id={0}", viewSet.View.IG2_VIEW_ID));
            }
            catch (Exception ex)
            {
                log.Error(ex);

                MessageBox.Alert("保存出错！");
            }
        }

        /// <summary>
        /// 根据视图,插入 TABLE 表
        /// </summary>
        /// <param name="viewSet"></param>
        private void InsertLinkTable(ViewSet viewSet)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = viewSet.ToTableSet();



        }


        /// <summary>
        /// 导入数据到真正的表中
        /// </summary>
        public ViewSet InputData(TmpViewSet tvSet)
        {
            int cataId = WebUtil.QueryInt("catalog_id", 104);

            Guid uid = WebUtil.QueryGuid("uid");

            DbDecipher decipher = ModelAction.OpenDecipher();


            ViewSet vSet = tvSet.ToViewSet();

            IG2_VIEW view = vSet.View;

            vSet.View.VIEW_NAME = BizCommon.BillIdentityMgr.NewCodeForNum("USER_VIEW", "UV_", 3);



            TableSet tSet = vSet.ToTableSet();


            IG2_TABLE ig2Table = tSet.Table;

            ig2Table.TABLE_TYPE_ID = "MORE_VIEW";
            ig2Table.TABLE_NAME = view.VIEW_NAME;
            ig2Table.DISPLAY = view.DISPLAY;

            ig2Table.TABLE_UID = Guid.NewGuid();
            ig2Table.IG2_CATALOG_ID = cataId;

            try
            {
                decipher.InsertModel(ig2Table);

                view.IG2_VIEW_ID = ig2Table.IG2_TABLE_ID;

                vSet.Insert(decipher);

                foreach (IG2_TABLE_COL col in tSet.Cols)
                {
                    col.TABLE_NAME = view.VIEW_NAME;
                    col.TABLE_UID = ig2Table.TABLE_UID;
                    col.IG2_TABLE_ID = ig2Table.IG2_TABLE_ID;

                    decipher.InsertModel(col);
                }


                return vSet;
            }
            catch (Exception ex)
            {
                throw new Exception("关联视图,导入到正式表失败.", ex);
            }


        }
    }
}