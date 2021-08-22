using HWQ.Entity;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade
{
    public static class BizDbMath
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 合计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static decimal Sum(string field, LModelList<LModel> models)
        {
            LModelElement modelElem = null;
            LModelFieldElement fieldElem = null;

            decimal result = 0;

            foreach (LModel model in models)
            {
                if (model.IsNull(field))
                {
                    continue;
                }

                modelElem = model.GetModelElement();

                if (!modelElem.TryGetField(field, out fieldElem))
                {
                    throw new Exception(string.Format("实体“{0}”中不存在此字段“{1}”。", modelElem.DBTableName, field));
                }

                if (!fieldElem.IsNumber)
                {
                    throw new Exception(string.Format("字段必须为数字才能进行 Sum(...) 计算。实体“{0}”字段“{1}”。", modelElem.DBTableName, field));
                }

                result += model.Get<decimal>(field);
            }

            return result;
        }


        /// <summary>
        /// 函数计算。平均
        /// </summary>
        /// <param name="models"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static  decimal Avg(string field, LModelList<LModel> models)
        {
            LModelElement modelElem = null;
            LModelFieldElement fieldElem = null;

            decimal result = 0;

            foreach (LModel model in models)
            {
                if (model.IsNull(field))
                {
                    continue;
                }

                modelElem = model.GetModelElement();

                if (!modelElem.TryGetField(field, out fieldElem))
                {
                    throw new Exception(string.Format("实体“{0}”中不存在此字段“{1}”。", modelElem.DBTableName, field));
                }

                if (!fieldElem.IsNumber)
                {
                    throw new Exception(string.Format("字段必须为数字才能进行 Sum(...) 计算。实体“{0}”字段“{1}”。", modelElem.DBTableName, field));
                }

                result += model.Get<decimal>(field);
            }

            result /= models.Count;

            return result;
        }

        public static bool LogicABString(string a, Logic logicAB, string b)
        {

            bool result = false;

            if (a == null) { a = string.Empty; }
            if (b == null) { b = string.Empty; }

            if (logicAB == Logic.Equality)
            {
                result = (a == b);
            }
            else if (logicAB == Logic.Inequality)
            {
                result = (a != b);
            }
            else
            {
                log.Debug("其它条件不成立.");
            }

            return result;
        }


        /// <summary>
        /// 比较两个值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logicAB"></param>
        /// <returns></returns>
        public static bool LogicAB(object a,Logic logicAB,  object b)
        {
            bool result = false;

            decimal a1, b1;

            switch (logicAB)
            {
                case Logic.Equality:

                    if (a != null && b != null)
                    {
                        a1 = Convert.ToDecimal(a);
                        b1 = Convert.ToDecimal(b);

                        result = (a.Equals(b));
                    }

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
