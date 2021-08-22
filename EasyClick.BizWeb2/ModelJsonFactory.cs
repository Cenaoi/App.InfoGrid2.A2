using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini;
using HWQ.Entity;
using EC5.Utility;

namespace EasyClick.BizWeb2
{
    public class ModelJsonFactory: EasyClick.Web.Mini.IMiniJsonFactory
    {
        public virtual string GetItemJson(object item)
        {
            return GetItemJson(item, null, null);
        }


        public virtual string GetItemJson(object item, string[] fields)
        {
            if (item is LightModel)
            {
                return GetJsonForModel((LightModel)item, fields);
            }
            else
            {
                return MiniHelper.GetItemJson(item, null, fields);
            }
        }

        string GetJsonData(object v)
        {
            if (v == null)
            {
                return string.Empty;
            }

            if (typeof(bool) == v.GetType())
            {
                return v.ToString().ToLower();
            }

            string vStr = v.ToString();

            if (vStr.Length == 0)
            {
                return string.Empty;
            }

            string jsonStr = ToJson(vStr);            

            return jsonStr;
        }


        string ToJson(string s)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {

                char c = s[i];

                switch (c)
                {

                    case '\"': sb.Append("\\\""); break;

                    case '\\': sb.Append("\\\\"); break;

                    case '/': sb.Append("\\/"); break;

                    case '\b': sb.Append("\\b"); break;

                    case '\f': sb.Append("\\f"); break;

                    case '\n': sb.Append("\\n"); break;

                    case '\r': sb.Append("\\r"); break;

                    case '\t': sb.Append("\\t"); break;

                    default: sb.Append(c); break;

                }

            }

            return sb.ToString();

        }


        string GetJsonForModel(LightModel item, string[] fields)
        {
            LModelElement modelElem = ModelElemHelper.GetElem(item);

            if (fields == null || fields.Length == 0)
            {
                fields = modelElem.Fields.ToStringArray();
            }

            string field;

            LModelFieldElement fieldElem;


            StringBuilder sb = new StringBuilder();
            sb.Append("{");



            field = fields[0];

            LModelFieldElementCollection fieldElems = modelElem.Fields;

            for (int i = 0; i < fields.Length; i++)
            {
                field = fields[i];

                if (i > 0)
                {
                    sb.Append(",");
                }


                if (!fieldElems.TryGetField(field,out fieldElem))
                {
                    sb.AppendFormat("\"{0}\": {1}", field, "null");
                    continue;
                }
                
                //object v = LightModel.GetFieldValue(item, fieldElem);

                string vStr = GetJsonPropValue(item, fieldElem, null);

                sb.AppendFormat("\"{0}\":{1}", fieldElem.DBField, vStr);
                


            }

            sb.Append("}");

            return sb.ToString();
        }





        public string GetItemJson(object item, DataFormatCollection dataFormats, string[] fields)
        {
            if (item == null)
            {
                return "null";
            }

            if (item is SModel)
            {
                return ((SModel)item).ToJson();
            }
            else if (!(item is LModel) && AttrUtil.GetAttr<DBTableAttribute>(item) != null)
            {
                return MiniHelper.GetItemJson(item, dataFormats, fields);

            }
            else if (item is LightModel)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("{");

                LModelElement modelElem = ModelElemHelper.GetElem(item);

                if (fields == null || fields.Length == 0)
                {
                    fields = modelElem.Fields.ToStringArray();
                }

                string field;

                LModelFieldElement fieldElem;


                for (int i = 0; i < fields.Length; i++)
                {
                    field = fields[i];

                    if (i > 0)
                    {
                        sb.Append(",");
                    }

                    if (!modelElem.TryGetField(field, out fieldElem))
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\"", field, "");
                        continue;
                    }

                    string valueStr = GetJsonPropValue(item, fieldElem, dataFormats);

                    sb.AppendFormat("\"{0}\":{1}", fieldElem.DBField, valueStr);
                }

                sb.Append("}");

                return sb.ToString();
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(item);
            }
        }


        private string GetJsonPropValue(object item, LModelFieldElement fieldElem, DataFormatCollection dataFormats)
        {
            string valueStr = null;

            string formatStr = null;

            if (dataFormats != null)
            {
                formatStr = dataFormats.GetFormatString(fieldElem.DBField);
            }

            if (string.IsNullOrEmpty( formatStr) && fieldElem.UI != null && !string.IsNullOrEmpty(fieldElem.UI.format))
            {
                formatStr = fieldElem.UI.format;
            }


            object v = LightModel.GetFieldValue(item, fieldElem.DBField);

            if (!string.IsNullOrEmpty(formatStr))
            {
                string vStr = string.Format(formatStr, v);
                valueStr = string.Format("\"{0}\"", GetJsonData(vStr));
            }
            else
            {
                if(v == null)
                {
                    valueStr = "null";
                }
                else if (ModelHelper.IsNumberType(fieldElem.DBType))
                {
                    if (string.Empty.Equals(v))
                    {
                        valueStr = "null";
                    }
                    else
                    {
                        valueStr = ParseNum(v).ToString();
                    }
                    
                }
                else if (fieldElem.DBType == LMFieldDBTypes.Boolean)
                {
                    if(v.GetType() != typeof(bool))
                    {
                        v = Convert.ChangeType(v, typeof(bool));
                    }

                    valueStr = ((bool)v) ? "true" : "false";
                }
                else if (ModelHelper.IsDataTimeType(fieldElem.DBType))
                {
                    valueStr = string.Format("\"{0:yyyy-MM-dd HH:mm:ss.fff}\"", v);
                }
                else
                {
                    string vStr = ModelHelper.ValueFormat(v, fieldElem);

                    valueStr = string.Format("\"{0}\"", GetJsonData(vStr));
                }

            }




            //if (fieldElem.UI != null && !string.IsNullOrEmpty(fieldElem.UI.format))
            //{
            //    object v = LightModel.GetFieldValue(item, fieldElem.DBField);

            //    string formatStr = dataFormats.GetFormatString(fieldElem.DBField);

            //    if (string.IsNullOrEmpty(formatStr))
            //    {
            //        string vStr = ModelHelper.ValueFormat(v, fieldElem);

            //        valueStr = string.Format("\"{0}\"", GetJsonData(vStr));
            //    }
            //    else
            //    {
            //        string vStr = string.Format(formatStr, v);
            //        valueStr = string.Format("\"{0}\"", GetJsonData(vStr));
            //    }
            //}
            //else
            //{
            //    object v = LightModel.GetFieldValue(item, fieldElem.DBField);

            //    string formatStr = dataFormats.GetFormatString(fieldElem.DBField);

            //    if (v == null)
            //    {

            //        v = string.Empty;
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(formatStr))
            //        {
            //            if (ModelHelper.IsDataTimeType(fieldElem.DBType))
            //            {
            //                v = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", v);
            //            }
            //            else if(fieldElem.DBType == LMFieldDBTypes.Boolean)
            //            {
            //                v = ((bool)v) ? "true" : "false";
            //            }
            //            else
            //            {
            //                v = v.ToString();
            //            }
            //        }
            //        else
            //        {
            //            v = string.Format(formatStr, v);
            //        }
            //    }
            //    else{
            //        valueStr = string.Format("\"{0}\"", GetJsonData(v));
            //    }


            //    //if (string.IsNullOrEmpty(formatStr) && fieldElem.IsNumber)
            //    //{

            //    //}
            //    //else
            //    //{
                   
            //    //}


            //}

            return valueStr;

        }


        /// <summary>
        /// 去除 Decimal 对象后面的0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ParseNum(object value)
        {
            string v = value.ToString();
            int n = v.LastIndexOf('.');

            //如果没有小数，就不处理
            if (n == -1) return value;

            v = v.TrimEnd('0');
            n = v.Length - 1;

            if (v[n] == '.') v = v.Remove(n);

            return v;
        }


    }
}
