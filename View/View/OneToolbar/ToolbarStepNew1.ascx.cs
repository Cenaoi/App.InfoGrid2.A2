using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;

using EC5.Utility.Web;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using EC5.Utility;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneToolbar
{
    public partial class ToolbarStepNew1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

            }
        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            int table_id = WebUtil.QueryInt("table_id");


            try
            {


                IG2_TOOLBAR tool = new IG2_TOOLBAR();

                tool.ROW_SID = 0;

                tool.DISPLAY = "视图按钮";
                tool.TABLE_ID = table_id;

                DbDecipher decipher = ModelAction.OpenDecipher();


                int rel = decipher.InsertModel(tool);


                //NEW（新增） EDIT（编辑） DELETE （删除） REFRESH（刷新）

                string[] ckvlas = StringUtil.Split(checkboxGroup1.Value, ",");

                foreach (string type in ckvlas)
                {
                    string btntext = GetBtnText(type);

                    IG2_TOOLBAR_ITEM btn = GetNewItem(table_id, tool.IG2_TOOLBAR_ID, btntext, "SYS", type);

                    decipher.InsertModel(btn);
                }


                string src_url = WebUtil.Query("src_url");

                MiniPager.Redirect(string.Format("SetToolbar.aspx?id={0}&table_id={1}&src_url={2}", tool.IG2_TOOLBAR_ID, table_id,src_url));

            }
            catch (Exception ex)
            {
                log.Error("创建工具栏失败。", ex);
                MessageBox.Alert("创建工具栏失败。");
            }
        }

        public void GoLast()
        {

            string urlCode = WebUtil.QueryBase64("src_url");

            if (!StringUtil.IsBlank(urlCode))
            {
                MiniPager.Redirect(urlCode);
            }
            else
            {
                int tableId = WebUtil.QueryInt("table_id");

                MiniPager.Redirect("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=" + tableId);
            }
        }

        string GetBtnText(string type)
        {
            switch (type)
            {
                case "INSERT":
                    return "新增";
                case "SAVE":
                    return "保存";
                case "DELETE":
                    return "删除";
                case "REFRESH":
                    return "刷新";
                case "TO_EXCEL":
                    return "导出Excel";
                case "SEARCH":
                    return "查询";
            }

            return "";
        }
        

        private IG2_TOOLBAR_ITEM GetNewItem(int table_id, int IG2_TOOLBAR_ID,string btn_text, string event_type, string sys_event_type)
        {
            IG2_TOOLBAR_ITEM btn = new IG2_TOOLBAR_ITEM()
            {
                TABLE_ID = table_id,
                IG2_TOOLBAR_ID = IG2_TOOLBAR_ID,
                ITEM_TYPE_ID = "BTN",
                EVENT_MODE_ID = event_type,
                ITEM_ID = sys_event_type,
                DISPLAY_MODE_ID = "DEFAULT", ITEM_TEXT=btn_text,
                ICON_ID="0",
                ENABLED = true,
                VISIBLE = true
            }; 

            return btn;
        }
    }
}