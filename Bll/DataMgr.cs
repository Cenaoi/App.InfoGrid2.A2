using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Bll
{
    public static class DataMgr
    {

        public static LModel GetModel(string tableName, int rowId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelElement modelElem = LightModel.GetLModelElement(tableName);

            LightModelFilter filter = new LightModelFilter(tableName);
            filter.AddFilter("ROW_SID >= 0");
            filter.And(modelElem.PrimaryKey, rowId);

            //rowFilter.Fields = new string[] {
            //    modelElem.PrimaryKey,
            //    "BIZ_SID",
            //    "BIZ_FLOW_SID",
            //    "BIZ_FLOW_INST_CODE",
            //    "BIZ_FLOW_DEF_CODE",
            //    "BIZ_FLOW_CUR_NODE_CODE",
            //    "BIZ_FLOW_CUR_NODE_TEXT"
            //};

            LModel row = decipher.GetModel(filter);


            return row;
        }


        public static LModel GetModel(DbDecipher decipher, string tableName, int rowId)
        {
            LModelElement modelElem = LightModel.GetLModelElement(tableName);

            LightModelFilter filter = new LightModelFilter(tableName);
            filter.AddFilter("ROW_SID >= 0");
            filter.And(modelElem.PrimaryKey, rowId);

            //rowFilter.Fields = new string[] {
            //    modelElem.PrimaryKey,
            //    "BIZ_SID",
            //    "BIZ_FLOW_SID",
            //    "BIZ_FLOW_INST_CODE",
            //    "BIZ_FLOW_DEF_CODE",
            //    "BIZ_FLOW_CUR_NODE_CODE",
            //    "BIZ_FLOW_CUR_NODE_TEXT"
            //};

            LModel row = decipher.GetModel(filter);


            return row;
        }

    }
}
