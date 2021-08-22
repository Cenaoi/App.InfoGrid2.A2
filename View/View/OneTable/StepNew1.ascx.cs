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

namespace App.InfoGrid2.View.OneTable
{
    /// <summary>
    /// 创建数据表
    /// </summary>
    public partial class StepNew1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void GoNext()
        {
            if (StringUtil.IsBlank(TB1.Value))
            {
                this.TB1.MarkInvalid("请填写工作表名");
                return;
            }

            

            int categoryId = WebUtil.QueryInt("");

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {

                string sessionId = this.Session.SessionID;
                Guid tmpId = Guid.NewGuid();

                IG2_TMP_TABLE table = CreateDefaultTable(TB1.Value, sessionId, tmpId);
                IG2_TMP_TABLECOL col = CreateDefaultCol(table.TABLE_UID, table.ID_FIELD, sessionId, tmpId);

                List<IG2_TMP_TABLECOL> cols = CreateCols(table.TABLE_UID, sessionId, tmpId);

                decipher.InsertModel(table);
                decipher.InsertModel(col);
                decipher.InsertModels<IG2_TMP_TABLECOL>(cols);

                MiniPager.Redirect("StepNew2.aspx?tmp_id=" + tmpId.ToString());

            }
            catch (Exception ex)
            {
                log.Error(ex);
                
                MessageBox.Alert("不能创建表：" + ex.Message);
            }


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

            col = CreateBizCol("datetime", "ROW_DATE_CREATE", "记录创建时间", true,"(GETDATE())", tableUid, sessionId, tmpGuid);
            cols.Add(col);

            col = CreateBizCol("datetime", "ROW_DATE_UPDATE", "记录更新时间", true,"(GETDATE())", tableUid, sessionId, tmpGuid);
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
            int catalog_id = WebUtil.QueryInt("catalog_id",101);

            IG2_TMP_TABLE table = new IG2_TMP_TABLE();
            table.IG2_CATALOG_ID = catalog_id;

            table.DISPLAY = this.TB1.Value.Trim();

            table.TABLE_UID = Guid.NewGuid();

            table.TABLE_TYPE_ID = "TABLE";
            table.ID_FIELD = "ROW_IDENTITY_ID";
            table.IDENTITY_FIELD = "ROW_IDENTITY_ID";
            table.USER_SEQ_FIELD = "ROW_USER_SEQ";
            table.STYLE_JSON_FIELD = "ROW_STYLE_JSON";
            
            table.TMP_OP_ID = "A";
            table.TMP_SESSION_ID = sessionId;
            table.TMP_GUID = tmpGuid;

            table.REMARK = this.ExTableTB2.Value.Trim();

            return table;
        }

        private IG2_TMP_TABLECOL CreateDefaultCol(Guid tableUid,string idField, string sessionId,Guid tmpGuid)
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
        /// <returns></returns>
        private IG2_TMP_TABLECOL CreateBizCol(string dbType, string dbField, string dispaly, bool isMandatory, string defaultValue,
            Guid tableUid, string sessionId, Guid tmpGuid)
        {
            IG2_TMP_TABLECOL col = new IG2_TMP_TABLECOL();

            col.TABLE_UID = tableUid;
            col.DB_TYPE = dbType;
            col.DB_FIELD = dbField;
            col.F_NAME = dispaly;

            col.DISPLAY = dispaly;
            col.IS_SEARCH_VISIBLE = false;
            
            col.DEFAULT_VALUE = defaultValue;

            col.IS_MANDATORY = isMandatory;

            col.IS_READONLY = true;
            col.IS_VISIBLE = false;
            col.IS_LIST_VISIBLE = false;
            col.IS_SEARCH_VISIBLE = false;

            col.SEC_LEVEL = 6;

            col.TMP_OP_ID = "A";
            col.TMP_SESSION_ID = sessionId;
            col.TMP_GUID = tmpGuid;


            return col;
        }



    }
}