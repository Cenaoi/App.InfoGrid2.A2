using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.FlowModels;
using EC5.AppDomainPlugin;
using EC5.SystemBoard;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View
{
    /// <summary>
    /// 流程操作
    /// </summary>
    public class FlowOperateMgr
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 操作类型
        /// </summary>
        enum OptionType
        {
            /// <summary>
            /// 提交, 往前走
            /// </summary>
            Next,

            /// <summary>
            /// 后退, 往后走
            /// </summary>
            Back
        }


        /// <summary>
        /// 操作名称.
        /// submit=提交; back=退回
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// 当前页面的id,例如复杂表id,视图表id....
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// 表id
        /// </summary>
        public int TableId { get; set; }

        public string TableName { get; set; }

        /// <summary>
        /// 当前主表记录id
        /// </summary>
        public int RowId { get; set; }

        public int MenuId { get; set; }

        /// <summary>
        /// 文档字符串, 作为超链接用.
        /// </summary>
        public string DocText { get; set; }

        public string DocUrl { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BillType { get; set; }

        public string BillCode { get; set; }

        /// <summary>
        /// 业务编码的字段名
        /// </summary>
        public string BillCodeField { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public SModel ExtFields { get; set; } = new SModel();

        /// <summary>
        /// 当前流程编码
        /// </summary>
        public string CurFlowCode { get;set;}

        /// <summary>
        /// 当前节点编码
        /// </summary>
        public string CurNodeCode { get; set; }

        /// <summary>
        /// 流程线段代码;
        /// 这两个属性,配合 ActionType 使用. back=退回上一节点;back_first=退回到最前面
        /// </summary>
        public string LineCode { get; set; }

        /// <summary>
        /// 提交或退回的意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public EcUserState User { get; set; }

        /// <summary>
        /// 提交给某个用户集合
        /// </summary>
        public List<string> SubmitPartys { get; set; }

        /// <summary>
        /// 抄送的人员
        /// </summary>
        public List<string> CopyToPartys { get; set; }

        /// <summary>
        /// 0-普通, 10-加急
        /// </summary>
        public int MaxTagID { get; set; }


        /// <summary>
        /// 错误信息
        /// </summary>
        public Exception Error { get; private set; }



        /// <summary>
        /// 根据表 ID,获取表名
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public string GetTableDefine(int tableId)
        {

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("IG2_TABLE_ID", tableId);
            filter.AddFilter("ROW_SID >= 0");
            filter.Fields = new string[] { "TABLE_NAME" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            string tableName = decipher.ExecuteScalar<string>(filter);

            return tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="def"></param>
        /// <param name="exp"></param>
        /// <param name="curFlowCode"></param>
        /// <param name="curNodeCode"></param>
        /// <param name="lineCode"></param>
        /// <param name="curPartyCode"></param>
        /// <param name="comment"></param>
        /// <param name="partyCodes"></param>
        private void ProStep_FlowSID_0(DbDecipher decipher, OptionType opType, FLOW_DEF def, FlowExParam exp,
            string curFlowCode, string curNodeCode, string lineCode, string curPartyCode,
            string comment,
            List<string> partyCodes)
        {
            

            //开始节点 
            FLOW_DEF_NODE startNode = FlowMgr.GetNodeForType(curFlowCode, "start");

            List<FLOW_DEF_LINE> startLine = FlowMgr.GetLines(curFlowCode, startNode.NODE_CODE);

            FLOW_DEF_LINE defLine = startLine[0];



            //找到草稿的节点
            FLOW_DEF_NODE curNode = FlowMgr.GetNode(decipher, curFlowCode, curNodeCode);



            FLOW_DEF_LINE line = FlowMgr.GetLine(curFlowCode, lineCode);

            FLOW_DEF_NODE fromNode = FlowMgr.GetNode(decipher,curFlowCode, line.FROM_NODE_CODE);
            FLOW_DEF_NODE toNode = FlowMgr.GetNode(decipher, curFlowCode, line.TO_NODE_CODE);

            if (toNode.NODE_TYPE == "node" && partyCodes.Count == 0)
            {
                throw new Exception("必须指定一个接收人.");
            }

            List<FLOW_DEF_NODE_PARTY> toPartys = FlowMgr.GetPartys(decipher, curFlowCode, toNode.NODE_CODE, partyCodes);





            //创建一个流程实例
            FLOW_INST inst = FlowInstMgr.CreateInst( def, exp);

            inst.FLOW_SID = 2;

            inst.START_USER_CODE = this.User.ExpandPropertys["USER_CODE"];
            inst.START_USER_TEXT = this.User.LoginName;

            inst.CUR_NODE_ID = toNode.FLOW_DEF_NODE_ID;
            inst.CUR_NODE_CODE = toNode.NODE_CODE;
            inst.CUR_NODE_TEXT = toNode.NODE_TEXT;

            decipher.SetIdentity(inst);


            //FLOW_INST_STEP stepStart = FlowMgr.CreateInstStep(def,Stack )


            //开始
            FLOW_INST_STEP stepStart = FlowInstMgr.CreateInstStep(decipher, def, null, null, null, startNode, inst);
            stepStart.STEP_SID = 4;
            stepStart.DATE_END = DateTime.Now;

            inst.N_STEP_IDENTITY++;
            decipher.SetIdentity(stepStart);


            //起草
            FLOW_INST_STEP step0 = FlowInstMgr.CreateInstStep(decipher, def, startNode, defLine, stepStart, curNode, inst);
            step0.STEP_SID = 4;
            step0.DATE_END = DateTime.Now;

            inst.N_STEP_IDENTITY++;
            decipher.SetIdentity(step0);


            FLOW_INST_PARTY curParty = FlowInstMgr.CreateInstParty( inst, curNode, this.User.ExpandPropertys["USER_CODE"], this.User.LoginName);
            curParty.INST_STEP_CODE = step0.INST_STEP_CODE;
            curParty.BIZ_SID = 2;
            curParty.COMMENT = comment;
            curParty.PARTY_TYPE = "USER";
            curParty.DATE_SUBMIT = DateTime.Now;
            curParty.SUBMIT_TAG_SID = exp.TagSID;

            decipher.SetIdentity(curParty);

            step0.OP_CHECK_DESC = this.User.LoginName;  //审核人...
            step0.MAX_TAG_SID = curParty.SUBMIT_TAG_SID;


            LModelElement modelElem = LightModel.GetLModelElement(exp.TableName);

            SModel sRow = new SModel();
            sRow["BIZ_FLOW_SID"] = 2;
            sRow["BIZ_FLOW_INST_CODE"] = inst.INST_CODE;
            sRow["BIZ_FLOW_STEP_CODE"] = step0.INST_STEP_CODE;
            sRow["BIZ_FLOW_DEF_CODE"] = inst.DEF_CODE;
            sRow["BIZ_FLOW_CUR_NODE_CODE"] = step0.DEF_NODE_CODE;
            sRow["BIZ_FLOW_CUR_NODE_TEXT"] = step0.DEF_NODE_TEXT;



            Exception exception = null;

            decipher.BeginTransaction();

            try
            {
                decipher.IdentityStop();

                decipher.InsertModel(inst);         //流程实例

                decipher.InsertModel(stepStart);    //流程步骤-开始

                decipher.InsertModel(step0);        //流程步骤-草稿

                decipher.InsertModel(curParty);     //流程步骤的当事人

                
                decipher.UpdateSModel(sRow, exp.TableName, $"{modelElem.PrimaryKey}={exp.RowId}");  //更新用户表的流程状态信息

                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                exception = new Exception("提交流程事务失败.", ex);
            }
            finally
            {
                decipher.IdentityRecover();
            }

            if (exception != null) throw exception;




            if (toNode.NODE_TYPE == "node")
            {
                ProNode(decipher, opType, def, exp, curNode, line, step0, toNode, inst, partyCodes);
            }
            else if (toNode.NODE_TYPE == "auto_node")
            {
                LModel row = DataMgr.GetModel(decipher, exp.TableName, exp.RowId);

                ProAutoNode(decipher, opType, def, exp, curNode, line, step0, toNode, inst, row, partyCodes);
            }
            else if (toNode.NODE_TYPE == "end")
            {
                LModel row = DataMgr.GetModel(decipher, exp.TableName, exp.RowId);

                EndStep(decipher, def, exp, curNode, line, step0, toNode, inst, row);
            }
        }



        /// <summary>
        /// 处理用户提交作业过程
        /// </summary>
        /// <param name="instCode"></param>
        /// <param name="inst"></param>
        /// <param name="step"></param>
        /// <param name="partyCode"></param>
        /// <param name="comment"></param>
        private void ProSubmit_StepParty(DbDecipher decipher, string instCode, FlowExParam exp, FLOW_INST inst, FLOW_INST_STEP step, string partyCode, string comment)
        {

            var instParty = FlowInstMgr.GetInstParty(decipher, instCode, inst.CUR_INST_STEP_CODE, partyCode);

            if (instParty == null)
            {
                throw new Exception("您无权限进行操作.");
            }

            if (instParty.BIZ_SID == 2)
            {
                throw new Exception("您已经提交过, 无法再提交.");
            }

            instParty.BIZ_SID = 2;
            instParty.COMMENT = comment;
            instParty.DATE_SUBMIT = DateTime.Now;
            instParty.ROW_DATE_UPDATE = DateTime.Now;

            instParty.SUBMIT_TAG_SID = exp.TagSID;


            decipher.BeginTransaction();

            try
            {
                decipher.UpdateModelProps(instParty, "BIZ_SID", "DATE_SUBMIT", "SUBMIT_TAG_SID", "COMMENT", "ROW_DATE_UPDATE");


                step.MAX_TAG_SID = Math.Max(step.MAX_TAG_SID, exp.TagSID);

                //更改步骤信息
                step.P_CUR_COUNT = FlowInstMgr.GetCheckPartyCount(decipher, instCode, inst.CUR_INST_STEP_CODE);
                step.P_SURPLUS_COUNT = step.P_MEET_COUNT - step.P_CUR_COUNT;

                if (step.P_SURPLUS_COUNT < 0)
                {
                    step.P_SURPLUS_COUNT = 0;
                }

                step.ROW_DATE_UPDATE = DateTime.Now;

                step.OP_CHECK_DESC = FlowInstMgr.GetCheckPartyDesc(decipher, instCode, inst.CUR_INST_STEP_CODE);    //审核人...
                step.OP_CHECK_COMMENTS = FlowInstMgr.GetCheckPartyComments(decipher, instCode, inst.CUR_INST_STEP_CODE);  //审核人的审核意见

                decipher.UpdateModelProps(step, "MAX_TAG_SID", "OP_CHECK_DESC", "OP_CHECK_COMMENTS", "P_CUR_COUNT", "P_SURPLUS_COUNT", "ROW_DATE_UPDATE");

                decipher.TransactionCommit();


            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("流程步骤提交失败.", ex);
            }


        }

        private void ProStep_Back(DbDecipher decipher, FLOW_DEF def, FlowExParam exp,
            string curFlowCode, string curNodeCode, string lineCode, string curPartyCode,
            string comment,
            List<string> partyCodes, LModel row)
        {

            string instCode = (string)row["BIZ_FLOW_INST_CODE"];

            var inst = FlowInstMgr.GetInst(decipher, instCode, curFlowCode);                  //当前流程实例

            string action_type = this.ActionType;// WebUtil.QueryLower("action_type", "submit");

            //string lineCode = WebUtil.Query("line_code");


            var curStep = FlowInstMgr.GetInstStep(decipher, instCode, inst.CUR_INST_STEP_CODE);  //当前流程的步骤


            int partyCount = FlowInstMgr.GetInstPartyCount(decipher, instCode, inst.CUR_INST_STEP_CODE, curPartyCode);

            if (partyCount == 0)
            {
                throw new Exception("您无权限进行操作, 或已经提交.");
            }

            //查找整个回滚链                                                      //查找整个回滚链
            List<FLOW_INST_STEP> stepList = new List<FLOW_INST_STEP>();

            if (lineCode == "back")
            {

                stepList = new List<FLOW_INST_STEP>();

                stepList.Add(curStep);

                string prevStepCode = curStep.PREV_STEP_CODE;

                //防止死循环,定了个 999.

                for (int i = 0; i < 999; i++)
                {
                    if (StringUtil.IsBlank(prevStepCode))
                    {
                        break;
                    }

                    //上一个步骤也要作废
                    var prevStep = FlowInstMgr.GetInstStep(decipher, instCode, prevStepCode);

                    var nodeType = prevStep.DEF_NODE_TYPE;

                    stepList.Add(prevStep);

                    if (nodeType == "start")
                    {
                        break;
                    }
                    else if (nodeType == "node")
                    {
                        if (prevStep.FROM_NODE_TYPE == "start")
                        {
                            //应该是第一个节点, 作为特殊处理.
                        }
                        else
                        {
                            break;
                        }
                    }

                    prevStepCode = prevStep.PREV_STEP_CODE;
                }


            }
            else if (lineCode == "back_first")
            {
                stepList = FlowInstMgr.GetFlowSteps(decipher, curStep.INST_CODE);

                stepList.Reverse();

            }

            //处理当前这个用户自己提交
            ProSubmit_StepParty(decipher, instCode, exp, inst, curStep, curPartyCode, comment);

            var opBatchCode = BillIdentityMgr.NewCodeForDay("FLOW_REVOKED", "FR-", "-");


            for (int i = 0; i < stepList.Count; i++)
            {
                FLOW_INST_STEP step = stepList[i];

                step.SetTakeChange(true);

                if (i == 0)
                {
                    step.STEP_SID = 4;
                    step.IS_BACK_OPERATE = true;
                    step.DATE_END = DateTime.Now;
                }


                if (step.DEF_NODE_TYPE == "auto_node")
                {
                    FLOW_DEF_NODE node = FlowMgr.GetNode(decipher, curFlowCode, step.DEF_NODE_CODE);

                    if (node.BACK_ACTION_TYPE == "SCRIPT-CS" && !StringUtil.IsBlank(node.BACK_ACTION_SCRIPT))
                    {
                        object result = ProScript(node.BACK_ACTION_SCRIPT, row);

                        step.BACK_ACTION_RESULT = result?.ToString();

                        EC5.IG2.BizBase.DbCascadeRule.Update(row);
                    }
                }



                step.IS_REVOKED = true;
                step.REVOKED_OP_BATCH_CODE = opBatchCode;
                step.REVOKED_DATE = DateTime.Now;
                step.REVOKED_PARTY_CODE = curPartyCode;

                decipher.UpdateModel(step, true);
            }


            #region 作废掉没有参与会签的人员

            List<string> stepCodeList = new List<string>();

            foreach (var item in stepList)
            {
                stepCodeList.Add(item.INST_STEP_CODE);
            }
            

            LightModelFilter filterP = new LightModelFilter(typeof(FLOW_INST_PARTY));

            filterP.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filterP.And("INST_CODE", curStep.INST_CODE);
            filterP.And("BIZ_SID", 0);  // 专门查找这个没有提交的人,进行作废

            //filterP.And("INST_STEP_CODE", curStep.INST_STEP_CODE);
            filterP.And("INST_STEP_CODE", stepCodeList, Logic.In);


            LModelList<FLOW_INST_PARTY> noPartyList = decipher.SelectModels<FLOW_INST_PARTY>(filterP);

            if (noPartyList != null && noPartyList.Count > 0)
            {
                foreach (var noParty in noPartyList)
                {
                    noParty.BIZ_SID = -1;
                    noParty.COMMENT = "(系统作废掉的记录)";
                    decipher.UpdateModelProps(noParty, "BIZ_SID", "COMMENT");
                }
            }

            #endregion

            int lastIndex = stepList.Count - 1;
            FLOW_INST_STEP firstStep = stepList[lastIndex];


            //重新创建一个节点,替换掉

            var fromNode = FlowMgr.GetNode(decipher, curFlowCode, firstStep.FROM_NODE_CODE);        //当前流程的节点
            var fromLine = FlowMgr.GetLine(curFlowCode, firstStep.FROM_LINE_CODE);

            var fromStep = FlowInstMgr.GetInstStep(decipher, curStep.INST_CODE, firstStep.PREV_STEP_CODE);

            var toNode = FlowMgr.GetNode(decipher, curFlowCode, firstStep.DEF_NODE_CODE);

            if (toNode.NODE_TYPE == "start")
            {
                object pkValue = row["ROW_IDENTITY_ID"];

                SModel model = new SModel();

                model["BIZ_FLOW_SID"] = 0;
                model["BIZ_FLOW_INST_CODE"] = string.Empty;
                model["BIZ_FLOW_DEF_CODE"] = string.Empty;
                model["BIZ_FLOW_CUR_NODE_CODE"] = string.Empty;
                model["BIZ_FLOW_CUR_NODE_TEXT"] = string.Empty;
                model["BIZ_FLOW_STEP_CODE"] = string.Empty;

                decipher.UpdateSModel(model, inst.EXTEND_TABLE, $"ROW_IDENTITY_ID = {pkValue}");

                inst.FLOW_SID = 1;  //作废
                decipher.UpdateModelProps(inst, "FLOW_SID");
            }
            else
            {
                ProNode(decipher, OptionType.Back, def, exp, fromNode, fromLine, fromStep, toNode, inst, partyCodes);
            }
        }

        private void ProStep_FlowSID_2(DbDecipher decipher, OptionType opType, FLOW_DEF def, FlowExParam exp,
            string curFlowCode, string curNodeCode, string lineCode, string curPartyCode,
            string comment,
            List<string> partyCodes, LModel row)
        {
            

            string instCode = (string)row["BIZ_FLOW_INST_CODE"];

            var inst = FlowInstMgr.GetInst(decipher, instCode, curFlowCode);                  //当前流程实例
            var node = FlowMgr.GetNode(decipher, curFlowCode, inst.CUR_NODE_CODE);        //当前流程的节点


            //开始进入下一个流程

            var line = FlowMgr.GetLine(decipher,curFlowCode, lineCode);
            var toNode = FlowMgr.GetNode(decipher, curFlowCode, line.TO_NODE_CODE);

            if (toNode.NODE_TYPE == "node" && (partyCodes == null || partyCodes.Count == 0))
            {
                throw new Exception("必须指定一个接收人.");
            }


            var step = FlowInstMgr.GetInstStep(decipher, instCode, inst.CUR_INST_STEP_CODE);  //当前流程的步骤

            //处理当前这个用户自己提交
            ProSubmit_StepParty(decipher,instCode, exp, inst, step, curPartyCode, comment);

            //如果需要会签, 检查下
            if (step.P_IS_CONSIGN)
            {
                if (step.P_SURPLUS_COUNT > 0)
                {
                    return;
                }

                //parallel=并行会签;only=唯一会签
                if (step.P_CONSIGN_TYPE == "parallel")
                {

                }
            }
            else
            {
                if (step.P_SURPLUS_COUNT != 0)
                {
                    return;
                }
            }


            #region 作废掉没有参与会签的人员

            LightModelFilter filterP = new LightModelFilter(typeof(FLOW_INST_PARTY));

            filterP.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filterP.And("INST_CODE", step.INST_CODE);
            filterP.And("INST_STEP_CODE", step.INST_STEP_CODE);
            filterP.And("BIZ_SID", 0);  // 专门查找这个没有提交的人,进行作废
           

            LModelList<FLOW_INST_PARTY> noPartyList = decipher.SelectModels<FLOW_INST_PARTY>(filterP);

            if (noPartyList != null && noPartyList.Count > 0)
            {
                foreach (var noParty in noPartyList)
                {
                    noParty.BIZ_SID = -1;
                    noParty.COMMENT = "(系统作废掉的记录)";
                    decipher.UpdateModelProps(noParty, "BIZ_SID", "COMMENT");
                }
            }

            #endregion


            /****  当前流程已经结束, 准备进入下一个流程节点   ****/

            //当前流程结束
            step.STEP_SID = 4;
            step.ROW_DATE_UPDATE = DateTime.Now;
            step.DATE_END = DateTime.Now;

            List<FLOW_INST_COPY> icList = null;

            if (step.IS_COPY_ENABLED)
            {
                icList = FlowInstMgr.CreateInstCopyPartys(decipher, step.DEF_CODE, step.DEF_NODE_CODE, step.INST_CODE, step.INST_STEP_CODE);

            }


            decipher.UpdateModelProps(step, "STEP_SID", "DATE_END", "ROW_DATE_UPDATE");

            if (icList != null)
            {
                decipher.InsertModels(icList);


                //创建抄送人的名单
                List<string> copyToPartys = new List<string>();

                foreach (var item in icList)
                {
                    copyToPartys.Add(item.P_USER_CODE);
                }

                this.CopyToPartys = copyToPartys;
            }



            if (toNode.NODE_TYPE == "node")
            {
                ProNode(decipher, opType, def, exp, node, line, step, toNode, inst, partyCodes);
            }
            else if (toNode.NODE_TYPE == "auto_node")
            {
                ProAutoNode(decipher, opType, def, exp, node, line, step, toNode, inst, row, partyCodes);
            }
            else if (toNode.NODE_TYPE == "end")
            {
                EndStep(decipher, def, exp, node, line, step, toNode, inst, row);
            }
        }


        private void ProNode(DbDecipher decipher, OptionType opType, FLOW_DEF def, FlowExParam exp,
            FLOW_DEF_NODE fromNode, FLOW_DEF_LINE fromLine, FLOW_INST_STEP fromStep,
            FLOW_DEF_NODE node, FLOW_INST inst, List<string> partyCodes)
        {


            List<FLOW_DEF_NODE_PARTY> toPartys;


            //下面这个地方需要选................................

            if (partyCodes == null || partyCodes.Count == 0)
            {
                toPartys = FlowMgr.GetPartys(decipher, def.DEF_CODE, node.NODE_CODE);
            }
            else
            {
                toPartys = FlowMgr.GetPartys(decipher, def.DEF_CODE, node.NODE_CODE, partyCodes);
            }

            if (toPartys == null || toPartys.Count == 0)
            {
                toPartys = FlowMgr.GetPartys(decipher, def.DEF_CODE, node.NODE_CODE);
            }

            if(toPartys == null || toPartys.Count == 0)
            {
                throw new Exception("没有目标用户,无法提交到流程.");
            }

            FLOW_INST_STEP step = FlowInstMgr.CreateInstStep(decipher, def, fromNode, fromLine, fromStep, node, inst);

            decipher.SetIdentity(step);


            step.STEP_SID = 2;

            if (step.P_IS_CONSIGN)
            {
                if (step.P_CONSIGN_MIN_COUNT > 0)
                {
                    step.P_SURPLUS_COUNT = step.P_CONSIGN_MIN_COUNT;
                    step.P_MEET_COUNT = step.P_CONSIGN_MIN_COUNT;
                }
                else
                {
                    step.P_SURPLUS_COUNT = toPartys.Count;
                    step.P_MEET_COUNT = toPartys.Count;
                }
            }
            else
            {
                step.P_SURPLUS_COUNT = 1;
                step.P_MEET_COUNT = 1;
            }



            List<FLOW_INST_PARTY> instPartys = new List<FLOW_INST_PARTY>();

            List<string> partyCodeList = new List<string>();//接收人的编号

            //起草后的节点
            foreach (var toParty in toPartys)
            {
                FLOW_INST_PARTY iParty = FlowInstMgr.CreateInstParty(inst, node, toParty);
                iParty.INST_STEP_CODE = step.INST_STEP_CODE;

                if (fromStep != null)
                {
                    iParty.TAG_SID = fromStep.MAX_TAG_SID;
                }

                decipher.SetIdentity(iParty);

                instPartys.Add(iParty);

                partyCodeList.Add(iParty.P_USER_CODE);
            }

            this.SubmitPartys = partyCodeList;


            inst.CUR_NODE_ID = node.FLOW_DEF_NODE_ID;
            inst.CUR_NODE_CODE = node.NODE_CODE;
            inst.CUR_NODE_TEXT = node.NODE_TEXT;
            inst.CUR_INST_STEP_CODE = step.INST_STEP_CODE;


            LModelElement modelElem = LightModel.GetLModelElement(exp.TableName);

            SModel sRow = new SModel();
            sRow["BIZ_FLOW_CUR_NODE_CODE"] = step.DEF_NODE_CODE;
            sRow["BIZ_FLOW_CUR_NODE_TEXT"] = step.DEF_NODE_TEXT;
            sRow["BIZ_FLOW_STEP_CODE"] = step.INST_STEP_CODE;



            Exception error = null;

            decipher.BeginTransaction();
            decipher.IdentityStop();

            try
            {

                decipher.InsertModel(step);
                decipher.InsertModels(instPartys);

                decipher.UpdateModelProps(inst, "CUR_NODE_ID", "CUR_NODE_CODE", "CUR_NODE_TEXT", "CUR_INST_STEP_CODE");

                decipher.UpdateSModel(sRow, exp.TableName, $"{modelElem.PrimaryKey}={exp.RowId}");

                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                error = ex;
            }
            finally
            {
                decipher.IdentityRecover();
            }

            if (error != null) throw error;

        }


        /// <summary>
        /// 处理脚本
        /// </summary>
        private object ProScript(string script, LModel row)
        {

            if (!script.Contains("return"))
            {
                script += "\nreturn null;";
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            ScriptInstance inst = ScriptFactory.Create(script);

            inst.Params["log"] = log;
            inst.Params["decipher"] = decipher;
            inst.Params["user"] = this.User;

            row.SetTakeChange(true);

            object result = inst.Exec(row);

            return result;
        }



        private void ProBizCheck(DbDecipher decipher, int srcBizSID, int curBizSID, LModel row)
        {

            if (!row.HasField("BIZ_SID"))
            {
                return;
            }

            
            //int srcBizSID = (int)row.GetOriginalValue("BIZ_SID");

            //int curBizSID = (int)row["BIZ_SID"];

            if (srcBizSID < curBizSID && curBizSID == 4)
            {
                List<string> fields = new List<string>();

                if (row.HasField("BIZ_CHECK_DATE"))
                {
                    row["BIZ_CHECK_DATE"] = DateTime.Now;
                    fields.Add("BIZ_CHECK_DATE");
                }

                if (row.HasField("BIZ_CHECK_USER_CODE"))
                {
                    row["BIZ_CHECK_USER_CODE"] = this.User.ExpandPropertys["USER_CODE"];
                    fields.Add("BIZ_CHECK_USER_CODE");
                }

                if (row.HasField("BIZ_CHECK_USER_TEXT"))
                {
                    row["BIZ_CHECK_USER_TEXT"] = this.User.LoginName;
                    fields.Add("BIZ_CHECK_USER_TEXT");
                }

                if (row.HasField("BIZ_CHECK_ORG_CODE"))
                {
                    row["BIZ_CHECK_ORG_CODE"] = this.User.FirstOrg.OrgID;
                    fields.Add("BIZ_CHECK_ORG_CODE");
                }

                if (fields.Count > 0)
                {
                    string[] fs = fields.ToArray();
                    decipher.UpdateModelProps(row, fs);
                }
            }



        }

        /// <summary>
        /// 处理自动节点
        /// </summary>
        /// <param name="opType">操作类型</param>
        /// <param name="def">流程定义</param>
        /// <param name="exp">流程扩展参数</param>
        /// <param name="fromNode">上一节点</param>
        /// <param name="fromLine">上一线段</param>
        /// <param name="fromStep">上一步骤</param>
        /// <param name="node">当前节点</param>
        /// <param name="inst">流程实例</param>
        /// <param name="row">当前操作的记录</param>
        /// <param name="partyCodes"></param>
        private void ProAutoNode(DbDecipher decipher, OptionType opType, FLOW_DEF def, FlowExParam exp,
            FLOW_DEF_NODE fromNode, FLOW_DEF_LINE fromLine, FLOW_INST_STEP fromStep,
            FLOW_DEF_NODE node, FLOW_INST inst, LModel row, List<string> partyCodes)
        {

            if (opType == OptionType.Next)
            {
                if (node.ACTION_TYPE == "SCRIPT-CS" && !StringUtil.IsBlank(node.ACTION_SCRIPT))
                {
                    string script = node.ACTION_SCRIPT;

                    //DbDecipher decipher = ModelAction.OpenDecipher();

                    FLOW_INST_STEP step = FlowInstMgr.CreateInstStep(decipher,def, fromNode, fromLine, fromStep, node, inst);
                    decipher.SetIdentity(step);

                    if (fromStep != null)
                    {
                        step.MAX_TAG_SID = fromStep.MAX_TAG_SID;
                    }

                    try
                    {
                        int srcBizSID = row.Get<int>("BIZ_SID");

                        row.SetTakeChange(true);

                        object result = ProScript(node.ACTION_SCRIPT, row);

                        int curBizSID = row.Get<int>("BIZ_SID");


                        string resultStr = result?.ToString();


                        List<FLOW_DEF_LINE> toLines = FlowMgr.GetLines(def.DEF_CODE, node.NODE_CODE);

                        step.ACTION_RESULT = resultStr;
                        step.DATE_END = DateTime.Now;
                        step.STEP_SID = 4;


                        //抄送列表
                        List<FLOW_INST_COPY> copyList = null;

                        if (step.IS_COPY_ENABLED)
                        {
                            copyList = FlowInstMgr.CreateInstCopyPartys(decipher, def.DEF_CODE, node.NODE_CODE, inst.INST_CODE, step.INST_STEP_CODE);


                        }


                        Exception error = null;

                        decipher.BeginTransaction();

                        try
                        {
                            ProBizCheck(decipher, srcBizSID, curBizSID, row);


                            decipher.IdentityStop();

                            decipher.InsertModel(step);

                            //decipher.UpdateModelProps(step, "ACTION_RESULT", "DATE_END", "STEP_SID");

                            //decipher.UpdateModel(row, true);



                            try
                            {
                                EC5.IG2.BizBase.DbCascadeRule.Update(decipher,null, row);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("联动触发异常.", ex);
                            }

                            /*** 注一定要在联动的后面做判断 ***/
                            if (StringUtil.IsBlank(inst.EXTEND_BILL_CODE))
                            {
                                string billField = inst.EXTEND_BILL_CODE_FIELD;

                                if (!StringUtil.IsBlank(billField) && row.HasField(billField))
                                {

                                    object billCode = row[billField];

                                    inst.EXTEND_BILL_CODE = billCode?.ToString();

                                    decipher.UpdateModelProps(inst, "EXTEND_BILL_CODE");

                                }
                            }



                            if (copyList != null)
                            {
                                try
                                {
                                    decipher.InsertModels(copyList);
                                }
                                catch (Exception ex3)
                                {
                                    throw new Exception("批量插入抄送人员错误.", ex3);
                                }
                            }

                            decipher.TransactionCommit();

                        }
                        catch (Exception ex)
                        {
                            decipher.TransactionRollback();
                            error = ex;
                        }
                        finally
                        {
                            decipher.IdentityRecover();
                        }

                        if (error != null)
                        {
                            throw error;
                        }


                        AutoNextStep(decipher,opType, def, exp, step, node, inst, row, resultStr, toLines, partyCodes);


                    }
                    catch (Exception ex)
                    {
                        throw new Exception("执行动态代码失败.", ex);
                    }

                }
                else
                {

                }
            }

        }

        private void EndStep(DbDecipher decipher, FLOW_DEF def, FlowExParam exp, FLOW_DEF_NODE fromNode, FLOW_DEF_LINE fromLine, FLOW_INST_STEP fromStep, FLOW_DEF_NODE node, FLOW_INST inst, LModel row)
        {

            //DbDecipher decipher = ModelAction.OpenDecipher();


            FLOW_INST_STEP step = FlowInstMgr.CreateInstStep(decipher, def, fromNode, fromLine, fromStep, node, inst);

            decipher.SetIdentity(step);

            step.DATE_END = DateTime.Now;
            step.STEP_SID = 4;


            inst.FLOW_SID = 4;
            inst.CUR_INST_STEP_CODE = step.INST_STEP_CODE;
            inst.CUR_NODE_ID = node.FLOW_DEF_NODE_ID;
            inst.CUR_NODE_CODE = node.NODE_CODE;
            inst.CUR_NODE_TEXT = node.NODE_TEXT;


            row["BIZ_FLOW_SID"] = 4;
            row["BIZ_FLOW_CUR_NODE_CODE"] = step.DEF_NODE_CODE; //节点编码
            row["BIZ_FLOW_CUR_NODE_TEXT"] = step.DEF_NODE_TEXT; //节点名称
            row["BIZ_FLOW_STEP_CODE"] = step.INST_STEP_CODE;    //步骤代码



            Exception error = null;

            decipher.BeginTransaction();

            try
            {
                decipher.IdentityStop();

                decipher.InsertModel(step);

                decipher.UpdateModelProps(inst, "FLOW_SID", "DATE_END", "CUR_INST_STEP_CODE", "CUR_NODE_ID", "CUR_NODE_CODE", "CUR_NODE_TEXT");

                decipher.UpdateModel(row, true);
                //EC5.IG2.BizBase.DbCascadeRule.Update(row);

                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                error = ex;

                decipher.TransactionRollback();
            }
            finally
            {
                decipher.IdentityRecover();
            }

            if (error != null) { throw error; }

        }


        private void AutoNextStep(DbDecipher decipher, OptionType opType, FLOW_DEF def, FlowExParam exp, FLOW_INST_STEP fromStep, FLOW_DEF_NODE node, FLOW_INST inst, LModel row, string result,
            List<FLOW_DEF_LINE> toLines, List<string> partyCodes)
        {
            FLOW_DEF_LINE curLine = null;


            if (toLines.Count == 1)
            {
                curLine = toLines[0];
            }
            else
            {
                foreach (FLOW_DEF_LINE line in toLines)
                {
                    if (line.LINE_CODE == result)
                    {
                        curLine = line;
                        break;
                    }
                }
            }

            if (curLine == null)
            {
                throw new Exception("返回没有找到对应的环节名.");
            }

            FLOW_DEF_NODE toNode = FlowMgr.GetNode(decipher, def.DEF_CODE, curLine.TO_NODE_CODE);

            if (toNode.NODE_TYPE == "end")
            {
                EndStep(decipher, def, exp, node, curLine, fromStep, toNode, inst, row);
            }
            else if (toNode.NODE_TYPE == "node")
            {
                ProNode(decipher, opType, def, exp, node, curLine, fromStep, toNode, inst, partyCodes);
            }
            else if (toNode.NODE_TYPE == "auto_node")
            {
                ProAutoNode(decipher,opType, def, exp, node, curLine, fromStep, toNode, inst, row, partyCodes);
            }


        }

        /// <summary>
        /// 获取选中的用户编码列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetCheckForPartyCodes()
        {
            return this.SubmitPartys;

            //List<string> partyCodes = new List<string>();

            //foreach (var rect in this.table1.CheckedRows)
            //{
            //    DataField df = rect.Fields["P_USER_CODE"];
            //    partyCodes.Add(df.Value);
            //}

            //return partyCodes;
        }



        private FlowExParam GetFlowExParam()
        {

            FlowExParam exp = new FlowExParam();

            exp.PageId = this.PageId;   // WebUtil.QueryInt("page_id");                   //页面id
            exp.TableId = this.TableId; // WebUtil.QueryInt("table_id");                 //用户数据表名
            exp.RowId = this.RowId;     // WebUtil.QueryInt("row_id");                     //用户数据表的主键值
            exp.MenuId = this.MenuId;   // WebUtil.QueryInt("menu_id");
            exp.DocText = this.DocText; // WebUtil.Query("doc_text");
            exp.DocUrl = this.DocUrl;   // WebUtil.QueryBase64("doc_url");

            exp.BillType = this.BillType;   // WebUtil.Query("bill_type");               //业务类型
            exp.BillCode = this.BillCode;   // WebUtil.Query("bill_code");              //业务编码

            exp.BillCodeField = this.BillCodeField;// WebUtil.Query("bill_code_field");   //业务编码的字段名称

            if (StringUtil.IsBlank(exp.BillType))
            {
                throw new Exception("必须指定'业务类型'.");
            }

            if (StringUtil.IsBlank(exp.BillCodeField))
            {
                throw new Exception("必须指定'业务编码'的字段名.");
            }

            exp.ExtCol1 = this.ExtFields["ext_col_1"];
            exp.ExtCol2 = this.ExtFields["ext_col_2"];
            exp.ExtCol3 = this.ExtFields["ext_col_3"];
            exp.ExtCol4 = this.ExtFields["ext_col_4"];

            exp.ExtColText1 = this.ExtFields["ext_col_text_1"];
            exp.ExtColText2 = this.ExtFields["ext_col_text_2"];
            exp.ExtColText3 = this.ExtFields["ext_col_text_3"];
            exp.ExtColText4 = this.ExtFields["ext_col_text_4"];


            return exp;
        }

        /// <summary>
        /// 进入运行状态
        /// </summary>
        public void GoEnter()
        {
            FlowExParam exp = GetFlowExParam();

            exp.TagSID = this.MaxTagID;// StringUtil.ToInt(this.radioGroup1.Value);



            string curPartyCode = this.User.ExpandPropertys["USER_CODE"];                //当前模拟的用户代码          



            string curFlowCode = this.CurFlowCode;  // WebUtil.Query("cur_flow_code");        //当前流程定义编码
            string curNodeCode = this.CurNodeCode;  // WebUtil.Query("cur_node_code");        //当前节点编码

            string lineCode = this.LineCode;    // this.cmb1.Value;                          //提交的按钮线段编码

            if (!StringUtil.IsBlank(this.TableName))
            {
                exp.TableName = this.TableName;                //用户表名, 例 U_001
            }
            else
            {
                exp.TableName = GetTableDefine(exp.TableId);
            }


            string text = this.Comment;// this.textarea1.Value;                         //意见信息



            LModel row = DataMgr.GetModel(exp.TableName, exp.RowId);


            //DataRecordCollection recds = this.table1.CheckedRows;



            int curBizSID = (int)row["BIZ_SID"];
            int curFlowSID = (int)row["BIZ_FLOW_SID"];


            if (curFlowSID >= 4)
            {
                this.Error = new Exception("流程已经结束,无需操作!");

                return;
            }


            FLOW_DEF def = FlowMgr.GetDefine(curFlowCode);


            List<string> partyCodes = GetCheckForPartyCodes();  // 获取选中的用户代码



            try
            {
                if (curFlowSID == 0)
                {
                    TransactionOptions tOpt = new TransactionOptions();
                    tOpt.IsolationLevel = IsolationLevel.ReadCommitted;
                    tOpt.Timeout = new TimeSpan(0, 2, 0);

                    using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Required, tOpt))
                    {
                        using(DbDecipher decipehr = DbDecipherManager.GetDecipherOpen())
                        {
                            ProStep_FlowSID_0(decipehr, OptionType.Next, def, exp, curFlowCode, curNodeCode, lineCode, curPartyCode, text, partyCodes);

                            tsCope.Complete();
                        }

                    }

                }
                else if (curFlowSID == 2)
                {

                    string action_type = this.ActionType;// WebUtil.QueryLower("action_type", "submit");


                    TransactionOptions tOpt = new TransactionOptions();

                    tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                    tOpt.Timeout = new TimeSpan(0, 2, 0);

                    using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Required, tOpt))
                    {
                        using (DbDecipher decipehr = DbDecipherManager.GetDecipherOpen())
                        {
                            //退回
                            if (action_type == "back")
                            {
                                ProStep_Back(decipehr,def, exp, curFlowCode, curNodeCode, lineCode, curPartyCode, text, partyCodes, row);
                            }
                            else if (action_type == "submit")
                            {
                                ProStep_FlowSID_2(decipehr, OptionType.Next, def, exp, curFlowCode, curNodeCode, lineCode, curPartyCode, text, partyCodes, row);

                            }

                            tsCope.Complete();
                        }
                    }
                }


                //Toast.Show("提交成功!");

                //ScriptManager.Eval("ownerWindow.close({result:'ok'});");
            }
            catch (Exception ex)
            {
                log.Error("提交流程失败", ex);

                this.Error = new Exception("提交流程失败", ex);

            }


        }




    }
}
