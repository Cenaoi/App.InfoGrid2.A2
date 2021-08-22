using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using EC5.Utility;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 扩展参数
    /// </summary>
    public class FlowExParam
    {
        public int PageId;

        public int TableId;

        public string TableName;

        public int RowId;

        public int MenuId;

        public string DocText;

        public string DocUrl;

        public string BillType;

        public string BillCode;

        /// <summary>
        /// 业务单号的字段名
        /// </summary>
        public string BillCodeField;

        /// <summary>
        /// 加急数值...值越大
        /// </summary>
        public int TagSID { get; set; }


        /// <summary>
        /// 扩展字段名
        /// </summary>
        public string ExtCol1;
        public string ExtCol2;
        public string ExtCol3;
        public string ExtCol4;

        /// <summary>
        /// 扩展字段标题
        /// </summary>
        public string ExtColText1;
        public string ExtColText2;
        public string ExtColText3;
        public string ExtColText4;



    }

    /// <summary>
    /// 流程节点的信息
    /// </summary>
    public class FlowNodeInfo
    {
        public FLOW_DEF_NODE Node { get; set; }

        public List<FLOW_DEF_LINE> Lines { get; set; }
    }


    /// <summary>
    /// 流程管理
    /// </summary>
    public static class FlowMgr
    {


        public static FLOW_DEF GetDefine( string flowCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_DEF flow_def = decipher.SelectToOneModel<FLOW_DEF>(filter);

            if(flow_def == null)
            {
                throw new Exception($"流程定义不存在: {flowCode}");
            }

            return flow_def;

        }

        /// <summary>
        /// 根据节点类型获取节点
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public static FLOW_DEF_NODE GetNodeForType(string flowCode, string nodeType)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_NODE));
            filter.Locks.Add(LockType.NoLock);
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("NODE_TYPE", nodeType);

            FLOW_DEF_NODE node = decipher.SelectToOneModel<FLOW_DEF_NODE>(filter);

            return node;
        }

        /// <summary>
        /// 根据节点类型获取节点
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        public static FLOW_DEF_NODE GetNode(DbDecipher decipher,string flowCode, string nodeCode)
        {

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_NODE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("NODE_CODE", nodeCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_DEF_NODE node = decipher.SelectToOneModel<FLOW_DEF_NODE>(filter);

            return node;
        }


        public static List<FLOW_DEF_LINE> GetLines(string flowCode, string fromNodeCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_LINE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("FROM_NODE_CODE", fromNodeCode);
            filter.Locks.Add(LockType.NoLock);

            List<FLOW_DEF_LINE> node = decipher.SelectModels<FLOW_DEF_LINE>(filter);

            return node;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="fromNodeCode"></param>
        /// <param name="toNodeCode">结束点</param>
        /// <returns></returns>
        public static FLOW_DEF_LINE GetLine( string flowCode, string fromNodeCode, string toNodeCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            FLOW_DEF_LINE node = GetLine(decipher, flowCode, fromNodeCode, toNodeCode);

            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="fromNodeCode"></param>
        /// <param name="toNodeCode">结束点</param>
        /// <returns></returns>
        public static FLOW_DEF_LINE GetLine(DbDecipher decipher,string flowCode, string fromNodeCode, string toNodeCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_LINE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("FROM_NODE_CODE", fromNodeCode);
            filter.And("TO_NODE_CODE", toNodeCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_DEF_LINE node = decipher.SelectToOneModel< FLOW_DEF_LINE>(filter);

            return node;
        }


        public static FLOW_DEF_LINE GetLine( string flowCode, string lineCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            
            FLOW_DEF_LINE node = GetLine(decipher, flowCode, lineCode);

            return node;
        }


        public static FLOW_DEF_LINE GetLine(DbDecipher decipher,string flowCode,string lineCode)
        {

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_LINE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("LINE_CODE", lineCode);
            filter.Locks.Add(LockType.NoLock);

            FLOW_DEF_LINE node = decipher.SelectToOneModel< FLOW_DEF_LINE>(filter);

            return node;
        }

        /// <summary>
        /// 获取节点的当事人集合
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        public static List<FLOW_DEF_NODE_PARTY> GetPartys(DbDecipher decipher, string flowCode, string nodeCode)
        {
            
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_NODE_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("NODE_CODE", nodeCode);
            filter.Locks.Add(LockType.NoLock);

            List<FLOW_DEF_NODE_PARTY> node = decipher.SelectModels<FLOW_DEF_NODE_PARTY>(filter);

            return node;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeCode"></param>
        /// <param name="partyCodes"></param>
        /// <returns></returns>
        public static List<FLOW_DEF_NODE_PARTY> GetPartys(DbDecipher decipher, string flowCode, string nodeCode, List<string> partyCodes)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_NODE_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("NODE_CODE", nodeCode);
            filter.And("P_USER_CODE", partyCodes, Logic.In);
            filter.Locks.Add(LockType.NoLock);

            List<FLOW_DEF_NODE_PARTY> node = decipher.SelectModels<FLOW_DEF_NODE_PARTY>(filter);

            return node;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowCode"></param>
        /// <param name="nodeCode"></param>
        /// <param name="partyCodes"></param>
        /// <returns></returns>
        public static List<FLOW_DEF_NODE_PARTY> GetPartys(string flowCode, string nodeCode, List<string> partyCodes)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            
            List<FLOW_DEF_NODE_PARTY> node = GetPartys(decipher, flowCode, nodeCode, partyCodes);

            return node;
        }


        /// <summary>
        /// 获取当事人未审核的文档
        /// </summary>
        /// <returns></returns>
        public static SModelList GetUserDocsAll(int bizSid, string tSqlWhere)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.Joins.Add(typeof(FLOW_INST), "FLOW_INST_PARTY.INST_CODE = FLOW_INST.INST_CODE");


            filter.And("FLOW_INST_PARTY.ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("FLOW_INST_PARTY.BIZ_SID", bizSid);
            filter.And("PARTY_TYPE", "USER");
            filter.TSqlWhere = tSqlWhere;
            filter.Fields = new string[] { "*", "FLOW_INST_PARTY.ROW_DATE_CREATE as A_ROW_DATE_CREATE", "FLOW_INST.ROW_DATE_CREATE as B_ROW_DATE_CREATE" };

            //filter.And("P_USER_CODE", userCode);
            filter.TSqlOrderBy = "FLOW_INST_PARTY.ROW_DATE_CREATE desc";

            SModelList instPartys = decipher.GetSModelList(filter);

            ChagneDocUrl(instPartys);


            return instPartys;

        }

        /// <summary>
        /// 获取当事人未审核的文档
        /// </summary>
        /// <returns></returns>
        public  static SModelList GetUserDocs(int bizSid,string userCode, string tSqlWhere,string orderBy = "")
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.Joins.Add(typeof(FLOW_INST), "FLOW_INST_PARTY.INST_CODE = FLOW_INST.INST_CODE");

            filter.And("FLOW_INST_PARTY.ROW_SID",0, Logic.GreaterThanOrEqual);
            filter.And("FLOW_INST_PARTY.BIZ_SID", bizSid);
            filter.And("PARTY_TYPE", "USER");
            filter.And("P_USER_CODE", userCode);
            filter.TSqlWhere = tSqlWhere;
            filter.Fields = new string[] { "*", "FLOW_INST_PARTY.ROW_DATE_CREATE as A_ROW_DATE_CREATE", "FLOW_INST.ROW_DATE_CREATE as B_ROW_DATE_CREATE" };

            if (string.IsNullOrWhiteSpace(orderBy))
            {

                filter.TSqlOrderBy = "FLOW_INST_PARTY.ROW_DATE_CREATE desc";

            }else
            {
                filter.TSqlOrderBy = orderBy;
            }

            SModelList instPartys = decipher.GetSModelList(filter);

            ChagneDocUrl(instPartys);

            return instPartys;
                        
        }


        /// <summary>
        /// 获取所有人未审核的文档
        /// 小渔夫新增的
        /// 2017-09-05
        /// </summary>
        /// <returns></returns>
        public static SModelList GetAllUserDocs(int bizSid, string tSqlWhere,string orderBy = "")
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.Joins.Add(typeof(FLOW_INST), "FLOW_INST_PARTY.INST_CODE = FLOW_INST.INST_CODE");

            filter.And("FLOW_INST_PARTY.ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("FLOW_INST_PARTY.BIZ_SID", bizSid);
            filter.And("PARTY_TYPE", "USER");
            filter.TSqlWhere = tSqlWhere;
            filter.Fields = new string[] { "*", "FLOW_INST_PARTY.ROW_DATE_CREATE as A_ROW_DATE_CREATE", "FLOW_INST.ROW_DATE_CREATE as B_ROW_DATE_CREATE" };

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                filter.TSqlOrderBy = "FLOW_INST_PARTY.ROW_DATE_CREATE desc";

            }else
            {
                filter.TSqlOrderBy = orderBy;
            }
            
            
            SModelList instPartys = decipher.GetSModelList(filter);

            ChagneDocUrl(instPartys);

            return instPartys;

        }


        /// <summary>
        /// 获取当事人未审核的文档
        /// 小渔夫新增的
        /// 2017-08-23
        /// </summary>
        /// <param name="bizSid">业务状态</param>
        /// <param name="userCode">用户编码</param>
        /// <param name="tSqlWhere">额外的sql语句</param>
        /// <param name="top">top数量</param>
        /// <returns></returns>
        public static SModelList GetUserDocs(int bizSid, string userCode, string tSqlWhere,int top)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_PARTY));
            filter.Joins.Add(typeof(FLOW_INST), "FLOW_INST_PARTY.INST_CODE = FLOW_INST.INST_CODE");

            filter.And("FLOW_INST_PARTY.ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("FLOW_INST_PARTY.BIZ_SID", bizSid);
            filter.And("PARTY_TYPE", "USER");
            filter.And("P_USER_CODE", userCode);
            filter.TSqlWhere = tSqlWhere;
            filter.Top = top;
            filter.Fields = new string[] { "*", "FLOW_INST_PARTY.ROW_DATE_CREATE as A_ROW_DATE_CREATE", "FLOW_INST.ROW_DATE_CREATE as B_ROW_DATE_CREATE" };

            filter.TSqlOrderBy = "FLOW_INST_PARTY.ROW_DATE_CREATE desc";

            SModelList instPartys = decipher.GetSModelList(filter);


            ChagneDocUrl(instPartys);


            return instPartys;

        }


        /// <summary>
        /// 改写 doc url
        /// </summary>
        /// <param name="table"></param>
        private static void ChagneDocUrl(SModelList table)
        {
            foreach (var sm in table)
            {
                if (sm.HasField("EXTEND_DOC_URL") && sm.HasField("EXTEND_TABLE"))
                {
                    string tabName = sm["EXTEND_TABLE"];
                    int menuId = sm["EXTEND_MENU_ID"];

                    string newUrl = FlowUrlMgr.Get("company", menuId, tabName, sm);

                    sm["EXTEND_DOC_URL"] = StringUtil.NoBlank(newUrl, sm["EXTEND_DOC_URL"]);
                }
            }
        }

        /// <summary>
        /// 获取当事人未审核的文档
        /// </summary>
        /// <returns></returns>
        public static SModelList GetUserStartDocs(int bizSid, string userCode, string tSqlWhere,string orderBy = "")
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST));

            filter.AddFilter("ROW_SID >= 0");
            //filter.And("BIZ_SID", bizSid);
            //filter.And("PARTY_TYPE", "USER");

            filter.And("START_USER_CODE", userCode);
            filter.TSqlWhere = tSqlWhere;

            if (string.IsNullOrWhiteSpace(orderBy))
            {

                filter.TSqlOrderBy = "ROW_DATE_CREATE desc";
            }else
            {
                filter.TSqlOrderBy = orderBy;
            }

    
            SModelList instPartys = decipher.GetSModelList(filter);

            ChagneDocUrl(instPartys);


            return instPartys;

        }


        /// <summary>
        /// 获取当事人抄送的文档
        /// 小渔夫 修改于 2017-09-06
        /// </summary>
        /// <returns></returns>
        public static SModelList GetInstCopys(int bizSid, string userCode, string tSqlWhere,string orderBy = "")
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_COPY));

            filter.AddFilter("ROW_SID >= 0");
            //filter.And("BIZ_SID", bizSid);
            //filter.And("PARTY_TYPE", "USER");

            filter.And("P_USER_CODE", userCode);
            filter.TSqlWhere = tSqlWhere;

            if (string.IsNullOrWhiteSpace(orderBy))
            {

                filter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            }else
            {
                filter.TSqlOrderBy = orderBy;

            }


            SModelList instPartys = decipher.GetSModelList(filter);

            ChagneDocUrl(instPartys);


            return instPartys;

        }



        /// <summary>
        /// 获取节点下面准备抄送的人员列表
        /// </summary>
        /// <param name="def_code"></param>
        /// <param name="node_Code"></param>
        /// <returns></returns>
        public static LModelList<FLOW_DEF_NODE_COPY_PARTY> GetCopyPartys(string def_code,string node_Code)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_NODE_COPY_PARTY));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", def_code);
            filter.And("NODE_CODE", node_Code);

            LModelList<FLOW_DEF_NODE_COPY_PARTY> cpList = decipher.SelectModels<FLOW_DEF_NODE_COPY_PARTY>(filter);

            return cpList;
        }


        

        public static FlowNodeInfo GetNodeInfo( string flowCode, string fromNodeCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            

            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_LINE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DEF_CODE", flowCode);
            filter.And("FROM_NODE_CODE", fromNodeCode);
            filter.Locks.Add(LockType.NoLock);

            List<FLOW_DEF_LINE> lines = decipher.SelectModels<FLOW_DEF_LINE>(filter);

            FlowNodeInfo ni = new FlowNodeInfo();
            ni.Node = GetNode(decipher,flowCode, fromNodeCode);
            ni.Lines = lines;

            return ni;            
        }


        /// <summary>
        /// 获取流程开始节点的信息
        /// </summary>
        /// <param name="flowCode"></param>
        /// <returns></returns>
        public static FlowNodeInfo GetStartNodeInfo(DbDecipher decipher, string flowCode)
        {
            FLOW_DEF def = FlowMgr.GetDefine(flowCode);


            if (def == null)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" .");
            }

            //开始节点 
            FLOW_DEF_NODE start = GetNodeForType(flowCode, "start");

            if (start == null)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" 的开始开始节点.");
            }

            List<FLOW_DEF_LINE> startLine = GetLines(flowCode, start.NODE_CODE);

            if (startLine == null || startLine.Count == 0)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" 的开始开始节点的下一步连接线.");
            }

            FLOW_DEF_LINE defLine = startLine[0];

            //找到草稿的节点
            FLOW_DEF_NODE node = GetNode(decipher, flowCode, defLine.TO_NODE_CODE);


            if (node == null)
            {
                throw new Exception($"找不到流程 \"{flowCode}\" 的节点 \"{defLine.TO_NODE_CODE}\".");
            }

            List<FLOW_DEF_LINE> lines = FlowMgr.GetLines(flowCode, node.NODE_CODE);

            //curFlow = def;
            //curNode = node;
            //curLines = lines;
            FlowNodeInfo info = new FlowNodeInfo();
            info.Node = node;
            info.Lines = lines;

            return info;
        }
    }
}
