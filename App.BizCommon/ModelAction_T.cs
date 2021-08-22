using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.Data;
using System.Collections;
using HWQ.Entity.Filter;

namespace App.BizCommon
{

    public class ModelAction<T> : ModelAction
    {
        public virtual DataTable GetTableAll()
        {
            DataTable table = this.Decipher.GetDataTable<T>();

            return table;
        }

        public virtual DataTable GetTableForFilter(LightModelFilter filter)
        {
            return null;
        }


        public virtual T UpdateFields(T model, params string[] fields)
        {
            this.Decipher.UpdateModelProps(model, fields);

            return model;
        }

        public virtual LModelList<T> GetModelAll()
        {
            LModelList<T> models = this.Decipher.SelectModels<T>();

            return models;
        }

        public bool ExistByPk(object id)
        {
            return this.Decipher.ExistsModelByPk<T>(id);
        }

        public virtual LModelList<T> GetModelsForFilter(LightModelFilter filter)
        {
            LModelList<T> models = this.Decipher.SelectModels<T>(filter);

            return models;
        }

        /// <summary>
        /// 获取所有行
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual int GetTotalRowsForModels(LightModelFilter filter)
        {
            int n = this.Decipher.SelectCount(filter);

            return n;
        }

        public virtual LModelList<T> GetPageForModels(int maxRows, int pageIndex,LightModelFilter filter)
        {
            int totalRows = GetTotalRowsForModels(filter);
            
            filter.Limit = new Limit(maxRows, pageIndex * maxRows);

            LModelList<T> models = this.Decipher.SelectModels<T>(filter);
            models.ExtendedProperties["TotalRows"] = totalRows;

            models.PagesInfo = new LModelPagesInfo(maxRows, pageIndex * maxRows, totalRows);

            return models;
        }

        /// <summary>
        /// 获取所有行
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual int SelectCount()
        {
            int n = this.Decipher.SelectCount(typeof(T));

            return n;
        }

        public virtual LModelList<T> GetPage(int maxRows, int pageIndex, string tSqlWhere, string tSqlOrder)
        {
            int totalRows = SelectCount();

            LightModelFilter filter = new LightModelFilter(typeof(T));
            filter.TSqlWhere = tSqlWhere;
            filter.TSqlOrderBy = tSqlOrder;

            filter.Limit = new Limit(maxRows, pageIndex * maxRows);

            LModelList<T> models = this.Decipher.SelectModels<T>(filter);
            models.ExtendedProperties["TotalRows"] = totalRows;
            
            return models;
        }

        public virtual T SelectByPk(object pkValue)
        {
            T model = this.Decipher.SelectModelByPk<T>(pkValue);

            return model;
        }

        public virtual LModelList<T> SelectByPkList(IList ids)
        {
            LModelElement modelElem = LightModel.GetLModelElement<T>();

            LModelList<T> models = this.Decipher.SelectModelsIn<T>(modelElem.PrimaryKey, ids);

            return models;
        }


        public virtual int Insert(T model)
        {
            int n = this.Decipher.InsertModel(model);

            return n;
        }

        public virtual int DeleteByPk(object pkValue)
        {
            int n = this.Decipher.DeleteModelByPk<T>(pkValue);

            return n;
        }

        public virtual int DeleteByPkList(IList pkValues)
        {
            LModelElement modelElem = LightModel.GetLModelElement<T>();

            string pkField = modelElem.PrimaryKey;

            int n = this.Decipher.DeleteModelsIn<T>(pkField, pkValues);

            return n;
        }

        public virtual T Update(T model)
        {
            if (model is LightModel)
            {
                LModelElement modelElem = LightModel.GetLModelElement<T>();

                LightModel mm = model as LightModel;
                mm.SetBlemish(modelElem.PrimaryKey, false);
            }
            

            int n = this.Decipher.UpdateModel(model,true);

            return model;
        }

        /// <summary>
        /// 改变行状态
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="pkId"></param>
        /// <param name="srcStatusId"></param>
        /// <param name="targetStatusId"></param>
        /// <returns></returns>
        public virtual int ChangeStatus(string fieldName, IList pkId, int[] srcStatusId, int targetStatusId)
        {
            LModelElement modelElem = LightModel.GetLModelElement<T>();

            string pkField = modelElem.PrimaryKey;

            LightModelFilter filter = new LightModelFilter(typeof(T));
            filter.And(pkField, pkId, Logic.In);
            filter.And(fieldName, srcStatusId, Logic.In);

            int n = this.Decipher.UpdateProps(filter, new object[] { fieldName,targetStatusId });

            return n;
        }

    }
}
