using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EC5.Utility;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.View.OneSearch
{
    public partial class StepNew2 : WidgetControl, IView
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

        /// <summary>
        /// 绑定枚举的字段
        /// </summary>
        private void BindEnumFieldList()
        {
            Guid TMP_GUID= WebUtil.QueryGuid("TMP_GUID"); 

            //int viewId = WebUtil.QueryInt("view_id");

            SelectColumn select1 = table1.Columns.FindByDataField("ENUM_VALUE_FIELD") as SelectColumn;
            SelectColumn select2 = table1.Columns.FindByDataField("ENUM_TEXT_FIELD") as SelectColumn;



            DbDecipher decipher = ModelAction.OpenDecipher();

            IList<IG2_TMP_TABLECOL> cols = decipher.SelectModels<IG2_TMP_TABLECOL>(LOrder.By("FIELD_SEQ ASC,IG2_TMP_TABLECOL_ID ASC"), 
                "TMP_GUID='{0}' AND SEC_LEVEL <= 6 AND ROW_SID >=0", TMP_GUID);

            ListItem itemNA = new ListItem();
            itemNA.TextEx = "--N/A--";

            select1.Items.Add(itemNA);

            itemNA = new ListItem();
            itemNA.TextEx = "--N/A--";

            select2.Items.Add(itemNA);


            foreach (IG2_TMP_TABLECOL col in cols)
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

            List<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>(
                LOrder.By("FIELD_SEQ ASC,IG2_TABLE_COL_ID ASC"), 
                "IG2_TABLE_ID={0} AND SEC_LEVEL <= 6 AND ROW_SID >= 0", tableId);

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
            string owner_type_Id = WebUtil.Query("owner_type_id");
            int owner_Table_Id = WebUtil.QueryInt("owner_table_id");
            string owner_Col_Id = WebUtil.Query("owner_col_id");

            Guid tmpGuid = WebUtil.QueryGuid("TMP_GUID");

            int curId = StringUtil.ToInt32(this.store1.CurDataId, -1);

            string sessionId = this.Session.SessionID;



            DbDecipher decipher = ModelAction.OpenDecipher();

            TmpTableSet.Delete(decipher, tmpGuid);


            string url = string.Format("StepNew1.aspx?owner_type_id={0}&owner_table_id={1}&owner_col_id={2}&TMP_GUID={3}",
                owner_type_Id, owner_Table_Id, owner_Col_Id, tmpGuid);

            MiniPager.Redirect(url);
        }

        public void GoLast()
        {
            string owner_type_Id = WebUtil.Query("owner_type_id");
            int owner_Table_Id = WebUtil.QueryInt("owner_table_id");
            string owner_Col_Id = WebUtil.Query("owner_col_id");

            Guid tmpGuid = WebUtil.QueryGuid("TMP_GUID");

            string sessionId = this.Session.SessionID;


            DbDecipher decipher = ModelAction.OpenDecipher();

            TmpTableSet ttSet = TmpTableSet.Select(decipher, tmpGuid);

            TableSet tSet = ttSet.ToTableSet();
            tSet.Table.TABLE_TYPE_ID = "VIEW";
            tSet.Table.VIEW_OWNER_TABLE_ID = owner_Table_Id;
            tSet.Table.VIEW_OWNER_COL_ID = owner_Col_Id;

            
            tSet.Insert(decipher);



            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', view_id:" + tSet.Table.IG2_TABLE_ID + ",table_name: '"+ tSet.Table.TABLE_NAME +"'});");

            

        }

        /// <summary>
        /// (未启用...可能挺麻烦的) 清理无关联引用的数据表
        /// </summary>
        private void DeleteViewOwner(int ownerTableId,string ownerColId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_TYPE_ID", "VIEW");
            filter.And("VIEW_OWNER_TABLE_ID", ownerTableId);
            filter.And("VIEW_OWNER_COL_ID", ownerColId);
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

            int count = decipher.SelectCount(filter);

            //如果超过1个引用这个视图表,那么就取消删除
            if (count > 1)
            {
                return;
            }

            TableSet tSet = new TableSet();
            tSet.DeleteRecycle(decipher, ownerTableId, ownerColId);
        }

    }
}