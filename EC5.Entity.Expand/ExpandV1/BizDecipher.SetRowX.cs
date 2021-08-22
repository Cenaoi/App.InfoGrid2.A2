using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWQ.Entity.LightModels;

namespace EC5.Entity.Expanding.ExpandV1
{
    partial class BizDecipher
    {

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


            if (modelElem.TryGetField("ROW_INSERT_BATCH_CODE", out fieldElem))
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

            if (modelElem.TryGetField("ROW_UPDATE_BATCH_CODE", out fieldElem))
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

            if (modelElem.TryGetField("ROW_DELETE_BATCH_CODE", out fieldElem))
            {
                model.SetValue(fieldElem, this.BatchCode);
                changedFields.Add(fieldElem.DBField);
            }

            return changedFields;

        }
    }
}
