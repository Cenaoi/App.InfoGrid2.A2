using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.BizEntity.Decipher
{
    /// <summary>
    /// 配合业务使用,数据库业务操作类
    /// </summary>
    public class BizDecipher
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



        private LModelElement GetModelElem(LightModel model)
        {
            if (model is LModel)
            {
                return ((LModel)model).GetModelElement();
            }

            return LightModel.GetLModelElement(model.GetType());
        }


        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model"></param>
        public List<string> SetRowInsert(LightModel model)
        {
            List<string> changedFields = new List<string>();    //改变的字段

            LModelElement modelElem = GetModelElem(model);

            LModelFieldElement fieldElem = null;

            DateTime now = DateTime.Now;

            if (modelElem.TryGetField("ROW_SID", out fieldElem))
            {
                model.SetValue(fieldElem, 0);

                changedFields.Add(fieldElem.DBField);
            }

            if (modelElem.TryGetField("ROW_DATE_CREATE", out fieldElem))
            {
                model.SetValue(fieldElem, now);

                changedFields.Add(fieldElem.DBField);
            }

            if (modelElem.TryGetField("ROW_DATE_UPDATE", out fieldElem))
            {
                model.SetValue(fieldElem, now);

                changedFields.Add(fieldElem.DBField);
            }


            if (modelElem.TryGetField("ROW_INSERT_BACTH_CODE", out fieldElem))
            {
                model.SetValue(fieldElem, this.BatchCode);

                changedFields.Add(fieldElem.DBField);
            }

            return changedFields;
        }

        public List<string> SetRowUpdate(LightModel model)
        {
            List<string> changedFields = new List<string>();    //改变的字段

            LModelElement modelElem = GetModelElem(model);

            LModelFieldElement fieldElem = null;

            if (modelElem.TryGetField("ROW_DATE_UPDATE", out fieldElem))
            {
                model.SetValue(fieldElem, DateTime.Now);

                changedFields.Add(fieldElem.DBField);
            }

            if (modelElem.TryGetField("ROW_UPDATE_BACTH_CODE", out fieldElem))
            {
                model.SetValue(fieldElem, this.BatchCode);

                changedFields.Add(fieldElem.DBField);
            }

            return changedFields;
        }

        public List<string> SetRowDelete(LightModel model)
        {
            List<string> changedFields = new List<string>();    //改变的字段

            LModelElement modelElem = GetModelElem(model);

            LModelFieldElement fieldElem = null;

            if (modelElem.TryGetField("ROW_SID", out fieldElem))
            {
                model.SetValue(fieldElem, -3);

                changedFields.Add(fieldElem.DBField);
            }

            if (modelElem.TryGetField("ROW_DATE_DELETE", out fieldElem))
            {
                model.SetValue(fieldElem, DateTime.Now);
                changedFields.Add(fieldElem.DBField);
            }

            if (modelElem.TryGetField("ROW_DELETE_BACTH_CODE", out fieldElem))
            {
                model.SetValue(fieldElem, this.BatchCode);
                changedFields.Add(fieldElem.DBField);
            }

            return changedFields;

        }

    }
}
