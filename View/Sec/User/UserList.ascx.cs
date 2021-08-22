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
using HWQ.Entity.LightModels;
using EC5.Utility;
using App.InfoGrid2.Model.SecModels;



namespace App.InfoGrid2.Sec.User
{
    public partial class UserList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        DataGridViewAction<SEC_LOGIN_ACCOUNT> m_SEC_ROLE_Action;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_SEC_ROLE_Action = new DataGridViewAction<SEC_LOGIN_ACCOUNT>(this.DataGridView1);
            m_SEC_ROLE_Action.AddFilterFixed("ROW_STATUS_ID", 0);

            m_SEC_ROLE_Action.AddFilterDelete("SYS_FIXED", false);

            if (!this.IsPostBack)
            {
                m_SEC_ROLE_Action.GoPage(0);
            }

        }

        public void SetupSec(int userId)
        {
            //DbDecipher decipher = ModelAction.OpenDecipher();

            //SEC_ROLE role = decipher.SelectModelByPk<SEC_ROLE>(roleId);

            EcView.show("ModuleTree.aspx?userId=" + userId, "用户权限定义");
        }


        public void InitPass(int userId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.And("SEC_LOGIN_ACCOUNT_ID", userId);
            filter.And("ROW_STATUS_ID", 0);


            SEC_LOGIN_ACCOUNT m = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(filter);

            m.LOGIN_PASS = Md5Util.ToString("XZ-123456");

            try
            {
            
                decipher.UpdateModelProps(m, "LOGIN_PASS");

                MiniHelper.Tooltip("初始化成功.123456");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Alert("初始化失败");
            }
        }


        public void NewBtn_Click()
        {

            EcView.SetClosedClientScript("refresh(sender,e);");
            EcView.showDialog("LoginNew.aspx", "创建账号", 400, 300);
            
            //m_SEC_ROLE_Action.New();
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
            m_SEC_ROLE_Action.Updated += new EventHandler<ModelEventArgs<SEC_LOGIN_ACCOUNT>>(m_SEC_ROLE_Action_Updated);
            m_SEC_ROLE_Action.ChangeStatus("ROW_STATUS_ID", 0, -3);
        }

        void m_SEC_ROLE_Action_Updated(object sender, ModelEventArgs<SEC_LOGIN_ACCOUNT> e)
        {
            e.Model.ROW_DATE_DELETE = DateTime.Now;

            DbDecipher decipher = ModelAction.OpenDecipher();
            decipher.UpdateModelProps(e.Model, "ROW_DATE_DELETE");
        }

        public void GoPage()
        {
            m_SEC_ROLE_Action.GoPage();
        }

        
        /// <summary>
        /// 改变角色
        /// </summary>
        public void ChangeRole()
        {
            string pk = this.DataGridView1.FocusedItem.Pk;

            EcView.SetClosedClientScript("refresh(sender,e)");
            EcView.showDialog("RoleSelectList.aspx?userId=" + pk, "选择角色", 400, 400);
        }


        /// <summary>
        /// 创建独立权限
        /// </summary>
        public void CreateUserSec()
        {
            string pk = this.DataGridView1.FocusedItem.Pk;

            EcView.show("ModuleTree.aspx?userId=" + pk, "用户权限设置");
        }


        /// <summary>
        /// 删除独立权限
        /// </summary>
        public void DeleteUserSec()
        {

        }


    }
}