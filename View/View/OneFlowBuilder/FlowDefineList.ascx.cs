using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace App.InfoGrid2.View.OneFlowBuilder
{
    public partial class FlowDefineList : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.Inserting += Store1_Inserting;
            this.table1.Command += Table1_Command;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        private void Store1_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            string code = BizCommon.BillIdentityMgr.NewCodeForDay("FLOW_DEF_CODE", "FLOW-", 2);
            model["DEF_CODE"] = code;
            model["DEF_TEXT"] = "流程-" + code;
        }

        private void Table1_Command(object sender, EasyClick.Web.Mini2.TableCommandEventArgs e)
        {
            if(e.CommandName == "GoEditFlow")
            {
                GoEditFlow(e.Record);
            }
            else if(e.CommandName == "GoPreview")
            {
                //string ruiDir = "/App/InfoGrid2/View/OneFlowBuilder";

                //string url = ruiDir + $"/FlowInstPreview.aspx?flow_def_id={e.Record.Id}&flow_inst_code={}&_rum={Guid.NewGuid()}";

                //EcView.Show(url, "流程预览");
            }
        }

        private void GoEditFlow(DataRecord record)
        {
            string ruiDir = "/App/InfoGrid2/View/OneFlowBuilder";

            string url = ruiDir + $"/FlowBuilder.aspx?flow_def_id={record.Id}&_rum={Guid.NewGuid()}";

            EcView.Show(url, "流程编辑");
        }


        /// <summary>
        /// 复制流程按钮事件
        /// </summary>
        public void GoCopyDef()
        {

            //流程ID
            string def_id =  store1.CurDataId;


            DbDecipher decipher = ModelAction.OpenDecipher();


            FLOW_DEF flow_def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            if (flow_def == null)
            {
                MessageBox.Alert("找不到焦点行数据了！");

                return;
            }

            LightModelFilter lmFilterNode = new LightModelFilter(typeof(FLOW_DEF_NODE));
            lmFilterNode.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterNode.And("DEF_ID",def_id);

            List<FLOW_DEF_NODE> flow_nodes = decipher.SelectModels<FLOW_DEF_NODE>(lmFilterNode);

            LightModelFilter lmFilterNodeParty = new LightModelFilter(typeof(FLOW_DEF_LINE));
            lmFilterNodeParty.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterNodeParty.And("DEF_ID", def_id);

            List<FLOW_DEF_NODE_PARTY> flow_node_partys = decipher.SelectModels<FLOW_DEF_NODE_PARTY>(lmFilterNodeParty);

            LightModelFilter lmFilterLine = new LightModelFilter(typeof(FLOW_DEF_LINE));
            lmFilterLine.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterLine.And("DEF_ID", def_id);

            List<FLOW_DEF_LINE> flow_lines = decipher.SelectModels<FLOW_DEF_LINE>(lmFilterLine);




            //新流程对象
            FLOW_DEF new_flow_def = new FLOW_DEF();
            //新节点集合
            List<FLOW_DEF_NODE> new_flow_nodes = new List<FLOW_DEF_NODE>();

            //新参与活动者
            List<FLOW_DEF_NODE_PARTY> new_flow_node_partys = new List<FLOW_DEF_NODE_PARTY>();

            //新路由集合
            List<FLOW_DEF_LINE> new_flow_lines = new List<FLOW_DEF_LINE>();


            //这里存放和路由有关联的节点集合
            Dictionary<FLOW_DEF_LINE, FLOW_DEF_LINE> temp_line_dic = new Dictionary<FLOW_DEF_LINE, FLOW_DEF_LINE>();


            //事务开始
            decipher.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

            IdentityFactory idFactory = decipher.IdentityFactory;



            #region 流程对象

            flow_def.CopyTo(new_flow_def);
            new_flow_def.BIZ_DATE_CREATE = new_flow_def.BIZ_DATE_UPDATE = DateTime.Now;

            new_flow_def.FLOW_DEF_ID = idFactory.GetNewIdentity("FLOW_DEF");
            new_flow_def.DEF_CODE = BillIdentityMgr.NewCodeForDay("FLOW_DEF_CODE", "FLOW-", 2);

            new_flow_def.DEF_TEXT = new_flow_def.DEF_TEXT + "-复制的";

            #endregion 



            foreach (FLOW_DEF_NODE flow_node in flow_nodes)
            {


                #region 流程节点对象

                FLOW_DEF_NODE new_flow_node = new FLOW_DEF_NODE();

                flow_node.CopyTo(new_flow_node);
                new_flow_node.ROW_DATE_CREATE = new_flow_node.ROW_DATE_UPDATE = DateTime.Now;
                new_flow_node.DEF_ID = new_flow_def.FLOW_DEF_ID;
                new_flow_node.DEF_CODE = new_flow_def.DEF_CODE;
                new_flow_node.FLOW_DEF_NODE_ID = idFactory.GetNewIdentity("FLOW_DEF_NODE");

                SModel sm_node = SModel.ParseJson(new_flow_node.GRAPHICS);
                sm_node["item_id"] = new_flow_node.FLOW_DEF_NODE_ID;

                new_flow_node.GRAPHICS = sm_node.ToJson();

                new_flow_nodes.Add(new_flow_node);

                #endregion


                #region 活动参数者对象

                //找出这个节点的活动参与者
                List<FLOW_DEF_NODE_PARTY> flow_def_node_partys = flow_node_partys.FindAll(p => p.NODE_ID == flow_node.FLOW_DEF_NODE_ID);


                foreach (FLOW_DEF_NODE_PARTY party in flow_def_node_partys)
                {

                    FLOW_DEF_NODE_PARTY new_party = new FLOW_DEF_NODE_PARTY();

                    party.CopyTo(new_party);

                    new_party.ROW_DATE_CREATE = new_party.ROW_DATE_UPDATE = DateTime.Now;

                    new_party.FLOW_DEF_NODE_PARTY_ID = idFactory.GetNewIdentity("FLOW_DEF_NODE_PARTY");

                    new_party.NODE_ID = new_flow_node.FLOW_DEF_NODE_ID;

                    new_party.DEF_ID = new_flow_def.FLOW_DEF_ID;
                    new_party.DEF_CODE = new_flow_def.DEF_CODE;

                    new_flow_node_partys.Add(new_party);

                }

                #endregion


                #region 流程路由对象

                //开始线
                List<FLOW_DEF_LINE> first_lines = flow_lines.FindAll(l => l.FROM_NODE_ID == flow_node.FLOW_DEF_NODE_ID );
                //结束线
                List<FLOW_DEF_LINE> last_lines = flow_lines.FindAll(l => l.TO_NODE_ID == flow_node.FLOW_DEF_NODE_ID);


                foreach (FLOW_DEF_LINE first_line in first_lines)
                {


                    FLOW_DEF_LINE new_first_line = null;


                    if (temp_line_dic.ContainsKey(first_line))
                    {

                        new_first_line = temp_line_dic[first_line];

                    }
                    else
                    {

                        new_first_line = new FLOW_DEF_LINE();

                        first_line.CopyTo(new_first_line);

                        new_first_line.ROW_DATE_CREATE = new_first_line.ROW_DATE_UPDATE = DateTime.Now;
                        new_first_line.DEF_ID = new_flow_def.FLOW_DEF_ID;
                        new_first_line.DEF_CODE = new_flow_def.DEF_CODE;

                        new_first_line.FLOW_DEF_LINE_ID = idFactory.GetNewIdentity("FLOW_DEF_LINE");

                        new_flow_lines.Add(new_first_line);

                        temp_line_dic.Add(first_line, new_first_line);

                    }

                    new_first_line.FROM_NODE_ID = new_flow_node.FLOW_DEF_NODE_ID;

                    SModel sm_line = SModel.ParseJson(new_first_line.GRAPHICS);
                    sm_line["item_id"] = new_first_line.FLOW_DEF_LINE_ID;
                    sm_line["start_node_id"] = new_flow_node.FLOW_DEF_NODE_ID;

                    SModel sm_line_setar = sm_line.Get<SModel>("start_point");

                    sm_line_setar["item_id"] = new_flow_node.FLOW_DEF_NODE_ID;

                    sm_line["start_point"] = sm_line_setar;


                    new_first_line.GRAPHICS = sm_line.ToJson();

                }

                foreach (FLOW_DEF_LINE last_line in last_lines)
                {


                    FLOW_DEF_LINE new_last_line = null;


                    if (temp_line_dic.ContainsKey(last_line))
                    {

                        new_last_line = temp_line_dic[last_line];

                    }
                    else
                    {

                        new_last_line = new FLOW_DEF_LINE();

                        last_line.CopyTo(new_last_line);

                        new_last_line.ROW_DATE_CREATE = new_last_line.ROW_DATE_UPDATE = DateTime.Now;
                        new_last_line.DEF_ID = new_flow_def.FLOW_DEF_ID;
                        new_last_line.DEF_CODE = new_flow_def.DEF_CODE;

                        new_last_line.FLOW_DEF_LINE_ID = idFactory.GetNewIdentity("FLOW_DEF_LINE");


                        new_flow_lines.Add(new_last_line);

                        temp_line_dic.Add(last_line, new_last_line);

                    }

                    new_last_line.TO_NODE_ID = new_flow_node.FLOW_DEF_NODE_ID;

                    SModel sm_line = SModel.ParseJson(new_last_line.GRAPHICS);
                    sm_line["item_id"] = new_last_line.FLOW_DEF_LINE_ID;
                    sm_line["end_node_id"] = new_flow_node.FLOW_DEF_NODE_ID;

                    SModel sm_line_setar = sm_line.Get<SModel>("end_point");

                    sm_line_setar["item_id"] = new_flow_node.FLOW_DEF_NODE_ID;

                    sm_line["end_point"] = sm_line_setar;

                    new_last_line.GRAPHICS = sm_line.ToJson();


                }


                #endregion

            }



            try
            {
                decipher.IdentityStop();

                decipher.InsertModel(new_flow_def);

                decipher.InsertModels(new_flow_nodes);

                decipher.InsertModels(new_flow_node_partys);

                decipher.InsertModels(new_flow_lines);


                decipher.TransactionCommit();

                decipher.IdentityRecover();


                store1.Refresh();

                

            }catch(Exception ex)
            {
                decipher.TransactionRollback();


                log.Error("复制流程信息出错了！", ex);

                MessageBox.Alert("哦噢，出错了！");

            }



        }

    }
}