using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using App.InfoGrid2.Bll;

namespace App.InfoGrid2.View.Biz.Core_Method
{
    public partial class Action : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpResult msg = null;

            string method = WebUtil.QueryTrim("method");

            try
            {
                msg = ProMethod(method);
            }
            catch(Exception ex)
            {
                msg = HttpResult.Error();

                log.Error(ex);
            }

            Response.Clear();
            Response.Write(msg);
            Response.End();
        }




        private HttpResult ProMethod(string method)
        {
            string code = WebUtil.QueryTrim("code");

            if (!ValidateUtil.SqlInput(method))
            {
                throw new Exception("参数异常.method=" + method);
            }

            if (!ValidateUtil.Regex(code, @"^\w{1,20}$"))
            {

                throw new Exception("参数异常.code=" + code);
            }


            HttpResult msg = null;


            switch (method)
            {
                case "get":
                    msg = ProExec(code);
                    break;
            }

            return msg;
        }
        

        private HttpResult ProExec(string code)
        {
            HttpResult msg = null;


            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_METHOD bm = decipher.SelectToOneModel<BIZ_METHOD>($"METHOD_CODE = '{code}'");

            if(bm == null)
            {
                throw new Exception("对应的函数编码不存在.code=" + code);
            }


            if(bm.EXEC_TYPE == "TSQL")
            {
                msg = ExecTSQL(bm); 
            }
            else
            {
                msg = ExecCode(bm);
            }

            return msg;
        }

        private HttpResult ExecTSQL(BIZ_METHOD bm)
        {
            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            string tSql = bm.TSQL.Replace("{{GetBizUserCode}}", userCode);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelReader reader = decipher.GetModelReader(tSql);

            string rType = bm.RETURN_TYPE;


            HttpResult msg;

            if (rType == "table")
            {
                SModelList table = ModelHelper.GetSModelList(reader);

                msg = HttpResult.Success(table);



                string tag = WebUtil.Query("tag");

                //特殊处理: 针对流程地址进行处理
                if(tag == "flow")
                {
                    foreach (var sm in table)
                    {
                        if (sm.HasField("EXTEND_DOC_URL") && sm.HasField("EXTEND_TABLE"))
                        {
                            string tabName = sm["EXTEND_TABLE"];
                            int menuId = sm["EXTEND_MENU_ID"];
                            string newUrl = FlowUrlMgr.Get("company",menuId, tabName, sm);

                            sm["EXTEND_DOC_URL"] = StringUtil.NoBlank( newUrl, sm["EXTEND_DOC_URL"]);
                        }
                    }
                }

            }
            else
            {
                object reValue = null;

                try
                {
                    if (reader.Read())
                    {
                        reValue = reader.GetObject(0);
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                
                msg = HttpResult.Success(reValue);
            }

            return msg;
        }

        private HttpResult ExecCode(BIZ_METHOD bm)
        {
            return null;
        }


    }
}