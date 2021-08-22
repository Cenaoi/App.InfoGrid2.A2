using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;


namespace App.InfoGrid2.View.Biz.Financial
{
    /// <summary>
    /// 这是金邦的商品对账单 
    /// </summary>
    public partial class FormSPDZD_JB : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.headLab.Value = "<span class='page-head' >商品对账单</span>";
            }
        }

        /// <summary>
        /// 查询财务信息
        /// </summary>
        public void btnSelect()
        {
            try
            {


                if (string.IsNullOrEmpty(this.tbxID.Value))
                {
                    MessageBox.Alert("请选择产品！");
                    return;
                }


                int id = WebUtil.FormInt("HID");

                string code = WebUtil.Form("HWarehouse");


                ///这是选择仓库节点下面的子节点所有编码
                StringBuilder codes = new StringBuilder();


                DbDecipher decipher = ModelAction.OpenDecipher();
                LightModelFilter lmFilter087 = new LightModelFilter("UT_087");
                lmFilter087.And("COL_1", code);
                lmFilter087.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                LModel lm = decipher.GetModel(lmFilter087);


                if (lm != null)
                {
                    CodeRecursive(codes, lm);

                }

                if (codes.Length == 0)
                {
                    codes.Append(code);
                }


                DateTime begTime = this.DateRangePicker1.StartDate ?? DateUtil.StartByMonth();
                DateTime endTime = this.DateRangePicker1.EndDate ?? DateUtil.EndByMonth();



                LightModelFilter lmFilter = new LightModelFilter(typeof(FINANCIAL_STATISTICS));
                lmFilter.And("SESSION_ID", Session.SessionID);

                decipher.DeleteModels(lmFilter);



                LightModelFilter lmFilter181 = new LightModelFilter("UT_117");
                lmFilter181.And("ROW_DATE_UPDATE", begTime, Logic.GreaterThanOrEqual);
                lmFilter181.And("ROW_DATE_UPDATE", endTime, Logic.LessThanOrEqual);
                lmFilter181.And("COL_25", id);
                lmFilter181.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                if (!string.IsNullOrEmpty(tbWarehouse.Value))
                {

                    lmFilter181.And("COL_43", codes.ToString(), HWQ.Entity.Filter.Logic.In);
                }


                lmFilter181.TSqlOrderBy = "ROW_DATE_UPDATE asc";

                List<LModel> lmList = decipher.GetModelList(lmFilter181);




                ////这里是选择时间之前的数据
                LightModelFilter lmFilterBef = new LightModelFilter("UT_181");
                lmFilterBef.And("ROW_DATE_UPDATE", begTime, HWQ.Entity.Filter.Logic.LessThan);
                lmFilterBef.And("COL_25", id);
                lmFilterBef.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                if (!string.IsNullOrEmpty(tbWarehouse.Value))
                {
                    lmFilterBef.And("COL_43", codes.ToString(), HWQ.Entity.Filter.Logic.In);
                }



                List<LModel> lmListBef = decipher.GetModelList(lmFilterBef);


                ///期初数量
                decimal rece = 0;
                ///期初金额
                decimal pre_rece = 0;

                foreach (var item in lmListBef)
                {

                    rece += item.Get<decimal>("COL_11") - item.Get<decimal>("COL_15");
                    pre_rece += item.Get<decimal>("COL_12") - item.Get<decimal>("COL_16");
                }




                List<FINANCIAL_STATISTICS> fsList = new List<FINANCIAL_STATISTICS>();

                foreach (var item in lmList)
                {
                    ///这是期末数量  =   期初数量 + 进仓数量 -  出仓数量
                    decimal end_rece = rece + item.Get<decimal>("COL_11") - item.Get<decimal>("COL_15");
                    ///这是期末金额 = 期初金额 + 进仓金额 - 出仓金额
                    decimal end_pre_rece = pre_rece + item.Get<decimal>("COL_12") - item.Get<decimal>("COL_16");


                    FINANCIAL_STATISTICS fs = new FINANCIAL_STATISTICS()
                    {
                        CUSTOMER_ID = id,
                        SESSION_ID = Session.SessionID,
                        DATE_TIME = item.Get<DateTime>("ROW_DATE_UPDATE"),
                        BEG_RECEIVABLES = rece,
                        BEG_PRE_RECEIVABLES = pre_rece,
                        ABSTRACT = item.Get<string>("COL_30"),
                        END_RECEIVABLES = end_rece,
                        END_PRE_RECEIVABLES = end_pre_rece,
                        CREATE_DATE_TIME = DateTime.Now,
                        NUMBER_IN = item.Get<decimal>("COL_11"),
                        MONERY_IN = item.Get<decimal>("COL_12"),
                        NUMBER_OUT = item.Get<decimal>("COL_15"),
                        MONERY_OUT = item.Get<decimal>("COL_16"),
                        WAREHOUSE_CODE = item.Get<string>("COL_26"),
                        WAREHOUSE_NAME = item.Get<string>("COL_13")

                    };

                    rece = end_rece;
                    pre_rece = end_pre_rece;


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

        /// <summary>
        /// 递归拿到下面仓库节点
        /// </summary>
        /// <param name="sb">所有节点编码</param>
        /// <param name="lm">上级</param>
        void CodeRecursive(StringBuilder sb, LModel lm)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter087 = new LightModelFilter("UT_087");
            lmFilter087.And("COL_9", lm.Get<int>("ROW_IDENTITY_ID"));
            lmFilter087.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            List<LModel> lm087List = decipher.GetModelList(lmFilter087);

            if (lm087List.Count == 0)
            {
                return;
            }

            for (int i = 0; i < lm087List.Count; i++)
            {
                if (i > 0 || sb.Length > 0)
                {
                    sb.Append(",");
                }

                LModel lm087 = lm087List[i];


                string code = lm087.Get<string>("COL_1");

                sb.Append(code);

                CodeRecursive(sb, lm087);


            }

        }
        /// <summary>
        /// 这是清空查询条件
        /// </summary>
        public void btnClear()
        {
            this.tbxID.Value = "";
            this.tbWarehouse.Value = "";
            this.DateRangePicker1.EndValue = "";
            this.DateRangePicker1.StartValue = "";
            this.tbName.Value = "";

        }

        /// <summary>
        ///  小渔夫加的 管理模板按钮事件
        /// </summary>
        public void ManageTemplate()
        {

            //那个ID是随便给的一个唯一的ID
            string urlStr = "/App/InfoGrid2/View/PrintTemplate/ManageTemplateDZD.aspx?id=1000006";

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

            //那个ID是随便给的一个唯一的ID
            string urlStr = "/App/InfoGrid2/View/PrintTemplate/PrintTemplateDZD.aspx?id=1000006";

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