using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 映射规则管理
    /// </summary>
    public static class MapMgr
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 缓冲
        /// </summary>
        static SortedList<int, MapSet> m_BufferItems = new SortedList<int, MapSet>();

        /// <summary>
        /// 清理所有缓冲
        /// </summary>
        public static void ClearBufferAll()
        {
            m_BufferItems.Clear();
        }

        /// <summary>
        /// 清理缓冲
        /// </summary>
        /// <param name="mapId"></param>
        public static void ClearBuffer(int mapId)
        {
            m_BufferItems.Remove(mapId);
        }

        /// <summary>
        /// 映射数据。从 SrcModel 拷贝到 TarModel 表。
        /// </summary>
        /// <param name="map">映射规则</param>
        /// <param name="srcModel">数据源表</param>
        /// <param name="tarModel">目标表</param>
        public static void MapData(int mapId, LModel srcModel, LModel toModel)
        {
            MapSet mapSet = null;


            if (!m_BufferItems.TryGetValue(mapId, out mapSet))
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                mapSet = MapSet.SelectSID_0(decipher, mapId);

                if (mapSet.Map != null)
                {
                    m_BufferItems.Add(mapId, mapSet);
                }
            }

            MapData(mapSet.Map, mapSet.Cols, srcModel, toModel);
        }


        /// <summary>
        /// 映射数据。从 SrcModel 拷贝到 TarModel 表。
        /// </summary>
        /// <param name="map">映射规则</param>
        /// <param name="srcModel">数据源表</param>
        /// <param name="tarModel">目标表</param>
        public static void MapData(MapSet mapSet, LModel srcModel, LModel toModel)
        {
            MapData(mapSet.Map, mapSet.Cols, srcModel, toModel);
        }


        /// <summary>
        /// 映射数据。从 SrcModel 拷贝到 TarModel 表。
        /// </summary>
        /// <param name="map">映射规则</param>
        /// <param name="srcModel">数据源表</param>
        /// <param name="tarModel">目标表</param>
        public static void MapData(IG2_MAP map,List<IG2_MAP_COL> mapCols, LModel srcModel, LModel toModel)
        {

            LModelElement srcModelElem = srcModel.GetModelElement();
            LModelElement toModelElem = toModel.GetModelElement();


            foreach (IG2_MAP_COL mapCol in mapCols)
            {
                bool success = MapCol(map,mapCol, srcModel, toModel, srcModelElem, toModelElem);
            }

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
        private static bool MapCol(IG2_MAP map, IG2_MAP_COL mapCol, LightModel srcModel, LightModel toModel,
            LModelElement srcModelElem, LModelElement toModelElem)
        {
            if (srcModelElem == null)
            {
                throw new Exception("srcModelElem 实体元素不能为空。");
            }

            if (toModelElem == null)
            {
                throw new Exception("toModelElem 实体元素不能为空。");
            }


            string lCode = mapCol.L_COL,
                rCode = mapCol.R_COL;


            string valueMode = mapCol.VALUE_MODE.ToUpper();


            LModelFieldElement srcFieldElem;
            LModelFieldElement toFieldElem;

            if (valueMode == "TABLE")
            {
                if (StringUtil.IsBlank(mapCol.R_COL))
                {
                    return false;
                }

                
                if (!srcModelElem.TryGetField(rCode, out srcFieldElem))
                {
                    //如果找不到字段名，测试用关联表的视图字段名格式查询
                    string vField = string.Concat(map.R_TABLE, "_", rCode);

                    if (!srcModelElem.TryGetField(vField, out srcFieldElem))
                    {
                        throw new Exception(string.Format("'数据源'实体不存在此字段:“{0}.{1}”。", srcModelElem.DBTableName, rCode));
                    }
                }

                if (!toModelElem.TryGetField(lCode, out toFieldElem))
                {
                    throw new Exception(string.Format("'目标'实体不存在此字段:“{0}.{1}”。", toModelElem.DBTableName, lCode)); 
                }

                try
                {
                    toModel[lCode] = ModelConvert.ChangeType( srcModel[rCode], toFieldElem);
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
                    object fixedValue = ModelConvert.ChangeType(mapCol.R_FIXED, fieldElem);

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
                    LModelFieldElement fieldElem = toModelElem.Fields[lCode];

                    EC5.LCodeEngine.LcFieldRule lcFRule = new EC5.LCodeEngine.LcFieldRule();

                    lcFRule.Field = mapCol.L_COL;
                    lcFRule.Code = mapCol.R_FUN;
                    lcFRule.CodeParse();

                    var resultValue = lcFRule.Exec((LModel)srcModel);

                    lcFRule.Dispose();

                    toModel[lCode] = ModelConvert.ChangeType( resultValue,fieldElem) ;
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
