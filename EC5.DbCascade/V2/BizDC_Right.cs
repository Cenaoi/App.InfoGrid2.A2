using EC5.DbCascade.DbCascadeEngine;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.V2
{




    /// <summary>
    /// 处理右边
    /// </summary>
    public class BizDC_Right
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbccModel"></param>
        /// <param name="leftModel"></param>
        public bool ProDBccItem(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStepPath stepPath)
        {
            string actCode = dbccModel.L_ActCode.ToUpper();

            bool proSuccess = false;

            switch (actCode)
            {
                case BizDbCascade.UPDATE:
                    proSuccess = ProDBccItem_Update(decipher, dbccModel, srcModel, stepPath);
                    break;
                case BizDbCascade.INSERT:
                    proSuccess = ProDBccItem_Insert(decipher, dbccModel, srcModel, stepPath);
                    break;
                case BizDbCascade.DELETE:
                    proSuccess = ProDBccItem_Delete(decipher, dbccModel, srcModel, stepPath);
                    break;
                case BizDbCascade.ALL:
                    proSuccess = ProDBccItem_All(decipher, dbccModel, srcModel, stepPath);
                    break;

            }

            return proSuccess;
        }



        private bool ProDBccItem_Update(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStepPath stepPath)
        {
            //LModelList<LModel> bModels = null;

            bool isChanged = IsChangedForUpdate(dbccModel, srcModel);

            if (!isChanged)
            {
                stepPath.Create("监听阻止,字段没有发生变化。", dbccModel, srcModel);
                return false;
            }


            if (dbccModel.R_IsSubFilter)
            {
                //bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);
            }
            else
            {
                bool isCancel = FilterR_SrcModel(decipher, dbccModel, srcModel);

                if (isCancel)
                {
                    stepPath.Create("右边过滤阻止，条件不成立", dbccModel, srcModel);
                    return false;
                }
            }

            return true;


        }



        private bool ProDBccItem_Insert(DbDecipher decipher, DbccModel dbccModel, LModel srcModel,
            BizDbStepPath stepPath)
        {

            //LModelList<LModel> bModels = null;

            bool isChanged = IsChangedForUpdate(dbccModel, srcModel);

            if (!isChanged)
            {
                stepPath.Create("监听阻止,字段没有发生变化。", dbccModel, srcModel);

                return false;
            }

            if (dbccModel.R_IsSubFilter == true)
            {
                //bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);
            }
            else
            {
                bool isCancel = FilterR_SrcModel(decipher, dbccModel, srcModel);

                if (isCancel)
                {
                    stepPath.Create("右边过滤阻止，条件不成立", dbccModel, srcModel);
                    return false;
                }
            }


            return true;

            //ProDBccItem_Left(decipher, dbccModel, srcModel, parentStep);

        }



        private bool ProDBccItem_Delete(DbDecipher decipher, DbccModel dbccModel, LModel rightModel, BizDbStepPath stepPath)
        {
            bool isChanged = IsChangedForUpdate(dbccModel, rightModel);

            if (!isChanged)
            {
                stepPath.Create("监听阻止,字段没有发生变化。", dbccModel, rightModel);
                return false;
            }



            if (dbccModel.R_IsSubFilter)
            {
                //bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);
            }
            else
            {
                bool isCancel = FilterR_SrcModel(decipher, dbccModel, rightModel);

                if (isCancel)
                {
                    stepPath.Create("右边过滤阻止，条件不成立", dbccModel, rightModel);
                    return false;
                }
            }

            return true;
        }



        private bool ProDBccItem_All(DbDecipher decipher, DbccModel dbccModel, LModel rightModel,
            BizDbStepPath stepPath)
        {
            bool isChanged = IsChangedForUpdate(dbccModel, rightModel);

            if (!isChanged)
            {
                stepPath.Create("监听阻止,字段没有发生变化。", dbccModel, rightModel);
                return false;
            }


            //LModelList<LModel> bModels = null;

            if (dbccModel.R_IsSubFilter == true)
            {
                //bModels = GetModel_BySubFilterR(decipher, dbccModel, rightModel);
            }
            else
            {
                bool isCancel = FilterR_SrcModel(decipher, dbccModel, rightModel);

                if (isCancel)
                {
                    stepPath.Create("右边过滤阻止，条件不成立", dbccModel, rightModel);
                    return false;
                }
            }

            return true;
            
        }




        /// <summary>
        /// 过滤右边
        /// </summary>
        private bool FilterR_SrcModel(DbDecipher decipher, DbccModel dbccModel, LModel srcModel)
        {
            bool isCancel = false;

            LModelElement srcModelElem = srcModel.GetModelElement();
            
            bool isLogic = false;

            foreach (DbccFilterItem filterItem in dbccModel.FilterRight)
            {

                object bValue = BizDcHelper.GetFilterValue_ForRight(decipher, dbccModel, filterItem, srcModel);

                object aValue = srcModel[filterItem.A_Col];

                LModelFieldElement srcFieldElem = srcModelElem.Fields[filterItem.A_Col];

                bValue = ModelConvert.ChangeType(bValue, srcFieldElem);
                aValue = ModelConvert.ChangeType(aValue, srcFieldElem);

                isLogic = false;

                if (srcFieldElem.IsNumber)
                {
                    try
                    {
                        isLogic = BizDbMath.LogicAB(aValue, filterItem.A_Logic, bValue);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("字段:{3},{4},比较两个数值错误。值A： {0}={1}, 值B: {2}",
                            filterItem.A_Col, aValue, bValue,
                            srcFieldElem.DBField,
                            srcFieldElem.Description), ex);
                    }
                }
                else if (srcFieldElem.DBType == LMFieldDBTypes.String)
                {
                    try
                    {
                        isLogic = BizDbMath.LogicABString(Convert.ToString(aValue), filterItem.A_Logic, Convert.ToString(bValue));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("字段:{3},{4}, 比较两个数值错误。值A： {0}={1}, 值B: {2}",
                            filterItem.A_Col, aValue, bValue,
                            srcFieldElem.DBField,
                            srcFieldElem.Description), ex);
                    }
                }
            

                if (isLogic == false)
                {
                    isCancel = true;
                    break;
                }
            }

            return isCancel;
        }






        /// <summary>
        /// 字段是否已经发生变化
        /// </summary>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        /// <param name="parStep">上级步骤</param>
        /// <returns></returns>
        private bool IsChangedForUpdate(DbccModel dbccModel, LModel srcModel)
        {
            //如果没有字段，默认监控全部
            if (!dbccModel.HasListen())
            {
                return true;
            }

            if (!srcModel.GetTakeChange())
            {
                return true;
            }

            bool isChanged = false;
            bool isValueChanged;

            LModelElement modelElem = srcModel.GetModelElement();

            LModelFieldElementCollection fieldElems = modelElem.Fields;

            if (dbccModel.ListenLogic == DbccLogic.OR)
            {
                isChanged = false;

                foreach (DbccListen dlField in dbccModel.ListenFields)
                {
                    if (!dlField.Enabled || StringUtil.IsBlank(dlField.DBField))
                    {
                        continue;
                    }

                    isValueChanged = srcModel.GetBlemish(dlField.DBField);

                    if (!isValueChanged)
                    {
                        continue;
                    }

                    bool isLintentChanged = IsValueChanged(dlField, fieldElems, srcModel);

                    if (isLintentChanged)
                    {
                        isChanged = true;
                        break;
                    }
                }


            }
            else if (dbccModel.ListenLogic == DbccLogic.AND)
            {
                isChanged = true;

                foreach (DbccListen dlField in dbccModel.ListenFields)
                {
                    if (!dlField.Enabled || StringUtil.IsBlank(dlField.DBField))
                    {
                        continue;
                    }


                    isValueChanged = srcModel.GetBlemish(dlField.DBField);

                    if (!isValueChanged)
                    {
                        isChanged = false;
                        break;
                    }


                    bool isLintentChanged = IsValueChanged(dlField, fieldElems, srcModel);

                    if (!isLintentChanged)
                    {
                        isChanged = false;
                        break;
                    }
                }

            }

            return isChanged;
        }



        /// <summary>
        /// 判断值是否发生变化了
        /// </summary>
        /// <returns></returns>
        private bool IsValueChanged(DbccListen dlField, LModelFieldElementCollection fieldElems, LModel srcModel)
        {
            //当前只处理单值的，没有处理数组

            bool isEmptyFrom = StringUtil.IsBlank(dlField.ValueFrom);
            bool isEmptyTo = StringUtil.IsBlank(dlField.ValueTo);

            if (isEmptyFrom && isEmptyTo)
            {
                return true;
            }

            LModelFieldElement fieldElem = fieldElems[dlField.DBField];

            //两个都不为空
            if (!isEmptyFrom && !isEmptyTo)
            {
                object valFrom = ModelConvert.ChangeType(dlField.ValueFrom, fieldElem);
                object valTo = ModelConvert.ChangeType(dlField.ValueTo, fieldElem);

                object mSrcValue = srcModel.GetOriginalValue(dlField.DBField);  //实体原始值
                object mNowValue = srcModel[dlField.DBField];

                if (fieldElem.IsNumber)
                {
                    bool isFromEqual = BizDbMath.LogicAB(valFrom, Logic.Equality, mSrcValue);

                    bool isToEqual = BizDbMath.LogicAB(valTo, Logic.Equality, mNowValue);

                    return (isFromEqual && isToEqual);
                }
                else if (fieldElem.DBType == LMFieldDBTypes.String)
                {
                    bool isFromEqual = BizDbMath.LogicABString(Convert.ToString(valFrom), Logic.Equality, Convert.ToString(mSrcValue));

                    bool isToEqual = BizDbMath.LogicABString(Convert.ToString(valTo), Logic.Equality, Convert.ToString(mNowValue));

                    return (isFromEqual && isToEqual);
                }
            }

            //原为空，目标不为空
            if (isEmptyFrom && !isEmptyTo)
            {
                object valTo = ModelConvert.ChangeType(dlField.ValueTo, fieldElem);

                object mSrcValue = srcModel.GetOriginalValue(dlField.DBField);  //实体原始值

                if (fieldElem.IsNumber)
                {
                    bool isToEqual = BizDbMath.LogicAB(mSrcValue, Logic.Equality, valTo);

                    return isToEqual;
                }
                else if (fieldElem.DBType == LMFieldDBTypes.String)
                {
                    bool isToEqual = BizDbMath.LogicABString(Convert.ToString(mSrcValue), Logic.Equality, Convert.ToString(valTo));

                    return isToEqual;
                }
            }

            //原不空，目标空。
            if (!isEmptyFrom && isEmptyTo)
            {
                object valFrom = ModelConvert.ChangeType(dlField.ValueFrom, fieldElem);

                object mNowValue = srcModel[dlField.DBField];

                if (fieldElem.IsNumber)
                {
                    bool isFromEqual = BizDbMath.LogicAB(valFrom, Logic.Equality, mNowValue);

                    return isFromEqual;
                }
                else if (fieldElem.DBType == LMFieldDBTypes.String)
                {
                    bool isFromEqual = BizDbMath.LogicABString(Convert.ToString(valFrom), Logic.Equality, Convert.ToString(mNowValue));

                    return isFromEqual;
                }
                
            }

            return false;
        }
    }
}
