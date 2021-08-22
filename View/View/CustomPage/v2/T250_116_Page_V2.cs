using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.CustomPage.v2
{
    /// <summary>
    /// 生产管理/领料单   副本
    /// </summary>
    public class T250_116_Page_V2 : ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        protected override void OnLoad()
        {
            //StringBuilder sb = new StringBuilder();

            //foreach (var item in this.Controls)
            //{
            //    sb.AppendLine(item.ID);   
            //}

            

                //	UT_165	生产领料单-材料明细
                Store st165 = this.FindControl("Store_UT_165") as Store;

           
            
        }
      


        



    }
}