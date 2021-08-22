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
using EC5.SystemBoard;
using App.InfoGrid2.Model.SecModels;


namespace App.InfoGrid2.Sec.User
{
    public partial class LoginPassEdit : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void Submit()
        {
            EcContext context = EcContext.Current;

            EcUserState userState = context.User;


            int userId = userState.Identity;

            if (StringUtil.IsBlank(this.OldPass1.Value) ||
                StringUtil.IsBlank(this.NewPass1.Value) || StringUtil.IsBlank(this.NewPass2.Value))
            {
                MiniHelper.Alert("请填写正确信息");
                return;
            }

            if (this.NewPass1.Value != this.NewPass2.Value)
            {
                MiniHelper.Alert("两个密码不一样");
                return;
            }


            
            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.And("SEC_LOGIN_ACCOUNT_ID", userId);
            filter.And("ROW_STATUS_ID", 0);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_LOGIN_ACCOUNT m = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(filter);

            m.LOGIN_PASS = NewPass1.Value;// Md5Util.ToString("XZ-" + NewPass1.Value);

            try
            {
                decipher.UpdateModelProps(m, "LOGIN_PASS");

                MiniHelper.Tooltip("保存成功");
                
                EcView.close();

            }
            catch (Exception ex)
            {
                log.Error(ex);

                MiniHelper.Alert("修改失败");
            }



        }


    }
}