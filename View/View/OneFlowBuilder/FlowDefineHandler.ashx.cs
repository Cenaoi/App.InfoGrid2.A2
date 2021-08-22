using App.BizCommon;
using App.InfoGrid2.Bll;
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
    /// FlowDefineHandler 的摘要说明
    /// </summary>
    public class FlowDefineHandler : IHttpHandler,IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            HttpResult result = null;
            string action = WebUtil.QueryTrimUpper("action");

            try
            {
                result = ProAction(context,action);
            }
            catch(Exception ex)
            {
                log.Error($"执行 Action={action} 错误.", ex);

                result = HttpResult.Error("提交错误");
            }
            
            context.Response.Write(result);
            
        }


        private HttpResult ProAction(HttpContext context,string action)
        {
            if (string.IsNullOrEmpty(action))
            {
                throw new Exception("必须指定 Action .");
            }

            HttpResult result = null;

            switch (action)
            {
                case "GET_DEF":
                    result = GetDefine(context);
                    break;
                case "NEW_DEF":
                    result = NewDefine(context);
                    break;
                case "NEW_LINE":
                    result = NewLine(context);
                    break;
                case "NEW_NODE":
                    result = NewNode(context);
                    break;
                case "SAVE_DEF":
                    result = SaveDefine(context);
                    break;
                case "REMOVE_ITEM":
                    result = RemoveItem(context);
                    break;
                case "ENABLED_ITEM":
                    result = NewItemEnabled(context);
                    break;
            }

            return result;
        }


        private HttpResult RemoveItem(HttpContext context)
        {
            string json = WebUtil.Form("data");
            SModel data = SModel.ParseJson(json);

            string item_type = data.Get<string>("item_type");

            HttpResult msg = null;

            if(item_type == "line")
            {
                msg = RemoveLine(context, data);
            }
            else if(item_type == "node")
            {
                msg = RemoveNode(context, data);
            }

            return msg;
        }


        /// <summary>
        /// 删除线
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HttpResult RemoveLine(HttpContext context, SModel data)
        {
            int def_id = WebUtil.FormInt("def_id");

            int item_id = data.Get<int>("item_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            //FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            FLOW_DEF_LINE def_line = decipher.SelectToOneModel<FLOW_DEF_LINE>($"FLOW_DEF_LINE_ID = {item_id} and DEF_ID={def_id}");

            def_line.ROW_SID = -3;
            def_line.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(def_line, "ROW_SID", "ROW_DATE_DELETE") ;


            return HttpResult.SuccessMsg("删除成功!") ;
        }


        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HttpResult RemoveNode(HttpContext context, SModel data)
        {
            int def_id = WebUtil.FormInt("def_id");

            int item_id = data.Get<int>("item_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            //FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            FLOW_DEF_NODE def_node = decipher.SelectToOneModel<FLOW_DEF_NODE>($"FLOW_DEF_NODE_ID = {item_id} and DEF_ID={def_id}");

            def_node.ROW_SID = -3;
            def_node.ROW_DATE_DELETE = DateTime.Now;

            decipher.UpdateModelProps(def_node, "ROW_SID", "ROW_DATE_DELETE");


            return HttpResult.SuccessMsg("删除成功!");
        }


        /// <summary>
        /// 获取定义
        /// </summary>
        /// <returns></returns>
        private HttpResult GetDefine(HttpContext context)
        {
            int def_id = WebUtil.FormInt("def_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            List<FLOW_DEF_LINE> def_lines = decipher.SelectModels<FLOW_DEF_LINE>($"ROW_SID >= 0 and DEF_ID = {def_id}");
            List<FLOW_DEF_NODE> def_nodes = decipher.SelectModels<FLOW_DEF_NODE>($"ROW_SID >= 0 and DEF_ID = {def_id}");



            SModel data = new SModel();

            data["def_id"] = def.FLOW_DEF_ID;

            SModelList items = new SModelList();

            foreach (var item in def_nodes)
            {
                SModel sm = SModel.ParseJson(item.GRAPHICS);
                sm["item_code"] = item.NODE_CODE;
                sm["item_fullname"] = item.G_FULLNAME;
                sm["text"] = item.NODE_TEXT;
                sm["style_name"] = item.STYLE_NAME;

                items.Add(sm);
            }

            foreach (var item in def_lines)
            {
                //if (StringUtil.IsBlank(item.G_FULLNAME))
                //{
                    item.G_FULLNAME = "Mini2.flow.extend.Line";
                //}

                SModel sm = SModel.ParseJson(item.GRAPHICS);
                sm["item_code"] = item.LINE_CODE;
                sm["item_fullname"] = item.G_FULLNAME;
                sm["text"] = item.LINE_TEXT;
                sm["style_name"] = item.STYLE_NAME;

                items.Add(sm);
            }


            data["items"] = items;
            

            return HttpResult.Success(data);
        }

        /// <summary>
        /// 新建定义
        /// </summary>
        /// <returns></returns>
        private HttpResult NewDefine(HttpContext context)
        {
            FLOW_DEF flowDef = new FLOW_DEF();

            flowDef.DEF_CODE = BizCommon.BillIdentityMgr.NewCodeForDay("FLOW_DEF_CODE", "FLOW-", "-", 2);
            flowDef.DEF_TEXT = "流程-" + flowDef.DEF_CODE;


            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.InsertModel(flowDef);


            return HttpResult.Success(new
            {
                new_id = flowDef.FLOW_DEF_ID
            });
        }

        /// <summary>
        /// 新建线
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HttpResult NewLine(HttpContext context)
        {

            int def_id = WebUtil.FormInt("def_id");

            string node_fullname = "Mini2.flow.extend.Line";


            DbDecipher decipher = ModelAction.OpenDecipher();
            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);


            def.N_LINE_IDENTITY++;
            decipher.UpdateModelProps(def, "N_LINE_IDENTITY");

            string newLineId = $"line_{def.N_LINE_IDENTITY}";

            FLOW_DEF_LINE line = new FLOW_DEF_LINE();
            line.ROW_SID = -1;
            line.DEF_ID = def.FLOW_DEF_ID;
            line.DEF_CODE = def.DEF_CODE;
            line.LINE_CODE = newLineId;
            line.LINE_TEXT = newLineId;

            decipher.InsertModel(line);


            SModel g = new SModel();
            g["item_id"] = line.FLOW_DEF_LINE_ID;
            g["item_code"] = line.LINE_CODE;
            g["item_type"] = "line";
            g["item_fullname"] = node_fullname;
            line.GRAPHICS = g.ToJson();

            decipher.UpdateModelProps(line, "GRAPHICS");

            

            return HttpResult.Success(new
            {
                line_id = line.FLOW_DEF_LINE_ID,
                item_fullname = node_fullname
            });

        }

        /// <summary>
        /// 新建节点
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HttpResult NewNode(HttpContext context)
        {

            int def_id = WebUtil.FormInt("def_id");
            string node_type = WebUtil.Form("node_type");

            string node_fullname;

            if (node_type == "start")
            {
                node_fullname = "Mini2.flow.extend.StartNode";
            }
            else if (node_type == "end")
            {
                node_fullname = "Mini2.flow.extend.EndNode";
            }
            else if (node_type == "auto_node")
            {
                node_fullname = "Mini2.flow.extend.AutoNode";
            }
            else if(node_type == "note")
            {
                node_fullname = "Mini2.flow.extend.Note";
            }
            else
            {
                node_fullname = "Mini2.flow.extend.Node";
            }



            DbDecipher decipher = ModelAction.OpenDecipher();


            int newId = BizCommonMgr.NewIdentity("FLOW_DEF", "N_NODE_IDENTITY", $"FLOW_DEF_ID={def_id}");

            string newNodeId = $"{node_type}_{newId}";


            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);
            
            FLOW_DEF_NODE node = new FLOW_DEF_NODE();
            node.ROW_SID = -1;
            node.DEF_ID = def.FLOW_DEF_ID;
            node.DEF_CODE = def.DEF_CODE;

            node.NODE_CODE = newNodeId;
            node.NODE_TEXT = newNodeId;
            node.NODE_TYPE = node_type;
            node.G_FULLNAME = node_fullname;

            decipher.InsertModel(node);


            //{"item_type":"node", "item_fullname":"Mini2.flow.Action", "item_id":44, "id":"node_3", "text":"节点", "x":806, "y":84, "width":92, "height":75}
            SModel g = new SModel();
            g["item_id"] = node.FLOW_DEF_NODE_ID;
            g["item_code"] = node.NODE_CODE;
            g["id"] = newNodeId;
            g["text"] = newNodeId;
            g["item_type"] = "node";
            g["width"] = 80;
            g["height"] = 30;

            g["item_fullname"] = node_fullname;

            node.GRAPHICS = g.ToJson();

            decipher.UpdateModelProps(node, "GRAPHICS");



            //SModel data = new SModel();
            //data["item_id"] = node.FLOW_DEF_NODE_ID;
            //data["node_code"] = node.NODE_CODE;
            //data["item_fullname"] = node_fullname;

            //return HttpResult.Success(data);

            return HttpResult.Success(new
            {
                item_id = node.FLOW_DEF_NODE_ID,
                node_code = node.NODE_CODE,
                item_fullname = node_fullname
            });
        }


        /// <summary>
        /// 保存定义
        /// </summary>
        /// <returns></returns>
        private HttpResult SaveDefine(HttpContext context)
        {
            string dataJson = WebUtil.Form("flow_define");

            SModel define = SModel.ParseJson(dataJson);


            SModelList def_data = define["data"] as SModelList;


            DbDecipher decipher = ModelAction.OpenDecipher();



            int def_id = define.Get<int>("def_id");

            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);


            foreach (SModel item in def_data)
            {
                string itemJson = item.ToJson();

                try
                {

                    string item_type = item["item_type"];

                    int item_id = item.Get<int>("item_id");


                    if (item_type == "line")
                    {
                        SaveLine(decipher, def_id, item_id, itemJson, item);
                    }
                    else if (item_type == "node")
                    {
                        SaveNode(decipher, def_id, item_id, itemJson, item);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("处理节点错误 json = \r\n" + itemJson, ex);
                }
            }



            return HttpResult.Success();
        }


        private void SaveNode(DbDecipher decipher, int def_id, int item_id, string itemJson, SModel item)
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_NODE));
            filter.And("FLOW_DEF_NODE_ID", item_id);
            filter.And("DEF_ID", def_id);

            var node = decipher.SelectToOneModel<FLOW_DEF_NODE>(filter);
            node.SetTakeChange(true);

            node.STYLE_NAME = (string)item["style_name"];
            node.GRAPHICS = itemJson;


            node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModel(node, true);
        }


        private void SaveLine(DbDecipher decipher, int def_id, int item_id,string itemJson, SModel item)
        {
            FLOW_DEF_NODE fromNode = null;
            FLOW_DEF_NODE toNode = null;

            SModel start_point = item["start_point"] as SModel;
            SModel end_point = item["end_point"] as SModel;

            if (start_point != null)
            {
                int fromId = start_point.Get<int>("item_id");
                fromNode = decipher.SelectModelByPk<FLOW_DEF_NODE>(fromId);
            }

            if (end_point != null)
            {
                int toId = end_point.Get<int>("item_id");
                toNode = decipher.SelectModelByPk<FLOW_DEF_NODE>(toId);
            }



            LightModelFilter filter = new LightModelFilter(typeof(FLOW_DEF_LINE));
            filter.And("FLOW_DEF_LINE_ID", item_id);
            filter.And("DEF_ID", def_id);

            var line = decipher.SelectToOneModel<FLOW_DEF_LINE>(filter);

            line.SetTakeChange(true);

            if (fromNode != null)
            {
                line.FROM_NODE_ID = fromNode.FLOW_DEF_NODE_ID;
                line.FROM_NODE_CODE = fromNode.NODE_CODE;
            }
            else
            {
                line.FROM_NODE_ID = 0;
                line.FROM_NODE_CODE = string.Empty;
            }


            if (toNode != null)
            {
                line.TO_NODE_ID = toNode.FLOW_DEF_NODE_ID;
                line.TO_NODE_CODE = toNode.NODE_CODE;
            }
            else
            {
                line.TO_NODE_ID = 0;
                line.TO_NODE_CODE = string.Empty;
            }

            line.G_FULLNAME = (string)item["item_fullname"];
            line.STYLE_NAME = (string)item["style_name"];
            line.GRAPHICS = itemJson;

            line.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModel(line, true);

        }


        private HttpResult NewItemEnabled(HttpContext context)
        {
            string json = WebUtil.Form("data");
            SModel data = SModel.ParseJson(json);

            string item_type = data.Get<string>("item_type");

            HttpResult msg = null;

            if (item_type == "line")
            {
                msg = NewLienEnabled(context, data);
            }
            else if (item_type == "node" || item_type =="action")
            {
                msg = NewNodeEnabled(context, data);
            }

            return msg;
        }


        /// <summary>
        /// 新建节点激活
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HttpResult NewNodeEnabled(HttpContext context, SModel data)
        {
            int def_id = WebUtil.FormInt("def_id");
            int item_id = data.Get<int>("item_id");


            DbDecipher decipher = ModelAction.OpenDecipher();


            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            FLOW_DEF_NODE def_node = decipher.SelectToOneModel<FLOW_DEF_NODE>($"FLOW_DEF_NODE_ID = {item_id} and ROW_SID = -1 and DEF_ID={def_id}");

            if (def_node == null)
            {
                return HttpResult.Error();
            }

            def_node.ROW_SID = 0;
            def_node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(def_node, "ROW_SID", "ROW_DATE_UPDATE");

            
            return HttpResult.Success();
        }


        private HttpResult NewLienEnabled(HttpContext context, SModel data)
        {
            int def_id = WebUtil.FormInt("def_id");
            int item_id = data.Get<int>("item_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            FLOW_DEF def = decipher.SelectModelByPk<FLOW_DEF>(def_id);

            FLOW_DEF_LINE def_line = decipher.SelectToOneModel<FLOW_DEF_LINE>($"FLOW_DEF_LINE_ID = {item_id} and ROW_SID =-1 and DEF_ID={def_id}");

            if(def_line == null)
            {
                return HttpResult.Error(); 
            }

            def_line.ROW_SID = 0;
            def_line.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(def_line, "ROW_SID", "ROW_DATE_UPDATE");

            return HttpResult.Success();
        }



        public bool IsReusable
        {
            get { return false; }
        }
    }
}