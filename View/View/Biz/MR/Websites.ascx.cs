using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace App.InfoGrid2.View.Biz.MR
{
    /// <summary>
    /// 网站列表
    /// </summary>
    public partial class Websites : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            this.store1.PageLoading += Store1_PageLoading;
            store1.CurrentChanged += Store1_CurrentChanged;
            store1.Inserting += Store1_Inserting;
            store1.Updating += Store1_Updating;
            store1.BatchDeleting += Store1_BatchDeleting;

            store2.PageLoading += Store2_PageLoading;
            store2.Updating += Store2_Updating;

            store2.BatchDeleting += Store2_BatchDeleting;


        }

        private void Store1_BatchDeleting(object sender, ObjectListCancelEventArgs e)
        {

            var drc = table1.CheckedRows;

            int[] ids = drc.GetIds<int>();

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            foreach (int id in ids)
            {
                DeleteWebsite(id);
            }

            store1.DataBind();

            Toast.Show("删除成功了！");

        }

        private void Store2_BatchDeleting(object sender, ObjectListCancelEventArgs e)
        {
            var drc = table2.CheckedRows;

            int[] ids = drc.GetIds<int>();


            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            foreach (int id in ids)
            {

                SModel sm = decipher.GetSModel("select * from MR_WEBSITE_MAP where ROW_SID >= 0 and MR_WEBSITE_MAP_ID =" + id);

                DeleteWebsiteMap(sm,decipher);


            }

            store2.DataBind();

            Toast.Show("删除成功了！");
        }


        private void Store1_Updating(object sender, ObjectCancelEventArgs e)
        {
            e.Cancel = true;

            SModel lm = new SModel("MR_WEBSITE");

            List<string> cols = new List<string>()
            {
                "WEBSITE_TEXT","IS_STOP","REG_DATE","END_DATE","WS_USER_TEL","WS_USER_TEXT","WS_USER_QQ","WS_USER_ADDRESS","REMARKS","ROOT_CATALOG","BIZ_SID"
            };

            int id = 0;

            foreach (var item in e.SrcRecord.Fields)
            {
                if (item.Name == "MR_WEBSITE_ID")
                {
                    id = Convert.ToInt32(item.Value);
                }

                if (cols.IndexOf(item.Name) < 0)
                {
                    continue;
                }

                lm[item.Name] = item.Value;

            }


            int biz_sid = lm.Get<int>("BIZ_SID");

            if (biz_sid == 999)
            {

                lm.Remove("ROOT_CATALOG");

            }




            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            UpdataDomainIsStop(lm, id, decipher);


            decipher.UpdateSModel(lm, "dbo.MR_WEBSITE", "MR_WEBSITE_ID = " + id);

            store1.DataBind();


        }

        private void Store2_Updating(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            e.Cancel = true;

            SModel lm = new SModel("MR_WEBSITE_MAP");

            List<string> cols = new List<string>()
            {
                "DOMAIN_TEXT","DB_CATALOG","USER_CODE"
            };

            string id = string.Empty;

            foreach (var item in e.SrcRecord.Fields)
            {
                if (item.Name == "MR_WEBSITE_MAP_ID")
                {
                    id = item.Value;
                }

                if (cols.IndexOf(item.Name) < 0)
                {
                    continue;
                }

                lm[item.Name] = item.Value;

            }

            string domain_text = lm.Get<string>("DOMAIN_TEXT");

            bool flag = CheckOnly(domain_text, id);

            if (!flag)
            {
                MessageBox.Alert($"这个域名【{domain_text?.Trim()}】已经存在了，请联系系统管理员！");
                return;
            }


            string db_catalog = lm.Get<string>("DB_CATALOG");

            string base_path = GlobelParam.GetValue<string>("BASE_PATH", @"D:\用户网站", "用户存在数据基本目录");


            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");


            //数据库目录不为空时执行的操作
            if (!string.IsNullOrWhiteSpace(db_catalog))
            {

                lm["DB_PATH"] = base_path + "\\" + lm.Get<string>("USER_CODE") + "\\" + db_catalog;

            }


            decipher.UpdateSModel(lm, "MR_WEBSITE_MAP", "MR_WEBSITE_MAP_ID = " + id);

            store2.DataBind();


        }

        private void Store1_Inserting(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {

            e.Cancel = true;


            EcUserState user = EcContext.Current.User;

            string agent_code = user.ExpandPropertys["AGENT_CODE"];

            SModel mr_ws = new SModel("MR_WEBSITE");
            mr_ws["USER_CODE"] = agent_code;
            mr_ws["USER_TEXT"] = user.LoginName;
            mr_ws["ROW_SID"] = 0;
            mr_ws["ROW_DATE_CREATE"] = mr_ws["ROW_DATE_UPDATE"] = DateTime.Now;

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            decipher.InsertSModel(mr_ws);

            store1.DataBind();

        }

        private void Store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {

            store2.DataBind();

        }

        private void Store2_PageLoading(object sender, EasyClick.Web.Mini2.CancelPageEventArags e)
        {
            e.Cancel = true;

            string main_id = store1.CurDataId;

            if (string.IsNullOrWhiteSpace(main_id))
            {
                return;
            }



            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            LightModelFilter filter = new LightModelFilter("MR_WEBSITE_MAP");
            filter.TSqlWhere = $"ROW_SID >=0  and MR_WEBSITE_ID = {main_id}";

            string tsqlWhere = GetTqlWhereBySearch(SearchFormLayout1);

            if (!string.IsNullOrWhiteSpace(tsqlWhere))
            {
                filter.TSqlWhere += "and " + tsqlWhere;
            }

            filter.Limit = Limit.ByPageIndex((store1.PageSize > 200 ? 20 : store1.PageSize), e.Page);
            filter.TSqlOrderBy = e.TSqlSort;

            if (string.IsNullOrWhiteSpace(e.TSqlSort))
            {
                filter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            }
            string tSql = GetTSql(filter, store1.SortText);

            string tSqlCount = GetTSqlCount(filter);

            int count = decipher.ExecuteScalar<int>(tSqlCount);

            LModelList<LModel> dataModels = decipher.GetModelList(tSql);


            store2.RemoveAll();

            store2.BeginLoadData();
            {
                store2.AddRange(dataModels);

                store2.SetTotalCount(count);
                store2.SetCurrentPage(e.Page);
            }
            store2.EndLoadData();

        }

        private void Store1_PageLoading(object sender, EasyClick.Web.Mini2.CancelPageEventArags e)
        {

            e.Cancel = true;

            EcUserState user = EcContext.Current.User;

            string agent_code = user.ExpandPropertys["AGENT_CODE"];

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            LightModelFilter filter = new LightModelFilter("MR_WEBSITE");
            filter.TSqlWhere = $"ROW_SID >= 0 ";
            //设计师不用过滤数据
            if (!user.Roles.Exist(IG2Param.Role.BUILDER))
            {
                filter.TSqlWhere += $" and USER_CODE = '{agent_code}'";
            }

            string tsqlWhere = GetTqlWhereBySearch(searchForm);

            if (!string.IsNullOrWhiteSpace(tsqlWhere))
            {
                filter.TSqlWhere += "and " + tsqlWhere;
            }

            filter.Limit = Limit.ByPageIndex((store1.PageSize > 200 ? 20 : store1.PageSize), e.Page);
            filter.TSqlOrderBy = e.TSqlSort;

            if (string.IsNullOrWhiteSpace(e.TSqlSort))
            {
                filter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            }
            string tSql = GetTSql(filter, store1.SortText);

            string tSqlCount = GetTSqlCount(filter);

            int count = decipher.ExecuteScalar<int>(tSqlCount);

            LModelList<LModel> dataModels = decipher.GetModelList(tSql);


            store1.RemoveAll();

            store1.BeginLoadData();
            {
                store1.AddRange(dataModels);

                store1.SetTotalCount(count);
                store1.SetCurrentPage(e.Page);
            }
            store1.EndLoadData();


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.store1.DataBind();
            }
        }


        /// <summary>
        /// 获取sql语句
        /// </summary>
        /// <param name="filter">过滤对象</param>
        /// <param name="sortText">数据仓库的排序文本</param>
        /// <returns></returns>
        string GetTSql(LightModelFilter filter, string sortText)
        {

            Limit limit = filter.Limit;
            string tSqlForm = filter.ModelName;
            string tSqlWhere = filter.TSqlWhere;
            string tSqlOrderBy = string.Empty;
            string tSqlSelect = "*";



            if (!string.IsNullOrEmpty(filter.TSqlOrderBy))
            {
                tSqlOrderBy = filter.TSqlOrderBy;
            }

            if (!string.IsNullOrEmpty(sortText))
            {
                if (!string.IsNullOrWhiteSpace(tSqlOrderBy))
                {
                    tSqlOrderBy += ",";
                }

                tSqlOrderBy += sortText;
            }

            StringBuilder tSql = new StringBuilder();

            tSql.AppendLine("SELECT * FROM ( ");
            {
                tSql.Append("  SELECT ").Append(tSqlSelect);

                tSql.AppendFormat(",row_number() over(order by {0}) as PAGE_ROW_NUMBER ", tSqlOrderBy);

                tSql.AppendFormat("\n  FROM {0} ", tSqlForm);

                if (!string.IsNullOrEmpty(tSqlWhere))
                {
                    tSql.AppendFormat("\n  WHERE {0} ", tSqlWhere);
                }
            }

            tSql.Append("\n) as T ");
            tSql.AppendFormat("WHERE (PAGE_ROW_NUMBER between {0} and {1})",
                limit.StartRowIndex + 1, limit.EndRowIndex + 1);



            return tSql.ToString();



        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="filter">过滤对象</param>
        /// <returns></returns>
        string GetTSqlCount(LightModelFilter filter)
        {
            string tSqlForm = filter.ModelName;
            string tSqlWhere = filter.TSqlWhere;
            string tSqlSelect = "Count(MR_WEBSITE_ID)";

            StringBuilder tSql = new StringBuilder();

            tSql.Append("  SELECT ").Append(tSqlSelect);

            tSql.AppendFormat("\n  FROM {0} ", tSqlForm);

            if (!string.IsNullOrEmpty(tSqlWhere))
            {
                tSql.AppendFormat("\n  WHERE {0} ", tSqlWhere);
            }

            return tSql.ToString();
        }

        /// <summary>
        /// 检查域名是否唯一
        /// </summary>
        /// <param name="domain_text">域名</param>
        /// <param name="id">自身ID</param>
        /// <returns></returns>
        bool CheckOnly(string domain_text, string id)
        {

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            string sql = $"select count(MR_WEBSITE_MAP_ID) from MR_WEBSITE_MAP where ROW_SID >=0 and DOMAIN_TEXT = '{domain_text?.Trim()}'";

            if (!string.IsNullOrWhiteSpace(id))
            {
                sql += $" and MR_WEBSITE_MAP_ID <> {id}";
            }

            int count = decipher.ExecuteScalar<int>(sql);


            if (count > 0)
            {
                return false;
            }


            return true;

        }


        /// <summary>
        /// 网站主表查询按钮点击事件
        /// </summary>
        public void GoStore1Select()
        {

            store1.DataBind();

        }

        /// <summary>
        /// 域名列表查询按钮点击事件
        /// </summary>
        public void GoStore2Select()
        {

            store2.DataBind();

        }

        string GetTqlWhereBySearch(Panel panel)
        {
            List<FieldBase> cons = FindFieldControls(panel);

            List<string> where = new List<string>();

            foreach (FieldBase field in cons)
            {


                string value = FullFilter_Item(field);

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                where.Add(value);



            }

            if (where.Count == 0)
            {
                return "";
            }


            return string.Join(" and ", where);


        }

        /// <summary>
        /// 查找字段的控件
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        List<FieldBase> FindFieldControls(Panel layout)
        {
            List<FieldBase> cons = new List<FieldBase>();

            foreach (System.Web.UI.Control item in layout.Controls)
            {
                FieldBase field = item as FieldBase;

                if (field == null || StringUtil.IsBlank(field.DataField))
                {
                    continue;
                }

                cons.Add(field);
            }

            return cons;
        }

        void FullFilter_IRange(LightModelFilter filter, FieldBase field, IRangeControl rangeCon)
        {
            //rangeCon = (IRangeControl)field;

            if (!StringUtil.IsBlank(rangeCon.StartValue))
            {
                DateTime startDate;

                if (DateTime.TryParse(rangeCon.StartValue, out startDate))
                {
                    filter.And(field.DataField, startDate, Logic.GreaterThanOrEqual);
                }
                else
                {
                    decimal value;

                    if (decimal.TryParse(rangeCon.StartValue, out value))
                    {
                        filter.And(field.DataField, value, Logic.GreaterThanOrEqual);
                    }
                }
            }

            if (!StringUtil.IsBlank(rangeCon.EndValue))
            {
                DateTime endDate;

                if (DateTime.TryParse(rangeCon.EndValue, out endDate))
                {
                    endDate = endDate.Date.Add(new TimeSpan(0, 23, 59, 59, 999));

                    filter.And(field.DataField, endDate, Logic.LessThanOrEqual);
                }
                else
                {
                    decimal value;

                    if (decimal.TryParse(rangeCon.EndValue, out value))
                    {
                        filter.And(field.DataField, value, Logic.LessThanOrEqual);
                    }
                }
            }
        }

        string FullFilter_Item(FieldBase field)
        {
            if (StringUtil.IsBlank(field.Value))
            {
                return "";
            }


            StringBuilder sb = new StringBuilder();


            string key = field.Value;

            Logic logic = ModelConvert.ToLogic(field.DataLogic);

            if (logic == Logic.Like)
            {
                if (!key.Contains("%"))
                {
                    key = "%" + key + "%";
                }
            }

            return $" {field.DataField} {field.DataLogic} '{key}' ";



        }

        /// <summary>
        /// 插入域名函数
        /// </summary>
        /// <param name="domain_text"></param>
        public void GoInsertDomain(string domain_text)
        {

            string main_id = store1.CurDataId;

            if (string.IsNullOrWhiteSpace(main_id))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(domain_text))
            {
                MessageBox.Alert("域名不能为空！");
                return;
            }


            bool flag = CheckOnly(domain_text, string.Empty);

            if (!flag)
            {
                MessageBox.Alert("域名重复了！请联系系统管理员！");
                return;
            }

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            SModel sm_ws = decipher.GetSModel($"select * from MR_WEBSITE where ROW_SID >=0 and MR_WEBSITE_ID = {main_id}");


            EcUserState user = EcContext.Current.User;

            string agent_code = user.ExpandPropertys["AGENT_CODE"];

            SModel sm = new SModel("MR_WEBSITE_MAP");
            sm["MR_WEBSITE_ID"] = main_id;
            sm["ROW_SID"] = 0;
            sm["ROW_DATE_CREATE"] = sm["ROW_DATE_UPDATE"] = DateTime.Now;
            sm["USER_CODE"] = agent_code;
            sm["USER_TEXT"] = user.LoginName;
            sm["DOMAIN_TEXT"] = domain_text;
            sm["ROOT_PATH"] = sm_ws["ROOT_PATH"];
            sm["ROOT_CATALOG"] = sm_ws["ROOT_CATALOG"];
            sm["DB_PATH"] = sm_ws["ROOT_PATH"];
            sm["DB_CATALOG"] = sm_ws["ROOT_CATALOG"];

            decipher.InsertSModel(sm);

            store2.DataBind();


        }

        /// <summary>
        /// 删除域名数据
        /// </summary>
        /// <param name="id"></param>
        void DeleteWebsite(int id)
        {

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");


            SModel sm = decipher.GetSModel($"select * from MR_WEBSITE where ROW_SID >=0 and MR_WEBSITE_ID = {id}");

            if (sm == null)
            {
                return;
            }

            string root_path = sm.Get<string>("ROOT_PATH");

            SModelList sms = decipher.GetSModelList("select * from MR_WEBSITE_MAP where ROW_SID >=0 and MR_WEBSITE_ID ="+id);

            foreach (SModel sm_map in sms)
            {
                DeleteWebsiteMap(sm_map,decipher);
            }

            if (string.IsNullOrWhiteSpace(root_path) || !Directory.Exists(root_path))
            {
                sm["ROW_SID"] = -3;
                sm["ROW_DATE_DELETE"] = DateTime.Now;

                sm.Remove("MR_WEBSITE_ID");

                decipher.UpdateSModel(sm, "MR_WEBSITE", "MR_WEBSITE_ID = " + id);

                return;

            }

            DirectoryInfo dicInfo = new DirectoryInfo(root_path);

            string new_path = dicInfo.FullName + "__DEL";


            dicInfo.MoveTo(new_path);


            SModel sm_delete1 = new SModel("MR_WEBSITE");


            sm_delete1["ROW_SID"] = -3;
            sm_delete1["ROW_DATE_DELETE"] = DateTime.Now;

            decipher.UpdateSModel(sm_delete1, "MR_WEBSITE", "MR_WEBSITE_ID = " + id);

        }

        /// <summary>
        /// 删除子表数据
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="decipher"></param>
        void DeleteWebsiteMap(SModel sm, DbDecipher decipher)
        {

            if (sm == null)
            {
                return;
            }

            try
            {
                sm["ROW_DATE_DELETE"] = DateTime.Now;


                decipher.InsertSModel("MR_WEBSITE_MAP_DEL", sm);

            }
            catch (Exception ex)
            {
                log.Error("插入域名删除表失败了！", ex);

                return;

            }

            string sql_del = "DELETE FROM MR_WEBSITE_MAP  WHERE ROW_SID >= 0 and MR_WEBSITE_MAP_ID = " + sm["MR_WEBSITE_MAP_ID"];

            decipher.ExecuteNonQuery(sql_del);

        }


        /// <summary>
        /// 生成目录按钮点击事件
        /// </summary>
        public void GoCreateDir()
        {


            string base_path = GlobelParam.GetValue<string>("BASE_PATH", @"D:\用户网站", "用户存在数据基本目录");


            int id = StringUtil.ToInt32(store1.CurDataId);

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");


            SModel sm = decipher.GetSModel($"select * from MR_WEBSITE where ROW_SID >=0 and BIZ_SID <> 999 and MR_WEBSITE_ID = {id}");

            if (sm == null)
            {
                return;
            }

            int biz_sid = sm.Get<int>("BIZ_SID");

            if (biz_sid == 999)
            {
                MessageBox.Alert("已经生成过目录了，不能重新生成了！");
                return;
            }



            string root_path = sm.Get<string>("ROOT_PATH");
            string root_catalog = sm.Get<string>("ROOT_CATALOG");
            string user_code = sm.Get<string>("USER_CODE");

            if (string.IsNullOrWhiteSpace(root_catalog))
            {
                MessageBox.Alert("目录名称不能为空！");
                return;
            }

            string dir_root_path = $"{base_path}\\{user_code}\\{root_catalog}";

            //目录不存在才新建
            if (!Directory.Exists(dir_root_path))
            {
                Directory.CreateDirectory(dir_root_path);
            }

            sm.Remove("MR_WEBSITE_ID");
            sm.Remove("ROW_DATE_DELETE");

            sm["ROOT_PATH"] = dir_root_path;
            sm["BIZ_SID"] = 999;
            sm["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateSModel(sm, "MR_WEBSITE", "MR_WEBSITE_ID = " + id);


            //子表数据也假删除
            string update_sql = $"UPDATE MR_WEBSITE_MAP SET ROOT_PATH = '{dir_root_path}',ROOT_CATALOG = '{root_path}' WHERE ROW_SID >= 0 and MR_WEBSITE_ID = " + id;

            decipher.ExecuteNonQuery(update_sql);


            Toast.Show("创建目录成功了！");

            store1.DataBind();


        }


        /// <summary>
        /// 资源管理器按钮点击事件
        /// </summary>
        public void GoShowExplorer()
        {

            int id = StringUtil.ToInt32(store1.CurDataId);

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen("WS");

            SModel sm = decipher.GetSModel($"select * from MR_WEBSITE where ROW_SID >=0 and MR_WEBSITE_ID = {id}");

            if (sm == null)
            {
                return;
            }

            string root_path = sm.Get<string>("ROOT_PATH");

            if (string.IsNullOrWhiteSpace(root_path) || !Directory.Exists(root_path))
            {
                MessageBox.Alert("请先生成目录！");
                return;
            }



            string url = $"/App/InfoGrid2/View/Biz/MR/DefExplorer.aspx?id={id}";


            EcView.Show(url, "资源管理器");



        }

        /// <summary>
        /// 更新域名列表中的停止字段
        /// </summary>
        void UpdataDomainIsStop(SModel sm,int id, DbDecipher decipher)
        {
           
            bool is_stop = sm.Get<bool>("IS_STOP");

            string sql = $"UPDATE MR_WEBSITE_MAP SET IS_STOP = {(is_stop ? 1 : 0)} WHERE MR_WEBSITE_ID = {id} and ROW_SID >= 0";


            decipher.ExecuteNonQuery(sql);

        }





    }

}