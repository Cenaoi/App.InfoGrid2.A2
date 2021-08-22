using System;
using System.Collections.Generic;
using System.Text;
using App.BizCommon;
using EC5.Utility;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using EC5.SystemBoard;
using System.Web;
using System.Web.SessionState;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Bll
{


    /// <summary>
    /// 登陆管理
    /// </summary>
    public class LoginMgr : ModelAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 登陆成功
        /// </summary>
        public void LoginSuccess()
        {
            //LModelElement modelElem = LightModel.GetLModelElement("C_ADMIN");

            //EcUserState user = EcContext.Current.User;

            //LightModelFilter filter = new LightModelFilter("C_ADMIN");
            //filter.And("LOGIN_NAME", user.LoginID);

            //LightModel admin = this.Decipher.SelectToOneModel(filter) as LightModel;

            //user.LoginName = (string)admin["TRUE_NAME"];

        }


        /// <summary>
        /// 验证登录账号密码是否正确
        /// </summary>
        /// <param name="loginName">账号</param>
        /// <param name="loginPass">密码</param>
        /// <returns></returns>
        public bool Login(string loginName, string loginPass)
        {

            if(!ValidateUtil.RangeLength(loginName,new decimal[]{1,36}) ||
                !ValidateUtil.RangeLength(loginPass, new decimal[] { 1, 36 }) ||
                !ValidateUtil.SqlInput(loginName) ||
               !ValidateUtil.SqlInput(loginPass)){

                return false;
            }

            string md5Pass = loginPass;// Md5Util.ToString("XZ-" + loginPass);


            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            filter.And("LOGIN_NAME", loginName);

            try
            {
                SEC_LOGIN_ACCOUNT m = this.Decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(filter);
                EcContext context = EcContext.Current;
                EcUserState userState = context.User;

                userState.Clear();

                if (m == null)
                {
                    return false;
                }


                if (m.SEC_MODE_ID == 0)
                {
                    return false;
                }

                if (!md5Pass.Equals(m.LOGIN_PASS))
                {
                    log.Debug("LoginPass:" + md5Pass);

                    return false;
                }


            }
            catch (Exception ex)
            {
                log.Error(ex);

                return false;
            }



            return true;
        }



        public void LoginByRole(string roleCode, string loginName, string loginPass)
        {

        }


        /// <summary>
        /// 成功登录后，获取用户信息
        /// </summary>
        /// <param name="loginName"></param>
        public void GetUserByLoginName(string loginName)
        {
            LightModelFilter filter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            filter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            filter.And("LOGIN_NAME", loginName);


            SEC_LOGIN_ACCOUNT m = this.Decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(filter);

            EcContext context = EcContext.Current;

            EcUserState userState = context.User;
            userState.Clear();
            userState.ExpandPropertys.Clear();

            userState.Identity = m.SEC_LOGIN_ACCOUNT_ID;
            userState.LoginID = m.LOGIN_NAME;
            userState.LoginName = m.TRUE_NAME;

            
            userState.IsVirtual = false;


            //string[] roles = GetCodeForRoleID(m.ARR_ROLE_ID);
            //userState.Roles.AddRange(roles);

            userState.Roles.Clear();
            userState.Roles.Add("EMPLOYEE");

            userState.ExpandPropertys["ARR_ROLE_ID"] = m.ARR_ROLE_ID;
            userState.ExpandPropertys["SEC_MODE_ID"] = m.SEC_MODE_ID.ToString();

            userState.ExpandPropertys["USER_CODE"] = m.BIZ_USER_CODE;
            userState.ExpandPropertys["ORG_CODE"] = m.BIZ_ORG_CODE;




            #region 加载用户权限

            int[] roleIds = StringUtil.ToIntList(m.ARR_ROLE_ID);

            int roleId = -1;

            if (roleIds.Length > 0)
            {
                roleId = roleIds[0];
            }


            UserSecritySet userSec = InitSecFuns(m.SEC_LOGIN_ACCOUNT_ID, roleId, m.SEC_MODE_ID);

            string[] arrUserCode = StringUtil.Split(m.REF_ARR_USER_CODE);

            arrUserCode = ArrayUtil.Union(arrUserCode, new string[] { m.BIZ_USER_CODE });

            userSec.ArrUserCode = arrUserCode;
            userSec.ArrRoleCode = StringUtil.Split(m.REF_ARR_ROLE_CODE);
            userSec.ArrStructCode = StringUtil.Split(m.REF_ARR_STRUCT_CODE);

            userSec.ArrCatalogCode = BizCatalogMgr.GetCodes(userSec.ArrStructCode);

            userSec.ArrModeId = m.SEC_MODE_ID;  //临时采用 SEC_MODE_ID 字段，以后取消。



            #endregion
        }


        public class AAA
        {
            public string LoginName;

            public HttpSessionState Session;
        }



        /// <summary>
        /// 初始化权限模块
        /// </summary>
        /// <param name="loginAccountId"></param>
        /// <param name="roleId"></param>
        /// <param name="secModeId"></param>
        /// <returns></returns>
        private UserSecritySet InitSecFuns(int loginAccountId, int roleId, int secModeId)
        {
            UserSecritySet funMgr = new UserSecritySet();
            funMgr.ModeId = secModeId;
                        
            SecFunMgr.SetUserSecuritySet(funMgr);

            if (secModeId == 0)
            {
                return funMgr;
            }

            if (secModeId == 1)
            {
                funMgr.RoleFuns = GetSecRoleFunDict(roleId);
            }
            else if (secModeId == 2)
            {
                funMgr.UserFuns = GetSecUserFunDict(loginAccountId);
            }
            else if (secModeId == 3)
            {
                funMgr.RoleFuns = GetSecRoleFunDict(roleId);
                funMgr.UserFuns = GetSecUserFunDict(loginAccountId);
            }

            return funMgr;
        }

        private SecUserFunDict GetSecUserFunDict(int loginAccountId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(SEC_USER_FUN));
            filter.And("LOGIN_ID", loginAccountId);
            filter.And("CHECK_STATE_ID", new int[] { 1, 2 }, Logic.In);

            LModelList<SEC_USER_FUN> models = this.Decipher.SelectModels<SEC_USER_FUN>(filter);

            SecUserFunDict userFuns = new SecUserFunDict();

            foreach (SEC_USER_FUN fun in models)
            {
                if (userFuns.ContainsKey(fun.SEC_FUN_DEF_ID))
                {
                    continue;
                }

                userFuns.Add(fun.SEC_FUN_DEF_ID, fun);
            }

            return userFuns;
        }


        private SecRoleFunDict GetSecRoleFunDict(int roleId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(SEC_ROLE_FUN));
            filter.And("SEC_ROLE_ID", roleId);
            filter.And("CHECK_STATE_ID", new int[] { 1, 2 }, Logic.In);

            LModelList<SEC_ROLE_FUN> models = this.Decipher.SelectModels<SEC_ROLE_FUN>(filter);

            SecRoleFunDict roleFuns = new SecRoleFunDict();

            foreach (SEC_ROLE_FUN fun in models)
            {
                if (roleFuns.ContainsKey(fun.SEC_FUN_DEF_ID))
                {
                    continue;
                }

                roleFuns.Add(fun.SEC_FUN_DEF_ID, fun);
            }

            return roleFuns;
        }



        private string[] GetCodeForRoleID(string arrRoleID)
        {
            int[] ids = StringUtil.ToIntList(arrRoleID);


            LModelList<SEC_ROLE> role = this.Decipher.SelectModelsIn<SEC_ROLE>("SEC_ROLE_ID", ids);

            if (role == null)
            {
                return new string[0];
            }

            string[] roles = role.GetColumnData<string>("ROLE_CODE");

            return roles;
        }


    }
}
