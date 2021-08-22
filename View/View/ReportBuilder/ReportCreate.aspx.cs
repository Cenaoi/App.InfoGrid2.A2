using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.Utility.Web;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class ReportCreate : System.Web.UI.Page, EC5.SystemBoard.Interfaces.IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("id");

            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);

            List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and ROW_SID >=0", id);

            if(it == null)
            {
                Response.Write("找不到数据表！");
                return;
            }

            IG2_TMP_TABLE itt = new IG2_TMP_TABLE();
            it.CopyTo(itt,true);

            itt.TMP_GUID = Guid.NewGuid();
            itt.IG2_TABLE_ID = id;
            itt.TMP_OP_ID = "E";
            itt.TMP_SESSION_ID = this.Session.SessionID;


            foreach (IG2_TABLE_COL col in colList)
            {
                IG2_TMP_TABLECOL item = new IG2_TMP_TABLECOL();
                col.CopyTo(item, true);

                item.TMP_GUID = itt.TMP_GUID;
                item.TMP_SESSION_ID = itt.TMP_SESSION_ID;
                item.ROW_DATE_CREATE = item.ROW_DATE_UPDATE = DateTime.Now;


                decipher.InsertModel(item);
            }


            decipher.InsertModel(itt);

            Response.Redirect(string.Format("/View/ReportBuilder/EditCrossReport.html?id={0}", itt.TMP_GUID));
        }
    }
}