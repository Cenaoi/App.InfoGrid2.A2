using EC5.AppDomainPlugin;
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

    public class BizDC_Left
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object DynModel { get; private set; }

        /// <summary>
        /// 处理左边事件
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        public bool ProDBccItem_Left(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStepPath stepPath, out BizDbAction opData)
        {
            string leftActCode = dbccModel.L_ActCode.ToUpper();

            //BizDbAction opData = null;

            bool success = false;


            switch (leftActCode)
            {
                case BizDbCascade.INSERT:

                    success = ProDBccItem_Left_INSERT(decipher, dbccModel, srcModel, stepPath, out opData);
                    break;
                case BizDbCascade.DELETE:

                    success = ProDBccItem_Left_DELETE(decipher, dbccModel, srcModel, stepPath, out opData);

                    break;
                case BizDbCascade.UPDATE:

                    opData = new BizDbAction(DbOperate.Update, dbccModel, srcModel);
                    opData.ActionID = dbccModel.ID;
                    opData.DbccModel = dbccModel;

                    success = true;

                    //if (dbccModel.R_IsSubFilter)
                    //{
                    //    //过滤子项集合,

                    //    LModelList<LModel> bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);

                    //    success = ProDBccItem_Left_UPDATE_SubFilter(decipher, dbccModel, srcModel, bModels, stepPath, out  opData);
                    //}
                    //else
                    //{
                    //    //普通更新

                    //    success = ProDBccItem_Left_UPDATE_Common(decipher, dbccModel, srcModel, stepPath, out  opData);
                    //}

                    break;
                default:
                    opData = null;
                    break;
            }

            return success;
        }


        public bool Update(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStepPath stepPath, out BizDbAction opData)
        {

            bool success = false;

            if (dbccModel.R_IsSubFilter)
            {
                //过滤子项集合,

                LModelList<LModel> bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);

                success = ProDBccItem_Left_UPDATE_SubFilter(decipher, dbccModel, srcModel, bModels, stepPath, out  opData);
            }
            else
            {
                //普通更新

                success = ProDBccItem_Left_UPDATE_Common(decipher, dbccModel, srcModel, stepPath, out  opData);
            }

            return success;
        }


        /// <summary>
        /// 常规更新
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        private bool ProDBccItem_Left_UPDATE_Common(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, 
            BizDbStepPath stepPath  ,
            out BizDbAction opModels)
        {




            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);


            try
            {
                foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
                {
                    object bValue = BizDcHelper.GetFilterValue_ForLeft(decipher, dbccModel, filterItem, srcModel);

                    filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("联动处理-“左边过滤”赋值发生错误。", ex);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);


            #region 执行 Then 条件

            BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, stepPath);

            if (result == BizResult.Stoped)
            {
                opModels = null;
                return false;
            }

            #endregion






            if (aModels.Count == 0 && dbccModel.L_NotExist_Then == "A")
            {
                LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

                if (modelElem == null)
                {
                    throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.L_Table));
                }

                LModel model = new LModel(modelElem);

                model.SetTakeChange(true);

                foreach (DbccItem dbccItem in dbccModel.Items)
                {
                    if (dbccItem.L_ActCode == "E")
                    {
                        continue;
                    }


                    object value = ItemValueHelper.GetItemValue(decipher, dbccModel, dbccItem, srcModel);
                    model[dbccItem.L_Field] = value;

                }

                try
                {
                    BizDcHelper.ExecLCode(model);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行动态代码错误。", ex);
                }

                ExecScriptCode(decipher, dbccModel, BizDbCascade.INSERT, model, srcModel);   //执行动态脚本


                opModels = new BizDbAction(DbOperate.Insert, model);
                opModels.ActionID = dbccModel.ID;
                opModels.DbccModel = dbccModel;
            }
            else
            {

                foreach (LModel aModel in aModels)
                {
                    LModelElement modelElem = srcModel.GetModelElement();

                    aModel.SetTakeChange(true);

                    foreach (DbccItem dbccItem in dbccModel.Items)
                    {
                        if (dbccItem.L_ActCode == "A")
                        {
                            continue;
                        }

                        object value = ItemValueHelper.GetItemValue(decipher, dbccModel, dbccItem, srcModel);

                        aModel[dbccItem.L_Field] = value;
                    }

                    try
                    {
                        BizDcHelper.ExecLCode(aModel);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("执行动态代码错误。", ex);
                    }

                    ExecScriptCode(decipher, dbccModel, aModel, srcModel);   //执行动态脚本

                }


                opModels = new BizDbAction(DbOperate.Update,aModels);
                opModels.ActionID = dbccModel.ID;
                opModels.DbccModel = dbccModel;
            }


            return true;
        }





        /// <summary>
        /// 子项更新
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        private bool ProDBccItem_Left_UPDATE_SubFilter(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, LModelList<LModel> bModels, 
            BizDbStepPath stepPath,
            out BizDbAction opModels)
        {


            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
            {
                object bValue = BizDcHelper.GetFilterValue_ForLeft(decipher, dbccModel, filterItem, srcModel);

                filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);



            #region 执行 Then 条件

            BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, stepPath);

            if (result == BizResult.Stoped)
            {
                opModels = null;
                return false;
            }

            #endregion


            if (aModels.Count == 0 && dbccModel.L_NotExist_Then == "A")
            {
                LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

                if (modelElem == null)
                {
                    throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.L_Table));
                }

                LModel model = new LModel(modelElem);

                model.SetTakeChange(true);

                foreach (DbccItem dbccItem in dbccModel.Items)
                {
                    if (dbccItem.L_ActCode == "E")
                    {
                        continue;
                    }


                    object value = ItemValueHelper.GetItemValue(decipher, dbccModel, dbccItem, srcModel);
                    model[dbccItem.L_Field] = value;

                }

                try
                {
                    BizDcHelper.ExecLCode(model);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行动态代码错误。", ex);
                }


                ExecScriptCode(decipher, dbccModel, BizDbCascade.INSERT, model, srcModel);   //执行动态脚本

                opModels = new BizDbAction(DbOperate.Insert, model);
                opModels.ActionID = dbccModel.ID;
                opModels.DbccModel = dbccModel;

            }
            else
            {

                foreach (LModel aModel in aModels)
                {
                    LModelElement modelElem = srcModel.GetModelElement();

                    aModel.SetTakeChange(true);

                    foreach (DbccItem dbccItem in dbccModel.Items)
                    {
                        if (dbccItem.L_ActCode == "A")
                        {
                            continue;
                        }

                        object value = ItemValueHelper.GetItemValue(decipher, dbccModel, dbccItem, bModels);

                        aModel[dbccItem.L_Field] = value;
                    }


                    try
                    {
                        BizDcHelper.ExecLCode(aModel);
                    }
                    catch (Exception ex)
                    {
                        log.Error("执行动态代码错误。", ex);
                    }

                    ExecScriptCode(decipher, dbccModel, aModel, srcModel);   //执行动态脚本
                }


                opModels = new BizDbAction(DbOperate.Update,  aModels);
                opModels.ActionID = dbccModel.ID;
                opModels.DbccModel = dbccModel;
            }


            return true;
        }



        private LModelList<LModel> GetModel_BySubFilterR(DbDecipher decipher, DbccModel dbccModel, LModel srcModel)
        {

            LightModelFilter filterRight = new LightModelFilter(dbccModel.R_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterRight)
            {
                //object bValue = BizDcHelper.GetFilterValue_ForLeft(decipher, dbccModel, filterItem, srcModel);

                object bValue = BizDcHelper.GetFilterValue_ForRight(decipher, dbccModel, filterItem, srcModel);

                filterRight.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> bModels = decipher.GetModelList(filterRight);

            return bModels;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        /// <param name="newModel">行插入的记录</param>
        private bool ProDBccItem_Left_INSERT(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStepPath stepPath, 
            out BizDbAction opModels)
        {
            bool success = false;

            LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

            LModelFieldElement fieldElem;

            LModel model = new LModel(modelElem);

            model.SetTakeChange(true);

            foreach (DbccItem item in dbccModel.Items)
            {
                if (item.L_ActCode == "E")
                {
                    continue;
                }

                model[item.L_Field] = ItemValueHelper.GetItemValue(decipher, dbccModel, item, srcModel);
            }

            try
            {
                BizDcHelper.ExecLCode(model);
            }
            catch (Exception ex)
            {
                stepPath.Errors.Add("执行动态代码错误。");

                throw new Exception("执行动态代码错误。", ex);
            }


            ExecScriptCode(decipher, dbccModel, model, srcModel);   //执行动态脚本


            opModels = new BizDbAction(DbOperate.Insert, model);
            opModels.ActionID = dbccModel.ID;
            opModels.DbccModel = dbccModel;
            
            return true;

        }

        /// <summary>
        /// 执行动态脚本
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="curModel"></param>
        /// <param name="parentModel"></param>
        private void ExecScriptCode(DbDecipher decipher, DbccModel dbccModel, LModel curModel, LModel parentModel)
        {
            ExecScriptCode(decipher, dbccModel, null, curModel, parentModel);
        }

        /// <summary>
        /// 执行动态脚本
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="curModel">当前实体</param>
        /// <param name="parentModel">上级实体</param>
        private void ExecScriptCode(DbDecipher decipher, DbccModel dbccModel, string actCode, LModel curModel, LModel parentModel)
        {
            actCode = StringUtil.NoBlank(actCode, dbccModel.L_ActCode);

            if (dbccModel.ActNewEnabeld && actCode == BizDbCascade.INSERT)
            {

                string code;

                if (dbccModel.ActNewSCode.Contains("return "))
                {
                    code = dbccModel.ActNewSCode;
                }
                else
                {
                    code = "return " + dbccModel.ActNewSCode + ";";
                }

                LModelElement curModelElem = curModel.GetModelElement();

                LModelElement pModelElem = parentModel.GetModelElement();

                ScriptInstance inst = ScriptFactory.Create(code);

                inst.Params["parent"] = parentModel;

                inst.Params[curModelElem.DBTableName] = curModel;
                inst.Params[pModelElem.DBTableName] = parentModel;

                inst.Params["decipher"] = decipher;

                inst.Params["log"] = log;
                  


                try
                {
                    object value = inst.Exec(curModel);
                }
                catch(Exception ex)
                {
                    throw new Exception("执行新建后的 ScriptCode 脚本错误: \n" + dbccModel.ActNewSCode,ex);
                }

                //result = ModelConvert.ChangeType(value, fieldElem);

            }

            if (dbccModel.ActUpdateEnabled && actCode == BizDbCascade.UPDATE)
            {
                string code;

                if (dbccModel.ActUpdateSCode.Contains("return "))
                {
                    code = dbccModel.ActUpdateSCode;
                }
                else
                {
                    code = "return " + dbccModel.ActUpdateSCode + ";";
                }

                LModelElement curModelElem = curModel.GetModelElement();
                LModelElement pModelElem = parentModel.GetModelElement();

                ScriptInstance inst = ScriptFactory.Create(code);
                
                inst.Params["parent"] = parentModel;

                inst.Params[curModelElem.DBTableName] = curModel;
                inst.Params[pModelElem.DBTableName] = parentModel;

                inst.Params["decipher"] = decipher;

                inst.Params["log"] = log;

                try
                {
                    object value = inst.Exec(curModel);
                }
                catch(Exception ex)
                {
                    throw new Exception("执行更新后的 ScriptCode 脚本错误: \n" + dbccModel.ActUpdateSCode, ex);
                }
                //result = ModelConvert.ChangeType(value, fieldElem);

            }

        }



        private bool ProDBccItem_Left_DELETE(DbDecipher decipher, DbccModel dbccModel, LModel rightModel, BizDbStepPath stepPath,
            out BizDbAction opModels)
        {


            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
            {
                object bValue = BizDcHelper.GetFilterValue_ForLeft(decipher, dbccModel, filterItem, rightModel);

                filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);


            #region 执行 Then 条件

            BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, stepPath);

            if (result == BizResult.Stoped)
            {
                opModels = null;

                return false;
            }

            #endregion



            opModels = new BizDbAction(DbOperate.Delete, aModels);
            opModels.ActionID = dbccModel.ID;
            opModels.DbccModel = dbccModel;

            return true;


        }










        private bool ProDbccItem_Left_ThenField(DbDecipher decipher, LModel firstModel, DbccModel dbccModel, DbccThen dThen,
            LModelList<LModel> aModels, BizDbStepPath stepPath)
        {
            bool stoped = false;

            string resultMsg = null;

            LModelElement modelElem = firstModel.GetModelElement();
            LModelFieldElement fieldElem = modelElem.Fields[dThen.A_Field];

            object fieldValue = firstModel[fieldElem];


            //处理空值, 并阻止.
            if ( (fieldValue == null || string.Empty.Equals( fieldValue)) )
            {
                bool result;

                if (dThen.A_Value == "[NULL]")
                {
                    result = (dThen.A_Login == Logic.Equality);


                }
                else if (fieldElem.IsNumber)
                {
                    decimal aValue = 0;

                    if(!decimal.TryParse(dThen.A_Value,out aValue))
                    {
                        throw new Exception($"左边-条件判断：{dThen.A_Field} 必须是数字类型. 您填写了错误 \"{dThen.A_Value}\"");
                    }

                    result = aValue.Equals(fieldValue);
                }
                else
                {
                    result = dThen.A_Value.Equals(fieldValue);
                }


                if (result && dThen.IsStop)
                {
                    resultMsg = dThen.ResultMessage;
                    stoped = true;
                }



                if (stoped)
                {
                    stepPath.Errors.Add("提示:" + resultMsg);
                }

                return stoped;
            }

            


            if (fieldElem.IsNumber)
            {
                decimal aValue = StringUtil.ToDecimal(dThen.A_Value);

                decimal fValue;
                bool isSucess;

                try
                {
                    fValue = (decimal)ModelConvert.ChangeType(fieldValue, LMFieldDBTypes.Decimal, true);
                }
                catch (Exception ex)
                {
                    throw new Exception($"左边-条件判断：转换格式错误，{dThen.A_Field} 必须是数字，不能为空。");
                }

                isSucess = BizDbMath.LogicAB(fValue, dThen.A_Login, aValue);

                if (isSucess && dThen.IsStop)
                {
                    resultMsg = dThen.ResultMessage;
                    stoped = true;
                }
            }
            else if (fieldElem.DBType == LMFieldDBTypes.String)
            {
                string fValue = (string)fieldValue;

                bool result = BizDbMath.LogicABString(fValue, dThen.A_Login, dThen.A_Value);


                if (result && dThen.IsStop)
                {
                    resultMsg = dThen.ResultMessage;
                    stoped = true;
                }
            }
            else
            {
                log.Debug("没有条件可处理。。。");
            }
            


            if (stoped)
            {
                //BizDbStep step = new BizDbStep(BizDbStepType.NONE);
                //step.ResultMessage = "提示:" + resultMsg;
                //step.IsDialogMsg = true;
                //step.Table = dbccModel.L_Table;

                //stepPath.Cur.Childs.Add(step);


                stepPath.Errors.Add("提示:" + resultMsg);
            }



            return stoped;
        }


        


        private BizResult ProDbccItem_Left_Then(DbDecipher decipher, DbccModel dbccModel, LModelList<LModel> aModels, BizDbStepPath stepPath)
        {

            bool stoped = false;

            string resultMsg = null;

            string aTypeText = string.Empty;
            LModel curModel = null;

            foreach (DbccThen dThen in dbccModel.Thens)
            {
                string aTypeID = dThen.A_TypeID.ToUpper();

                if (aTypeID == "COUNT")
                {
                    decimal aValue = StringUtil.ToDecimal(dThen.A_Value);

                    bool isSucess = BizDbMath.LogicAB(aModels.Count, dThen.A_Login, aValue);

                    if (isSucess && dThen.IsStop)
                    {
                        aTypeText = "记录数量";
                        resultMsg = dThen.ResultMessage;
                        stoped = true;
                        break;
                    }
                }
                else if (aTypeID == "FIELD")
                {
                    foreach (var model in aModels)
                    {
                        bool isSucess = ProDbccItem_Left_ThenField(decipher, model, dbccModel, dThen, aModels, stepPath);

                        if (isSucess && dThen.IsStop)
                        {
                            curModel = model;
                            aTypeText = "字段";
                            resultMsg = dThen.ResultMessage;
                            stoped = true;
                            break;
                        }
                    }
                }
                else if (aTypeID == "FIRST_FIELD")
                {
                    if (aModels.Count > 0)
                    {
                        LModel firstModel = aModels[0];

                        bool isSucess = ProDbccItem_Left_ThenField(decipher, firstModel, dbccModel, dThen, aModels, stepPath);

                        if (isSucess && dThen.IsStop)
                        {
                            curModel = firstModel;
                            aTypeText = "首个字段";
                            resultMsg = dThen.ResultMessage;
                            stoped = true;
                            break;
                        }
                    }
                }

            }

            if (stoped)
            {
                //BizDcHelper.CreateMsgStep(resultMsg, dbccModel, curModel, parentStep);

                stepPath.Errors.Add(resultMsg);
                

                return BizResult.Stoped;
            }

            return BizResult.Resume;
        }




    }
}
