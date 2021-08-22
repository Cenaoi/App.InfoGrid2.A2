using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using EC5.IO;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.View.OneFlowBuilder
{
    /// <summary>
    /// FlowInstHandler 的摘要说明
    /// </summary>
    public class FlowInstHandler :  IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string json;


            HttpResult result = null;
            string action = WebUtil.QueryTrimUpper("action");

            try
            {
                result = ProAction(context, action);
            }
            catch (Exception ex)
            {
                log.Error($"执行 Action={action} 错误.", ex);

                result = HttpResult.Error("提交错误");
            }
            
            context.Response.Write(result);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private HttpResult ProAction(HttpContext context, string action)
        {
            if (string.IsNullOrEmpty(action))
            {
                throw new Exception("必须指定 Action .");
            }


            HttpResult result = null;

            switch (action)
            {
                case "GET_FLOW_INST":
                    result = GetFlowInst(context);
                    break;
            }

            return result;
        }


        private HttpResult GetFlowInst(HttpContext context)
        {
            //

            string instCode = WebUtil.FormTrim("flow_inst_code");   //流程实例编码

            if (StringUtil.IsBlank(instCode) || !ValidateUtil.SqlInput(instCode))
            {
                throw new Exception("参数异常.");
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            FLOW_INST inst = decipher.SelectToOneModel<FLOW_INST>($"ROW_SID >= 0 and INST_CODE = '{instCode}'");
            
            if(inst == null)
            {
                throw new Exception("参数异常.");
            }

            int def_id = inst.DEF_ID;

            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            List<FLOW_DEF_LINE> def_lines = decipher.SelectModels<FLOW_DEF_LINE>($"ROW_SID >= 0 and DEF_ID = {def_id}");
            List<FLOW_DEF_NODE> def_nodes = decipher.SelectModels<FLOW_DEF_NODE>($"ROW_SID >= 0 and DEF_ID = {def_id}");

            LModelList<FLOW_INST_STEP> inst_Steps = decipher.SelectModels<FLOW_INST_STEP>($"ROW_SID >= 0 and INST_CODE = '{instCode}' and IS_REVOKED=0");

            LModelGroup<FLOW_INST_STEP,string> instNodeGroup = inst_Steps.ToGroup<string>("DEF_NODE_CODE");

            LModelGroup<FLOW_INST_STEP, string> instLineGroup = inst_Steps.ToGroup<string>("FROM_LINE_CODE");

            LModelList<FLOW_INST_STEP> stepList;


            SModel data = new SModel();

            data["def_id"] = def.FLOW_DEF_ID;

            SModelList items = new SModelList();

            foreach (var item in def_nodes)
            {

                SModel sm = SModel.ParseJson(item.GRAPHICS);
                sm["item_fullname"] = item.G_FULLNAME;
                sm["text"] = item.NODE_TEXT;
                sm["style_name"] = item.STYLE_NAME;

                sm["is_preview"] = true;

                if (instNodeGroup.TryGetValue(item.NODE_CODE, out stepList))
                {
                    sm["step_count"] = stepList.Count;
                    
                }

                items.Add(sm);
            }

            foreach (var item in def_lines)
            {
                SModel sm = SModel.ParseJson(item.GRAPHICS);
                sm["item_fullname"] = item.G_FULLNAME;
                sm["text"] = item.LINE_TEXT;
                sm["style_name"] = item.STYLE_NAME;

                sm["is_preview"] = true;

                if (instLineGroup.TryGetValue(item.LINE_CODE, out stepList))
                {
                    sm["step_count"] = stepList.Count;
                    
                }

                items.Add(sm);
            }


            data["items"] = items;

            
            return HttpResult.Success(data);
        }

    }
}