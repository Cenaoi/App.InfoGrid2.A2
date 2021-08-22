using App.BizCommon;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.JF.Bll
{
    /// <summary>
    /// 业务助手
    /// </summary>
    public class BusHelper
    {
        /// <summary>
        /// 自动登录  
        /// </summary>
        /// <returns>true -- 可以自动登录  false -- 不能自动登录 </returns>
        public static bool AutoLogin()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_W_ACCOUNT account = decipher.SelectModelByPk<ES_W_ACCOUNT>(user.Identity);
            
            if(account != null)
            {

                return true;

            }


            HttpCookie cookie = HttpContext.Current.Request.Cookies["USER_GUID"];


            if(cookie == null)
            {
                return false;
            }

            Guid guid;

            if (!Guid.TryParse(cookie.Value, out guid))
            {
                return false;
            }



            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_W_ACCOUNT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("W_GUID", guid);
            lmFilter.And("GUID_LIMIT_DATE", DateTime.Now, Logic.GreaterThan);


            account = decipher.SelectToOneModel<ES_W_ACCOUNT>(lmFilter);

            if(account == null)
            {

                return false;

            }


            user.Clear();

            user.ExpandPropertys.Clear();

            user.Identity = account.ES_W_ACCOUNT_ID;
            user.LoginID = account.W_OPENID;
            user.LoginName = account.W_NICKNAME;

            user.Roles.Clear();
            user.Roles.Add("WX");

            user.ExpandPropertys["W_CODE"] = account.PK_W_CODE;

            return true;
        }

        /// <summary>
        /// 根据自定义主键编码获取用户对象
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="user_code">自定义主键编码</param>
        /// <returns></returns>
        public static ES_W_ACCOUNT GetUserByCode(DbDecipher decipher, string user_code)
        {
            //判断用户自定义主键编码是否为空
            if (string.IsNullOrWhiteSpace(user_code))
            {
                return null;
            }

            LightModelFilter lmFilterAccount = new LightModelFilter(typeof(ES_W_ACCOUNT));
            lmFilterAccount.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterAccount.And("PK_W_CODE", user_code);

            return decipher.SelectToOneModel<ES_W_ACCOUNT>(lmFilterAccount);


        }


    }
}