using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.BizBase;
using EC5.IG2.Core.UI;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EC5.IG2.Core
{
    /// <summary>
    /// 业务助手类。临时
    /// </summary>
    public static class BizHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// 填充更新的数据
        /// </summary>
        public static void FullForUpdate(LModel model)
        {
            if (model == null)
            {
                return;
            }

            LModelElement modelElem = model.GetModelElement();

            LModelFieldElement rowUpdate;

            if (modelElem.TryGetField("ROW_DATE_UPDATE", out rowUpdate))
            {
                if (!model.GetBlemish("ROW_DATE_UPDATE"))
                {
                    model.SetValue(rowUpdate, DateTime.Now);
                }
            }
                                 

        }

        /// <summary>
        /// 填充新建的数据
        /// </summary>
        /// <param name="model"></param>
        public static void FullForInsert(LModel model)
        {


        }



        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="checkeIds">选中的主键ID集合</param>
        /// <param name="dialogId">弹出的视图ID</param>
        /// <param name="mapId">映射的ID</param>
        /// <param name="parentField">上级字段</param>
        /// <param name="parentId">上级ID</param>
        /// <param name="autoInsert">自动插入数据</param>
        public static IList<LModel> ImportData(int dialogId, int mapId, int[] checkeIds, string parentField, int parentId, bool autoInsert)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            MapSet mapSet = MapSet.SelectSID_0(decipher, mapId);


            IG2_MAP map = mapSet.Map;

            if (map == null)
            {
                throw new Exception("映射规则已经失效! MAP_ID=" + mapId);
            }

            string rTable = map.R_TABLE;

            List<LModel> rModels = null;


            if (StringUtil.StartsWith(rTable, "UV_"))
            {
                ViewSet vSet = ViewSet.Select(decipher, dialogId);

                IG2_VIEW view = vSet.View;

                string tSqlSelect = ViewMgr.GetTSqlSelect(vSet);
                string tSqlFrom = ViewMgr.GetTSqlForm(vSet);

                string tWhere = string.Format(" {0}.{1} in ({2})",
                    view.MAIN_TABLE_NAME, view.MAIN_ID_FIELD,
                    ArrayUtil.ToString(checkeIds));

                string tSql = string.Concat("SELECT ", tSqlSelect, " FROM ", tSqlFrom, " WHERE ", tWhere);

                rModels = decipher.GetModelList(tSql);
            }
            else
            {
                LModelElement rModelElem = LModelDna.GetElementByName(rTable);

                LightModelFilter filter = new LightModelFilter(rTable);

                filter.And(rModelElem.PrimaryKey, checkeIds, Logic.In);

                rModels = decipher.GetModelList(filter);
            }

            List<LModel> lModels = new List<LModel>();

            foreach (LModel rModel in rModels)
            {

                LModel lModel = new LModel(map.L_TABLE);

                //映射数据
                App.InfoGrid2.Bll.MapMgr.MapData(mapSet, rModel, lModel);


                lModel[parentField] = parentId;
                lModels.Add(lModel);
            }

            if (!autoInsert)
            {
                return lModels;
            }
         
            try
            {
                decipher.InsertModels<LModel>(lModels);
            }
            catch (Exception ex)
            {
                log.DebugFormat("弹出窗口ID:{0}, 映射ID：{1}", dialogId, mapId);
                Debug_Models(lModels);

                throw ex;
            }


            return lModels;
        }

        /// <summary>
        /// 日志输出 Debug 调试信息
        /// </summary>
        /// <param name="models"></param>
        private static void Debug_Models(List<LModel> models)
        {

            foreach (var model in models)
            {
                Debug_Model(model);
            }
        }

        /// <summary>
        /// 日志输出 Debug 调试信息数据
        /// </summary>
        /// <param name="model"></param>
        private static void Debug_Model(LModel model)
        {
            LModelElement modelElem = model.GetModelElement();

            log.DebugFormat("数据表内容：{0}", modelElem.DBTableName);

            foreach (var fieldElem in modelElem.Fields)
            {
                object value = model[fieldElem];

                if (value == null)
                {
                    continue;
                }

                LMFieldDBTypes dbType = ModelConvert.ToDbType(value.GetType());

                if (dbType != fieldElem.DBType)
                {
                    log.DebugFormat("错误类型的字段: {0}={3}, 数据类型:{1}, 要求类型:{2}",
                        fieldElem.DBField, dbType, fieldElem.DBType, value);
                }

            }

        }



        

        private static void ChangeBizSID_GetPs(string jsonPs, out int startSID, out  int endSID)
        {
            jsonPs = "[" + jsonPs + "]";

            JToken jObj = (JToken)JsonConvert.DeserializeObject(jsonPs);

            JArray jArray = (JArray)jObj;

            JToken jToken0 = jArray[0];
            JToken jToken1 = jArray[1];

            startSID = jToken0.Value<int>();
            endSID = jToken1.Value<int>();
        }







        private static ResultBase ChangeField_GetParam(string jsonPs)
        {
            jsonPs = jsonPs.Trim();

            if (!StringUtil.StartsWith(jsonPs, "{") && !StringUtil.EndsWith(jsonPs, "}"))
            {
                jsonPs = "{" + jsonPs + "}";
            }

            JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonPs);

            string subTable = string.Empty;

            try
            {
                JProperty subTableProp = jObj.Property("sub_table");

                if (subTableProp.HasValues)
                {
                    JValue stJv = (JValue)subTableProp.Value;

                    subTable = stJv.Value.ToString();
                }

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            JProperty fieldProp = jObj.Property("field");
            JValue jv = (JValue)fieldProp.Value;

            string field = jv.Value.ToString();

            
            fieldProp = jObj.Property("from");

            //jv = (JValue)fieldProp.Value;

            JToken jtValue = fieldProp.Value;


            List<int> fromIds = new List<int>();

            if (jtValue.Type == JTokenType.Integer)
            {
                fromIds.Add(jtValue.Value<int>());
            }
            else if (jtValue.Type == JTokenType.String)
            {

            }
            else if (jtValue.Type == JTokenType.Array)
            {
                foreach (var item in jtValue.Values<int>())
                {
                    fromIds.Add(item);
                }
            }


            int toId = jObj.Property("to").Value.Value<int>();

            ResultBase result = new ResultBase();

            result["sub_table"] = subTable;
            result["field"] = field;
            result["from"] = fromIds.ToArray();
            result["to"] = toId;

            return result;
        }




        /// <summary>
        /// 改变某个字段的值
        /// </summary>
        /// <param name="psStr">json 参数.业务值变化状态.
        /// 格式为: { field:"字段1", from: 数值, to:数值} . 例: {field:'字段1', from:0, to:2}
        /// </param>
        /// <param name="mainStore">界面上主数据仓库</param>
        /// <returns></returns>
        public static ResultBase ChangeField(string paramStr, Store mainStore)
        {

            if (StringUtil.IsBlank(paramStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }

            if (StringUtil.IsBlank(mainStore.Model)) { throw new ArgumentNullException("数据的 Model 属性不能为空."); }


            ResultBase jsonResult;

            try
            {
                jsonResult = ChangeField_GetParam(paramStr);
            }
            catch
            {
                throw new Exception("解析 json 格式失败. json=" + paramStr);
            }

            string field = (string)jsonResult["field"];

            int[] fromValues = (int[])jsonResult["from"];
            int toValue = (int)jsonResult["to"];


            LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

            LModelFieldElement fieldElem;

            if (!modelElem.TryGetField(field, out fieldElem))
            {
                throw new Exception(string.Format("这个数据没有 {0} 字段,无法改变值", field));
            }
            

            LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];
            
            int id = Convert.ToInt32( mainStore.CurDataId);


            LightModelFilter filter = new LightModelFilter(mainStore.Model);
            filter.And(modelElem.PrimaryKey, id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And(field, fromValues, Logic.In);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);


            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    model[field] = toValue;

                    try
                    {
                        DbCascadeRule.Update(mainStore, model);



                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error("联动改变状态失败:" + ex.Message, ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    mainStore.SetRecordValue(model.GetPk(), field, toValue);
                }

                decipher.TransactionCommit();


                return new ResultBase(0, "改变状态成功.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }

        }


        /// <summary>
        /// 改变某个字段的值
        /// </summary>
        /// <param name="psStr">json 参数.业务值变化状态.
        /// 格式为: { field:"字段1", from: 数值, to:数值} . 例: {field:'字段1', from:0, to:2}
        /// </param>
        /// <param name="mainStore">界面上主数据仓库</param>
        /// <returns></returns>
        public static ResultBase ChangeField_SubTable(string paramStr,SortedList<string,Store> subStoreDict)
        {

            if (StringUtil.IsBlank(paramStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }

            ResultBase jsonResult;

            try
            {
                jsonResult = ChangeField_GetParam(paramStr);
            }
            catch
            {
                throw new Exception("解析 json 格式失败. json=" + paramStr);
            }

            string subTable = (string)jsonResult["sub_table"];

            string field = (string)jsonResult["field"];

            int[] fromValues = (int[])jsonResult["from"];
            int toValue = (int)jsonResult["to"];


            LModelElement modelElem = LightModel.GetLModelElement(subTable);

            LModelFieldElement fieldElem;

            if (!modelElem.TryGetField(field, out fieldElem))
            {
                throw new Exception(string.Format("这个数据没有 {0} 字段,无法改变值", field));
            }


            Store subStore = subStoreDict[subTable];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = subStore.GetList() as LModelList<LModel>;


            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    model[field] = toValue;

                    try
                    {
                        DbCascadeRule.Update(subStore, model);



                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error("联动改变状态失败:" + ex.Message, ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    subStore.SetRecordValue(model.GetPk(), field, toValue);
                }

                decipher.TransactionCommit();


                return new ResultBase(0, "改变状态成功.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }

        }

        /// <summary>
        /// 改变某个字段的值
        /// </summary>
        /// <param name="psStr">json 参数.业务值变化状态.
        /// 格式为: { field:"字段1", from: 数值, to:数值} . 例: {field:'字段1', from:0, to:2}
        /// </param>
        /// <param name="mainTable">界面上表格的主表</param>
        /// <param name="mainStore">界面上主数据仓库</param>
        /// <returns></returns>
        public static ResultBase ChangeField(string paramStr, Table mainTable, Store mainStore)
        {

            if (StringUtil.IsBlank(paramStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainTable == null) { throw new ArgumentNullException("mainTable", "界面主表表格不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }

            if (StringUtil.IsBlank(mainStore.Model)) { throw new ArgumentNullException("数据的 Model 属性不能为空."); }


            ResultBase jsonResult;

            try
            {
                jsonResult = ChangeField_GetParam(paramStr);
            }
            catch
            {
                throw new Exception("解析 json 格式失败. json=" + paramStr);
            }

            string field = (string)jsonResult["field"];

            int[] fromValues = (int[])jsonResult["from"];
            int toValue = (int)jsonResult["to"];


            LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

            LModelFieldElement fieldElem;

            if (!modelElem.TryGetField(field, out fieldElem))
            {
                throw new Exception(string.Format("这个数据没有 {0} 字段,无法改变值", field));
            }

            if (mainTable.CheckedRows.Count == 0)
            {
                return new ResultBase(1, "没有选中记录。");
            }



            LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

            int[] ids = mainTable.CheckedRows.GetIds<int>();

            LightModelFilter filter = new LightModelFilter(mainStore.Model);
            filter.And(modelElem.PrimaryKey, ids, Logic.In);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And(field, fromValues, Logic.In);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);


            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    model[field] = toValue;

                    try
                    {
                        DbCascadeRule.Update(mainStore, model);

                        

                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error("联动改变状态失败:" + ex.Message, ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    mainStore.SetRecordValue(model.GetPk(), field, toValue);
                }

                decipher.TransactionCommit();


                return new ResultBase(0, "改变状态成功.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }

        }

        /// <summary>
        /// 改变全部业务状态
        /// </summary>
        /// <param name="psStr"></param>
        /// <param name="mainStore">数据仓库</param>
        /// <returns></returns>
        public static ResultBase ChangeBizSIDAll(string psStr, Store mainStore)
        {
            log.Debug("改变所有记录的业务状态.");

            if (StringUtil.IsBlank(psStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }

            if (StringUtil.IsBlank(mainStore.Model)) { throw new Exception($"数据仓库必须指定一个实体名称."); }



            int startSID, endSID;

            try
            {
                ChangeBizSID_GetPs(psStr, out startSID, out endSID);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化参数错误:参数=" + psStr, ex);
            }

            LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

            if (!modelElem.Fields.ContainsField("BIZ_SID"))
            {
                throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
            }
            

            LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

            //int[] ids = mainTable.CheckedRows.GetIds<int>();

            LightModelFilter filter = new LightModelFilter(mainStore.Model);
            filter.And("BIZ_SID", startSID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            //filter.And(modelElem.PrimaryKey, ids, Logic.In);




            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            log.Debug($"需要处理 {models.Count} 条记录.");

            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            int j = 0;

            try
            {
                foreach (LModel model in models)
                {
                    j++;

                    log.Debug($"执行联动: 改 BIZ_SID {startSID}->{endSID}, {model.GetModelName()} ,PK={model.GetPk()}");

                    model.SetTakeChange(true);

                    model["BIZ_SID"] = endSID;
           
                    try
                    {
                        DbCascadeRule.Update(mainStore, model);

                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error($"联动改变状态失败: {ex.Message}, 第{j}条记录.", ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    //mainStore.SetRecordValue(model.GetPk(), "BIZ_SID", endSID);
                }

                decipher.TransactionCommit();


                return new ResultBase(0, $"改变状态成功, 共 {models.Count} 条记录.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败, 已回滚事务.", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }
        }




        /// <summary>
        /// 把时间字段设置为当前时间
        /// </summary>
        /// <param name="psStr"></param>
        /// <param name="mainStore">数据仓库</param>
        /// <param name="subStore">子表数据仓库</param>
        /// <returns></returns>
        public static ResultBase ChangeFieldNow(string psStr, Store mainStore, SortedList<string,Store> subStoreDict)
        {
            log.Debug("改变字段为当前时间.");

            if (StringUtil.IsBlank(psStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }

            if (StringUtil.IsBlank(mainStore.Model)) { throw new Exception($"数据仓库必须指定一个实体名称."); }

            // { main_field:'', sub_field:'' }

            string mainField = string.Empty;    //主表字段名

            string subTable = string.Empty; //子表的表名
            string subField = string.Empty; //子表的字段名

            try
            {
                if (!StringUtil.StartsWith(psStr, "{") && !StringUtil.EndsWith(psStr, "}"))
                {
                    psStr = "{" + psStr + "}";
                }

                SModel sm = SModel.ParseJson(psStr);

                mainField = sm["main_field"];

                subTable = sm["sub_table"];
                subField = sm["sub_field"];
            }
            catch (Exception ex)
            {
                throw new Exception("序列化参数错误:参数=" + psStr, ex);
            }

            
            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!StringUtil.IsBlank(mainField))
            {
                LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

                if (!modelElem.HasField(mainField))
                {
                    throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
                }


                LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

                int id = Convert.ToInt32(mainStore.CurDataId);




                LightModelFilter filter = new LightModelFilter(mainStore.Model);

                filter.And(modelElem.PrimaryKey, id);
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


                LModelList<LModel> models = decipher.GetModelList(filter);

                log.Debug($"需要处理 {models.Count} 条记录.");

                decipher.BeginTransaction(IsolationLevel.RepeatableRead);

                int j = 0;

                try
                {
                    DateTime now = DateTime.Now;

                    foreach (LModel model in models)
                    {
                        j++;

                        log.Debug($"执行联动: 改 为当前时间值 {model.GetModelName()} ,PK={model.GetPk()}");

                        model.SetTakeChange(true);

                        model[mainField] = now;

                        try
                        {
                            DbCascadeRule.Update(mainStore, model);

                        }
                        catch (Exception ex)
                        {
                            decipher.TransactionRollback();

                            log.Error($"联动改变状态失败: {ex.Message}, 第{j}条记录.", ex);

                            return new ResultBase(1, "状态改变失败：" + ex.Message);
                        }
                    }

                    decipher.TransactionCommit();


                    return new ResultBase(0, $"改变状态成功, 共 {models.Count} 条记录.");
                }
                catch (Exception ex)
                {
                    decipher.TransactionRollback();

                    log.Error("状态改变失败, 已回滚事务.", ex);

                    return new ResultBase(1, "状态改变失败：" + ex.Message);

                }
            }

            if (subStoreDict != null && subStoreDict.Count > 0 && !StringUtil.IsBlank(subTable) && !StringUtil.IsBlank(subField))
            {
                Store subStore = subStoreDict[subTable];

                LModelElement modelElem = LightModel.GetLModelElement(subStore.Model);

                if (!modelElem.HasField(subField))
                {
                    throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
                }


                LModelList<LModel> models = subStore.GetList() as LModelList<LModel>;

                log.Debug($"需要处理 {models.Count} 条记录.");

                decipher.BeginTransaction(IsolationLevel.RepeatableRead);

                int j = 0;

                try
                {
                    DateTime now = DateTime.Now;

                    foreach (LModel model in models)
                    {
                        j++;

                        log.Debug($"执行联动: 改 子表为当前时间值 {model.GetModelName()} ,PK={model.GetPk()}");

                        model.SetTakeChange(true);

                        model[subField] = now;

                        try
                        {
                            DbCascadeRule.Update(mainStore, model);

                        }
                        catch (Exception ex)
                        {
                            decipher.TransactionRollback();

                            log.Error($"联动改变状态失败: {ex.Message}, 第{j}条记录.", ex);

                            return new ResultBase(1, "状态改变失败：" + ex.Message);
                        }
                    }

                    decipher.TransactionCommit();


                    return new ResultBase(0, $"改变状态成功, 共 {models.Count} 条记录.");
                }
                catch (Exception ex)
                {
                    decipher.TransactionRollback();

                    log.Error("状态改变失败, 已回滚事务.", ex);

                    return new ResultBase(1, "状态改变失败：" + ex.Message);

                }
            }


            return new ResultBase("没有处理任何数据");

        }



        /// <summary>
        /// 把时间字段设置为当前时间
        /// </summary>
        /// <param name="psStr"></param>
        /// <param name="table"></param>
        /// <param name="mainStore">数据仓库</param>
        /// <param name="subStoreDict">子表数据仓库</param>
        /// <returns></returns>
        public static ResultBase ChangeFieldNow(string psStr, Table table, Store mainStore, SortedList<string, Store> subStoreDict)
        {
            log.Debug("改变字段为当前时间.");

            if (StringUtil.IsBlank(psStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }

            if (StringUtil.IsBlank(mainStore.Model)) { throw new Exception($"数据仓库必须指定一个实体名称."); }

            // { main_field:'', sub_field:'' }

            string mainField = string.Empty;    //主表字段名

            string subTable = string.Empty; //子表的表名
            string subField = string.Empty; //子表的字段名

            try
            {
                if (!StringUtil.StartsWith(psStr, "{") && !StringUtil.EndsWith(psStr, "}"))
                {
                    psStr = "{" + psStr + "}";
                }

                SModel sm = SModel.ParseJson(psStr);

                mainField = sm["main_field"];

                subTable = sm["sub_table"];
                subField = sm["sub_field"];
            }
            catch (Exception ex)
            {
                throw new Exception("序列化参数错误:参数=" + psStr, ex);
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!StringUtil.IsBlank(mainField))
            {
                LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

                if (!modelElem.HasField(mainField))
                {
                    throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
                }


                LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

                int[] ids = table.CheckedRows.GetIds<int>();

                LightModelFilter filter = new LightModelFilter(mainStore.Model);

                filter.And(modelElem.PrimaryKey, ids, Logic.In);
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


                LModelList<LModel> models = decipher.GetModelList(filter);

                log.Debug($"需要处理 {models.Count} 条记录.");

                decipher.BeginTransaction(IsolationLevel.RepeatableRead);

                int j = 0;

                try
                {
                    DateTime now = DateTime.Now;

                    foreach (LModel model in models)
                    {
                        j++;

                        log.Debug($"执行联动: 改 为当前时间值 {model.GetModelName()} ,PK={model.GetPk()}");

                        model.SetTakeChange(true);

                        model[mainField] = now;

                        try
                        {
                            DbCascadeRule.Update(mainStore, model);

                        }
                        catch (Exception ex)
                        {
                            decipher.TransactionRollback();

                            log.Error($"联动改变状态失败: {ex.Message}, 第{j}条记录.", ex);

                            return new ResultBase(1, "状态改变失败：" + ex.Message);
                        }
                    }

                    decipher.TransactionCommit();


                    return new ResultBase(0, $"改变状态成功, 共 {models.Count} 条记录.");
                }
                catch (Exception ex)
                {
                    decipher.TransactionRollback();

                    log.Error("状态改变失败, 已回滚事务.", ex);

                    return new ResultBase(1, "状态改变失败：" + ex.Message);

                }
            }

            if (subStoreDict != null && subStoreDict.Count > 0 && !StringUtil.IsBlank(subTable) && !StringUtil.IsBlank(subField))
            {
                Store subStore = subStoreDict[subTable];

                LModelElement modelElem = LightModel.GetLModelElement(subStore.Model);

                if (!modelElem.HasField(subField))
                {
                    throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
                }


                LModelList<LModel> models = subStore.GetList() as LModelList<LModel>;

                log.Debug($"需要处理 {models.Count} 条记录.");

                decipher.BeginTransaction(IsolationLevel.RepeatableRead);

                int j = 0;

                try
                {
                    DateTime now = DateTime.Now;

                    foreach (LModel model in models)
                    {
                        j++;

                        log.Debug($"执行联动: 改 子表为当前时间值 {model.GetModelName()} ,PK={model.GetPk()}");

                        model.SetTakeChange(true);

                        model[subField] = now;

                        try
                        {
                            DbCascadeRule.Update(mainStore, model);

                        }
                        catch (Exception ex)
                        {
                            decipher.TransactionRollback();

                            log.Error($"联动改变状态失败: {ex.Message}, 第{j}条记录.", ex);

                            return new ResultBase(1, "状态改变失败：" + ex.Message);
                        }
                    }

                    decipher.TransactionCommit();


                    return new ResultBase(0, $"改变状态成功, 共 {models.Count} 条记录.");
                }
                catch (Exception ex)
                {
                    decipher.TransactionRollback();

                    log.Error("状态改变失败, 已回滚事务.", ex);

                    return new ResultBase(1, "状态改变失败：" + ex.Message);

                }
            }


            return new ResultBase("没有处理任何数据");

        }



        public static void ValidateBlack_TableCols(StringBuilder sb, LModel model, int curId, Store mainStore, TableSet ts)
        {
           
        }


        public static void ValidateBlack_TableCols(StringBuilder sb, LModel model, Table mainTable, Store mainStore, TableSet ts)
        {
            foreach (BoundField bField in mainTable.Columns)
            {
                if (!bField.Required)
                {
                    continue;
                }


                if (bField is NumColumn)
                {
                    object mValue = model[bField.DataField];

                    if (mValue == null || 0.Equals(mValue))
                    {
                        sb.AppendLine($"{bField.HeaderText} 是必填项!");
                    }
                }
                else if (bField is DateColumn)
                {
                    object mValue = model[bField.DataField];

                    if (mValue == null)
                    {
                        sb.AppendLine($"{bField.HeaderText} 是必填项!");
                    }
                }
                else
                {

                    object mValue = model[bField.DataField];

                    if (mValue == null || string.IsNullOrEmpty(mValue.ToString()))
                    {
                        sb.AppendLine($"{bField.HeaderText} 是必填项!");
                    }
                }
            }

        }




        /// <summary>
        /// 验证空值
        /// </summary>
        /// <param name="models"></param>
        /// <param name="mainTable"></param>
        /// <param name="mainStore"></param>
        public static void ValidateBlack(LModelList<LModel> models, Table mainTable, Store mainStore, TableSet ts)
        {

            if (mainTable.ReadOnly)
            {
                return;
            }


            foreach (LModel model in models)
            {

                StringBuilder sb = new StringBuilder();

                ValidateBlack_TableCols(sb, model, mainTable, mainStore,ts);
                               

                if (sb.Length > 0)
                {
                    throw new Exception(JsonUtil.ToJson(sb.ToString()));
                }

            }

        }

        /// <summary>
        /// 验证空值
        /// </summary>
        /// <param name="models"></param>
        /// <param name="mainTable"></param>
        /// <param name="mainStore"></param>
        public static void ValidateBlack(LModelList<LModel> models, int curId, Store mainStore, TableSet ts)
        {

            foreach (LModel model in models)
            {

                StringBuilder sb = new StringBuilder();

                ValidateBlack_TableCols(sb, model, curId, mainStore,ts);
                               

                if (sb.Length > 0)
                {
                    throw new Exception(JsonUtil.ToJson(sb.ToString()));
                }

            }

        }


        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        /// <param name="psStr">json 参数.业务值变化状态.
        /// 格式为:数值,数值. 例:0,2
        /// </param>
        /// <param name="mainTable">界面上表格的主表</param>
        /// <param name="mainStore">界面上主数据仓库</param>
        public static ResultBase ChangeBizSID(string psStr, int curId,  Store mainStore)
        {
            if (StringUtil.IsBlank(psStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }


            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }


            int startSID, endSID;

            try
            {
                ChangeBizSID_GetPs(psStr, out startSID, out endSID);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化参数错误:参数=" + psStr, ex);
            }



            LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

            if (!modelElem.Fields.ContainsField("BIZ_SID"))
            {
                throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
            }
            

            LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

            //int[] ids = new int[] { curId };

            LightModelFilter filter = new LightModelFilter(mainStore.Model);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("BIZ_SID", startSID);
            filter.And(modelElem.PrimaryKey, curId);




            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            if (models == null || models.Count == 0)
            {
                return new ResultBase(1, "没有需要处理的记录。");
            }

            if (startSID < endSID && endSID > 0)
            {
                ValidateBlack(models, curId, mainStore, null);


                M2ValidateFactory validFty = new M2ValidateFactory();

                foreach (LModel model in models)
                {
                    bool isValid = validFty.ValidModelFieldAll(mainStore, model, null);

                    if (!isValid)
                    {
                        throw new Exception("请检查填写正确后，再提交.");
                    }
                }


            }


            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    model["BIZ_SID"] = endSID;

                    if (endSID > startSID)
                    {
                        if (endSID == 2)
                        {
                            SetHasField(model, "BIZ_CREATE_ROLE_CODE", BizServer.FirstRoleName);
                            SetHasField(model, "BIZ_CREATE_COMP_CODE", BizServer.OpCompCode);
                            SetHasField(model, "BIZ_CREATE_USER_CODE", BizServer.UserCode);
                            SetHasField(model, "BIZ_CREATE_ORG_CODE", BizServer.OrgCode);

                            SetHasField(model, "BIZ_CREATE_ROLE_TEXT", BizServer.FirstRoleName);
                            //SetHasField(model, "BIZ_CREATE_COMP_TEXT", string.Empty);
                            SetHasField(model, "BIZ_CREATE_USER_TEXT", BizServer.LoginName);
                            //SetHasField(model, "BIZ_CREATE_ORG_TEXT", string.Empty);


                            SetHasField(model, "BIZ_CREATE_DATE", DateTime.Now);


                        }
                        else if (endSID == 4)
                        {
                            SetHasField(model, "BIZ_CHECK_ROLE_CODE", BizServer.FirstRoleName);
                            SetHasField(model, "BIZ_CHECK_COMP_CODE", BizServer.OpCompCode);
                            SetHasField(model, "BIZ_CHECK_USER_CODE", BizServer.UserCode);
                            SetHasField(model, "BIZ_CHECK_ORG_CODE", BizServer.OrgCode);

                            SetHasField(model, "BIZ_CHECK_ROLE_TEXT", BizServer.FirstRoleName);
                            //SetHasField(model, "BIZ_CREATE_COMP_TEXT", string.Empty);
                            SetHasField(model, "BIZ_CHECK_USER_TEXT", BizServer.LoginName);
                            //SetHasField(model, "BIZ_CREATE_ORG_TEXT", string.Empty);


                            SetHasField(model, "BIZ_CHECK_DATE", DateTime.Now);
                        }
                    }
                    else if(endSID < startSID)
                    {
                        if(endSID == 0)
                        {
                            SetHasField(model, "BIZ_FLOW_SID", 0);
                            SetHasField(model, "BIZ_FLOW_INST_CODE", string.Empty);
                            SetHasField(model, "BIZ_FLOW_DEF_CODE", string.Empty);
                            SetHasField(model, "BIZ_FLOW_CUR_NODE_CODE", string.Empty);
                            SetHasField(model, "BIZ_FLOW_CUR_NODE_TEXT", string.Empty);
                            SetHasField(model, "BIZ_FLOW_STEP_CODE", string.Empty);
                        }
                    }


                    try
                    {
                        DbCascadeRule.Update(mainStore, model);

                        
                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error("联动改变状态失败:" + ex.Message, ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    mainStore.SetRecordValue(model.GetPk(), "BIZ_SID", endSID);
                }

                decipher.TransactionCommit();


                return new ResultBase(0, "改变状态成功.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }

        }


        private static void SetHasField(LModel model, string field, object value)
        {
            if (model.HasField(field))
            {
                model[field] = value;
            }
        }

        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        /// <param name="psStr">json 参数.业务值变化状态.
        /// 格式为:数值,数值. 例:0,2
        /// </param>
        /// <param name="mainTable">界面上表格的主表</param>
        /// <param name="mainStore">界面上主数据仓库</param>
        public static ResultBase ChangeBizSID(string psStr,Table mainTable, Store mainStore)
        {
            if (StringUtil.IsBlank(psStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainTable == null) { throw new ArgumentNullException("mainTable", "界面主表表格不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }


            int startSID, endSID;

            try
            {
                ChangeBizSID_GetPs(psStr, out startSID, out endSID);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化参数错误:参数=" + psStr, ex);
            }



            LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

            if (!modelElem.Fields.ContainsField("BIZ_SID"))
            {
                throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
            }

            if (mainTable.CheckedRows.Count == 0)
            {
                return new ResultBase(1, "没有选中记录。");
            }

            LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

            int[] ids = mainTable.CheckedRows.GetIds<int>();

            LightModelFilter filter = new LightModelFilter(mainStore.Model);
            filter.And("BIZ_SID", startSID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And(modelElem.PrimaryKey, ids, Logic.In);


            

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            if(models == null || models.Count == 0)
            {
                return new ResultBase(1, "没有需要处理的记录。");
            }
            
            if ( startSID < endSID && endSID > 0)
            {
                ValidateBlack(models, mainTable, mainStore,null);


                M2ValidateFactory validFty = new M2ValidateFactory();

                foreach (LModel model in models)
                {
                    bool isValid = validFty.ValidModelFieldAll(mainStore, model, null);

                    if (!isValid)
                    {
                        throw new Exception("请检查填写正确后，再提交.");
                    }
                }


            }


            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    model["BIZ_SID"] = endSID;

                    

                    try
                    {
                        DbCascadeRule.Update(mainStore, model);
                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error("联动改变状态失败:" + ex.Message, ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    mainStore.SetRecordValue(model.GetPk(), "BIZ_SID", endSID);
                }

                decipher.TransactionCommit();


                return new ResultBase(0,"改变状态成功.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }

        }



        public static ResultBase ChangeBizSID(string psStr, Table mainTable, Store mainStore, TableSet tableSet)
        {
            SortedList<string, TableSet> tsList = new SortedList<string, TableSet>();

            tsList.Add(tableSet.Table.TABLE_NAME, tableSet);

            return ChangeBizSID(psStr, mainTable, mainStore, tsList);
        }



        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        /// <param name="psStr">json 参数.业务值变化状态.
        /// 格式为:数值,数值. 例:0,2
        /// </param>
        /// <param name="mainTable">界面上表格的主表</param>
        /// <param name="mainStore">界面上主数据仓库</param>
        /// <param name="tableSets">表名索引的 TableSet 对象</param>
        public static ResultBase ChangeBizSID(string psStr, Table mainTable, Store mainStore ,SortedList<string,TableSet> tableSets)
        {
            if (StringUtil.IsBlank(psStr)) { throw new ArgumentNullException("psStr", "json 格式的参数不能为空."); }
            if (mainTable == null) { throw new ArgumentNullException("mainTable", "界面主表表格不能为空."); }
            if (mainStore == null) { throw new ArgumentNullException("mainStore", "主数据仓库不能为空."); }


            int startSID, endSID;

            try
            {
                ChangeBizSID_GetPs(psStr, out startSID, out endSID);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化参数错误:参数=" + psStr, ex);
            }



            LModelElement modelElem = LightModel.GetLModelElement(mainStore.Model);

            if (!modelElem.HasField("BIZ_SID"))
            {
                throw new Exception("这个数据没有 BIZ_SID 字段,无法改变状态");
            }

            if (mainTable.CheckedRows.Count == 0)
            {
                return new ResultBase(1, "没有选中记录。");
            }

            LModelFieldElement pkFieldElem = modelElem.Fields[modelElem.PrimaryKey];

            int[] ids = mainTable.CheckedRows.GetIds<int>();

            LightModelFilter filter = new LightModelFilter(mainStore.Model);
            filter.And("BIZ_SID", startSID);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And(modelElem.PrimaryKey, ids, Logic.In);




            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            if (models == null || models.Count == 0)
            {
                return new ResultBase(1, "没有需要处理的记录。");
            }

            if (startSID < endSID && endSID > 0)
            {
                TableSet ts = null;

                tableSets.TryGetValue(mainStore.Model, out ts);
                 
                ValidateBlack(models, mainTable, mainStore,ts);



                M2ValidateFactory validFty = new M2ValidateFactory();

                foreach (LModel model in models)
                {
                    bool isValid = validFty.ValidModelFieldAll(mainStore, model, ts);

                    if (!isValid)
                    {
                        throw new Exception("请检查填写正确后，再提交.");
                    }
                }

            }


            decipher.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                foreach (LModel model in models)
                {
                    model.SetTakeChange(true);

                    model["BIZ_SID"] = endSID;



                    try
                    {
                        DbCascadeRule.Update(mainStore, model);
                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        log.Error("联动改变状态失败:" + ex.Message, ex);

                        return new ResultBase(1, "状态改变失败：" + ex.Message);
                    }


                    mainStore.SetRecordValue(model.GetPk(), "BIZ_SID", endSID);
                }

                decipher.TransactionCommit();


                return new ResultBase(0, "改变状态成功.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("状态改变失败", ex);

                return new ResultBase(1, "状态改变失败：" + ex.Message);

            }

        }


        /// <summary>
        /// 填充下拉框
        /// </summary>
        /// <param name="comboBox">下拉框控件</param>
        /// <param name="tableName">表名</param>
        /// <param name="valueField">值的字段名</param>
        /// <param name="displayField">显示的字段名</param>
        public static void Full(ComboBox comboBox, string tableName, string valueField, string displayField)
        {
            if (comboBox == null) throw new ArgumentNullException("comboBox");
            if (StringUtil.IsBlank(tableName)) throw new ArgumentNullException("tableName");
            if (StringUtil.IsBlank(valueField)) throw new ArgumentNullException("valueField");


            List <string> fields = new List<string>();

            fields.Add(valueField);

            if (!StringUtil.IsBlank( displayField) &&  !fields.Contains(displayField))
            {
                fields.Add(displayField);
            }

            if (StringUtil.IsBlank(displayField))
            {
                displayField = valueField;
            }



            LModelElement modelElem = LModelDna.GetElementByName(tableName);



            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(tableName);

            if (modelElem.HasField("ROW_SID"))
            {
                lmFilter.AddFilter("ROW_SID >= 0");
            }

            lmFilter.Fields = fields.ToArray();

            List<LModel> models = decipher.GetModelList(lmFilter);
            

            foreach (LModel model in models)
            {
                string value = model.Get<string>(valueField);
                string text = model.Get<string>(displayField);

                comboBox.Items.Add(value, text);

            }
        }



    }
}
