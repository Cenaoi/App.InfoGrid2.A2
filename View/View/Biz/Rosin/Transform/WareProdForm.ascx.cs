using App.BizCommon;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin.Transform
{
    public partial class WareProdForm : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected override void OnLoad(EventArgs e)
        {
            DbCascadeRule.Bind(this.storeMain1);

            this.storeMain1.Updating += StoreMain1_Updating;

            storeMain1.Updated += StoreMain1_Updated;


            if (!this.IsPostBack)
            {
                this.storeMain1.DataBind();

                this.storeMD1.DataBind();

                OnInitData();
            }

        }

        private void StoreMain1_Updated(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel lm = e.Object as LModel;

            if (lm.GetBlemish("COL_23") || lm.GetBlemish("COL_21"))
            {

                UpdataNum((int)lm["COL_1"]);

            }
        }

        private void StoreMain1_Updating(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model.GetBlemish("COL_3"))
            {
                AutoAdd_UT002(model);
            }

            BizHelper.FullForUpdate(e.Object as LModel);

        }

        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            string title = io_tag == "I" ? "入库货物项目" : "出库货物项目";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_007");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            LModelList<LModel> models = decipher.GetModelList(filter);

            foreach (LModel m in models)
            {
                COL_7_cb.Items.Add(m["CD_TEXT"].ToString());


            }

        }




        /// <summary>
        /// 自动插入数据
        /// </summary>
        private void AutoAdd_UT002(LModel ownerModel)
        {
            bool isRosinSystem = GlobelParam.GetValue<bool>("IS_ROSIN_SYSTEM", false);

            if (!isRosinSystem)
            {
                return;
            }

            LModelElement modelElem = ownerModel.GetModelElement();

            if (modelElem.DBTableName == "UT_002" && modelElem.HasField("COL_3"))
            {
                string clientName = ownerModel.Get<string>("COL_3").Trim();

                if (StringUtil.IsBlank(clientName))
                {
                    return;
                }


                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter("UT_006");
                filter.AddFilter("ROW_SID >= 0");
                filter.And("PROD_TEXT", clientName);


                if (!decipher.ExistsModels(filter))
                {
                    LModel model = new LModel("UT_006");
                    model["PROD_TEXT"] = clientName;

                    decipher.InsertModel(model);
                }

            }
        }


        /// <summary>
        /// 更新主表的数量和金额
        /// </summary>
        /// <param name="id"></param>
        void UpdataNum(int id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm001 = decipher.GetModelByPk("UT_001", id);


            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", id);
            lmFilter.And("IO_TAG", lm001["IO_TAG"]);

            List<LModel> lm002s = decipher.GetModelList(lmFilter);

            lm001["NUM_TOTAL"] = LModelMath.Sum(lm002s, "COL_21");
            lm001["WEIGHT_TOTAL"] = LModelMath.Sum(lm002s, "COL_23");

            decipher.UpdateModelProps(lm001, "NUM_TOTAL", "WEIGHT_TOTAL");
        }
    }
}