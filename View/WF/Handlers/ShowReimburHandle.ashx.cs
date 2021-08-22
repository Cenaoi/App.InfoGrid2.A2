using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.WF.Bll;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// ShowReimburHandle 的摘要说明
    /// </summary>
    public class ShowReimburHandle : IHttpHandler, IRequiresSessionState
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

                    case "INIT_DATA_SHOW_REIMBUR":
                        result = InitDataShowReimbur();
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



        HttpResult InitDataShowReimbur()
        {
            int id = WebUtil.FormInt("r_id");

            SModel result = new SModel();

            result["img_data"] = GetImgsArray(id);
            result["annex_data"] = GetAnnexsArray(id);
            result["flow_insts"] = GetFlowInstArray(id);
            result["reim_obj"] = GetReimObj(id);
            result["old_flow_insts"] = GetOldFlowInstArray(id);
            result["btn_enabled"] = GetBtnEnabledBool(id);

            return HttpResult.Success(result);
        }


        /// <summary>
        /// 获取报销单头对象
        /// </summary>
        /// <returns></returns>
        public SModel GetReimObj(int id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_346", id);

            SModel sm = new SModel();

            sm["col_23"] = lm346.Get<decimal>("COL_23").ToString("0.00");
            sm["col_1"] = BusHelper.FormatDate(lm346.Get<string>("COL_1"), "yyyy-MM");
            sm["col_5"] = lm346["COL_5"];
            sm["col_6"] = lm346["COL_6"];
            sm["col_9"] = lm346["COL_9"];
            sm["col_10"] = lm346["COL_10"];
            sm["col_12"] = lm346["COL_12"];
            sm["col_13"] = lm346["COL_13"];
            sm["col_14"] = lm346["COL_14"];
            sm["col_15"] = lm346["COL_15"];
            sm["col_20"] = lm346["COL_20"];
            sm["col_37"] = lm346["COL_37"];
            sm["col_52"] = lm346["COL_52"];
            sm["col_53"] = lm346["COL_53"];
            sm["biz_flow_inst_code"] = lm346["BIZ_FLOW_INST_CODE"];

            return sm;

        }

        /// <summary>
        /// 获取上传图片集合数据
        /// </summary>
        /// <returns></returns>
        public SModelList GetImgsArray(int id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_346", id);

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
        public SModelList GetAnnexsArray(int id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel UT_346 = decipher.GetModelByPk("UT_346", id);

            if (UT_346 == null)
            {
                return null;
            }

            string annexs_str = UT_346.Get<string>("COL_50");

            SModelList sm_imgs = BusHelper.GetFilesByField(annexs_str);

            return sm_imgs;
        }

        /// <summary>
        /// 获取流程实例数据集合
        /// </summary>
        /// <returns></returns>
        public SModelList GetFlowInstArray(int id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("EXTEND_TABLE", "UT_346");
            lmFilter.And("EXTEND_ROW_ID", id);

            lmFilter.TSqlOrderBy = "FLOW_INST_ID desc";

            FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);

            if (f_inst == null)
            {
                return null;
            }

            LightModelFilter lmFilterStep = new LightModelFilter(typeof(FLOW_INST_STEP));
            lmFilterStep.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterStep.And("INST_CODE", f_inst.INST_CODE);
            lmFilterStep.And("DEF_NODE_TYPE", new string[] { "node", "start", "end" }, Logic.In);

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
                sm["def_node_text"] = GetNodeText(item);
                sm["biz_sid_text"] = GetBizSidText(item);

                sm_insts.Add(sm);

            }

            return sm_insts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetBtnEnabledBool(int id)
        {
            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_346", id);

            if (lm346 == null)
            {
                return false;
            }

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            lmFilter.And("INST_STEP_CODE", lm346["BIZ_FLOW_STEP_CODE"]);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("P_USER_CODE", user_code);
            lmFilter.And("BIZ_SID", 0);

            bool flag = decipher.ExistsModels(lmFilter);

            return true;
        }

        /// <summary>
        /// 获取历史流程数据
        /// </summary>
        /// <returns></returns>
        public SModelList GetOldFlowInstArray(int id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("EXTEND_TABLE", "UT_346");
            lmFilter.And("EXTEND_ROW_ID", id);

            lmFilter.TSqlOrderBy = "FLOW_INST_ID desc";

            List<FLOW_INST> f_insts = decipher.SelectModels<FLOW_INST>(lmFilter);

            if (f_insts.Count == 0)
            {
                return null;
            }

            f_insts.RemoveAt(0);

            SModelList sms = new SModelList();

            foreach (var f_inst in f_insts)
            {
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
        /// 获取节点名称
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string GetNodeText(FLOW_INST_STEP item)
        {

            if (item.DEF_NODE_TYPE == "start")
            {
                return "开始";
            }

            if (item.DEF_NODE_TYPE == "end")
            {
                return "结束";

            }


            return item.DEF_NODE_TEXT;



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