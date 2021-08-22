using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class NewStep2 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid reulID = WebUtil.QueryGuid("id");

            if(reulID == null)
            {
                Response.Redirect("/Error.aspx");
            }


        }

        /// <summary>
        /// 从选择窗口拿到表
        /// </summary>
        public void SetTableName(int id)
        {
            try
            {
                Guid reulID = WebUtil.QueryGuid("id");

                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_TMP_TABLE rule = decipher.SelectToOneModel<IG2_TMP_TABLE>("TMP_GUID='{0}' and TMP_SESSION_ID='{1}'", reulID,this.Session.SessionID);

                IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);
               

                if (it == null || rule == null)
                {
                    MessageBox.Alert("没有选择表");
                    return;
                }

               


                this.tbxTableName.Value = it.TABLE_NAME;
                this.TextBox1.Value = it.DISPLAY;
                this.tbxTableID.Value = id.ToString();

                rule.TABLE_NAME = it.TABLE_NAME;
                rule.VIEW_OWNER_TABLE_ID = id;
                

                decipher.UpdateModelProps(rule, "TABLE_NAME", "VIEW_OWNER_TABLE_ID");



            }
            catch (Exception ex)
            {
                log.Error("选择表出错了！", ex);
                MessageBox.Alert("选择表出错了！");
                return;
            }


        }


        /// <summary>
        /// 显示编辑列界面
        /// </summary>
        public void GoNext() 
        {
            Guid reulID = WebUtil.QueryGuid("id");
            if(string.IsNullOrEmpty(this.tbxTableName.Value))
            {
                MessageBox.Alert("请选择数据表！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TMP_TABLE rule = decipher.SelectToOneModel<IG2_TMP_TABLE>("TMP_GUID='{0}' and TMP_SESSION_ID='{1}'", reulID, this.Session.SessionID);

            List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and ROW_SID >=0", this.tbxTableID.Value);

            foreach (IG2_TABLE_COL col in colList)
            {
                IG2_TMP_TABLECOL item = new IG2_TMP_TABLECOL();
                col.CopyTo(item, true);

                item.TMP_GUID = rule.TMP_GUID;
                item.TMP_SESSION_ID = rule.TMP_SESSION_ID;
                item.ROW_DATE_CREATE = item.ROW_DATE_UPDATE = DateTime.Now;


                decipher.InsertModel(item);
            }


            //string url = string.Format("/App/InfoGrid2/View/ReportBuilder/EdiorCell.aspx?id={0}", reulID); 之前的代码

            string url = string.Format("/View/ReportBuilder/EditCrossReport.html?id={0}", reulID);

            MiniPager.Redirect(url);
        }


        /// <summary>
        /// 跳转到上一步
        /// </summary>
        public void GoPre() 
        {
            Guid reulID = WebUtil.QueryGuid("id");

            string url = string.Format("/App/InfoGrid2/View/ReportBuilder/NewStep1.aspx?id={0}", reulID);
            MiniPager.Redirect(url);

        }



    }
}