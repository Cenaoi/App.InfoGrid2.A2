using System;
using System.Collections.Generic;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.IG2.Core.UI;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EC5.IG2.Plugin;
using System.Reflection;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.View.MoreView
{
    public partial class MoreViewPreview : WidgetControl, IView
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


            List<string> fields = new List<string>();

            this.store1.ReadOnly = true;

            StoreTSqlQuerty tSql = this.store1.TSqlQuery;

            tSql.Enabeld = true;
            tSql.Select = ViewMgr.GetTSqlSelect(m_ViewSet, ref fields);
            tSql.Form = ViewMgr.GetTSqlForm(m_ViewSet);
            tSql.Where = ViewMgr.GetTSqlWhere(m_ViewSet);
            tSql.OrderBy = ViewMgr.GetTSqlOrder(m_ViewSet, fields);


            #region 权限-结构过滤

            foreach (IG2_VIEW_TABLE vTab in m_ViewSet.Tables)
            {
                IG2_TABLE srcTab = TableMgr.GetTableForName(vTab.TABLE_NAME);

                if (!srcTab.SEC_STRUCT_ENABLED)
                {
                    continue;
                }

                UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

                if (uSec == null)
                {
                    continue;
                }

                IList<LModel> cols = TableMgr.GetLCols(srcTab.IG2_TABLE_ID,
                    new string[] { "DB_FIELD", "FILTER_CATA_TABLE", "FILTER_CATA_FIELD" });

                foreach (LModel col in cols)
                {
                    string dbField = col.Get<string>("DB_FIELD");
                    string cataTable = col.Get<string>("FILTER_CATA_TABLE");
                    string cataField = col.Get<string>("FILTER_CATA_FIELD");

                    if (string.IsNullOrEmpty(cataTable) || string.IsNullOrEmpty(cataField))
                    {
                        continue;
                    }


                    string tSqlWhere = string.Format("{0} IN (SELECT {1} FROM {2} WHERE ROW_SID >=0 AND BIZ_CATA_CODE in ({3}))",
                        vTab.TABLE_NAME + "." + dbField,
                        cataField,
                        cataTable, uSec.ArrCatalogCodeString());

                    if (!string.IsNullOrEmpty(tSql.Where))
                    {
                        tSql.Where += " AND ";
                    }

                    tSql.Where += tSqlWhere;
                }
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        ViewSet m_ViewSet;

        int m_PageId;
        int m_MenuId;
        
        /// <summary>
        /// 原作为安全标识，现已经没起作用。
        /// </summary>
        string m_SecTag;


        /// <summary>
        /// 处理没有 DBType 数据类型的字段
        /// </summary>
        /// <param name="tSet"></param>
        private void FullDbType(TableSet tSet)
        {
            int srcIndex = 0;

            foreach (var col in tSet.Cols)
            {
                if (!StringUtil.IsBlank(col.DB_TYPE))
                {
                    continue;
                }

                string[] sp = StringUtil.Split(col.VIEW_FIELD_SRC, ".");

                TableSet srcTable = TableBufferMgr.GetTable(sp[0]);

                if (srcTable == null)
                {
                    continue;
                }

                IG2_TABLE_COL tCol = srcTable.Find(sp[1],out srcIndex);

                if (tCol != null)
                {
                    col.DB_TYPE = tCol.DB_TYPE;
                }
            }
        }

        private void CreateUI_ForTable()
        {
            int id = WebUtil.QueryInt("id", 2);

            m_PageId = WebUtil.QueryInt("id");

            m_SecTag = WebUtil.Query("sec_tag");
            m_MenuId = WebUtil.QueryInt("menu_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            m_ViewSet = ViewSet.Select(decipher, id);


            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);

            IG2_TABLE model = tSet.Table;

            //处理没有 DBType 数据类型的字段
            FullDbType(tSet);


            string alias_title = WebUtil.Query("alias_title");

            HeadPanel.Visible = tSet.Table.IS_BIG_TITLE_VISIBLE;
            headLab.Value = string.Concat( "<span class='page-head' >" , StringUtil.NoBlank(alias_title, model.DISPLAY) ,"</span>"); 


            LModelElement modelElem = ViewMgr.GetModelElem(m_ViewSet);

            this.store1.Model = modelElem.DBTableName;
            this.store1.IdField = m_ViewSet.View.MAIN_TABLE_NAME + "_" + m_ViewSet.View.MAIN_ID_FIELD;


            M2SearchFormFactory searchFormFty = new M2SearchFormFactory(this.IsPostBack,this.store1.ID);
            searchFormFty.CreateControls(this.searchForm, tSet);


            M2TableFactory tableFty = new M2TableFactory(this.IsPostBack);
            tableFty.CreateTableColumns(this.table1, this.store1, tSet);


            M2SecurityUiFactory secUiFty = new M2SecurityUiFactory();
            secUiFty.InitSecUI(m_PageId, "TABLE", m_MenuId, tSet);
            
            secUiFty.Filter("", m_SecTag, "",this.Toolbar1, this.table1, this.store1);

            secUiFty.FilterForForm("", m_SecTag, "", this.searchForm, this.store1);



            ToolbarSet toolbarSet = new ToolbarSet();
            toolbarSet.SelectForTable(decipher, id);

            if (toolbarSet.Toolbar != null)
            {
                this.Toolbar1.Items.Clear();

                M2ToolbarFactory toolbarFactory = new M2ToolbarFactory();
                
                toolbarFactory.CreateItems(this.Toolbar1, toolbarSet);
            }


        }
        

        public void StepEdit3()
        {
            Guid opGuid = Guid.NewGuid();

            int id = WebUtil.QueryInt("id");

            try
            {
                InitData(opGuid);

                MiniPager.Redirect(string.Format("MViewStepEdit3.aspx?id={0}&uid={1}", id, opGuid));
            }
            catch (Exception ex)
            {
                log.Error( ex);
                MessageBox.Alert(ex.Message);
            }
        }


        /// <summary>
        /// 列设置
        /// </summary>
        public void StepEdit5()
        {
            Guid opGuid = Guid.NewGuid();

            int id = WebUtil.QueryInt("id");

            MiniPager.Redirect(string.Format("MViewStepEdit5.aspx?id={0}&uid={1}", id, opGuid));
        }

        /// <summary>
        /// 列高级设置
        /// </summary>
        public void StepEdit4()
        {
            Guid opGuid = Guid.NewGuid();

            int id = WebUtil.QueryInt("id");

            MiniPager.Redirect(string.Format("MViewStepEdit4.aspx?id={0}&uid={1}" , id,opGuid));
        }


        /// <summary>
        /// 工具栏设置
        /// </summary>
        public void ToolbarSetup()
        {
            Guid opGuid = Guid.NewGuid();

            int id = WebUtil.QueryInt("id");


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_ID", id);
            filter.Fields = new string[] { "IG2_TOOLBAR_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            int toolbarId = decipher.ExecuteScalar<int>(filter);

            string srcUrl = Base64Util.ToString($"/App/InfoGrid2/View/MoreView/MoreViewPreview.aspx?id={id}", Base64Mode.Http);

            string urlStr;

            if (toolbarId > 0)
            {
                urlStr = $"/app/infogrid2/view/OneToolbar/SetToolbar.aspx?id={toolbarId}&table_id={id}&src_url={srcUrl}";
            }
            else
            {
                urlStr = $"/app/infogrid2/view/OneToolbar/ToolbarStepNew1.aspx?table_id={id}&src_url={srcUrl}";
            }

            MiniPager.Redirect(urlStr);
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


                ViewSet vSet = ViewSet.Select(decipher, id);


                TmpViewSet tvSet = vSet.ToTmpViewSet();

                tvSet.Insert(decipher, uid, this.Session.SessionID);

            }
            catch (Exception ex)
            {
                throw new Exception("拷贝到临时表数据出错。",ex);
            }


        }



        /// <summary>
        /// 导出 Excel 
        /// </summary>
        public void ToExcel()
        {
            string plugClass = "EC5.IG2.Plugin.Custom.InputOutExcelPlugin";
            string plugMethod = "InputOut";

            Type plugT = PluginManager.Get(plugClass);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + plugClass);
                return;
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
                return;
            }


            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;
                inter.DymEvent += inter_DymEvent;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = "{}";

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = this.store1;
                inter.SrcTable = this.table1;

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }

        }

        void inter_DymEvent(object sender, PagePluginEventArgs e)
        {
            if (e.Action == "sec_filter")
            {
            
                string alias_title = WebUtil.Query("alias_title");

                e.Params["display"] = StringUtil.NoBlank(alias_title, (string)e["display"]);

                TableSet tSet = e["table_set"] as TableSet;

                M2SecurityUiFactory secUiFty = new M2SecurityUiFactory();
                secUiFty.InitSecUI(m_PageId, "TABLE", m_MenuId, tSet);
            
                secUiFty.Filter_For_TableSet("", m_SecTag, "", tSet);

            }
        }


        
        public void ToPrint()
        {
            int id = WebUtil.QueryInt("id", 2);

            string urlStr = "/App/InfoGrid2/View/PrintTemplate/PrintTemplateMore.aspx?id=" + id;

            Window win = new Window("打印");
            win.ContentPath = urlStr;
            win.State = WindowState.Max;
            win.ShowDialog();
        }

        /// <summary>
        /// 小渔夫加的  打印按钮事件
        /// </summary>
        public void btnPrint() 
        {
            ToPrint();
        }


        /// <summary>
        ///  小渔夫加的 管理模板按钮事件
        /// </summary>
        public void ManageTemplate()
        {
            int id = WebUtil.QueryInt("id", 2);

            string urlStr = "/App/InfoGrid2/View/PrintTemplate/ManageTemplateMore.aspx?id=" + id;

            Window win = new Window("模板管理");
            win.ContentPath = urlStr;
            win.State = WindowState.Max;
            win.ShowDialog();
        }



    }
}