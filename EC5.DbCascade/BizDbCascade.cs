using System;
using System.Collections.Generic;
using System.Text;
using EC5.DbCascade.DbCascadeEngine;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity;
using EC5.Utility;
using System.ComponentModel;
using EC5.LcValueEngine;


namespace EC5.DbCascade
{




    /// <summary>
    /// 业务联动数据操作
    /// </summary>
    public class BizDbCascade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 数据库删除操作
        /// </summary>
        public const string DELETE = "DELETE";

        /// <summary>
        /// 数据库更新操作
        /// </summary>
        public const string UPDATE = "UPDATE";

        /// <summary>
        /// 数据库插入操作
        /// </summary>
        public const string INSERT = "INSERT";

        /// <summary>
        /// 数据库全部操作
        /// </summary>
        public const string ALL = "ALL";

        /// <summary>
        /// 更新事件
        /// </summary>
        public event ObjectEventHandler Updating;

        /// <summary>
        /// 触发更新事件
        /// </summary>
        /// <param name="model">实体</param>
        protected void OnUpdating(object model)
        {
            if (Updating != null)
            {
                Updating(this, new ObjectEventArgs(model));
            }
        }




        /// <summary>
        /// 开始关联操作
        /// </summary>
        /// <param name="decipher">数据操作</param>
        /// <param name="actCode">操作代码。INSERT-插入操作，UPDATE-更新操作, DELETE-删除操作</param>
        /// <param name="rightModel">实体</param>
        public BizDbStep StartCascade(DbDecipher decipher, string actCode, LModel rightModel)
        {
            if (rightModel == null)
            {
                throw new Exception("数据源不存在,无法进行关联操作");
            }


            actCode = actCode.ToUpper();

            LModelElement modelElem = rightModel.GetModelElement();

            BizDbStep step = new BizDbStep(actCode, rightModel);
            step.Table = modelElem.DBTableName;
            step.ActionId = 0;
            step.CreateLog();

            //开始处理联动操作的集合
            ProDBccList(decipher, actCode, modelElem, rightModel, step);

            return step;
        }

        /// <summary>
        /// 处理被监听的对象集 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="actCode"></param>
        /// <param name="modelElem"></param>
        /// <param name="rightModel"></param>
        /// <param name="parentStep"></param>
        private void ProDBccList(DbDecipher decipher, string actCode, LModelElement modelElem, LModel rightModel, BizDbStep parentStep)
        {

            DbccModelCollection items = DbccManager.Acts.GetModels(modelElem.DBTableName, actCode);

            if (items == null)
            {
                return;
            }

            foreach (DbccModel item in items)
            {
                try
                {
                    ProDBccItem(decipher, item, rightModel, parentStep);
                }
                catch (Exception ex)
                {
                    throw new Exception("第" + parentStep.Depth + "层," + string.Format("执行联动规则错误：规则ID={0}, 备注={1}, \n 左表={2}:{3}, 描述={4}, \n右表={5}:{6}, 描述={7}",
                        item.ID, item.Remark, 
                        item.L_ActCode, item.L_Table, item.L_Display,
                        item.R_ActCode, item.R_Table, item.R_Display),ex); 
                }
            }


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbccModel"></param>
        /// <param name="leftModel"></param>
        private void ProDBccItem(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStep parentStep)
        {
            string actCode = dbccModel.L_ActCode.ToUpper();

            switch (actCode)
            {
                case BizDbCascade.UPDATE:
                    ProDBccItem_Update(decipher, dbccModel, srcModel, parentStep);
                    break;
                case BizDbCascade.INSERT:
                    ProDBccItem_Insert(decipher, dbccModel, srcModel, parentStep);
                    break;
                case BizDbCascade.DELETE:
                    ProDBccItem_Delete(decipher, dbccModel, srcModel, parentStep);
                    break;
                case BizDbCascade.ALL:
                    ProDBccItem_All(decipher, dbccModel, srcModel, parentStep);
                    break;

            }
        }




        private object GetFilterValue_ForFun(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
        {
            return null;
        }

        private object GetFilterValue_ForTabel(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
        {
            LModelElement modelElem = rightModel.GetModelElement();

            string rCol = filterItem.B_ValueCol;

            if (!modelElem.Fields.ContainsField(rCol))
            {
                throw new Exception(string.Format("实体“{0}”,字段“{0}”不存在",modelElem.DBTableName, rCol));
            }

            object fieldValue = rightModel[rCol];

            return fieldValue;

        }

        private object GetFilterValue(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
        {
            object bValue = null;

            switch (filterItem.B_ValueMode)
            {
                case DbccValueModes.Fixed:

                    LModelElement modelElem = LightModel.GetLModelElement(dbccModel.R_Table);

                    if (modelElem == null)
                    {
                        throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.R_Table));
                    }

                    LModelFieldElement fieldElem = modelElem.Fields[filterItem.A_Col];

                    bValue = ModelConvert.ChangeType(filterItem.B_ValueFixed, fieldElem);


                    break;
                case DbccValueModes.Fun:

                    bValue = GetFilterValue_ForFun(decipher, dbccModel, filterItem, rightModel);

                    break;
                case DbccValueModes.Table:

                    bValue = GetFilterValue_ForTabel(decipher, dbccModel, filterItem, rightModel); 
                    
                    break;
            }

            return bValue;
        }




        private object GetItemValue_ForFun(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
        {
            object result = null;

            string fun = item.R_ValueFun.ToUpper();

            switch (fun)
            {
                case "TIME_NOW": result = DateTime.Now; break;
                case "DATE_NOW": result = DateTime.Today; break;
            }

            return result;
        }

        private object GetItemValue_ForTable(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
        {
            LModelElement modelElem = rightModel.GetModelElement();
            LModelFieldElement fieldElem = null;

            string rCol = item.R_ValueCol;

            if (!modelElem.TryGetField(rCol, out fieldElem))
            {
                throw new Exception(string.Format("表'{0}'的字段'{1}'不存在", modelElem.DBTableName, rCol));
            }

            object fieldValue = rightModel[fieldElem];

            return fieldValue;
        }

        
        private object GetItemValue_ForTable(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
        {

            if (!StringUtil.IsBlank(item.R_ValueTotalFun))
            {
                object result = null;

                string fun = item.R_ValueTotalFun.ToUpper();

                switch (fun)
                {
                    case "SUM": result = BizDbMath.Sum(item.R_ValueCol, rightModels); break;
                    case "AVG": result = BizDbMath.Avg(item.R_ValueCol, rightModels); break;
                    case "COUNT": result = rightModels.Count; break;
                }

                return result;
            }
            else
            {

                //如果没有指定集合的统计函数。。就取第一条记录
                if (rightModels.Count > 0)
                {
                    LModel rModel = rightModels[0];
                    LModelElement modelElem = rModel.GetModelElement();

                    string rCol = item.R_ValueCol;

                    if (!modelElem.Fields.ContainsField(rCol))
                    {
                        throw new Exception(string.Format("表‘{0}’的字段‘{1}’不存在。", modelElem.DBTableName, rCol));
                    }

                    object fieldValue = rModel[rCol];

                    return fieldValue;
                }
            }

            return null;
            
        }

        /// <summary>
        /// 获取函数值
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="item"></param>
        /// <param name="rightModels"></param>
        /// <returns></returns>
        private object GetItemValue_ForFun(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
        {
            object result = null;

            string fun = item.R_ValueFun.ToUpper();

            switch (fun)
            {
                case "TIME_NOW": result = DateTime.Now; break;
                case "DATE_NOW": result = DateTime.Today; break;
                default: throw new Exception(string.Format("参数无效\"{0}\".", item.R_ValueFun));
            }

            return result;
        }



        private object GetItemValue_ForUserFunc(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
        {
            object result = null;

            string userFunc = item.R_ValueUserFunc;
            Exception exception = null;

            EC5.LCodeEngine.LcFieldRule lcFRule = new LCodeEngine.LcFieldRule();
            lcFRule.Code = userFunc;
            lcFRule.Field = item.L_Field;

            lcFRule.CodeParse();

            try
            {
                foreach (LModel model in rightModels)
                {
                    try
                    {
                        result = lcFRule.Exec(model);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception("处理赋值错误:" + model.GetModelElement().DBTableName, ex2);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = new Exception("联动自定义函数错误。", ex);
            }

            lcFRule.Dispose();

            if (exception != null) { throw exception; }

            return result;
        }



        private object GetItemValue_ForUserFunc(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
        {
            object result = null;

            string userFunc = item.R_ValueUserFunc;

            LModelElement modelElem = rightModel.GetModelElement();
            string modelName = modelElem.DBTableName;


            EC5.LCodeEngine.LcFieldRule lcFRule = new LCodeEngine.LcFieldRule();

            lcFRule.Field = item.L_Field;
            lcFRule.Code = item.R_ValueUserFunc;
            lcFRule.CodeParse();

            var resultValue = lcFRule.Exec(rightModel);

            return resultValue;


        }

        private object GetItemValue(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
        {
            object bValue = null;

            switch (item.R_ValueMode)
            {
                case DbccValueModes.Fixed:
                    
                    LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

                    if (modelElem == null)
                    {
                        throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.L_Table));
                    }

                    LModelFieldElement fieldElem = modelElem.Fields[item.L_Field];

                    try
                    {
                        bValue = ModelConvert.ChangeType(item.R_ValueFixed, fieldElem);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("处理联动的“固定值”错误：值“{3}” ID={0},动作={1},{2} ",
                            dbccModel.ID, dbccModel.L_ActCode, dbccModel.Remark,item.R_ValueFixed), ex);
                    }


                    break;
                case DbccValueModes.Fun:

                    bValue = GetItemValue_ForFun(decipher, dbccModel, item, rightModel);

                    break;
                case DbccValueModes.Table:

                    bValue = GetItemValue_ForTable(decipher, dbccModel, item, rightModel);

                    break;
                case DbccValueModes.User_Func:

                    bValue = GetItemValue_ForUserFunc(decipher, dbccModel, item, rightModel);

                    break;
            }

            return bValue;
        }

        private object GetItemValue(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
        {
            object bValue = null;

            switch (item.R_ValueMode)
            {
                case DbccValueModes.Fixed:

                    LModelElement modelElem = LightModel.GetLModelElement(dbccModel.R_Table);

                    if (modelElem == null)
                    {
                        throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.R_Table));
                    }

                    LModelFieldElement fieldElem = modelElem.Fields[item.L_Field];

                    bValue = ModelConvert.ChangeType(item.R_ValueFixed, fieldElem);

                    break;
                case DbccValueModes.Fun:
                    bValue = GetItemValue_ForFun(decipher, dbccModel, item, rightModels);
                    break;
                case DbccValueModes.Table:

                    bValue = GetItemValue_ForTable(decipher, dbccModel, item, rightModels);

                    break;
                case DbccValueModes.User_Func:

                    bValue = GetItemValue_ForUserFunc(decipher, dbccModel, item, rightModels);

                    break;
            }

            return bValue;
        }


        private void ProDBccItem_Delete(DbDecipher decipher, DbccModel dbccModel, LModel rightModel, BizDbStep parentStep)
        {
            bool isChanged = IsChangedForUpdate(dbccModel, rightModel,parentStep);

            if (!isChanged)
            {
                CreateMsgStep("监听阻止,字段没有发生变化。", dbccModel, rightModel, parentStep);
                return;
            }
            
            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
            {
                object bValue = GetFilterValue(decipher, dbccModel, filterItem, rightModel);

                filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);

            foreach (LModel aModel in aModels)
            {
                LModelElement modelElem = aModel.GetModelElement();

                decipher.DeleteModel(aModel);

                //记录步骤
                BizDbStep step = parentStep.Childs.Add(dbccModel.L_ActCode, aModel);
                step.Table = modelElem.DBTableName;
                step.ActionId = dbccModel.ID;
                step.CreateLog();

                ProDBccList(decipher, dbccModel.L_ActCode, modelElem, aModel, step);
            }


            ////真正删除记录
            //foreach (LModel aModel in aModels)
            //{
            //}
        }


        private void ProDBccItem_All(DbDecipher decipher, DbccModel dbccModel, LModel rightModel, BizDbStep parentStep)
        {
            bool isChanged = IsChangedForUpdate(dbccModel, rightModel,parentStep);

            if (!isChanged)
            {
                CreateMsgStep("监听阻止,字段没有发生变化。", dbccModel, rightModel, parentStep);
                return;
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
                    CreateMsgStep("右边过滤阻止，条件不成立", dbccModel, rightModel, parentStep);
                    return;
                }
            }


            ProDBccItem_Left(decipher, dbccModel, rightModel, parentStep);


        }


        private LModelList<LModel> GetModel_BySubFilterR(DbDecipher decipher, DbccModel dbccModel, LModel srcModel)
        {

            LightModelFilter filterRight = new LightModelFilter(dbccModel.R_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterRight)
            {
                object bValue = GetFilterValue(decipher, dbccModel, filterItem, srcModel);

                filterRight.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> bModels = decipher.GetModelList(filterRight);

            return bModels;
        }

        /// <summary>
        /// 过滤右边
        /// </summary>
        private bool FilterR_SrcModel(DbDecipher decipher, DbccModel dbccModel, LModel srcModel)
        {
            bool isCancel = false;

            LModelElement srcModelElem = srcModel.GetModelElement();

            foreach (DbccFilterItem filterItem in dbccModel.FilterRight)
            {

                object bValue = GetFilterValue(decipher, dbccModel, filterItem, srcModel);

                object aValue = srcModel[filterItem.A_Col];

                LModelFieldElement srcFieldElem = srcModelElem.Fields[filterItem.A_Col];

                bValue = ModelConvert.ChangeType(bValue, srcFieldElem);
                aValue = ModelConvert.ChangeType(aValue, srcFieldElem);

                bool isLogic = false;

                if (srcFieldElem.IsNumber)
                {
                    isLogic = BizDbMath.LogicAB(aValue, filterItem.A_Logic, bValue);
                }
                else if (srcFieldElem.DBType == LMFieldDBTypes.String)
                {
                    string fValue = (string)aValue;
                    string bFValue = (string)bValue;

                    isLogic = BizDbMath.LogicABString(fValue,filterItem.A_Logic, bFValue);
                    
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
        private bool IsChangedForUpdate(DbccModel dbccModel, LModel srcModel, BizDbStep parStep)
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
            bool isValueChanged ;

            LModelElement modelElem = srcModel.GetModelElement();
            
            LModelFieldElementCollection fieldElems  = modelElem.Fields;

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
        private bool IsValueChanged(DbccListen dlField, LModelFieldElementCollection fieldElems,LModel srcModel)
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

                bool isFromEqual = BizDbMath.LogicAB(valFrom, Logic.Equality, mSrcValue);

                bool isToEqual = BizDbMath.LogicAB(valTo, Logic.Equality, mNowValue);

                return (isFromEqual && isToEqual);
            }
            
            //原为空，目标不为空
            if (isEmptyFrom && !isEmptyTo)
            {
                object valTo = ModelConvert.ChangeType(dlField.ValueTo, fieldElem);

                object mSrcValue = srcModel.GetOriginalValue(dlField.DBField);  //实体原始值

                bool isToEqual = BizDbMath.LogicAB(mSrcValue, Logic.Equality, valTo);

                return isToEqual;
            }
            
            //原不空，目标空。
            if (!isEmptyFrom && isEmptyTo)
            {
                object valFrom = ModelConvert.ChangeType(dlField.ValueFrom, fieldElem);

                object mNowValue = srcModel[dlField.DBField];

                bool isFromEqual = BizDbMath.LogicAB(valFrom, Logic.Equality, mNowValue);

                return isFromEqual;
            }

            return false;
        }

        private void ProDBccItem_Update(DbDecipher decipher, DbccModel dbccModel, LModel srcModel,BizDbStep parentStep)
        {
            //LModelList<LModel> bModels = null;

            bool isChanged = IsChangedForUpdate(dbccModel, srcModel, parentStep);

            if (!isChanged)
            {
                CreateMsgStep("监听阻止,字段没有发生变化。", dbccModel, srcModel, parentStep);
                return;
            }


            if (dbccModel.R_IsSubFilter )
            {
                //bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);
            }
            else
            {
                bool isCancel = FilterR_SrcModel(decipher, dbccModel, srcModel);

                if (isCancel)
                {
                    CreateMsgStep("右边过滤阻止，条件不成立", dbccModel, srcModel, parentStep);
                    return;
                }
            }


            ProDBccItem_Left(decipher, dbccModel, srcModel, parentStep);

        }






        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="srcRecord"></param>
        /// <param name="model"></param>
        /// <returns>返回修改过的字段集合</returns>
        private void ExecLCode( LModel model)
        {

            LModelElement modelElem = model.GetModelElement();

            string modelName = modelElem.DBTableName;

            EC5.LCodeEngine.LcModel cModel = null;

            if (EC5.LCodeEngine.LcModelManager.Models.TryGetValue(modelName, out cModel))
            {

                string[] blemishFields = LightModel.GetBlemishPropNames(model);

                if (blemishFields != null)
                {
                    string[] updateFs;  //更新的字段集

                    foreach (string bF in blemishFields)
                    {
                        updateFs = null;

                        if (!cModel.IsListen(bF))
                        {
                            continue;
                        }

                        cModel.Exec(bF, model, out updateFs);
                    }
                }

            }

            
            LcValueManager.Exec(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        private void ProDBccItem_Left_INSERT(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, BizDbStep parentStep)
        {

            LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);


            if (modelElem == null)
            {
                throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.L_Table));
            }

            LModel model = new LModel(modelElem);

            model.SetTakeChange(true);

            foreach (DbccItem item in dbccModel.Items)
            {
                model[item.L_Field] = GetItemValue(decipher, dbccModel, item, srcModel);
            }

            try
            {
                ExecLCode(model);
            }
            catch (Exception ex)
            {
                throw new Exception("执行动态代码错误。", ex);
            }


            //#region 执行 Then 条件

            //BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, parentStep);

            //if (result == BizResult.Stoped)
            //{
            //    return;
            //}

            //#endregion


            decipher.InsertModel(model);

            BizDbStep step =  parentStep.Childs.Add(dbccModel.L_ActCode, model);
            step.Table = modelElem.DBTableName;
            step.ActionId = dbccModel.ID;
            step.CreateLog();

            ProDBccList(decipher, dbccModel.L_ActCode, modelElem, model, step);
        }



        /// <summary>
        /// 常规更新
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        private void ProDBccItem_Left_UPDATE_Common(DbDecipher decipher, DbccModel dbccModel, LModel srcModel,BizDbStep parentStep)
        {





            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
            {
                object bValue = GetFilterValue(decipher, dbccModel, filterItem, srcModel);

                filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);

            #region 执行 Then 条件

            BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, parentStep);

            if (result == BizResult.Stoped)
            {
                return;
            }

            #endregion

            if (aModels.Count == 0 && dbccModel.L_NotExist_Then == "A")
            {
                LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

                LModel model = new LModel(modelElem);

                foreach (DbccItem dbccItem in dbccModel.Items)
                {
                    if (dbccItem.L_ActCode == "E")
                    {
                        continue;
                    }


                    object value = GetItemValue(decipher, dbccModel, dbccItem, srcModel);
                    model[dbccItem.L_Field] = value;

                }

                try
                {
                    ExecLCode(model);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行动态代码错误。", ex);
                }

                decipher.InsertModel(model);


                //记录步骤
                BizDbStep step = parentStep.Childs.Add(BizDbStepType.INSERT, model);
                step.Table = modelElem.DBTableName;
                step.ActionId = dbccModel.ID;
                step.CreateLog();

                ProDBccList(decipher, "INSERT", modelElem, model, step);
            }
            else
            {

                foreach (LModel aModel in aModels)
                {
                    LModelElement modelElem = srcModel.GetModelElement();

                    aModel.SetTakeChange(true);

                    foreach (DbccItem dbccItem in dbccModel.Items)
                    {
                        object value = GetItemValue(decipher, dbccModel, dbccItem, srcModel);

                        aModel[dbccItem.L_Field] = value;
                    }

                    try
                    {
                        ExecLCode(aModel);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("执行动态代码错误。", ex);
                    }

                    OnUpdating(aModel);

                    decipher.UpdateModel(aModel, true);

                    LModelElement aModelElem = aModel.GetModelElement();

                    BizDbStep step = parentStep.Childs.Add(BizDbStepType.UPDATE, aModel);
                    step.Table = aModelElem.DBTableName;
                    step.ActionId = dbccModel.ID;
                    step.CreateLog();

                    ProDBccList(decipher, dbccModel.L_ActCode, aModelElem, aModel, step);
                }
            }
        }

        /// <summary>
        /// 子项更新
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        private void ProDBccItem_Left_UPDATE_SubFilter(DbDecipher decipher, DbccModel dbccModel, LModel srcModel, LModelList<LModel> bModels, BizDbStep parentStep)
        {


            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
            {
                object bValue = GetFilterValue(decipher, dbccModel, filterItem, srcModel);

                filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);

            if (aModels.Count == 0 && dbccModel.L_NotExist_Then == "A")
            {
                LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

                if (modelElem == null)
                {
                    throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.L_Table));
                }


                LModel model = new LModel(modelElem);

                foreach (DbccItem dbccItem in dbccModel.Items)
                {
                    if (dbccItem.L_ActCode == "E")
                    {
                        continue;
                    }


                    object value = GetItemValue(decipher, dbccModel, dbccItem, srcModel);
                    model[dbccItem.L_Field] = value;

                }



                try
                {
                    ExecLCode(model);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行动态代码错误。", ex);
                }

                decipher.InsertModel(model);


                //记录步骤
                BizDbStep step = parentStep.Childs.Add(BizDbStepType.INSERT, model);
                step.Table = modelElem.DBTableName;
                step.ActionId = dbccModel.ID;
                step.CreateLog();

                ProDBccList(decipher,"INSERT", modelElem, model, step);
            }
            else
            {

                foreach (LModel aModel in aModels)
                {
                    LModelElement modelElem = srcModel.GetModelElement();

                    aModel.SetTakeChange(true);

                    foreach (DbccItem dbccItem in dbccModel.Items)
                    {
                        object value = GetItemValue(decipher, dbccModel, dbccItem, bModels);

                        aModel[dbccItem.L_Field] = value;
                    }

                    #region 执行 Then 条件

                    BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, parentStep);

                    if (result == BizResult.Stoped)
                    {
                        return;
                    }

                    #endregion

                    try
                    {
                        ExecLCode(aModel);
                    }
                    catch (Exception ex)
                    {
                        log.Error("执行动态代码错误。", ex);
                    }


                    OnUpdating(aModel);

                    decipher.UpdateModel(aModel, true);

                    LModelElement aModelElem = aModel.GetModelElement();

                    //记录步骤
                    BizDbStep step = parentStep.Childs.Add(BizDbStepType.UPDATE, aModel);
                    step.Table = aModelElem.DBTableName;
                    step.ActionId = dbccModel.ID;
                    step.CreateLog();

                    ProDBccList(decipher, dbccModel.L_ActCode, aModelElem, aModel, step);
                }
            }

        }


        private bool ProDbccItem_Left_ThenField(DbDecipher decipher, LModel firstModel, DbccModel dbccModel,DbccThen dThen, LModelList<LModel> aModels, BizDbStep parentStep)
        {
            bool stoped = false;

            string resultMsg = null;

            LModelElement modelElem = firstModel.GetModelElement();
            LModelFieldElement fieldElem = modelElem.Fields[dThen.A_Field];

            object fieldValue = firstModel[dThen.A_Field];

            if (fieldElem.IsNumber)
            {
                decimal aValue = StringUtil.ToDecimal(dThen.A_Value);

                decimal fValue = (decimal)ModelConvert.ChangeType(fieldValue, LMFieldDBTypes.Decimal, true);

                bool isSucess = BizDbMath.LogicAB(fValue, dThen.A_Login, aValue);

                if (isSucess && dThen.IsStop)
                {
                    resultMsg = dThen.ResultMessage;
                    stoped = true;
                }
            }
            else if (fieldElem.DBType == LMFieldDBTypes.String)
            {
                string fValue = (string)fieldValue;

                bool result = BizDbMath.LogicABString(fValue,dThen.A_Login, dThen.A_Value);

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
                BizDbStep step = new BizDbStep(BizDbStepType.NONE);
                step.ResultMessage = "条件阻止:" + resultMsg;
                step.Table = dbccModel.L_Table;
                parentStep.Childs.Add(step);

            }


            return stoped;
        }


        private BizResult ProDbccItem_Left_Then(DbDecipher decipher, DbccModel dbccModel, LModelList<LModel> aModels, BizDbStep parentStep)
        {

            bool stoped = false;

            string resultMsg = null;

            foreach (DbccThen dThen in dbccModel.Thens)
            {
                string aTypeID = dThen.A_TypeID.ToUpper();

                if (aTypeID == "COUNT")
                {
                    decimal aValue = StringUtil.ToDecimal(dThen.A_Value);

                    bool isSucess = BizDbMath.LogicAB(aModels.Count, dThen.A_Login, aValue);

                    if (isSucess && dThen.IsStop)
                    {
                        resultMsg = dThen.ResultMessage;
                        stoped = true;
                        break;
                    }
                }
                else if (aTypeID == "FIELD")
                {
                    foreach (var model in aModels)
                    {
                        bool isSucess = ProDbccItem_Left_ThenField(decipher, model, dbccModel, dThen, aModels, parentStep);

                        if (isSucess && dThen.IsStop)
                        {
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

                        bool isSucess = ProDbccItem_Left_ThenField(decipher, firstModel, dbccModel, dThen, aModels, parentStep);

                        if (isSucess && dThen.IsStop)
                        {
                            resultMsg = dThen.ResultMessage;
                            stoped = true;
                            break;
                        }
                    }
                }

            }

            if (stoped)
            {
                return BizResult.Stoped;
            }

            return BizResult.Resume;
        }


        private void ProDBccItem_Left_DELETE(DbDecipher decipher, DbccModel dbccModel, LModel rightModel, BizDbStep parentStep)
        {


            LightModelFilter filterLeft = new LightModelFilter(dbccModel.L_Table);

            foreach (DbccFilterItem filterItem in dbccModel.FilterLeft)
            {
                object bValue = GetFilterValue(decipher, dbccModel, filterItem, rightModel);

                filterLeft.And(filterItem.A_Col, bValue, filterItem.A_Logic);
            }


            LModelList<LModel> aModels = decipher.GetModelList(filterLeft);


            #region 执行 Then 条件

            BizResult result = ProDbccItem_Left_Then(decipher, dbccModel, aModels, parentStep);

            if (result == BizResult.Stoped)
            {
                return;
            }

            #endregion

            foreach (LModel aModel in aModels)
            {
                decipher.DeleteModel(aModel);   //真正删除记录

                LModelElement modelElem = aModel.GetModelElement();

                //记录步骤
                BizDbStep step = parentStep.Childs.Add(BizDbStepType.DELETE, aModel);
                step.Table = modelElem.DBTableName;
                step.ActionId = dbccModel.ID;
                step.CreateLog();

                ProDBccList(decipher, dbccModel.L_ActCode, modelElem, aModel, step);
            }


        }



        /// <summary>
        /// 处理左边事件
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        private void ProDBccItem_Left(DbDecipher decipher, DbccModel dbccModel, LModel srcModel,BizDbStep parentStep)
        {
            string leftActCode = dbccModel.L_ActCode.ToUpper();

            switch (leftActCode)
            {
                case BizDbCascade.INSERT:

                    ProDBccItem_Left_INSERT(decipher, dbccModel, srcModel, parentStep);

                    break;
                case BizDbCascade.DELETE:

                    ProDBccItem_Left_DELETE(decipher, dbccModel, srcModel, parentStep);

                    break;
                case BizDbCascade.UPDATE:

                    if (dbccModel.R_IsSubFilter)
                    {
                        LModelList<LModel> bModels = GetModel_BySubFilterR(decipher, dbccModel, srcModel);

                        ProDBccItem_Left_UPDATE_SubFilter(decipher, dbccModel, srcModel, bModels, parentStep);
                    }
                    else
                    {
                        ProDBccItem_Left_UPDATE_Common(decipher, dbccModel, srcModel, parentStep);
                    }

                    break;
            }
        }



        private void ProDBccItem_Insert(DbDecipher decipher, DbccModel dbccModel, LModel srcModel,BizDbStep parentStep)
        {

            //LModelList<LModel> bModels = null;

            bool isChanged = IsChangedForUpdate(dbccModel, srcModel,parentStep);

            if (!isChanged)
            {
                CreateMsgStep("监听阻止,字段没有发生变化。", dbccModel, srcModel, parentStep);

                return;
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
                    CreateMsgStep("右边过滤阻止，条件不成立", dbccModel, srcModel, parentStep);
                    return;
                }
            }

            ProDBccItem_Left(decipher, dbccModel, srcModel, parentStep);

        }

        /// <summary>
        /// 创建被阻止的节点
        /// </summary>
        /// <param name="resultMsg"></param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        /// <param name="parStep"></param>
        private void CreateMsgStep(string resultMsg, DbccModel dbccModel, LModel srcModel, BizDbStep parStep)
        {
            BizDbStep step = parStep.Childs.Add(BizDbStepType.NONE);
            step.ActionId = dbccModel.ID;
            step.Table = srcModel.GetModelElement().DBTableName;

            step.Models.Add(srcModel);

            step.CreateLog();

            step.ResultMessage = resultMsg;// "过滤阻止.";
        }



    }


}
