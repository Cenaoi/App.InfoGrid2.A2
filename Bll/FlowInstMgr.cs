using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 流程实例管理
    /// </summary>
    public static class FlowInstMgr
    {


        /// <summary>
        /// 填充扩展字段内容
        /// </summary>
        /// <param name="instCode"></param>
        public static void FullExtend(DbDecipher decipher, SModelList instPartys)
        {


            foreach (var item in instPartys)
            {
                FLOW_INST inst = GetInst(decipher,(string)item["INST_CODE"]);

                item["EXTEND_DOC_TEXT"] = inst.EXTEND_DOC_TEXT;
                item["EXTEND_DOC_URL"] = inst.EXTEND_DOC_URL;

            }
        }


        /// <summary>
        /// 是否为这个节点的人员
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="flowNodeCode"></param>
        /// <returns></returns>
        public static bool IsFlowNodeParty(DbDecipher decipher, string userCode, string instCode, string instStepCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("BIZ_SID", 0);
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", instStepCode);
            filter.And("P_USER_CODE", userCode);
            filter.Locks.Add(LockType.NoLock);
            
            bool exist = decipher.ExistsModels(filter);

            return exist;
        }



        /// <summary>
        /// 创建实例
        /// </summary>8am>
        /// <param name="node_code"></param>
        /// <param name="inst_code"></param>
        /// <param name="step_code"></param>
        /// <returns></returns>
        public static List<FLOW_INST_COPY> CreateInstCopyPartys(DbDecipher decipher, string def_code, string node_code, string inst_code, string step_code)
        {

            LModelList<FLOW_DEF_NODE_COPY_PARTY> cpList = FlowMgr.GetCopyPartys(def_code, node_code);

            FLOW_INST inst = FlowInstMgr.GetInst(decipher,inst_code);

            FLOW_DEF_NODE node = FlowMgr.GetNode(decipher, def_code, node_code);

            //抄送给发起人
            if (node.COPY_TO_START_PARTY_ENABLED)
            {


                var group = cpList.ToGroup<string>("P_USER_CODE");

                if (!group.ContainsKey(inst.START_USER_CODE))
                {


                    FLOW_DEF_NODE_COPY_PARTY startParty = new FLOW_DEF_NODE_COPY_PARTY();

                    startParty.DEF_ID = inst.DEF_ID;
                    startParty.DEF_CODE = inst.DEF_CODE;
                    startParty.NODE_ID = inst.CUR_NODE_ID;
                    startParty.NODE_CODE = inst.CUR_NODE_TEXT;
                    startParty.PARTY_TYPE = "USER";

                    startParty.P_USER_CODE = inst.START_USER_CODE;
                    startParty.P_USER_TEXT = inst.START_USER_TEXT;

                    startParty.ROW_SID = 0;
                    startParty.ROW_DATE_CREATE = DateTime.Now;
                    startParty.ROW_DATE_UPDATE = DateTime.Now;
                    startParty.ROW_DATE_DELETE = null;

                    cpList.Add(startParty);
                }
            }

            if (cpList.Count == 0)
            {
                return null;
            }

            List<FLOW_INST_COPY> newCopys = new List<FLOW_INST_COPY>(cpList.Count);

            Queue<int> newIds = decipher.IdentityFactory.GetNewBatchIdentity("FLOW_INST_COPY", "FLOW_INST_COPY_ID", cpList.Count);


            foreach (var cp in cpList)
            {
                FLOW_INST_COPY ic = new FLOW_INST_COPY();

                inst.CopyTo(ic, true);

                cp.CopyTo(ic, true);

                ic.FLOW_INST_COPY_ID = newIds.Dequeue();

                ic.INST_STEP_CODE = step_code;
                ic.DEF_NODE_CODE = node_code;

                ic.ROW_SID = 0;
                ic.ROW_DATE_CREATE = DateTime.Now;
                ic.ROW_DATE_UPDATE = DateTime.Now;
                ic.ROW_DATE_DELETE = null;

                newCopys.Add(ic);
            }


            return newCopys;
        }






        public static FLOW_INST GetInst(DbDecipher decipher,string instCode, string defCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", defCode);
            filter.And("INST_CODE", instCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_INST node = decipher.SelectToOneModel<FLOW_INST>(filter);

            return node;
        }

        public static FLOW_INST GetInst(DbDecipher decipher,string instCode)
        {            
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_INST node = decipher.SelectToOneModel<FLOW_INST>(filter);

            return node;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeCode"></param>
        /// <param name="partyCodes"></param>
        /// <returns></returns>
        public static FLOW_INST_PARTY GetInstParty(DbDecipher decipher, string instCode, string stepCode, string partyCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", stepCode);
            filter.And("P_USER_CODE", partyCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_INST_PARTY node = decipher.SelectToOneModel<FLOW_INST_PARTY>(filter);

            return node;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeCode"></param>
        /// <param name="partyCodes"></param>
        /// <returns></returns>
        public static int GetInstPartyCount(DbDecipher decipher,string instCode, string stepCode, string partyCode)
        {
            
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", stepCode);
            filter.And("P_USER_CODE", partyCode);
            filter.Locks.Add(LockType.NoLock);

            int count = decipher.SelectCount(filter);

            return count;
        }

        /// <summary>
        /// 获取步骤
        /// </summary>
        /// <param name="instCode"></param>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        public static FLOW_INST_STEP GetInstStep(DbDecipher decipher,string instCode, string stepCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_STEP));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", stepCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_INST_STEP node = decipher.SelectToOneModel<FLOW_INST_STEP>(filter);

            return node;
        }

        /// <summary>
        /// 获取流程的步骤集
        /// </summary>
        /// <param name="instCode"></param>
        /// <returns></returns>
        public static List<FLOW_INST_STEP> GetFlowSteps(DbDecipher decipher,string instCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_STEP));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("IS_REVOKED", false);
            filter.TSqlOrderBy = "SEQ_NUM ASC";
            filter.Locks.Add(LockType.NoLock);

            List<FLOW_INST_STEP> models = decipher.SelectModels<FLOW_INST_STEP>(filter);

            return models;
        }



        /// <summary>
        /// 获取已经提交的人数
        /// </summary>
        /// <param name="instCode">实例编码</param>
        /// <param name="stepCode">步骤编码</param>
        /// <returns></returns>
        public static int GetCheckPartyCount(DbDecipher decipher, string instCode, string stepCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", stepCode);
            filter.And("BIZ_SID", 2);

            int count = decipher.SelectCount(filter);

            return count;
        }




        /// <summary>
        /// 获取审核人的描述信息
        /// </summary>
        /// <param name="instCode">实例编码</param>
        /// <param name="stepCode">步骤编码</param>
        /// <returns></returns>
        public static string GetCheckPartyDesc(DbDecipher decipher, string instCode, string stepCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", stepCode);
            filter.And("BIZ_SID", 2);

            List<FLOW_INST_PARTY> ps = decipher.SelectModels<FLOW_INST_PARTY>(filter);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < ps.Count; i++)
            {
                if (i > 0) { sb.Append("; "); }

                FLOW_INST_PARTY ip = ps[i];
                sb.Append(ip.P_USER_TEXT);
            }

            return sb.ToString();
        }


        /// <summary>
        /// 获取审核人的审核意见集合
        /// </summary>
        /// <param name="instCode">实例编码</param>
        /// <param name="stepCode">步骤编码</param>
        /// <returns></returns>
        public static string GetCheckPartyComments(DbDecipher decipher, string instCode, string stepCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("INST_CODE", instCode);
            filter.And("INST_STEP_CODE", stepCode);
            filter.And("BIZ_SID", 2);

            List<FLOW_INST_PARTY> ps = decipher.SelectModels<FLOW_INST_PARTY>(filter);

            StringBuilder sb = new StringBuilder();

            if (ps.Count == 1)
            {
                FLOW_INST_PARTY ip = ps[0];

                sb.Append($"{ip.COMMENT}");
            }
            else
            {
                for (int i = 0; i < ps.Count; i++)
                {
                    if (i > 0) { sb.Append("; "); }

                    FLOW_INST_PARTY ip = ps[i];

                    sb.Append($"{ip.P_USER_TEXT}: {ip.COMMENT}");
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 创建一个流程实例
        /// </summary>
        /// <param name="def"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static FLOW_INST CreateInst(FLOW_DEF def, FlowExParam ex)
        {

            FLOW_INST inst = new FLOW_INST();
            inst.DEF_ID = def.FLOW_DEF_ID;
            inst.DEF_CODE = def.DEF_CODE;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
            {
                inst.INST_CODE = BillIdentityMgr.NewCodeForDay("DEF_INST", "INST-", "-", 0);
            }

            inst.INST_TEXT = def.DEF_TEXT;


            inst.DATE_START = DateTime.Now;

            inst.EXTEND_PAGE_ID = ex.PageId;
            inst.EXTEND_TABLE = ex.TableName;
            inst.EXTEND_ROW_ID = ex.RowId;
            inst.EXTEND_MENU_ID = ex.MenuId;
            inst.EXTEND_DOC_URL = ex.DocUrl;

            inst.EXTEND_DOC_TEXT = ex.DocText;

            inst.EXTEND_BILL_TYPE = ex.BillType;
            inst.EXTEND_BILL_CODE = ex.BillCode;

            inst.EXT_COL_1 = ex.ExtCol1;
            inst.EXT_COL_2 = ex.ExtCol2;
            inst.EXT_COL_3 = ex.ExtCol3;
            inst.EXT_COL_4 = ex.ExtCol4;

            inst.EXT_COL_TEXT_1 = ex.ExtColText1;
            inst.EXT_COL_TEXT_2 = ex.ExtColText2;
            inst.EXT_COL_TEXT_3 = ex.ExtColText3;
            inst.EXT_COL_TEXT_4 = ex.ExtColText4;


            inst.EXTEND_BILL_CODE_FIELD = ex.BillCodeField;


            return inst;
        }




        /// <summary>
        /// 创建实例的人员
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="node"></param>
        /// <param name="nParty"></param>
        /// <returns></returns>
        public static FLOW_INST_PARTY CreateInstParty(FLOW_INST inst, FLOW_DEF_NODE node, FLOW_DEF_NODE_PARTY nParty)
        {
            FLOW_INST_PARTY p = new FLOW_INST_PARTY();

            nParty.CopyTo(p, true);

            p.DEF_NODE_ID = nParty.NODE_ID;
            p.DEF_NODE_CODE = nParty.NODE_CODE;
            p.DEF_NODE_TEXT = node.NODE_TEXT;

            p.INST_ID = inst.FLOW_INST_ID;
            p.INST_CODE = inst.INST_CODE;


            p.ROW_SID = 0;
            p.ROW_DATE_CREATE = DateTime.Now;
            p.ROW_DATE_UPDATE = DateTime.Now;

            return p;
        }

        /// <summary>
        /// 创建实例的人员
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="node"></param>
        /// <param name="nParty"></param>
        /// <returns></returns>
        public static FLOW_INST_PARTY CreateInstParty(FLOW_INST inst, FLOW_DEF_NODE node, string userCode, string userText)
        {
            FLOW_INST_PARTY p = new FLOW_INST_PARTY();

            p.DEF_ID = node.DEF_ID;
            p.DEF_CODE = node.DEF_CODE;
            p.DEF_NODE_ID = node.FLOW_DEF_NODE_ID;
            p.DEF_NODE_CODE = node.NODE_CODE;
            p.DEF_NODE_TEXT = node.NODE_TEXT;

            p.INST_ID = inst.FLOW_INST_ID;
            p.INST_CODE = inst.INST_CODE;

            p.P_USER_CODE = userCode;
            p.P_USER_TEXT = userText;

            p.ROW_SID = 0;
            p.ROW_DATE_CREATE = DateTime.Now;
            p.ROW_DATE_UPDATE = DateTime.Now;

            return p;
        }



        /// <summary>
        /// 创建一个流程步骤
        /// </summary>
        /// <param name="def"></param>
        /// <param name="node"></param>
        /// <param name="inst"></param>
        /// <returns></returns>
        public static FLOW_INST_STEP CreateInstStep(DbDecipher decipher, FLOW_DEF def,
            FLOW_DEF_NODE fromNode, FLOW_DEF_LINE fromLine, FLOW_INST_STEP fromStep,
            FLOW_DEF_NODE node, FLOW_INST inst)
        {
            int newId = BizCommonMgr.NewIdentity(decipher, "FLOW_INST", "N_STEP_IDENTITY", $"INST_CODE='{inst.INST_CODE}'", inst.N_STEP_IDENTITY + 1);


            FLOW_INST_STEP step = new FLOW_INST_STEP();

            if (fromStep != null)
            {
                step.PREV_STEP_CODE = fromStep.INST_STEP_CODE;
            }

            if (fromNode != null)
            {
                step.FROM_NODE_CODE = fromNode.NODE_CODE;
                step.FROM_NODE_TEXT = fromNode.NODE_TEXT;
                step.FROM_NODE_TYPE = fromNode.NODE_TYPE;
            }

            if (fromLine != null)
            {
                step.FROM_LINE_CODE = fromLine.LINE_CODE;
                step.FROM_LINE_TEXT = fromLine.LINE_TEXT;
            }

            step.INST_ID = inst.FLOW_INST_ID;
            step.INST_CODE = inst.INST_CODE;

            step.SEQ_NUM = newId;
            step.INST_STEP_CODE = $"{inst.INST_CODE}-{newId}";

            step.DEF_ID = inst.DEF_ID;
            step.DEF_CODE = inst.DEF_CODE;

            step.DEF_NODE_ID = node.FLOW_DEF_NODE_ID;
            step.DEF_NODE_CODE = node.NODE_CODE;
            step.DEF_NODE_TEXT = node.NODE_TEXT;
            step.DEF_NODE_TYPE = node.NODE_TYPE;

            step.P_IS_CONSIGN = node.P_IS_CONSIGN;
            step.P_CONSIGN_TYPE = node.P_CONSIGN_TYPE;
            step.P_CONSIGN_MIN_COUNT = node.P_CONSIGN_MIN_COUNT;

            step.IS_COPY_ENABLED = node.IS_COPY_ENABLED;    //抄送
            step.COPY_TO_START_PARTY_ENABLED = node.COPY_TO_START_PARTY_ENABLED;

            step.DATE_START = DateTime.Now;

            //step.P_MEET_COUNT = node.P_MEET_COUNT;
            //step.P_CUR_COUNT
            //step.P_SURPLUS_COUNT


            return step;
        }



    }
}
