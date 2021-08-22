using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.XmlModel;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Financial
{
    /// <summary>
    /// 客户单据汇总校对单
    /// </summary>
    public partial class KHDJHZJDD : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 自定义配置信息
        /// </summary>
        CustomView m_cv;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            InitData();

            if (!IsPostBack)
            {

              

                this.headLab.Value = "<span class='page-head' >客户单据汇总校对单 </span>";
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {
            try
            {

                string path = "Biz/Financial/KHDJHZJDD.xml";

                m_cv = CustomViewMgr.GetXml(path);

            }
            catch (Exception ex)
            {

                log.Error("读取自定义配置文件出错了！", ex);

                Error404.Send("客户单据汇总校对单", "配置文件读取出错了，请联系系统管理员！");

            }


        }



        /// <summary>
        /// 查询财务信息
        /// </summary>
        public void btnSelect()
        {
            try
            {


                if (string.IsNullOrEmpty(this.tbID.Value))
                {
                    MessageBox.Alert("请选择客户！");
                    return;
                }


                int id = WebUtil.FormInt("HID");


                string[] values = StringUtil.SplitTrim(this.cbmCOL_30.Value,",");
                

                //找到表名自定义参数对象
                var setting = m_cv.appSettings.appSettings.Find(a => a.key == "table_name");

                //拿到表名
                string table_name = setting.value.Trim();





                DbDecipher decipher = ModelAction.OpenDecipher();



                DateTime begTime = this.DateRangePicker1.StartDate?? DateUtil.StartByMonth();
                DateTime endTime = this.DateRangePicker1.EndDate?? DateUtil.EndByMonth();
                                

                LightModelFilter lmFilter = new LightModelFilter(typeof(FINANCIAL_STATISTICS));
                lmFilter.And("SESSION_ID", Session.SessionID);

                decipher.DeleteModels(lmFilter);



                LightModelFilter lmFilter181 = new LightModelFilter(table_name);

                lmFilter181.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
                lmFilter181.And("COL_2", begTime, Logic.GreaterThanOrEqual);
                lmFilter181.And("COL_2", endTime, Logic.LessThanOrEqual);
                lmFilter181.And("COL_36", id);
                lmFilter181.And("COL_1", "收");
                if (values.Length > 0)
                {
                    lmFilter181.And("COL_30",values, Logic.In);

                }



                lmFilter181.TSqlOrderBy = "COL_2 asc";

                List<LModel> lmList = decipher.GetModelList(lmFilter181);


                List<FINANCIAL_STATISTICS> fsList = new List<FINANCIAL_STATISTICS>();

                foreach (var item in lmList)
                {


                    FINANCIAL_STATISTICS fs = new FINANCIAL_STATISTICS()
                    {
                        CUSTOMER_ID = id,
                        SESSION_ID = Session.SessionID,
                        DATE_TIME = item.Get<DateTime>("COL_2"),
                        F_NO = item.Get<string>("COL_3"),
                        ABSTRACT = item.Get<string>("COL_19"),
                        F_NUMBER = item.Get<decimal>("COL_20"),
                        RECEIVABLES = item.Get<decimal>("COL_22"),
                        PRE_RECEIVABLES = item.Get<decimal>("COL_23"),
                        REMARKS = item.Get<string>("COL_28"),
                        CREATE_DATE_TIME = DateTime.Now

                    };


                    fsList.Add(fs);
                }


                if (fsList.Count == 0)
                {
                    MessageBox.Alert("没有找到数据！");
                    return;
                }

                decipher.InsertModels(fsList);

            }
            catch (Exception ex)
            {
                log.Error("出错了！", ex);
                MessageBox.Alert("出错了！");
                return;
            }
            this.store1.DataBind();


        }



        public void btnClear()
        {
            this.tbID.Value = "";
            this.DateRangePicker1.Clear();

        }

        /// <summary>
        ///  小渔夫加的 管理模板按钮事件
        /// </summary>
        public void ManageTemplate()
        {

            //那个ID是随便给的一个唯一的ID
            string urlStr = "/App/InfoGrid2/View/PrintTemplate/ManageTemplateDZD.aspx?id=1000009";

            Window win = new Window("模板管理");
            win.ContentPath = urlStr;
            win.State = WindowState.Max;
            win.ShowDialog();
        }


        /// <summary>
        /// 小渔夫加的  打印按钮事件
        /// </summary>
        public void btnPrint()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.FormInt("HID");

            string name = this.tbID.Value;


            LModel lm071 = decipher.GetModelByPk("UT_071", id);

            if (lm071 == null)
            {
                MessageBox.Alert("请选择客户才能打印！");
                return;
            }

            //客户编码
            string code = lm071.Get<string>("COL_1");

            DateTime begTime = this.DateRangePicker1.StartDate?? DateUtil.StartByMonth();
            DateTime endTime = this.DateRangePicker1.EndDate?? DateUtil.EndByMonth();

            //那个ID是随便给的一个唯一的ID
            string urlStr = $"/App/InfoGrid2/View/PrintTemplate/PrintTemplateDZD.aspx?id=1000009&name={name}&code={code}&begTime={begTime.ToString("yyyy-MM-dd")}&endTime={endTime.ToString("yyyy-MM-dd")}"; 

            Window win = new Window("打印");
            win.ContentPath = urlStr;
            win.State = WindowState.Max;
            win.ShowDialog();


        }


        /// <summary>
        /// 获取展示规则
        /// </summary>
        /// <returns></returns>
        public string GetDisplayRule()
        {
            return App.InfoGrid2.Bll.DisplayRuleMgr.GetJScript();
        }

    }
}