using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.View.OneTable;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EC5.IG2.Core.UI;

namespace App.InfoGrid2.View.MoreView
{
    public partial class DataImportDialog : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            CreateUI_ForTable();

            store1.PageChanged += store1_PageChanged; 

        }

        string m_Filter2;


        void store1_PageChanged(object sender, EventArgs e)
        {
            //if (m_ViewSet != null && m_ViewSet.View != null)
            //{
            //    string tSqlWhere2 = GetTSqlWhere_ForFilter2(m_Filter2);

            //    string tSql = ViewMgr.GetTSql(m_ViewSet, this.store1.CurPage, 20, tSqlWhere2);

            //    DbDecipher decipher = ModelAction.OpenDecipher();

            //    LModelList<LModel> models = decipher.GetModelList(tSql);

            //    this.store1.AddRange(models);

            //}
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
                string p_type = GetJsonValue(item, "p_type","DEFAULT");
                string field = GetJsonValue(item, "field");
                string logic = GetJsonValue(item, "logic","=");
                string value = GetJsonValue(item, "value");

                if (i++ > 0) { sb.Append(" AND "); }

                p_type = p_type.ToUpper();

                if (p_type == "TSQL_WHERE")
                {
                    sb.Append("(").Append(value).Append(")");
                }
                else
                {
                    if (StringUtil.IsBlank(field))
                    {
                        throw new Exception("二次筛选的参数字段名不能为空。");
                    }

                    sb.AppendFormat("({0} {1} '{2}')", field, logic, value);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetJsonValue(JObject obj, string attr,string defualtValue)
        {
            string value = obj.Value<string>(attr);

            return StringUtil.NoBlank(value, defualtValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetJsonValue(JObject obj, string attr)
        {
            string value = obj.Value<string>(attr);


            return value;
        }


        /// <summary>
        /// 获取 T-SQL 的 Where 子语句
        /// </summary>
        /// <param name="filter2"></param>
        /// <returns></returns>
        private List<Param> GetParmas_ForFilter2(string filter2)
        {
            List<Param> ps = new List<Param>();

            if (StringUtil.IsBlank(filter2))
            {
                return ps;
            }

            JArray items = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(filter2);


            int i = 0;

            foreach (JObject item in items)
            {
                string field = item.Value<string>("field");
                string logic = item.Value<string>("logic");
                string value = item.Value<string>("value");

                Param p = new Param(field, value);
                p.Logic = logic;

                ps.Add(p);
            }

            return ps;
        }



        protected override void OnLoad(EventArgs e)
        {
            m_Filter2 = WebUtil.QueryBase64("filter2");  //二次过滤参数


            if (m_ViewSet == null || m_ViewSet.View == null)
            {
                this.store1.FilterParams.Add(new Param("ROW_SID", "0", DbType.Int32) { Logic = ">=" });

                List<Param> ps = GetParmas_ForFilter2(m_Filter2);

                this.store1.SelectQuery.AddRange(ps);
            }
            else
            {

            }

            try
            {
                if (m_CustomPage != null)
                {
                    m_CustomPage.OnProLoad();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("执行自定义类的 OnLoad 代码错误", ex);
            }



            if (!this.IsPostBack)
            {
                if (m_ViewSet != null && m_ViewSet.View != null)
                {
                    string tSqlWhere2 = GetTSqlWhere_ForFilter2(m_Filter2);

                    //string tSql = ViewMgr.GetTSql(m_ViewSet, this.store1.CurPage, 20, tSqlWhere2);

                    //DbDecipher decipher = ModelAction.OpenDecipher();

                    //LModelList<LModel> models = decipher.GetModelList(tSql);

                    //this.store1.AddRange(models);

                    if (!StringUtil.IsBlank(tSqlWhere2))
                    {
                        this.store1.FilterParams.Add(new EasyClick.Web.Mini2.TSqlWhereParam(tSqlWhere2));
                    }

                    this.store1.DataBind();
                }
                else
                {

                    this.store1.DataBind();
                }
            }
        }

        ViewSet m_ViewSet;

        /// <summary>
        /// 创建界面 UI
        /// </summary>
        private void CreateUI_ForTable()
        {
            int id = WebUtil.QueryInt("id", 2);


            DbDecipher decipher = ModelAction.OpenDecipher();


            m_ViewSet = ViewSet.Select(decipher, id);


            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);


            M2TableFactory tableFactory = new M2TableFactory(this.IsPostBack);

            tableFactory.CreateTableColumns(this.table1, this.store1, tSet);

            M2SearchFormFactory searchFFty = new M2SearchFormFactory(this.IsPostBack,this.store1.ID);
            searchFFty.CreateControls(this.searchForm, tSet);


            LModelElement modelElem;

            if (m_ViewSet != null && m_ViewSet.View != null)
            {
                modelElem = ViewMgr.GetModelElem(m_ViewSet);

                this.store1.Model = modelElem.DBTableName;
                this.store1.IdField = m_ViewSet.View.MAIN_TABLE_NAME + "_" + m_ViewSet.View.MAIN_ID_FIELD;


                List<string> fields = new List<string>();

                StoreTSqlQuerty tSql = this.store1.TSqlQuery;

                tSql.Enabeld = true;
                tSql.Select = ViewMgr.GetTSqlSelect(m_ViewSet, ref fields);
                tSql.Form = ViewMgr.GetTSqlForm(m_ViewSet);
                tSql.Where = ViewMgr.GetTSqlWhere(m_ViewSet);
                tSql.OrderBy = ViewMgr.GetTSqlOrder(m_ViewSet, fields);
            }
            else
            {
                modelElem = TableMgr.GetModelElem(tSet);


                this.store1.Model = modelElem.DBTableName;
                this.store1.IdField = tSet.Table.ID_FIELD;

            }



            IG2_TABLE m_Table = tSet.Table;

            try
            {
                InitCustomPage(m_Table.SERVER_CLASS);
            }
            catch (Exception ex)
            {
                log.Error("加载自定义类错误:" + m_Table.SERVER_CLASS, ex);
            }



        }



        ExPage m_CustomPage;

        List<Control> m_UserControls = new List<Control>();

        /// <summary>
        /// 初始化自定义服务器代码
        /// </summary>
        private void InitCustomPage(string serClass)
        {
            if (StringUtil.IsBlank(serClass))
            {
                return;
            }


            serClass = serClass.Trim();

            Type customPageT = Type.GetType(serClass);

            if (customPageT == null)
            {
                log.ErrorFormat("自定义类“{0}”没有找到", serClass);
                return;
            }

            m_CustomPage = Activator.CreateInstance(customPageT) as ExPage;

            if (m_CustomPage == null)
            {
                log.ErrorFormat("自定义类“{0}”没法实例化。", serClass);
                return;
            }

            m_CustomPage.SetDefaultValue(this, this.searchForm, this.store1, this.Toolbar1, this.table1);
            m_CustomPage.UserControls = m_UserControls;
            m_CustomPage.ID = "ExPage";

            viewport1.Controls.Add(m_CustomPage);
        }

        public void StepEdit2()
        {
            Guid opGuid = Guid.NewGuid();

            int id = WebUtil.QueryInt("id");

            //操作的 Guid,主要是避免操作重复
            //var opGuid = Mini2.Guid.newGuid();

            InitData(opGuid);

            MiniPager.Redirect(string.Format("MViewStepEdit3.aspx?id={0}&uid={1}", id, opGuid));
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData(Guid uid)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                int id = WebUtil.QueryInt("id");
                //Guid uid = WebUtil.QueryGuid("uid");

                ViewSet vSet = ViewSet.Select(decipher, id);


                TmpViewSet tvSet = vSet.ToTmpViewSet();

                tvSet.Insert(decipher, uid, this.Session.SessionID);

            }
            catch (Exception ex)
            {
                MessageBox.Alert("拷贝到临时表数据出错.");

                throw new Exception("拷贝到临时表数据出错。", ex);
            }


        }


        /// <summary>
        /// 点击了确定按钮
        /// </summary>
        public void GoSubmit()
        {
            if (this.table1.CheckedRows.Count == 0)
            {
                MessageBox.Alert("你没有,选中需要导入的数据.");
                return;
            }


            List<int> ids = new List<int>();

            foreach (DataRecord rec in this.table1.CheckedRows)
            {
                ids.Add(StringUtil.ToInt(rec.Id));
            }

            string idStr = ArrayUtil.ToString(ids.ToArray());



            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:'" + idStr + "'})");

        }



    }
}