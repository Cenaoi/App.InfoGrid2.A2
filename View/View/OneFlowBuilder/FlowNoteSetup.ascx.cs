using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.OneFlowBuilder
{
    public partial class FlowNoteSetup : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.storeMain1.DataBind();
            }
        }
    }
}