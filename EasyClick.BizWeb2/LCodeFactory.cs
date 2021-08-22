using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.LCodeEngine;
using EC5.Utility;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 单元格的动态代码
    /// </summary>
    public class LCodeFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 绑定数据仓库
        /// （单元格的动态代码）
        /// </summary>
        /// <param name="store"></param>
        public void BindStore(Store store)
        {

            store.Updating += new ObjectCancelEventHandler(store_Updating);

        }

        void store_Updating(object sender, ObjectCancelEventArgs e)
        {

            LModel model = e.Object as LModel;

            try
            {
                ExecLCode((Store)sender, e.SrcRecord, model);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                e.Cancel = true;
            }

        }

        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string[] ExecLCode(Store store, LModel model)
        {
            return ExecLCode(store, null, model);
        }


        /// <summary>
        /// 执行动态代码
        /// </summary>
        /// <param name="store"></param>
        /// <param name="srcRecord"></param>
        /// <param name="model"></param>
        /// <returns>返回修改过的字段集合</returns>
        public string[] ExecLCode(Store store, DataRecord srcRecord, LModel model)
        {

            LModelElement modelElem = model.GetModelElement();

            string modelName = modelElem.DBTableName;

            LcModel cModel = null;

            if (!LcModelManager.Models.TryGetValue(modelName, out cModel))
            {
                return null;
            }



            //获取第一次弄脏的字段
            string[] blemishFields = LightModel.GetBlemishPropNames(model);

            string[] fieldAll = new string[0];  //全部发生变化的字段



            //当前执行发生变化的字段
            string[] curFields;

            fieldAll = ProExceLCode(model, cModel, fieldAll, blemishFields, out curFields);

            int depth = 0;   //记录循环的次数，如果超出 20次，就证明是循环嵌套

            while (true)
            {
                if (depth++ > 20)
                {
                    log.ErrorFormat("发生死循环规则。{0}",cModel.ToString());

                    break;
                }

                fieldAll = ProExceLCode(model, cModel, fieldAll, curFields, out curFields);

                if (curFields == null || curFields.Length == 0)
                {
                    break;
                }
            }


            if (store != null && srcRecord != null && fieldAll != null && fieldAll.Length > 0)
            {
                string recId = srcRecord.Id;

                //输出
                foreach (string uField in fieldAll)
                {
                    if (model.IsNull(uField))
                    {
                        store.SetRecordValue(recId, uField,string.Empty);
                    }
                    else
                    {
                        object uValue = model[uField];
                        store.SetRecordValue(recId, uField, uValue.ToString());
                    }
                }
            }

            return fieldAll;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cModel">规则</param>
        /// <param name="blemishFields">变化的字段</param>
        /// <returns></returns>
        private string[] ProExceLCode(LModel model, LcModel cModel,string[] fieldAll, string[] blemishFields,out string[] curFields)
        {
            string[] fields = new string[0];
            

            foreach (string bF in blemishFields)
            {
                if (!cModel.IsListen(bF))
                {
                    continue;
                }

                string[] updateFs = null;

                //执行 LightCode 代码
                cModel.Exec(bF, model, out updateFs);

                fields = ArrayUtil.Union(fields, updateFs);

            }

            fieldAll = ArrayUtil.Union(fieldAll, fields);

            curFields = fields;

            return fieldAll;
        }

    }
}