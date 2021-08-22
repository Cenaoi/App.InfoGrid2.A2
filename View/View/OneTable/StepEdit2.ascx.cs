using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using App.InfoGrid2.Bll;
using HWQ.Entity.Xml;
using App.InfoGrid2.Model.DataSet;


namespace App.InfoGrid2.View.OneTable
{
    public partial class StepEdit2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.Updating += new ObjectCancelEventHandler(store1_Updating);


            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        void store1_Updating(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            model["DB_FIELD"] = model["DB_FIELD"].ToString().Trim();

            string dbType = model.Get<string>("DB_TYPE");

            int dbLen = model.Get<int>("DB_LEN");

            switch (dbType)
            {
                case "currency":
                    model["DB_DOT"] = 2;
                    model["DB_LEN"] = 18;
                    break;
                case "decimal":
                    model["DB_DOT"] = 6;
                    model["DB_LEN"] = 18;
                    break;
                case "string":

                    if (dbLen == 0)
                    {
                        model["DB_LEN"] = 50;
                    }

                    model["DB_DOT"] = 0;
                    break;
                case "boolean":
                    model["DB_LEN"] = 0;
                    model["DB_LEN"] = 0;
                    break;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void GoCancel()
        {
            int id = WebUtil.QueryInt("id");
            Guid tmp_id = WebUtil.QueryGuid("tmp_id");

            string sessionId = this.Session.SessionID;


            LightModelFilter tabFilter = new LightModelFilter(typeof(IG2_TMP_TABLE));
            tabFilter.And("TMP_GUID", tmp_id);
            tabFilter.And("TMP_SESSION_ID", sessionId);

            LightModelFilter colFilter = new LightModelFilter(typeof(IG2_TMP_TABLECOL));
            colFilter.And("TMP_GUID", tmp_id);
            colFilter.And("TMP_SESSION_ID", sessionId);


            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.DeleteModels(tabFilter);
            decipher.DeleteModels(colFilter);


            MiniPager.Redirect("TablePreview.aspx?id=" + id);
        }

        /// <summary>
        /// 跳转到最后
        /// </summary>
        public void GoLast()
        {
            int id = WebUtil.QueryInt("id");
            Guid tmp_id = WebUtil.QueryGuid("tmp_id");

            TmpTableMgr.TempTable2Table_Edit(tmp_id);


            MiniPager.Redirect("TablePreview.aspx?id=" + id);
        }


        /// <summary>
        /// 同步数据库的实际字符串长度
        /// </summary>
        public void GoSyncDBLen()
        {
            int id = WebUtil.QueryInt("id");
            Guid tmp_id = WebUtil.QueryGuid("tmp_id");


            DbDecipher decipher = ModelAction.OpenDecipher();
            
            TableSet tSet = TableSet.Select(decipher, id);

            TmpTableSet tmpTSet = TmpTableSet.Select(decipher, tmp_id);


            string tableName = tSet.Table.TABLE_NAME;   // TableMgr.GetTableNameForId(id);


            DatabaseBuilder db = decipher.DatabaseBuilder;

            XmlModelElem xModelElem = db.GetModelElemByTable(tableName);

            List<IG2_TABLE_COL> cols = new List<IG2_TABLE_COL>();   //长度发生变化的字段
            
            int count = 0;

            log.DebugFormat("准备同步数据表“{0}”的字段长度。", tableName);

            foreach (IG2_TABLE_COL tCol in tSet.Cols)
            {
                //if (tCol.DB_TYPE != "string")
                //{
                //    continue;
                //}

                XmlFieldElem xFieldElem = xModelElem.Fields[tCol.DB_FIELD];

                if (tCol.DB_LEN == xFieldElem.MaxLen)
                {
                    continue;
                }

                log.DebugFormat("调整字段“{0}:{1}”长度 {2} 改为 {3}", tCol.DB_FIELD , tCol.F_NAME, tCol.DB_LEN, xFieldElem.MaxLen);

                tCol.DB_LEN = xFieldElem.MaxLen;

                cols.Add(tCol);

                count++;
            }


            foreach (IG2_TMP_TABLECOL tCol in tmpTSet.Cols)
            {
                //if (tCol.DB_TYPE != "string")
                //{
                //    continue;
                //}

                XmlFieldElem xFieldElem = xModelElem.Fields[tCol.DB_FIELD];

                if (tCol.DB_LEN == xFieldElem.MaxLen)
                {
                    continue;
                }

                tCol.DB_LEN = xFieldElem.MaxLen;

                decipher.UpdateModelProps(tCol, "DB_LEN");

                count++;
            }


            try
            {
                //只同步工作表，视图表其他表的，没有同步
                foreach (IG2_TABLE_COL tCol in cols)
                {
                    decipher.UpdateModelProps(tCol, "DB_LEN");
                }

                log.DebugFormat("共调整 {0} 个字段。", count);

                Toast.Show($"共调整 {count} 个字段。");

                this.store1.Refresh();
            }
            catch (Exception ex)
            {
                log.Error("同步数据表字段长度失败", ex);

                MessageBox.Alert("同步数据表字段长度失败");
            }

        }
    }
}