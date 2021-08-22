using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.DbCascade;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EC5.IG2.BizBase
{

    /// <summary>
    /// 联动规则
    /// </summary>
    public static class DbCascadeRule
    {

        /// <summary>
        /// 填充更新的数据
        /// </summary>
        private static void FullForUpdate(LModel model)
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

        private static DbCascadeFactory ExectRule(DbDecipher decipher, Store store, LModel model)
        {
            DbCascadeFactory fty = new DbCascadeFactory(decipher);
            fty.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                if (dcE.Steps != null && dcE.Steps.Length > 0)
                {
                    EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
                }
            };

            //单元格公式
            LCodeFactory lcFactory = new LCodeFactory();
            lcFactory.ExecLCode(store, model);

            //简单流程
            LCodeValueFactory lcvFactiry = new LCodeValueFactory();
            lcvFactiry.ExecLCode(store, model);

            return fty;
        }
        

        private static LModel BaseInsert(DbDecipher decipher,   Store store, LModel model)
        {
            model.SetTakeChange(true);
            model.SetBlemishAll(true);

            

            DbCascadeFactory fty = ExectRule(decipher,store, model);
            

            decipher.InsertModel(model);

            BizDbStepPath stepPath = fty.Inserted(store, model);

            if (stepPath.Errors.Count > 0)
            {
                throw new Exception(stepPath.Errors[0]);
            }

            return model;
        }


        /// <summary>
        /// 触发插入联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        public static LModel Insert(Store store, LModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "参与'联动插入'的实体对象不能为空.");
            }

            if (Transaction.Current == null)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                model = BaseInsert(decipher, store, model);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                {
                    DbDecipher decipher = ModelAction.OpenDecipher();
                    model = BaseInsert(decipher, store, model);

                    ts.Complete();
                }
            }

            return model;
        }


        public static IEnumerable<LModel> BaseInsert(DbDecipher decipher, Store store, IEnumerable<LModel> models)
        {

            foreach (var model in models)
            {
                model.SetTakeChange(true);
                model.SetBlemishAll(true);

                DbCascadeFactory fty = ExectRule(decipher,store, model);

                decipher.InsertModel(model);

                BizDbStepPath stepPath = fty.Inserted(store, model);

                if (stepPath.Errors.Count > 0)
                {
                    throw new Exception(stepPath.Errors[0]);
                }
            }


            return models;
        }


        /// <summary>
        /// 触发插入联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Insert(Store store, IEnumerable<LModel> models)
        {
            if (models == null)
            {
                throw new ArgumentNullException("model", "参与'联动插入'的实体集合不能为空.");
            }

            if (Transaction.Current == null)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                models = BaseInsert(decipher, store, models);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                {
                    DbDecipher decipher = ModelAction.OpenDecipher();
                    models = BaseInsert(decipher, store, models);

                    ts.Complete();
                }
            }

            return models;


        }


        /// <summary>
        /// 触发插入联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Insert(DbDecipher decipher, Store store, IEnumerable<LModel> models)
        {
            if (models == null)
            {
                throw new ArgumentNullException("model", "参与'联动插入'的实体集合不能为空.");
            }


            if (Transaction.Current == null)
            {
                models = BaseInsert(decipher, store, models);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                {
                    models = BaseInsert(decipher, store, models);

                    ts.Complete();
                }
            }

            return models;


        }

        /// <summary>
        /// 触发插入联动规则
        /// </summary>
        /// <param name="model"></param>
        public static LModel Insert(LModel model)
        {
            return Insert(null, model);
        }

        /// <summary>
        /// 触发插入联动规则
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Insert(IEnumerable<LModel> models)
        {
            return Insert(null, models);
        }



        private static LModel BaseUpdate(DbDecipher decipher, Store store, LModel model)
        {

            DbCascadeFactory fty = ExectRule(decipher,store, model);

            FullForUpdate(model);

            decipher.Locks.Add(LockType.RowLock);
            decipher.UpdateModel(model, true);

            BizDbStepPath stepPath = fty.Updated(store, model);

            if (stepPath.Errors.Count > 0)
            {
                throw new Exception(stepPath.Errors[0]);
            }

            return model;
        }

        /// <summary>
        /// 触发更新联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        public static LModel Update(DbDecipher decipher, Store store, LModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "参与'联动更新'的实体对象不能为空.");
            }


            if (Transaction.Current == null)
            {
                model = BaseUpdate(decipher, store, model);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                {
                    model = BaseUpdate(decipher, store, model);

                    ts.Complete();
                }
            }

            return model;
        }

        /// <summary>
        /// 触发更新联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        public static LModel Update(Store store, LModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "参与'联动更新'的实体对象不能为空.");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (Transaction.Current == null)
            {

                model = BaseUpdate(decipher, store, model);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                {
                    model = BaseUpdate(decipher, store, model);

                    ts.Complete();
                }
            }


            return model;
        }


        private static IEnumerable<LModel> BaseUpdate(DbDecipher decipher,Store store, IEnumerable<LModel> models)
        {
            
            foreach (var model in models)
            {
                DbCascadeFactory fty = ExectRule(decipher,store, model);

                FullForUpdate(model);

                decipher.UpdateModel(model, true);

                BizDbStepPath stepPath = fty.Updated(store, model);

                if (stepPath.Errors.Count > 0)
                {
                    throw new Exception(stepPath.Errors[0]);
                }
            }

            return models;
        }



        /// <summary>
        /// 触发更新联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Update(DbDecipher decipher, Store store, IEnumerable<LModel> models)
        {

            if (models == null)
            {
                throw new ArgumentNullException("model", "参与'联动更新'的实体集合不能为空.");
            }


            if (Transaction.Current == null)
            {
                models = BaseUpdate(decipher, store, models);
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                {
                    models = BaseUpdate(decipher, store, models);

                    ts.Complete();
                }
            }

            return models;
        }

        /// <summary>
        /// 触发更新联动规则
        /// </summary>
        /// <param name="model"></param>
        public static LModel Update(LModel model)
        {
            return Update(null, model);
        }

        /// <summary>
        /// 触发更新联动规则
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Update(IEnumerable<LModel> models)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            return Update(decipher, null, models);
        }

        private static LModel BaseDelete(DbDecipher decipher, Store store,LModel model)
        {

            DbCascadeFactory fty = ExectRule(decipher, store, model);

            decipher.DeleteModel(model);

            BizDbStepPath stepPath = fty.Deleted(store, model);

            if (stepPath.Errors.Count > 0)
            {
                throw new Exception(stepPath.Errors[0]);
            }

            return model;
        }

        /// <summary>
        /// 触发删除联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        public static LModel Delete(Store store, LModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "参与'联动删除'的实体对象不能为空.");
            }

            LModel delModel;

            try
            {
                if (Transaction.Current == null)
                {
                    DbDecipher decipher = ModelAction.OpenDecipher();
                    delModel = BaseDelete(decipher, store, model);
                }
                else
                {
                    TransactionOptions tOpt = new TransactionOptions();
                    tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                    {
                        DbDecipher decipher = ModelAction.OpenDecipher();
                        delModel = BaseDelete(decipher, store, model);
                        ts.Complete();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("触发删除联动规则错误.", ex);
            }

            return delModel;
        }


        private static IEnumerable<LModel> BaseDelete(DbDecipher decipher, Store store, IEnumerable<LModel> models)
        {

            foreach (var model in models)
            {
                DbCascadeFactory fty = ExectRule(decipher,store, model);

                decipher.DeleteModel(model);

                BizDbStepPath stepPath = fty.Deleted(store, model);

                if (stepPath.Errors.Count > 0)
                {
                    throw new Exception(stepPath.Errors[0]);
                }
            }

            return models;
        }


        /// <summary>
        /// 触发删除联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Delete(Store store, IEnumerable<LModel> models)
        {
            if (models == null)
            {
                throw new ArgumentNullException("model", "参与'联动删除'的实体集合不能为空.");
            }


            try
            {
                if (Transaction.Current == null)
                {
                    DbDecipher decipher = ModelAction.OpenDecipher();
                    models = BaseDelete(decipher,store, models);
                }
                else
                {
                    TransactionOptions tOpt = new TransactionOptions();
                    tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                    {
                        DbDecipher decipher = ModelAction.OpenDecipher();
                        models = BaseDelete(decipher,store, models);

                        ts.Complete();
                    }

                }
            }
            catch(Exception ex)
            {
                throw new Exception("触发删除联动规则错误.", ex);
            }

            return models;
        }


        /// <summary>
        /// 触发删除联动规则
        /// </summary>
        /// <param name="store"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Delete(DbDecipher decipher, Store store, IEnumerable<LModel> models)
        {
            if (models == null)
            {
                throw new ArgumentNullException("model", "参与'联动删除'的实体集合不能为空.");
            }


            try
            {
                if (Transaction.Current == null)
                {
                    models = BaseDelete(decipher,store, models);
                }
                else
                {
                    TransactionOptions tOpt = new TransactionOptions();
                    tOpt.IsolationLevel = IsolationLevel.ReadCommitted;

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tOpt))
                    {
                        models = BaseDelete(decipher, store, models);

                        ts.Complete();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("触发删除联动规则错误.", ex);
            }

            return models;
        }


        /// <summary>
        /// 触发删除联动规则
        /// </summary>
        /// <param name="model"></param>
        public static LModel Delete(LModel model)
        {
            return Delete(null, model);
        }

        /// <summary>
        /// 触发删除联动规则
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<LModel> Delete(IEnumerable<LModel> models)
        {
            return Delete(null, models);
        }


        /// <summary>
        /// 绑定数据仓库
        /// </summary>
        /// <param name="uiStore"></param>
        public static void Bind(Store uiStore)
        {

            //单元格公式
            LCodeFactory lcFty = new LCodeFactory();
            lcFty.BindStore(uiStore);

            //简单流程
            LCodeValueFactory lcvFty = new LCodeValueFactory();
            lcvFty.BindStore(uiStore);


            DbCascadeFactory fty = new DbCascadeFactory();
            fty.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                if (dcE.Steps != null && dcE.Steps.Length > 0)
                {
                    EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
                }
            };
            fty.BindStore(uiStore);
        }

    }

}
