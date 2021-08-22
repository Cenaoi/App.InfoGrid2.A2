using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.OneForm
{
    public partial class FlowInstPreview : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";



            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                storeProd1.PageLoaded += StoreProd1_PageLoaded;

                storeMain1.DataBind();
                storeProd1.DataBind();
            }
        }

        private void StoreProd1_PageLoaded(object sender, EasyClick.Web.Mini2.ObjectListEventArgs e)
        {
            foreach (var item in e.ObjectList)
            {
                string nodeType = (string)LightModel.GetFieldValue(item, "DEF_NODE_TYPE");

                if(nodeType == "start")
                {
                    LightModel.SetFieldValue(item, "DEF_NODE_TEXT", "开始");
                }
                else if(nodeType == "end")
                {
                    LightModel.SetFieldValue(item, "DEF_NODE_TEXT", "结束");
                }


            }
        }
    }
}