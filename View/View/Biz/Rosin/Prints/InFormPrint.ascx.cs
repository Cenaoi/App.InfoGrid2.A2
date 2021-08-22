using App.BizCommon;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Rosin.Prints
{
    public partial class InFormPrint : WidgetControl, IView
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


        /// <summary>
        /// 获取单头
        /// </summary>
        /// <returns></returns>
        public LModel GetBill()
        {
            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel model001 = decipher.GetModelByPk("UT_016", row_id);

            return model001;

        }

        /// <summary>
        /// 获取货物明细
        /// </summary>
        /// <returns></returns>
        public LModelList<LModel> GetProds()
        {
            int row_id = WebUtil.QueryInt("row_id");

            //string pMode = WebUtil.QueryLower("p_mode");

            //int startI = WebUtil.QueryInt("p_start") - 1;
            //int endI = WebUtil.QueryInt("p_end") - 1;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_017_PROD");
            filter.AddFilter("ROW_SID >=0");
            filter.And("DOC_PARENT_ID", row_id);
            filter.And("IO_TAG", "I");

            //if (pMode != "all")
            //{
            //    filter.Limit = new Limit(endI - startI + 1, startI);
            //}

            LModelList<LModel> model001 = decipher.GetModelList(filter);

            return model001;

        }
    }
}