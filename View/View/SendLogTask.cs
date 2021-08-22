using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Threading;
using HWQ.Entity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace App.InfoGrid2.View
{


    public class SendLogTask : WebTask
    {

        /// <summary>
        /// 判断是否要发送日志信息
        /// </summary>
        public static class SendLogState
        {
            /// <summary>
            /// true -- 是有要发送的日志信息
            /// </summary>
            public static bool flag { get; set; }
        }


        public SendLogTask()
        {
            this.TaskSpan = new TimeSpan(0, 0, 20);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Parse("2011-1-1");
        }



       public  HttpApplicationState appliceation = null;


        


        public override void Exec()
        {
            bool flag = false;


            lock (appliceation)
            {


                if (appliceation["SEND_LOG_STATE"] == null)
                {
                    appliceation["SEND_LOG_STATE"] = false;
                }
                else
                {
                    flag = (bool)appliceation["SEND_LOG_STATE"];

                }

            }

            if (flag == false)
            {
                return;
            }


            DbDecipher decipher = null;

            try
            {
                decipher = DbDecipherManager.GetDecipherOpen();
            }
            catch (Exception ex)
            {
                return;
            }


            try {
                SendLogMessage(decipher);
            }
            catch (Exception ex)
            {
               

                try
                {

                    string path =  HttpContext.Current.Server.MapPath("/_Temporary");


                    

                    if (Directory.Exists(path) == false)
                    {
                        Directory.CreateDirectory(path);
                    }


                    StreamWriter sw = new StreamWriter(path + "\\logTest.txt", true);

                    sw.WriteLine(string.Format("时间：{0}。出错了！，很底层的错啦！错误消息：{1},详细信息：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"), ex.Message, ex.StackTrace));

                    sw.Close();

                    sw.Dispose();
                }
                catch (Exception e)
                {

                }
            }
            finally
            {
                decipher.Dispose();
            }
 
        }


        void SendLogMessage(DbDecipher decipher)
        {

            




            LightModelFilter lmFilter = new LightModelFilter(typeof(LOG4_SERVER));
            lmFilter.And("BIZ_SID",0);
            lmFilter.Top = 5;

            List<LOG4_SERVER> log4List = decipher.SelectModels<LOG4_SERVER>(lmFilter);


            if (log4List.Count == 0)
            {



                DateTime dateYue = DateTime.Now.AddDays(-10);

                LightModelFilter lmFilterDe = new LightModelFilter(typeof(LOG4_SERVER));
                lmFilterDe.And("LOG_DATE", dateYue, HWQ.Entity.Filter.Logic.LessThanOrEqual);

                ///删除掉十天以前的日志信息
                decipher.DeleteModels(lmFilterDe);




                appliceation["SEND_LOG_STATE"] = false;
                return;
            }


            string url = GlobelParam.GetValue<string>(decipher, "POST_LOG_URL", "http://ax43257.vicp.cc:809/View/ReceiveLog.aspx", "提交日志的服务器路径");


            foreach (LOG4_SERVER log4 in log4List)
            {
               PostLogMessage(log4, url);

               log4.BIZ_SID = 2;

               decipher.UpdateModelProps(log4, "BIZ_SID");

            }

        }


        void PostLogMessage(LOG4_SERVER log4, string url)
        {


            string json = ModelConvert.ToJson(log4);

            BinaryFormatter bf = new BinaryFormatter();


            MemoryStream stream = new MemoryStream();


            ProtoBuf.Serializer.Serialize(stream, log4);



            try
            {


                WebClient wc = new WebClient();


                byte[] jsons = wc.UploadData(url, "POST", stream.ToArray());

                stream.Close();
                stream.Dispose();

                return;


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}