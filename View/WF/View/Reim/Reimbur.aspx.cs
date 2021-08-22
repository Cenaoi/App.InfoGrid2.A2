using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.WF.Bll;
using EC5.SystemBoard;
using EC5.Utility;
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

namespace App.InfoGrid2.WF.View.Reim
{
    public partial class Reimbur : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// 获取报销单头对象
        /// </summary>
        /// <returns></returns>
        public string GetReimObj()
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_346", id);

            SModel sm = new SModel();

            sm["COL_23"] = lm346.Get<decimal>("COL_23").ToString("0.00");
            sm["COL_1"] = BusHelper.FormatDate(lm346.Get<string>("COL_1"), "yyyy-MM");
            sm["ROW_IDENTITY_ID"] = lm346["ROW_IDENTITY_ID"];

            return sm.ToJson();


        }


        /// <summary>
        /// 获取上传图片集合数据
        /// </summary>
        /// <returns></returns>
        public string GetImgsArray()
        {

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm346 = decipher.GetModelByPk("UT_346", id);

            if (lm346 == null)
            {
                return "0";
            }


            string imgs_str = lm346.Get<string>("COL_49");


            SModelList sm_imgs = BusHelper.GetFilesByField(imgs_str);

            return sm_imgs.ToJson();


        }

        /// <summary>
        /// 获取上传附件集合数据
        /// </summary>
        /// <returns></returns>
        public string GetAnnexsArray()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel UT_346 = decipher.GetModelByPk("UT_346", id);

            if (UT_346 == null)
            {
                return "0";
            }

            string annexs_str = UT_346.Get<string>("COL_50");

            SModelList sm_imgs = BusHelper.GetFilesByField(annexs_str);

            return sm_imgs.ToJson();

        }

        /// <summary>
        /// 获取流程实例数据集合
        /// </summary>
        /// <returns></returns>
        public string GetFlowInstArray()
        {

            int reim_id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm346 = decipher.GetModelByPk("UT_346", reim_id);


            if (lm346 == null)
            {
                return "[]";
            }

            string biz_flow_step_code = lm346.Get<string>("BIZ_FLOW_STEP_CODE");

            if (string.IsNullOrWhiteSpace(biz_flow_step_code))
            {
                return "[]";
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

            return sm_insts.ToJson();


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
        public string GetOldFlowInstArray()
        {
            int reim_id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("EXTEND_TABLE", "UT_346");
            lmFilter.And("EXTEND_ROW_ID", reim_id);

            lmFilter.TSqlOrderBy = "FLOW_INST_ID desc";

            List<FLOW_INST> f_insts = decipher.SelectModels<FLOW_INST>(lmFilter);

            if (f_insts.Count == 0)
            {
                return "[]";
            }

            

            LModel lm346 = decipher.GetModelByPk("UT_346", reim_id);


            if (lm346 == null)
            {
                return "[]";
            }

            string biz_flow_step_code = lm346.Get<string>("BIZ_FLOW_STEP_CODE");

            SModelList sms = new SModelList();

            foreach (var f_inst in f_insts)
            {

                if(f_inst.INST_CODE == biz_flow_step_code)
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



            return sms.ToJson();

        }



    }
}