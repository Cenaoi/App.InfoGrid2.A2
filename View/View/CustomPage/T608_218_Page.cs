using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 库存明细
    /// </summary>
    public class T608_218_Page : ExPage
    {
        protected override void OnLoad()
        {
            string id = WebUtil.Query("StoreID");


            if(string.IsNullOrEmpty(id))
            {
                return;
            }


            this.MainStore.FilterParams.Add(new QueryStringParam() { Name = "COL_12", QueryStringField = "StoreID", Logic = "in" });
        }
    }
}