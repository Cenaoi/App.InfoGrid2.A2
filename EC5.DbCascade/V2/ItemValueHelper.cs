using EC5.DbCascade.DbCascadeEngine;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.V2
{
    public static class ItemValueHelper
    {




        private static object GetItemValue_ForFun(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
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

        private static object GetItemValue_ForTable(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
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


        private static object GetItemValue_ForTable(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
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
        private static object GetItemValue_ForFun(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
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



        private static object GetItemValue_ForUserFunc(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
        {
            object result = null;

            string userFunc = item.R_ValueUserFunc;
            Exception exception = null;

            EC5.LCodeEngine.LcFieldRule lcFRule = new LCodeEngine.LcFieldRule();
            lcFRule.Code = userFunc;
            lcFRule.Field = item.L_Field;

            lcFRule.DbDecipher = decipher;

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



        private static object GetItemValue_ForUserFunc(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
        {

            string userFunc = item.R_ValueUserFunc;

            LModelElement modelElem = rightModel.GetModelElement();
            string modelName = modelElem.DBTableName;


            EC5.LCodeEngine.LcFieldRule lcFRule = new LCodeEngine.LcFieldRule();

            lcFRule.DbDecipher = decipher;
            lcFRule.Field = item.L_Field;
            lcFRule.Code = item.R_ValueUserFunc;
            lcFRule.CodeParse();

            var resultValue = lcFRule.Exec(rightModel);

            lcFRule.Dispose();

            return resultValue;
        }

        public  static object GetItemValue(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModel rightModel)
        {
            object bValue = null;

            switch (item.R_ValueMode)
            {
                case DbccValueModes.Fixed:

                    LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);
                    LModelFieldElement fieldElem = modelElem.Fields[item.L_Field];

                    try
                    {
                        bValue = ModelConvert.ChangeType(item.R_ValueFixed, fieldElem);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"处理联动的“固定值”错误：值“{item.R_ValueFixed}” ID={dbccModel.ID},动作={dbccModel.L_ActCode},{dbccModel.Remark} ",  ex);
                    }


                    break;
                case DbccValueModes.Fun:

                    

                    try
                    {
                        bValue = GetItemValue_ForFun(decipher, dbccModel, item, rightModel);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("处理联动的“函数值”错误：值“{3}” ID={0},动作={1},{2} ",
                            dbccModel.ID, dbccModel.L_ActCode, dbccModel.Remark, item.R_ValueFun), ex);
                    }


                    break;
                case DbccValueModes.Table:

                    

                    try
                    {
                        bValue = GetItemValue_ForTable(decipher, dbccModel, item, rightModel);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("处理联动的“表和字段”错误：值“{3}” ID={0},动作={1},{2} ",
                            dbccModel.ID, dbccModel.L_ActCode, dbccModel.Remark, item.R_ValueTable), ex);
                    }

                    break;
                case DbccValueModes.User_Func:

                    try
                    {
                        bValue = GetItemValue_ForUserFunc(decipher, dbccModel, item, rightModel);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("处理联动的“自定义公式”错误：值“{3}” ID={0},动作={1},{2} ",
                            dbccModel.ID, dbccModel.L_ActCode, dbccModel.Remark, item.R_ValueUserFunc), ex);
                    }
                    break;
            }

            return bValue;
        }

        
        public static object GetItemValue(DbDecipher decipher, DbccModel dbccModel, DbccItem item, LModelList<LModel> rightModels)
        {
            object bValue = null;

            switch (item.R_ValueMode)
            {
                case DbccValueModes.Fixed:

                    LModelElement modelElem = LightModel.GetLModelElement(dbccModel.L_Table);
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

    }
}
