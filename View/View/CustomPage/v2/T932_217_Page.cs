using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.View.CustomPage.v2
{
    /// <summary>
    /// 仓库管理 / 库存明细
    /// </summary>
    public class T932_217_Page : ExPage
    {
        protected override void OnLoad()
        {
            string id = WebUtil.Query("StoreID");


            if (string.IsNullOrEmpty(id))
            {
                return;
            }


            this.MainStore.FilterParams.Add(new QueryStringParam() { Name = "COL_12", QueryStringField = "StoreID", Logic = "in" });
        }

    }
}