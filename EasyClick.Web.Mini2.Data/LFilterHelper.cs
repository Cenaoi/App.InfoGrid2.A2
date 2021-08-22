using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 实体过滤助手
    /// </summary>
    public static class LFilterHelper
    {

        private static void FullFilter_IRange(LightModelFilter filter , FieldBase field,IRangeControl rangeCon)
        {
            //rangeCon = (IRangeControl)field;

            if (!StringUtil.IsBlank(rangeCon.StartValue))
            {
                DateTime startDate;

                if (DateTime.TryParse(rangeCon.StartValue, out startDate))
                {
                    filter.And(field.DataField, startDate, Logic.GreaterThanOrEqual);
                }
                else
                {
                    decimal value;

                    if (decimal.TryParse(rangeCon.StartValue, out value))
                    {
                        filter.And(field.DataField, value, Logic.GreaterThanOrEqual);
                    }
                }
            }

            if (!StringUtil.IsBlank(rangeCon.EndValue))
            {
                DateTime endDate;

                if (DateTime.TryParse(rangeCon.EndValue, out endDate))
                {
                    endDate = endDate.Date.Add(new TimeSpan(0, 23, 59, 59, 999));

                    filter.And(field.DataField, endDate, Logic.LessThanOrEqual);
                }
                else
                {
                    decimal value;

                    if (decimal.TryParse(rangeCon.EndValue, out value))
                    {
                        filter.And(field.DataField, value, Logic.LessThanOrEqual);
                    }
                }
            }
        }

        private static void FullFilter_Item(LightModelFilter filter, FieldBase field)
        {
            if (StringUtil.IsBlank(field.Value))
            {
                return;
            }

            string[] values = StringUtil.Split(field.Value, " ");

            if (values.Length == 1)
            {
                string key = field.Value;

                Logic logic = ModelConvert.ToLogic(field.DataLogic);

                if(logic == Logic.Like)
                {
                    if (!key.Contains("%"))
                    {
                        key = "%" + key + "%";
                    }
                }

                filter.And(field.DataField, key, logic);
            }
            else
            {

                if (field.DataType == DataType.Number)
                {
                    //多个关键字的情况下，做特殊处理

                    List<decimal> valueList = new List<decimal>();

                    foreach (var v in values)
                    {
                        decimal valueD;

                        if (decimal.TryParse(v, out valueD))
                        {
                            valueList.Add(valueD);
                        }
                    }
                    
                    filter.And(field.DataField, valueList, Logic.In);

                }
                else
                {

                    StringBuilder orSB = new StringBuilder();

                    //多个关键字的情况下，做特殊处理

                    orSB.Append("(");

                    int n = 0;

                    foreach (var v in values)
                    {
                        string key = v.Replace("'", "''");

                        if (n++ > 0) { orSB.Append(" OR "); }

                        if (!key.Contains("%"))
                        {
                            key = "%" + key + "%";
                        }

                        orSB.Append($"({field.DataField} like '{key}')");
                    }
                    orSB.Append(")");

                    if (!StringUtil.IsBlank(filter.TSqlOrderBy))
                    {
                        filter.TSqlOrderBy += " OR ";
                    }

                    filter.TSqlOrderBy += orSB.ToString();
                }
            }
        } 


        /// <summary>
        /// 填充过滤器
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="panel"></param>
        public static void FullFilter(LightModelFilter filter , Panel panel)
        {
            if(filter == null) { throw new ArgumentNullException("filter", "过滤对象不能为空."); }
            if(panel == null) { throw new ArgumentNullException("panel", "版面不能为空"); }
            
            List<FieldBase> cons = FindFieldControls(panel);
            
            foreach (FieldBase field in cons)
            {
                if (field is IRangeControl)
                {
                    FullFilter_IRange(filter, field, (IRangeControl)field);
                }
                else
                {
                    FullFilter_Item(filter, field);
                }
            }
        }

        /// <summary>
        /// 查找字段的控件
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        private static List<FieldBase> FindFieldControls(Panel layout)
        {
            List<FieldBase> cons = new List<FieldBase>();

            foreach (System.Web.UI.Control item in layout.Controls)
            {
                FieldBase field = item as FieldBase;

                if (field == null || StringUtil.IsBlank(field.DataField))
                {
                    continue;
                }

                cons.Add(field);
            }

            return cons;
        }


    }
}
