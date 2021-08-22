using App.BizCommon;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using EC5.Web.WebAPI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.LM
{
    public partial class ShowCalendar : WidgetControl, IView, IBllAction
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

            store1.Filtering += Store1_Filtering;

        

        }

        private void Store1_Filtering(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            string hid_day = hidDay.Value;

            if (DateTime.TryParse(hid_day, out DateTime cur_day))
            {

                store1.FilterParams.Add("T_DATE", cur_day);

            }

        }


        /// <summary>
        /// 根据用户ID获取工单数据
        /// </summary>
        /// <returns></returns>
        [Ajax("GET_DATA_BY_MONTH")]
        HttpResult GetDataByMonth()
        {

            //年份
            int year = WebUtil.FormInt("year");
            //月份
            int month = WebUtil.FormInt("month");

            if(StringUtil.IsBlank(year) || StringUtil.IsBlank(month))
            {

                return HttpResult.Error("年份或者月份不能为空");
            }


            //这个月开始时间
            DateTime beg_time = new DateTime(year, month, 1);

            //下个月开始时间
            DateTime end_time = new DateTime(year, month + 1, 1);


            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter("UT_440");
            filter.And("T_DATE", beg_time, Logic.GreaterThanOrEqual);
            filter.And("T_DATE", end_time, Logic.LessThan);


            SModelList sms = decipher.GetSModelList(filter);

            return HttpResult.Success(sms);

        }

        void IBllAction.Init()
        {
            
        }


    }
}