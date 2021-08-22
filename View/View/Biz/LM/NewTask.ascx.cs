using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.Entity.Expanding.ExpandV1;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.LM
{
    public partial class NewTask : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                store1.DataBind();
                store2.DataBind();
            }

        }


        /// <summary>
        /// 生成排程任务按钮点击事件
        /// </summary>
        public void GoCreateTask()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string task_id = store1.CurDataId;

            string dept_id = store2.CurDataId;


            LModel lm425 = decipher.GetModelByPk("UT_425_MESYSTEM_FRONT", task_id); 

            if(lm425 == null)
            {
                MessageBox.Alert("请选择一条任务记录");
                return;
            }

            LModel lm428 = decipher.GetModelByPk("UT_428_MESYSTEM_DEPT", dept_id);

            if(lm428 == null)
            {
                MessageBox.Alert("请选择一个工作组");
                return;
            }


            DateTime begDate = DateTime.Now;


            if (!lm428.IsNull("COL_22"))
            {
                begDate = lm428.Get<DateTime>("COL_22").AddDays(1);
            }


            //字段
            string col_27 = lm428.Get<string>("COL_27");

            if (StringUtil.IsBlank(col_27))
            {
                MessageBox.Alert("占用情况代码字段为空");
                return;
            }

          
            // 工作天数 =  任务 工作量  /  工作组产能  （取整）
            decimal num_day = Math.Ceiling(lm425.Get<decimal>("COL_22") / lm428.Get<decimal>("COL_14"));


            List<LModel> lm440s = new List<LModel>();

            for (int i = 0; i < num_day; i++)
            {

                begDate = begDate.AddDays(i);

                BizFilter filter = new BizFilter("UT_439");
                filter.And("COL_1", new DateTime(begDate.Year, begDate.Month, begDate.Day));
                LModel lm439 = decipher.GetModel(filter);

                if (lm439 == null)
                {
                    MessageBox.Alert($"还没有生成【{begDate.ToShortDateString()}】这一天的工作组工作安排");
                    return;
                }


                if (!lm439.HasField(col_27))
                {
                    MessageBox.Alert("占用情况代码配置出错了");
                    return;
                }


                bool flag = lm439[col_27];


                LModel lm440 = new LModel("UT_440")
                {
                    ["T_DATE"] = new DateTime(begDate.Year, begDate.Month, begDate.Day),
                    ["FK_DEPT_ID"] = dept_id,
                    ["D_TEXT"] = lm428["COL_4"],
                    ["FK_DEPT_CODE"] = lm428["COL_3"],
                    ["BIZ_SID"] = 0,
                    ["IS_MANUAL_CHANGE"] = false

                };


                //只有可以排程日期才插入这些数据
                if (flag)
                {
                    // biz_sid 0 -- 休假  2 -- 计划中 999 -- 完成 
                    lm440["BIZ_SID"] = 2;
                    lm440["FK_TASK_CODE"] = lm425["COL_4"];
                    lm440["FK_TASK_ID"] = task_id;
                    lm440["T_TEXT"] = lm425["COL_12"];


                }

                lm440s.Add(lm440);

            }

            decipher.BeginTransaction();

            try
            {

                decipher.InsertModels(lm440s);
                lm425["BIZ_SID"] = 4;
                lm425["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.UpdateModelProps(lm425, "BIZ_SID", "ROW_DATE_UPDATE");

                lm428["COL_22"] = begDate;
                lm428["ROW_DATE_UPDATE"] = DateTime.Now;



                decipher.UpdateModelProps(lm428, "COL_22", "ROW_DATE_UPDATE");


                decipher.TransactionCommit();


                MessageBox.Alert("生成排程任务成功了");

            }catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

                MessageBox.Alert("哦噢，出错了喔");

            }
            



        }


        /// <summary>
        /// 显示日历界面
        /// </summary>
        public void GoShowCalendar()
        {

            Window win = new Window("日历");
            win.ContentPath = $"/App/InfoGrid2/View/Biz/LM/ShowCalendar.aspx?{RandomUtil.Next()}";
            win.State = WindowState.Max;
            win.ShowDialog();

        }


    }
}