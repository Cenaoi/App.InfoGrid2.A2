using App.BizCommon;
using App.InfoGrid2.Handlers;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.View;
using EC5.IO;
using App.InfoGrid2.WF.Bll;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// 报销处理类
    /// </summary>
    public class ReimHandler : IHttpHandler, IRequiresSessionState
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

                    case "NEW_REIM_DETA":
                        result = NewReimDeta();
                        break;
                    case "SAVE_REIM_DETA":
                        result = SaveReimDeta();
                        break;
                    case "DELETE_REIM_DETA":
                        result = DeleteReimDeta();
                        break;
                    case "NEW_TRIP_DETA":
                        result = NewTripDeta();
                        break;
                    case "NEW_WORK_REPORT":
                        result = NewWorkReport();
                        break;
                    case "SAVEALL":
                        result = SaveAll();
                        break;
                    case "SUBMIT_REIM":
                        result = SubmitReim();
                        break;
                    case "SAVETRIPDETA": 
                         result= SaveTripDeta();
                         break;
                    case "WORKREPOSRT_SAVE_ALL":
                        result = WorkReposrtSaveAll();
                        break;
                    case "GET_REIMBUR_DATA":
                        result = GetReimburData();
                        break;
                    default:
                        result = HttpResult.Error("写错了吧！");
                        break;


                }

            }catch(Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error("哦噢，出错了！");

            }

            context.Response.Write(result);

        }


        /// <summary>
        /// 新建工作报告数据
        /// </summary>
        HttpResult NewWorkReport()
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            LModel lm351 = new LModel("UT_351");

            lm351["ROW_SID"] = 0;
            lm351["ROW_DATE_CREATE"] = lm351["ROW_DATE_UPDATE"] = lm351["BIZ_CREATE_DATE"] = lm351["BIZ_UPDATE_DATE"] = DateTime.Now;
            lm351["ROW_AUTHOR_USER_CODE"] = user_code;
            lm351["BIZ_CREATE_USER_CODE"] = user_code;
            lm351["BIZ_CREATE_USER_TEXT"] = user.LoginName;
            lm351["COL_24"] = id;


            decipher.InsertModel(lm351);

            return HttpResult.Success(lm351);

        }

        /// <summary>
        /// 新建出差明细数据
        /// </summary>
        HttpResult NewTripDeta()
        {

            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            LModel lm350 = new LModel("UT_350");

            lm350["ROW_SID"] = 0;
            lm350["ROW_DATE_CREATE"] = lm350["ROW_DATE_UPDATE"] = lm350["BIZ_CREATE_DATE"] = lm350["BIZ_UPDATE_DATE"] = DateTime.Now;
            lm350["ROW_AUTHOR_USER_CODE"] = user_code;
            lm350["BIZ_CREATE_USER_CODE"] = user_code;
            lm350["BIZ_CREATE_USER_TEXT"] = user.LoginName;
            lm350["COL_24"] = id;

            decipher.InsertModel(lm350);

            return HttpResult.Success(lm350);

        }

        /// <summary>
        /// 删除报销明细数据
        /// </summary>
        HttpResult DeleteReimDeta()
        {

            int id = WebUtil.FormInt("id");

            string table_name = WebUtil.FormTrim("table_name");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModelByPk(table_name, id);

            lm["ROW_DATE_DELETE"] = DateTime.Now;
            lm["ROW_SID"] = -3;

            decipher.UpdateModelProps(lm, "ROW_SID", "ROW_DATE_DELETE");


            return HttpResult.SuccessMsg("删除成功了！");

        }

        /// <summary>
        /// 保存报销明细函数
        /// </summary>
        HttpResult SaveReimDeta()
        {
            int id = WebUtil.FormInt("id");

            string reim_deta_json_str = WebUtil.Form("reim_deta_json_str");

            string change_field_str = WebUtil.Form("change_files_str");

            string table_name = WebUtil.FormTrim("table_name");

            if (string.IsNullOrWhiteSpace(table_name))
            {
                return HttpResult.Error("不能没有标明！");
            }

            if (string.IsNullOrWhiteSpace(change_field_str))
            {
                return HttpResult.SuccessMsg("没有改变字段，所以不用保存！");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            int cur_id = 0;


            SModel sm = ParseSModel(reim_deta_json_str, change_field_str, table_name, "ROW_IDENTITY_ID", out cur_id);


            LModel lm = decipher.GetModelByPk(table_name, cur_id);

            lm.SetTakeChange(true);



            foreach(var s in sm.GetFields())
            {
                lm[s] = sm[s];

            }

            try
            {
                EC5.IG2.BizBase.DbCascadeRule.Update(lm);
            }
            catch (Exception ex)
            {
                throw new Exception("联动触发异常.", ex);
            }

            return HttpResult.SuccessMsg("保存成功了！");

        }

        /// <summary>
        /// 创建报销明细
        /// </summary>
        HttpResult NewReimDeta()
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            LModel lm347 = new LModel("UT_347");

            lm347["ROW_SID"] = 0;
            lm347["ROW_DATE_CREATE"] = lm347["ROW_DATE_UPDATE"] = lm347["BIZ_CREATE_DATE"] = lm347["BIZ_UPDATE_DATE"] = DateTime.Now;
            lm347["COL_24"] = id;
            lm347["ROW_AUTHOR_USER_CODE"] = user_code;
            lm347["BIZ_CREATE_USER_CODE"] = user_code;
            lm347["BIZ_CREATE_USER_TEXT"] = user.LoginName;

            decipher.InsertModel(lm347);

            return HttpResult.Success(lm347);

        }
            

        /// <summary>
        /// 解析传上来的字符串成SModel对象
        /// </summary>
        /// <param name="sm_json_str">整个实体json字符串</param>
        /// <param name="change_field_str">改变的字段json字符串</param>
        /// <param name="table_name">表名</param>
        /// <param name="pk_field">主键字段</param>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        SModel ParseSModel(string sm_json_str,string change_field_str,string table_name,string pk_field,out int id)
        {


            JArray ja = JArray.Parse(change_field_str);



            List<string> old_fields = new List<string>();


            foreach (var item in ja)
            {
                old_fields.Add(item.Value<string>());
            }


            SModel sm_002 = SModel.ParseJson(sm_json_str);


            DbDecipher decipher = ModelAction.OpenDecipher();

            id = sm_002.Get<int>(pk_field);

            LModelElement modelElem = LightModel.GetLModelElement(table_name);
            LModelFieldElement fieldElem;

            SModel uModel = new SModel();

            foreach (var item in old_fields)
            {
                if (modelElem.TryGetField(item, out fieldElem))
                {
                    uModel[item] = ModelConvert.ChangeType(sm_002[item], fieldElem);
                }
            }

            return uModel;


        }


        /// <summary>
        /// 提交报销
        /// </summary>
        HttpResult SubmitReim()
        {

            int id = WebUtil.FormInt("id");

            //DbDecipher decipher = ModelAction.OpenDecipher();

            //LModel lm346 = decipher.GetModelByPk("UT_346", id);

            //lm346["BIZ_SID"] = 2;
            //lm346["ROW_DATE_UPDATE"] = DateTime.Now;

            //decipher.UpdateModelProps(lm346, "BIZ_SID", "ROW_DATE_UPDATE");

            HttpResult result = CheckUT347(id);

            if (!result.success)
            {
                return result;
            }

            EcUserState user = EcContext.Current.User;

            if (string.IsNullOrWhiteSpace(user.ExpandPropertys["USER_CODE"]))
            {
                return HttpResult.Error("请重新登录");
            }

            try
            {
                FlowSubmit();

                return HttpResult.Success("提交成功");
            }
            catch(Exception ex)
            {
                log.Error("提交失败.", ex);

                return HttpResult.Error("提交失败");
            }


        }



        /// <summary>
        /// 月度报销单提交-检查报销明细必填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HttpResult CheckUT347(int id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_347");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.And("COL_24", id);

            lmFilter.Fields = new string[] { "ROW_IDENTITY_ID", "COL_8", "COL_17", "COL_40", "COL_27", "COL_22", "COL_23", "COL_12", "COL_13", "COL_14", "COL_15", "COL_28", "COL_29" };

            SModelList sm_reim_detas = decipher.GetSModelList(lmFilter);

            foreach (var item in sm_reim_detas)
            {
                if (string.IsNullOrWhiteSpace(item.GetString("COL_8")))
                {
                    return HttpResult.Error("请选择报销日期");
                }

                if (string.IsNullOrWhiteSpace(item.GetString("COL_13")))
                {
                    return HttpResult.Error("请选择报销类型");
                }

                if (string.IsNullOrWhiteSpace(item.GetString("COL_27")))
                {
                    return HttpResult.Error("请填写费用明细描述");
                }
            }

            return HttpResult.Success("ok");

        }


        HttpResult SaveAll() {

            log.Debug("进来了========================================");
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();


                var sm_json = WebUtil.Form("smlistjson");

                log.Debug("sm_json" + sm_json);

                var table_name = WebUtil.Form("tablename");

                log.Debug("table_name" + table_name);

                SModelList sList = SModelList.ParseJson(sm_json);

                log.Debug("SModellist=====================" + sList);
                foreach (SModel sm in sList)
                {
                    int id = sm.Get<int>("ROW_IDENTITY_ID");

                    LModel lmodel = decipher.GetModelByPk(table_name, id);

                    lmodel.SetTakeChange(true);


                    sm.Remove("ROW_IDENTITY_ID");
                    sm.Remove("imgs");
                    sm.Remove("annexs");
                    foreach (var s in sm.GetFields())
                    {
                        lmodel[s] = sm[s];
                    }

                    try
                    {
                        EC5.IG2.BizBase.DbCascadeRule.Update(lmodel);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"保存联动出错了,数据集合:{sm_json},表名:{table_name}", ex);
                        throw new Exception("联动触发异常.", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error("全部保存失败", ex);
            }

            return HttpResult.SuccessMsg("成功");

        }


        #region 流程操作


        string m_FlowCode = "FLOW-16120602";  //流程编码
        string m_FlowTableName = "UT_346";

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

                partys = InfoGrid2.Bll.FlowMgr.GetPartys(decipher, m_FlowCode, curNode);
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
            flowMgr.DocUrl = $"/WF/View/Reim/ShowReimbur.aspx?reim_id={id}";
            flowMgr.PageId = GlobelParam.GetValue("月度报销单_PAGE_ID",1702, "流程名称【月度报销】");
            flowMgr.MenuId = GlobelParam.GetValue("月度报销单_MENU_ID", 683, "流程名称【月度报销】");
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


            flowMgr.User = EcContext.Current.User;

            flowMgr.GoEnter();

            if(flowMgr.Error != null)
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
        private void FlowBlack()
        {

        }


        #endregion

        HttpResult SaveTripDeta()
        {
            try
            {
                log.Debug("TripDaeta 保存并返回方法进来了");

                DbDecipher decipher = ModelAction.OpenDecipher();

                var sm_json = WebUtil.Form("smlistjson");

                var table_name = WebUtil.Form("table_name");

                SModelList sList = SModelList.ParseJson(sm_json);

                foreach (SModel item in sList)
                {
                    int pk_id = item.GetInt("ROW_IDENTITY_ID");

                    LModel lmodel = decipher.GetModelByPk(table_name, pk_id);

                    item.Remove("ROW_IDENTITY_ID");
                    item.Remove("imgs");
                    item.Remove("annexs");
                    lmodel.SetTakeChange(true);

                    foreach (string fild in item.GetFields())
                    {
                        lmodel[fild] = item[fild];
                    }

                  
                    try
                    {
                        EC5.IG2.BizBase.DbCascadeRule.Update(lmodel);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("联动触发异常.", ex);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("错误信息",ex);
            }


            return HttpResult.SuccessMsg("保存成功了！");
        }


        HttpResult WorkReposrtSaveAll()
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                string json_str = WebUtil.Form("json_to_str");

                string table_name = WebUtil.Form("table_name");

                log.Debug("WorkReposrtSaveAll集合数据" + json_str);

                SModelList slist = SModelList.ParseJson(json_str);

                foreach (SModel sm in slist)
                {
                    int pk_id = sm.GetInt("ROW_IDENTITY_ID");

                    LModel model = decipher.GetModelByPk(table_name, pk_id);

                    model.SetTakeChange(true);

                    sm.Remove("ROW_IDENTITY_ID");
                    sm.Remove("imgs");
                    sm.Remove("annexs");

                    foreach (var fild in sm.GetFields())
                    {
                        model[fild] = sm[fild];
                    }

                    try
                    {
                        EC5.IG2.BizBase.DbCascadeRule.Update(model);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("联动触发异常.", ex);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("错误信息为", ex);
            }


            return HttpResult.SuccessMsg("成功");
        }



        /// <summary>
        /// Reimbur页面获取数据
        /// </summary>
        /// <returns></returns>
        HttpResult GetReimburData()
        {
            int id = WebUtil.FormInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_346", id);

            SModel sm = new SModel();
            sm["img_data"] = GetImgsArray(lm346);
            sm["annex_data"] = GetAnnexsArray(lm346);
            sm["reim_obj"] = GetReimObj(lm346);
            sm["flow_insts"] = GetFlowInstArray(lm346);
            sm["old_flow_insts"] = GetOldFlowInstArray(lm346, id);

            return HttpResult.Success(sm);
        }



        /// <summary>
        /// 获取报销单头对象
        /// </summary>
        /// <returns></returns>
        public SModel GetReimObj(LModel lm346)
        {
            SModel sm = new SModel();

            sm["COL_23"] = lm346.Get<decimal>("COL_23").ToString("0.00");
            sm["COL_1"] = BusHelper.FormatDate(lm346.Get<string>("COL_1"), "yyyy-MM");
            sm["ROW_IDENTITY_ID"] = lm346["ROW_IDENTITY_ID"];

            return sm;
        }


        /// <summary>
        /// 获取上传图片集合数据
        /// </summary>
        /// <returns></returns>
        public SModelList GetImgsArray(LModel lm346)
        {
            if (lm346 == null)
            {
                return null;
            }

            string imgs_str = lm346.Get<string>("COL_49");

            SModelList sm_imgs = BusHelper.GetFilesByField(imgs_str);

            return sm_imgs;
        }

        /// <summary>
        /// 获取上传附件集合数据
        /// </summary>
        /// <returns></returns>
        public SModelList GetAnnexsArray(LModel lm346)
        {
            if (lm346 == null)
            {
                return null;
            }

            string annexs_str = lm346.Get<string>("COL_50");

            SModelList sm_imgs = BusHelper.GetFilesByField(annexs_str);

            return sm_imgs;

        }

        /// <summary>
        /// 获取流程实例数据集合
        /// </summary>
        /// <returns></returns>
        public SModelList GetFlowInstArray(LModel lm346)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            if (lm346 == null)
            {
                return null;
            }

            string biz_flow_step_code = lm346.Get<string>("BIZ_FLOW_STEP_CODE");

            if (string.IsNullOrWhiteSpace(biz_flow_step_code))
            {
                return null;
            }

            LightModelFilter lmFilterStep = new LightModelFilter(typeof(FLOW_INST_STEP));
            lmFilterStep.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterStep.And("INST_CODE", biz_flow_step_code);
            lmFilterStep.And("DEF_NODE_TYPE", "node");

            lmFilterStep.TSqlOrderBy = "SEQ_NUM";

            List<FLOW_INST_STEP> fiss = decipher.SelectModels<FLOW_INST_STEP>(lmFilterStep);

            SModelList sm_insts = new SModelList();

            foreach (FLOW_INST_STEP item in fiss)
            {

                SModel sm = new SModel();
                sm["date_end"] = item.DATE_END;
                sm["op_check_comments"] = item.OP_CHECK_COMMENTS;
                sm["op_check_desc"] = item.OP_CHECK_DESC;
                sm["step_sid"] = item.STEP_SID;
                sm["date_start"] = item.DATE_START;
                sm["is_back_operate"] = item.IS_BACK_OPERATE;
                sm["from_line_text"] = item.FROM_LINE_TEXT;
                sm["def_node_text"] = item.DEF_NODE_TEXT;
                sm["biz_sid_text"] = GetBizSidText(item);

                sm_insts.Add(sm);
            }

            return sm_insts;
        }



        /// <summary>
        /// 获取节点状态名称
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string GetBizSidText(FLOW_INST_STEP item)
        {

            string biz_sid_text = string.Empty;

            if (item.STEP_SID == 2)
            {
                biz_sid_text = "审核中";
            }
            else if (item.STEP_SID == 4)
            {
                if (item.IS_BACK_OPERATE)
                {
                    biz_sid_text = "退回";
                }
                else
                {
                    biz_sid_text = "通过";
                }
            }

            return biz_sid_text;
        }

        /// <summary>
        /// 获取历史流程数据
        /// </summary>
        /// <returns></returns>
        public SModelList GetOldFlowInstArray(LModel lm346, int reim_id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("EXTEND_TABLE", "UT_346");
            lmFilter.And("EXTEND_ROW_ID", reim_id);

            lmFilter.TSqlOrderBy = "FLOW_INST_ID desc";

            List<FLOW_INST> f_insts = decipher.SelectModels<FLOW_INST>(lmFilter);

            SModelList sms = new SModelList();

            if (f_insts.Count == 0)
            {
                return sms;
            }

            if (lm346 == null)
            {
                return sms;
            }

            string biz_flow_step_code = lm346.Get<string>("BIZ_FLOW_STEP_CODE");

            foreach (var f_inst in f_insts)
            {

                if (f_inst.INST_CODE == biz_flow_step_code)
                {
                    continue;
                }

                LightModelFilter lmFilterStep = new LightModelFilter(typeof(FLOW_INST_STEP));
                lmFilterStep.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilterStep.And("INST_CODE", f_inst.INST_CODE);
                lmFilterStep.And("DEF_NODE_TYPE", "node");

                lmFilterStep.TSqlOrderBy = "SEQ_NUM";

                List<FLOW_INST_STEP> fiss = decipher.SelectModels<FLOW_INST_STEP>(lmFilterStep);

                SModel sm_inst = new SModel();

                sm_inst["inst_code"] = f_inst.INST_CODE;

                SModelList sm_insts = new SModelList();

                foreach (FLOW_INST_STEP item in fiss)
                {

                    SModel sm = new SModel();
                    sm["date_end"] = item.DATE_END;
                    sm["op_check_comments"] = item.OP_CHECK_COMMENTS;
                    sm["op_check_desc"] = item.OP_CHECK_DESC;
                    sm["step_sid"] = item.STEP_SID;
                    sm["date_start"] = item.DATE_START;
                    sm["is_back_operate"] = item.IS_BACK_OPERATE;
                    sm["from_line_text"] = item.FROM_LINE_TEXT;
                    sm["def_node_text"] = item.DEF_NODE_TEXT;
                    sm["biz_sid_text"] = GetBizSidText(item);

                    sm_insts.Add(sm);
                }

                sm_inst["data"] = sm_insts;

                sms.Add(sm_inst);
            }

            return sms;
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