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

namespace App.InfoGrid2.View.Biz.Rosin.Transform_V1
{
    /// <summary>
    ///  过户  审核过了就显示这个界面
    /// </summary>
    public partial class WarhouseList_2 : WidgetControl, IView
    {


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();

                store1.DataBind();

            }
        }


        void InitData()
        {

            headLab.Value = "<span class='page-head' >过户单</span>";


            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm018 = decipher.GetModelByPk("UT_018", row_id);

            UpdataViewData(lm018);

        }

        /// <summary>
        /// 更新界面上的一些主表数据
        /// </summary>
        /// <param name="lm018">主表</param>
        void UpdataViewData(LModel lm018)
        {

            cb_ut_018_biz_sid.Value = lm018.Get<string>("BIZ_SID");

            tbx_change_bill_no.Value = lm018.Get<string>("CHANGE_BILL_NO");

            tbx_now_cust_name.Value = lm018.Get<string>("CLIENT_TEXT");

            tbx_old_cust_name.Value = lm018.Get<string>("SRC_CLIENT_TEXT");


        }

    }
}