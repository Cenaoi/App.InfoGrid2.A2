using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.LCodeEngine;
using EC5.LcValueEngine;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;

namespace EasyClick.BizWeb2.LcValue
{

    /// <summary>
    /// 简单流程工厂
    /// </summary>
    public class LCodeValueFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 绑定数据仓库
        /// </summary>
        /// <param name="store"></param>
        public void BindStore(Store store)
        {
            //store.Inserting += store_Inserting;
            store.Updating += store_Updating;

        }

        void store_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            try
            {
                ExecLCode((Store)sender, e.SrcRecord, model);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                e.Cancel = true;
            }
        }

        void store_Updating(object sender, ObjectCancelEventArgs e)
        {

            LModel model = e.Object as LModel;

            try
            {
                ExecLCode((Store)sender, e.SrcRecord, model);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                e.Cancel = true;
            }

        }

        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string[] ExecLCode(Store store, LModel model)
        {
            return ExecLCode(store, null, model);
        }

        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="srcRecord"></param>
        /// <param name="model"></param>
        /// <returns>返回修改过的字段集合</returns>
        public string[] ExecLCode(Store store, DataRecord srcRecord, LModel model)
        {

            LModelElement modelElem = model.GetModelElement();

            string modelName = modelElem.DBTableName;


            LcValueTableCollection lcvTables =  LcValueManager.GetTables(modelName);

            if (lcvTables == null)
            {
                return null;
            }


            string[] blemishFields = LightModel.GetBlemishPropNames(model);

            foreach (var lcvTable in lcvTables)
            {

                foreach (var lcvIf in lcvTable.IfList)
                {
                    bool exist = ArrayUtil.Exist(blemishFields, lcvIf.Field);

                    if (!exist)
                    {
                        continue;
                    }

                    LModelFieldElement fieldElem = null;

                    if (!modelElem.TryGetField(lcvIf.Field,out fieldElem))
                    {
                        throw new Exception(string.Format("实体“{0}”不存在此字段 “{1}”", modelName, lcvIf.Field));
                    }


                    object lcvValue = ModelConvert.ChangeType(lcvIf.ValueTo, fieldElem);

                    object mValue = model[fieldElem];

                    bool eq = this.LogicAB(mValue, lcvValue, lcvIf.Logic);

                    if (!eq)
                    {
                        continue;
                    }


                    foreach (var lcvThen in lcvIf.ThenList)
                    {
                        if (!modelElem.Fields.ContainsField(lcvThen.Field))
                        {
                            throw new Exception(string.Format("实体“{0}”不存在此字段 “{1}”", modelName, lcvThen.Field));
                        }
                            
                        LModelFieldElement thenFieldElem = modelElem.Fields[lcvThen.Field];

                        string lcvValueStr = lcvThen.ValueTo;

                        if (StringUtil.IsBlank(lcvValueStr))
                        {
                            continue;
                        }

                        object srcValue = model[thenFieldElem];

                        if (lcvThen.Logic == LcValueThenLogic.IF_NULL)
                        {
                            if (!model.IsNull(lcvThen.Field))
                            {
                                continue;
                            }

                            if (thenFieldElem.DBType == LMFieldDBTypes.String)
                            {
                                if (!StringUtil.IsBlank(Convert.ToString(srcValue)))
                                {
                                    continue;
                                }

                            }
                        }


                        if (lcvValueStr[0] == '=')
                        {
                            object result = LcModelManager.Exec(lcvValueStr, model);

                            model[thenFieldElem] = result;
                        }
                        else if (lcvValueStr.StartsWith("Fun.", StringComparison.OrdinalIgnoreCase))
                        {

                        }
                        else
                        {
                            object comValue = HWQ.Entity.ModelConvert.ChangeType(lcvThen.ValueTo, thenFieldElem);

                            model[thenFieldElem] = comValue;
                        }

                    }


                }

            }




            //string[] changedFields = new string[0];

            //string[] blemishFields = LightModel.GetBlemishPropNames(model);

            //foreach (string bF in blemishFields)
            //{
            //    if (!cModel.IsListen(bF))
            //    {
            //        continue;
            //    }

            //    string[] updateFs;

            //    //执行 LightCode 代码
            //    cModel.Exec(bF, model, out updateFs);

            //    changedFields = ArrayUtil.Union(changedFields, updateFs);
            //}


            //if (store != null)
            //{
            //    //输出
            //    foreach (string uField in changedFields)
            //    {
            //        object uValue = model[uField];
            //        store.SetRecordValue(srcRecord.Id.ToString(), uField, uValue.ToString());
            //    }
            //}

            //return changedFields;

            return null;

        }




        /// <summary>
        /// 比较两个值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="logicAB"></param>
        /// <returns></returns>
        private bool LogicAB(object a, object b, Logic logicAB)
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
