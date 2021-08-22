using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.JF.WeChat.Model;
using App.InfoGrid2.Model.JF;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.JF.WeChat
{
    /// <summary>
    /// PostLoginHandler 的摘要说明
    /// </summary>
    public class PostLoginHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";

            string user_json_str = WebUtil.FormTrim("user_json_str");

            //上级编码
            string parent_code = WebUtil.FormTrim("parent_code");

            //通过access token 和 openid 获取的用户信息
            WeiXinUserInfo wxuserInfo = null;

            log.Info($"用户信息的json是：{user_json_str}");

            try
            {

                wxuserInfo = JsonConvert.DeserializeObject<WeiXinUserInfo>(user_json_str);

            }
            catch (Exception ex)
            {
                log.Error($"序列化 提交上来的微信用户数据出错了了！ json：{user_json_str}", ex);
                throw new Exception("没授权不能登录微信界面！", ex);

            }


            if (string.IsNullOrWhiteSpace(wxuserInfo.openid))
            {
                log.Error("没有找到微信账户唯一标识码！传过来的数据："+ user_json_str);
                throw new Exception("没授权不能登录微信界面！");
            }



            DbDecipher decipher = null;

            try
            {



                decipher = DbDecipherManager.GetDecipherOpen();



                LightModelFilter lmFilter = new LightModelFilter(typeof(ES_W_ACCOUNT));
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("W_OPENID", wxuserInfo.openid);

                ES_W_ACCOUNT lmWxUser = decipher.SelectToOneModel<ES_W_ACCOUNT>(lmFilter);

                if (lmWxUser == null)
                {

                    ES_W_ACCOUNT account_p = BusHelper.GetUserByCode(decipher, parent_code);


                    string wechat_code = BillIdentityMgr.NewCodeForDay("W_CODE", "W");

                    lmWxUser = new ES_W_ACCOUNT();
                    lmWxUser.W_OPENID = wxuserInfo.openid;
                    lmWxUser.W_NICKNAME = wxuserInfo.nickname;
                    lmWxUser.HEAD_IMG_URL = wxuserInfo.headimgurl;
                    lmWxUser.SEX = wxuserInfo.sex == 2 ? "女" : "男";
                    lmWxUser.W_ADDRESS = wxuserInfo.province + "省" + wxuserInfo.city + "市";
                    lmWxUser.ROW_DATE_CREATE = lmWxUser.ROW_DATE_UPDATE = DateTime.Now;
                    lmWxUser.ROW_SID = 0;
                    lmWxUser.PK_W_CODE = wechat_code;
                    lmWxUser.W_GUID = Guid.NewGuid();
                    lmWxUser.GUID_LIMIT_DATE = DateTime.Now.AddDays(3);
                    lmWxUser.PARENT_CODE = parent_code;
                    lmWxUser.PARENT_CODE_2 = account_p?.PARENT_CODE;

                    decipher.InsertModel(lmWxUser);

                }

                lmWxUser.W_GUID = Guid.NewGuid();
                lmWxUser.GUID_LIMIT_DATE = DateTime.Now.AddDays(3);

                decipher.UpdateModelProps(lmWxUser, "W_GUID", "GUID_LIMIT_DATE");


                log.Debug("准备返回GUID:" + lmWxUser.W_GUID.ToString());

                context.Response.Write(lmWxUser.W_GUID.ToString());




            }
            catch (Exception ex)
            {

                log.Error("插入微信用户出错了！", ex);


                return;
            }
            finally
            {
                decipher.Dispose();
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}