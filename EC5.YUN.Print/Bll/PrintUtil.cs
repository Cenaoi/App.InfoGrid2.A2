using App.BizCommon;
using EasyClick.BizWeb2;
using EC5.YUN.Print.Model;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EC5.YUN.Print.Bll
{
    /// <summary>
    /// 打印帮助类
    /// </summary>
   public static class PrintUtil
    {


        /// <summary>
        /// 飞蛾打印机的接口ip地址
        /// </summary>
        public static string m_ip = "http://115.28.225.82/FeieServer/";




        //   <C></C> 是居中    <BR />是换行符    <B></B> 是变粗   <CB></CB> 居中并变粗


        /// <summary>
        /// 发送打印内容到打印机上
        /// </summary>
        /// <param name="yp">打印数据对象</param>
        /// <returns>json 数据 </returns>
        public static string SendMessage(YUN_DEVICH_V2 yp)
        {
            try
            {

                //拿前台的打印机编码
                string url = GlobelParam.GetValue<string>("SEND_PRINT_PATH", "http://127.0.0.1:55972/", "发送打印数据的地址");

                url += "App/PrintMgr/SendData/PrintAction.aspx";

                BinaryFormatter bf = new BinaryFormatter();


                MemoryStream stream = new MemoryStream();


                ProtoBuf.Serializer.Serialize(stream, yp);


                WebClient wc = new WebClient();

                byte[] json = wc.UploadData(url, "POST", stream.ToArray());

                wc.Dispose();

                stream.Close();

                stream.Dispose();

                string cont = Encoding.UTF8.GetString(json);

                return cont;
            }
            catch (Exception ex)
            {
                throw new Exception("打印小票出错了！", ex);
            }
        }

        /// <summary>
        /// 生成厨房的打印内容
        /// </summary>
        /// <param name="fenlei">台号：桌号</param>
        /// <param name="lm001">桌子信息</param>
        /// <param name="lm004">一份菜品信息</param>
        /// <returns>字符串</returns>
        public static string CreateKitchenContent(string fenlei, LModel lm001, LModel lm004)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ROOT v='1'>");

            sb.AppendFormat("<CB>桌位:{0}/{1}</CB><BR />", fenlei, lm001["COL_1"]);
            sb.AppendFormat("时间：{0}<BR />", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendFormat("<B>名称　　　        数量    备注</B><BR />");
            sb.AppendFormat("--------------------------------<BR />");



            //数量
            string COL_3 = lm004.Get<decimal>("COL_3").ToString("F0");
            //备注
            string COL_6 = lm004.Get<string>("COL_6");
            //名称
            string name = lm004.Get<string>("COL_1");

            string name1, name2 = string.Empty;

            //字符长度
            int len = Encoding.Default.GetByteCount(name);


            //名称超过7个字符就放到下一行去
            if (len > 18)
            {

                name1 = name.Substring(0, 18);
                name2 = name.Substring(18);
            }
            else
            {

                name1 = name + "".PadRight(18 - len, ' ');
            }

            sb.AppendFormat("<B>{0}{1}{2}</B><BR />", name1, COL_3.PadRight(8, ' '), COL_6);

            if (!string.IsNullOrEmpty(name2))
            {
                sb.AppendFormat("<B>{0}</B><BR />", name2);
            }

            sb.AppendFormat("--------------------------------<BR />");


            sb.Append("</ROOT>");


            return sb.ToString();
        }


        /// <summary>
        /// 生成点菜单内容  58纸张 是 16个字符一行的  空格算半个字符
        /// </summary>
        /// <param name="fenlei">分类 大厅 包间</param>
        /// <param name="lm001">桌位表信息</param>
        /// <param name="lm004List">商品明细表</param>
        /// <param name="lm011">餐厅信息</param>
        public static string CreateConsumptionContent(string fenlei, LModel lm001, List<LModel> lm004List, LModel lm011)
        {
 
            StringBuilder sb = new StringBuilder();

            sb.Append("<ROOT v='1'>");

            sb.AppendFormat("<CB>{0}</CB><BR />", lm011["COL_1"]);
            sb.AppendFormat("<C>(点菜单)</C><BR />");
            sb.AppendFormat("--------------------------------<BR />");
            sb.AppendFormat("订单号:{0}  <BR />", lm001["COL_4"]);
            sb.AppendFormat("台号:{0}/{1}<BR />", fenlei, lm001["COL_1"]);
            sb.AppendFormat("就餐人数:{0}<BR />", lm001["COL_5"]);
            sb.AppendFormat("--------------------------------<BR />");
            sb.AppendFormat("<B>序名称　　　   单价  数量 金额</B><BR />");
            sb.AppendFormat("--------------------------------<BR />");

            //序号来的
            int i = 0;

            //总计
            decimal total = 0;



            foreach (var item in lm004List)
            {
                //单价
                string COL_2 = string.Empty;
                //金额
                decimal num = 0;


                COL_2 = item.Get<decimal>("COL_2").ToString("F2");

                num = item.Get<decimal>("COL_2") * item.Get<decimal>("COL_3");




                //数量
                string COL_3 = item.Get<decimal>("COL_3").ToString("F0");

                //名称
                string name = item.Get<string>("COL_1");


                string name1, name2 = string.Empty;


                //字符长度
                int len = Encoding.Default.GetByteCount(name);


                //名称超过7个字符就放到下一行去
                if (len > 14)
                {

                    name1 = name.Substring(0, 14);
                    name2 = name.Substring(14);
                }
                else
                {

                    name1 = name + "".PadRight(14 - len, ' ');
                }

                sb.AppendFormat("<B>{0}{1}{2}{3}{4}</B><BR />", ++i, name1, COL_2.PadRight(7, ' '), COL_3.PadRight(4, ' '), num.ToString("F2"));

                if (!string.IsNullOrEmpty(name2))
                {
                    sb.AppendFormat("<B> {0}<B><BR />", name2);
                }

                total += num;

                sb.AppendFormat("--------------------------------<BR/>");
            }



            sb.AppendFormat("<B>总计:{0}</B><BR />", total.ToString("F2"));


            sb.Append("</ROOT>");

            return sb.ToString();
        }


        /// <summary>
        /// 生成收银订单内容  58纸张 是 16个字符一行的
        /// </summary>
        /// <param name="fenlei">分类 大厅 包间</param>
        /// <param name="lm001">桌位表信息</param>
        /// <param name="lm004List">商品明细表</param>
        /// <param name="lm007">结账单信息</param>
        /// <param name="lm011">餐厅信息</param>
        public static string CreateOrderContent(string fenlei, LModel lm001, List<LModel> lm004List, LModel lm007, LModel lm011)
        {


            StringBuilder sb = new StringBuilder();

            sb.Append("<ROOT v='1'>");

            sb.AppendFormat("<CB>{0}</CB><BR />", lm011["COL_1"]);
            sb.AppendFormat("<C>(结算单)</C>");
            sb.AppendFormat("--------------------------------<BR />");
            sb.AppendFormat("订单号:{0}  <BR />", lm001["COL_4"]);
            sb.AppendFormat("台号:{0}/{1}<BR />", fenlei, lm001["COL_1"]);
            sb.AppendFormat("就餐人数:{0}<BR />", lm001["COL_5"]);
            sb.AppendFormat("收银员:{0}  {1}<BR />", BizServer.LoginName, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendFormat("--------------------------------<BR />");
            sb.AppendFormat("<B>序名称　　　   单价  数量 金额</B><BR />");
            sb.AppendFormat("--------------------------------<BR />");

            //序号来的
            int i = 0;


            foreach (var item in lm004List)
            {
                //单价
                string COL_2 = string.Empty;
                //金额
                decimal num = 0;

                COL_2 = item.Get<decimal>("COL_2").ToString("F2");

                num = item.Get<decimal>("COL_2") * item.Get<decimal>("COL_3");


                //数量
                string COL_3 = item.Get<decimal>("COL_3").ToString("F0");
                //名称
                string name = item.Get<string>("COL_1");

                string name1, name2 = string.Empty;


                //字符长度
                int len = Encoding.Default.GetByteCount(name);

                //名称超过7个字符就放到下一行去
                if (len > 14)
                {

                    name1 = name.Substring(0, 14);
                    name2 = name.Substring(14);
                }
                else
                {

                    name1 = name + "".PadRight(14 - len, ' ');
                }

                sb.AppendFormat("{0}{1}{2}{3}{4}<BR />", ++i, name1, COL_2.PadRight(7, ' '), COL_3.PadRight(4, ' '), num.ToString("F2"));

                if (!string.IsNullOrEmpty(name2))
                {
                    sb.AppendFormat(" {0}<BR />", name2);
                }

            }

            sb.AppendFormat("--------------------------------<BR />");
            sb.AppendFormat("合计:{0}<BR />", lm007.Get<decimal>("COL_3").ToString("F2"));
            sb.AppendFormat("实收:{0}<BR />", lm007.Get<decimal>("COL_5").ToString("F2"));
            sb.AppendFormat("找零:{0}<BR />", lm007.Get<decimal>("COL_6").ToString("F2"));
            sb.AppendFormat("--------------------------------<BR />");
            sb.AppendFormat("电话:{0}<BR />", lm011["COL_2"]);
            sb.AppendFormat("地址:{0}<BR />", lm011["COL_7"]);
            sb.AppendFormat("<CB>欢迎下次光临</CB><BR />");

            sb.Append("</ROOT>");

            return sb.ToString();
        }


        #region 飞蛾打印机的方法


        /// <summary>
        /// 发送打印内容到打印机上 飞蛾
        /// </summary>
        /// <param name="lm012">打印对象</param>
        /// <returns></returns>
        public static string SendMessage_FE(LModel lm012)
        {
            string url = m_ip + "printOrderAction";

            NameValueCollection nv = new NameValueCollection();


            nv["sn"] = lm012.Get<string>("PRINT_SN");  

            nv["key"] = lm012.Get<string>("PRINT_KEY");  

            nv["printContent"] = lm012.Get<string>("PRINT_CONTENT");

            WebClient wc = new WebClient();

            byte[] json = wc.UploadValues(url, "POST", nv);

            string cont = Encoding.UTF8.GetString(json);

            return cont;

        }


        /// <summary>
        /// 获取打印机状态  飞蛾
        ///----------服务器返回的结果有如下几种：----------
        ///{"responseCode":0,"msg":"已打印"}
        ///{"responseCode":0,"msg":"未打印"}
        ///{"responseCode":1,"msg":"请求参数错误"}
        ///{"responseCode":2,"msg":"没有找到该索引的订单"}
        /// </summary>
        /// <param name="orderIndex">订单索引，打印机服务器返回来的</param>
        /// <returns>json数据</returns>
        public static string GetPrintStatus_FE(string orderIndex,string sn,string key)
        {
            string url = m_ip + "queryOrderStateAction";

            NameValueCollection nv = new NameValueCollection();
            nv["sn"] = sn;

            nv["key"] = key;

            nv["index"] = orderIndex;

            WebClient wc = new WebClient();

            byte[] json = wc.UploadValues(url, "POST", nv);

            string cont = Encoding.UTF8.GetString(json);

            return cont;

        }



        /// <summary>
        /// 生成收银订单内容  58纸张 是 16个字符一行的  飞蛾
        /// </summary>
        /// <param name="fenlei">分类 大厅 包间</param>
        /// <param name="lm001">桌位表信息</param>
        /// <param name="lm004List">商品明细表</param>
        /// <param name="lm007">结账单信息</param>
        /// <param name="lm011">餐厅信息</param>
        /// <returns>订单内容</returns>
        public static string CreateOrderContent_FE(string fenlei, LModel lm001, List<LModel> lm004List, LModel lm007, LModel lm011)
        {


            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<CB>{0}</CB><BR>", lm011["COL_1"]);
            sb.AppendFormat("<C>(结算单)</C>");
            sb.AppendFormat("--------------------------------<BR>");
            sb.AppendFormat("订单号:{0}  <BR>", lm001["COL_4"]);
            sb.AppendFormat("台号:{0}/{1}<BR>", fenlei, lm001["COL_1"]);
            sb.AppendFormat("就餐人数:{0}<BR>", lm001["COL_5"]);
            sb.AppendFormat("收银员:{0}  {1}<BR>", BizServer.LoginName, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendFormat("--------------------------------<BR>");
            sb.AppendFormat("序名称　　　   单价  数量 金额<BR>");
            sb.AppendFormat("--------------------------------<BR>");

            //序号来的
            int i = 0;


            foreach (var item in lm004List)
            {
                //单价
                string COL_2 = string.Empty;
                //金额
                decimal num = 0;

                COL_2 = item.Get<decimal>("COL_2").ToString("F2");

                num = item.Get<decimal>("COL_2") * item.Get<decimal>("COL_3");


                //数量
                string COL_3 = item.Get<decimal>("COL_3").ToString("F0");
                //名称
                string name = item.Get<string>("COL_1");

                string name1, name2 = string.Empty;


                //字符长度
                int len = Encoding.Default.GetByteCount(name);

                //名称超过7个字符就放到下一行去
                if (len > 14)
                {

                    name1 = name.Substring(0, 14);
                    name2 = name.Substring(14);
                }
                else
                {

                    name1 = name + "".PadRight(14 - len, ' ');
                }

                sb.AppendFormat("{0}{1}{2}{3}{4}<BR>", ++i, name1, COL_2.PadRight(7, ' '), COL_3.PadRight(4, ' '), num.ToString("F2"));

                if (!string.IsNullOrEmpty(name2))
                {
                    sb.AppendFormat(" {0}<BR>", name2);
                }

            }

            sb.AppendFormat("--------------------------------<BR>");
            sb.AppendFormat("合计:{0}<BR>", lm007.Get<decimal>("COL_3").ToString("F2"));
            sb.AppendFormat("实收:{0}<BR>", lm007.Get<decimal>("COL_5").ToString("F2"));
            sb.AppendFormat("找零:{0}<BR>", lm007.Get<decimal>("COL_6").ToString("F2"));
            sb.AppendFormat("--------------------------------<BR>");
            sb.AppendFormat("电话:{0}<BR>", lm011["COL_2"]);
            sb.AppendFormat("地址:{0}<BR>", lm011["COL_7"]);
            sb.AppendFormat("<CB>欢迎下次光临</CB><BR>");


            return sb.ToString();
        }


        /// <summary>
        /// 生成厨房的打印内容 飞蛾
        /// </summary>
        /// <param name="fenlei">分类信息</param>
        /// <param name="lm001">桌子信息</param>
        /// <param name="lm004">商品信息</param>
        /// <returns></returns>
        public static string CreateKitchenContent_FE(string fenlei, LModel lm001, LModel lm004)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<CB>桌位:{0}/{1}</CB><BR>", fenlei, lm001["COL_1"]);
            sb.AppendFormat("时间：{0}<BR>", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendFormat("<L>名称　　　        数量    备注</L><BR>");
            sb.AppendFormat("--------------------------------<BR>");



            //数量
            string COL_3 = lm004.Get<decimal>("COL_3").ToString("F0");
            //备注
            string COL_6 = lm004.Get<string>("COL_6");
            //名称
            string name = lm004.Get<string>("COL_1");

            string name1, name2 = string.Empty;

            //字符长度
            int len = Encoding.Default.GetByteCount(name);


            //名称超过7个字符就放到下一行去
            if (len > 18)
            {

                name1 = name.Substring(0, 18);
                name2 = name.Substring(18);
            }
            else
            {

                name1 = name + "".PadRight(18 - len, ' ');
            }

            sb.AppendFormat("<L>{0}{1}{2}</L><BR>", name1, COL_3.PadRight(8, ' '), COL_6);

            if (!string.IsNullOrEmpty(name2))
            {
                sb.AppendFormat("<L>{0}</L><BR>", name2);
            }

            sb.AppendFormat("--------------------------------<BR>");



            return sb.ToString();
        }


        /// <summary>
        /// 生成点菜单内容  58纸张 是 16个字符一行的  空格算半个字符 飞蛾
        /// </summary>
        /// <param name="fenlei">分类 大厅 包间</param>
        /// <param name="lm001">桌位表信息</param>
        /// <param name="lm004List">商品明细表</param>
        /// <param name="lm011">餐厅信息</param>
        public static string CreateConsumptionContent_FE(string fenlei, LModel lm001, List<LModel> lm004List, LModel lm011)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<CB>{0}</CB><BR>", lm011["COL_1"]);
            sb.AppendFormat("<C>(点菜单)</C>");
            sb.AppendFormat("--------------------------------<BR>");
            sb.AppendFormat("订单号:{0}  <BR>", lm001["COL_4"]);
            sb.AppendFormat("台号:{0}/{1}<BR>", fenlei, lm001["COL_1"]);
            sb.AppendFormat("就餐人数:{0}<BR>", lm001["COL_5"]);
            sb.AppendFormat("--------------------------------<BR>");
            sb.AppendFormat("<L>序名称　　　   单价  数量 金额</L><BR>");
            sb.AppendFormat("--------------------------------<BR>");

            //序号来的
            int i = 0;

            //总计
            decimal total = 0;



            foreach (var item in lm004List)
            {
                //单价
                string COL_2 = string.Empty;
                //金额
                decimal num = 0;


                COL_2 = item.Get<decimal>("COL_2").ToString("F2");

                num = item.Get<decimal>("COL_2") * item.Get<decimal>("COL_3");




                //数量
                string COL_3 = item.Get<decimal>("COL_3").ToString("F0");

                //名称
                string name = item.Get<string>("COL_1");


                string name1, name2 = string.Empty;


                //字符长度
                int len = Encoding.Default.GetByteCount(name);


                //名称超过7个字符就放到下一行去
                if (len > 14)
                {

                    name1 = name.Substring(0, 14);
                    name2 = name.Substring(14);
                }
                else
                {

                    name1 = name + "".PadRight(14 - len, ' ');
                }

                sb.AppendFormat("<L>{0}{1}{2}{3}{4}</L><BR>", ++i, name1, COL_2.PadRight(7, ' '), COL_3.PadRight(4, ' '), num.ToString("F2"));

                if (!string.IsNullOrEmpty(name2))
                {
                    sb.AppendFormat("<L> {0}<L><BR>", name2);
                }

                total += num;

                sb.AppendFormat("--------------------------------<BR>");
            }



            sb.AppendFormat("<B>总计:{0}</B><BR>", total.ToString("F2"));


            return sb.ToString();
        }


        #endregion



        /// <summary>
        /// 创建打印列表的数据
        /// </summary>
        /// <param name="lm010">打印机实体</param>
        /// <param name="col_4">卡号</param>
        /// <param name="content">要打印的内容</param>
        /// <param name="type_id">印类型，0--厨房单，2--点菜单，4--消费单</param>
        /// <returns>UT_012 实体</returns>
        public static LModel CreatLm012(LModel lm010,string col_4,string content,int type_id)
        {
            
            LModel lm012 = new LModel("UT_012");

            lm012["ROW_DATE_CREATE"] = DateTime.Now;
            lm012["PRINT_SN"] = lm010["COL_10"];
            lm012["PRINT_KEY"] = lm010["COL_8"];
            lm012["DEV_IP"] = lm010["COL_7"];
            lm012["DEV_SIZE"] = lm010["COL_11"];
            lm012["DEV_TYPE"] = lm010["COL_12"];
            lm012["ORDER_NO"] = col_4;
            lm012["PRINT_CODE"] = lm010["COL_5"];
            lm012["PRINT_TYPE"] = type_id;
            lm012["PRINT_CONTENT"] = content;
            lm012["ROW_AUTHOR_USER_CODE"] = BizServer.UserCode;
            lm012["PRINT_USER_NAME"] = BizServer.LoginName;

            return lm012;

        }

        /// <summary>
        /// 根据Guid 来获取桌子信息
        /// </summary>
        /// <param name="gid">Guid</param>
        /// <param name="BIZ_SID">桌子状态 0--空桌 2--预定 4--使用  默认是 0 </param>
        /// <returns></returns>
        public static LModel GetLm001(Guid gid,int BIZ_SID = 0)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_001");
            lmFilter.And("GID", gid);
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID",BIZ_SID);

            LModel lm001 = decipher.GetModel(lmFilter);

            return lm001;

        }


        /// <summary>
        /// 根据编码获取打印机信息
        /// </summary>
        /// <param name="code">打印机编码</param>
        /// <returns></returns>
        public static LModel GetLm010(string code)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter010 = new LightModelFilter("UT_010");
            lmFilter010.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter010.And("COL_5", code);

            LModel lm010 = decipher.GetModel(lmFilter010);

            return lm010;

        }

        



    }
}
