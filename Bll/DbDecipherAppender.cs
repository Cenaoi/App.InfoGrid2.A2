using App.InfoGrid2.Model;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Transactions;

namespace App.InfoGrid2.Bll
{
    public class DbDecipherAppender : AppenderSkeleton
    {

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {


            if (string.IsNullOrEmpty(DbDecipherManager.DefaultDecipherName))
            {
                return;
            }

            if(Transaction.Current == null)
            {
                baseAppend(loggingEvent);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;
                tOpt.Timeout = new TimeSpan(0, 2, 0);

                using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Suppress, tOpt))
                {
                    baseAppend(loggingEvent);
                }
            }

        }

        private void baseAppend(log4net.Core.LoggingEvent loggingEvent)
        {

            HttpContext content = HttpContext.Current;


            EcUserState user = (content != null && content.Session != null) ? EcContext.Current.User : null;

            DbDecipher decipher = null;

            using(decipher = DbDecipherManager.GetDecipherOpen())
            {
                try
                {
                    InsertRow(loggingEvent, content, user, decipher);
                }
                catch (Exception ex)
                {

                    try
                    {

                        string path = content.Server.MapPath("/_Temporary");

                        if (Directory.Exists(path) == false)
                        {
                            Directory.CreateDirectory(path);
                        }


                        StreamWriter sw = new StreamWriter(path + "\\logTest.txt", true);

                        sw.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff} 很底层的错啦！错误消息：{1}, 详细信息：{2}", DateTime.Now, ex.Message, ex.StackTrace);

                        sw.Close();

                        sw.Dispose();
                    }
                    catch (Exception e)
                    {

                    }

                    throw ex;

                }
                finally
                {

                }
            }


            if (content != null)
            {
                lock (content.Application)
                {
                    content.Application["SEND_LOG_STATE"] = true;
                }
            }
        }

        private void InsertRow(log4net.Core.LoggingEvent loggingEvent, HttpContext content,EcUserState user, DbDecipher decipher)
        {

            string url = string.Empty;
            string browserName = string.Empty;
            string browserEdition = string.Empty;
            string host = string.Empty;

            if (content != null && content.Session != null)
            {
                url = Convert.ToString(content.Items["EC_ViewUri"]) ;

                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;

                ///获取浏览器型号和版本
                browserName = bc.Browser.ToString();
                browserEdition = bc.Version.ToString();

                ///获取服务器名
                host = Convert.ToString(content.Request.ServerVariables.Get("Server_Name"));

            }
            

            string exception = string.Empty;

            string messagex = string.Empty;

            if (loggingEvent.ExceptionObject != null)
            {


                ///错误消息
                exception = loggingEvent.ExceptionObject.Message + "\r\n" + loggingEvent.ExceptionObject.StackTrace;

                ///错误消息
                messagex = loggingEvent.RenderedMessage;
            }
            else
            {
                ///错误消息
                exception = loggingEvent.RenderedMessage;

                if (loggingEvent.MessageObject is System.Exception)
                {

                    ///错误消息
                    messagex = ((System.Exception)(loggingEvent.MessageObject)).Message;
                }
                else
                {
                    messagex = loggingEvent.MessageObject.ToString();
                }


            }


            //if (content != null && content.Session != null)
            //{
            //    LightModelFilter lmFilter = new LightModelFilter(typeof(LOG4_SERVER));
            //    lmFilter.And("SESSION_ID", content.Session.SessionID);
            //    lmFilter.TSqlOrderBy = "LOG4_SERVER_ID desc";
            //    lmFilter.Top = 1;


            //    LOG4_SERVER log4Old = decipher.SelectToOneModel<LOG4_SERVER>(lmFilter);

            //    if (log4Old != null && log4Old.EXCEPTION == exception)
            //    {
            //        return;
            //    }
            //}

            LOG4_SERVER log4 = new LOG4_SERVER();
            
            log4.COMPANY_NAME = BizCompanyMgr.GetName();

            if (content != null && content.Session != null)
            {
                log4.SESSION_ID = content.Session.SessionID;
            }
            
            log4.BIZ_SID = 0;
            log4.URL = url;            
            log4.EXCEPTION = exception;            
            log4.MESSAGE = messagex;

            if (user != null)
            {
                log4.U_LOGIN = user.LoginName;
                log4.U_ID = user.Identity;                
                log4.IP = user.HostIP;
            }
            
            log4.LOG_DATE = DateTime.Now;            
            log4.LOGGER = loggingEvent.LoggerName;            
            log4.THREAD = loggingEvent.ThreadName;            
            log4.LOG4_LEVEL = loggingEvent.Level.Name;            
            log4.BROWSER = browserName + browserEdition;            
            log4.MACHINE_NAME = host;


            decipher.InsertModel(log4);

        }
    }
}
