using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.Model.SecModels;
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


namespace App.InfoGrid2.View.OneForm
{
    public partial class FlowHandle : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = WebUtil.QueryUpper("action");

            HttpResult pack = null;

            try
            {
                pack = ProAction(action);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                pack = HttpResult.Error("执行指令错误:" + ex.Message);
            }
                        
            Response.Clear();

            Response.Write(pack);

            Response.End();

        }


        private HttpResult ProAction(string action)
        {
            HttpResult pack = null;

            switch (action)
            {
                case "GET_BUTTONS":
                    pack = GetButtons();
                    break;
                
            }

            return pack;
        }


        private LModel GetRow(string tableName, int rowId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelElement modelElem = LightModel.GetLModelElement(tableName);

            LightModelFilter rowFilter = new LightModelFilter(tableName);
            rowFilter.AddFilter("ROW_SID >= 0");
            rowFilter.And(modelElem.PrimaryKey, rowId);

            rowFilter.Fields = new string[] {
                modelElem.PrimaryKey,
                "BIZ_SID",
                "BIZ_FLOW_SID",
                "BIZ_FLOW_INST_CODE",
                "BIZ_FLOW_DEF_CODE",
                "BIZ_FLOW_CUR_NODE_CODE",
                "BIZ_FLOW_CUR_NODE_TEXT"
            };

            LModel row = decipher.GetModel(rowFilter);

            if (row == null)
            {
                throw new Exception( $"tableName={tableName}, row_id={rowId} 不存在");
            }

            return row;
        }


        private LModel GetTable(int table_id)
        {
            if(table_id <= 0) { throw new Exception($"实体定义不存在.table_id ={ table_id }"); }

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("IG2_TABLE_ID", table_id);
            filter.AddFilter("ROW_SID >= 0");
            filter.Fields = new string[] { "TABLE_NAME", "FLOW_ENABLED", "FLOW_PARAMS" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModel(filter);

            if (model == null)
            {
                throw new Exception($"实体定义不存在. table_id={table_id}");
            }

            return model;
        }

        private SModelList ParseJson(string flowParamsJson)
        {

            SModelList smList;

            try
            {
                smList = SModelList.ParseJson(flowParamsJson);
            }
            catch (Exception ex)
            {
                log.Error("流程参数定义错误: \r\n" + flowParamsJson, ex);

                throw new Exception("流程内部参数定义错误: \r\n" + flowParamsJson,ex);
            }

            return smList;
        }

        /// <summary>
        /// 根据记录的 BIZ_SID 获取流程
        /// </summary>
        /// <param name="smList"></param>
        /// <param name="curBizSID"></param>
        /// <returns></returns>
        private List<SModel> GetDefineCode(SModelList smList,int curBizSID,SModel exParam)
        {
            EcUserState userState = EcContext.Current.User;

            UserSecritySet useSec = SecFunMgr.GetUserSecuritySet();

            

            List<SModel> flowCodes = new List<SModel>();


            string curFlowCode = null;

            int[] curBizSIDList;

            foreach (var sm in smList)
            {
                object row_sid_list = sm["BIZ_SID"];
                string flowCode = (string)sm["FLOW_CODE"];
                string structCode = (string)sm["SEC_STRUCE_CODE"];//权限结构

                #region 过滤结构权限

                if (!StringUtil.IsBlank(structCode))
                {
                    //如果这个用户没有这个结构编码, 就过滤掉
                    if(!ArrayUtil.Exist(useSec.ArrStructCode, structCode))
                    {
                        continue;
                    }
                }


                #endregion


                if (ModelHelper.IsNumberType(row_sid_list.GetType()))
                {
                    curBizSIDList = new int[] { Convert.ToInt32(row_sid_list) };
                }
                else if (row_sid_list is string)
                {
                    curBizSIDList = StringUtil.ToIntList((string)row_sid_list);
                }
                else
                {
                    curBizSIDList = null;
                }

                if (ArrayUtil.Exist(curBizSIDList, curBizSID))
                {
                    curFlowCode = flowCode;

                    if (exParam != null)
                    {
                        if (exParam["bill_type"] == null)
                        {
                            exParam["bill_type"] = sm["BILL_TYPE"];
                        }

                        if (exParam["bill_code_field"] == null)
                        {
                            exParam["bill_code_field"] = sm["BILL_CODE_FIELD"];
                        }
                    }

                    SModel flow = new SModel();
                    flow["code"] = flowCode;
                    flow["text"] = sm["TEXT"];

                    flow["bill_type"] = sm["BILL_TYPE"];
                    flow["bill_code_field"] = sm["BILL_CODE_FIELD"];    //单据编码的字段

                    flow["bill_type_field"] = sm["BILL_TYPE_FIELD"];    //之路单据类型的字段

                    for (int i = 1; i <= 4; i++)
                    {
                        flow["ext_col_" + i] = sm["EXT_COL_" + i];
                        flow["ext_col_text_" + i] = sm["EXT_COL_TEXT_" + i];
                    }


                    flowCodes.Add(flow);
                }
            }


            if (flowCodes.Count == 0)
            {
                throw new Exception($"流程定义不存在: {curFlowCode}");
            }

            return flowCodes;

        }


        private HttpResult GetButtons()
        {
            int table_id = WebUtil.FormInt("table_id");

            int row_id = WebUtil.FormInt("row_id");



            LModel model = GetTable(table_id);


            string tableName = (string)model["TABLE_NAME"];
            

            if (model.Get<bool>("FLOW_ENABLED") == false)
            {
                return HttpResult.Success();
            }

            string flowParamsJson = (string)model["FLOW_PARAMS"];

            if (StringUtil.IsBlank(flowParamsJson))
            {
                log.Error("流程参数为空!");
                return HttpResult.Error("流程参数为空");
            }


            SModelList smList = ParseJson(flowParamsJson);
                                    

            LModel row = GetRow(tableName, row_id);



            int curBizSID = (int)row["BIZ_SID"];
            
            int curFlowSID = (int)row["BIZ_FLOW_SID"];

            string curFlowCode ;


            List<SModel> flowCodeList;



            SModel exParam = new SModel(); //存储扩展参数

            flowCodeList = GetDefineCode(smList, curBizSID,exParam);

            SModelList btns = null;

            HttpResult pack;

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (curFlowSID > 0)
            {
                curFlowCode = (string)row["BIZ_FLOW_DEF_CODE"];

                btns = CreateBtnsForOne(decipher,curFlowCode, curFlowSID, exParam, row, tableName, row_id);
            }
            else
            {
                try
                {

                    if (flowCodeList.Count == 1)
                    {
                        curFlowCode = (string)flowCodeList[0]["code"];
                        btns = CreateBtnsForOne(decipher,curFlowCode, curFlowSID, flowCodeList[0], row, tableName, row_id);
                    }
                    else
                    {
                        btns = CreateBtnsForMore(decipher,flowCodeList, row, tableName, row_id);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("创建按钮失败", ex);


                    string bfInstCode = (string)row["BIZ_FLOW_INST_CODE"];

                    if (!StringUtil.IsBlank(bfInstCode))
                    {
                        //创建流程按钮集合
                        SModelList btns2 = new SModelList();


                        SModel viewBtnX = new SModel();

                        viewBtnX["role"] = "view-flow";
                        viewBtnX["inst_code"] = bfInstCode;
                        viewBtnX["text"] = "查看流程";

                        btns2.Add(viewBtnX);
                        
                        pack = HttpResult.Success(btns2);
                    }
                    else
                    {
                        pack = HttpResult.Success();
                    }

                    return pack;
                }
            }


            return HttpResult.Success(btns); ;
        }




        /// <summary>
        /// 创建多个流程的按钮
        /// </summary>
        /// <param name="curFlowCode"></param>
        /// <param name="curFlowSID"></param>
        /// <param name="exParam"></param>
        /// <param name="row"></param>
        /// <param name="tableName"></param>
        /// <param name="row_id"></param>
        /// <returns></returns>
        private SModelList CreateBtnsForMore(DbDecipher decipher ,List<SModel> flowCodeList, LModel row, string tableName, int row_id)
        {
            //创建流程按钮集合
            SModelList btns = new SModelList();


            SModel viewBtn = new SModel();

            viewBtn["role"] = "view-flow";

            viewBtn["text"] = "查看流程";

            viewBtn["flow_list"] = GetFlowList(tableName, row_id);

            btns.Add(viewBtn);


            SModel selectFlowBtn = new SModel();

            SModelList flow_items = new SModelList();

            selectFlowBtn["role"] = "select-flow";

            selectFlowBtn["text"] = $"提交({flowCodeList.Count})";

            selectFlowBtn["flow_items"] = flow_items;


            foreach (var item in flowCodeList)
            {
                string flowCode = (string)item["code"];

                FLOW_DEF curFlow = null;
                FLOW_DEF_NODE curNode = null;
                List<FLOW_DEF_LINE> curLines = null;

                GetStartParam(decipher, flowCode, out curFlow, out curNode, out curLines);

                item["text"] = curFlow.DEF_TEXT;

                item["cur_flow_code"] = curNode.DEF_CODE;
                //item["cur_flow_text"] = curFlow.DEF_TEXT;

                item["cur_node_code"] = curNode.NODE_CODE;

                item["cur_line_code"] = curLines[0].LINE_CODE;



                flow_items.Add(item);
            }




            btns.Add(selectFlowBtn);



            return btns;
        }


        private void GetStartParam(DbDecipher decipher, string flowCode, out FLOW_DEF curFlow, out FLOW_DEF_NODE curNode, out List<FLOW_DEF_LINE> curLines)
        {
            FLOW_DEF def = FlowMgr.GetDefine(flowCode);


            if (def == null)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" .");
            }

            //开始节点 
            FLOW_DEF_NODE start = FlowMgr.GetNodeForType(flowCode, "start");

            if(start == null)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" 的开始开始节点.");
            }

            List<FLOW_DEF_LINE> startLine = FlowMgr.GetLines(flowCode, start.NODE_CODE);

            if (startLine == null || startLine.Count == 0)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" 的开始开始节点的下一步连接线.");
            }

            FLOW_DEF_LINE defLine = startLine[0];

            //找到草稿的节点
            FLOW_DEF_NODE node = FlowMgr.GetNode(decipher, flowCode, defLine.TO_NODE_CODE);


            if (node == null )
            {
                throw new Exception($"找不到流程 \"{flowCode}\" 的节点 \"{defLine.TO_NODE_CODE}\".");
            }

            List<FLOW_DEF_LINE> lines = FlowMgr.GetLines(flowCode, node.NODE_CODE);

            curFlow = def;
            curNode = node;
            curLines = lines;
        }


        /// <summary>
        /// 创建单个流程的按钮
        /// </summary>
        /// <param name="curFlowCode"></param>
        /// <param name="curFlowSID"></param>
        /// <param name="exParam"></param>
        /// <param name="row"></param>
        /// <param name="tableName"></param>
        /// <param name="row_id"></param>
        /// <returns></returns>
        private SModelList CreateBtnsForOne(DbDecipher decipher, string curFlowCode, int curFlowSID, SModel exParam,  LModel row, string tableName, int row_id)
        {
            FLOW_DEF def;

            FLOW_DEF_NODE curNode = null;
            List<FLOW_DEF_LINE> curLines = null;

            string inst_code = null;

            if (curFlowSID == 0)
            {
                GetStartParam(decipher, curFlowCode, out def, out curNode, out curLines);
            }
            else if (curFlowSID == 2)
            {
                string bizFlowInstCode = (string)row["BIZ_FLOW_INST_CODE"];

                FLOW_INST inst = FlowInstMgr.GetInst(decipher, bizFlowInstCode);

                FLOW_DEF_NODE node = FlowMgr.GetNode(decipher, curFlowCode, inst.CUR_NODE_CODE);

                List<FLOW_DEF_LINE> lines = FlowMgr.GetLines(curFlowCode, node.NODE_CODE);

                inst_code = inst.INST_CODE;

                curNode = node;
                curLines = lines;
            }
            else
            {
                inst_code = (string)row["BIZ_FLOW_INST_CODE"];
            }


            //创建流程按钮集合
            SModelList btns = new SModelList();


            SModel viewBtn = new SModel();

            viewBtn["role"] = "view-flow";
            viewBtn["inst_code"] = inst_code;
            viewBtn["text"] = "查看流程";

            viewBtn["flow_list"] = GetFlowList(tableName, row_id);

            btns.Add(viewBtn);


            if (curLines != null && curNode != null)
            {
                foreach (var defline in curLines)
                {
                    SModel btn = new SModel();
                    btn["role"] = "submit";
                    btn["id"] = defline.FLOW_DEF_LINE_ID;
                    btn["code"] = defline.LINE_CODE;
                    btn["text"] = defline.LINE_TEXT;

                    btn["bill_type"] = exParam["bill_type"];
                    btn["bill_code_field"] = exParam["bill_code_field"];

                    btn["cur_flow_code"] = curNode.DEF_CODE;

                    btn["cur_node_code"] = curNode.NODE_CODE;

                    btn["ext_col_1"] = exParam["ext_col_1"];
                    btn["ext_col_2"] = exParam["ext_col_2"];
                    btn["ext_col_3"] = exParam["ext_col_3"];
                    btn["ext_col_4"] = exParam["ext_col_4"];


                    btns.Add(btn);
                }

                //节点按钮开关

                if (curFlowSID == 2)
                {
                    if (curNode.BTN_BACKFIRST_VISIBLE)
                    {
                        SModel btn = GetBtn_BackFirst(curNode, exParam);
                        btns.Add(btn);
                    }

                    if (curNode.BTN_BACK_VISIBLE)
                    {
                        SModel btn = GetBtn_Back(curNode, exParam);
                        btns.Add(btn);
                    }
                }
            }

            return btns;
        }


        /// <summary>
        /// 获取流程的步骤集
        /// </summary>
        /// <param name="instCode"></param>
        /// <returns></returns>
        public List<FLOW_INST_STEP> GetFlowSteps(string instCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_STEP));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("DEF_NODE_TYPE", "auto_node", HWQ.Entity.Filter.Logic.Inequality);
            filter.TSqlOrderBy = "SEQ_NUM ASC";

            List<FLOW_INST_STEP> models = decipher.SelectModels<FLOW_INST_STEP>(filter);

            return models;
        }

        /// <summary>
        /// 获取流程集合
        /// </summary>
        /// <returns></returns>
        private SModelList GetFlowList(string table,int rowId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("EXTEND_TABLE", table);
            filter.And("EXTEND_ROW_ID", rowId);
            filter.TSqlOrderBy = "DATE_START DESC";

            List<FLOW_INST> instList = decipher.SelectModels<FLOW_INST>(filter);


            if(instList.Count == 0)
            {
                return null;
            }


            SModelList flowList = new SModelList();

            foreach (var inst in instList)
            {
                SModel flow = new SModel();

                flowList.Add(flow);

                flow["def_code"] = inst.DEF_CODE;   //流程编码
                flow["date_start"] = inst.DATE_START;
                flow["flow_sid"] = inst.FLOW_SID;
                flow["inst_code"] = inst.INST_CODE;

                if(inst.FLOW_SID == 2)
                {
                    flow["state"] = "进行中...";
                }
                else if(inst.FLOW_SID == 1)
                {
                    flow["state"] = "作废";
                }
                else if(inst.FLOW_SID == 4)
                {
                    flow["state"] = "结束";
                }


                SModelList stepItems = new SModelList();

                flow["step_items"] = stepItems;

                //步骤
                var steps = GetFlowSteps(inst.INST_CODE);

                foreach (var step in steps)
                {
                    SModel stepItem = new SModel();


                    stepItem["step_sid"] = step.STEP_SID;
                    
                    if(step.DEF_NODE_TYPE == "start")
                    {
                        stepItem["text"] = "流程开始";
                    }
                    else if(step.DEF_NODE_TYPE == "end")
                    {
                        stepItem["text"] = "流程结束";
                    }
                    else
                    {
                        stepItem["text"] = step.DEF_NODE_TEXT;
                    }

                    stepItem["date_end"] = step.DATE_END;
                    stepItem["comments"] = step.OP_CHECK_COMMENTS;
                    stepItem["is_revoked"] = step.IS_REVOKED;
                    stepItem["is_back_operate"] = step.IS_BACK_OPERATE;

                    if (step.STEP_SID == 2)
                    {
                        stepItem["state"] = "审核中";
                    }
                    else if(step.STEP_SID == 4)
                    {
                        if (step.IS_BACK_OPERATE)
                        {
                            stepItem["state"] = "退回";
                        }
                        else
                        {
                            stepItem["state"] = "通过";
                        }
                    }

                    stepItems.Add(stepItem);
                }

            }



            return flowList;
        }       


        private SModel GetBtn_BackFirst(FLOW_DEF_NODE curNode, SModel exParam)
        {
            SModel btn = new SModel();
            btn["role"] = "submit";

            btn["action_type"] = "back";
            btn["id"] = 0;
            btn["code"] = "back_first";
            btn["text"] = "退回首环节";

            btn["bill_type"] = exParam["bill_type"];
            btn["bill_code_field"] = exParam["bill_code_field"];

            btn["cur_flow_code"] = curNode.DEF_CODE;

            btn["cur_node_code"] = curNode.NODE_CODE;

            return btn;
        }

        private SModel GetBtn_Back(FLOW_DEF_NODE curNode, SModel exParam)
        {
            SModel btn = new SModel();
            btn["role"] = "submit";

            btn["action_type"] = "back";
            btn["id"] = 0;
            btn["code"] = "back";
            btn["text"] = "退回上一环节";

            btn["bill_type"] = exParam["bill_type"];
            btn["bill_code_field"] = exParam["bill_code_field"];

            btn["cur_flow_code"] = curNode.DEF_CODE;

            btn["cur_node_code"] = curNode.NODE_CODE;

            return btn;
        }


    }
}