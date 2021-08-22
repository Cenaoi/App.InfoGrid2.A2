using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2.Data;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.Filter;
using HWQ.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EC5.Utility;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model;
using EasyClick.BizWeb2;
using EC5.IG2.BizBase;

namespace EC5.IG2.Plugin.Custom
{
    /// <summary>
    /// 单据提交(插件)
    /// </summary>
    public class BillPlugin : PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// 提交单据.
        /// </summary>
        /// <param name="storeUi"></param>
        /// <param name="tableUi"></param>
        /// <param name="billCodeField">提单的代码字段</param>
        /// <param name="billCode">单据代码</param>
        public void Submit()
        {
            Store storeUi = this.MainStore;
            Table tableUi = this.MainTable;
            string plugParam = this.Params;


            JObject ppm;

            string billCode;
            string bill_no_Field;


            try
            {
                ppm = (JObject)JsonConvert.DeserializeObject(plugParam);
                
                billCode = ppm.Value<string>("bill_code");

                bill_no_Field = ppm.Value<string>("bill_no_field");
            }
            catch (Exception ex)
            {
                log.Error("解析 json 命令参数出错。" + plugParam , ex);
                MessageBox.Alert("解析命令参数出错.");
                return;
            }

            string tableName = storeUi.Model;

            LModelElement modelElem = LightModel.GetLModelElement(tableName);

            DataRecordCollection recds = tableUi.CheckedRows;

            if (recds.Count == 0)
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            foreach (DataRecord recd in recds)
            {
                LModelFieldElement fieldElem = modelElem.Fields[modelElem.PrimaryKey];

                object pkValue = ModelConvert.ChangeType(recd.Id, fieldElem);

                LightModelFilter filter = new LightModelFilter(tableName);
                filter.And(modelElem.PrimaryKey, pkValue);
                //filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                LModel model = decipher.GetModel(filter);
                model.SetTakeChange(true);

                if (model == null)
                {
                    continue;
                }


                bool valid = ValidModel( storeUi,model, recd,null, new string[]{bill_no_Field}); //验证数据

                if (!valid)
                {

                    continue;
                }

                int rowSid = model.Get<int>("ROW_SID");

                if (rowSid > 0)
                {
                    continue;
                }


                string billNo = model.Get<string>(bill_no_Field);

                if (StringUtil.IsBlank(billNo))
                {
                    billNo = BillIdentityMgr.NewCodeForMonth("BILL_" + tableName, billCode);

                    model[bill_no_Field] = billNo;
                }

                model["ROW_SID"] = 2;


                DbCascadeRule.Update(storeUi, model);

                storeUi.SetRecordValue(recd.Id, "ROW_SID", 2);
                storeUi.SetRecordValue(recd.Id, bill_no_Field, model[bill_no_Field]);
                
            }

        }



        /// <summary>
        /// 验证实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ValidModel(Store storeUi, LModel model, DataRecord recd,string[] fields, string[] expFields)
        {
            LModelElement modelElem = model.GetModelElement();

            string tableName = modelElem.DBTableName;

            TableSet tSet = TableMgr.GetTableSet(tableName);
            IG2_TABLE table = tSet.Table;

            string errorMessage;

            bool validModel = true;



            row_style rs = null;

            if (!string.IsNullOrEmpty(table.STYLE_JSON_FIELD))
            {
                string styleJson = model.Get<string>(table.STYLE_JSON_FIELD);

                if (!string.IsNullOrEmpty(styleJson))
                {
                    rs = row_style.ParseJson(styleJson);
                }
            }

            if (rs == null)
            {
                rs = new row_style();
            }

            foreach (IG2_TABLE_COL tCol in tSet.Cols)
            {
                col_style colStyle = rs.cols[tCol.DB_FIELD];

                if ( ArrayUtil.Exist(expFields, tCol.DB_FIELD))
                {
                    if (colStyle != null)
                    {
                        colStyle.msgs.Clear();
                    }

                    continue;
                }

                bool valid = ValidField(tCol, tCol.DB_FIELD, model, out errorMessage);



                if (!valid)
                {
                    validModel = false;
                    if (colStyle == null)
                    {
                        colStyle = new col_style();
                        colStyle.name = tCol.DB_FIELD;
                        rs.cols.Add(colStyle);
                    }

                    colStyle.msgs.Clear();
                    colStyle.msgs.Add(new col_msg(DataRecord.ERROR, errorMessage));
                }
                else if (colStyle != null)
                {
                    colStyle.msgs.Clear();
                }


            }


            string json = rs.ToJsonString();

            if (!string.IsNullOrEmpty(table.STYLE_JSON_FIELD))
            {

                storeUi.SetRecordValue(recd.Id, table.STYLE_JSON_FIELD, json);
                model[table.STYLE_JSON_FIELD] = json;
            }
            else
            {
                storeUi.SetRecordValue(recd.Id, table.STYLE_JSON_FIELD, json);
            }

            return validModel;
        }


        private bool ValidField(IG2_TABLE_COL col, string field, LModel model, out string errorMessage)
        {
            object value = model[field];

            if (col.VALID_REQUIRED)
            {
                if (value == null || StringUtil.IsBlank(value.ToString()))
                {
                    errorMessage = "必填";
                    return false;
                }
            }

            if (col.VALID_UNIQUE)
            {
                LModelElement modelElem = model.GetModelElement();

                string idField = modelElem.PrimaryKey;

                object id = model[idField];

                LightModelFilter filter = new LightModelFilter(modelElem.DBTableName);
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And(idField, id, Logic.Inequality);
                filter.And(field, value);

                DbDecipher decipher = ModelAction.OpenDecipher();

                bool exist = decipher.ExistsModels(filter);

                if (exist)
                {
                    errorMessage = "出现重复";
                    return false;
                }

            }


            if (col.VALID_TYPE_ID == "METADATA")
            {

            }
            else if (col.VALID_TYPE_ID == "PLUG")
            {

            }

            errorMessage = null;
            return true;
        }
        

    }
}