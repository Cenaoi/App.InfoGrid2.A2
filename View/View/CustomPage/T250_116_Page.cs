using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using App.InfoGrid2.View.V2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using System.Text;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 生产管理/领料单
    /// </summary>
    public class T250_116_Page : ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// UT_165	生产领料单-材料明细
        /// </summary>
        Store st165 = null;

        ///显示库存按钮
        ToolBarButton btnMessage;

        protected override void OnLoad()
        {
            //StringBuilder sb = new StringBuilder();

            //foreach (var item in this.Controls)
            //{
            //    sb.AppendLine(item.ID);   
            //}



            Toolbar tbar = this.FindControl("toolbarT607") as Toolbar;

            


            st165 = this.FindControl("Store_UT_165") as Store;

            st165.Updating += st165_Updating;

            st165.CurrentChanged += st165_CurrentChanged;


            //显示库存按钮
            btnMessage = new ToolBarButton("");
            btnMessage.ID = "tbMessage";
            tbar.Items.Add(btnMessage);


        }

        void st165_CurrentChanged(object sender, ObjectEventArgs e)
        {
            LModel lm = e.Object as LModel;

            if (lm == null)
            {
                return;
            }


            //产品ID
            int COL_21 = lm.Get<int>("COL_7");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_118");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_15", COL_21);

            List<LModel> lmList118 = decipher.GetModelList(lmFilter);

            StringBuilder sb = new StringBuilder();

            //库存合计 
            decimal sum = 0;


            sb.Append("合计：{0}    ");

            foreach (var item in lmList118)
            {

                decimal col9 = item.Get<decimal>("COL_9");

                sum += col9;

                sb.Append(item.Get<string>("COL_11") + ":" + col9.ToString("0.######") + "     ");

            }



            string numStr = sum.ToString("0.######");

            string message = string.Format(sb.ToString(), numStr);
            btnMessage.Text = message;
        }

       
        /// <summary>
        /// 	UT_165	生产领料单-材料明细 更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void st165_Updating(object sender, ObjectCancelEventArgs e)
        {

            LModel lm = (LModel)e.Object;

            if(lm == null)
            {
                return;
            }

            //板长
            decimal COL_36 = lm.Get<decimal>("COL_36");
            //长度
            decimal COL_19 = lm.Get<decimal>("COL_19");
            //计量单位
            string COL_59 = lm.Get<string>("COL_59");
            //需求量单位
            string COL_61 = lm.Get<string>("COL_61");
            //需求量
            decimal COL_60 = lm.Get<decimal>("COL_60");
            //数量
            decimal COL_35 = lm.Get<decimal>("COL_35");
            //密度
            decimal COL_5 = lm.Get<decimal>("COL_5");
            //厚度
            decimal COL_17 = lm.Get<decimal>("COL_17");
            //宽度
            decimal COL_4 = lm.Get<decimal>("COL_4");
            //主单位
            string COL_9 = lm.Get<string>("COL_9");
            //主数量
            decimal COL_10 = lm.Get<decimal>("COL_10");


            //计量单位值改变 就执行 需求量 换算 数量 公式 
            bool b = lm.GetBlemish("COL_59");

            MyJosn json = null;

            if (b)
            {

                //这是 需求量 换算 数量
                json = UnitUtil.UnitConversion(lm, COL_59, COL_35, COL_19, COL_5, COL_17, COL_4, COL_36, COL_61, COL_60, "COL_35", "COL_19");

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);

                    return;
                }

                COL_35 = lm.Get<decimal>("COL_35");


            }

            if ((lm.GetBlemish("COL_35") || b) && COL_35 !=0)
            {


                // 这是 数量 换算 主数量
                json = UnitUtil.UnitConversion2(lm, COL_59, COL_9, COL_35, COL_19, COL_10, COL_5, COL_17, COL_4, COL_36, "COL_10", "COL_19");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }

            }

            string id = lm.GetPk().ToString();

            st165.SetRecordValue(id, "COL_35", lm["COL_35"]);
            st165.SetRecordValue(id, "COL_19", lm["COL_19"]);
            st165.SetRecordValue(id, "COL_10", lm["COL_10"]);


        }



        



    }
}