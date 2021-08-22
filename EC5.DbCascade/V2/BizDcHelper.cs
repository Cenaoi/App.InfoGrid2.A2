using EC5.DbCascade.DbCascadeEngine;
using EC5.LcValueEngine;
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
    public static class BizDcHelper
    {

        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="srcRecord"></param>
        /// <param name="model"></param>
        /// <returns>返回修改过的字段集合</returns>
        public static void ExecLCode(LModel model)
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
        /// 创建被阻止的节点
        /// </summary>
        /// <param name="resultMsg">返回的消息</param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        /// <param name="parStep">上级步骤</param>
        private static void CreateMsgStep(string resultMsg, DbccModel dbccModel, LModel srcModel, BizDbStep parStep)
        {
            BizDbStep step = parStep.Childs.Add(BizDbStepType.NONE);
            step.ActionId = dbccModel.ID;

            if (srcModel != null)
            {
                LModelElement modelElem = srcModel.GetModelElement();
                step.Table = modelElem.DBTableName;

                step.Models.Add(srcModel);
            }

            step.CreateLog();

            step.ResultMessage = resultMsg;// "过滤阻止.";
        }






        private static object GetFilterValue_ForFun(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
        {
            return null;
        }

        private static object GetFilterValue_ForTabel(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
        {
            LModelElement modelElem = rightModel.GetModelElement();

            string rCol = filterItem.B_ValueCol;

            if (!modelElem.Fields.ContainsField(rCol))
            {
                throw new Exception(string.Format("实体“{0}”,字段“{0}”不存在", modelElem.DBTableName, rCol));
            }

            object fieldValue = rightModel[rCol];

            return fieldValue;

        }


        /// <summary>
        /// 右边的值处理
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="filterItem"></param>
        /// <param name="rightModel"></param>
        /// <returns></returns>
        public static object GetFilterValue_ForLeft(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
        {
            object bValue = null;

            switch (filterItem.B_ValueMode)
            {
                case DbccValueModes.Fixed:

                    LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);

                    if (modelElem == null)
                    {
                        throw new Exception(string.Format("不存在此实体“{0}”。", dbccModel.L_Table));
                    }

                    LModelFieldElement fieldElem ;

                    if (!modelElem.TryGetField(filterItem.A_Col, out fieldElem))
                    {
                        throw new Exception(string.Format("实体“{0}”不存在此字段“{1}”。", dbccModel.L_Table, filterItem.A_Col));
                    }

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

        /// <summary>
        /// 右边的值处理
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dbccModel"></param>
        /// <param name="filterItem"></param>
        /// <param name="rightModel"></param>
        /// <returns></returns>
        public static object GetFilterValue_ForRight(DbDecipher decipher, DbccModel dbccModel, DbccFilterItem filterItem, LModel rightModel)
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




    }
}
