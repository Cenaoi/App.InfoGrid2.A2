using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.View;
using App.InfoGrid2.WF.Bll;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// ReportHandler 的摘要说明
    /// </summary>
    public class ReportHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpResult result = null;

            try
            {

                string action = WebUtil.FormTrimUpper("action");

                switch (action)
                {
                    case "GET_REPORT_ID":
                        result = GetReportID();
                        break;
                    case "SUBMIT_REPORT":
                        result = SubmitReport();
                        break;
                    case "NEXT_REPORT":
                        result = NextReport();
                        break;
                    default:
                        result = HttpResult.Error("写错了吧！");
                        break;

                }

            }
            catch (Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error("哦噢，出错了！");

            }


            context.Response.Write(result);
        }


        /// <summary>
        /// 获取最近一天未修改的报告ID
        /// </summary>
        /// <returns></returns>
        HttpResult GetReportID()
        {


            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_371");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.And("BIZ_CREATE_USER_CODE", user_code);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE  desc";
            lmFilter.Top = 1;

            lmFilter.Fields = new string[] { "ROW_IDENTITY_ID" };

            SModel sm371 = decipher.GetSModel(lmFilter);

            if(sm371 != null)
            {
                return HttpResult.Success(sm371);
            }

            LModel lm = new LModel("UT_371");

            //lm["COL_1"] = DateTime.Now;
            lm["ROW_SID"] = 0;
            lm["ROW_DATE_CREATE"] = lm["ROW_DATE_UPDATE"] = lm["BIZ_CREATE_DATE"] = lm["BIZ_UPDATE_DATE"] = DateTime.Now;
            lm["ROW_AUTHOR_USER_CODE"] = user_code;
            lm["BIZ_CREATE_USER_CODE"] = user_code;
            lm["BIZ_CREATE_USER_TEXT"] = user.LoginName;
            lm["ROW_USER_SEQ"] = 100000000m;
            lm["COL_44"] = "草稿";
            lm["COL_35"] = "周报告单";
            lm["COL_36"] = "501";
            lm["COL_38"] = "周报告";
            lm["COL_39"] = "102";
            lm["COL_17"] = "是";
            lm["COL_5"] = user.LoginName;
            lm["COL_46"] = "等待核对";
            lm["COL_20"] = "草稿";
            lm["COL_21"] = "101";
            lm["COL_41"] = "制单中";
            lm["COL_51"] = user_code;
            lm["COL_3"] = DateTime.Now.Year;
            lm["COL_4"] = DateTime.Now.Month;
            lm["COL_57"] = 1;

            LightModelFilter lmFilter116 = new LightModelFilter("UT_116");
            lmFilter116.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter116.And("COL_19", user_code);


            LModel lm116 = decipher.GetModel(lmFilter116);

            //不等于空的时候才处理
            if (lm116 != null)
            {

                lm["COL_27"] = lm116["COL_1"];

                lm["COL_8"] = lm116["COL_13"];
                lm["COL_9"] = lm116["COL_12"];

                lm["COL_10"] = lm116["COL_11"];
                lm["COL_11"] = lm116["COL_16"];
                lm["COL_7"] = lm116["COL_14"];

                lm["COL_6"] = lm116["COL_15"];

            }


            decipher.InsertModel(lm);


            return HttpResult.Success(new { ROW_IDENTITY_ID=lm["ROW_IDENTITY_ID"] });

            
        }

        /// <summary>
        /// 提交报销
        /// </summary>
        HttpResult SubmitReport()
        {

            int id = WebUtil.FormInt("id");


            //DbDecipher decipher = ModelAction.OpenDecipher();

            //LModel lm346 = decipher.GetModelByPk("UT_371", id);

            //lm346["BIZ_SID"] = 2;
            //lm346["ROW_DATE_UPDATE"] = DateTime.Now;

            //decipher.UpdateModelProps(lm346, "BIZ_SID", "ROW_DATE_UPDATE");

            try
            {
                FlowSubmit();

                return HttpResult.Success("提交成功");
            }
            catch (Exception ex)
            {
                log.Error("提交失败.", ex);

                return HttpResult.Error("提交失败");
            }


        }


        #region 流程操作


        string m_FlowCode = "FLOW-17021502";  //流程编码
        string m_FlowTableName = "UT_371";

        /// <summary>
        /// 获取当前流程节点的信息
        /// </summary>
        /// <returns></returns>
        private FlowNodeInfo GetNodeInfo(out List<FLOW_DEF_NODE_PARTY> partys)
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(m_FlowTableName);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", id);
            filter.Fields = new string[] { "BIZ_FLOW_SID", "BIZ_FLOW_INST_CODE", "BIZ_FLOW_STEP_CODE" ,
                "BIZ_FLOW_DEF_CODE","BIZ_FLOW_CUR_NODE_CODE","BIZ_FLOW_CUR_NODE_TEXT"};

            SModel sRow = decipher.GetSModel(filter);

           FlowNodeInfo flowNInfo = null;

            int flowSID = sRow.Get<int>("BIZ_FLOW_SID");

            //List<FLOW_DEF_NODE_PARTY> partys = null;

            partys = null;

            if (flowSID == 0)
            {
                flowNInfo = FlowMgr.GetStartNodeInfo(decipher,m_FlowCode);
            }
            else if (flowSID > 0)
            {
                string curNode = (string)sRow["BIZ_FLOW_CUR_NODE_CODE"];
                flowNInfo =FlowMgr.GetNodeInfo(m_FlowCode, curNode);

                partys = FlowMgr.GetPartys(decipher, m_FlowCode, curNode);
            }
            else
            {
                throw new Exception("傻了吧, 改错数据了吧.");
            }

            return flowNInfo;
        }

        /// <summary>
        /// 流程提交,或 审核通过
        /// </summary>
        private void FlowSubmit()
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<FLOW_DEF_NODE_PARTY> partys = null;    //下一个接受人信息

            FlowNodeInfo flowNInfo = GetNodeInfo(out partys);   //获取当前节点信息


            FlowOperateMgr flowMgr = new FlowOperateMgr();

            flowMgr.CurFlowCode = m_FlowCode;
            flowMgr.CurNodeCode = flowNInfo.Node.NODE_CODE;
            flowMgr.LineCode = flowNInfo.Lines[0].LINE_CODE;

            flowMgr.ActionType = "submit";
            flowMgr.Comment = ".提交";
            flowMgr.DocUrl = $"/WF/View/Report/ReportFlow.aspx?report_id={id}";

            flowMgr.PageId = GlobelParam.GetValue("周报告_PAGE_ID", 1894, "流程名称【周报告】");
            flowMgr.MenuId = GlobelParam.GetValue("周报告_MENU_ID", 755, "流程名称【周报告】");

            flowMgr.DocText = "周报告";

            flowMgr.BillType = "周报告";
            flowMgr.BillCodeField = "COL_37";   //业务单号

            flowMgr.TableName = m_FlowTableName;
            flowMgr.RowId = id;

            if (partys != null)
            {
                List<string> pCodes = new List<string>();

                foreach (var item in partys)
                {
                    pCodes.Add(item.P_USER_CODE);
                }

                flowMgr.SubmitPartys = pCodes;

            }


            flowMgr.User = EcContext.Current.User;

            flowMgr.GoEnter();

            if (flowMgr.Error != null)
            {
                throw flowMgr.Error;
            }


            try
            {

                BusHelper.CreateFlowTemp(m_FlowTableName, id, flowMgr, "report");

            }
            catch (Exception ex)
            {
                log.Error("创建流程模板消息出错了！-------------------------------------------------", ex);
            }


        }


        #endregion


        /// <summary>
        /// 下一个周报告明细
        /// </summary>
        /// <returns></returns>
        HttpResult NextReport()
        {

            int flow_id = WebUtil.FormInt("flow_id");

            int biz_sid = WebUtil.FormInt("biz_sid");

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserDocs(biz_sid, userCode, $"FLOW_INST_PARTY.FLOW_INST_PARTY_ID < {flow_id}");

            if(docs.Count < 1)
            {
                return HttpResult.Error("没有多余的数据了");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModel sm = docs[0];

            SModel new_sm = new SModel();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("INST_CODE", sm["INST_CODE"]);

            FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);


            new_sm["create_date"] = sm["ROW_DATE_CREATE"];
            new_sm["title"] = sm["EXTEND_DOC_TEXT"] + "(" + f_inst.START_USER_TEXT + ")";
            new_sm["text"] = "未审核";

            StringBuilder sb = new StringBuilder();

            for (int i = 1; i < 5; i++)
            {
                if (!StringUtil.IsBlank(f_inst["EXT_COL_VALUE_" + i]))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("；");
                    }

                    sb.Append($"{f_inst["EXT_COL_TEXT_" + i]}:{f_inst["EXT_COL_VALUE_" + i]}");
                }

            }

            new_sm["sub_text"] = $"单号：{f_inst.EXTEND_BILL_CODE}," + sb.ToString();

            //new_sm["url"] = sm["EXTEND_DOC_URL"];

            string url = FlowUrlMgr.Get("mobile", 0, sm["EXTEND_TABLE"], sm);
            new_sm["url"] = StringUtil.NoBlank(url, sm["EXTEND_DOC_URL"]);
            new_sm["flow_id"] = sm["FLOW_INST_PARTY_ID"];
            new_sm["biz_sid"] = biz_sid;

            return HttpResult.Success(new_sm);


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