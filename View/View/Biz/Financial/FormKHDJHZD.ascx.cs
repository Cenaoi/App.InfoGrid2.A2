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
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Financial
{
    /// <summary>
    /// 客户单据汇总单
    /// </summary>
    public partial class FormKHDJHZD : WidgetControl, IView
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

              

                this.headLab.Value = "<span class='page-head' >客户单据汇总账单</span>";
            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {
            try
            {

                string path = "Biz/Financial/FormKHDJHZD.xml";

                m_cv = CustomViewMgr.GetXml(path);

            }
            catch (Exception ex)
            {

                log.Error("读取自定义配置文件出错了！", ex);

                Error404.Send("供应商单据汇总单", "配置文件读取出错了，请联系系统管理员！");

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

                ///这是产品类型
                string typeID = WebUtil.Form("HType");


                //找到表名自定义参数对象
                var setting = m_cv.appSettings.appSettings.Find(a => a.key == "table_name");

                //拿到表名
                string table_name = setting.value.Trim();



                DbDecipher decipher = ModelAction.OpenDecipher();
               


                DateTime begTime = StringUtil.ToDateTime(this.DateRangePicker1.StartValue, DateUtil.StartByMonth());
                DateTime endTime = StringUtil.ToDateTime(this.DateRangePicker1.EndValue, DateUtil.EndByMonth());

                if (!string.IsNullOrEmpty(this.DateRangePicker1.EndValue))
                {
                    endTime = DateUtil.EndDay(this.DateRangePicker1.EndValue);
                }


                LightModelFilter lmFilter = new LightModelFilter(typeof(FINANCIAL_STATISTICS));
                lmFilter.And("SESSION_ID", Session.SessionID);

                decipher.DeleteModels(lmFilter);



                LightModelFilter lmFilter181 = new LightModelFilter(table_name);
                lmFilter181.And("COL_2", begTime, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                lmFilter181.And("COL_2", endTime, HWQ.Entity.Filter.Logic.LessThanOrEqual);
                lmFilter181.And("COL_36", id);
                lmFilter181.And("COL_1", "收");
                lmFilter181.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                if (!string.IsNullOrEmpty(typeID))
                {
                    lmFilter181.And("COL_39", typeID);
                }

                lmFilter181.TSqlOrderBy = "COL_2 asc";

                List<LModel> lmList = decipher.GetModelList(lmFilter181);




                ////这里是选择时间之前的数据
                LightModelFilter lmFilterBef = new LightModelFilter(table_name);
                lmFilterBef.And("COL_2", begTime, HWQ.Entity.Filter.Logic.LessThan);
                lmFilterBef.And("COL_36", id);
                lmFilterBef.And("COL_1", "收");
                lmFilterBef.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                if (!string.IsNullOrEmpty(typeID))
                {
                    lmFilterBef.And("COL_39", typeID);
                }


                List<LModel> lmListBef = decipher.GetModelList(lmFilterBef);


                ///这是应收款
                decimal rece = 0;
                ///这是预收款
                decimal pre_rece = 0;

                foreach (var item in lmListBef)
                {
                    rece += item.Get<decimal>("COL_22");
                    pre_rece += item.Get<decimal>("COL_23");
                }


                ///这是实际应收款
                decimal aa_rece = rece - pre_rece;

                List<FINANCIAL_STATISTICS> fsList = new List<FINANCIAL_STATISTICS>();

                foreach (var item in lmList)
                {
                    ///这是期末数的应收
                    decimal end_rece = rece + item.Get<decimal>("COL_22");
                    ///这是期末数的预收款
                    decimal end_pre_rece = pre_rece + item.Get<decimal>("COL_23");

                    ///这是期末数的实际应收
                    decimal end_aa_rece = end_rece - end_pre_rece;

                    FINANCIAL_STATISTICS fs = new FINANCIAL_STATISTICS()
                    {
                        CUSTOMER_ID = id,
                        SESSION_ID = Session.SessionID,
                        DATE_TIME = item.Get<DateTime>("COL_2"),
                        F_NO = item.Get<string>("COL_3"),
                        BEG_RECEIVABLES = rece,
                        BEG_PRE_RECEIVABLES = pre_rece,
                        BEG_AARECEIVABLE = aa_rece,
                        ABSTRACT = item.Get<string>("COL_19"),
                        F_NUMBER = item.Get<decimal>("COL_20"),
                        RECEIVABLES = item.Get<decimal>("COL_22"),
                        PRE_RECEIVABLES = item.Get<decimal>("COL_23"),
                        END_RECEIVABLES = end_rece,
                        END_PRE_RECEIVABLES = end_pre_rece,
                        END_AARECEIVABLES = end_aa_rece,
                        REMARKS = item.Get<string>("COL_28"),
                        CREATE_DATE_TIME = DateTime.Now

                    };

                    rece = end_rece;
                    pre_rece = end_pre_rece;
                    aa_rece = end_aa_rece;

                    fsList.Add(fs);
                }


                if (fsList.Count == 0)
                {
                    MessageBox.Alert("没有找到数据！");
                    return;
                }

                decipher.InsertModels<FINANCIAL_STATISTICS>(fsList);

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
            this.DateRangePicker1.EndValue = "";
            this.DateRangePicker1.StartValue = "";

        }

        /// <summary>
        ///  小渔夫加的 管理模板按钮事件
        /// </summary>
        public void ManageTemplate()
        {

            //那个ID是随便给的一个唯一的ID
            string urlStr = "/App/InfoGrid2/View/PrintTemplate/ManageTemplateDZD.aspx?id=1000004";

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

            DateTime begTime = StringUtil.ToDateTime(this.DateRangePicker1.StartValue, DateUtil.StartByMonth());
            DateTime endTime = StringUtil.ToDateTime(this.DateRangePicker1.EndValue, DateUtil.EndByMonth());

            if (!string.IsNullOrEmpty(this.DateRangePicker1.EndValue))
            {
                endTime = DateUtil.EndDay(this.DateRangePicker1.EndValue);
            }


            //那个ID是随便给的一个唯一的ID
            string urlStr = $"/App/InfoGrid2/View/PrintTemplate/PrintTemplateDZD.aspx?id=1000004&name={name}&code={code}&begTime={begTime.ToString("yyyy - MM - dd")}&endTime={endTime.ToString("yyyy - MM - dd")}";

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