using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Entity.Expanding.ExpandV1;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage.LM
{
    /// <summary>
    /// 给工作组排序日期列表用的
    /// </summary>
    public class T2280_961_Page: ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit()
        {
            Toolbar tbar = this.FindControl("Toolbar1") as Toolbar;

            ToolBarButton tbarBom = new ToolBarButton("生成排程日期");
            tbarBom.OnClick = GetMethodJs("GoCreateSchedulingDate");

            tbar.Items.Add(tbarBom);


            ToolBarButton btnDelete = new ToolBarButton("删除排程日期");
            btnDelete.OnClick = GetMethodJs("GoDeleteAll");

            tbar.Items.Add(btnDelete);



        }

        /// <summary>
        /// 生成排程日期事件
        /// </summary>
        public void GoCreateSchedulingDate()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_439");
            lmFilter.Top = 1;
            lmFilter.TSqlOrderBy = "COL_1 desc";

            LModel lm439 = decipher.GetModel(lmFilter);

            //排程开始日期
            DateTime schedulingBeginDate = DateTime.Now;

            if (lm439 != null)
            {
                schedulingBeginDate = lm439.Get<DateTime>("COL_1").AddDays(1);
            }


            //排程结束日期
            DateTime schedulingEndDate = schedulingBeginDate.AddYears(1);


            BizFilter filter = new BizFilter("UT_428_MESYSTEM_DEPT");
            filter.And("COL_16", true);

            List<LModel> lm428s = decipher.GetModelList(filter);



            List<LModel> lm439s = new List<LModel>();


            while (schedulingBeginDate < schedulingEndDate)
            {

                LModel lm439Item = new LModel("UT_439")
                {
                    ["COL_1"] = new DateTime(schedulingBeginDate.Year, schedulingBeginDate.Month, schedulingBeginDate.Day)
                };


                foreach (var item in lm428s)
                {

                    string col_23 = item.Get<string>("COL_23");

                    //字段
                    string col_27 = item.Get<string>("COL_27");

                    //如果为空就不用添加
                    if (StringUtil.IsBlank(col_27))
                    {
                        continue;
                    }

                    //要有这个字段才能存放数据
                    if (!lm439Item.HasField(col_27))
                    {
                        continue;
                    }


                    filter = new BizFilter("UT_437_CREATION_TIME");
                    filter.And("Work_code", col_23);
                    filter.Top = 1;

                    LModel lm437 = decipher.GetModel(filter);

                    if (lm437 == null)
                    {
                        bool flag = true;

                        if (schedulingBeginDate.DayOfWeek == DayOfWeek.Sunday || schedulingBeginDate.DayOfWeek == DayOfWeek.Saturday)
                        {
                            flag = false;
                        }

                        lm439Item[col_27] = flag;

                        continue;
                    }

                    lm439Item[col_27] = lm437[schedulingBeginDate.DayOfWeek.ToString()];
                    lm439Item["DAY_OF_WEEK"] = schedulingBeginDate.DayOfWeek.ToString();

                }

                schedulingBeginDate = schedulingBeginDate.AddDays(1);

                lm439s.Add(lm439Item);

            }

            decipher.InsertModels(lm439s);


            MainStore.Refresh();



            MessageBox.Alert("生成排程日期成功了！");


        }


        public void GoDeleteAll()
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.ExecuteNonQuery("delete UT_439");


            MessageBox.Alert("删除成功了！");
            
        }



    }
}