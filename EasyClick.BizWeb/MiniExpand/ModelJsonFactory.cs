using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini;
using HWQ.Entity;
using EC5.SystemBoard.Web;
using EC5.Utility;

namespace EasyClick.BizWeb.MiniExpand
{
    /// <summary>
    /// 实体转换为 Json 的处理工厂
    /// </summary>
    public class ModelJsonFactory : EasyClick.Web.Mini.IMiniJsonFactory
    {
        public virtual string GetItemJson(object item)
        {
            return GetItemJson(item,null, null);
        }


        public virtual string GetItemJson(object item, string[] fields)
        {
            if (item is LightModel)
            {
                return GetJsonForModel((LightModel)item, fields);
            }
            else
            {
                return MiniHelper.GetItemJson(item,null, fields);
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

            string vStr= v.ToString();

            if (vStr.Length == 0)
            {
                return string.Empty;
            }

            string jsonStr = vStr.Replace("\"", "\\\"")
                .Replace("\n", @"\n")
                .Replace("\r", @"\r")
                .Replace("\t", @"\t");

            return jsonStr;
        }

        string GetJsonForModel(LightModel item, string[] fields)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            LModelElement modelElem = ModelElemHelper.GetElem(item) ;

            if (fields == null || fields.Length == 0)
            {
                fields = modelElem.Fields.ToStringArray();
            }

            string field ;

            LModelFieldElement fieldElem;

            field = fields[0];



            for (int i = 0; i < fields.Length; i++)
            {
                field = fields[i];

                if (i > 0)
                {
                    sb.AppendLine(",");
                }

                if (!modelElem.Fields.ContainsField(field))
                {
                    sb.AppendFormat("\"{0}\":\"{1}\"", field, field);
                    continue;
                }

                fieldElem = modelElem.Fields[field];

                if (fieldElem.UI != null && !string.IsNullOrEmpty(fieldElem.UI.format))
                {
                    object v = LightModel.GetFieldValue(item, fieldElem);

                    string vStr = ModelHelper.ValueFormat(v, fieldElem);

                    sb.AppendFormat("\"{0}\":\"{1}\"", fieldElem.DBField, GetJsonData(vStr));
                }
                else
                {
                    object v = LightModel.GetFieldValue(item, fieldElem);

                    if (v == null)
                    {
                        v = string.Empty;
                    }
                    else
                    {
                        if (v.GetType() == typeof(DateTime))
                        {
                            v = string.Format("{0:yyyy-MM-dd}", v);
                        }
                        else
                        {
                            v = v.ToString();
                        }
                    }

                    if (ModelHelper.IsNumberType(fieldElem.DBType))
                    {
                        if (v == null || (v.GetType() == typeof(string) && String.IsNullOrEmpty((string)v)))
                        {
                            v = "\"\"";
                        }
                        else
                        {
                            v = v.ToString();
                        }


                        sb.AppendFormat("\"{0}\":{1}", fieldElem.DBField, v);
                    }
                    else
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\"", fieldElem.DBField, GetJsonData(v));
                    }


                    
                }
            }

            sb.Append("}");

            return sb.ToString();
        }


        public string GetItemJson(object item, DataFormatCollection dataFormats, string[] fields)
        {
            //if (!(item is LModel) && AttrUtil.GetAttr<DBTableAttribute>(item) != null)
            //{
            //    return MiniHelper.GetItemJson(item, dataFormats, fields);

            //}



            if (dataFormats == null)
            {
                dataFormats = new DataFormatCollection();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            LModelElement modelElem = ModelElemHelper.GetElem(item) ;

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
                    sb.AppendLine(",");
                }

                if (!modelElem.Fields.ContainsField(field))
                {
                    sb.AppendFormat("\"{0}\":\"{1}\"", field, field);
                    continue;
                }

                fieldElem = modelElem.Fields[field];

                if (fieldElem.UI != null && !string.IsNullOrEmpty(fieldElem.UI.format))
                {
                    object v = LightModel.GetFieldValue(item, fieldElem.DBField);

                    string formatStr = dataFormats.GetFormatString(fieldElem.DBField);

                    if (string.IsNullOrEmpty(formatStr))
                    {
                        string vStr = ModelHelper.ValueFormat(v, fieldElem);

                        sb.AppendFormat("\"{0}\":\"{1}\"", fieldElem.DBField, GetJsonData(vStr));
                    }
                    else
                    {
                        string vStr = string.Format(formatStr, v);
                        sb.AppendFormat("\"{0}\":\"{1}\"", fieldElem.DBField, GetJsonData(vStr));
                    }
                }
                else
                {
                    object v = LightModel.GetFieldValue(item, fieldElem.DBField);

                    string formatStr = dataFormats.GetFormatString(fieldElem.DBField);

                    if (v == null)
                    {

                        v = string.Empty;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(formatStr))
                        {
                            if (ModelHelper.IsDataTimeType(fieldElem.DBType))
                            {
                                v = string.Format("{0:yyyy-MM-dd}", v);
                            }
                            else
                            {
                                v = v.ToString();
                            }
                        }
                        else
                        {
                            v = string.Format(formatStr, v);
                        }
                    }

                    if (string.IsNullOrEmpty(formatStr) && ModelHelper.IsNumberType(fieldElem.DBType))
                    {
                        if (v == null || (v.GetType() == typeof(string) && String.IsNullOrEmpty((string)v)))
                        {
                            v = "\"\"";
                        }
                        else
                        {
                            v = v.ToString();
                        }

                        sb.AppendFormat("\"{0}\":{1}", fieldElem.DBField, v);
                    }
                    else
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\"", fieldElem.DBField, GetJsonData(v));
                    }
                }
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
