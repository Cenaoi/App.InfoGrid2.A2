using App.BizCommon;
using App.InfoGrid2.JF.WeChat.Model;
using EC5.Utility;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace App.InfoGrid2.JF.Bll
{
    /// <summary>
    /// 微信助手
    /// </summary>
    public class WeChatUtil
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="appid">公众号唯一标识</param>
        /// <param name="appSecret">应用密钥</param>
        /// <param name="code">授权编码</param>
        /// <returns></returns>
        public static WeiXinUserInfo GetWeChatUserInfo(string appid, string appSecret, string code)
        {

            //获取accessToken的地址
            string accessTokenurl = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={appSecret}&code={code}&grant_type=authorization_code";


            WebClient wc = new WebClient();

            byte[] bResponse = wc.DownloadData(accessTokenurl);

            string strResponse = Encoding.UTF8.GetString(bResponse);


            log.Info($"返回来的json是：{strResponse}");

            //通过code获取的access token 对象
            WeiXinAccessTokenModel wxatm = null;

            try
            {

                wxatm = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(strResponse);

            }
            catch (Exception ex)
            {
                log.Error("根据code来获取access token 出错了!", ex);
                throw new Exception("没授权不能登录微信界面！", ex);
            }

            //通过 access token 和 openid 获取用户信息的接口
            string userInfoUrl = $"https://api.weixin.qq.com/sns/userinfo?access_token={wxatm.access_token}&openid={wxatm.openid}&lang=zh_CN";


            //通过 access token 和 openid 获取用户信息返回来的json
            string json = string.Empty;

            log.Info($"通过 access token 和 openid 获取用户信息的接口:{userInfoUrl}");

            try
            {

                json = Encoding.UTF8.GetString(wc.DownloadData(userInfoUrl));

            }
            catch (Exception ex)
            {
                log.Error($"调用 通过 access token 和 openid 获取用户信息的接口出错了！url:{userInfoUrl}", ex);
                throw new Exception("没授权不能登录微信界面！", ex);
            }


            //通过access token 和 openid 获取的用户信息
            WeiXinUserInfo wxuserInfo = null;

            log.Info($"用户信息的json是：{json}");


            try
            {

                wxuserInfo = JsonConvert.DeserializeObject<WeiXinUserInfo>(json);

                return wxuserInfo;

            }
            catch (Exception ex)
            {
                log.Error($"序列化  通过access token 和 openid 获取的用户信息 时出错了！json：{json}", ex);
                throw new Exception("没授权不能登录微信界面！", ex);

            }

        }


        /// <summary>
        /// 收款函数
        /// </summary>
        /// <param name="body">商品名称</param>
        /// <param name="out_trade_no">订单编号 BillIdentityMgr.NewCodeForYear("WE_CHAT", "WX", 8)</param>
        /// <param name="total_fee">总金额 以分为单位 正常金额要记得乘以100</param>
        /// <param name="ip">IP地址</param>
        /// <param name="notify_url">回调url</param>
        /// <param name="openid">微信用户的openid</param>
        /// <returns></returns>
        public static SModel Receivables(string body, string out_trade_no, decimal total_fee, string ip, string notify_url, string openid = "ohkCww1Fg0-UdVnISI6hvOSgAlnA")
        {
            SModel sm = new SModel();


            string appid = GlobelParam.GetValue<string>("WX_APPID", "wxfa3fa4c793ea10cc", "微信公众号APPID");

            string mch_id = GlobelParam.GetValue<string>("WX_MCH_ID", "1389072202", "微信支付商户号");


            sm["appid"] = appid;
            sm["mch_id"] = mch_id;
            sm["nonce_str"] = GetNonce();
            sm["body"] = body;                                                                  //商品名称
            sm["out_trade_no"] = out_trade_no;                                                  //订单编号
            sm["total_fee"] = total_fee.ToString("0");                                          //总金额
            sm["spbill_create_ip"] = ip;
            sm["notify_url"] = notify_url;
            sm["trade_type"] = "JSAPI";
            sm["openid"] = openid;

            log.Debug("准备加密的数据：");
            log.Debug(sm.ToJson());



            sm["sign"] = GetSign(sm);

            byte[] data = GetByteBySm(sm);

            WebClient wc = new WebClient();

            try
            {


                //这是提交键值对类型的
                byte[] json = wc.UploadData("https://api.mch.weixin.qq.com/pay/unifiedorder", data);

                string result = Encoding.UTF8.GetString(json);

                log.Debug("统一下单返回来的信息：" + result);

                ReceResult rr = GetResultObj<ReceResult>(result);

                TimeSpan span = DateTime.Now - new DateTime(1970, 1, 1);

                int ss = (int)span.TotalSeconds;

                SModel sm_result = new SModel();
                sm_result["appId"] = rr.appid;
                sm_result["timeStamp"] = ss.ToString();
                sm_result["nonceStr"] = GetNonce();
                sm_result["package"] = "prepay_id=" + rr.prepay_id;
                sm_result["signType"] = "MD5";
                sm_result["paySign"] = GetSign(sm_result);


                log.Debug("返回界面上的json数据：" + sm_result.ToJson());


                return sm_result;

            }
            catch (Exception ex)
            {
                log.Error("出错了！", ex);

                throw ex;

            }
            finally
            {

                wc.Dispose();

            }

        }


        /// <summary>
        /// 企业账号付款给微信用户
        /// </summary>
        /// <param name="desc">付款描述</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="total_fee">总金额 最少1元  以分为单位</param>
        /// <param name="ip">ip地址</param>
        /// <param name="user_name">转账用户名</param>
        /// <param name="openid">微信openid</param>
        /// <param name="sned_json">发送json</param>
        /// <param name="out_data">微信返回来的数据</param>
        /// <returns></returns>
        public static PayResult Pay(string desc, string out_trade_no, decimal total_fee, string ip, string user_name, out string sned_json, out string out_data, string openid = "ohkCww1Fg0-UdVnISI6hvOSgAlnA")
        {
            //企业账号付款给用户接口
            string api_url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

            SModel sm = new SModel();

            string appid = GlobelParam.GetValue<string>("WX_APPID", "wxfa3fa4c793ea10cc", "微信公众号APPID");

            string mch_id = GlobelParam.GetValue<string>("WX_MCH_ID", "1389072202", "微信支付商户号");


            sm["mch_appid"] = appid;                                                    //公众账号 appid
            sm["mchid"] = mch_id;                                                       //商户号
            sm["openid"] = openid;                                                         //用户的唯一标志码
            sm["nonce_str"] = GetNonce();
            sm["partner_trade_no"] = out_trade_no;                                         //商户订单号
            sm["check_name"] = "FORCE_CHECK";                                              //强制校验用户姓名
            sm["re_user_name"] = user_name;                                                 //收款用户姓名
            sm["amount"] = total_fee.ToString("0");                                           //金额
            sm["desc"] = desc;                                                             //企业付款描述信息
            sm["spbill_create_ip"] = ip;                                                   //Ip地址
            sm["sign"] = GetSign(sm);

            sned_json = sm.ToJson();

            byte[] data = GetByteBySm(sm, "C:\\b.xml");

            //证书地址
            string cert = GlobelParam.GetValue<string>("CERT_ADDRESS", @"C:\cert\apiclient_cert.p12", "企业付款api要用的证书地址");
            //证书密码
            string password = mch_id;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //X509Certificate cer = new X509Certificate(cert, password);



            //发送上服务器要用这个  一定要记住
            X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            log.Debug("这个是证书的信息：" + cer.Issuer + " -- " + cer.Subject);

            try
            {

                log.Debug("准备发数据给微信了！");

                log.Debug("发送的数据：" + sned_json);

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(api_url);

                webrequest.ClientCertificates.Add(cer);
                webrequest.Method = "post";

                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);//写入参数 
                newStream.Close();

                HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
                Stream stream = webreponse.GetResponseStream();
                string resp = string.Empty;
                using (StreamReader reader = new StreamReader(stream))
                {
                    resp = reader.ReadToEnd();
                }

                log.Debug("回来的结果： " + resp);

                out_data = resp;

                PayResult pr = GetResultObj<PayResult>(resp);

                return pr;



            }
            catch (Exception ex)
            {
                log.Error("出错了！", ex);

                throw ex;

            }
        }


        static T GetResultObj<T>(string result)
        {
            byte[] array = Encoding.UTF8.GetBytes(result);

            MemoryStream stream = new MemoryStream(array);

            XmlSerializer xmlSearializer = new XmlSerializer(typeof(T), new XmlRootAttribute("xml"));

            T rr = (T)xmlSearializer.Deserialize(stream);

            return rr;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="sm">键值对实体</param>
        /// <param name="key">支付安全秘钥</param>
        /// <returns></returns>
        static string GetSign(SModel sm, string key = "psdfnjetu5421478434187dsfds41d7s")
        {
            SortedList<string, string> dic = new SortedList<string, string>();

            foreach (string name in sm.GetFields())
            {

                string value = (string)sm[name];

                dic.Add(name, value);

            }


            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (var item in dic)
            {

                if (i++ > 0)
                {
                    sb.Append("&");
                }

                sb.Append(item.Key + "=" + item.Value);
            }


            sb.Append("&key=" + key);

            string stringSignTemp = sb.ToString();



            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(stringSignTemp));

            StringBuilder sb1 = new StringBuilder();

            foreach (byte b in bs)
            {
                sb1.Append(b.ToString("x2"));
            }


            return sb1.ToString().ToUpper();
        }

        static char[] abcd = "0123456789qwertyuiopasdfghjklzxcvbnm".ToCharArray();

        /// <summary>
        /// 把sm对象转换成数据流
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="xml_path">保存xml的地址  默认C:\\a.xml</param>
        /// <returns></returns>
        static byte[] GetByteBySm(SModel sm, string xml_path = "C:\\a.xml")
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");
            XmlElement root = doc.DocumentElement;
            foreach (string name in sm.GetFields())
            {
                //sbXml.AppendFormat("<{0}>{1}</{0}>",name,sm[name]).AppendLine();

                XmlNode node = doc.CreateElement(name);
                node.InnerText = (string)sm[name];
                root.AppendChild(node);

            }

            string xml = XmlUtil.ToString(doc);

            byte[] data = Encoding.UTF8.GetBytes(xml);

            //File.WriteAllBytes(xml_path, data);

            return data;

        }

        /// <summary>
        /// 获取随机字符串，不长于32位
        /// </summary>
        /// <returns></returns>
        static string GetNonce()
        {

            StringBuilder sb = new StringBuilder(32);

            for (int i = 0; i < 32; i++)
            {
                sb.Append(abcd[RandomUtil.Next(abcd.Length)]);
            }

            return sb.ToString();

        }

        static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        /// <summary>
        /// 获取微信用户json字符串
        /// </summary>
        /// <param name="appid">公众号唯一标识</param>
        /// <param name="appSecret">应用密钥</param>
        /// <param name="code">授权编码</param>
        /// <returns></returns>
        public static string GetWXUserJsonStr(string appid, string appSecret, string code)
        {
            //获取accessToken的地址
            string accessTokenurl = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={appSecret}&code={code}&grant_type=authorization_code";


            WebClient wc = new WebClient();

            byte[] bResponse = wc.DownloadData(accessTokenurl);

            string strResponse = Encoding.UTF8.GetString(bResponse);


            log.Info($"返回来的json是：{strResponse}");

            //通过code获取的access token 对象
            WeiXinAccessTokenModel wxatm = null;

            try
            {

                wxatm = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(strResponse);

            }
            catch (Exception ex)
            {
                log.Error("根据code来获取access token 出错了!", ex);
                throw new Exception("没授权不能登录微信界面！", ex);
            }

            //通过 access token 和 openid 获取用户信息的接口
            string userInfoUrl = $"https://api.weixin.qq.com/sns/userinfo?access_token={wxatm.access_token}&openid={wxatm.openid}&lang=zh_CN";


            //通过 access token 和 openid 获取用户信息返回来的json
            string json = string.Empty;

            log.Info($"通过 access token 和 openid 获取用户信息的接口:{userInfoUrl}");

            try
            {

                json = Encoding.UTF8.GetString(wc.DownloadData(userInfoUrl));

                return json;

            }
            catch (Exception ex)
            {
                log.Error($"调用 通过 access token 和 openid 获取用户信息的接口出错了！url:{userInfoUrl}", ex);
                throw new Exception("没授权不能登录微信界面！", ex);
            }

        }




    }
}