using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
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
    public partial class LoginNew : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        private void InitData()
        {
            //DbDecipher decipher = ModelAction.OpenDecipher();


            //LModelList<SEC_ROLE> models = decipher.SelectModels<SEC_ROLE>();

            //foreach (SEC_ROLE m in models)
            //{
            //    ListItem item = new ListItem(m.SEC_ROLE_ID, m.TEXT);

            //    this.RoleID_DDL.Items.Add(item);
            //}

        }

        public void Submit()
        {
            if (StringUtil.IsBlank(this.TextBox2.Value))
            {
                MiniHelper.Alert("登陆账号不能为空");
                return;
            }

            SEC_LOGIN_ACCOUNT m = FormAction.GetModel<SEC_LOGIN_ACCOUNT>(this.TableLayoutPanel1);

            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.And("LOGIN_NAME", m.LOGIN_NAME); 
            filter.And("ROW_STATUS_ID", 0);

            DbDecipher decipher = ModelAction.OpenDecipher();

            bool exist = decipher.ExistsModels(filter);

            if (exist)
            {
                MiniHelper.Alert("账号重复,请重新选一个");
                return;
            }

            m.BIZ_USER_CODE = BizCommon.BillIdentityMgr.NewCode(BillCodeType.Num, "USER_CODE", "", 3);

            m.ARR_ROLE_ID = m.SEC_MODE_ID.ToString();
            m.ARR_ROLE_NAME = m.ARR_ROLE_NAME = "自定义角色";

            //m.ARR_ROLE_NAME = GetArrRoleID(m.ARR_ROLE_ID);
            m.LOGIN_PASS = Md5Util.ToString("XZ-" + m.LOGIN_PASS);

            try
            {
                
                decipher.InsertModel(m);

                MiniHelper.Tooltip("创建成功");
                EcView.close("{result:'ok'}");
            }
            catch (Exception ex)
            {
                log.Error(ex);

                MiniHelper.Alert("创建失败");
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


    }
}