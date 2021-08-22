using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.CMS;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.CMS
{
    public partial class CmsMenu : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            store1.Inserting += Store1_Inserting;

            if (!IsPostBack)
            {

                store1.DataBind();

            }

        }

        private void Store1_Inserting(object sender, ObjectCancelEventArgs e)
        {

            LModel lm = e.Object as LModel;

            lm["PK_MENU_CODE"] = BillIdentityMgr.NewCodeForDay("CMS_MENU_CODE", "M", 5);
            


        }

        public void GoCreateTable()
        {

            int id =  StringUtil.ToInt(store1.CurDataId,0);

            DbDecipher decipher = ModelAction.OpenDecipher();

            CMS_MENU cms_menu = decipher.SelectModelByPk<CMS_MENU>(id);

            if(cms_menu == null)
            {
                MessageBox.Alert("请选择一条记录！");
                return;
            }

            if (string.IsNullOrWhiteSpace(cms_menu.MENU_TEXT))
            {
                Toast.Show("菜单名称不能为空！");
                return;
            }


            if (string.IsNullOrWhiteSpace(cms_menu.MENU_TYPE))
            {

                MessageBox.Alert("菜单类型为空不能创建表！");
                return;

            }
            

            if(cms_menu.MENU_TYPE == "TILE")
            {
                CreateTile(cms_menu);
                return;
            }

            if (cms_menu.MENU_TYPE == "ARTICLE")
            {

                IG2_TABLE ig2_table_item = decipher.SelectModelByPk<IG2_TABLE>(cms_menu.ITEM_TABLE_ID);


                if (ig2_table_item != null)
                {                    
                    Toast.Show("已经创建表了，不能重复创建！");
                    return;
                }

                CreateNewTable(cms_menu);
                return;
            }
                
        }


        /// <summary>
        ///根据菜单对象创建的单页数据
        /// </summary>
        /// <param name="cms_menu">菜单对象</param>
        void CreateTile(CMS_MENU cms_menu)
        {


            string pk_menu_code = cms_menu.PK_MENU_CODE;

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter(typeof(CMS_TILE));
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_MENU_CODE", pk_menu_code);

            bool falg = decipher.ExistsModels(lmFilter);

            if (falg)
            {
                Toast.Show("创建单页数据成功！");
                return;
            }


            CMS_TILE tile = new CMS_TILE();
            tile.ROW_SID = 0;
            tile.ROW_DATE_CREATE = tile.ROW_DATE_UPDATE = DateTime.Now;
            tile.MENU_TEXT = cms_menu.MENU_TEXT;
            tile.FK_MENU_CODE = pk_menu_code;
            tile.PK_TILE_CODE = BillIdentityMgr.NewCodeForDay("CMS_TILE_CODE", "T", 5);

            decipher.InsertModel(tile);

            Toast.Show("创建单页数据成功！");    

        }


        void CreateNewTable(CMS_MENU cms_menu)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();



            try
            {

                
                string table_name = BillIdentityMgr.NewCodeForNum("CMS_USER_TABLE", "CMS_UT_", 3);


                cms_menu.TABLE_NAME = table_name;
                cms_menu.ROW_DATE_UPDATE = DateTime.Now;

                cms_menu.ITEM_TABLE_ID = CreateItem(cms_menu.MENU_TEXT,table_name);

                cms_menu.CATA_TABLE_ID = CreateCata(cms_menu.MENU_TEXT, table_name);

                decipher.UpdateModelProps(cms_menu, "TABLE_NAME", "ROW_DATE_UPDATE", "ITEM_TABLE_ID", "CATA_TABLE_ID");



                Toast.Show("创建表成功了！");

            }
            catch (Exception ex)
            {
                log.Error(ex);


                MessageBox.Alert("哦噢，创建明细表和目录表出错了！");

                return;

            }


        }

        /// <summary>
        /// 根据菜单名称创建明细表
        /// </summary>
        /// <param name="menu_text">菜单名称</param>
        /// <param name="table_name">统一的表名称</param>
        /// <returns></returns>
        int CreateItem(string menu_text,string table_name)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sessionId = Session.SessionID;
            Guid tmpId = Guid.NewGuid();

            IG2_TMP_TABLE table = CreateDefaultTable(menu_text + "明细表", sessionId, tmpId);


            table.TABLE_NAME = table_name + "_ITEM";

            IG2_TMP_TABLECOL col = CreateDefaultCol(table.TABLE_UID, table.ID_FIELD, sessionId, tmpId);

            List<IG2_TMP_TABLECOL> cols = CreateCols(table.TABLE_UID, sessionId, tmpId);


            #region  明细表默认字段

            IG2_TMP_TABLECOL col_1 = CreateBizCol("string", "FK_CATALOG_CODE", "外键，目录自定义主键编码", true, string.Empty, table.TABLE_UID, sessionId, tmpId, is_visible: true, sec_level: 0,is_readonly:false);
            IG2_TMP_TABLECOL col_2 = CreateBizCol("string", "CATALOG_TEXT", "目录名称", true, string.Empty, table.TABLE_UID, sessionId, tmpId,250, is_visible: true, sec_level: 0, is_readonly: false);
            IG2_TMP_TABLECOL col_3 = CreateBizCol("string", "TITLE", "标题", true, string.Empty, table.TABLE_UID, sessionId, tmpId,250, is_visible: true, sec_level: 0);
            IG2_TMP_TABLECOL col_4 = CreateBizCol("string", "IMG_URL", "图片地址", true, string.Empty, table.TABLE_UID, sessionId, tmpId, 250, is_visible: true, sec_level: 0,display_type: "file_upload", is_readonly: false);
            IG2_TMP_TABLECOL col_5 = CreateBizCol("string", "ITEM_CONTENT", "内容", true, string.Empty, table.TABLE_UID, sessionId, tmpId, is_remark:true, is_visible: true, sec_level: 0,display_type: "html_edit", is_readonly: false);
            IG2_TMP_TABLECOL col_6 = CreateBizCol("string", "PK_ITEM_CODE", "自定义主键编码", true, string.Empty, table.TABLE_UID, sessionId, tmpId, is_visible: true, sec_level: 0);

            cols.Add(col_1);
            cols.Add(col_2);
            cols.Add(col_3);
            cols.Add(col_4);
            cols.Add(col_5);
            cols.Add(col_6);

            #endregion


            decipher.InsertModel(table);
            decipher.InsertModel(col);
            decipher.InsertModels(cols);


            IG2_TABLE def_table = TmpTableMgr.TempTable2TableCMS(tmpId);


            CreateViewByTable(def_table);

            InserMenu(def_table.IG2_TABLE_ID);


            return def_table.IG2_TABLE_ID;

        }


        /// <summary>
        /// 根据正式表创建一张视图表
        /// </summary>
        /// <param name="def_table">正式表对象</param>
        void CreateViewByTable(IG2_TABLE def_table)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("IG2_TABLE_ID", def_table.IG2_TABLE_ID);


            List<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>(lmFilter);

            List<IG2_TABLE_COL> new_cols = new List<IG2_TABLE_COL>();



           IG2_TABLE table_view = new IG2_TABLE();

            def_table.CopyTo(table_view);

            table_view.TABLE_TYPE_ID = "VIEW";
            table_view.TABLE_SUB_TYPE_ID = "USER";
            table_view.IG2_CATALOG_ID = 102;

            //启动事务了
            decipher.BeginTransaction();


            try
            {


                decipher.InsertModel(table_view);

                foreach (IG2_TABLE_COL col in cols)
                {

                    IG2_TABLE_COL new_col = new IG2_TABLE_COL();


                    col.CopyTo(new_col, true);

                    new_col.TABLE_NAME = table_view.TABLE_NAME;
                    new_col.IG2_TABLE_ID = table_view.IG2_TABLE_ID;


                    new_cols.Add(new_col);

                }


                decipher.InsertModels(new_cols);

                def_table.SetTakeChange(true);

                //显示编辑按钮
                def_table.VISIBLE_BTN_EDIT = true;
                def_table.FORM_EDIT_TYPE = "TABLE_FORM";
                def_table.FORM_NEW_TYPE = "TABLE_FORM";
                def_table.FORM_NEW_PAGEID = table_view.IG2_TABLE_ID;
                def_table.FORM_EDIT_PAGEID = table_view.IG2_TABLE_ID;

                decipher.UpdateModel(def_table, true);


                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();


                throw new Exception("创建视图表出错了！", ex);
            }

        }


        /// <summary>
        /// 根据菜单名称创建目录表
        /// </summary>
        /// <param name="menu_text">菜单名称</param>
        /// <param name="table_name">统一的表名称</param>
        /// <returns></returns>
        int CreateCata(string menu_text, string table_name)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sessionId = Session.SessionID;

            Guid tmpId = Guid.NewGuid();

            IG2_TMP_TABLE table = CreateDefaultTable(menu_text + "目录表", sessionId, tmpId);

            
            table.TABLE_NAME = table_name + "_CATALOG";

            IG2_TMP_TABLECOL col = CreateDefaultCol(table.TABLE_UID, table.ID_FIELD, sessionId, tmpId);

            List<IG2_TMP_TABLECOL> cols = CreateCols(table.TABLE_UID, sessionId, tmpId);

            #region 分类表默认字段

            IG2_TMP_TABLECOL col_1 = CreateBizCol("string", "PK_CATALOG_CODE", "自定义主键编码", true, string.Empty, table.TABLE_UID, sessionId, tmpId,is_visible:true, sec_level:0);
            IG2_TMP_TABLECOL col_2 = CreateBizCol("string", "PARENT_CODE", "上级自定义主键编码", true, string.Empty, table.TABLE_UID, sessionId, tmpId, sec_level: 0);
            IG2_TMP_TABLECOL col_3 = CreateBizCol("string", "CATA_TEXT", "目录文字", true, string.Empty, table.TABLE_UID, sessionId, tmpId,250, is_visible: true, is_readonly: false, sec_level: 0);
            IG2_TMP_TABLECOL col_4 = CreateBizCol("string", "ICON_URL", "图标路径", true, string.Empty, table.TABLE_UID, sessionId, tmpId,250,sec_level: 0);
            IG2_TMP_TABLECOL col_5 = CreateBizCol("string", "INTRO_DESC", "简要说明", true, string.Empty, table.TABLE_UID, sessionId, tmpId,is_remark:true, is_visible: true,is_readonly:false, sec_level: 0);
            IG2_TMP_TABLECOL col_6 = CreateBizCol("string", "DETAIL_DESC", "详细说明", true, string.Empty, table.TABLE_UID, sessionId, tmpId, is_remark: true, sec_level: 0);
            IG2_TMP_TABLECOL col_7 = CreateBizCol("string", "CATA_TYPE", "目录类型", true, string.Empty, table.TABLE_UID, sessionId, tmpId,  sec_level: 0);
            IG2_TMP_TABLECOL col_8 = CreateBizCol("string", "ARR_CHILD_ID", "所有子节点集合", true, string.Empty, table.TABLE_UID, sessionId, tmpId, is_remark: true,  sec_level: 0);

            cols.Add(col_1);
            cols.Add(col_2);
            cols.Add(col_3);
            cols.Add(col_4);
            cols.Add(col_5);
            cols.Add(col_6);
            cols.Add(col_7);
            cols.Add(col_8);

            #endregion


            decipher.InsertModel(table);
            decipher.InsertModel(col);
            decipher.InsertModels(cols);


            IG2_TABLE def_table = TmpTableMgr.TempTable2TableCMS(tmpId);

            InserMenu(def_table.IG2_TABLE_ID);


            return def_table.IG2_TABLE_ID;
        }


        /// <summary>
        /// 创建系统字段
        /// </summary>
        /// <returns></returns>
        private List<IG2_TMP_TABLECOL> CreateCols(Guid tableUid, string sessionId, Guid tmpGuid)
        {
            List<IG2_TMP_TABLECOL> cols = new List<IG2_TMP_TABLECOL>();

            IG2_TMP_TABLECOL col;

            col = CreateBizCol("int", "ROW_SID", "记录状态", true, "0", tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("datetime", "ROW_DATE_CREATE", "记录创建时间", true, "(GETDATE())", tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("datetime", "ROW_DATE_UPDATE", "记录更新时间", true, "(GETDATE())", tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("datetime", "ROW_DATE_DELETE", "记录删除时间", false, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("decimal", "ROW_USER_SEQ", "记录用户自定义排序", true, "0", tableUid, sessionId, tmpGuid);
            col.DB_DOT = 6;
            col.DB_LEN = 18;

            cols.Add(col);


            col = CreateBizCol("string", "ROW_STYLE_JSON", "记录行的样式", true, string.Empty, tableUid, sessionId, tmpGuid);
            col.DB_LEN = 18;
            col.SEC_LEVEL = 8;
            col.IS_REMARK = true;
            cols.Add(col);


            #region 记录创建人

            col = CreateBizCol("string", "ROW_AUTHOR_ROLE_CODE", "记录创建角色代码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "ROW_AUTHOR_COMP_CODE", "记录创建公司代码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "ROW_AUTHOR_ORG_CODE", "记录创建部门代码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "ROW_AUTHOR_USER_CODE", "记录创建人员代码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            #endregion

            #region 记录修改人

            col = CreateBizCol("string", "ROW_UPDATE_USER_CODE", "记录更新人员代码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);


            col = CreateBizCol("string", "ROW_DELETE_USER_CODE", "记录删除人员代码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            #endregion





            #region 业务状态

            col = CreateBizCol("int", "BIZ_SID", "业务状态", true, "0", tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "BIZ_ORG_CODE", "业务所属部门编码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "BIZ_USER_CODE", "业务所属人编码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "BIZ_CATA_CODE", "业务所结构编码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);


            col = CreateBizCol("string", "BIZ_UPDATE_USER_CODE", "业务更新人编码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("string", "BIZ_DELETE_USER_CODE", "业务删除人编码", true, string.Empty, tableUid, sessionId, tmpGuid);
            cols.Add(col);

            #endregion



            return cols;
        }

        /// <summary>
        /// 创建表默认参数
        /// </summary>
        private IG2_TMP_TABLE CreateDefaultTable(string display, string sessionId, Guid tmpGuid)
        {
            int catalog_id = 107;

            IG2_TMP_TABLE table = new IG2_TMP_TABLE();
            table.IG2_CATALOG_ID = catalog_id;

            table.DISPLAY = display;

            table.TABLE_UID = Guid.NewGuid();

            table.TABLE_TYPE_ID = "TABLE";
            table.ID_FIELD = "ROW_IDENTITY_ID";
            table.IDENTITY_FIELD = "ROW_IDENTITY_ID";
            table.USER_SEQ_FIELD = "ROW_USER_SEQ";
            table.STYLE_JSON_FIELD = "ROW_STYLE_JSON";

            table.TMP_OP_ID = "A";
            table.TMP_SESSION_ID = sessionId;
            table.TMP_GUID = tmpGuid;

            return table;
        }

        private IG2_TMP_TABLECOL CreateDefaultCol(Guid tableUid, string idField, string sessionId, Guid tmpGuid)
        {
            IG2_TMP_TABLECOL col = new IG2_TMP_TABLECOL();

            col.TABLE_UID = tableUid;
            col.DB_TYPE = "int";
            col.DB_FIELD = idField;
            col.F_NAME = "[数据主键]";
            col.DISPLAY = "ID";

            col.SEC_LEVEL = 4;

            col.IS_READONLY = true;
            col.IS_MANDATORY = true;

            col.TMP_OP_ID = "A";
            col.TMP_SESSION_ID = sessionId;
            col.TMP_GUID = tmpGuid;


            return col;
        }

        /// <summary>
        /// 创建UI 消息提示字段
        /// </summary>
        /// <param name="tableUid"></param>
        /// <param name="idField"></param>
        /// <param name="sessionId"></param>
        /// <param name="tmpGuid"></param>
        /// <returns></returns>
        private IG2_TMP_TABLECOL CreateDefaultUICol(Guid tableUid, string idField, string sessionId, Guid tmpGuid)
        {
            IG2_TMP_TABLECOL col = new IG2_TMP_TABLECOL();

            col.TABLE_UID = tableUid;
            col.DB_TYPE = "int";
            col.DB_FIELD = idField;
            col.F_NAME = "[数据主键]";
            col.DISPLAY = "ID";

            col.SEC_LEVEL = 4;

            col.IS_READONLY = true;


            col.TMP_OP_ID = "A";
            col.TMP_SESSION_ID = sessionId;
            col.TMP_GUID = tmpGuid;


            return col;
        }



        /// <summary>
        /// 创建临时字段
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dbField"></param>
        /// <param name="dispaly"></param>
        /// <param name="isMandatory"></param>
        /// <param name="tableUid"></param>
        /// <param name="sessionId"></param>
        /// <param name="tmpGuid"></param>
        /// <param name="field_len">字段长度 默认 50</param>
        /// <param name="is_remark">是否是大字段</param>
        /// <param name="is_visible">列可视</param>
        /// <param name="sec_level">权限级别 默认 = 6  自定义一般为 0</param>
        /// <param name="is_readonly">只读 默认 true 为只读 </param>
        /// <param name="display_type">显示类型</param>
        /// <returns></returns>
        private IG2_TMP_TABLECOL CreateBizCol(string dbType, string dbField, string dispaly, bool isMandatory, string defaultValue,
            Guid tableUid, string sessionId, Guid tmpGuid,int field_len = 50,bool is_remark = false,bool is_visible =false,int sec_level = 6,bool is_readonly = true,
            string display_type = "")
        {
            IG2_TMP_TABLECOL col = new IG2_TMP_TABLECOL();

            col.TABLE_UID = tableUid;
            col.DB_TYPE = dbType;
            col.DB_FIELD = dbField;
            col.F_NAME = dispaly;

            col.DISPLAY = dispaly;
            col.DISPLAY_TYPE = display_type;
            col.IS_SEARCH_VISIBLE = false;

            col.DEFAULT_VALUE = defaultValue;

            col.IS_MANDATORY = isMandatory;

            col.DB_LEN = field_len;

            col.IS_REMARK = is_remark;

            col.IS_READONLY = is_readonly;
            col.IS_VISIBLE = is_visible;
            col.IS_LIST_VISIBLE = is_visible;
            col.IS_SEARCH_VISIBLE = false;

            col.SEC_LEVEL = sec_level;

            col.TMP_OP_ID = "A";
            col.TMP_SESSION_ID = sessionId;
            col.TMP_GUID = tmpGuid;


            return col;
        }


        /// <summary>
        /// 创建表菜单
        /// </summary>
        /// <param name="id"></param>
        void InserMenu(int id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);

            LModel lm = new LModel("BIZ_C_MENU");
            lm["URI"] = string.Format("/app/infogrid2/view/onetable/tablepreview.aspx?id={0}", id);
            lm["PARENT_ID"] = 80;
            lm["NAME"] = it.DISPLAY;
            lm["MENU_ENABLED"] = true;
            lm["SEC_PAGE_ID"] = id;
            lm["SEC_PAGE_TYPE_ID"] = "TABLE";

            decipher.InsertModel(lm);

        }


    }
}