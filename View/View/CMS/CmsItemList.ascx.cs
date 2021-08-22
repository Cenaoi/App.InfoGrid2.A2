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

namespace App.InfoGrid2.View.CMS
{
    public partial class CmsItemList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            this.table1.Command += Table1_Command;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            string url = $"/App/InfoGrid2/View/CMS/CmsItemEdit.aspx?id={e.Record.Id}";

            EcView.Show(url, "编辑文章内容");

        }
    }
}