using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.DbCascade;
using EC5.IG2.BizBase;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.JLSLBZ.Scjhxqzx3
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
            this.StoreUT103.CurrentChanged += StoreUT103_CurrentChanged;
            if (!this.IsPostBack)
            {

                this.StoreUT102.DataBind();
            }
        }

        void StoreUT103_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel lm = (LModel)e.Object;
            if (lm == null) { return; }

            string col_52 = lm.Get<string>("COL_52");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_118");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_15", col_52);

            List<LModel> lmList118 = decipher.GetModelList(lmFilter);

            StringBuilder sb = new StringBuilder();

            //库存合计 
            decimal sum = 0;


            sb.Append("合计：{0}    ");

            foreach (var item in lmList118)
            {
                sum += item.Get<decimal>("COL_9");

                sb.Append(item.Get<string>("COL_11") + ":" + item.Get<string>("COL_9") + "     ");

            }


            string message = string.Format(sb.ToString(), sum);
            this.tbMessage.Text = message;


        }



        void StoreUT102_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model == null)
            {
                return;
            }

            string billNo = model.Get<string>("COL_13");

            this.StoreUT103.FilterParams.Add(new Param("COL_15", billNo));
            this.StoreUT104.FilterParams.Add(new Param("COL_12", billNo));
            this.StoreUT105.FilterParams.Add(new Param("COL_12", billNo));
            this.StoreUT106.FilterParams.Add(new Param("COL_11", billNo));
            this.StoreUT104B.FilterParams.Add(new Param("COL_12", billNo));
            this.StoreUT104C.FilterParams.Add(new Param("COL_12", billNo));
            this.StoreUT216.FilterParams.Add(new Param("COL_15", billNo));

            this.StoreUT103.DataBind();
            this.StoreUT104.DataBind();
            this.StoreUT105.DataBind();
            this.StoreUT106.DataBind();
            this.StoreUT104B.DataBind();
            this.StoreUT104C.DataBind();
            this.StoreUT216.DataBind();
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


        /// <summary>
        /// 采购需求分析按钮事件
        /// </summary>
        public void btnChangeRowSid0_2()
        {
            string id = this.StoreUT103.CurDataId;


            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择一条记录！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModelByPk("UT_103", id);

            if (lm.Get<decimal>("COL_67") == 2)
            {
                MessageBox.Alert("已经分析过！");
                return;
            }

            lm.SetTakeChange(true);

            lm["COL_67"] = 2;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;

            EasyClick.Web.Mini.MiniHelper.Eval("ShowGif()");


            try
            {
                DbCascadeRule.Update(lm);
                
            }
            catch (Exception ex)
            {
   
                log.Error(ex);
                MessageBox.Alert("分析错误！");
            }

        }

        /// <summary>
        /// 撤销需求分析按钮事件
        /// </summary>
        public void btnChangeRowSid2_0()
        {
            string id = this.StoreUT103.CurDataId;


            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择一条记录！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModelByPk("UT_103", id);

            if (lm.Get<decimal>("COL_67") == 0)
            {
                MessageBox.Alert("没有分析过，不用撤销！");
                return;
            }


            lm.SetTakeChange(true);
            lm["COL_67"] = 0;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModel(lm, true);


        }

    }
}