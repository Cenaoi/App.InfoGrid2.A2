using System;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using EC5.SystemBoard;
using EC5.Utility;

namespace App.InfoGrid2.View
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PrintServer : System.Web.UI.Page,IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 下次清理时间
        /// </summary>
        static DateTime m_NextTimeClear = DateTime.MinValue;

        private void ClearHeartbeat()
        {

            #region 半小时清理一次

            if (DateTime.Now > m_NextTimeClear)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                m_NextTimeClear = DateTime.Now.AddHours(1);

                DateTime t1 = DateTime.Now.AddDays(-1);

                int deleteCount = decipher.DeleteModels<BIZ_PRINT_HEARTBEAT>($"ROW_DATE_CREATE < '{DateUtil.ToDateTimeString(t1)}'");

                if (deleteCount > 0)
                {
                    log.Info($"清理打印心跳日志 {deleteCount} 条记录.");
                }
            }

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            
            string opt = WebUtil.Query("opt").ToUpper();

            try
            {
                ClearHeartbeat();
            }
            catch(Exception ex)
            {
                log.Error("清理失败", ex);
            }

            try
            {
                Exec_Action(opt);
            }
            catch (Exception ex)
            {
                log.Error("打印模块错误", ex);

                Response.Clear();
                Response.Write("NO|未知错误->" + ex.Message);
            }
        }

        private void Exec_Action(string opt)
        {

            switch (opt)
            {
                case "GET": Action_Get(); break;
                case "LINE": Action_Line(); break;
                case "CLEAR_LINE": Action_ClearLine(); break;
                case "PRINT_END": Action_PrintEnd(); break;
                case "LOG": Action_Log(); break;
            }
        }

        /// <summary>
        /// 设置打印状态
        /// </summary>
        private void Action_PrintEnd()
        {
            Guid fileGuid = WebUtil.QueryGuid("file_guid");

            if (fileGuid == Guid.Empty)
            {
                Response.Write("NO|文件状态更改错误.501");
                return;
            }

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_PRINT_FILE));
            filter.And("FILE_GUID", fileGuid);
            filter.And("ROW_SID", 2);

            
            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_PRINT_FILE model = decipher.SelectToOneModel<BIZ_PRINT_FILE>(filter);

            if (model == null)
            {
                Response.Write("NO|文件状态更改错误.");
                return;
            }

            model.ROW_SID = 4;
            model.TIME_PRINT_END = DateTime.Now;

            decipher.UpdateModelProps(model, "ROW_SID", "TIME_PRINT_END");

        }


        /// <summary>
        /// 清理断开连接的打印机
        /// </summary>
        private void Action_ClearLine()
        {
            int overtime = GlobelParam.GetValue<int>("PRINT_LINE_TIME", 20, "打印机离线超时时间(秒)");

            DateTime time = DateTime.Now.AddSeconds(-overtime);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_PRINT_CLIENT));
            filter.And("IS_LINE", true);
            filter.And("LAST_LINE_TIME", time, Logic.LessThan);


            LightModelFilter printFilter= new LightModelFilter(typeof(BIZ_PRINT));
            printFilter.And("IS_LINE", true);
            printFilter.And("LAST_LINE_TIME", time, Logic.LessThan);

            decipher.UpdateProps(filter, new object[] { "IS_LINE", false });
            decipher.UpdateProps(printFilter, new object[] { "IS_LINE", false });
        }




        private void LogPrintHear(DbDecipher decipher, Guid guid, EcUserState user, string driveName, string remark)
        {
            bool logPrintHear = GlobelParam.GetValue<bool>(decipher, "LOG_PRINT_HEARTBEAT", false, "登录打印机心跳日志");

            if (logPrintHear)
            {
                BIZ_PRINT_HEARTBEAT hrartbeat = new BIZ_PRINT_HEARTBEAT()
                {
                    PCLIENT_GUID = guid,
                    C_IP = user.HostIP,
                    DRIVER_NAME = driveName,
                    REMARK = remark

                };

                decipher.InsertModel(hrartbeat);

                #region 顺便删除掉历史

                {

                    DateTime dateM2 = DateTime.Now.AddMinutes(-1);

                    LightModelFilter delFilter = new LightModelFilter(typeof(BIZ_PRINT_HEARTBEAT));
                    delFilter.And("ROW_DATE_CREATE", dateM2, Logic.LessThanOrEqual);
                    delFilter.And("PCLIENT_GUID", guid);

                    try
                    {
                        decipher.DeleteModels(delFilter);
                    }
                    catch(Exception ex)
                    {
                        log.Error("删除打印日志数据异常", ex);
                    }
                }

                #endregion
            }
        }

        private void Action_Line()
        {
            string code = WebUtil.Form("PRINT_CODE");
            Guid guid = WebUtil.FormGuid("PCLIENT_GUID");
            string name = WebUtil.Form("PRINT_NAME");
            string driveName = WebUtil.Form("DRIVE_NAME");
            string remark = WebUtil.Form("REMARK");
            EcUserState user = EcContext.Current.User;

            //NO|错误信息
            //YES|编码

            if (guid == Guid.Empty)
            {
                Response.Write("NO|Guid没有传过来");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LogPrintHear(decipher, guid, user, driveName, remark);

            try
            {
                BIZ_PRINT_CLIENT client = decipher.SelectToOneModel<BIZ_PRINT_CLIENT>("PCLIENT_GUID='{0}'", guid);




                if (client == null)
                {
                    client = InsertData(guid, remark);
                }

                if (client.BIZ_SID == 0)
                {
                    Response.Write("NO|客户端未激活");
                    return;
                }




                client.IS_LINE = true;
                client.LAST_LINE_TIME = DateTime.Now;
                decipher.UpdateModelProps(client, "IS_LINE","LAST_LINE_TIME");

                LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_PRINT_NAME));
                lmFilter.And("PCLIENT_GUID", guid);
                lmFilter.And("PRINT_NAME", name);
                lmFilter.And("PRINT_DRIVE_NAME", driveName);

                BIZ_PRINT_NAME info = decipher.SelectToOneModel<BIZ_PRINT_NAME>(lmFilter);

                if (info == null)
                {
                    info = InsertData(guid,name, driveName);
                }



                if (string.IsNullOrEmpty(info.PRINT_CODE))
                {
                    Response.Write("NO|未分配编码，请稍等！");
                    return;
                }


                BIZ_PRINT print = decipher.SelectToOneModel<BIZ_PRINT>("PRINT_CODE='{0}'", info.PRINT_CODE);

                if (print == null)
                {
                    Response.Write(string.Format( "NO|打印机“{0}”未注册",info.PRINT_CODE));
                    return;
                }

                if (info.BIZ_SID == 0)
                {
                    Response.Write("NO|打印机未激活");
                    return;
                }


                print.LAST_LINE_TIME = DateTime.Now;
                print.IS_LINE = true;
                decipher.UpdateModelProps(print, "IS_LINE", "LAST_LINE_TIME");


                string msg = "YES|" + info.PRINT_CODE;
                Response.Write(msg);

            }
            catch (Exception ex)
            {
                log.Error("插入数据输错了！", ex);
                Response.Write("NO|服务器插入数据出错!");
            }
        }

        /// <summary>
        /// 是否存在客户端
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private bool AllowClient(Guid guid)
        {
            if (Guid.Empty == guid)
            {
                return false;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_PRINT_CLIENT));
            filter.And("PCLIENT_GUID", guid);
            filter.And("BIZ_SID", 2, Logic.GreaterThanOrEqual);

            bool exist = decipher.ExistsModels(filter);

            return exist;
        }


        private void Action_Get()
        {
            Guid guid = WebUtil.QueryGuid("PCLIENT_GUID", Guid.Empty);

            string code = WebUtil.Query("PRINT_CODE");

            string[] codeList = StringUtil.Split(code, "||");

            if (guid == Guid.Empty || codeList.Length == 0)
            {
                Response.Write("NO|禁止访问.");
                return;
            }



            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                bool isAllow = AllowClient(guid);

                if (!isAllow)
                {
                    Response.Write("NO|禁止访问.");
                    return;
                }


                #region 查找需要打印的文件

                DateTime lastPrint = DateTime.Now.AddHours(-2);

                LightModelFilter filter = new LightModelFilter(typeof(BIZ_PRINT_FILE));
                filter.And("PRINT_CODE", codeList, Logic.In);
                filter.And("ROW_SID", 0);
                filter.And("ROW_DATE_CREATE", lastPrint, Logic.GreaterThanOrEqual);
                filter.TSqlOrderBy = "ROW_DATE_CREATE ASC";


                //YSE|打印机名|下载地址
                BIZ_PRINT_FILE data = decipher.SelectToOneModel<BIZ_PRINT_FILE>(filter);

                if (data == null)
                {
                    Response.Write("NO|没有需要打印的文件.");
                    return;
                }

                #endregion

                #region 查找客户端对应的打印机

                LightModelFilter printFilter = new LightModelFilter(typeof(BIZ_PRINT_NAME));
                printFilter.And("PCLIENT_GUID", guid);
                printFilter.And("PRINT_CODE", data.PRINT_CODE);
                printFilter.And("IS_LINE", true);
                printFilter.And("BIZ_SID", 2, Logic.GreaterThanOrEqual);

                BIZ_PRINT_NAME clientPrint = decipher.SelectToOneModel<BIZ_PRINT_NAME>(printFilter);

                if (clientPrint == null)
                {
                    Response.Write("NO|没有对应的打印机.");
                    return;
                }

                #endregion



                string cmd = string.Format("YES|{0}|{1}|{2}", clientPrint.PRINT_NAME, data.FILE_URL,data.FILE_GUID);

                Response.Write(cmd);

                data.ROW_SID = 2;

                decipher.UpdateModelProps(data, "ROW_SID","FILE_GUID");

            }
            catch (Exception ex)
            {
                log.Error("这是打印出错了！", ex);

                Response.Clear();
                Response.Write("NO|打印出错了");
            }

        }


        private BIZ_PRINT_CLIENT InsertData(Guid guid, string remark)
        {
            int bizSID = GlobelParam.GetValue<int>("PRITN_HELPER_BIZ_SID", 2, "打印机助手默认注册状态.0-未激活，2-激活");

            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_PRINT_CLIENT client = new BIZ_PRINT_CLIENT()
            {
                IS_LINE = true,
                PCLIENT_GUID = guid,
                REMARK = remark,
                LAST_LINE_TIME = DateTime.Now,
                BIZ_SID = bizSID
            };

            decipher.InsertModel(client);

            return client;
        }

        private BIZ_PRINT_NAME InsertData(Guid guid, string name, string driveName)
        {
            int bizSID = GlobelParam.GetValue<int>("PRITN_BIZ_SID", 2, "打印机默认注册状态.0-未激活，2-激活");

            DbDecipher decipher = ModelAction.OpenDecipher();
            BIZ_PRINT_NAME info = new BIZ_PRINT_NAME()
            {
                PCLIENT_GUID = guid,
                IS_LINE = true,
                PRINT_NAME = name,
                PRINT_DRIVE_NAME = driveName,
                
                BIZ_SID = bizSID
            };
            decipher.InsertModel(info);

            return info;

        }


        /// <summary>
        /// 保存客户端日志
        /// </summary>
        void Action_Log() 
        {

            //客户端名称
            string name = WebUtil.Form("Name");
            //打印信息
            string message = WebUtil.Form("DataMessage");

            string logType = WebUtil.Form("Type");


            LOG_PRINT_CLIENT lpc = new LOG_PRINT_CLIENT()
            {
                CLIENT_NAME = name,
                LOG_MESSAGE = message,
                LOG_TYPE = logType
            };

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.InsertModel(lpc);


            
                  
        }



    }
}