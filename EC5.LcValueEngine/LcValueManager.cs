using EC5.Utility;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.LcValueEngine
{
    public class LcValueTableSorted
    {
        object m_Lock = new object();

        SortedList<string, LcValueTableCollection> m_Items = new SortedList<string, LcValueTableCollection>();

        public void Add(LcValueTable vTable)
        {
            string table = vTable.Table.ToUpper();

            LcValueTableCollection vTables;

            lock (m_Lock)
            {
                if (!m_Items.TryGetValue(table, out vTables))
                {
                    vTables = new LcValueTableCollection();

                    m_Items.Add(table, vTables);
                }

                vTables.Add(vTable);
            }
        }

        public void Clear()
        {
            lock (m_Lock)
            {
                m_Items.Clear();
            }
        }

        public LcValueTableCollection GetTableList(string table)
        {
            table = table.ToUpper();

            LcValueTableCollection vTables;

            lock (m_Lock)
            {
                m_Items.TryGetValue(table, out vTables);

                return vTables;
            }
        }

        
    }

    /// <summary>
    /// 事件数据
    /// </summary>
    public class LcValueEventArgs:EventArgs
    {
        LModel m_Model;

        string m_TargetField;
        string m_Code;

        public LcValueEventArgs()
        {
        }

        public LcValueEventArgs(string code, string targetField, LModel model)
        {
            m_Code = code;
            m_TargetField = targetField;
            m_Model = model;
        }

        /// <summary>
        /// 目标字段
        /// </summary>
        public string TargetField
        {
            get { return m_TargetField; }
        }

        /// <summary>
        /// 函数代码
        /// </summary>
        public string Code
        {
            get { return m_Code;}
        }

        /// <summary>
        /// 实体
        /// </summary>
        public LModel Model
        {
            get { return m_Model;}
        }
    }

    /// <summary>
    /// 动态代码管理
    /// </summary>
    public static class LcValueManager
    {
        static LcValueTableSorted m_Table = new LcValueTableSorted();

        public static LcValueTableSorted Table
        {
            get { return m_Table; }
            set { m_Table = value; }
        }

        public static LcValueTableCollection GetTables(string table)
        {
            return m_Table.GetTableList(table);
        }


        public static event EventHandler<LcValueEventArgs> LcValueChanging;



        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="srcRecord"></param>
        /// <param name="model"></param>
        /// <returns>返回修改过的字段集合</returns>
        public static string[] Exec(LModel model)
        {
            if (model == null)
            {
                return null;
            }

            LModelElement modelElem = model.GetModelElement();

            string modelName = modelElem.DBTableName;


            LcValueTableCollection lcvTables = LcValueManager.GetTables(modelName);

            if (lcvTables == null)
            {
                return null;
            }


            string[] blemishFields = LightModel.GetBlemishPropNames(model);

            if (blemishFields == null)
            {
                return null;
            }

            foreach (var item in lcvTables)
            {
                try
                {
                    Exec_ForTable(item, model, blemishFields, modelElem, modelName);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("编码规则错误：ID={0}, Table={1}, Display={2}, Remark={3}",
                        item.ID, item.Table, item.Display, item.Remark),ex);

                }
            }

            return null;
        }

        /// <summary>
        /// 根据数据表执行。
        /// </summary>
        /// <param name="lcvTable"></param>
        /// <param name="model"></param>
        /// <param name="blemishFields"></param>
        /// <param name="modelElem"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        private static string[] Exec_ForTable(LcValueTable lcvTable, LModel model, string[] blemishFields, LModelElement modelElem, string modelName)
        {
            LModelFieldElement fieldElem;

            foreach (var lcvIf in lcvTable.IfList)
            {
                bool exist = ArrayUtil.Exist(blemishFields, lcvIf.Field);

                if (!exist)
                {
                    continue;
                }

                if (!modelElem.TryGetField(lcvIf.Field, out fieldElem))
                {
                    throw new Exception(string.Format("实体“{0}”不存在此字段 “{1}”", modelName, lcvIf.Field));
                }


                object lcvValue = HWQ.Entity.ModelConvert.ChangeType(lcvIf.ValueTo, fieldElem);

                object mValue = model[fieldElem];

                bool eq = LogicAB(mValue, lcvValue, lcvIf.Logic);

                if (!eq)
                {
                    continue;
                }


                Exec_Then(lcvIf, model, modelElem, modelName);

            }

            return null;
        }


        /// <summary>
        /// 执行 If 的 Then 子句
        /// </summary>
        /// <param name="lcvIf"></param>
        /// <param name="model"></param>
        /// <param name="modelElem"></param>
        /// <param name="modelName"></param>
        private static void Exec_Then(LcValueIf lcvIf, LModel model, LModelElement modelElem, string modelName)
        {
            LModelFieldElement thenFieldElem;


            foreach (var lcvThen in lcvIf.ThenList)
            {
                if (!modelElem.TryGetField(lcvThen.Field, out thenFieldElem))
                {
                    throw new Exception(string.Format("代码规则错误：实体“{0}”不存在此字段 “{1}”", modelName, lcvThen.Field));
                }

                string lcvValueStr = lcvThen.ValueTo;

                if (StringUtil.IsBlank(lcvValueStr))
                {
                    continue;
                }

                if (lcvThen.Logic == LcValueThenLogic.IF_NULL)
                {
                    object fieldValue = model[thenFieldElem];

                    if (!model.IsNull(lcvThen.Field) && !StringUtil.IsBlank(Convert.ToString(fieldValue)))
                    {
                        continue;
                    }


                    //if (thenFieldElem.DBType == LMFieldDBTypes.String)
                    //{
                    //    if (!StringUtil.IsBlank( Convert.ToString( fieldValue)))
                    //    {
                    //        continue;
                    //    }
                    //}
                }



                //如果是 = 开头，那么就采用公式
                if (StringUtil.StartsWith(lcvValueStr,"="))
                {
                    if (LcValueChanging != null)
                    {
                        LcValueChanging(null, new LcValueEventArgs(lcvValueStr, lcvThen.Field, model));
                    }

                    //object result = LcModelManager.Exec(lcvValueStr, model);

                    //model[thenFieldElem] = result;
                }
                //else if (lcvValueStr.StartsWith("=F.", StringComparison.OrdinalIgnoreCase))
                //{

                //}
                else
                {
                    object comValue = HWQ.Entity.ModelConvert.ChangeType(lcvThen.ValueTo, thenFieldElem);

                    model[thenFieldElem] = comValue;
                }

            }
        }

            

        /// <summary>
        /// 比较两个值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logicAB"></param>
        /// <returns></returns>
        private static bool LogicAB(object a, object b, Logic logicAB)
        {
            bool result = false;

            decimal a1, b1;

            switch (logicAB)
            {
                case Logic.Equality:



                    result = (a.Equals(b));
                    break;
                case Logic.GreaterThan:

                    if (a != null && b != null)
                    {
                        a1 = Convert.ToDecimal(a);
                        b1 = Convert.ToDecimal(b);
                        result = (a1 > b1);
                    }

                    break;
                case Logic.LessThan:

                    if (a != null && b != null)
                    {
                        a1 = Convert.ToDecimal(a);
                        b1 = Convert.ToDecimal(b);
                        result = (a1 < b1);
                    }

                    break;
                case Logic.LessThanOrEqual:

                    if (a != null && b != null)
                    {
                        a1 = Convert.ToDecimal(a);
                        b1 = Convert.ToDecimal(b);
                        result = (a1 <= b1);
                    }
                    break;
                case Logic.GreaterThanOrEqual:

                    if (a != null && b != null)
                    {
                        a1 = Convert.ToDecimal(a);
                        b1 = Convert.ToDecimal(b);
                        result = (a1 >= b1);
                    }
                    break;
                case Logic.Inequality:

                    if (a != null && b != null)
                    {
                        a1 = Convert.ToDecimal(a);
                        b1 = Convert.ToDecimal(b);
                        result = (a1 != b1);
                    }

                    break;
                case Logic.In:
                    break;
                case Logic.LeftLike:
                    break;
                case Logic.Like:
                    break;
                case Logic.NotIn:
                    break;
                case Logic.NotLike:
                    break;
                case Logic.RightLike:
                    break;
                default:
                    break;
            }

            return result;
        }


    }
}
