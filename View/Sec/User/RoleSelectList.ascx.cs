using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using EasyClick.BizWeb.UI;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.Utility;
using EasyClick.Web.Mini;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.User
{
    public partial class RoleSelectList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DataGridViewAction<SEC_ROLE> m_ROLE_Act;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_ROLE_Act = new DataGridViewAction<SEC_ROLE>(this.DataGridView1);

            if (!this.IsPostBack)
            {
                m_ROLE_Act.GoPage(0);
            }
        }

        public void GoPage()
        {
            m_ROLE_Act.GoPage();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OkBtn()
        {
            int userId = WebUtil.QueryInt("userId");

            int[] ids = m_ROLE_Act.GetCheckedIntList();

            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.And("SEC_LOGIN_ACCOUNT_ID", userId);
            filter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_LOGIN_ACCOUNT m = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(filter);

            m.ARR_ROLE_ID = StringUtil.ToString(ids);
            m.ARR_ROLE_NAME = GetArrRoleID(m.ARR_ROLE_ID);

            try
            {
                decipher.UpdateModelProps(m, "ARR_ROLE_ID", "ARR_ROLE_NAME");

                EcView.close("{result:'ok'}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private string GetArrRoleID(string arrRoleID)
        {
            int[] ids = StringUtil.ToIntList(arrRoleID);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<SEC_ROLE> role = decipher.SelectModelsIn<SEC_ROLE>("SEC_ROLE_ID", ids);

            if (role == null)
            {
                return string.Empty;
            }

            string[] roles = role.GetColumnData<string>("TEXT");

            return StringUtil.ToString(roles);
        }

        public void ClearBtn()
        {
            EcView.close();
        }

    }
}