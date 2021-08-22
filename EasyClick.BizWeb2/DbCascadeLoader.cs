using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.DbCascade;
using HWQ.Entity.Filter;
using HWQ.Entity;
using EC5.DbCascade.DbCascadeEngine;
using EC5.DbCascade.Model;
using EC5.DbCascade.Model.DataSet;
using EC5.Utility;

namespace EasyClick.BizWeb2
{

    /// <summary>
    /// 联动数据初始化
    /// </summary>
    public class DbCascadeLoader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        /// <summary>
        /// 联动数据初始化(构造函数)
        /// </summary>
        public DbCascadeLoader()
        {
        }

        /// <summary>
        /// 初始化事务
        /// </summary>
        public void InitDbcc()
        {
            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_ACTION));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("ENABLED", true);
            filter.TSqlOrderBy = "EXEC_SEQ ASC,IG2_ACTION_ID ASC";
           
            filter.Fields = new string[] { "IG2_ACTION_ID" };

            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader, 0);


            int id = 0;

            DbccManager.Acts.Clear();

            DbccModel dModel = null;

            for (int i = 0; i < ids.Length; i++)
            {
                id = ids[i];

                DbccActionSet daSet = new DbccActionSet();
                daSet.Select(decipher, id);


                try
                {
                    dModel = ConvertModel(daSet);

                    if (dModel != null)
                    {
                        DbccManager.Acts.Add(dModel);
                    }
                }
                catch (Exception ex)
                {
                    string modelValues = ToModelFieldValue(daSet.Act);

                    Exception ex2 = new Exception("转化联动实体错误。\n" + modelValues, ex);

                    log.Error(ex2);
                }

            }

        }


        private DbccModel ConvertModel(DbccActionSet daSet)
        {
            IG2_ACTION act = daSet.Act;

            if (StringUtil.IsBlank(act.L_TABLE))
            {
                return null;
            }


            DbccModel dModel = new DbccModel();

            dModel.ID = act.IG2_ACTION_ID;
            dModel.Remark = act.REMARK;

            dModel.Enabled = act.ENABLED;

            dModel.L_Table = act.L_TABLE;
            dModel.L_ActCode = act.L_ACT_CODE;
            dModel.L_NotExist_Then = act.L_NOT_EXIST_THEN; 

            dModel.R_Table = act.R_TABLE;
            dModel.R_ActCode = act.R_ACT_CODE;

            dModel.L_IsSubFilter = act.L_IS_SUB_FILTER;
            dModel.R_IsSubFilter = act.R_IS_SUB_FILTER;

            dModel.ListenLogic = EnumUtil.Parse<DbccLogic>(act.LISTEN_AO, DbccLogic.OR);



            foreach (IG2_ACTION_LISTEN item in daSet.ListenFields)
            {
                try
                {
                    DbccListen dListen = ConvertForListen(item);

                    dModel.ListenFields.Add(dListen);
                }
                catch (Exception ex)
                {
                    string modelValues = ToModelFieldValue(item);

                    throw new Exception("转换 “监听”数据错误。\n" + modelValues, ex);
                }
            }


            foreach (IG2_ACTION_ITEM item in daSet.Items)
            {
                try
                {
                    DbccItem dItem = ConvertForItem(item);
                    dModel.Items.Add(dItem);
                }
                catch (Exception ex)
                {
                    string modelValues = ToModelFieldValue(item);

                    throw new Exception("转换 “Item”数据错误。\n" + modelValues, ex);
                }
            }

            foreach (IG2_ACTION_THEN item in daSet.Thens)
            {
                try
                {
                    DbccThen dItem = ConvertForThen(item);

                    dModel.Thens.Add(dItem);
                }
                catch (Exception ex)
                {
                    string modelValues = ToModelFieldValue(item);

                    throw new Exception("转换 “Then”数据错误。\n" + modelValues, ex);
                }
            }

            foreach (IG2_ACTION_FILTER actFilter in daSet.FilterLeft)
            {
                try
                {
                    DbccFilterItem dfItem = ConvertForFilter(actFilter);
                    dModel.FilterLeft.Add(dfItem);
                }
                catch (Exception ex)
                {
                    string modelValues = ToModelFieldValue(actFilter);

                    throw new Exception("转换 “左边过滤”数据错误。" + modelValues, ex);
                }
            }

            foreach (IG2_ACTION_FILTER actFilter in daSet.FilterRight)
            {
                try
                {
                    DbccFilterItem dfItem = ConvertForFilter(actFilter);
                    dModel.FilterRight.Add(dfItem);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("转换 “右边过滤”数据错误。"), ex);
                }
            }



            return dModel;
        }

        /// <summary>
        /// 输出各个字段的变量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ToModelFieldValue(LightModel model)
        {
            StringBuilder sb = new StringBuilder();

            LModelElement modelElem = LightModel.GetLModelElement(model.GetType());

            foreach (var item in modelElem.Fields)
            {
                sb.AppendFormat("@{0} = {1}", item.DBField, model[item]).AppendLine();    
            }

            return sb.ToString();

        }

        
        /// <summary>
        /// 转换实体 IG2_ACTION_ITEM
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private DbccItem ConvertForItem(IG2_ACTION_ITEM item)
        {
            DbccItem dItem = new DbccItem();
            dItem.L_Field = item.L_COL;


            dItem.R_ValueMode = EnumUtil.Parse<DbccValueModes>(item.R_VALUE_MODE, DbccValueModes.ALL, true);

            dItem.R_ValueFixed = item.R_VALUE_FIXED;

            dItem.R_ValueFun = item.R_VALUE_FUN;

            dItem.R_ValueTable = item.R_VALUE_TABLE;
            dItem.R_ValueCol = item.R_VALUE_COL;

            dItem.R_ValueTotalFun = item.R_VALUE_TOTAL_FUN;

            dItem.L_ActCode = item.L_ACT_CODE;

            dItem.R_ValueUserFunc = item.R_VALUE_USER_FUNC;


            return dItem;
        }


        private DbccThen ConvertForThen(IG2_ACTION_THEN item)
        {
            DbccThen dThen = new DbccThen();

            dThen.A_Field = item.A_FIELD;
            dThen.A_FieldText = item.A_FIELD_TEXT;
            dThen.A_Login = ModelConvert.ToLogic(item.A_LOGIN);// EnumUtil.Parse<Logic>(item.A_LOGIN, Logic.Equality, true);
            dThen.A_TotalFun = item.A_TOTAL_FUN;
            dThen.A_TypeID = item.A_TYPE_ID;
            dThen.A_Value = item.A_VALUE;
            dThen.IsStop = item.IS_STOP;
            dThen.ResultMessage = item.RSULT_MESSAGE;

            return dThen;
        }


        /// <summary>
        /// 转换 IG2_ACTION_FILTER
        /// </summary>
        /// <param name="actFilter"></param>
        /// <returns></returns>
        private DbccFilterItem ConvertForFilter(IG2_ACTION_FILTER actFilter)
        {
            DbccFilterItem item = new DbccFilterItem();

            item.A_Col = actFilter.A_COL;
            item.A_Logic = ModelConvert.ToLogic(actFilter.A_LOGIN);

            item.B_ValueMode = EnumUtil.Parse<DbccValueModes>(actFilter.B_VALUE_MODE, DbccValueModes.ALL, true);

            item.B_ValueFixed = actFilter.B_VALUE_FIXED;
            item.B_ValueFun = actFilter.B_VALUE_FUN;
            item.B_ValueTable = actFilter.B_VALUE_TABLE;
            item.B_ValueCol = actFilter.B_VALUE_COL;

            item.B_ValueUserFunc = actFilter.B_VALUE_USER_FUNC;

            return item;
        }

        /// <summary>
        /// 转换实体 IG2_ACTION_LISTEN
        /// </summary>
        /// <param name="actListen"></param>
        /// <returns></returns>
        private DbccListen ConvertForListen(IG2_ACTION_LISTEN actListen)
        {
            DbccListen item = new DbccListen();

            item.DBField = actListen.R_DB_FIELD;
            item.Enabled = actListen.LISTEN_ENABLED;
            item.FieldText = actListen.R_FIELD_TEXT;

            item.ValueFrom = actListen.VALUE_FROM;
            item.ValueTo = actListen.VALUE_TO;

            return item;
        }

    }



}
