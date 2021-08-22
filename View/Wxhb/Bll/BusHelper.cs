using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Model.WeChat;
using App.InfoGrid2.Wxhb.Model;
using EC5.SystemBoard;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.Wxhb.Bll
{
    /// <summary>
    /// 业务助手类
    /// </summary>
    public class BusHelper
    {

        /// <summary>
        /// 微信推送消息列表 用来去重的  关键字 FromUserName-CreateTime   值是 CreateTime 用来清除一些旧消息
        /// </summary>
        static Dictionary<string, long> wx_msgs = new Dictionary<string, long>();

        static Cache<string> m_Cache = new Cache<string>();

        /// <summary>
        /// 自动登录  
        /// </summary>
        /// <returns>true -- 可以自动登录  false -- 不能自动登录 </returns>
        public static bool AutoLogin()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user.Identity);

            if (account != null)
            {

                return true;

            }


            HttpCookie cookie = HttpContext.Current.Request.Cookies["USER_GUID"];

            if (cookie == null)
            {
                return false;
            }

            Guid guid;

            if (!Guid.TryParse(cookie.Value, out guid))
            {
                return false;
            }

            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT_EX));
            lmFilter.And("LOGIN_GUID", guid);
            lmFilter.And("GUID_LIMIT_DATE", DateTime.Now, Logic.GreaterThan);


            //SEC_LOGIN_ACCOUNT_EX account_ex = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT_EX>(lmFilter);

            //if (account_ex == null)
            //{
            //    return false;

            //}

            //return true;

            bool isExist = decipher.ExistsModels(lmFilter);

            return isExist;
        }

        /// <summary>
        /// 判断微信传上来的消息是否重复了
        /// </summary>
        /// <param name="wsd">微信推送过来的数据对象</param>
        /// <returns></returns>
        public static bool IsWxMsgRepeat(WxSendData wsd)
        {

            string key = $"{wsd.FromUserName}-{wsd.CreateTime}";

            object value = null;

            return m_Cache.TryGet(key, out value);

        }


        /// <summary>
        /// 添加微信推送上来的消息到列表中去，用来排重的
        /// <param name="wsd">微信推送过来的数据对象</param>
        /// </summary>
        public static void AddWxMsg(WxSendData wsd)
        {

            string key = $"{wsd.FromUserName}-{wsd.CreateTime}";

            m_Cache.Set(key, 1000 * 60 * 60, null);

            m_Cache.ClearOvertime();


        }

        /// <summary>
        /// 移除报错的消息
        /// </summary>
        public static void RemoveMsg(WxSendData wsd)
        {
           
          string key = $"{wsd.FromUserName}-{wsd.CreateTime}";

            object value = null;

            m_Cache.TryRemove(key, out value);

        }


        /// <summary>
        /// 将UNIX时间戳转换成系统时间
        /// </summary>
        /// <returns></returns>
        static DateTime DateParseTimeStamp(long CreateTime)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long aa = CreateTime * 10000000;

            TimeSpan span = new TimeSpan(aa);

            return dtStart.Add(span);
            
        }


        /// <summary>
        /// 根据微信用户openid来获取数据库里面的信息
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <returns></returns>
        public static WX_ACCOUNT GetWxAccountByOpenid(string openid)
        {


            if (string.IsNullOrWhiteSpace(openid))
            {
                return null;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(WX_ACCOUNT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("W_OPENID", openid);

            WX_ACCOUNT wx_account = decipher.SelectToOneModel<WX_ACCOUNT>(lmFilter);

            return wx_account;

            

        }


        /// <summary>
        /// 从字段中获取文件集合数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetFilesByField(string field_value)
        {
            SModelList sm_imgs = new SModelList();

            if (string.IsNullOrWhiteSpace(field_value))
            {

                return sm_imgs;

            }

            string[] imgs = StringUtil.Split(field_value, "\n");

            foreach (string img in imgs)
            {
                if (string.IsNullOrEmpty(img))
                {
                    continue;
                }

                string[] pro = StringUtil.Split(img, "||");
                SModel sm = new SModel();
                sm["url"] = pro[0];
                sm["thumb_url"] = pro[1];
                sm["name"] = pro[2];
                sm["code"] = pro[3];
                sm_imgs.Add(sm);
            }

            return sm_imgs;

        }

        /// <summary>
        /// 解析传上来的字符串成SModel对象
        /// </summary>
        /// <param name="sm_json_str">整个实体json字符串</param>
        /// <param name="change_field_str">改变的字段json字符串</param>
        /// <param name="table_name">表名</param>
        /// <param name="pk_field">主键字段</param>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public static SModel ParseSModel(string sm_json_str, string change_field_str, string table_name, string pk_field, out int id)
        {


            JArray ja = JArray.Parse(change_field_str);



            List<string> old_fields = new List<string>();


            foreach (var item in ja)
            {
                old_fields.Add(item.Value<string>());
            }


            SModel sm_002 = SModel.ParseJson(sm_json_str);


            DbDecipher decipher = ModelAction.OpenDecipher();

            id = sm_002.Get<int>(pk_field);

            LModelElement modelElem = LightModel.GetLModelElement(table_name);
            LModelFieldElement fieldElem;

            SModel uModel = new SModel();

            foreach (var item in old_fields)
            {
                if (modelElem.TryGetField(item, out fieldElem))
                {
                    uModel[item] = ModelConvert.ChangeType(sm_002[item], fieldElem);
                }
            }

            return uModel;


        }



    }
}