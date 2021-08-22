using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.WF.Bll;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.WF.View.Report
{
    public partial class ReportFlow : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 获取周报告详细信息
        /// </summary>
        /// <returns></returns>
        public string GetReportObj()
        {

            int id = WebUtil.QueryInt("report_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm371 = decipher.GetModelByPk("UT_371", id);

            SModel sm = new SModel();

            lm371.CopyTo(sm);

            int row_id = lm371.Get<int>("ROW_IDENTITY_ID");


            sm["COL_1"] = BusHelper.FormatDate(lm371.Get<string>("COL_1"));

            SModel imgs = new SModel();

            imgs["data"] = BusHelper.GetFilesByField(lm371.Get<string>("COL_49"));
            imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_371&tag_code=report_img&row_id=" + row_id;
            imgs["delete_img_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
            imgs["row_id"] = row_id;
            imgs["table_name"] = "UT_371";
            imgs["tag_code"] = "report_img";
            imgs["btn_id"] = "uploader_img_" + row_id;
            imgs["field_text"] = "COL_49";
            imgs["read_only"] = true;
            
            sm["imgs"] = imgs;

            SModel annexs = new SModel();

            annexs["data"] = BusHelper.GetFilesByField(sm.Get<string>("COL_50"));
            annexs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_371&tag_code=report_annex&row_id=" + row_id;
            annexs["delete_annex_url"] = "/WF/Handlers/UploaderFileHandle.ashx";
            annexs["btn_id"] = "uploader_annex_" + row_id;
            annexs["row_id"] = row_id;
            annexs["table_name"] = "UT_371";
            annexs["tag_code"] = "report_annex";
            annexs["field_text"] = "COL_50";
            annexs["read_only"] = true;
            
            sm["annexs"] = annexs;


            return sm.ToJson();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetBtnEnabledBool()
        {


            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            int reim_id = WebUtil.QueryInt("report_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_371", reim_id);

            if (lm346 == null)
            {
                return "0";
            }

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            lmFilter.And("INST_STEP_CODE", lm346["BIZ_FLOW_STEP_CODE"]);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("P_USER_CODE", user_code);
            lmFilter.And("BIZ_SID", 0);

            bool flag = decipher.ExistsModels(lmFilter);


            return flag ? "true" : "false";


        }

        /// <summary>
        /// 获取历史流程数据
        /// </summary>
        /// <returns></returns>
        public string GetOldFlowInstArray()
        {
            int reim_id = WebUtil.QueryInt("report_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("EXTEND_TABLE", "UT_371");
            lmFilter.And("EXTEND_ROW_ID", reim_id);

            lmFilter.TSqlOrderBy = "FLOW_INST_ID desc";


            List<FLOW_INST> f_insts = decipher.SelectModels<FLOW_INST>(lmFilter);


            if (f_insts.Count == 0)
            {
                return "[]";
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
            return sms.ToJson();

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
        /// 获取流程实例数据集合
        /// </summary>
        /// <returns></returns>
        public string GetFlowInstArray()
        {

            int reim_id = WebUtil.QueryInt("report_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("EXTEND_TABLE", "UT_371");
            lmFilter.And("EXTEND_ROW_ID", reim_id);

            lmFilter.TSqlOrderBy = "FLOW_INST_ID desc";


            FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);


            if (f_inst == null)
            {
                return "[]";
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

            return sm_insts.ToJson();


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

    }
}