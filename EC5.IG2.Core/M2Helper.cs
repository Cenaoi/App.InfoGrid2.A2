using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using EC5.SystemBoard;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EC5.IG2.Core
{
    



    /// <summary>
    /// 临时对象。用于不明确的，未归类的函数。
    /// </summary>
    public static class M2Helper
    {
        /// <summary>
        /// 获取用户对应的菜单Id
        /// </summary>
        /// <returns></returns>
        public static int[] GetUserMenuId()
        {
            EcContext context = EcContext.Current;
            EcUserState userState = context.User;


            string userCode = userState.ExpandPropertys["USER_CODE"];

            int[] menuIds = GetSecFunIds(userCode);

            return menuIds;
        }

        /// <summary>
        /// 根据用户 ID ，获取能显示的菜单 ID
        /// </summary>
        /// <param name="logicCode"></param>
        /// <returns></returns>
        private static int[] GetSecFunIds(string loginCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter(typeof(SEC_USER_FUN));
            filter.And("LOGIN_CODE", loginCode);
            filter.And("CHECK_STATE_ID", new int[] { 1, 2 }, Logic.In);
            filter.Fields = new string[] { "SEC_FUN_DEF_ID" };
            filter.Distinct = true;



            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader);

            return ids;

        }
    }


    /// <summary>
    /// 实体映射
    /// </summary>
    public static class M2MapHelper
    {

        /// <summary>
        /// 映射对象
        /// </summary>
        /// <param name="srcModel">原实体</param>
        /// <param name="mapId">映射ID</param>
        /// <param name="toModel">目标实体</param>
        public static void MapTable(LightModel srcModel, int mapId, LightModel toModel)
        {
            Exception exception = null;

            LModelElement srcModelElem = (srcModel is LModel)?((LModel)srcModel).GetModelElement(): LightModel.GetLModelElement(srcModel.GetType());
            LModelElement toModelElem = (toModel is LModel) ? ((LModel)toModel).GetModelElement() : LightModel.GetLModelElement(toModel.GetType());


            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            try
            {
                MapSet mSet = MapSet.SelectSID_0(decipher, mapId);


                IG2_MAP map = mSet.Map;
                List<IG2_MAP_COL> mapCols = mSet.Cols;

                if (map == null)
                {
                    throw new Exception("映射规则已经失效! MAP_ID = " + mapId);
                }

                int count = 0;

                foreach (IG2_MAP_COL mapCol in mapCols)
                {
                    bool isMap = MapCol(decipher, mapCol, srcModel, toModel, srcModelElem, toModelElem);

                    count += (isMap ? 1 : 0);
                }

                
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                decipher.Close();
                decipher.Dispose();
            }

            //触发错误
            if (exception != null) { throw exception; }

        }


        /// <summary>
        /// 映射列数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="mapCol"></param>
        /// <param name="srcModel"></param>
        /// <param name="toModel"></param>
        /// <param name="srcModelElem"></param>
        /// <param name="toModelElem"></param>
        private static bool MapCol(DbDecipher decipher, IG2_MAP_COL mapCol, LightModel srcModel, LightModel toModel, 
            LModelElement srcModelElem, LModelElement toModelElem)
        {

            string lCode = mapCol.L_COL,
                rCode = mapCol.R_COL;


            string valueMode = mapCol.VALUE_MODE.ToUpper();

            if (valueMode == "TABLE")
            {
                if (StringUtil.IsBlank(mapCol.R_COL))
                {
                    return false;
                }

                try
                {
                    toModel[lCode] = srcModel[rCode];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("转换字段错误: {0} -> {1}", rCode, lCode), ex);
                }

                return true;
            }
            else if (valueMode == "FIXED")
            {
                try
                {
                    LModelFieldElement fieldElem = toModelElem.Fields[lCode];
                    object fixedValue = HWQ.Entity.ModelConvert.ChangeType(mapCol.R_FIXED, fieldElem);

                    toModel[lCode] = fixedValue;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("转换固定值错误: {0} -> {1}", mapCol.R_FIXED, lCode), ex);
                }

                return true;
            }
            else if (valueMode == "FUN")
            {
                try
                {
                    EC5.LCodeEngine.LcFieldRule lcFRule = new LCodeEngine.LcFieldRule();

                    lcFRule.Field = mapCol.L_COL;
                    lcFRule.Code = mapCol.R_FUN;
                    lcFRule.CodeParse();

                    var resultValue = lcFRule.Exec((LModel)srcModel);

                    toModel[lCode] = resultValue;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("函数执行错误：【{0}】 -> {1}", mapCol.R_FUN, lCode), ex);
                }

                return true;
            }

            return false;

        }



    }
}
