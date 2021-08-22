using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.AC3;
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
using EC5.Entity.Expanding.ExpandV1;

namespace App.InfoGrid2.View.MoreActionBuilder
{
    /// <summary>
    /// DwgHandler 的摘要说明
    /// </summary>
    public class DwgHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string DWG_CODE_FORMAT = @"^DWG-\d{6}-\d{1,6}$";

        const string ITEM_CODE_FORMAT = @"^\b\w*$";



        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            HttpResult pack = null;

            string action = WebUtil.QueryTrimUpper("action");

            try
            {
                pack = ProAction(context, action);
            }
            catch (Exception ex)
            {
                log.Error($"执行 Action={action} 错误.", ex);

                pack = HttpResult.Error("提交错误");
            }


            context.Response.Write(pack);

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
                case "GET_DWG":
                    result = GetDwg(context);
                    break;
                case "NEW_DWG":
                    result = NewDefine(context);
                    break;
                case "NEW_LINE":
                    result = NewLine(context);
                    break;
                case "NEW_NODE":
                    result = NewNode(context);
                    break;
                case "SAVE_DWG":
                    result = SaveDefine(context);
                    break;
                case "REMOVE_ITEM":
                    result = RemoveItem(context);
                    break;
                case "ENABLED_ITEM":
                    result = NewItemEnabled(context);
                    break;
                case "SAVE_LISTEN_FILTER":
                    result = SaveListenFilter(context);
                    break;
                case "GET_LISTEN_FILTER":
                    result = GetListenFilter(context);
                    break;

                case "SAVE_LISTEN_VCHANGE":
                    result = SaveListenVChange(context);
                    break;
                case "GET_LISTEN_VCHANGE":
                    result = GetListenVChange(context);
                    break;

                case "SAVE_OPERATE_FILTER":
                    result = SaveOperateFilter(context);
                    break;
                case "GET_OPERATE_FILTER":
                    result = GetOperateFilter(context);
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

            if (item_type == "line")
            {
                msg = RemoveLine(context, data);
            }
            else if (item_type == "node")
            {
                msg = RemoveNode(context, data);
            }

            return msg;
        }


        private string WebForm(string key, string regex)
        {
            string value = WebUtil.FormTrim(key);

            bool valid = ValidateUtil.Regex(value, regex);

            if(!valid)
            {
                throw new Exception($"表单参数错误: key={key}, value={value}");
            }

            return value;
        }


        /// <summary>
        /// 删除线
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HttpResult RemoveLine(HttpContext context, SModel data)
        {
            string opBatchCode = BillIdentityMgr.NewCodeForDay("OP_BATCH_CODE", "OBC-", "-");
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01

            string item_code = data.Get<string>("item_code");   //line_1
            


            DbDecipher decipher = ModelAction.OpenDecipher();

            BizDecipher bizDecipher = new BizDecipher(decipher);

            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");

            AC3_DWG_LINE line = decipher.SelectToOneModel<AC3_DWG_LINE>($"ROW_SID >= 0 and FK_DWG_CODE = '{dwg_code}' and PK_LINE_CODE='{item_code}'");

            bizDecipher.BatchCode = opBatchCode;
            bizDecipher.BizDeleteModel(line);
            

            return HttpResult.SuccessMsg("删除成功");
        }


        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private HttpResult RemoveNode(HttpContext context, SModel data)
        {
            string opBatchCode = BillIdentityMgr.NewCodeForDay("OP_BATCH_CODE", "OBC-", "-");

            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01

            string item_code = data.Get<string>("item_code");   //line_1

            DbDecipher decipher = ModelAction.OpenDecipher();


            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");



            AC3_DWG_NODE node = decipher.SelectToOneModel<AC3_DWG_NODE>($"ROW_SID >= 0 and FK_DWG_CODE = '{dwg_code}' and PK_NODE_CODE='{item_code}'");

            node.ROW_SID = -3;
            node.ROW_DATE_DELETE = DateTime.Now;
            node.ROW_DELETE_BATCH_CODE = opBatchCode;



            string nodeTSqlWhere = $"ROW_SID >= 0 and FK_DWG_CODE = '{dwg_code}' and FK_NODE_CODE='{item_code}'";
            object[] fields = new object[] {
                "ROW_SID",-3,
                "ROW_DATE_DELETE" , DateTime.Now,
                "ROW_DELETE_BATCH_CODE", opBatchCode
            };

            decipher.BeginTransaction();

            try
            {
                decipher.Locks.Add(LockType.RowLock);
                decipher.UpdateModelProps(node, "ROW_SID", "ROW_DATE_DELETE", "ROW_DELETE_BATCH_CODE");


                decipher.Locks.Add(LockType.RowLock);

                if (node.EX_NODE_TYPE == "listen_table")
                {
                    decipher.UpdateProps<AC3_LISTEN_TABLE>(nodeTSqlWhere, fields);
                }
                else if (node.EX_NODE_TYPE == "operate_table")
                {
                    decipher.UpdateProps<AC3_OPERATE_TABLE>(nodeTSqlWhere, fields);
                }

                decipher.TransactionCommit();
                
                return HttpResult.SuccessMsg("删除成功");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error($"删除节点失败! FK_DWG_CODE = '{dwg_code}' and FK_NODE_CODE='{item_code}'", ex);

                return HttpResult.Error("删除失败");
            }

            
        }




        /// <summary>
        /// 获取定义
        /// </summary>
        /// <returns></returns>
        private HttpResult GetDwg(HttpContext context)
        {
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01

            DbDecipher decipher = ModelAction.OpenDecipher();

            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");

            List<AC3_DWG_LINE> def_lines = decipher.SelectModels<AC3_DWG_LINE>($"ROW_SID >= 0 and FK_DWG_CODE = '{dwg_code}'");
            List<AC3_DWG_NODE> def_nodes = decipher.SelectModels<AC3_DWG_NODE>($"ROW_SID >= 0 and FK_DWG_CODE = '{dwg_code}'");



            SModel data = new SModel();


            SModelList items = new SModelList();

            foreach (var item in def_nodes)
            {
                if (item.NODE_TYPE == "note")
                {
                    item.G_FULLNAME = "Mini2.flow.ac3.Note";
                }
                else
                {
                    item.G_FULLNAME = "Mini2.flow.ac3.Node";
                }


                SModel sm = SModel.ParseJson(item.GRAPHICS);
                sm["item_fullname"] = item.G_FULLNAME;

                sm["id"] = item.PK_NODE_CODE;
                sm["node_id"] = item.PK_NODE_CODE;
                sm["item_code"] = item.PK_NODE_CODE;

                sm["ex_node_type"] = item.EX_NODE_TYPE;

                sm["text"] = item.NODE_TEXT;
                sm["style_name"] = item.STYLE_NAME;

                items.Add(sm);
            }

            foreach (var item in def_lines)
            {
                //if (StringUtil.IsBlank(item.G_FULLNAME))
                //{
                //    item.G_FULLNAME = "Mini2.flow.ac3.Line";
                //}
                item.G_FULLNAME = "Mini2.flow.ac3.Line";


                SModel sm = SModel.ParseJson(item.GRAPHICS);
                sm["item_fullname"] = item.G_FULLNAME;
                sm["item_code"] = item.PK_LINE_CODE;
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
            AC3_DWG dwg = new AC3_DWG();

            dwg.PK_DWG_CODE = BizCommon.BillIdentityMgr.NewCodeForDay("AC3_DWG_CODE", "DWG-", "-", 2);
            dwg.DWG_TEXT = "联动图_" + dwg.PK_DWG_CODE;


            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.InsertModel(dwg);

            
            return HttpResult.Success(new
            {
                DWG_CODE = dwg.PK_DWG_CODE,
                DWG_TEXT = dwg.DWG_TEXT
            });
        }

        /// <summary>
        /// 新建线
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HttpResult NewLine(HttpContext context)
        {

            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01

            string node_fullname = "Mini2.flow.ac3.Line";


            DbDecipher decipher = ModelAction.OpenDecipher();

            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");

            int newId = BizCommonMgr.NewIdentity("AC3_DWG", "N_LINE_IDENTITY", $"PK_DWG_CODE='{dwg_code}'");
            
            string newLineId = $"line_{newId}";

            SModel g = new SModel();
            
            g["line_code"] = newLineId;
            g["item_type"] = "line";
            g["item_fullname"] = node_fullname;

            AC3_DWG_LINE line = new AC3_DWG_LINE();
            line.ROW_SID = -1;
            line.FK_DWG_CODE = dwg.PK_DWG_CODE;
            line.PK_LINE_CODE = newLineId;
            line.LINE_IDENTIFIER = newLineId;
            line.LINE_TEXT = newLineId;
            line.GRAPHICS = g.ToJson();

            decipher.InsertModel(line);

                        

            return HttpResult.Success(new {
                dwg_code= dwg_code,
                item_code = line.PK_LINE_CODE,
                item_fullname= node_fullname
            });
        }

        /// <summary>
        /// 新建节点
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HttpResult NewNode(HttpContext context)
        {
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01

            string node_type = WebUtil.Form("node_type");
            string ex_node_type = WebUtil.Form("ex_node_type");

            string node_fullname;

            if (node_type == "note")
            {
                node_fullname = "Mini2.flow.ac3.Note";
            }
            else
            {
                node_fullname = "Mini2.flow.ac3.Node";
            }



            DbDecipher decipher = ModelAction.OpenDecipher();


            int newId = BizCommonMgr.NewIdentity("AC3_DWG", "N_NODE_IDENTITY", $"PK_DWG_CODE='{dwg_code}'");

            string newNodeId;

            if (ex_node_type == "listen_table")
            {
                newNodeId = $"lt_{newId}";
            }
            else if(ex_node_type == "operate_table")
            {
                newNodeId = $"op_{newId}";
            }
            else
            {
                newNodeId = $"{node_type}_{newId}";
            }


            //{"item_type":"node", "item_fullname":"Mini2.flow.Action", "item_id":44, "id":"node_3", "text":"节点", "x":806, "y":84, "width":92, "height":75}
            SModel g = new SModel();

            g["node_id"] = newNodeId;
            g["node_code"] = newNodeId;
            g["text"] = newNodeId;
            g["item_type"] = "node";
            g["width"] = 80;
            g["height"] = 30;

            g["item_fullname"] = node_fullname;



            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");

            AC3_DWG_NODE node = new AC3_DWG_NODE();
            node.ROW_SID = -1;
            node.FK_DWG_CODE = dwg.PK_DWG_CODE;

            node.PK_NODE_CODE = newNodeId;
            node.NODE_IDENTIFIER = newNodeId;

            node.NODE_TEXT = newNodeId;
            node.NODE_TYPE = node_type;

            node.EX_NODE_TYPE = ex_node_type;

            node.G_FULLNAME = node_fullname;

            node.GRAPHICS = g.ToJson();



            decipher.InsertModel(node);

            if (ex_node_type == "listen_table")
            {
                AC3_LISTEN_TABLE listen = new AC3_LISTEN_TABLE();
                listen.ROW_SID = -1;
                listen.FK_DWG_CODE = dwg.PK_DWG_CODE;
                listen.FK_NODE_CODE = newNodeId;
                listen.PK_LISTEN_CODE = newNodeId;

                decipher.InsertModel(listen);
            }
            else if (ex_node_type == "operate_table")
            {
                AC3_OPERATE_TABLE operate = new AC3_OPERATE_TABLE();
                operate.ROW_SID = -1;
                operate.FK_DWG_CODE = dwg.PK_DWG_CODE;
                operate.FK_NODE_CODE = newNodeId;
                operate.PK_OPERATE_CODE = newNodeId;

                decipher.InsertModel(operate);
            }

            

            return HttpResult.Success(new {
                node_id = node.PK_NODE_CODE,
                node_code = node.PK_NODE_CODE,
                item_fullname = node_fullname,
                ex_node_type = ex_node_type
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



            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01

            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");


            foreach (SModel item in def_data)
            {

                string item_type = item.Get<string>("item_type");

                string item_code = item.Get<string>("item_code");


                if (item_type == "line")
                {
                    SaveLine(decipher, dwg_code, item_code, item);
                }
                else if (item_type == "node")
                {
                    SaveNode(decipher, dwg_code, item_code, item);
                }

            }

            
            return HttpResult.Success();
        }


        private void SaveNode(DbDecipher decipher, string dwg_code, string item_code,  SModel item)
        {
            string itemJson = item.ToJson();

            var node = decipher.SelectToOneModel<AC3_DWG_NODE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and PK_NODE_CODE='{item_code}'");
            node.SetTakeChange(true);

            node.STYLE_NAME = (string)item["style_name"];
            node.GRAPHICS = itemJson;
            
            node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModel(node, true);
        }


        private void SaveLine(DbDecipher decipher, string dwg_code, string item_code,  SModel item)
        {
            AC3_DWG_NODE fromNode = null;
            AC3_DWG_NODE toNode = null;

            SModel start_point = item["start_point"] as SModel;
            SModel end_point = item["end_point"] as SModel;

            if (start_point != null)
            {
                string fromCode = start_point.Get<string>("item_code");
                fromNode = decipher.SelectToOneModel<AC3_DWG_NODE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and PK_NODE_CODE='{fromCode}'");

                
            }

            if (end_point != null)
            {
                string toCode = end_point.Get<string>("item_code");
                toNode = decipher.SelectToOneModel<AC3_DWG_NODE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and PK_NODE_CODE='{toCode}'");
            }

            

            
            var line = decipher.SelectToOneModel<AC3_DWG_LINE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and PK_LINE_CODE='{item_code}'");

            line.SetTakeChange(true);
            
            line.FROM_NODE_CODE = fromNode?.PK_NODE_CODE;
            line.FROM_NODE_TEXT = fromNode?.NODE_TEXT;
                                   
            line.TO_NODE_CODE = toNode?.PK_NODE_CODE;
            line.TO_NODE_TEXT = toNode?.NODE_TEXT;



            line.G_FULLNAME = (string)item["item_fullname"];
            line.STYLE_NAME = (string)item["style_name"];
            line.GRAPHICS = item.ToJson(); ;

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
            else if (item_type == "node" || item_type == "action")
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
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01
            string item_code = data.Get<string>("item_code");


            DbDecipher decipher = ModelAction.OpenDecipher();



            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");

            
            AC3_DWG_NODE node = decipher.SelectToOneModel<AC3_DWG_NODE>($"ROW_SID = -1 and FK_DWG_CODE='{dwg_code}' and PK_NODE_CODE='{item_code}'");

            if (node == null)
            {
                return HttpResult.Error();
            }

            node.ROW_SID = 0;
            node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(node, "ROW_SID", "ROW_DATE_UPDATE");


            string nodeTSqlWhere = $"ROW_SID = -1 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'";

            if (node.EX_NODE_TYPE == "listen_table")
            {
                decipher.UpdateProps<AC3_LISTEN_TABLE>(nodeTSqlWhere, new object[] {
                    "ROW_SID",0,
                    "ROW_DATE_UPDATE" , DateTime.Now
                });
            }
            else if (node.EX_NODE_TYPE == "operate_table")
            {
                decipher.UpdateProps<AC3_OPERATE_TABLE>(nodeTSqlWhere, new object[] {
                    "ROW_SID",0,
                    "ROW_DATE_UPDATE" , DateTime.Now
                });
            }

            
            return HttpResult.Success();
        }


        private HttpResult NewLienEnabled(HttpContext context, SModel data)
        {
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT); //DWG-060925-01
            string item_code = data.Get<string>("item_code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            
            AC3_DWG dwg = decipher.SelectToOneModel<AC3_DWG>($"ROW_SID >= 0 and PK_DWG_CODE='{dwg_code}'");

            AC3_DWG_LINE line = decipher.SelectToOneModel<AC3_DWG_LINE>($"ROW_SID = -1 and FK_DWG_CODE='{dwg_code}' and PK_LINE_CODE='{item_code}'");

            if (line == null)
            {
                return HttpResult.Error();
            }

            line.ROW_SID = 0;
            line.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(line, "ROW_SID", "ROW_DATE_UPDATE");

            
            return HttpResult.Success() ;
        }


        private HttpResult SaveListenFilter(HttpContext context)
        {
            string json = WebUtil.Form("data");
            SModel data = SModel.ParseJson(json);

            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT);
            string item_code = WebUtil.Form("item_code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            var node = decipher.SelectToOneModel<AC3_LISTEN_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'");

            node.COND_SCRIPT_JSON = data.ToJson();
            node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(node, "COND_SCRIPT_JSON", "ROW_DATE_UPDATE");

            
            return HttpResult.Success();
        }

        /// <summary>
        /// 获取监听值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetListenFilter(HttpContext context)
        {
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT);
            string item_code = WebUtil.Form("item_code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            var node = decipher.SelectToOneModel<AC3_LISTEN_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'");
            

            return HttpResult.Success(node.COND_SCRIPT_JSON);
        }



        /// <summary>
        /// 保存值变化的脚本
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HttpResult SaveListenVChange(HttpContext context)
        {
            string json = WebUtil.Form("data");
            SModel data = SModel.ParseJson(json);

            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT);
            string item_code = WebUtil.Form("item_code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            var node = decipher.SelectToOneModel<AC3_LISTEN_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'");

            node.VCHANGE_SCRIPT_JSON = data.ToJson();
            node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(node, "VCHANGE_SCRIPT_JSON", "ROW_DATE_UPDATE");


            return HttpResult.Success();
        }


        /// <summary>
        /// 值变化的脚本
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetListenVChange(HttpContext context)
        {

            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT);
            string item_code = WebUtil.Form("item_code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            var node = decipher.SelectToOneModel<AC3_LISTEN_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'");
            


            return HttpResult.Success(node.VCHANGE_SCRIPT_JSON);

        }



        private HttpResult SaveOperateFilter(HttpContext context)
        {
            string json = WebUtil.Form("data");

            SModel data = SModel.ParseJson(json);

            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT);
            string item_code = WebForm("item_code", ITEM_CODE_FORMAT);


            DbDecipher decipher = ModelAction.OpenDecipher();

            var node = decipher.SelectToOneModel<AC3_OPERATE_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'");

            node.COND_SCRIPT_JSON = data.ToJson();
            node.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(node, "COND_SCRIPT_JSON", "ROW_DATE_UPDATE");
            
            return HttpResult.Success();
        }

        /// <summary>
        /// 获取监听值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResult GetOperateFilter(HttpContext context)
        {
            string dwg_code = WebForm("dwg_code", DWG_CODE_FORMAT);
            string item_code = WebForm("item_code", ITEM_CODE_FORMAT);


            DbDecipher decipher = ModelAction.OpenDecipher();

            var node = decipher.SelectToOneModel<AC3_OPERATE_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg_code}' and FK_NODE_CODE='{item_code}'");
            
            return HttpResult.Success(node.COND_SCRIPT_JSON) ;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}