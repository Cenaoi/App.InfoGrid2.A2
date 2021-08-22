using EC5.IO;
using EC5.Utility.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.View;
using App.InfoGrid2.Bll;
using EC5.SystemBoard;
using App.BizCommon;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using EC5.Utility;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.WF.Bll;
using EC5.Entity.Expanding.ExpandV1;
using System.Text;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// FlowHandler 的摘要说明
    /// </summary>
    public class FlowHandler : IHttpHandler,IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string aciton = WebUtil.FormTrimUpper("action");



            HttpResult result = null;

            try
            {

                switch (aciton)
                {

                    case "SETP_AGREE":
                        result = SetpAgree();
                        break;
                    case "SETP_BACK":
                        result = SetpBack();
                        break;
                    case "SETP_BACK_STRAT":
                        result = SetpBackFirst();
                        break;
                    case "SETP_AGREE_BY_REPORT":
                        result = SetpAgreeByReport();
                        break;
                    case "SETP_BACK_BY_REPORT":
                        result = SetpBackByReport();
                        break;
                    case "SETP_BACK_STRAT_BY_REPORT":
                        result = SetpBackStratByReport();
                        break;
                    default:
                        result = HttpResult.Error("哦噢，写错了！");
                        break;
                }

            }catch(Exception ex)
            {

                log.Error(ex);
                result = HttpResult.Error(ex.Message);

            }

            context.Response.Write(result);


        }


        /// <summary>
        /// 步骤确定按钮点击事件
        /// </summary>
        HttpResult SetpAgree()
        {
            HttpResult result = null;

            int id = WebUtil.FormInt("id");


            string comment = WebUtil.FormTrim("comment");


            string input_text = WebUtil.FormTrim("input_text");

            try
            {
                FlowSubmit(input_text, comment);

                result = HttpResult.SuccessMsg("提交成功");
            }
            catch(Exception ex)
            {
                log.Error("流程提交失败", ex);

                result = HttpResult.Error(ex.Message);
            }

            return result ;

        }

        /// <summary>
        /// 退回到上一节点
        /// </summary>
        /// <returns></returns>
        HttpResult SetpBack()
        {
            
            HttpResult result = null;

            int id = WebUtil.FormInt("id");

            string comment = WebUtil.FormTrim("comment");


            string input_text = WebUtil.FormTrim("input_text");

            try
            {
                FlowBlack(input_text, "back", comment);

                result = HttpResult.SuccessMsg("提交成功");
            }
            catch (Exception ex)
            {
                log.Error("流程提交失败", ex);

                result = HttpResult.Error(ex.Message);
            }

            return result;

        }


        /// <summary>
        /// 退到首环节
        /// </summary>
        /// <returns></returns>
        HttpResult SetpBackFirst()
        {
            
            HttpResult result = null;

            int id = WebUtil.FormInt("id");

            string comment = WebUtil.FormTrim("comment");


            string input_text = WebUtil.FormTrim("input_text");

            try
            {
                FlowBlack(input_text, "back_first", comment);

                result = HttpResult.SuccessMsg("提交成功");
            }
            catch (Exception ex)
            {
                log.Error("流程提交失败", ex);

                result = HttpResult.Error(ex.Message);
            }

            return result;

        }

        #region 流程操作


        string m_FlowCode = "FLOW-16120602";  //流程编码
        string m_FlowTableName = "UT_346";

        /// <summary>
        /// 获取当前流程节点的信息
        /// </summary>
        /// <returns></returns>
        private FlowNodeInfo GetNodeInfo(string table_name,out List<FLOW_DEF_NODE_PARTY> partys)
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(table_name);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", id);
            filter.Fields = new string[] { "BIZ_FLOW_SID", "BIZ_FLOW_INST_CODE", "BIZ_FLOW_STEP_CODE" ,
                "BIZ_FLOW_DEF_CODE","BIZ_FLOW_CUR_NODE_CODE","BIZ_FLOW_CUR_NODE_TEXT"};

            SModel sRow = decipher.GetSModel(filter);

            InfoGrid2.Bll.FlowNodeInfo flowNInfo = null;

            int flowSID = sRow.Get<int>("BIZ_FLOW_SID");

            //List<FLOW_DEF_NODE_PARTY> partys = null;



            partys = null;

            if (flowSID == 0)
            {
                flowNInfo = InfoGrid2.Bll.FlowMgr.GetStartNodeInfo(decipher, m_FlowCode);
            }
            else if (flowSID > 0)
            {
                string curNode = (string)sRow["BIZ_FLOW_CUR_NODE_CODE"];
                flowNInfo = InfoGrid2.Bll.FlowMgr.GetNodeInfo(m_FlowCode, curNode);

                partys = InfoGrid2.Bll.FlowMgr.GetPartys(decipher,m_FlowCode, curNode);
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
        /// <param name="virtual_UserCode">虚拟用户代码</param>
        /// <param name="comment">审批建议</param>
        private void FlowSubmit(string virtual_UserCode,string comment)
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<FLOW_DEF_NODE_PARTY> partys = null;    //下一个接受人信息

            FlowNodeInfo flowNInfo = GetNodeInfo(m_FlowTableName, out partys);   //获取当前节点信息


            FlowOperateMgr flowMgr = new FlowOperateMgr();

            flowMgr.CurFlowCode = m_FlowCode;
            flowMgr.CurNodeCode = flowNInfo.Node.NODE_CODE;
            flowMgr.LineCode = flowNInfo.Lines[0].LINE_CODE;

            flowMgr.ActionType = "submit";
            flowMgr.Comment = comment;
            flowMgr.DocUrl = $"/WF/View/Reim/ShowReimbur.aspx?reim_id={id}";
            flowMgr.DocText = "月度报销";

            flowMgr.BillType = "月度报销单";
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

            if (!StringUtil.IsBlank(virtual_UserCode))
            {
                flowMgr.User = GetVirtualUser(virtual_UserCode);
            }
            else
            {
                flowMgr.User = EcContext.Current.User;
            }

            flowMgr.GoEnter();

            if (flowMgr.Error != null)
            {
                throw flowMgr.Error;
            }


            try
            {

                BusHelper.CreateFlowTemp(m_FlowTableName, id, flowMgr, "reim");

            }
            catch (Exception ex)
            {
                log.Error("创建流程模板消息出错了！-------------------------------------------------", ex);
            }


        }

        /// <summary>
        /// 流程退回
        /// </summary>
        /// <param name="virtual_UserCode">虚拟用户代码</param>
        /// <param name="comment">审批建议</param>
        private void FlowBlack(string virtual_UserCode, string backCommand, string comment )
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<FLOW_DEF_NODE_PARTY> partys = null;    //下一个接受人信息

            FlowNodeInfo flowNInfo = GetNodeInfo(m_FlowTableName, out partys);   //获取当前节点信息


            FlowOperateMgr flowMgr = new FlowOperateMgr();

            flowMgr.CurFlowCode = m_FlowCode;
            flowMgr.CurNodeCode = flowNInfo.Node.NODE_CODE;
            flowMgr.LineCode = backCommand; //必须是 back | back_first   flowNInfo.Lines[0].LINE_CODE;

            flowMgr.ActionType = "back";
            flowMgr.Comment = comment;
            flowMgr.DocUrl = $"/WF/View/Reim/ShowReimbur.aspx?reim_id={id}";
            flowMgr.DocText = "月度报销";

            flowMgr.BillType = "月度报销单";
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


            if (!StringUtil.IsBlank(virtual_UserCode))
            {              
                flowMgr.User = GetVirtualUser(virtual_UserCode);
            }
            else
            {
                flowMgr.User = EcContext.Current.User;
            }

            flowMgr.GoEnter();

            if (flowMgr.Error != null)
            {
                throw flowMgr.Error;
            }

            try
            {

                BusHelper.CreateFlowTemp(m_FlowTableName, id, flowMgr, "reim");

            }
            catch (Exception ex)
            {
                log.Error("创建流程模板消息出错了！-------------------------------------------------", ex);
            }

        }

        /// <summary>
        /// 获取一个虚拟用户
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        private EcUserState GetVirtualUser(string userCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter vuFilter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            //vuFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            vuFilter.And("BIZ_USER_CODE", userCode);
            //.Fields = new string[] { "TRUE_NAME", "LOGIN_NAME" };

            SEC_LOGIN_ACCOUNT acct = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT>(vuFilter);

            if (acct == null)
            {
                throw new Exception("没有这个用户编码: " + userCode);
            }

            EcUserState vUser = new EcUserState();
            vUser.ExpandPropertys["USER_CODE"] = userCode;
            vUser.LoginID = acct.LOGIN_NAME;
            vUser.LoginName = acct.TRUE_NAME;

            return vUser;
        }


        #endregion

        /// <summary>
        /// 周报告的流程同意函数
        /// </summary>
        /// <returns></returns>
        HttpResult SetpAgreeByReport()
        {


            HttpResult result = null;

            int id = WebUtil.FormInt("id");


            string comment = WebUtil.FormTrim("comment");


            string input_text = WebUtil.FormTrim("input_text");

            try
            {
                FlowSubmitReport(input_text, comment);

                result = HttpResult.SuccessMsg("提交成功");
            }
            catch (Exception ex)
            {
                log.Error("流程提交失败", ex);

                result = HttpResult.Error(ex.Message);
            }

            return result;


        }

        /// <summary>
        /// 周报告的流程拒绝函数
        /// </summary>
        /// <returns></returns>
        HttpResult SetpBackByReport()
        {


            HttpResult result = null;

            int id = WebUtil.FormInt("id");

            string comment = WebUtil.FormTrim("comment");


            string input_text = WebUtil.FormTrim("input_text");

            try
            {
                FlowBlackReport(input_text, "back", comment);

                result = HttpResult.SuccessMsg("提交成功");
            }
            catch (Exception ex)
            {
                log.Error("流程提交失败", ex);

                result = HttpResult.Error(ex.Message);
            }

            return result;

        }

        /// <summary>
        /// 周报告的流程退到首环节
        /// </summary>
        /// <returns></returns>
        HttpResult SetpBackStratByReport()
        {


            HttpResult result = null;

            int id = WebUtil.FormInt("id");

            string comment = WebUtil.FormTrim("comment");


            string input_text = WebUtil.FormTrim("input_text");

            try
            {
                FlowBlackReport(input_text, "back_first", comment);

                result = HttpResult.SuccessMsg("提交成功");
            }
            catch (Exception ex)
            {
                log.Error("流程提交失败", ex);

                result = HttpResult.Error(ex.Message);
            }

            return result;

        }



        #region 周报告的流程操作

        string m_FlowCodeReport = "FLOW-17021502";  //流程编码

        string m_FlowTableNameReport = "UT_371";


        /// <summary>
        /// 流程提交,或 审核通过 周报告用的
        /// </summary>
        /// <param name="virtual_UserCode">虚拟用户代码</param>
        /// <param name="comment">审批建议</param>
        private void FlowSubmitReport(string virtual_UserCode, string comment)
        {

            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<FLOW_DEF_NODE_PARTY> partys = null;    //下一个接受人信息

            FlowNodeInfo flowNInfo = GetNodeInfo(m_FlowTableNameReport, out partys);   //获取当前节点信息


            FlowOperateMgr flowMgr = new FlowOperateMgr();

            flowMgr.CurFlowCode = m_FlowCodeReport;
            flowMgr.CurNodeCode = flowNInfo.Node.NODE_CODE;
            flowMgr.LineCode = flowNInfo.Lines[0].LINE_CODE;

            flowMgr.ActionType = "submit";
            flowMgr.Comment = comment;
            flowMgr.DocUrl = $"/WF/View/Report/ReportFlow.aspx?report_id={id}";
            flowMgr.DocText = "周报告";

            flowMgr.BillType = "周报告";
            flowMgr.BillCodeField = "COL_37";   //业务单号

            flowMgr.TableName = m_FlowTableNameReport;
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

            if (!StringUtil.IsBlank(virtual_UserCode))
            {
                flowMgr.User = GetVirtualUser(virtual_UserCode);
            }
            else
            {
                flowMgr.User = EcContext.Current.User;
            }

            flowMgr.GoEnter();

            if (flowMgr.Error != null)
            {
                throw flowMgr.Error;
            }



            try
            {

                BusHelper.CreateFlowTemp(m_FlowTableNameReport, id, flowMgr, "report");

            }catch(Exception ex)
            {
                log.Error("创建流程模板消息出错了！-------------------------------------------------", ex);
            }


        }


        /// <summary>
        /// 流程退回
        /// </summary>
        /// <param name="virtual_UserCode">虚拟用户代码</param>
        /// <param name="backCommand">拒绝意见</param>
        /// <param name="comment">审批建议</param>
        private void FlowBlackReport(string virtual_UserCode, string backCommand, string comment)
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<FLOW_DEF_NODE_PARTY> partys = null;    //下一个接受人信息

            FlowNodeInfo flowNInfo = GetNodeInfo(m_FlowTableNameReport, out partys);   //获取当前节点信息


            FlowOperateMgr flowMgr = new FlowOperateMgr();

            flowMgr.CurFlowCode = m_FlowCodeReport;
            flowMgr.CurNodeCode = flowNInfo.Node.NODE_CODE;
            flowMgr.LineCode = backCommand; //必须是 back | back_first   flowNInfo.Lines[0].LINE_CODE;

            flowMgr.ActionType = "back";
            flowMgr.Comment = comment;
            flowMgr.DocUrl = $"/WF/View/Report/ReportFlow.aspx?report_id={id}";
            flowMgr.DocText = "周报告";

            flowMgr.BillType = "周报告";
            flowMgr.BillCodeField = "COL_37";   //业务单号

            flowMgr.TableName = m_FlowTableNameReport;
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


            if (!StringUtil.IsBlank(virtual_UserCode))
            {
                flowMgr.User = GetVirtualUser(virtual_UserCode);
            }
            else
            {
                flowMgr.User = EcContext.Current.User;
            }

            flowMgr.GoEnter();

            if (flowMgr.Error != null)
            {
                throw flowMgr.Error;
            }

            try
            {

                BusHelper.CreateFlowTemp(m_FlowTableNameReport, id, flowMgr, "report");

            }
            catch (Exception ex)
            {
                log.Error("创建流程模板消息出错了！-------------------------------------------------", ex);
            }



        }



        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}