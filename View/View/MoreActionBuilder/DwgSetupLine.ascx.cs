using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.MoreActionBuilder
{
    public partial class DwgSetupLine : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.storeMain1.DataBind();
            }
        }
    }
}