using App.InfoGrid2.View.V2;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 生产管理 / 退料单
    /// </summary>
    public class T527_203_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// UT_167	生产退料单-材料明细
        /// </summary>
        Store st167 = null;


        protected override void OnLoad()
        {
            //StringBuilder sb = new StringBuilder();

            //foreach (var item in this.Controls)
            //{
            //    sb.AppendLine(item.ID);   
            //}

            

                st167 = this.FindControl("Store_UT_167") as Store;

                st167.Updating += st167_Updating;

            

        }
        /// <summary>
        /// UT_167	生产退料单-材料明细 更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void st167_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;

            if (lm == null)
            {
                return;
            }


            //板长
            decimal COL_36 = lm.Get<decimal>("COL_36");
            //长度
            decimal COL_19 = lm.Get<decimal>("COL_19");
            //计量单位
            string COL_58 = lm.Get<string>("COL_58");
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


            if (lm.GetBlemish("COL_35"))
            {

                var json = UnitUtil.UnitConversion2(lm, COL_58, COL_9, COL_35, COL_19, COL_10, COL_5, COL_17, COL_4, COL_36, "COL_35", "COL_19");

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }
            }

            string id = lm.GetPk().ToString();

            st167.SetRecordValue(id, "COL_35", lm["COL_35"]);
            st167.SetRecordValue(id, "COL_19", lm["COL_19"]);


        }
    }
}