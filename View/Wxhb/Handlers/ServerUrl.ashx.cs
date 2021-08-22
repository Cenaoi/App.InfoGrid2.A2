using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using App.InfoGrid2.Wxhb.Bll;
using App.InfoGrid2.Wxhb.Model;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Wxhb.Handlers
{
    /// <summary>
    /// 这是在微信公众号上面的服务器配置的url 地址函数
    /// </summary>
    public class ServerUrl : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";

            string http_method = context.Request.HttpMethod;

            log.Info("这是给微信公众号的服务器地址用的，访问过来的地址："+context.Request.UserHostAddress);

            if (http_method == "POST")
            {
                HandlerPost();

            }
            else
            {


                string signature = WebUtil.Query("signature");

                string timestamp = WebUtil.Query("timestamp");

                string nonce = WebUtil.Query("nonce");

                string echostr = WebUtil.Query("echostr");

                string token = "WXHB_2017_01_13";


                List<string> temp_arr = new List<string>();
                temp_arr.Add(nonce);
                temp_arr.Add(token);
                temp_arr.Add(timestamp);

                temp_arr.Sort();

                string temp_str = string.Join("", temp_arr.ToArray());

                string sha1_str = WeChatUtil.HashCode(temp_str);

                if (signature == sha1_str)
                {
                    log.Info("准备返回【echostr】值啦：" + echostr);

                    WebWrite(echostr);

                    return;
                }

            }


        }


        /// <summary>
        /// 处理post方式提交上来的数据！
        /// </summary>
        void HandlerPost()
        {


            log.Info("进来post处理函数了！");

            StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream);

            string xmlData = reader.ReadToEnd();

            log.Info("微信传上来的数据:" + xmlData);

            WxSendData wsd = WeChatUtil.GetResultObj<WxSendData>(xmlData);

            try
            {
                //微信那边重复上传了！
                if (BusHelper.IsWxMsgRepeat(wsd))
                {
                    return;
                }

                BusHelper.AddWxMsg(wsd);

                switch (wsd.MsgType)
                {

                    case "event":
                        EventHandler(wsd);
                        break;
                    case "text":
                        string result = CreateXmlStrImg(wsd.FromUserName, "这是标题", "这是图文消息描述", "http://tse2.mm.bing.net/th?id=OIP.aDaPfyIQSNaJ33R81jIa5gEsDI&pid=15.1", "http://www.baidu.com");

                        WebWrite(result);

                        break;
                    default:
                        log.Error($"MsgType【{wsd.MsgType}】消息类型好像出错了！");
                        break;

                }


      


               
            }catch(Exception ex)
            {
                BusHelper.RemoveMsg(wsd);

                log.Error(ex);



            }


        }


        /// <summary>
        /// 事件处理函数
        /// </summary>
        /// <param name="wsd"></param>
        void EventHandler(WxSendData wsd)
        {

            //用户关注事件  有两种情况 一种是有EventKey值的 一种是没有的
            if (wsd.Event == "subscribe")
            {

                Subscribe(wsd);

                string result = CreateXmlStrText(wsd.FromUserName, "<a href='http://wx.cmshome.cc/WXHB/View/Msg/Course.aspx'>公众号使用说明</a>");

                WebWrite(result);

                return;
        
            }

            //微信用户点击菜单事件
            if(wsd.Event == "CLICK")
            {
                GetWxAccount(wsd);

                WebWrite("success");

                return;

            }

            //微信用户点击菜单是地址的事件
            if(wsd.Event == "VIEW")
            {

                GetWxAccount(wsd);

                WebWrite("success");

                return;

            }


            //用户取消关注事件
            if (wsd.Event == "unsubscribe")
            {

                GetWxAccount(wsd);

                WebWrite("success");

                return;
            }

            //用户每一次进来微信公众号获取用户地理位置事件
            if(wsd.Event == "LOCATION")
            {

                WX_ACCOUNT wx_account = GetWxAccount(wsd);


                wx_account.W_LONGITUDE = wsd.Longitude;
                wx_account.W_LATITUDE = wsd.Latitude;
                wx_account.W_PRECISION = wsd.Precision;
                wx_account.ROW_DATE_UPDATE = DateTime.Now;

                DbDecipher decipher = ModelAction.OpenDecipher();

                decipher.UpdateModelProps(wx_account, "W_LONGITUDE", "W_LATITUDE", "W_PRECISION", "ROW_DATE_UPDATE");


                WebWrite("success");


                return;

            }

        }


        /// <summary>
        /// 获取微信用户账号
        /// </summary>
        /// <param name="wsd">微信传上来的值对象</param>
        /// <returns></returns>
        WX_ACCOUNT GetWxAccount(WxSendData wsd)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(WX_ACCOUNT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("W_OPENID", wsd.FromUserName);
            lmFilter.Locks.Add(LockType.NoLock);

            WX_ACCOUNT wx_account = decipher.SelectToOneModel<WX_ACCOUNT>(lmFilter);

            if(wx_account != null)
            {

                return wx_account;

            }


            string url = GlobelParam.GetValue<string>("WX_MENU_API", @"http://yq.gzbeih.com/API", "微信公共API地址");

            string key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "WXHB", "在微信公共API中的关键字");


            url += "/GetUser.ashx";

            string result = string.Empty;

            NameValueCollection nv = new NameValueCollection();

            try
            {

                nv["openid"] = wsd.FromUserName;
                nv["key"] = key;

                using (WebClient wc = new WebClient())
                {

                    byte[] recData = wc.UploadValues(url, "POST", nv);

                    result = Encoding.UTF8.GetString(recData);

                    log.Info("获取微信用户信息返回来的数据！");

                    log.Info(result);

                }

                HttpResult sm_result = HttpResult.ParseJson(result);

                if (!sm_result.success)
                {
                    throw new Exception("哦噢，获取用户信息出错了！" + sm_result.error_msg);   
                }

                SModel data = sm_result.data;

                WeiXinUserInfo wxui = new WeiXinUserInfo();

                wx_account = new WX_ACCOUNT();

                wx_account.W_OPENID = wsd.FromUserName;
                wx_account.W_ADDRESS = data["province"] + "" + data["city"];
                wx_account.W_NICKNAME = data["nickname"];
                wx_account.SEX = data.Get<int>("sex") == 1 ? "男" : "女";
                wx_account.HEAD_IMG_URL = data["headimgurl"];
                wx_account.PK_W_CODE = BillIdentityMgr.NewCodeForDay("WX_ACCOUNT_CODE", "W", 6);


                LightModelFilter lmFilterAgain = new LightModelFilter(typeof(WX_ACCOUNT));
                lmFilterAgain.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilterAgain.And("W_OPENID", wsd.FromUserName);
                lmFilterAgain.Locks.Add(LockType.RowLock);

                decipher.BeginTransaction();

                WX_ACCOUNT wx_account_again = decipher.SelectToOneModel<WX_ACCOUNT>(lmFilterAgain);

                if (wx_account_again != null)
                {
                    decipher.TransactionCommit();

                    return wx_account_again;

                }

                decipher.InsertModel(wx_account);

                decipher.TransactionCommit();

                return wx_account;

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();
                log.Error(ex);
                throw ex;
            }

        }


        /// <summary>
        /// 用户关注公众号事件
        /// </summary>
        /// <param name="wsd">微信传上来的值对象</param>
        void Subscribe(WxSendData wsd)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(WX_ACCOUNT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("W_OPENID", wsd.FromUserName);

            WX_ACCOUNT wx_account = decipher.SelectToOneModel<WX_ACCOUNT>(lmFilter);
            //已经在数据库中的就不处理了
            if (wx_account != null)
            {
                return;
            }

            wx_account = GetWxAccount(wsd);

            //如果这个值为空就不处理了！
            if (string.IsNullOrWhiteSpace(wsd.EventKey))
            {

                return;

            }


            string p_id = wsd.EventKey.Substring(8);

            WX_ACCOUNT p_account = decipher.SelectModelByPk<WX_ACCOUNT>(p_id);


            wx_account.PARENT_W_CODE_1 = p_account.PK_W_CODE;
            wx_account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(wx_account, "PARENT_W_CODE_1", "ROW_DATE_UPDATE");

        }

        /// <summary>
        /// 创建文字类型xml字符串 
        /// </summary>
        /// <param name="openid">用户openid</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        string CreateXmlStrText(string openid,string content)
        {
            long create_time = DateUtil.ToTimestamp1970(DateTime.Now);


            string xml_str = "<xml>" +
                                $"<ToUserName><![CDATA[{openid}]]></ToUserName>" +
                                "<FromUserName><![CDATA[gh_0c73460b3ec0]]></FromUserName>" +
                                $"<CreateTime>{create_time}</CreateTime>" +
                                "<MsgType><![CDATA[text]]></MsgType>"+
                                $"<Content><![CDATA[{content}]]></Content>"+
                             "</xml>";

            return xml_str;

        }

        /// <summary>
        /// 创建图文类型 xml 字符串
        /// </summary>
        /// <param name="openid">用户openid</param>
        /// <param name="title">这是标题</param>
        /// <param name="description">图文消息描述</param>
        /// <param name="picurl">图片地址</param>
        /// <param name="url">点击跳转的地址</param>
        /// <returns></returns>
        string CreateXmlStrImg(string openid,string title,string description,string picurl,string url)
        {

            long create_time = DateUtil.ToTimestamp1970(DateTime.Now);

            string xml_str = "<xml>"+
                                $"<ToUserName><![CDATA[{openid}]]></ToUserName>"+
                                "<FromUserName><![CDATA[gh_0c73460b3ec0]]></FromUserName>" +
                                $"<CreateTime>{create_time}</CreateTime>" +
                                "<MsgType><![CDATA[news]]></MsgType>"+
                                "<ArticleCount>1</ArticleCount>"+
                                    "<Articles>"+
                                        "<item>"+
                                            $"<Title><![CDATA[{title}]]></Title>" +
                                            $"<Description><![CDATA[{description}]]></Description>" +
                                            $"<PicUrl><![CDATA[{picurl}]]></PicUrl>" +
                                            $"<Url><![CDATA[{url}]]></Url>"+
                                        "</item>"+
                                   "</Articles>"+
                            "</xml>";


            return xml_str;



        }


        void WebWrite(string msg)
        {

            HttpContext.Current.Response.Write(msg);

            log.Info("返回给微信的数据："+msg);


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