using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWQ.Entity.Filter;
using System.Collections;

namespace EC5.Entity.Expanding.ExpandV1
{
    /// <summary>
    /// 配合业务使用,数据库业务操作类
    /// </summary>
    public partial class BizDecipher
    {
        DbDecipher m_DbDecipher;

        /// <summary>
        /// 数据库操作的公共类
        /// </summary>
        public DbDecipher DbDecipher
        {
            get { return m_DbDecipher; }
            set { m_DbDecipher = value; }
        }

        #region 构造函数

        /// <summary>
        /// (构造函数) 配合业务使用,数据库业务操作类
        /// </summary>
        public BizDecipher()
        {

        }

        /// <summary>
        /// (构造函数) 配合业务使用,数据库业务操作类
        /// </summary>
        /// <param name="decipher">数据库操作的公共类</param>
        public BizDecipher(DbDecipher decipher)
        {
            m_DbDecipher = decipher;
        }

        #endregion



        /// <summary>
        /// 批操作的代码
        /// </summary>
        public string BatchCode { get; set; }


        public SModel GetSModel(LightModelFilter filter)
        {            
            return m_DbDecipher.GetSModel(filter);
        }


        public LModel GetModel(LightModelFilter filter)
        {
            return m_DbDecipher.GetModel(filter);
        }

        /// <summary>
        /// 业务假删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BizDeleteModel(LightModel model)
        {
            if (!model.HasField("ROW_SID"))
            {
                throw new Exception("此实体不存在  ROW_SID 字段, 无法作为业务假删除.");
            }

            if (!model.GetTakeChange())
            {
                model.SetTakeChange(true);
            }

            List<string> changeFs = SetRowDelete(model);

            int count = m_DbDecipher.UpdateModel(model, true) ;

            return (count == 1);


        }

        /// <summary>
        /// 业务假删除
        /// </summary>
        /// <param name="model"></param>
        public void BizDeleteModels(System.Collections.IList models)
        {
            foreach (var m in models)
            {
                if (m is LightModel)
                {
                    LightModel model = (LightModel)m;

                    if (!model.GetTakeChange())
                    {
                        model.SetTakeChange(true);
                    }

                    List<string> changeFs = SetRowDelete(model);
                }
            }


            int count = m_DbDecipher.UpdateModels(models, true);
            
        }

        /// <summary>
        /// 业务假删除
        /// </summary>
        /// <param name="model"></param>
        public void BizInsertModels(IEnumerable models)
        {
            foreach (var m in models)
            {
                if (m is LightModel)
                {
                    LightModel model = (LightModel)m;

                    List<string> changeFs = SetRowInsert(model);
                }
            }


            m_DbDecipher.InsertModels(models);

        }


        private LModelElement GetModelElem(LightModel model)
        {
            if (model is LModel)
            {
                return ((LModel)model).GetModelElement();
            }

            return LightModel.GetLModelElement(model.GetType());
        }


        

        public LModelList<LModel> GetModelList(LightModelFilter filter)
        {
            return m_DbDecipher.GetModelList(filter);
        }

        public void BizUpdateModel(LModel model, bool onlyUpdateFields)
        {
            if (!model.GetTakeChange())
            {
                model.SetTakeChange(true);
            }

            SetRowUpdate(model);

            m_DbDecipher.UpdateModel(model, onlyUpdateFields);
        }

        /// <summary>
        /// 业务更新
        /// </summary>
        /// <param name="models"></param>
        /// <param name="onlyUpdateFields"></param>
        public int BizUpdateModels(System.Collections.IList models, bool onlyUpdateFields)
        {
            foreach (var m in models)
            {
                if (m is LightModel)
                {
                    LightModel model = (LightModel)m;

                    if (!model.GetTakeChange())
                    {
                        model.SetTakeChange(true);
                    }

                    SetRowUpdate(model);
                }
            }


            int count = m_DbDecipher.UpdateModels(models, onlyUpdateFields);

            return count;
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ModelSet BizGetModelSet(ModelTreeFilter filter)
        {
            ModelSet ms = new ModelSet();

            BizFilter rootFilter = new BizFilter(filter.Table);

            foreach (var item in filter.FilterConditions)
            {
                rootFilter.And(item.Name, item.Value, item.Logic);
            }

            


            return ms;
        }
    }
}
