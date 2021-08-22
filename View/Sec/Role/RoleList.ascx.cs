using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using EasyClick.BizWeb.UI;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EasyClick.Web.Mini;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.Role
{
    public partial class RoleList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        DataGridViewAction<SEC_ROLE> m_SEC_ROLE_Action;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_SEC_ROLE_Action = new DataGridViewAction<SEC_ROLE>(this.DataGridView1);

            if (!this.IsPostBack)
            {
                m_SEC_ROLE_Action.GoPage(0);
            }

        }

        public void SetupSec(int roleId)
        {
            //DbDecipher decipher = ModelAction.OpenDecipher();

            //SEC_ROLE role = decipher.SelectModelByPk<SEC_ROLE>(roleId);

            EcView.show("ModuleTree.aspx?roleId=" + roleId, "角色权限定义");
        }

        public void NewBtn_Click()
        {
            m_SEC_ROLE_Action.New();
        }

        public void Save_Click()
        {
            m_SEC_ROLE_Action.Save();
        }

        public void ResetBtn_Click()
        {
            m_SEC_ROLE_Action.Refresh();
        }

        public void DeleteItems()
        {
            m_SEC_ROLE_Action.DeleteItems();
        }

        public void GoPage()
        {
            m_SEC_ROLE_Action.GoPage();
        }


    }
}