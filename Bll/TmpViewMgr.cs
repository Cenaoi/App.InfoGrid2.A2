using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.Utility;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.Bll
{
    public static class TmpViewMgr
    {




        public static List<string> GetFieldNames(Guid uid,string sessionId)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<string> localFs = new List<string>();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TMP_VIEW_FIELD));
            filter.And("TMP_GUID", uid);
            filter.And("TMP_SESSION_ID", sessionId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "TABLE_NAME", "FIELD_NAME", "ALIAS" };


            List<LModel> models = decipher.GetModelList(filter);

            foreach (LModel model in models)
            {
                string table = model.Get<string>("TABLE_NAME");
                string field = model.Get<string>("FIELD_NAME");
                string alias = model.Get<string>("ALIAS");

                if (!StringUtil.IsBlank(alias))
                {
                    localFs.Add(alias);
                }
                else
                {
                    localFs.Add(table + "_" + field);
                }
            }

            return localFs;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="localFs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetExprName(List<string> localFs, string name)
        {
            if (!localFs.Contains(name))
            {
                return name;
            }

            string exName = string.Empty;

            for (int i = 0; i < 999; i++)
            {
                exName = "Expr" + i;

                if (!localFs.Contains(exName))
                {
                    break;
                }
            }

            return exName;

        }


    }
}
