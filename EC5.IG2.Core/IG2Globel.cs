using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EC5.BizLogger;
using EC5.IG2.BizBase;
using EC5.IG2.Core.DymCSharpFile;
using EC5.IG2.Core.LcValue;
using EC5.IG2.Core.TempDataManager;
using EC5.IG2.Plugin;
using EC5.IG2.Plugin.Custom;
using EC5.LCodeEngine;
using EC5.LcValueEngine;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using EC5.Action3;

namespace EC5.IG2.Core
{



    public class IG2Globel : IBllAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void Init()
        {

            IG2TempMgr.ClearTemp();  //清理临时数据


            LogStepMgr.Clear(DateTime.Today.AddDays(-2));

            LcValueManager.LcValueChanging += LcValueManager_LcValueChanging;


            InitBizCode();

            InitStoreParam();   //初始化数据仓库的参数

            InitDymModel(); //初始化动态代码

            InitLCodeValue();//简单业务

            InitDbcc();     //初始化事务

            InitPlugin();   //初始化插件

            InitJTemplateFunc();    //初始化模板函数


            MapMgr.ClearBufferAll();    //清理所有映射缓冲

            Init_Action3(); //初始化-联动3


            try
            {
                InitBizData();


                CSharpFileLoader csLoader = new CSharpFileLoader();
                csLoader.Load(HttpContext.Current.Server.MapPath(@"~\View\CustomPage\V3"));
            }
            catch (Exception ex)
            {
                log.Error("动态编译失败。", ex);
            }
        }

        /// <summary>
        /// 初始化 Biz 业务数据
        /// </summary>
        private void InitBizData()
        {
            DbDecipher decipher;

            decipher = DbDecipherManager.GetDecipherOpen();

            try
            {
                if (!decipher.ExistsModelByPk<IG2_CATALOG>(106))
                {
                    IG2_CATALOG cata = new IG2_CATALOG();
                    cata.IG2_CATALOG_ID = 106;
                    cata.TEXT = "交叉表";
                    cata.DEFAULT_TABLE_TYPE = "CROSS_TABLE";

                    decipher.IdentityStop();
                    decipher.InsertModel(cata);
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


        /// <summary>
        /// 初始化模板的函数
        /// </summary>
        private void InitJTemplateFunc()
        {
            Type tihsT = this.GetType();

            JTemplateFunc func;

            JTemplateFuncManager.Commons.Clear();

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("NewCode"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("GetBizUserCode"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("GetBizOrgCode"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("GetLoginName"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("GetLoginID"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("GetUserIdentity"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("GetDate"));

            JTemplateFuncManager.Commons.Add(this, tihsT.GetMethod("NewId"));


        }

        /// <summary>
        /// 初始化'联动3'
        /// </summary>
        private void Init_Action3()
        {
            
            Ac3DataMgr mgr = new Ac3DataMgr();

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                Action3.Xml.XDocument xdoc = mgr.ToDocXml(decipher);


                DrawingLibrary lib = ActConvert.LibFrom(xdoc.Library);
                lib.Code = "default";

                LibraryManager.TryAdd(lib);
            }

        }

        #region 自定义函数

        public DateTime GetDate(params object[] dymPs)
        {
            return DateTime.Now;
        }

        public Guid NewId(params object[] dymPs)
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 获取当前正在操作的公司代码
        /// </summary>
        /// <param name="dymPs"></param>
        /// <returns></returns>
        public string GetOpCompCode(params object[] dymPs)
        {
            return BizServer.OpCompCode;
        }

        /// <summary>
        /// 创建单据编码
        /// </summary>
        /// <param name="tCode">编码代码</param>
        /// <returns></returns>
        public string NewCode(params object[] dymPs)
        {
            string tCode = (string)dymPs[0];
            return EC5.BizCoder.BizCodeMgr.NewCode(tCode); 
        }
        
        /// <summary>
        /// 用户代码
        /// </summary>
        /// <returns></returns>
        public string GetBizUserCode(params object[] dymPs)
        {
            return BizServer.UserCode ?? string.Empty;
        }

        /// <summary>
        /// 组织结构代码
        /// </summary>
        /// <returns></returns>
        public string GetBizOrgCode(params object[] dymPs)
        {
            return BizServer.OrgCode ?? string.Empty;
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        /// <returns></returns>
        public string GetLoginName(params object[] dymPs)
        {
            return BizServer.LoginName ?? string.Empty;
        }

        /// <summary>
        /// 登录账号
        /// </summary>
        /// <returns></returns>
        public string GetLoginID(params object[] dymPs)
        {
            return BizServer.LoginID ?? string.Empty;
        }

        /// <summary>
        /// 数据库主键id
        /// </summary>
        /// <returns></returns>
        public string GetUserIdentity(params object[] dymPs)
        {
            return BizServer.UserIdentity.ToString();
        }


        #endregion


        /// <summary>
        /// 初始化编码格式
        /// </summary>
        private void InitBizCode()
        {
            try
            {
                BizCodeLoader loader = new BizCodeLoader();
                loader.Load();
            }
            catch (Exception ex)
            {
                log.Error("加载编码定义失败", ex);
            }
        }

        private void LcValueManager_LcValueChanging(object sender, LcValueEventArgs e)
        {
            object result = LcModelManager.Exec(e.Code, e.Model);

            e.Model[e.TargetField] = result;

        }

        bool m_InitModel = false;




        /// <summary>
        /// 初始化动态实体
        /// </summary>
        private void InitDymModel()
        {

            if (m_InitModel)
            {
                return;
            }



            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "IG2_TABLE_ID" };

            log.Info("准备加载系统业务规则...");

            try
            {
                LcModelManager.Models.Clear();

                LModelReader reader = decipher.GetModelReader(filter);

                int[] ids = ModelHelper.GetColumnData<int>(reader);

                foreach (int id in ids)
                {
                    try
                    {
                        TableSet ts = TableSet.Select(decipher, id);

                        TableMgr.GetModelElem(ts);

                        InitLCode(ts);
                    }
                    catch (Exception ex2)
                    {
                        log.Error("系统启动，加载实体元素错误.", ex2);
                    }
                }


                m_InitModel = true;
            }
            catch (Exception ex)
            {
                log.Error("初始化动态实体错误", ex);
            }
            finally
            {
                decipher.Dispose();
            }


            try
            {
                foreach (LcModel cModel in LcModelManager.Models.Values)
                {
                    cModel.Reset();
                }
            }
            catch (Exception ex)
            {
                log.Error("重置规则管理器错误。", ex);
            }


            log.Info("完成加载系统业务规则.");
        }

        private void InitLCodeValue()
        {
            LCodeValueLoader loader = new LCodeValueLoader();

            try
            {
                loader.Init();
            }
            catch (Exception ex)
            {
                log.Error("加载简单业务错误", ex);
            }
        }

        /// <summary>
        /// 初始化 LightCode 简单表达式
        /// </summary>
        private void InitLCode(TableSet tSet)
        {

            LcModel cModel = null;

            foreach (IG2_TABLE_COL col in tSet.Cols)
            {
                if (StringUtil.IsBlank(col.L_CODE))
                {
                    continue;
                }

                if (cModel == null)
                {
                    cModel = new LcModel();
                    cModel.TableName = tSet.Table.TABLE_NAME;
                }

                cModel.Add(col.DB_FIELD, col.L_CODE);

            }

            if (cModel != null)
            {
                LcModelManager.Models.Add(tSet.Table.TABLE_NAME, cModel);
            }
        }



        /// <summary>
        /// 初始化数据仓库的参数
        /// </summary>
        private void InitStoreParam()
        {
            EasyClick.Web.Mini2.StoreParamValueManager.Clear();



            EasyClick.Web.Mini2.StoreParamValueManager.Add("NewCode", this.NewCode);

            //获取用户编码
            EasyClick.Web.Mini2.StoreParamValueManager.Add("GetBizUserCode", this.GetBizUserCode);

            //获取用户组织编码
            EasyClick.Web.Mini2.StoreParamValueManager.Add("GetBizOrgCode", this.GetBizOrgCode);
            
            EasyClick.Web.Mini2.StoreParamValueManager.Add("GetLoginName", this.GetLoginName);
            EasyClick.Web.Mini2.StoreParamValueManager.Add("GetLoginID", this.GetLoginID);
            EasyClick.Web.Mini2.StoreParamValueManager.Add("GetUserIdentity", this.GetUserIdentity);
        }



        /// <summary>
        /// 初始化插件
        /// </summary>
        private void InitPlugin()
        {
            PluginManager.Clear();

            PluginManager.Add(typeof(BillPlugin).FullName, typeof(BillPlugin));
            PluginManager.Add(typeof(ImportPlug).FullName, typeof(ImportPlug));

            PluginManager.Add(typeof(InputOutExcelPlugin).FullName, typeof(InputOutExcelPlugin));
            PluginManager.Add(typeof(PrintPlugin).FullName, typeof(PrintPlugin));
            PluginManager.Add(typeof(SyncGbDataPlugin).FullName, typeof(SyncGbDataPlugin));
            PluginManager.Add(typeof(ImportGbDataPlugin).FullName, typeof(ImportGbDataPlugin));
        }


        /// <summary>
        /// 初始化联动事务
        /// </summary>
        private void InitDbcc()
        {

            log.Info("准备初始化，数据表联动业务...");

            try
            {
                DbCascadeLoader dbCCLoader = new DbCascadeLoader();
                dbCCLoader.InitDbcc();
            }
            catch (Exception ex)
            {
                log.Error("初始化业务数据库事务失败.", ex);
            }

            log.Info("完成初始化，数据表联动业务.");
        }



    }
}
