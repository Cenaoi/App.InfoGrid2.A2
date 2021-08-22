using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2.Data;
using HWQ.Entity.Filter;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using App.InfoGrid2.Model;
using App.InfoGrid2.Bll;
using Newtonsoft.Json.Linq;

namespace EC5.IG2.Core.UI
{
    public class M2ValidateFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TableSet m_TableSet;

        IG2_TABLE m_Table;

        /// <summary>
        /// 绑定数据仓库
        /// </summary>
        /// <param name="store"></param>
        public void BindStore(Store store)
        {
            
            TableSet tSet = TableMgr.GetTableSet(store.Model);

            if (tSet == null)
            {
                return;
            }

            m_TableSet = tSet;
            m_Table = tSet.Table;

            store.Updating += new ObjectCancelEventHandler(store_Updating);

        }

        void store_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model != null)
            {
                ValidModel((Store)sender, model, e.SrcRecord.Id);
            }

        }



        /// <summary>
        /// 获取字段定义
        /// </summary>
        /// <param name="tSet"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private IG2_TABLE_COL GetCol(TableSet tSet, string field)
        {
            foreach (IG2_TABLE_COL col in tSet.Cols)
            {
                if (col.DB_FIELD == field)
                {
                    return col;
                }
            }

            return null;
        }


        /// <summary>
        /// 验证实体的全部字段
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        /// <param name="tableSet"></param>
        public bool ValidModelFieldAll(Store store,LModel model, TableSet tableSet)
        {
            row_style rs = null;

            LModelElement modelElem = model.GetModelElement();

            string errorMessage;

            if(m_TableSet == null)
            {
                if (tableSet != null)
                {
                    m_TableSet = tableSet;
                }
                else
                {
                    m_TableSet = TableMgr.GetTableSet(modelElem.DBTableName);
                }
            }

            if(m_TableSet == null)
            {
                return true;
            }

            m_Table = m_TableSet.Table;

            if (!string.IsNullOrEmpty(m_Table.STYLE_JSON_FIELD))
            {
                string styleJson = model.Get<string>(m_Table.STYLE_JSON_FIELD);

                if (!string.IsNullOrEmpty(styleJson))
                {
                    rs = row_style.ParseJson(styleJson);
                }
            }

            if (rs == null)
            {
                rs = new row_style();
            }


            foreach (IG2_TABLE_COL col in m_TableSet.Cols)
            {

                string field = col.DB_FIELD;

                bool valid = ValidField(col, field, model, out errorMessage);

                col_style colStyle = rs.cols[field];


                if (!valid)
                {
                    if (colStyle == null)
                    {
                        colStyle = new col_style();
                        colStyle.name = field;
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


            string styleField = StringUtil.NoBlank( m_Table.STYLE_JSON_FIELD, "ROW_STYLE_JSON");


            if (rs.cols.Count > 0 && model.HasField(styleField))
            {

                model[styleField] = json;

                DbDecipher decipher = ModelAction.OpenDecipher();

                decipher.UpdateModelProps(model, styleField);
            }

            if (store != null && !StringUtil.IsBlank(styleField))
            {
                store.SetRecordValue(model.GetPk(), styleField, json);
                
            }

            return rs.cols.Count == 0;
        }


        private void ValidModel(Store store, LModel model, string srcRecordId)
        {
            string[] fields = LightModel.GetBlemishPropNames(model);

            row_style rs = null;

            string errorMessage;

            if (!string.IsNullOrEmpty(m_Table.STYLE_JSON_FIELD))
            {
                string styleJson = model.Get<string>(m_Table.STYLE_JSON_FIELD);

                if (!string.IsNullOrEmpty(styleJson))
                {
                    rs = row_style.ParseJson(styleJson);
                }
            }

            if (rs == null)
            {
                rs = new row_style();
            }


            foreach (string field in fields)
            {
                //e.SrcRecord.MarkInvalid(field, "有错误啦");

                //this.store1.MarkInvalid(e.SrcRecord, DataRecord.WARNING, field, "有异常了啊! ");
                IG2_TABLE_COL col = GetCol(m_TableSet, field);

                if (col == null)
                {
                    continue;
                }

                bool valid = ValidField(col, field, model, out errorMessage);

                col_style colStyle = rs.cols[field];


                if (!valid)
                {
                    if (colStyle == null)
                    {
                        colStyle = new col_style();
                        colStyle.name = field;
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
            

            if (!string.IsNullOrEmpty(m_Table.STYLE_JSON_FIELD))
            {
                model[m_Table.STYLE_JSON_FIELD] = json;
            }
            else if(!StringUtil.IsBlank(m_Table.STYLE_JSON_FIELD))
            {
                store.SetRecordValue(srcRecordId, m_Table.STYLE_JSON_FIELD, json);
            }
        }


        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="col"></param>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool ValidField(IG2_TABLE_COL col, string field, LModel model, out string errorMessage)
        {
            object value = model[field];


            if(!StringUtil.IsBlank(col.VALID_CRITERIA))
            {
                //验证条件的格式. BIZ_SID=(0->2,4),(2->4)


            }


            if (col.VALID_REQUIRED )
            {
                if (StringUtil.IsBlank(value))
                {
                    errorMessage = "必填(v)";
                    return false;
                }
            }
            else if( col.IS_BIZ_MANDATORY)
            {
                if (StringUtil.IsBlank(value))
                {
                    errorMessage = "必填项";
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

                if (modelElem.HasField("BIZ_SID"))
                {
                    filter.And("BIZ_SID", 0, Logic.GreaterThan);
                }

                filter.And(idField, id, Logic.Inequality);
                filter.And(field, value);
                filter.Locks.Add(LockType.NoLock);

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
                string metaData = col.VALID_METADATA;

                errorMessage = null;

                try
                {
                    JObject o = JObject.Parse(metaData);

                    bool isValid = ValueField(o, value.ToString(), ref errorMessage);

                    if(!isValid)
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("校验字段错误。", ex);
                }

            }
            else if (col.VALID_TYPE_ID == "PLUG")
            {

            }

            errorMessage = null;
            return true;
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="o">json对象</param>
        public bool ValueField(JObject o, string curValue,ref string message)
        {
            bool isValidate = true;

            foreach (JProperty prop in o.Properties())
            {

                string name = prop.Name;

                if (name == "messages")
                {
                    //o = JObject.Parse(prop.Value.ToString());

                    //foreach (JProperty item in o.Properties())
                    //{
                    //    SetMessageControl(item);
                    //}

                }
                else
                {
                    isValidate = ValidValueForRule(prop, curValue,ref message);
                }

                if (!isValidate)
                {
                    break;
                }
            }

            return isValidate;

        }


        /// <summary>
        /// 设置值文本框
        /// </summary>
        /// <param name="prop">Json属性</param>
        public bool ValidValueForRule(JProperty prop, string curValue, ref string message)
        {
            bool isValid = true;
            string msg = string.Empty;


            string name = prop.Name;

            string maxValue = "";
            string minValue = "";

            //属性值
            string value = prop.Value.ToString();  //prop.Value.Value<string>();

            //这是拿出最大值和最小值
            if (value.IndexOf("[") == 0 && value.IndexOf("]") > 0)
            {
                string newValue = value.Substring(1, value.Length - 2);

                string[] strList = newValue.Split(',');
                minValue = strList[0].Replace("\r\n", "");
                maxValue = strList[1].Replace("\r\n", ""); ;

            }



            //根据节点名称复制给相应的控件
            switch (name)
            {
                case "email": isValid = ValidateUtil.Email(curValue); break;
                case "url": isValid = ValidateUtil.Url(curValue); break;
                case "date": isValid = ValidateUtil.Date(curValue); break;
                case "dateISO": isValid = ValidateUtil.DateISO(curValue); break;
                case "number": isValid = ValidateUtil.Number(curValue); break;
                case "digits": isValid = ValidateUtil.Digits(curValue); break;

                //case "equalTo": this.cbEqualTo.Checked = true; this.tbxEqualToValue.Value = value; break;
                case "maxlength":
                    isValid = ValidateUtil.MaxLength(curValue, StringUtil.ToInt(value));
                    msg = $"输入长度最多是 {StringUtil.ToInt(value)} 的字符串";
                    break;
                case "minlength":
                    isValid = ValidateUtil.MinLength(curValue, StringUtil.ToInt(value));
                    msg = $"输入长度最少是 {StringUtil.ToInt(value)} 的字符串";
                    break;
                case "rangelength":
                    int minValueI = StringUtil.ToInt(minValue);
                    int maxValueI = StringUtil.ToInt(maxValue);

                    isValid = ValidateUtil.RangeLength(curValue, new decimal[] { minValueI, maxValueI }); break;
                case "range": isValid = ValidateUtil.Regex(curValue, value); break;
                case "max":
                    isValid = ValidateUtil.Max(curValue, StringUtil.ToDecimal(value));
                    break;
                case "min":
                    isValid = ValidateUtil.Min(curValue, StringUtil.ToDecimal(value));
                    break;
            }

            message = msg;

            return isValid;
        }




    }
}
