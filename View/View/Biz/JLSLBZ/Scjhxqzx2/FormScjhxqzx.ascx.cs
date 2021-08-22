using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.Filter;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.Biz.JLSLBZ.Scjhxqzx2
{
    public partial class FormScjhxqzx : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.StoreUT102.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(StoreUT102_CurrentChanged);

            #region 特殊处理“COL_56 = '多客户'”

            string c56_filter = WebUtil.Query("c56_filter");

            if (c56_filter == "mc")
            {
                this.StoreUT102.FilterParams.Add("COL_56", "多客户");
            }

            #endregion

            if (!this.IsPostBack)
            {
                this.StoreUT102.DataBind();
            }
        }



        void StoreUT102_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model == null)
            {
                return;
            }

            string billNo = model.Get<string>("COL_13");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", billNo);

            LModel lm = decipher.GetModel(filter);
            string text = EasyClick.Web.Mini2.Data.IfUtil.NotBlank(lm["COL_6"], "");
             this.RemarkTB.Value = text;

            this.StoreUT103.FilterParams.Add(new Param("COL_15", billNo));
            this.StoreUT104.FilterParams.Add(new Param("COL_12", billNo));
            this.StoreUT105.FilterParams.Add(new Param("COL_12", billNo));
            this.StoreUT106.FilterParams.Add(new Param("COL_12", billNo));


            this.StoreUT103.DataBind();
            this.StoreUT104.DataBind();
            this.StoreUT105.DataBind();
            this.StoreUT106.DataBind();

        }

        /// <summary>
        /// 自动关联
        /// </summary>
        public void GoAutoLink()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            int count = 0;

            try
            {
                LModelList<LModel> models = decipher.GetModelList(filter);


                foreach (LModel model in models)
                {

                    string billCode = (string)model["COL_17"];

                    LightModelFilter filterSub = new LightModelFilter("UT_101");
                    filterSub.And("COL_1", billCode);
                    filterSub.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                    LModel subModel = decipher.GetModel(filterSub);

                    if (subModel != null)
                    {
                        model["COL_13"] = subModel["ROW_IDENTITY_ID"];  //关联主键

                        subModel["COL_5"] = model["COL_16"];    //单据类型

                        count += decipher.UpdateModelProps(model, "COL_13");
                        decipher.UpdateModelProps(subModel, "COL_5");
                    }
                }

                MessageBox.Alert(string.Format("共更新 {0} 条记录", count));
            }
            catch (Exception ex)
            {
                log.Error("更新自动关联失败", ex);
                MessageBox.Alert(string.Format("更新过程失败，只更新了 {0} 条记录", count));
            }

        }

    }
}