using System;
using System.Collections.Generic;
using System.Text;
using App.BizCommon;
using EC5.SystemBoard;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using HWQ.Entity.Filter;
using System.Web;
using App.InfoGrid2.Model.SecModels;
using HWQ.Entity;

namespace App.InfoGrid2.Bll.Sec
{

    /// <summary>
    /// 权限模块
    /// </summary>
    public class SecFunMgr:ModelAction
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string SEC_FUN_MANAGER = "SEC_FUN_MANAGER";

        /// <summary>
        /// 获取用户的安全权限
        /// </summary>
        /// <returns></returns>
        public static UserSecritySet GetUserSecuritySet()
        {
            HttpContext context = HttpContext.Current;

            if (context == null) { throw new Exception("HttpContext.Current 上下文不能为空的."); }

            UserSecritySet funMgr = context.Session[SEC_FUN_MANAGER] as UserSecritySet;

            return funMgr;
        }

        /// <summary>
        /// 设置用户的安全权限
        /// </summary>
        /// <param name="userSecSet"></param>
        public static void SetUserSecuritySet(UserSecritySet userSecSet)
        {
            if (userSecSet == null) { throw new ArgumentNullException("userSecSet","参数不能为空."); }

            HttpContext context = HttpContext.Current;

            if (context == null) { throw new Exception("HttpContext.Current 上下文不能为空的."); }

            context.Session[SEC_FUN_MANAGER] = userSecSet;

        }

        /// <summary>
        /// 清理用户权限
        /// </summary>
        public static void Clear()
        {
            HttpContext context = HttpContext.Current;

            if (context == null) { throw new Exception("HttpContext.Current 上下文不能为空的."); }

            context.Session.Remove(SEC_FUN_MANAGER);
        }

        /// <summary>
        /// 是否存在这方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsFunVisible(int id)
        {
            UserSecritySet funMgr = GetUserSecuritySet();

            if (funMgr == null || funMgr.ModeId　== 0)
            {
                return false;
            }


            int modeId = funMgr.ModeId;

            bool reValue = false;

            switch (modeId)
            {
                case 2: //用户权限
                    reValue = IsModuleVisible_ForUser(funMgr, id);
                    break;
                case 3: //角色权限
                    reValue = IsModuleVisible_ForRole(funMgr, id);
                    break;
                //case 4: 
                //    break;
            }

            return reValue;
        }

        private static bool IsModuleVisible_ForUser(UserSecritySet funMgr, int id)
        {
            if (funMgr == null)
            {
                throw new ArgumentNullException("funMgr", "参数不能为空.");
            }

            SEC_USER_FUN fun;

            if (!funMgr.UserFuns.TryGetValue(id, out fun))
            {
                return false;
            }

            return (fun.CHECK_STATE_ID > 0);
        }

        private static bool IsModuleVisible_ForRole(UserSecritySet funMgr, int id)
        {
            if (funMgr == null)
            {
                throw new ArgumentNullException("funMgr", "参数不能为空.");
            }

            SEC_ROLE_FUN fun;

            if (!funMgr.RoleFuns.TryGetValue(id, out fun))
            {
                return false;
            }

            return (fun.CHECK_STATE_ID > 0);
        }


        /// <summary>
        /// 是否显示这模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsModuleVisible(int id)
        {
            UserSecritySet funMgr = GetUserSecuritySet();

            if (funMgr == null || funMgr.ModeId == 0)
            {
                return false;
            }


            int modeId = funMgr.ModeId;

            bool reValue = false;

            switch (modeId)
            {
                case 2: 
                    reValue = IsModuleVisible_ForUser(funMgr, id); 
                    break;
                case 3: 
                    reValue = IsModuleVisible_ForRole(funMgr, id); 
                    break;
                //case 4: 
                //    break;
            }

            return reValue;
        }


        private static bool ExistForRole(int moduleId, int[] arrRoleId)
        {
            if (arrRoleId == null) { throw new ArgumentNullException("arrRoleId", "参数不能为空"); }

            LightModelFilter filter = new LightModelFilter(typeof(SEC_ROLE_FUN));
            filter.And("SEC_FUN_DEF_ID", moduleId);
            filter.And("SEC_ROLE_ID", arrRoleId, Logic.In);
            filter.And("CHECK_STATE_ID", new int[] { 1, 2 }, Logic.In);
            filter.Locks.Add(LockType.NoLock);

            DbDecipher decipher = ModelAction.OpenDecipher();

            bool exist = decipher.ExistsModels(filter);

            return exist;
        }

        private static void CreateFunCodes(SEC_ROLE_FUN fun )
        {
            if (fun == null || fun.ArrCheckedCode != null)
            {
                return;
            }

            fun.ArrCheckedCode = new SortedList<string, bool>();

            DbDecipher decipher = ModelAction.OpenDecipher();
            int[] arrFunId = StringUtil.ToIntList(fun.FUN_ARR_CHILD_ID);

            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("SEC_FUN_DEF_ID", arrFunId, Logic.In);
            filter.Fields = new string[] { "SEC_FUN_DEF_ID", "CODE" };
            filter.Locks.Add(LockType.NoLock);

            LModelReader reader = decipher.GetModelReader(filter);

            string[] codes = ModelHelper.GetColumnData<string>(reader, 1);

            foreach (string code in codes)
            {
                if (fun.ArrCheckedCode.ContainsKey(code))
                {
                    continue;
                }

                fun.ArrCheckedCode.Add(code, true);
            }

        }

        private static void CreateFunCodes(SEC_USER_FUN fun)
        {
            if (fun == null || fun.ArrCheckedCode != null)
            {
                return;
            }

            fun.ArrCheckedCode = new SortedList<string, bool>();

            DbDecipher decipher = ModelAction.OpenDecipher();
            int[] arrFunId = StringUtil.ToIntList(fun.FUN_ARR_CHILD_ID);

            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("SEC_FUN_DEF_ID", arrFunId, Logic.In);
            filter.Fields = new string[] { "SEC_FUN_DEF_ID", "CODE" };
            filter.Locks.Add(LockType.NoLock);


            LModelReader reader = decipher.GetModelReader(filter);

            string[] codes = ModelHelper.GetColumnData<string>(reader, 1);

            foreach (string code in codes)
            {
                if (fun.ArrCheckedCode.ContainsKey(code))
                {
                    continue;
                }

                fun.ArrCheckedCode.Add(code, true);
            }

        }

        /// <summary>
        /// 判断是否存在这个方法名称
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="funCode"></param>
        /// <returns></returns>
        public static bool IsFunVisible(int moduleId, string funCode)
        {
            if (StringUtil.IsBlank(funCode)) { throw new ArgumentNullException("funCode","参数不能为空."); }

            UserSecritySet funMgr = GetUserSecuritySet();

            if (funMgr == null || funMgr.ModeId == 0)
            {
                return false;
            }


            if (funMgr.ModeId == 2)
            {
                SEC_USER_FUN fun;
                if (!funMgr.UserFuns.TryGetValue(moduleId,out fun))
                {
                    return false;
                }
            
                if (fun.CHECK_STATE_ID == 0)
                {
                    return false;
                }

                CreateFunCodes(fun);

                if (fun.ArrCheckedCode != null && fun.ArrCheckedCode.ContainsKey(funCode))
                {
                    return true;
                }

                return false;
            }
            else if (funMgr.ModeId == 3)
            {
                SEC_ROLE_FUN fun;

                if (!funMgr.RoleFuns.TryGetValue(moduleId,out fun))
                {
                    return false;
                }

                if (fun.CHECK_STATE_ID == 0)
                {
                    return false;
                }

                CreateFunCodes(fun);

                if (fun.ArrCheckedCode != null && fun.ArrCheckedCode.ContainsKey(funCode))
                {
                    return true;
                }

                return false;
            }


            return false;

        }


        /// <summary>
        /// 是否存在这方法
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="funCode"></param>
        /// <returns></returns>
        public static bool IsFunVisible(int moduleId, int funId)
        {
            int[] arrFunId = GetArrFunId(moduleId);

            if (ArrayUtil.Exist(arrFunId, funId))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 获取模块下面的方法ID
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static int[] GetArrFunId(int moduleId)
        {
            EC5.SystemBoard.EcContext content = EcContext.Current;

            EcUserState user = content.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            string strArrRoleId = user.ExpandPropertys["ARR_ROLE_ID"];

            int[] arrRoleId = StringUtil.ToIntList(strArrRoleId);

            return GetArrFunId(arrRoleId, moduleId);
        }


        /// <summary>
        /// 获取模块下面的方法ID
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static int[] GetArrFunId(int[] arrRoleId, int moduleId)
        {
            if (arrRoleId == null) { throw new ArgumentNullException("arrRoleId", "参数不能为空."); }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(SEC_ROLE_FUN));
            filter.And("SEC_FUN_DEF_ID", moduleId);
            filter.And("SEC_ROLE_ID", arrRoleId, Logic.In);
            filter.And("VISIBLE", true);
            filter.Locks.Add(LockType.NoLock);

            LModelList<SEC_ROLE_FUN> models = decipher.SelectModels<SEC_ROLE_FUN>(filter);

            int[] arrFunIds = new int[0];

            //处理拥有多个角色用户的方法ID
            foreach (SEC_ROLE_FUN item in models)
            {
                int[] funIds = StringUtil.ToIntList(item.FUN_ARR_CHILD_ID);

                arrFunIds = ArrayUtil.Union(arrFunIds, funIds);
            }

            return arrFunIds;

        }


    }
}
