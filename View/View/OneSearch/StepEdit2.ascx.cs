using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;


namespace App.InfoGrid2.View.OneSearch
{
    public partial class StepEdit2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";


            base.OnInit(e);

        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            BindEnumFieldList();

            BindFieldList();
        }


        protected override void OnLoad(EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 绑定枚举的字段
        /// </summary>
        private void BindEnumFieldList()
        {
            int viewId = WebUtil.QueryInt("view_id");

            SelectColumn select1 = table1.Columns.FindByDataField("ENUM_VALUE_FIELD") as SelectColumn;
            SelectColumn select2 = table1.Columns.FindByDataField("ENUM_TEXT_FIELD") as SelectColumn;



            DbDecipher decipher = ModelAction.OpenDecipher();

            IList<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} AND SEC_LEVEL < 6 AND ROW_SID >=0", viewId);

            ListItem itemNA = new ListItem();
            itemNA.TextEx = "--N/A--";

            select1.Items.Add(itemNA);

            itemNA = new ListItem();
            itemNA.TextEx = "--N/A--";

            select2.Items.Add(itemNA);


            foreach (IG2_TABLE_COL col in cols)
            {
                select1.Items.Add(col.DB_FIELD, col.DISPLAY);
                select2.Items.Add(col.DB_FIELD, col.DISPLAY);
            }

        }


        /// <summary>
        /// 绑定字段
        /// </summary>
        private void BindFieldList()
        {
            int tableId = WebUtil.QueryInt("owner_table_id");


            SelectColumn selectCol = table2.Columns.FindByDataField("EVENT_AFTER_FIELD_ID") as SelectColumn;

            if (selectCol == null)
            {
                throw new Exception("下拉框不存在.");
            }



            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE sview = decipher.SelectModelByPk<IG2_TABLE>(tableId);

            List<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} AND SEC_LEVEL < 6", tableId);

            ListItem itemNA = new ListItem();
            itemNA.TextEx = "--N/A--";

            selectCol.Items.Add(itemNA);

            foreach (IG2_TABLE_COL item in cols)
            {
                selectCol.Items.Add(item.DB_FIELD, item.F_NAME);
            }

        }


        public void GoPre()
        {
            int tableId = WebUtil.QueryInt("owner_table_id");
            string colId = WebUtil.Query("owner_col_id");

            //owner_type_id : 当前表类型[TABLE | VIEW | PAGE]
            //owner_table_id : 当前表的 ID ,主键值 IG2_TABLE_ID
            //owner_col_id : 当前是由那个字段管理的. 字段名
            string urlStr = string.Format("/App/InfoGrid2/view/OneSearch/StepNew1.aspx?owner_type_id={0}&owner_table_id={1}&owner_col_id={2}",
                "TABLE", tableId, colId);

            MiniPager.Redirect(urlStr);

        }


        public void GoLast()
        {
            string owner_type_Id = WebUtil.Query("owner_type_id");
            int owner_Table_Id = WebUtil.QueryInt("owner_table_id");
            string owner_Col_Id = WebUtil.Query("owner_col_id");

            //Guid tmpGuid = WebUtil.QueryGuid("TMP_GUID");

            //string sessionId = this.Session.SessionID;


            //DbDecipher decipher = ModelAction.OpenDecipher();

            //TmpTableSet ttSet = new TmpTableSet();
            //ttSet.Select(decipher, tmpGuid);

            //TableSet tSet = ttSet.ToTableSet();
            //tSet.Table.TABLE_TYPE_ID = "VIEW";
            //tSet.Table.VIEW_OWNER_TABLE_ID = owner_Table_Id;
            //tSet.Table.VIEW_OWNER_COL_ID = owner_Col_Id;


            //tSet.Insert(decipher);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close();");

        }

    }
}