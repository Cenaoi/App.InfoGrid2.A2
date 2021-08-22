using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini;
using App.InfoGrid2.Model;
using EasyClick.BizWeb.UI;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec
{
    public partial class SecFunDefTree : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TreeViewAction<SEC_FUN_DEF_VIEW, int> m_TreeAction;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_TreeAction = new TreeViewAction<SEC_FUN_DEF_VIEW, int>(this.TreeView1);
            m_TreeAction.NodeTypeChange("2", "page");

            int[] typeIDs = new int[] { 0,2};
            m_TreeAction.FilterSearch("FUN_TYPE_ID", typeIDs, Logic.In);

            if (!this.IsPostBack)
            {
                LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
                filter.Joins.Add( JoinTypes.Left, typeof(SEC_ROLE_FUN),"SEC_FUN_DEF.SEC_FUN_DEF_ID = SEC_ROLE_FUN.SEC_FUN_DEF_ID");
                filter.JoinPk = "SEC_ROLE_FUN_ID";
                filter.And("FUN_TYPE_ID", new int[] { 0, 2 }, Logic.In);
                //filter.And("SEC_FUN_DEF_ID", 105, Logic.LessThan);

                DbDecipher decipher = ModelAction.OpenDecipher();

                LModelReader reader = decipher.GetModelReader(filter);

                LModelList<SEC_FUN_DEF_VIEW> models = new LModelList<SEC_FUN_DEF_VIEW>();

                models = ModelHelper.GetModels<SEC_FUN_DEF_VIEW>(reader);


                m_TreeAction.ExplandNodes(models);
            
            }


        }
    }
}