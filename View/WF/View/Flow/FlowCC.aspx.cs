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

namespace App.InfoGrid2.WF.View.Flow
{
    public partial class FlowCC : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 获取当前用用抄送未读数据集合
        /// </summary>
        /// <returns></returns>
        public string GetOnCheckArray()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetInstCopys(0, userCode, "IS_OPEN = 0");


            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList new_docs = new SModelList();

            foreach (SModel sm in docs)
            {
                SModel new_sm = new SModel();


                LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("INST_CODE", sm["INST_CODE"]);

                FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);

                int flow_sid = sm.Get<int>("FLOW_SID");

                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = sm["EXTEND_DOC_TEXT"] + "(" + f_inst.START_USER_TEXT + ")";
                new_sm["text"] = "审批已完成";

                new_sm["url"] = sm["EXTEND_DOC_URL"] + "&copy_id="+sm["FLOW_INST_COPY_ID"];

                new_docs.Add(new_sm);

            }

            return new_docs.ToJson();


        }

        /// <summary>
        ///  获取当前用户抄送已读数据集合
        /// </summary>
        /// <returns></returns>
        public string GetYesCheckArray()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetInstCopys(0, userCode, "IS_OPEN = 1");

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList new_docs = new SModelList();

            foreach (SModel sm in docs)
            {
                SModel new_sm = new SModel();

                LightModelFilter lmFilter = new LightModelFilter(typeof(FLOW_INST));
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("INST_CODE", sm["INST_CODE"]);

                FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(lmFilter);

                int flow_sid = sm.Get<int>("FLOW_SID");

                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = sm["EXTEND_DOC_TEXT"] + "(" + f_inst.START_USER_TEXT + ")";
                new_sm["text"] = "审批已完成";

                new_sm["url"] = sm["EXTEND_DOC_URL"];


                new_docs.Add(new_sm);

            }

            return new_docs.ToJson();

        }
    }
}