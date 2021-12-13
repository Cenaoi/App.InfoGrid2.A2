using System;
using System.IO;
using System.Text;
using System.Web;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini;
using EC5.SystemBoard.IO;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Yahoo.Yui.Compressor;
using System.Timers;
using Sysboard.Web.Tasks;
using EC5.IG2.Plugin;
using EC5.IG2.Plugin.Custom;
using EC5.IG2.Core;
using App.BizCommon;

namespace App.InfoGrid2
{
    public class Global : EC5HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        public Global()
            : base("InfoGrid2")
        {

        }


        Timer m_Timer;

        /// <summary>
        /// 启动自动更新
        /// </summary>
        private void StartTask()
        {
            WebTaskManager.Clear();



            //Mall.Task.SyncTask st = new Mall.Task.SyncTask();

            //WebTaskManager.Add(st);

            string proj_tag;

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                proj_tag = GlobelParam.GetValue(decipher, "PROJ_TAG", "MALL", "项目标记");
            }

            if ("MALL" == proj_tag)
            {
                //Mall.Task.SyncSpec st = new Mall.Task.SyncSpec();

                //WebTaskManager.Add(st);

                //Mall.Task.SyncUT083 sy_ut083 = new Mall.Task.SyncUT083();

                //WebTaskManager.Add(sy_ut083);

                //Mall.Task.SalesTimer salertimer = new Mall.Task.SalesTimer();

                //WebTaskManager.Add(salertimer);
            }


            if("WF" == proj_tag)
            {

                //WF.Task.SendWxTempMsg swtm = new WF.Task.SendWxTempMsg();

                //WebTaskManager.Add(swtm);
            }

            //View.DistrTask distrTask = new View.DistrTask();


            //WebTaskManager.Add(distrTask);

            if ("GBZZZD" == proj_tag)
            {
                GBZZZD.Task.SyncOrderHelper.SDbConn = DbDecipherManager.GetConnectionString("GUBO_2021");

                GBZZZD.Task.SyncOrderHelper.TDbConn = DbDecipherManager.GetConnectionString("ERP_YD_2021");

                GBZZZD.Task.ImportOrderDataTask importOrderDataTask = new GBZZZD.Task.ImportOrderDataTask();

                WebTaskManager.Add(importOrderDataTask);

                GBZZZD.Task.SyncSaleOrderTask syncSaleOrderTask = new GBZZZD.Task.SyncSaleOrderTask();

                WebTaskManager.Add(syncSaleOrderTask);

                //GBZZZD.Task.SyncSaleOrderItemsTask syncSaleOrderItemTask = new GBZZZD.Task.SyncSaleOrderItemsTask();

                //WebTaskManager.Add(syncSaleOrderItemTask);

                GBZZZD.Task.SyncOrderTask syncOrderTask = new GBZZZD.Task.SyncOrderTask();

                WebTaskManager.Add(syncOrderTask);

                GBZZZD.Task.SyncOrderItemsTask syncOrderTaskItem = new GBZZZD.Task.SyncOrderItemsTask();

                WebTaskManager.Add(syncOrderTaskItem);
            }

            if (m_Timer == null)
            {
                m_Timer = new Timer();
                m_Timer.Elapsed += delegate
                {
                    WebTaskManager.Run();
                };
            }
            
           // m_Timer.Interval = 1000 * 20;   //10秒

            m_Timer.Interval = 500; 

            if (!m_Timer.Enabled)
            {
                m_Timer.Start();
            }
        }


        private void XXXXX()
        {

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            LModelList<IG2_TABLE> tables = decipher.SelectModels<IG2_TABLE>("SEC_LEVEL = 99");

            try
            {
                foreach (IG2_TABLE tab in tables)
                {

                    LModelList<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID = {0} ", tab.IG2_TABLE_ID);


                    decipher.BeginTransaction();

                    try
                    {
                        foreach (IG2_TABLE_COL col in cols)
                        {
                            if (col.TABLE_UID != tab.TABLE_UID)
                            {
                                col.TABLE_UID = tab.TABLE_UID;
                                col.TABLE_NAME = tab.TABLE_NAME;

                                decipher.UpdateModelProps(col, "TABLE_UID", "TABLE_NAME");
                            }

                        }

                        tab.SEC_LEVEL = 2;

                        decipher.UpdateModelProps(tab, "SEC_LEVEL");

                        decipher.TransactionCommit();
                    }
                    catch (Exception ex2)
                    {
                        decipher.TransactionRollback();
                        log.Error(ex2);
                    }
                }


            }
            catch (Exception ex)
            {

                log.Error(ex);
            }
            finally
            {
                decipher.Dispose();
            }

        }

        IG2Globel m_IG2Globel;

        public override void EC5_Init()
        {
            StartTask();

            MiniHelper.UniteMiniSystemJS("~/Core/Scripts/Mini/1.1/MiniHtml.min.js");

            UniteMiniSystemJS();

            Mini2JScriptHelper.UniteMiniSystemJS();

            MiniConfiguration.IconResConfig.Clear();
            MiniConfiguration.IconResConfig.Load(this.Server.MapPath("~/res/icon"), "/res/icon");

            EasyClick.Web.Mini2.FormLayoutHelper.Add("ENTITY", typeof(EasyClick.Web.Mini2.Data.EntityFormEngine));


            MiniConfiguration.JsonFactory = new EasyClick.BizWeb2.ModelJsonFactory();

            DbDecipherManager.TraceOperate = new DbTrachOperate();

            m_IG2Globel = new IG2Globel();
            m_IG2Globel.Init();


            
            ReadFlowConfig();   //读取流程配置

        }


        /// <summary>
        /// 合并 Mini.js
        /// </summary>
        public static void UniteMiniSystemJS()
        {
            bool isMinFile = true;

            log.InfoFormat("{0}() Begin: 合并 Mini.js", System.Reflection.MethodBase.GetCurrentMethod().Name);

            HttpContext con = HttpContext.Current;

            if (con == null || con.Server == null) { return; }

            string uniteConfigFile = con.Server.MapPath("~/Core/Scripts/m5/M5.join.ini");
            string targetFile;

            if (isMinFile)
            {
                targetFile = con.Server.MapPath("~/Core/Scripts/M5/M5.min.js");
            }
            else
            {
                targetFile = con.Server.MapPath("~/Core/Scripts/M5/M5.js");
            }


            StringBuilder jsScript = new StringBuilder();

            StringBuilder fs = new StringBuilder();

            fs.AppendFormat("/// Mini2.js 创建日期:{0}", DateTime.Now).AppendLine();


            string[] jsFiles = File.ReadAllLines(uniteConfigFile);


            foreach (string jsFile in jsFiles)
            {
                if (jsFile.Trim().Length == 0)
                {
                    jsScript.AppendLine();
                    continue;
                }

                if (jsFile.StartsWith("--") || jsFile.StartsWith("//"))
                {
                    continue;
                }

                string file = con.Server.MapPath("~/" + jsFile);

                if (!File.Exists(file))
                {
                    log.ErrorFormat("文件不存在:\"{0}\"", jsFile);
                    continue;
                }

                jsScript.Append("<script src=\"");
                jsScript.Append( jsFile);
                jsScript.AppendLine("\" type=\"text/javascript\" ></script>");



                string lines = File.ReadAllText(file);


                try
                {
                    //初始化JS压缩类
                    var js = new JavaScriptCompressor();
                    js.CompressionType = CompressionType.Standard;//压缩类型
                    js.Encoding = Encoding.UTF8;//编码
                    js.IgnoreEval = false;//大小写转换

                    js.ObfuscateJavascript = true;


                    js.ThreadCulture = System.Globalization.CultureInfo.CurrentCulture;


                    //压缩该js
                    string strContent = js.Compress(lines);

                    //string targetMiniFile = con.Server.MapPath("~/Core/Scripts/Mini2/mini/" + jsFile);

                    //if (isMinFile)
                    //{
                    fs.AppendLine(strContent);
                    //}
                    //else
                    //{
                    //    fs.Append(lines);
                    //}

                    //EcFile.WriteAllText(targetMiniFile, strContent, true);
                }
                catch (Exception ex)
                {
                    log.Error("压缩 " + jsFile + " 错误", ex);


                }
            }

            string targetFile2 = con.Server.MapPath("~/Core/Scripts/M5/M5.script.txt");

            string jsContent = fs.ToString();

            File.WriteAllText(targetFile2, jsScript.ToString(), Encoding.UTF8);

            File.WriteAllText(targetFile, fs.ToString(), Encoding.UTF8);


            log.InfoFormat("{0}() End", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
            
            EasyClick.Web.Mini2.StoreEngineManager.AddStoreT("Default", typeof(EasyClick.Web.Mini2.Data.EntityStoreEngine));
            EasyClick.Web.Mini2.StoreEngineManager.AddStoreT("tree-default", typeof(EasyClick.Web.Mini2.Data.EntityTreeStoreEngine));
            EasyClick.Web.Mini2.StoreEngineManager.DefaultStoreName = "Default";
        }


        


        public override void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpContext context = HttpContext.Current;

            //if (context != null)
            //{
            //    log.Debug($"访问 ({context.Request.RawUrl})");
            //}


            base.Application_BeginRequest(sender, e);
            




            //DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            //decipher.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

            //try
            //{
            //    BIZ_FILTER model = decipher.SelectModelByPk<BIZ_FILTER>(1);

            //    model.FILTER_TEXT = "呵呵 - 1";

            //    decipher.UpdateModelProps(model, "FILTER_TEXT");



            //    BIZ_FILTER model2 = decipher.SelectModelByPk<BIZ_FILTER>(1);

            //    model2.FILTER_TEXT += ",呵呵-2";

            //    decipher.UpdateModelProps(model2, "FILTER_TEXT");



            //    decipher.TransactionCommit();
            //}
            //catch (Exception ex)
            //{
            //    decipher.TransactionRollback();

            //    log.Error(ex);
            //}
            //finally
            //{
            //    decipher.Close();
            //    decipher.Dispose();
            //}
            

        }

        /// <summary>
        /// 读取流程配置
        /// </summary>
        private void ReadFlowConfig()
        {
            string url = Server.MapPath("/App_Biz/App_Data/WF/flow-preview.json");

            if (!File.Exists(url))
            {
                return;
            }

            FlowUrlMgr.ReadConfig(url);
        }

        public override void Application_Error(object sender, System.EventArgs e)
        {
            HttpContext content = HttpContext.Current;

            Exception[] exp = content.AllErrors;

            foreach (var item in exp)
            {
                log.Error(item);
            }
        }

    }
}