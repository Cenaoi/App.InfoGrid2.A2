using EasyClick.Web.Mini2;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// Json 过滤助手类（临时）
    /// </summary>
    public static class JFilterHelper
    {


        /// <summary>
        /// 获取 T-SQL 的 Where 子语句
        /// </summary>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public static List<Param> GetParmas(string filter2)
        {
            List<Param> ps = new List<Param>();

            if (StringUtil.IsBlank(filter2))
            {
                return ps;
            }


            List<JFilterItem> jFilterItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JFilterItem>>(filter2);

            foreach (var jItem in jFilterItems)
            {
                if (jItem.P_Type == "TSQL_WHERE")
                {
                    EasyClick.Web.Mini2.TSqlWhereParam sqlWhereParam = new EasyClick.Web.Mini2.TSqlWhereParam(jItem.Value);

                    ps.Add(sqlWhereParam);
                }
                else
                {
                    if (StringUtil.IsBlank(jItem.Field))
                    {
                        throw new Exception("二次筛选的参数字段名不能为空。");
                    }

                    Param p = new Param(jItem.Field, jItem.Value);
                    p.Logic = jItem.Logic;

                    ps.Add(p);
                }

            }

            return ps;
        }



        /// <summary>
        /// 获取 Json 对象
        /// </summary>
        /// <param name="formLayout"></param>
        /// <param name="excludeFields">排除字段集合</param>
        /// <returns></returns>
        public static string GetJson(FormLayout formLayout)
        {

            List<JFilterItem> jFilters = GetJFilterItems(formLayout, null);

            string filter2 = Newtonsoft.Json.JsonConvert.SerializeObject(jFilters);

            return filter2;
        }


        /// <summary>
        /// 获取 Json 对象
        /// </summary>
        /// <param name="formLayout"></param>
        /// <param name="excludeFields">排除字段集合</param>
        /// <returns></returns>
        public static string GetJson(FormLayout formLayout, string[] excludeFields)
        {

            List<JFilterItem> jFilters = GetJFilterItems(formLayout,excludeFields);

            string filter2 = Newtonsoft.Json.JsonConvert.SerializeObject(jFilters);

            return filter2;
        }


        public static List<JFilterItem> GetJFilterItems(FormLayout formLayout)
        {
            return GetJFilterItems(formLayout, null);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formLayout"></param>
        /// <param name="excludeFields">排除字段集合</param>
        /// <returns></returns>
        public static List<JFilterItem> GetJFilterItems(FormLayout formLayout, string[] excludeFields)
        {

            List<FieldBase> cons = FindFieldControls(formLayout);

            object value;

            List<JFilterItem> jFilterItems = new List<JFilterItem>();

            foreach (FieldBase item in cons)
            {

                if(excludeFields != null && ArrayUtil.Exist(excludeFields, item.DataField))
                {
                    continue;
                }


                if (StringUtil.IsBlank(item.DataLogic))
                {
                    item.DataLogic = "=";
                }

                if (item is IChecked)
                {
                    value = ((IChecked)item).Checked;

                    if (false.Equals(value))
                    {
                        continue;
                    }

                    JFilterItem jItem = new JFilterItem();
                    jItem.Field = item.DataField;
                    jItem.Value = value.ToString().ToLower();

                    jFilterItems.Add(jItem);
                }
                else if (item is IRangeControl)
                {
                    IRangeControl rangeCon = (IRangeControl)item;

                    if (!StringUtil.IsBlank(rangeCon.StartValue))
                    {
                        JFilterItem jItem = new JFilterItem();
                        jItem.Field = item.DataField;
                        jItem.Logic = ">=";
                        jItem.Value = rangeCon.StartValue;

                        jFilterItems.Add(jItem);
                    }


                    if (!StringUtil.IsBlank(rangeCon.EndValue))
                    {
                        JFilterItem jItem = new JFilterItem();
                        jItem.Field = item.DataField;
                        jItem.Logic = "<=";
                        jItem.Value = rangeCon.EndValue;

                        jFilterItems.Add(jItem);
                    }

                }
                else
                {
                    if (!StringUtil.IsBlank(item.Value))
                    {
                        JFilterItem jItem = new JFilterItem();
                        jItem.Field = item.DataField;
                        jItem.Logic = item.DataLogic;
                        jItem.Value = item.Value;

                        jFilterItems.Add(jItem);
                    }


                }
            }

            return jFilterItems;

        }


        /// <summary>
        /// 查找字段的控件
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        private static List<FieldBase> FindFieldControls(FormLayout layout)
        {
            if (layout == null) { throw new ArgumentNullException("layout"); }

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
