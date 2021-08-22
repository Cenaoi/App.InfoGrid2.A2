using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EC5.Utility;
using HWQ.Entity;
using System.Web;
using HWQ.Entity.Filter;
using System.Data;
using System.Transactions;

namespace EasyClick.Web.Mini2.Data
{


    /// <summary>
    /// 实体仓库构造工厂
    /// </summary>
    public partial class EntityStoreEngine : StoreEngine
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// load 初始化
        /// </summary>
        public override void OnLoad()
        {

            Store uiStore = this.Store;

            string fieldStr = this.Store.StringFields;

            if (!StringUtil.IsBlank(fieldStr))
            {
                if (fieldStr != null)
                {
                    //注: 下面这行替换没有错...不用想着改进. [2016-9-3 1:33]
                    fieldStr = fieldStr
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\t", string.Empty)
                        .Trim();
                }
            }

            if (!StringUtil.IsBlank(fieldStr))
            {
                return;
            }

            if (StringUtil.IsBlank(uiStore.Model))
            {
                return;
            }

            LModelElement modelElem = LightModel.GetLModelElement(uiStore.Model);

            if (modelElem == null)
            {
                throw new Exception(string.Format("数据仓库 Store.Model 对应的实体“{0}”不存在, StoreID={1}。", uiStore.Model, uiStore.ID));
            }

            LModelFieldElementCollection fs = modelElem.Fields;
            LModelFieldElement fieldElem;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fs.Count; i++)
            {
                if (i > 0) { sb.Append(","); }

                fieldElem = fs[i];

                sb.Append(fieldElem.DBField);
            }

            uiStore.StringFields = sb.ToString();

        }

        /// <summary>
        /// 获取数据 DataTable
        /// </summary>
        /// <returns></returns>
        public override DataTable GetDataTable()
        {
            Store store = this.Store;

            DataTable models = null;

            if (store.TSqlQuery != null && store.TSqlQuery.Enabeld)
            {
                models = LoadPage_ForTSQL_Table(-1);
            }
            else
            {
                if (string.IsNullOrEmpty(store.Model))
                {
                    throw new Exception(string.Format("数据仓库 {0}.Model 属性'实体名'不能为空.", store.ID));
                }

                models = LoadPage_ForCommonTable(-1);
            }

            return models;

        }

        /// <summary>
        /// 获取数据实体
        /// </summary>
        /// <returns></returns>
        public override IList GetList()
        {
            Store store = this.Store;

            IList models = null;

            if (store.TSqlQuery != null && store.TSqlQuery.Enabeld)
            {
                models = LoadPage_ForTSQL(-1);
            }
            else
            {
                if (string.IsNullOrEmpty(store.Model))
                {
                    throw new Exception(string.Format("数据仓库 {0}.Model 属性'实体名'不能为空.", store.ID));
                }

                models = LoadPage_ForCommon(-1);
            }

            return models;

        }

        /// <summary>
        /// 加载分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public override IList LoadPage(int page)
        {

            Store store = this.Store;

            IList models = null;

            if (store.TSqlQuery != null && store.TSqlQuery.Enabeld)
            {
                models = LoadPage_ForTSQL(page);
            }
            else
            {
                if (string.IsNullOrEmpty(store.Model))
                {
                    throw new Exception(string.Format("数据仓库 {0}.Model 属性'实体名'不能为空.", store.ID));
                }

                models = LoadPage_ForCommon(page);
            }

            return models;
        }


        private DataTable LoadPage_ForTSQL_Table(int page)
        {
            Store storeUi = this.Store;

            string tSql = SqlEngineHelper.GetTSQL(storeUi, page, (page >= 0));

            string countTSql = SqlEngineHelper.GetCountTSql(storeUi);

            DbDecipher decipher = this.OpenDecipher();

            decipher.Locks.Add(LockType.NoLock);

            DataTable models;
            int rowTotal;

            try
            {
                models = decipher.GetDataTable(tSql);
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据错误。 SQL语句是:\r\n" + tSql, ex);
            }

            try
            {
                rowTotal = decipher.ExecuteScalar<int>(countTSql);

            }
            catch (Exception ex)
            {
                throw new Exception("获取数量错误。SQL 语句是:\r\n" + countTSql, ex);
            }


            return models;
        }




        private IList LoadPage_ForTSQL(int page)
        {
            Store storeUi = this.Store;

            string tSql = SqlEngineHelper.GetTSQL(storeUi, page, (page >= 0));

            string countTSql = SqlEngineHelper.GetCountTSql(storeUi);

            DbDecipher decipher = this.OpenDecipher();

            decipher.Locks.Add(LockType.NoLock);

            LModelList<LModel> models;
            int rowTotal;

            try
            {
                models = decipher.GetModelList(tSql);
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据错误。 SQL语句是:\r\n" + tSql, ex);
            }

            try
            {
                rowTotal = decipher.ExecuteScalar<int>(countTSql);

            }
            catch (Exception ex)
            {
                throw new Exception("获取数量错误。SQL 语句是:\r\n" + countTSql, ex);
            }


            this.ItemTotal = rowTotal;

            LoadSummary();

            return models;
        }



        private DataTable LoadPage_ForCommonTable(int page)
        {
            HttpContext context = HttpContext.Current;

            Store store = this.Store;

            LModelElement modelElem = LightModel.GetLModelElement(store.Model);

            if (modelElem == null)
            {
                throw new Exception(string.Format("数据仓库 Store.Model 对应的实体“{0}”不存在, StoreID={1}。", store.Model, store.ID));
            }


            if (modelElem.Mode == DBTableMode.Virtual)
            {
                if (!store.VirtualModelEnabled)
                {
                    return null;
                }
            }

            DataRequest dr = store.GetAction();

            dr.TSqlSort = SqlEngineHelper.GetSqlSort(store, dr);

            LightModelFilter filter = new LightModelFilter(store.Model);

            if (page >= 0)
            {
                filter.Limit = Limit.ByPageIndex(store.PageSize, page);
            }

            filter.TSqlOrderBy = dr.TSqlSort;
            filter.Locks.Add(LockType.NoLock);

            AddFilter(filter, store.FilterParams);
            AddFilter(filter, store.SelectQuery);

            DataTable models = null;


            DbDecipher decipher = this.OpenDecipher();


            try
            {
                models = decipher.GetDataTable(filter);

                //this.ItemTotal = models.PagesInfo.RowTotal;

                //LoadSummary();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Store:{0},加载数据错误。", store.ID), ex);
            }

            return models;
        }


        private IList LoadPage_ForCommon(int page)
        {
            HttpContext context = HttpContext.Current;

            Store store = this.Store;

            LModelElement modelElem = LightModel.GetLModelElement(store.Model);

            if (modelElem == null)
            {
                throw new Exception(string.Format("数据仓库 Store.Model 对应的实体“{0}”不存在, StoreID={1}。", store.Model, store.ID));
            }


            if (modelElem.Mode == DBTableMode.Virtual)
            {
                if (!store.VirtualModelEnabled)
                {
                    return null;
                }
            }

            DataRequest dr = store.GetAction();

            dr.TSqlSort = SqlEngineHelper.GetSqlSort(store, dr);

            LightModelFilter filter = new LightModelFilter(store.Model);

            if (page >= 0)
            {
                filter.Limit = Limit.ByPageIndex(store.PageSize, page);
            }

            filter.TSqlOrderBy = dr.TSqlSort;
            filter.Locks.Add(LockType.NoLock);

            AddFilter(filter, store.FilterParams);
            AddFilter(filter, store.SelectQuery);

            LModelList<LModel> models = null;


            DbDecipher decipher = this.OpenDecipher();


            try
            {
                models = decipher.GetModelsByPage(filter);

                this.ItemTotal = models.PagesInfo.RowTotal;

                LoadSummary();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Store:{0},加载数据错误。", store.ID), ex);
            }

            return models;
        }



        /// <summary>
        /// 加载汇总信息
        /// </summary>
        public override void LoadSummary()
        {
            Store store = this.Store;

            if (!store.HasSummaryField())
            {
                return;
            }

            bool tSqlQueryEnabled = (store.TSqlQuery != null && store.TSqlQuery.Enabeld);

            foreach (SummaryField sField in store.SummaryFields)
            {
                string field = sField.DataField;

                string name = StringUtil.NoBlank(sField.Name, field);

                try
                {
                    if (tSqlQueryEnabled)
                    {
                        decimal value = GetSummary(sField.SrcViewField, sField.SummaryType, sField.Filter);
                        store.SetSummary(name, value);
                    }
                    else
                    {
                        decimal value = GetSummary(field, sField.SummaryType, sField.Filter);
                        store.SetSummary(name, value);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Store:{0},汇总错误.在字段“{1}”", store.ID, field), ex);
                }
            }
        }


        /// <summary>
        /// 获取某个字段的汇总信息（单表）
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="summartyType">类型</param>
        /// <param name="summaryFilter"></param>
        /// <returns></returns>
        public override decimal GetSummary(string field, SummaryType summartyType, ParamCollection summaryFilter)
        {
            if (StringUtil.IsBlank(field)) throw new ArgumentNullException(nameof(field));

            #region 警告: 下面这个 bug 是为修正'关联表'定制的, 针对木材厂导出数据           

            if (field.StartsWith("UT_") && field.Contains("_COL_"))
            {
                field = field.Replace("_COL_", ".COL_");
            }

            #endregion


            string resultField = null;

            switch (summartyType)
            {
                case SummaryType.SUM: resultField = string.Format("SUM({0})", field); break;
                case SummaryType.COUNT: resultField = string.Format("COUNT({0})", field); break;
                case SummaryType.AVG: resultField = string.Format("AVG({0})", field); break;
                case SummaryType.MAX: resultField = string.Format("MAX({0})", field); break;
                case SummaryType.MIN: resultField = string.Format("MIN({0})", field); break;
            }

            if (resultField == null)
            {
                return 0;
            }

            decimal result = 0;

            Store store = this.Store;

            DbDecipher decipher = this.OpenDecipher();

            if (store.TSqlQuery.Enabeld)
            {
                StoreTSqlQuerty query = store.TSqlQuery;

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("SELECT {0} ", resultField).AppendFormat("FROM {0} ", query.Form);

                if (summaryFilter != null && summaryFilter.Count > 0)
                {
                    foreach (Param param in summaryFilter)
                    {
                        if (param.ParamMode == StoreParamMode.TSql)
                        {
                            if (!string.IsNullOrEmpty(query.Where))
                            {
                                query.Where += " AND ";
                            }

                            query.Where += param.Evaluate(HttpContext.Current, null);
                        }
                        else
                        {

                        }
                    }
                }

                string filterWhere;

                try
                {
                    filterWhere = SqlEngineHelper.GetTSQLWhereForFilter(store);
                }
                catch (Exception ex)
                {
                    throw new Exception("获取TSQL Where 过滤参数错误。", ex);
                }

                string tSqlWhere = query.Where;

                if (filterWhere.Length > 0)
                {
                    if (!StringUtil.IsBlank(tSqlWhere))
                    {
                        tSqlWhere += " AND ";
                    }

                    tSqlWhere += filterWhere.ToString();
                }


               IfUtil.NotBlank_AppendFormat(sb, "WHERE {0} ", tSqlWhere);

                string tSql = sb.ToString();

                try
                {
                    result = decipher.ExecuteScalar<decimal>(tSql);
                }
                catch (Exception ex)
                {
                    log.Error("执行汇总 SQL 语句错误： " + tSql, ex);
                }
            }
            else
            {
                LightModelFilter filter = new LightModelFilter(store.Model);

                AddFilter(filter, store.FilterParams);
                AddFilter(filter, store.SelectQuery);

                AddFilter(filter, summaryFilter);

                filter.Locks.Add(LockType.NoLock);

                filter.Fields = new string[] { resultField };

                try
                {
                    result = decipher.ExecuteScalar<decimal>(filter);
                }
                catch (Exception ex)
                {
                    log.Error($"执行汇总 SQL Filter 错误。实体名“{store.Model}”,汇总字段名“{resultField}”", ex);
                }
            }

            return result;
        }



        /// <summary>
        /// 获取某个字段的汇总信息（单表）
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="summartyType">类型</param>
        /// <returns></returns>
        public override decimal GetSummary(string field, SummaryType summartyType)
        {
            if (StringUtil.IsBlank(field)) { throw new ArgumentNullException(nameof(field)); }


            
            #region 警告: 下面这个 bug 是为修正'关联表'定制的, 针对木材厂导出数据           

            if(field.StartsWith("UT_") && field.Contains("_COL_"))
            {
                field = field.Replace("_COL_", ".COL_");
            }

            #endregion

            string resultField = null;

            switch (summartyType)
            {
                case SummaryType.SUM: resultField = string.Format("SUM({0})", field); break;
                case SummaryType.COUNT: resultField = string.Format("COUNT({0})", field); break;
                case SummaryType.AVG: resultField = string.Format("AVG({0})", field); break;
                case SummaryType.MAX: resultField = string.Format("MAX({0})", field); break;
                case SummaryType.MIN: resultField = string.Format("MIN({0})", field); break;
            }

            if (resultField == null)
            {
                return 0;
            }

            decimal result = 0;

            Store store = this.Store;

            DbDecipher decipher = this.OpenDecipher();

            if (store.TSqlQuery != null && store.TSqlQuery.Enabeld)
            {
                StoreTSqlQuerty query = store.TSqlQuery;

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("SELECT {0} ", resultField).AppendFormat("FROM {0} ", query.Form);

                IfUtil.NotBlank_AppendFormat(sb, "WHERE {0} ", query.Where);

                string tSql = sb.ToString();

                try
                {
                    result = decipher.ExecuteScalar<decimal>(tSql);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行汇总 SQL 语句错误： " + tSql, ex);
                }
            }
            else
            {
                LightModelFilter filter = new LightModelFilter(store.Model);
                filter.Locks.Add(LockType.NoLock);

                AddFilter(filter, store.FilterParams);
                AddFilter(filter, store.SelectQuery);

                filter.Fields = new string[] { resultField };


                result = decipher.ExecuteScalar<decimal>(filter);
            }

            return result;
        }



        /// <summary>
        /// 全部数据 
        /// </summary>
        /// <returns></returns>
        public override IList Select()
        {
            Store store = this.Store;

            HttpContext context = HttpContext.Current;

            DbDecipher decipher = this.OpenDecipher();



            IList models = null;

            if (store.TSqlQuery != null && store.TSqlQuery.Enabeld)
            {
                string tSql = SqlEngineHelper.GetTSQL(store, 0, false);

                models = decipher.GetModelList(tSql);

            }
            else
            {



                LightModelFilter filter = new LightModelFilter(store.Model);

                AddFilter(filter, store.FilterParams);
                AddFilter(filter, store.SelectQuery);

                if (!string.IsNullOrEmpty(store.SortText))
                {
                    filter.TSqlOrderBy = store.SortText;
                }
                else if (!string.IsNullOrEmpty(store.SortField))
                {
                    filter.TSqlOrderBy = string.Format("{0} ASC,{1} ASC", store.SortField, store.IdField);
                }


                models = decipher.GetModelList(filter);
            }


            return models;
        }


        private void SetFieldValue(LModel model, DataRecord rect, LModelElement modelElem)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "实体不能为空.");
            }

            if (rect == null)
            {
                throw new ArgumentNullException("rect", "DataRecord 数据记录不能为 null.");
            }

            if (modelElem == null)
            {
                throw new ArgumentNullException("modelElem", "实体元素不能为 null.");
            }


            LModelFieldElement fieldElem;

            string fieldName;

            foreach (DataField field in rect.Fields)
            {
                fieldName = field.Name;

                if (StringUtil.IsBlank(fieldName))
                {
                    continue;
                }

                if (!modelElem.TryGetField(fieldName, out fieldElem))
                {
                    continue;
                }

                object value = null;

                try
                {
                    value = ModelConvert.ChangeType(field.Value, fieldElem);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("数据转换错误。ModelConvert.ChangeType(...), field.Name = {0}, field.Value = {1}, fieldElem = {2}",
                        fieldName, field.Value, fieldElem.DBType), ex);
                }

                model[fieldName] = value;
                model.SetBlemish(fieldName, true);
                //fs.Add(field.Name);
            }

        }


        /// <summary>
        /// 获取需要更新实体信息
        /// </summary>
        /// <returns></returns>
        private LModel GetUpdateInfos(DataRecord rect, LModelElement modelElem)
        {
            if (this.Store.TSqlQuery != null && this.Store.TSqlQuery.Enabeld)
            {
                return null;
            }

            if (rect == null)
            {
                throw new ArgumentNullException("rect", "DataRecord 数据记录不能为 null.");
            }

            if (modelElem == null)
            {
                throw new ArgumentNullException("modelElem", "实体元素不能为 null.");
            }

            LModelFieldElementCollection fieldElems = modelElem.Fields;

            LModel model = new LModel(modelElem);
            string pkField = Store.IdField;

            LModelFieldElement pkFieldElem;

            if (!StringUtil.IsBlank(pkField))
            {
                if (!fieldElems.TryGetField(pkField, out pkFieldElem))
                {
                    throw new Exception(string.Format("数据表“{0}”中，主键字段“{1}”不存在。", modelElem.DBTableName, pkField));
                }

                object pk = ModelConvert.ChangeType(rect.Id, pkFieldElem);

                model[pkField] = pk;
            }

            model.SetTakeChange(true);

            SetFieldValue(model, rect, modelElem);

            return model;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="modelElem"></param>
        /// <param name="decipher"></param>
        /// <param name="models"></param>
        private ObjectEventArgs[] OnUpdatedModels(DataBatch batch, LModelElement modelElem, DbDecipher decipher, LModelList<LModel> models)
        {
            Store uiStore = this.Store;

            ObjectEventArgs[] eaList = null;

            if (uiStore.Has(StoreEvent.Updated))
            {
                eaList = new ObjectEventArgs[batch.Records.Count];
                int i = 0;

                foreach (DataRecord rec in batch.Records)
                {
                    eaList[i++] = uiStore.OnProUpdated(rec, rec.Model, null);
                }
            }

            return eaList;
        }


        /// <summary>
        /// 获取需要更新的是具体对象
        /// </summary>
        /// <param name="batch">数据批次，从json转换过来的对象。</param>
        /// <param name="modelElem"></param>
        /// <param name="decipher"></param>
        /// <param name="cancel">是否取消更新</param>
        /// <returns></returns>
        private LModelList<LModel> GetUpdatingModels(DataBatch batch, LModelElement modelElem, DbDecipher decipher, out bool cancel)
        {
            Store uiStore = this.Store;

            LModelList<LModel> models = new LModelList<LModel>();


            if (uiStore.Has(StoreEvent.Updating))
            {

                foreach (DataRecord rect in batch.Records)
                {
                    LModelFieldElement pkFieldElem = modelElem.Fields[uiStore.IdField];

                    object pk = ModelConvert.ChangeType(rect.Id, pkFieldElem);

                    if (uiStore.TranEnabled)
                    {
                        decipher.Locks.Add(LockType.RowLock);
                    }

                    LightModelFilter filter = new LightModelFilter(uiStore.Model);
                    AddFilter(filter, uiStore.FilterParams);
                    AddFilter(filter, uiStore.SelectQuery);

                    filter.And(uiStore.IdField, pk);

                    LModel model = decipher.GetModel(filter);   // decipher.GetModelByPk(modelElem.Name, pk);

                    if (model == null) { throw new Exception($"无法找到需要过滤的实体 \"{uiStore.Model}\", 请检查数据仓库的过滤条件."); }

                    model.SetTakeChange(true);
                    SetFieldValue(model, rect, modelElem);


                    bool saveItemCancel = uiStore.OnUpdating(rect, model, null);

                    if (saveItemCancel)
                    {
                        cancel = true;
                        return null;
                    }

                    rect.Model = model;

                    models.Add(model);
                }

            }
            else
            {
                foreach (DataRecord rect in batch.Records)
                {
                    LModel model = GetUpdateInfos(rect, modelElem);


                    bool saveItemCancel = uiStore.OnUpdating(rect, model, null);

                    if (saveItemCancel)
                    {
                        cancel = true;
                        return null;
                    }

                    rect.Model = model;
                    models.Add(model);
                }
            }

            cancel = false;

            return models;
        }


        /// <summary>
        /// 触发异常
        /// </summary>
        private void ThrowException(ref bool isExceptionHandled, ObjectEventArgs[] eventArgs)
        {
            if (eventArgs != null)
            {
                foreach (var saveEa in eventArgs)
                {
                    if (saveEa.Exception != null && saveEa.ExceptionHandled)
                    {
                        isExceptionHandled = false;

                        throw saveEa.Exception;
                    }
                }
            }
        }


        /// <summary>
        /// 保存全部（事务模式）
        /// </summary>
        private int SaveAll_ForTran()
        {
            int count = 0;
            Store uiStore = this.Store;

            ObjectEventArgs[] saveingEaList;
            ObjectEventArgs[] savedEaList;

            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    this.m_Decipher = decipher;

                    bool isExceptionHandled = true;

                    try
                    {
                        count = this.SaveAll(uiStore, decipher, out saveingEaList, out savedEaList);

                        //触发保存前异常
                        ThrowException(ref isExceptionHandled, saveingEaList);

                        //触发保存后的异常
                        ThrowException(ref isExceptionHandled, savedEaList);


                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        if (isExceptionHandled)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 保存全部
        /// </summary>
        /// <returns></returns>
        public override int SaveAll()
        {
            Store uiStore = this.Store;

            int count = 0;

            if (uiStore.TSqlQuery != null && uiStore.TSqlQuery.Enabeld)
            {
                if (uiStore.Has(StoreEvent.Updating))
                {
                    DataBatch batch = uiStore.GetDataChangedBatch();

                    DataRecord curRecord = uiStore.GetDataCurrent();

                    if (batch.Records.Count == 0 && curRecord != null)
                    {
                        batch.Records.Add(curRecord);
                    }

                    if (batch.Records.Count == 0) { return 0; }

                    bool cancel = false;

                    foreach (var item in batch.Records)
                    {
                        cancel = uiStore.OnProUpdateing(curRecord, null, null);

                        if (cancel)
                        {
                            break;
                        }
                    }

                    if (cancel)
                    {
                        return 0;
                    }

                    foreach (var item in batch.Records)
                    {
                        uiStore.OnProUpdated(curRecord, null, null);

                    }


                }
            }
            else
            {
                if (uiStore.TranEnabled)
                {
                    count = SaveAll_ForTran();
                }
                else
                {
                    ObjectEventArgs[] saveingEaList;
                    ObjectEventArgs[] savedEaList;

                    DbDecipher decipher = this.OpenDecipher();
                    count = this.SaveAll(uiStore, decipher, out saveingEaList, out savedEaList);
                }
            }

            return count;
        }


        private void SetModelUpdateValue(Store uiStore, LModelList<LModel> models)
        {
            if (uiStore.UpdateParams.Count == 0)
            {
                return;
            }

            HttpContext context = HttpContext.Current;

            if (models.Count > uiStore.UpdateParams.Count)
            {
                foreach (LModel model in models)
                {
                    foreach (Param p in uiStore.UpdateParams)
                    {
                        model[p.Name] = p.Evaluate(context, uiStore);
                    }
                }
            }
            else
            {
                foreach (Param p in uiStore.UpdateParams)
                {
                    foreach (LModel model in models)
                    {
                        model[p.Name] = p.Evaluate(context, uiStore);
                    }

                }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiStore">UI 数据仓库</param>
        /// <param name="decipher">数据操作对象</param>
        /// <param name="saveingEaList">保存前触发的异常事件</param>
        /// <param name="savedEaList">保存后触发的异常事件</param>
        /// <returns></returns>
        private int SaveAll(Store uiStore, DbDecipher decipher, out ObjectEventArgs[] saveingEaList, out ObjectEventArgs[] savedEaList)
        {
            //ObjectEventArgs[] saveingEaList = null;
            //ObjectEventArgs[] savedEaList = null;   //执行事件返回个
            savedEaList = null;
            saveingEaList = null;

            int count = 0;

            DataBatch batch = uiStore.GetDataChangedBatch();

            DataRecord curRecord = uiStore.GetDataCurrent();

            if (batch.Records.Count == 0 && curRecord != null)
            {
                batch.Records.Add(curRecord);
            }

            if (batch.Records.Count == 0) { return 0; }

            if (StringUtil.IsBlank(uiStore.Model))
            {
                throw new Exception($"未定义“{uiStore.ID}”Store 的 Model 属性.");
            }

            LModelElement modelElem = LightModel.GetLModelElement(uiStore.Model);

            if (modelElem == null)
            {
                throw new Exception(string.Format("数据仓库 Store.Model 对应的实体“{0}”不存在, StoreID={1}。", uiStore.Model, uiStore.ID));
            }

            if (StringUtil.IsBlank(uiStore.IdField))
            {
                throw new Exception(string.Format("没有指定实体“{0}”主键字段。 IdField", modelElem.FullName));
            }



            LModelFieldElement pkFieldElem = modelElem.Fields[uiStore.IdField];


            bool isCancelItem = false;

            #region 提示例子

            //foreach (DataRecord cccc in batch.Records)
            //{
            //    //cccc.MarkInvalid("COL_1", "严重错误啦");

            //    foreach (DataField df in cccc.Fields)
            //    {
            //        this.Store.MarkInvalid(cccc.Id, df.Name, "严重错误");
            //    }
            //}

            #endregion

            LModelList<LModel> models = GetUpdatingModels(batch, modelElem, decipher, out isCancelItem);

            if (isCancelItem)
            {
                return 0;
            }


            LModel curModel = null;

            SetModelUpdateValue(uiStore, models);

            ObjectEventItemCollection eventItems = new ObjectEventItemCollection();
            eventItems["decipher"] = decipher;

            bool saveCancel = uiStore.OnSavingAll(models, eventItems);

            if (saveCancel)
            {
                return 0;
            }


            string[] blemishFieldAll = new string[0]; //全部被污染的字段 


            object curPk = null;

            if (curRecord != null)
            {
                curPk = ModelConvert.ChangeType(curRecord.Id, pkFieldElem);
            }


            try
            {
                int i = 0;

                foreach (LModel model in models)
                {
                    DataRecord rec = batch.Records[i++];

                    if (model.GetTakeChange())
                    {
                        string idField = uiStore.IdField;

                        model.SetOriginalValue(idField, rec.Id);
                        model.SetValue(idField, rec.Id);
                        model.SetBlemish(idField, false);  //禁止更新主键
                    }


                    string[] fields = model.GetBlemishFields();
                    blemishFieldAll = ArrayUtil.Union(blemishFieldAll, fields);

                    count += decipher.UpdateModel(model, true);

                    object pk = model.GetPk();

                    if (curPk != null && !curPk.Equals(pk))
                    {
                        curModel = model;
                    }


                    ObjectEventArgs ea = uiStore.OnProUpdated(rec, model, eventItems);

                    if (ea != null && ea.Exception != null)
                    {
                        count--;

                        savedEaList = new ObjectEventArgs[] { ea };
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("更新数据失败", ex);
            }


            uiStore.OnSavedAll(models, blemishFieldAll, eventItems);


            if (models.Count > 1)
            {
                foreach (LModel model in models)
                {
                    string[] fields = model.GetBlemishFields();

                    if (fields.Length == 0)
                    {
                        continue;
                    }


                    foreach (var field in fields)
                    {
                        object value;

                        if (model.TryGetValue(field, out value))
                        {
                            uiStore.SetRecordValue(model.GetPk(), field, value);
                        }
                    }
                }

                foreach (DataRecord rect in batch.Records)
                {
                    uiStore.CommitChanges(rect);
                }
            }
            else if (models.Count == 1)
            {
                LModel model = models[0];

                DataRecord rect = batch.Records[0];

                List<string> commitFs = new List<string>();

                foreach (var field in rect.Fields)
                {
                    commitFs.Add(field.Name);
                }

                string[] biFs = model.GetBlemishFields();


                if (biFs.Length > 0)
                {
                    foreach (var field in biFs)
                    {
                        if (commitFs.Contains(field))
                        {
                            continue;
                        }

                        object value;

                        if (model.TryGetValue(field, out value))
                        {
                            uiStore.SetRecordValue(model.GetPk(), field, value);
                        }
                    }
                }

                uiStore.CommitChanges(rect);
            }

            //数据发生变化, 重新计算合计

            if (blemishFieldAll != null && blemishFieldAll.Length > 0)
            {
                ResetSummary(decipher, blemishFieldAll);
            }



            ////如果焦点行发生变化，就触发重新选择
            //if (curModel != null)
            //{
            //    uiStore.OnCurrentChanged(curRecord, curModel);
            //}

            return count;
        }

        /// <summary>
        /// 重置合计项目
        /// </summary>
        /// <param name="fields"></param>
        private void ResetSummary(DbDecipher decipher, string[] fields)
        {
            Store uiStore = this.Store;

            bool tSqlQueryEnabled = false;

            if (uiStore.TSqlQuery != null)
            {
                tSqlQueryEnabled = uiStore.TSqlQuery.Enabeld;
            }

            foreach (var field in fields)
            {
                SummaryField sField = uiStore.SummaryFields.Get(field);

                if (sField == null)
                {
                    continue;
                }

                string name = StringUtil.NoBlank(sField.Name, field);

                try
                {
                    if (tSqlQueryEnabled)
                    {
                        decimal value = GetSummary(sField.SrcViewField, sField.SummaryType, sField.Filter);
                        uiStore.SetSummary(name, value);
                    }
                    else
                    {
                        decimal value = GetSummary(field, sField.SummaryType, sField.Filter);
                        uiStore.SetSummary(name, value);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Store:{0},汇总错误.在字段“{1}”", uiStore.ID, field), ex);
                }


            }

        }



        /// <summary>
        /// （功能未启用）更新记录
        /// </summary>
        /// <returns></returns>
        public override int Update()
        {
            return 0;
        }

        private void InsertFull(LModel model, Store uiStore, HttpContext context)
        {

            //设置默认值
            foreach (Param p in uiStore.InsertParams)
            {
                try
                {
                    model[p.Name] = p.Evaluate(context, uiStore);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("处理数据仓库值错误。“{0}”属性 InsertParams, ParamType = “{1}”, Param.Name = “{2}”, DefaultValue = “{3}”",
                        uiStore.ID, p.GetType().FullName, p.Name, p.DefaultValue), ex);
                }
            }

        }

        /// <summary>
        /// 插入操作
        /// </summary>
        /// <returns></returns>
        public override int Insert()
        {
            Store uiStore = this.Store;

            HttpContext context = HttpContext.Current;
            LModel model = new LModel(this.Store.Model);

            model.SetTakeChange(true);

            InsertFull(model, uiStore, context);

            int count = 0;

            bool cancel = false;


            DbDecipher decipher = null;

            if (uiStore.TranEnabled)
            {
                using (TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    using (DbDecipher trnDecipher = DbDecipherManager.GetDecipherOpen())
                    {
                        m_Decipher = trnDecipher;

                        count = Insert_Item(uiStore, trnDecipher, model, out cancel);

                        tran.Complete();
                    }

                }

            }
            else
            {
                decipher = OpenDecipher();
                count = Insert_Item(uiStore, decipher, model, out cancel);
            }

            if (cancel) { return count; }

            decipher = decipher ?? OpenDecipher();



            if (uiStore.DefaultInsertPos == InsertPosition.First)
            {
                uiStore.Insert(0, model);
            }
            else
            {
                uiStore.Add(model);
            }

            int rowCount = 0;

            LightModelFilter filter = new LightModelFilter(uiStore.Model);
            filter.Locks.Add(LockType.NoLock);

            AddFilter(filter, uiStore.FilterParams);
            AddFilter(filter, uiStore.SelectQuery);

            try
            {
                rowCount = decipher.SelectCount(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取记录数量错误。数据控件ID：{uiStore.ID}。", ex);
            }

            uiStore.SetTotalCount(rowCount);


            return count;
        }

        private int Insert_Item(Store uiStore, DbDecipher decipher, LModel model, out bool cancel)
        {
            int count = 0;

            ObjectEventItemCollection eventItems = new ObjectEventItemCollection();
            eventItems["decipher"] = decipher;

            try
            {
                cancel = uiStore.OnPreInserting(model, eventItems);

                if (cancel) { return count; }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行 {0}.OnInserting（...) 函数错误。", uiStore.ID), ex);
            }



            if (!StringUtil.IsBlank(uiStore.SortField))
            {
                decimal userSEQ = model.Get<decimal>(uiStore.SortField);

                if (userSEQ == 0)
                {
                    model[uiStore.SortField] = 100000000;
                }
            }




            try
            {
                count = decipher.InsertModel(model);
            }
            catch (Exception ex)
            {
                LModelElement modelElem = model.GetModelElement();

                log.DebugFormat("插入 {0} 表失败，以下是字段值。", modelElem.DBTableName);

                foreach (LModelFieldElement fieldElem in modelElem.Fields)
                {
                    log.DebugFormat("字段 {0} = {1}\r{2}\r{3}", fieldElem.DBField, model[fieldElem], model.IsNull(fieldElem.DBField), fieldElem.DBType);
                }

                throw new Exception(string.Format("插入数据记录失败。数据仓库“{0}”, EntityStoreEngine.Insert(...)", uiStore.ID), ex); ;
            }

            try
            {
                uiStore.OnProInserted(model, eventItems);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行'插入后'事件错误 {1}.OnInserted(...)", uiStore.ID), ex);
            }

            return count;
        }


        private void ProParams_DataRecords(LightModelFilter filter, Param p, DataRecordCollection records)
        {
            if (StringUtil.IsBlank(p.Name))
            {
                return;
            }

            if (records == null || records.Count == 0)
            {
                filter.And(p.Name, new object[0], Logic.In);

                return;
            }

            Store store = this.Store;

            LModelElement modelElem;
            LModelFieldElement fieldElem;

            modelElem = LModelDna.GetElementByName(store.Model);

            if (modelElem.TryGetField(store.IdField, out fieldElem))
            {

            }


            List<object> pkList = new List<object>();

            object pk;

            foreach (DataRecord rec in records)
            {
                if (fieldElem != null)
                {
                    pk = ModelConvert.ChangeType(rec.Id, fieldElem);
                }
                else
                {
                    pk = rec.Id;
                }

                pkList.Add(pk);
            }

            filter.And(p.Name, pkList.ToArray(), Logic.In);
        }


        private string ConvertValue_Like(Logic logic, string value)
        {
            if (value.Contains("%"))
            {
                return value;
            }

            switch (logic)
            {
                case Logic.NotLike:
                case Logic.Like: value = "%" + value + "%"; break;
                case Logic.LeftLike: value = "%" + value; break;
                case Logic.RightLike: value += "%"; break;
            }

            return value;
        }


        /// <summary>
        /// 添加过滤参数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="paramList"></param>
        private void AddFilter(LightModelFilter filter, ParamCollection paramList)
        {
            if (paramList == null || paramList.Count == 0)
            {
                return;
            }

            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }


            HttpContext context = HttpContext.Current;

            Store uiStore = this.Store;

            LModelElement modelElem;
            LModelFieldElement fieldElem;

            if (LModelDna.TryGetElementByName(uiStore.Model, out modelElem))
            {

            }

            object value;

            foreach (Param p in paramList)
            {
                try
                {
                    value = p.Evaluate(context, uiStore);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("执行 Param.Evaluate(...) 错误. 控件类型 = “{0}”, Param.Name = {1},DefaultValue = {2}.",
                        p.GetType().FullName, p.Name, p.DefaultValue), ex);
                }


                if (p.ParamMode == StoreParamMode.Default)
                {
                    if (value is DataRecordCollection)
                    {
                        ProParams_DataRecords(filter, p, (DataRecordCollection)value);
                    }
                    else if (!StringUtil.IsBlank(p.Name))
                    {
                        //允许忽略空值
                        if (p.IgnoreEmpty && (value == null || StringUtil.IsBlank(value.ToString())))
                        {
                            continue;
                        }


                        Logic logic;


                        logic = ModelConvert.ToLogic(p.Logic);


                        if (ModelHelper.IsLikeType(logic))
                        {
                            value = ConvertValue_Like(logic, value.ToString());
                        }
                        else if (logic == Logic.In || logic == Logic.NotIn)
                        {
                            if (!(value is Array))
                            {
                                if (value is string)
                                {
                                    string[] valuePs = StringUtil.Split((string)value, ",");

                                    value = valuePs;
                                }
                            }
                        }

                        if (modelElem != null)
                        {
                            if (!(value is Array) && modelElem.TryGetField(p.Name, out fieldElem))
                            {

                                value = ModelConvert.ChangeType(value, fieldElem);
                            }
                        }

                        filter.And(p.Name, value, logic);
                    }
                }
                else if (p.ParamMode == StoreParamMode.TSql)
                {
                    if (!StringUtil.IsBlank(filter.TSqlWhere))
                    {
                        filter.TSqlWhere += " AND ";
                    }

                    filter.TSqlWhere += (string)value;
                }

            }
        }


        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        public override int Delete()
        {
            int count = 0;

            Store uiStore = this.Store;



            if (uiStore.TSqlQuery != null && uiStore.TSqlQuery.Enabeld)
            {
                bool cancel = false;

                if (uiStore.Has(StoreEvent.BatchDeleting))
                {
                    cancel = uiStore.OnBatchDeleting(null, null);
                }

                if (!cancel && uiStore.Has(StoreEvent.BatchDeleted))
                {
                    uiStore.OnBatchDeleted(null, null);
                }
            }
            else
            {
                DbDecipher decipher = OpenDecipher();

                if (uiStore.DeleteRecycle)
                {
                    count = this.DeleteRecycle(uiStore, decipher);
                }
                else
                {
                    count = this.DeleteRows(uiStore, decipher);
                }
            }

            return count;
        }


        private int DeleteRows(Store uiStore, DbDecipher decipher)
        {
            if (decipher == null) { throw new ArgumentNullException("decipher"); }
            if (uiStore == null) { throw new ArgumentNullException("uiStore"); }

            int count = 0;

            LightModelFilter filter = new LightModelFilter(uiStore.Model);

            AddFilter(filter, uiStore.FilterParams);
            AddFilter(filter, uiStore.DeleteQuery);

            LModelList<LModel> models = decipher.GetModelList(filter);


            if (uiStore.TranEnabled)
            {
                foreach (LModel model in models)
                {
                    Exception error = null;
                    bool cancel = false;

                    using (TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        using (DbDecipher tranDecipher = DbDecipherManager.GetDecipherOpen())
                        {
                            m_Decipher = tranDecipher;

                            count += DeleteRows_Item(uiStore, tranDecipher, HttpContext.Current, model, out error, out cancel);


                            tran.Complete();
                        }

                    }
                }

            }
            else
            {

                foreach (LModel model in models)
                {
                    Exception error = null;
                    bool cancel = false;

                    count += DeleteRows_Item(uiStore, decipher, HttpContext.Current, model, out error, out cancel);

                }
            }



            return count;
        }

        private int DeleteRows_Item(Store uiStore, DbDecipher decipher, HttpContext context, LModel model, out Exception error, out bool cancel)
        {
            int count = 0;

            error = null;

            ObjectEventItemCollection eventItems = new ObjectEventItemCollection();
            eventItems["decipher"] = decipher;

            model.SetTakeChange(true, true);

            cancel = uiStore.OnDeleting(model, eventItems);

            if (cancel) { return count; }

            count = decipher.DeleteModel(model);


            uiStore.OnProDeleted(model, false, eventItems);

            object pk = model[uiStore.IdField];

            uiStore.RemoveById(pk);

            return count;
        }


        /// <summary>
        /// 删除到回收站
        /// </summary>
        /// <returns></returns>
        private int DeleteRecycle(Store uiStore, DbDecipher decipher)
        {
            if (decipher == null) { throw new ArgumentNullException("decipher"); }
            if (uiStore == null) { throw new ArgumentNullException("uiStore"); }

            LightModelFilter filter = new LightModelFilter(uiStore.Model);

            AddFilter(filter, uiStore.FilterParams);
            AddFilter(filter, uiStore.DeleteQuery);


            int count = 0;


            LModelList<LModel> models = decipher.GetModelList(filter);


            bool cancle = uiStore.OnBatchDeleting(models, null);

            if (cancle)
            {
                return 0;
            }


            HttpContext context = HttpContext.Current;


            ParamCollection recys = uiStore.DeleteRecycleParams;
            int recyCount = recys.Count;
            string[] recyFields = new string[recyCount];

            for (int i = 0; i < recyCount; i++)
            {
                recyFields[i] = recys[i].Name;
            }

            if (uiStore.TranEnabled)
            {

                bool isExceptionHandled = true;

                Exception error = null;

                foreach (LModel model in models)
                {
                    bool cancel;


                    using (TransactionScope tran = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        using (DbDecipher tranDecipher = DbDecipherManager.GetDecipherOpen())
                        {

                            count += DeleteRecycle_Item(uiStore, tranDecipher, context, model, recyFields, isExceptionHandled, out error, out cancel);

                            if (cancel) { continue; }

                            if (error != null) { break; }
                        }

                        tran.Complete();
                    }
                }

                LoadSummary();

                uiStore.OnBatchDeleted(models, null);
            }
            else
            {

                bool isExceptionHandled = true;

                Exception error = null;

                foreach (LModel model in models)
                {
                    bool cancel;

                    count += DeleteRecycle_Item(uiStore, decipher, context, model, recyFields, isExceptionHandled, out error, out cancel);

                    if (cancel) { continue; }

                    if (error != null) { break; }
                }

                LoadSummary();

                uiStore.OnBatchDeleted(models, null);
            }

            return count;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiStore"></param>
        /// <param name="decipher"></param>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <param name="recyFields"></param>
        /// <param name="isExceptionHandled"></param>
        /// <param name="cancel"></param>
        private int DeleteRecycle_Item(Store uiStore, DbDecipher decipher, HttpContext context, LModel model, string[] recyFields, bool isExceptionHandled, out Exception error, out bool cancel)
        {
            int count = 0;

            error = null;

            ObjectEventItemCollection eventItems = new ObjectEventItemCollection();
            eventItems["decipher"] = decipher;

            model.SetTakeChange(true);

            cancel = uiStore.OnDeleting(model, eventItems);

            if (cancel) { return count; }

            SetModelValue(model, uiStore, uiStore.DeleteRecycleParams, context);

            count = decipher.UpdateModelProps(model, recyFields);

            ObjectEventArgs ea = uiStore.OnDeleted(model, true, eventItems);

            if (ea != null && ea.Exception != null)
            {
                isExceptionHandled = false;
                error = ea.Exception;

                throw ea.Exception;
            }

            object pk = model[uiStore.IdField];

            uiStore.RemoveById(pk);


            if (ea != null && ea.Exception != null)
            {
                LModelElement modelElem = ModelElemHelper.GetElem(model);

                error = new Exception($"删除到回收站错误! table={modelElem.DBTableName}, pk={model.GetPk()}", ea.Exception);
            }

            return count;
        }


        private void SetModelValue(LModel model, Store uiStore, ParamCollection recys, HttpContext context)
        {
            if (model == null) { throw new ArgumentNullException("model"); }
            if (recys == null) { throw new ArgumentNullException("recys"); }

            if (context == null) { throw new ArgumentNullException("context"); }


            foreach (Param p in recys)
            {
                try
                {

                    model[p.Name] = p.Evaluate(context, uiStore);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("执行 Param.Evaluate(...) 错误. 控件类型 = “{0}”, Param.Name = {1}", p.GetType().FullName, p.Name), ex);
                }
            }
        }


        public void RecordSortFilter(LightModelFilter filter)
        {
            if (filter == null) { throw new ArgumentNullException("filter"); }

            Store uiStore = this.Store;

            if (uiStore == null) { throw new Exception("Store 不能为空."); }

            AddFilter(filter, uiStore.FilterParams);
            AddFilter(filter, uiStore.SelectQuery);
        }

        /// <summary>
        /// 排序重置
        /// </summary>
        /// <returns></returns>
        public override bool SortReset()
        {
            Store uiStore = this.Store;

            if (StringUtil.IsBlank(uiStore.SortField))
            {
                throw new Exception("排序失败，因为您未指定排序的字段名 Store.SortField 。");
            }

            RecordSortFactory rs = new RecordSortFactory(this);
            rs.TableName = uiStore.Model;
            rs.IdField = uiStore.IdField;
            rs.SortField = uiStore.SortField;
            rs.FilterMethod = this.GetType().GetMethod("RecordSortFilter");
            rs.Decipher = this.OpenDecipher();

            rs.SortReset();

            return true;
        }

        /// <summary>
        /// 焦点行下移
        /// </summary>
        /// <returns></returns>
        public override bool MoveDown()
        {
            Store uiStore = this.Store;

            if (StringUtil.IsBlank(uiStore.SortField))
            {
                throw new Exception("排序失败，因为您未指定排序的字段名 Store.SortField 。");
            }

            List<int> ids = new List<int>();

            DataRecord record = uiStore.GetDataCurrent();

            if (record != null)
            {
                ids.Add(StringUtil.ToInt(record.Id));
            }

            if (ids.Count == 0)
            {
                return false;
            }

            RecordSortFactory rs = new RecordSortFactory(this);
            rs.TableName = uiStore.Model;
            rs.IdField = uiStore.IdField;
            rs.SortField = uiStore.SortField;
            rs.FilterMethod = this.GetType().GetMethod("RecordSortFilter");
            rs.Decipher = this.OpenDecipher();

            bool result = rs.MoveDown(ids.ToArray());

            rs.Dispose();

            if (result)
            {
                uiStore.Refresh();
                uiStore.SetCurrntForId(record.Id);
            }

            return result;
        }

        /// <summary>
        /// 上移动
        /// </summary>
        /// <returns></returns>
        public override bool MoveUp()
        {
            Store uiStore = this.Store;

            if (StringUtil.IsBlank(uiStore.SortField))
            {
                throw new Exception("排序失败，因为您未指定排序的字段名 Store.SortField 。");
            }

            List<int> ids = new List<int>();

            DataRecord record = uiStore.GetDataCurrent();

            if (record != null)
            {
                ids.Add(StringUtil.ToInt(record.Id));
            }

            if (ids.Count == 0)
            {
                return false;
            }


            RecordSortFactory rs = new RecordSortFactory(this);
            rs.TableName = uiStore.Model;
            rs.IdField = uiStore.IdField;
            rs.SortField = uiStore.SortField;
            rs.FilterMethod = this.GetType().GetMethod("RecordSortFilter");
            rs.Decipher = this.OpenDecipher();
            //rs.TSqlWhere = string.Format("SEC_LEVEL < 6 AND IG2_TABLE_ID = {0}", id);

            bool result = rs.MoveUp(ids.ToArray());

            rs.Dispose();

            if (result)
            {
                uiStore.Refresh();
                uiStore.SetCurrntForId(record.Id);
            }

            return result;
        }


        /// <summary>
        /// 设置当前焦点行
        /// </summary>
        /// <param name="record">数据记录</param>
        public override void SetCurRecord(DataRecord record)
        {
            Store uiStore = this.Store;

            if (uiStore == null) { throw new Exception(string.Format("Store:{0},数据仓库不能为空.", uiStore.ID)); }

            LModel model = null;

            if (record != null)
            {
                LModelElement modelElem = LModelDna.GetElementByName(uiStore.Model);

                if (modelElem == null) { throw new Exception(string.Format("Store:{0}, 实体不存在 {1} ", uiStore.ID, uiStore.Model)); }

                model = GetUpdateInfos(record, modelElem);
            }

            uiStore.OnCurrentChanged(record, model, null);
        }

        public override void PreCurrentChanged(DataRecord record, object data)
        {
            Store uiStore = this.Store;

            if (uiStore == null) { throw new Exception(string.Format("Store:{0},数据仓库不能为空.", uiStore.ID)); }

            if (record != null && data == null)
            {
                LModelElement modelElem = LModelDna.GetElementByName(uiStore.Model);
                LModel model = GetUpdateInfos(record, modelElem);

                uiStore.OnCurrentChanged(record, model, null);
            }
            else
            {
                uiStore.OnCurrentChanged(record, data, null);
            }

        }


        /// <summary>
        /// 判断是否有子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override bool HasChild(object parent)
        {
            bool exist = false;
            Store store = this.Store;
            string tSqlWhere;

            if (parent is string)
            {
                tSqlWhere = (string)parent;
            }
            else
            {
                object itemId = LightModel.GetFieldValue(parent, store.IdField);

                tSqlWhere = $"{store.ParentField}='{itemId}'";

            }

            LightModelFilter filter = new LightModelFilter(store.Model);
            filter.Locks.Add(LockType.NoLock);

            AddFilter(filter, store.FilterParams);
            AddFilter(filter, store.SelectQuery);

            if (!StringUtil.IsBlank(filter.TSqlWhere))
            {
                filter.TSqlWhere += $" AND ({tSqlWhere})";
            }
            else
            {
                filter.TSqlWhere = tSqlWhere;
            }


            DbDecipher decipher = this.OpenDecipher();

            exist = decipher.ExistsModels(filter);



            return exist;
        }

    }
}
