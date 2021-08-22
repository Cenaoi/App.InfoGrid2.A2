using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.WF.Bll;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.Utility;
using System.Text;

namespace App.InfoGrid2.WF.View.Flow
{
    public partial class FlowEaa : Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 获取当前用户未审核数据集合
        /// </summary>
        /// <returns></returns>
        public string GetOnCheckArray()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserDocs(0, userCode, string.Empty);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList new_docs = new SModelList();

            foreach (SModel sm in docs)
            {
                SModel new_sm = new SModel();


                LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("INST_CODE", sm["INST_CODE"]);

                FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);


                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = sm["EXTEND_DOC_TEXT"]+"(" + f_inst.START_USER_TEXT + ")";
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

                string url = FlowUrlMgr.Get("mobile",0, sm["EXTEND_TABLE"], sm);
                new_sm["url"] = StringUtil.NoBlank(url, sm["EXTEND_DOC_URL"]);
                new_sm["flow_id"] = sm["FLOW_INST_PARTY_ID"];
                new_sm["biz_sid"] = 0;


                new_docs.Add(new_sm);

            }

            return new_docs.ToJson();


        }

        /// <summary>
        ///  获取当前用户已审核数据集合
        /// </summary>
        /// <returns></returns>
        public string GetYesCheckArray()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserDocs(2, userCode, string.Empty);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList new_docs = new SModelList();

            foreach (SModel sm in docs)
            {
                SModel new_sm = new SModel();


                LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("INST_CODE", sm["INST_CODE"]);

                FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);

                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = sm["EXTEND_DOC_TEXT"] + "(" + f_inst.START_USER_TEXT + ")";
                new_sm["text"] = GetFlowSid(sm["FLOW_SID"]);

                //new_sm["url"] = sm["EXTEND_DOC_URL"];



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


                string url = FlowUrlMgr.Get("mobile",0, sm["EXTEND_TABLE"], sm);
                new_sm["url"] = StringUtil.NoBlank(url, sm["EXTEND_DOC_URL"]);
                new_sm["flow_id"] = sm["FLOW_INST_PARTY_ID"];
                new_sm["biz_sid"] = 2;

                new_docs.Add(new_sm);

            }

            return new_docs.ToJson();

        }

        string GetFlowSid(int flow_sid)
        {

            if(flow_sid == 2)
            {
                return "进行中...";
            }


            if(flow_sid == 4)
            {
                return "流程结束";
            }

            return "作废的";


        }


    }
}