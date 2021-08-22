using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class NewStep1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitData();
            }
        }


        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext() 
        {
            string name = this.TB1.Value;
            if(string.IsNullOrEmpty(name))
            {
                MessageBox.Alert("请输入交叉报表名称！");
                return;
            }

            int id = WebUtil.QueryInt("catalog_id");

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                IG2_TMP_TABLE tables = new IG2_TMP_TABLE()
                {
                    DISPLAY = name,
                    TABLE_TYPE_ID = "CROSS_TABLE",
                    ROW_SID = -3,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_DATE_UPDATE = DateTime.Now,
                    TMP_GUID = Guid.NewGuid(),
                    TMP_SESSION_ID = this.Session.SessionID,
                    IG2_CATALOG_ID = id
                };


                decipher.InsertModel(tables);


                MiniPager.Redirect("/App/InfoGrid2/View/ReportBuilder/NewStep2.aspx?id="+tables.TMP_GUID);



            }
            catch (Exception ex) 
            {
                log.Error("插入交叉报表数据出错了！",ex);
                MessageBox.Alert("哎呦，出错了喔！");
            }





        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {
            Guid reulID = WebUtil.QueryGuid("id");




            DbDecipher decipher = ModelAction.OpenDecipher();
            IG2_TMP_TABLE tables = decipher.SelectToOneModel<IG2_TMP_TABLE>("TMP_GUID='{0}' and TMP_SESSION_ID='{1}'", reulID,this.Session.SessionID);
            if (tables == null)
            {
                return;
            }

            this.TB1.Value = tables.DISPLAY;


        }


    }
}