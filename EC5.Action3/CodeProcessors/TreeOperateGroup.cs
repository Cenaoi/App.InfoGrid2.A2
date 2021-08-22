using EC5.Action3.SEngine;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EC5.Action3.CodeProcessors
{

    /// <summary>
    /// 操作组
    /// 1. 操作; 当全部操作完成后, 发出监听
    /// </summary>
    public class TreeOperateGroup : TreeNode
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 运行次数
        /// </summary>
        int m_ExecCount = 0;


        public TreeOperateGroup()
        {

        }





        int m_OperateIndex = 0;

        List<TreeOperateItem> m_Operates;

        /// <summary>
        /// 条件成立
        /// </summary>
        bool m_IsConditionsHold = true;



        public List<TreeOperateItem> Operates
        {
            get
            {
                if (m_Operates == null)
                {
                    m_Operates = new List<TreeOperateItem>();
                }

                return m_Operates;
            }
        }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExecCount
        {
            get { return m_ExecCount; }
        }


        /// <summary>
        /// 条件成立(默认成立)
        /// </summary>
        /// <returns></returns>
        public bool IsConditionsHold()
        {
            return m_IsConditionsHold;
        }


        #region 执行过程


        #region 赋值

        private void SetValue(OperateField opField, LModelElement curModelElem, LModelFieldElement curFieldElem, LightModel curModel)
        {
            ActionModeType valueModel = opField.ValueMode;

            object value = null;

            string field = opField.Code;

            if (valueModel == ActionModeType.Fixed)
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
            else if(valueModel == ActionModeType.Fun )
            {                
                ScriptInstance inst = ScriptFactory.Create(opField.Value);

                foreach (var item in this.Context.CurParams.GetFields())
                {
                    inst.Params[item] = this.Context.CurParams[item];
                }
                                

                try
                {
                    value = inst.Exec(curModel);
                }
                catch (Exception ex)
                {
                    throw new Exception($"(函数值)字段赋值错误. field={opField.Code}, value={value}", ex);
                }

                curModel[field] = ModelConvert.ChangeType(value, curFieldElem);
            }
        }

        private void SetNewValues(OperateTable op, string newTable, LModel newModel)
        {
            LModelElement newModelElem = newModel.GetModelElement();

            LModelFieldElement fieldElem;

            foreach (OperateField opField in op.NewFields)
            {
                fieldElem = newModelElem.Fields[opField.Code];

                SetValue(opField, newModelElem, fieldElem, newModel);
            }
        }
        
        private void SetUpdateValues(OperateTable op, LightModel curModel)
        {
            LModelElement curModelElem = ModelElemHelper.GetElem(curModel);
            LModelFieldElement fieldElem;

            foreach (OperateField opField in op.UpdateFields)
            {
                fieldElem = curModelElem.Fields[opField.Code];

                SetValue(opField, curModelElem, fieldElem, curModel);

            }
        }

        


        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <param name="op"></param>
        /// <param name="filter"></param>
        /// <param name="firstModel"></param>
        /// <returns></returns>
        private string GetTSqlWhere(OperateTable op, FilterItem filter)
        {
            

            LModelElement curModelElem = LightModel.GetLModelElement(op.Table);

            StringBuilder sb = new StringBuilder();

            if (filter is FilterGroup)
            {
                FilterGroup filterGroup = (FilterGroup)filter;
                
                try
                {
                    GetTSqlWhere(sb, filterGroup, curModelElem);
                }
                catch(Exception ex)
                {
                    throw new Exception("获取过滤 '条件组' 错误.", ex);
                }
            }

            return sb.ToString();
        }





        /// <summary>
        /// 获取过滤字段值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="curModelElem"></param>
        /// <param name="firstModelElem"></param>
        /// <param name="firstModel"></param>
        /// <returns></returns>
        private object GetFilterFieldValue(FilterField field, LModelElement curModelElem)
        {
            ActionModeType valueModel = field.Mode;

            LModelFieldElement fieldElem = curModelElem.Fields[field.Name];

            object result = null;

            if (valueModel == ActionModeType.Fixed)
            {
                result = ModelConvert.ChangeType(field.Value, fieldElem);
            }
            else if (valueModel == ActionModeType.Fun)
            {
                ScriptInstance inst = ScriptFactory.Create((string)field.Value);

                foreach (var item in this.Context.CurParams.GetFields())
                {
                    inst.Params[item] = this.Context.CurParams[item];
                }

                object value;

                try
                {
                    value = inst.Exec();
                }
                catch (Exception ex)
                {
                    throw new Exception("执行动态代码错误, code=\n" + field.Value, ex);
                }
                finally
                {
                    inst.Dispose();
                }

                result = ModelConvert.ChangeType(value, fieldElem);
            }

            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="group"></param>
        /// <param name="curModelElem"></param>
        /// <param name="filterTable"></param>
        /// <param name="firstModelElem"></param>
        /// <param name="firstModel"></param>
        private void GetTSqlWhere(StringBuilder sb, FilterGroup group, LModelElement curModelElem)
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

                    GetTSqlWhere(sb, fg, curModelElem);

                    sb.Append(")");

                }
                else if (item is FilterField)
                {
                    FilterField field = (FilterField)item;


                    LModelFieldElement fieldElem = curModelElem.Fields[field.Name];

                    object value = null;

                    try
                    {
                        value = GetFilterFieldValue(field, curModelElem);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("获取值错误.", ex);
                    }

                    sb.Append(field.Name);

                    if (value == null)
                    {
                        if (field.Logic == "<>" || field.Logic == "!=")
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
                        string logic = StringUtil.NoBlank(field.Logic, "=");

                        sb.Append(logic);

                        if (fieldElem.IsNumber)
                        {
                            sb.Append(value);
                        }
                        else if (fieldElem.DBType == LMFieldDBTypes.Boolean)
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




        #endregion


        private void CallInsertingScript(CodeContext context, OperateTable op, object srcData)
        {
            ScriptBase sBase = op.InsertingScript;

            if (sBase == null)
            {
                return;
            }

            if (sBase is ScriptCSharp)
            {
                ScriptCSharp script = (ScriptCSharp)sBase;
                
                ScriptInstance inst = ScriptFactory.Create(script.Code);

                foreach (var field in context.CurParams.GetFields())
                {
                    inst.Params[field] = context.CurParams[field];
                }

                if (srcData is LightModel)
                {
                    try
                    {
                        inst.Exec((LightModel)srcData);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("执行插入数据(LightModel)前事件错误", ex);
                    }
                }
                else if (srcData is IList)
                {
                    try
                    {
                        inst.Exec((IList)srcData);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("执行插入数据(IList)前事件错误", ex);
                    }
                }
            }
            else
            {
                throw new Exception("未知脚本类型.");
            }

        }

        private LModel ExecOperateTable_Insert(DbDecipher decipher, TreeOperateItem treeOpItem, OperateTable op)
        {

            LModelElement newModelElem = LModelDna.GetElementByName(op.Table);

            LModel mm = new LModel(newModelElem);
            
            SetNewValues(op, op.Table,mm);

            CallInsertingScript(this.Context, treeOpItem.Operate, mm);

            decipher.InsertModel(mm);

            return mm;
        }

        private LModelList<LModel> ExecOperateTable_Update(DbDecipher decipher, TreeOperateItem treeOpItem, OperateTable op)
        {
            
            LightModelFilter filter = new LightModelFilter(op.Table);


            try
            {
                if (op.Filter is JsonSQL.ScriptJQL)
                {
                    JsonSQL.ScriptJQL jql = (JsonSQL.ScriptJQL)op.Filter;

                    filter.TSqlWhere = GetTSqlWhere(op, jql.Where);

                    filter.Distinct = jql.Distinct;

                    filter.TSqlOrderBy = jql.GetOrderString();

                    filter.Limit = new Limit(jql.Limit.Count, jql.Limit.Start);
                }
                else
                {
                    filter.TSqlWhere = GetTSqlWhere(op, op.Filter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("构造 T-SQL Where 语句错误.", ex);
            }

            //filter.TSqlWhere = GetTSqlWhere(op, op.Filter);


            LModelList<LModel> models = decipher.GetModelList(filter);


            foreach (LModel model in models)
            {
                model.SetTakeChange(true);
                SetUpdateValues(op, model);
            }


            decipher.UpdateModels(models, true);

            return models;
        }

        private LModelList<LModel> ExecOperateTable_Select(DbDecipher decipher, TreeOperateItem treeOpItem,OperateTable op)
        {

            LightModelFilter filter = new LightModelFilter(op.Table);

            try
            {
                if (op.Filter is JsonSQL.ScriptJQL)
                {
                    JsonSQL.ScriptJQL jql = (JsonSQL.ScriptJQL)op.Filter;

                    filter.TSqlWhere = GetTSqlWhere(op, jql.Where);

                    filter.Distinct = jql.Distinct;

                    filter.TSqlOrderBy = jql.GetOrderString();

                    filter.Limit = new Limit(jql.Limit.Count, jql.Limit.Start);
                }
                else
                {
                    filter.TSqlWhere = GetTSqlWhere(op, op.Filter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("构造 T-SQL Where 语句错误.", ex);
            }

            LModelList<LModel> models = decipher.GetModelList(filter);
                        

            return models;
        }

        private LModelList<LModel> ExecOperateTable_Delete(DbDecipher decipher, TreeOperateItem treeOpItem, OperateTable op)
        {
            
            LightModelFilter filter = new LightModelFilter(op.Table);

            try
            {
                if (op.Filter is JsonSQL.ScriptJQL)
                {
                    JsonSQL.ScriptJQL jql = (JsonSQL.ScriptJQL)op.Filter;

                    filter.TSqlWhere = GetTSqlWhere(op, jql.Where);

                    filter.Distinct = jql.Distinct;

                    filter.TSqlOrderBy = jql.GetOrderString();

                    filter.Limit = new Limit(jql.Limit.Count, jql.Limit.Start);
                }
                else
                {
                    filter.TSqlWhere = GetTSqlWhere(op, op.Filter);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("构造 T-SQL Where 语句错误.", ex);
            }

            LModelList<LModel> models = decipher.GetModelList(filter);


            foreach (LModel model in models)
            {
                decipher.DeleteModel(model);
            }

            return models;
        }

        private void ExecOperateTable(TreeOperateItem treeOpItem, OperateTable op)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            CodeContext context = this.Context;

            DbDecipher decipher = context.Decipher;

            object opData = null;

            if (op.Method == OperateMethod.Insert)
            {
                opData = ExecOperateTable_Insert(decipher, treeOpItem, op);
            }
            else if (op.Method == OperateMethod.Update)
            {
                opData = ExecOperateTable_Update(decipher, treeOpItem, op);
            }
            else if (op.Method == OperateMethod.Delete)
            {
                opData = ExecOperateTable_Delete(decipher, treeOpItem, op);
            }
            else if(op.Method == OperateMethod.Select)
            {
                opData = ExecOperateTable_Select(decipher, treeOpItem, op);
            }
            else
            {
                throw new Exception($"未知操作...{op.Method}");
            }

            sw.Stop();

            treeOpItem.OperateData = opData;
            treeOpItem.ExecTime = sw.Elapsed;
        }


        #endregion

        /// <summary>
        /// 运行代码块
        /// </summary>
        /// <param name="i"></param>
        /// <param name="item"></param>
        private void RunBlock(int i, TreeOperateItem item)
        {
            CodeContext context = this.Context;

            OperateTable li = item.Operate;

            object srcData = item.SourceData;

            string code = item.Operate.Code;

            SModel sm = new SModel();
            sm["code"] = code;
            sm["role"] = "operate";
            sm["data"] = srcData;

            context.CurParams[code] = sm;


            if (li.Method == OperateMethod.Insert)
            {
                
            }
            else if(li.Method == OperateMethod.Update)
            {
                if(li.UpdatingScript != null)
                {

                }
            }
            else if(li.Method == OperateMethod.Delete)
            {
                if(li.DeletingScript != null)
                {

                }
            }


            
            ExecOperateTable(item, li);


            sm["data"] = item.OperateData;


            if (li.Method == OperateMethod.Insert)
            {
                if (li.InsertedScript != null)
                {

                }
            }
            else if (li.Method == OperateMethod.Update)
            {
                if (li.UpdatedScript != null)
                {

                }
            }
            else if (li.Method == OperateMethod.Delete)
            {
                if (li.DeletedScript != null)
                {

                }
            }



            //创建路由节点
            if (li.HasRoute())
            {

                RouteItem route = li.Routes[0];

                OperateTable opTable = route.TargetNode as OperateTable;

                TreeOperateItem nextItem = new TreeOperateItem
                {
                    Operate=opTable,
                    SourceData= item.OperateData
                };
                //(opTable, item.OperateData);

                this.Operates.Add(nextItem);
            }

        }

        public override bool Read(CodeContext context)
        {
            this.Context = context;

            if (this.Status == StepStatus.None)
            {
                this.Status = StepStatus.Running;
            }

            if (this.Status == StepStatus.Running)
            {
                ProStatus_Running();
            }


            if (this.Status == StepStatus.End)
            {
                ProStatus_End();
            }


            return (this.Status == StepStatus.End);
        }

        private void ProStatus_Running()
        {
            if (m_OperateIndex < m_Operates.Count)
            {

                TreeOperateItem item = m_Operates[m_OperateIndex];

                log.Debug($"code={item.Operate.Code},【操作】运行中");



                RunBlock(m_OperateIndex, item);

                m_OperateIndex++;

                m_ExecCount++;
            }

            if (m_OperateIndex >= m_Operates.Count)
            {
                this.Status = StepStatus.End;
            }
        }
        
        /// <summary>
        /// 处理结束的状态
        /// </summary>
        private void ProStatus_End()
        {
            DrawingLibrary lib = this.Context.Library;

            OperateTable opTable;
            object opData;

            IList dataList;

            List<ListenTable> listenList;

            foreach (var item in m_Operates)
            {
                opTable = item.Operate;
                opData = item.OperateData;

                if (!opTable.AutoContinue || opData == null)
                {
                    continue;
                }


                dataList = opData as IList;

                if (dataList == null)
                {
                    dataList = new ArrayList { opData };
                }
                                
                if(dataList.Count == 0)
                {
                    continue;
                }


                //判断是否有监听
                ListenMethod liMethod = Conver(opTable.Method);
                listenList = lib.GetListenItems(opTable.Table, liMethod);


                if (listenList.Count == 0 )
                {
                    continue;
                }



                foreach (var data in dataList)
                {
                    var groups = CreateGroups(listenList, data);

                    this.Childs.AddRange(groups);
                }

            }
        }

        private List<TreeListenGroup> CreateGroups(List<ListenTable> listenList, object data)
        {
            var groups = new List<TreeListenGroup>(listenList.Count);

            foreach (ListenTable listen in listenList)
            {
                var liGroup = new TreeListenGroup(new TreeListenItem
                {
                    Listen = listen,
                    SourceData = data
                });

                groups.Add(liGroup);
            }

            return groups;
        }


        private ListenMethod Conver(OperateMethod opMethod)
        {
            ListenMethod liMethod ;

            switch (opMethod)
            {
                case OperateMethod.Select: liMethod = ListenMethod.Select; break;
                case OperateMethod.Insert: liMethod = ListenMethod.Insert; break;
                case OperateMethod.Update: liMethod = ListenMethod.Update; break;
                case OperateMethod.Delete: liMethod = ListenMethod.Delete; break;
                default: liMethod = ListenMethod.All; break;
            }
                        
            return liMethod;
        }


    }

}
