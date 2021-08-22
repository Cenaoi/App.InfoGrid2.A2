using EC5.Action3.Steps;
using EC5.AppDomainPlugin;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.Processors
{

    /// <summary>
    /// 数据库处理机
    /// </summary>
    public class DatabaseProcessor
    {
        DrawingLibrary m_Library;
        

        DbDecipher m_MainDecipher;


        public DrawingLibrary Library
        {
            get { return m_Library; }
            set { m_Library = value; }
        }


        /// <summary>
        /// 主数据操作类
        /// </summary>
        public DbDecipher MainDecipher
        {
            get { return m_MainDecipher; }
            set { m_MainDecipher = value; }
        }




        /// <summary>
        /// 过滤通过的监听条件
        /// </summary>
        /// <param name="listenList"></param>
        /// <returns></returns>
        public List<ListenTable> FilterListenPassList(List<ListenTable> listenList, LightModel model)
        {
            //寻找通过条件的监听事件
            List<ListenTable> listenPassList = new List<ListenTable>();    //条件通过的

            foreach (ListenTable listen in listenList)
            {
                Filter filte = listen.CondFilter;

                bool valid = filte.Valid(model);

                if (valid)
                {
                    listenPassList.Add(listen);
                }
            }

            return listenPassList;
        }
        

        private void SetValue(OperateField opField, LModelElement curModelElem,LModelFieldElement curFieldElem,LightModel curModel, 
            LModelElement firstModelElem, LightModel firstModel)
        {
            object value = null;

            string field = opField.Code;

            if (opField.ValueMode == ActionModeType.Fixed)
            {
                value = opField.Value;

                try
                {
                    curModel[field] = ModelConvert.ChangeType(value, curFieldElem);
                }
                catch (Exception ex)
                {
                    throw new Exception($"(固定值)字段赋值错误. field={opField.Code}, value={value}", ex);
                }
            }
            else
            {
                try
                {
                    ScriptInstance inst = ScriptFactory.Create("return " + opField.Value + ";");

                    inst.Params[firstModelElem.DBTableName] = new DynModel(firstModel);

                    value = inst.Exec(curModel);

                    curModel[field] = ModelConvert.ChangeType(value, curFieldElem);
                }
                catch (Exception ex)
                {
                    throw new Exception($"(函数值)字段赋值错误. field={opField.Code}, value={value}", ex);
                }
            }
        }

        private void SetNewValues(OperateTable op, LightModel firstModel, string newTable, LModel newModel)
        {
            LModelElement newModelElem = newModel.GetModelElement();
            LModelElement firstModelElem = ModelElemHelper.GetElem(firstModel);

            LModelFieldElement fieldElem;

            foreach (OperateField opField in op.NewFields)
            {
                fieldElem = newModelElem.Fields[opField.Code];

                SetValue(opField, newModelElem, fieldElem, newModel, firstModelElem, firstModel);
            }
        }



        private void SetUpdateValues(OperateTable op, LightModel curModel, LModelElement firstModelElem, LightModel firstModel)
        {
            LModelElement curModelElem = ModelElemHelper.GetElem( curModel);
            LModelFieldElement fieldElem;

            foreach (OperateField opField in op.UpdateFields)
            {
                fieldElem = curModelElem.Fields[opField.Code];

                SetValue(opField, curModelElem, fieldElem, curModel, firstModelElem, firstModel);

            }
        }




        private object GetFilterFieldValue( FilterField field, LModelElement curModelElem, LModelElement firstModelElem, LightModel firstModel)
        {
            ActionModeType valueModel = field.ValueMode;

            LModelFieldElement fieldElem = curModelElem.Fields[field.Name];

            object result = null;

            if(valueModel == ActionModeType.Fixed)
            {
                result = ModelConvert.ChangeType(field.Value, fieldElem);
            }
            else if(valueModel == ActionModeType.Fun)
            {
                string tabName = firstModelElem.DBTableName;

                ScriptInstance inst = ScriptFactory.Create("return " + field.Value + ";");

                inst.Params[tabName] = new DynModel(firstModel);

                object value = inst.Exec();

                result = ModelConvert.ChangeType(value, fieldElem);
            }

            return result;
        }


        private void GetTSqlWhere(StringBuilder sb, FilterGroup group, LModelElement curModelElem, string filterTable, LModelElement firstModelElem, LightModel firstModel)
        {

            int n = 0;

            foreach (FilterItem item in group.Items)
            {
                if (n++ > 0)
                {
                    sb.Append(group.Condition == FilterCondition.And ? " and " : " or ");
                }


                if (item is FilterGroup)
                {
                    FilterGroup fg = (FilterGroup)item;

                    sb.Append("(");

                    GetTSqlWhere(sb, fg, curModelElem, filterTable, firstModelElem, firstModel);

                    sb.Append(")");
                    
                }
                else  if (item is FilterField)
                {
                    FilterField field = (FilterField)item;


                    LModelFieldElement fieldElem = curModelElem.Fields[field.Name];

                    object value = GetFilterFieldValue(field, curModelElem, firstModelElem, firstModel);

                    sb.Append(field.Name);

                    if (value == null)
                    {
                        if(field.Logic == "<>" || field.Logic == "!=" )
                        {
                            sb.Append(" is not ");
                        }
                        else 
                        {
                            sb.Append(" is ");
                        }

                        sb.Append("null");
                    }
                    else
                    {
                        string logic = field.Logic;
                        
                        sb.Append(logic);

                        if (fieldElem.IsNumber)
                        {
                            sb.Append(value);
                        }
                        else if(fieldElem.DBType == LMFieldDBTypes.Boolean)
                        {
                            bool valueBool = (bool)value;

                            sb.Append(valueBool ? 1 : 0);
                        }
                        else
                        {
                            string valueStr = value.ToString().Replace("'", "''");

                            sb.Append($"'{valueStr}'");
                        }
                    }

                }
                else
                {
                    throw new Exception($"位置类型:{item}");
                }
            }
        }

        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="firstModel"></param>
        /// <returns></returns>
        private string GetTSqlWhere(OperateTable op, Filter filter,  LightModel firstModel)
        {

            LModelElement firstModelElem = ModelElemHelper.GetElem(firstModel);  //  当前过滤表
            LModelElement curModelElem = LightModel.GetLModelElement(op.Table);

            StringBuilder sb = new StringBuilder();

            GetTSqlWhere(sb, filter, curModelElem, firstModelElem.DBTableName, firstModelElem, firstModel);


            return sb.ToString();
        }

        private void ExecOperateTable(StepNode step, OperateTable op, LightModel firstModel)
        {

            if (op.Method == OperateMethod.Insert)
            {
                LModelElement newModelElem = LModelDna.GetElementByName(op.Table);

                LModel mm = new LModel(newModelElem);

                SetNewValues(op, firstModel, newModelElem.DBTableName, mm);

                this.MainDecipher.InsertModel(mm);

                step.OperateData = mm;

            }
            else if(op.Method == OperateMethod.Update)
            {
                LModelElement firstModelElem = ModelElemHelper.GetElem(firstModel);
                

                LightModelFilter filter = new LightModelFilter(op.Table);

                filter.TSqlWhere = GetTSqlWhere(op, op.Filter, firstModel);


                LModelList<LModel> models = this.MainDecipher.GetModelList(filter);


                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    SetUpdateValues(op, model, firstModelElem, firstModel);

                    this.MainDecipher.UpdateModel(model, true);
                }


                step.OperateData = models;

            }
            else if(op.Method == OperateMethod.Delete)
            {
                LModelElement firstModelElem = ModelElemHelper.GetElem(firstModel);
                

                LightModelFilter filter = new LightModelFilter(op.Table);

                filter.TSqlWhere = GetTSqlWhere(op, op.Filter, firstModel);


                LModelList<LModel> models = this.MainDecipher.GetModelList(filter);


                foreach (LModel model in models)
                {
                    this.MainDecipher.DeleteModel(model);
                }


                step.OperateData = models;
            }

        }


        private ListenMethod Conver(OperateMethod opMethod)
        {
            ListenMethod liMethod = ListenMethod.All;

            if (opMethod == OperateMethod.Insert)
            {
                liMethod = ListenMethod.Insert;
            }
            else if (opMethod == OperateMethod.Update)
            {
                liMethod = ListenMethod.Update;
            }
            else if (opMethod == OperateMethod.Delete)
            {
                liMethod = ListenMethod.Delete;
            }

            return liMethod;
        }



        /// <summary>
        /// 处理未监听
        /// </summary>
        /// <param name="context"></param>
        private void ProListen(StepContext context)
        {
            StepNode cur = context.CurNode;

            cur.Status = StepStatus.Running;

            ListenTable listen = cur.ActionItem as ListenTable;


            Filter filte = listen.CondFilter;

            object source = cur.Source;

            if (source is LightModel)
            {
                bool valid = filte.Valid((LightModel)source);

                if (!valid)
                {
                    cur.Status = StepStatus.End;

                    return;
                }

                if (listen.Routes.Count == 0)
                {
                    cur.Status = StepStatus.End;

                    return;
                }


                RouteItem route = listen.Routes[0];

                OperateTable opTab = route.TargetNode as OperateTable;

                StepNode newStep = new StepNode(StepNodeType.Operate, opTab);
                newStep.Source = source;

                cur.Childs.Add(newStep);
            }

            cur.Status = StepStatus.End;
        }

        /// <summary>
        /// 处理监听结束
        /// </summary>
        /// <param name="context"></param>
        private void ProListenEnd(StepContext context)
        {
            StepNode cur = context.CurNode;

            ListenTable listen = cur.ActionItem as ListenTable;

            Filter filte = listen.CondFilter;

            object source = cur.Source;

            if (source is LightModel)
            {
                bool valid = filte.Valid((LightModel)source);

                if (!valid)
                {
                    cur.Status = StepStatus.End;
                    return;
                }

                if (!listen.HasRoute())
                {
                    cur.Status = StepStatus.End;
                    return;
                }


                RouteItem route = listen.Routes[0];

                OperateTable opTab = route.TargetNode as OperateTable;

                StepNode newStep = new StepNode(StepNodeType.Operate, opTab);
                newStep.Source = source;

                cur.Childs.Add(newStep);
            }

        }


        private void ProOperate(StepContext context)
        {
            StepNode cur = context.CurNode;

            cur.Status = StepStatus.Running;

            OperateTable opTable = cur.ActionItem as OperateTable;

            object source = cur.Source;

            if (source is LightModel)
            {
                ExecOperateTable(cur, opTable, (LightModel)source);


                //创建路由节点
                if (opTable.HasRoute())
                {
                    RouteItem route = opTable.Routes[0];

                    StepNode nextNode = new StepNode(StepNodeType.Operate, route.TargetNode);
                    nextNode.Source = cur.OperateData;

                    StepNode parent = (StepNode)cur.Parent;
                    parent.Childs.Add(nextNode);
                }

            }
            
            cur.Status = StepStatus.End;
        }

        private void ProOperateEnd(StepContext context)
        {
            StepNode cur = context.CurNode;

            OperateTable opTable = cur.ActionItem as OperateTable;

            object opData = cur.OperateData;

            if (opData != null)
            {
                IList dataList = null;

                if (opData is IList)
                {
                    dataList = (IList)opData;
                }
                else
                {
                    dataList = new ArrayList() { opData };
                }

                int count = dataList.Count;



                //判断是否有监听
                ListenMethod liMethod = Conver(opTable.Method);
                List<ListenTable> listenList = GetListenItems(opTable.Table, liMethod);


                if (listenList.Count > 0 && count > 0)
                {
                    foreach (var item in dataList)
                    {
                        StepNode group = new StepNode(StepNodeType.Group);

                        cur.Childs.Add(group);

                        foreach (ListenTable listen in listenList)
                        {
                            StepNode step = new StepNode(StepNodeType.Listen, listen);

                            step.Source = item;

                            group.Childs.Add(step);
                        }
                    }


                }

            }
        }


        private void ProGroupStart(StepContext context)
        {
            StepNode cur = context.CurNode;

            cur.Status = StepStatus.Running;




        }

        private void ProGroupRunngin(StepContext context)
        {

        }

        private void ProGroupEnd(StepContext context)
        {

        }


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="model"></param>
        /// <param name="method"></param>
        public void Exec(LightModel model, OperateMethod method)
        {
            if (model == null) { throw new ArgumentNullException("model"); }



            LModelElement modelElem = ModelElemHelper.GetElem(model);
            string table = modelElem.DBTableName;

            StepContext context = new StepContext();


            //创建一个操作的触发源.
            OperateTable opRoot = new OperateTable("_ROOT", method);
            opRoot.Table = table;
           

            StepNode root = new StepNode(StepNodeType.Operate, opRoot);
            root.OperateData = model;

            root.Status = StepStatus.End;

            context.RootNode = root;

            

            //开始执行, 并计算路径
            context.Path.Push(root);
            context.CurNode = root;

            while (true)
            {
                Read(context);

                if (context.Path.Count == 0)
                {
                    break;
                }


            }




        }

        private void Read(StepContext context)
        {
            StepNode cur = context.CurNode;

            if (!cur.IsPass)
            {
                if (cur.StepType == StepNodeType.Group)
                {

                    if (cur.Status == StepStatus.None)
                    {
                        ProGroupStart(context);
                    }
                    else if(cur.Status == StepStatus.Running)
                    {
                        ProGroupRunngin(context);
                    }
                    else if (cur.Status == StepStatus.End)
                    {
                        ProGroupEnd(context);
                    }
                }
                else if (cur.StepType == StepNodeType.Listen)
                {
                    if (cur.Status == StepStatus.None)
                    {
                        ProListen(context);
                    }
                    else if (cur.Status == StepStatus.End)
                    {
                        ProListenEnd(context);
                    }
                }
                else if (cur.StepType == StepNodeType.Operate)
                {
                    if (cur.Status == StepStatus.None)
                    {
                        ProOperate(context);
                    }
                    else if (cur.Status == StepStatus.End)
                    {
                        ProOperateEnd(context);
                    }
                }
            }


            if (cur.Status == StepStatus.End)
            {
                StepNode next = (StepNode)cur.Next;
                StepNode first = (StepNode)cur.FirstChild;

                //获取下一个节点
                if (next != null && next.Status == StepStatus.None)
                {
                    cur.IsPass = true;

                    cur = next;

                    context.CurNode = cur;
                    context.Path.Push(cur);
                }
                else if (first != null && first.Status == StepStatus.None)
                {
                    cur.IsPass = true;

                    cur = first;

                    context.CurNode = cur;
                    context.Path.Push(cur);
                }
                else
                {
                    StepNode tmpCur = null;
                    context.Path.TryPop(out tmpCur);

                    context.CurNode = (StepNode)tmpCur.Parent;
                }
            }

            
        }

    }
}
