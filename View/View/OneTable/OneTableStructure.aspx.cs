using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity;
using System.Text;
using App.InfoGrid2.Bll;
using EC5.IO;

namespace App.InfoGrid2.View.OneTable
{
    /// <summary>
    /// 表结构
    /// </summary>
    public partial class OneTableStructure : System.Web.UI.Page,IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpResult pack = null;

            try
            {
                object data = ProAction();

                pack = HttpResult.Success(data);

                Response.Write(pack);
            }
            catch (Exception ex)
            {                
                log.ErrorFormat("获取数据错误", ex);

                pack = HttpResult.Error("获取数据错误");

            }

            Response.Clear();
            Response.Write(pack);
        }

        private object ProAction()
        {
            string text = WebUtil.QueryTrimUpper("action");


            object result;

            switch (text)
            {
                case "GET_TABLE": result = this.GetTable(); break;
                case "GET_FIELDS": result = this.GetFields(); break;
                default: result = string.Empty; break;
            }

            return result;
        }


        private LModel GetTable()
        {
            string text = WebUtil.Query("table");
            LModel lTableForName = TableMgr.GetLTableForName(text, new string[]
			{
				"IG2_TABLE_ID",
				"TABLE_NAME",
				"DISPLAY"
			});
            return lTableForName;
        }

        private IList<LModel> GetFields()
        {
            string text = WebUtil.Query("table");
            int tableId = TableMgr.GetTableId(text);

            IList<LModel> lCols = TableMgr.GetLCols(tableId, new string[]
			{
				"DB_FIELD",
				"F_NAME",
				"DB_TYPE"
			});

            return lCols;
        }



    }
}