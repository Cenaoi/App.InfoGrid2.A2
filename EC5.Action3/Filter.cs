using EC5.Action3.CodeProcessors;
using EC5.Action3.SEngine;
using EC5.Utility;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    public class CSharpFilter: FilterItem
    {
        string m_Code;

        /// <summary>
        /// 解析过滤条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CSharpFilter ParseCSarp(string data)
        {
            CSharpFilter filter = new CSharpFilter();
            filter.m_Code = data;

            return filter;
        }


        public override bool Valid(CodeContext context, object item)
        {
            if (item is SModel)
            {
                return Valid(context,(SModel)item);
            }
            else if (item is LightModel)
            {
                return Valid(context,(LightModel)item);
            }

            return false;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Valid(CodeContext context, SModel model)
        {
            ActionItemBase actItem = context.CurSubItem;

            bool result = true;

            ScriptInstance inst = ScriptFactory.Create(m_Code);

            foreach (var item in context.CurParams.GetFields())
            {
                inst.Params[item] = context.CurParams[item];
            }

            //inst.Params[actItem.Code] = model;

            object result_obj;

            try
            {
                result_obj = inst.Exec(model);
            }
            catch(Exception ex)
            {
                throw new Exception("执行脚本错误: " + m_Code);
            }

            if (false.Equals(result_obj))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Valid(CodeContext context, LightModel model)
        {
            ActionItemBase actItem = context.CurSubItem;

            bool result = true;



            ScriptInstance inst = ScriptFactory.Create(m_Code);

            foreach (var item in context.CurParams.GetFields())
            {
                inst.Params[item] = context.CurParams[item];
            }

            //inst.Params[actItem.Code] = model;

            object result_obj = inst.Exec(model);

            if (false.Equals(result_obj))
            {
                result = false;
            }

            return result;
        }


    }

    /// <summary>
    /// 过滤条件
    /// </summary>
    public class Filter: FilterGroup
    {
        public override bool Valid(CodeContext context, object item)
        {
            if(item is SModel)
            {
                return Valid((SModel)item);
            }
            else if(item is LightModel)
            {
                return Valid((LightModel)item);
            }

            return false;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Valid(SModel model)
        {
            bool result = true;

            result = FilterValidateSModel.ValidGroup(model, this);

            return result;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Valid(LightModel model)
        {
            bool result = true;

            result = FilterValidateLightModel.ValidGroup(model, this);

            return result;
        }


        /// <summary>
        /// 解析过滤条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Filter ParseJson(string data)
        {
            SModelList smList = SModelList.ParseJson(data);


            Filter filter = new Filter();

            ParseSModel(filter, smList);

            return filter;
        }

        public static void ParseSModel(FilterGroup group, SModelList smList)
        {
            if(smList == null || smList.Count == 0)
            {
                return;
            }

            foreach (SModel sm in smList)
            {
                string role = sm["role"]?.ToString();

                if (role == "and" || role == "or")
                {
                    SModelList subSMList = sm["items"] as SModelList;

                    if (subSMList != null && subSMList.Count > 0)
                    {
                        FilterGroup fg = new FilterGroup();

                        group.Items.Add(fg);

                        if (role == "or")
                        {
                            fg.Condition = FilterCondition.Or;
                        }

                        ParseSModel(fg, subSMList);
                    }

                }
                else
                {
                    FilterField ff = new FilterField();
                    ff.Name = (string)sm["field"];
                    ff.Value = sm["value"];
                    ff.Logic = (string)sm["logic"];

                    ff.IsNull = BoolUtil.ToBool(sm["is_null"], false);
                    ff.Mode = EnumUtil.Parse(sm["mode"]?.ToString(), ActionModeType.None);

                    //校验函数格式
                    if(ff.Mode == ActionModeType.Fun )
                    {
                        if(ff.Value == null)
                        {
                            throw new Exception($"函数 '{ff.Name}' 不能为空..");
                        }

                        string code = ff.Value.ToString();

                        if (string.IsNullOrWhiteSpace(code))
                        {
                            ff.Mode = ActionModeType.None;
                        }
                        else
                        {
                            if(!code.Contains("return "))
                            {
                                ff.Value = "return " + ff.Value + ";";
                            }
                        }
                    }

                    group.Items.Add(ff);


                    
                }

               
            }
        }


    }

    static class FilterValidateSModel
    {


        public static bool ValidGroup(SModel model, FilterGroup group)
        {
            bool result;

            if (group.Condition == FilterCondition.And)
            {
                result = ValidGroupAnd(model, group);
            }
            else
            {
                result = ValidGroupOr(model, group);
            }


            return result;
        }


        private static bool ValidGroupAnd(SModel model, FilterGroup group)
        {
            bool result = true;

            foreach (FilterItem item in group.Items)
            {
                if (item is FilterField)
                {
                    bool itemResult = ValidField(model, (FilterField)item);

                    if (!itemResult)
                    {
                        result = false;
                        break;
                    }
                }
                else if (item is FilterGroup)
                {
                    bool itemResult = ValidGroup(model, (FilterGroup)item);

                    if (!itemResult)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private static bool ValidGroupOr(SModel model, FilterGroup group)
        {
            bool result = false;

            foreach (FilterItem item in group.Items)
            {
                if (item is FilterField)
                {
                    bool itemResult = ValidField(model, (FilterField)item);

                    if (itemResult)
                    {
                        result = true;
                        break;
                    }
                }
                else if (item is FilterGroup)
                {
                    bool itemResult = ValidGroup(model, (FilterGroup)item);

                    if (itemResult)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;

        }

        private static bool ValidField(SModel model, FilterField field)
        {
            bool result = false;

            object v = model[field.Name];

            switch (field.Logic)
            {
                case "==":
                    result = v.Equals(field.Value);
                    break;
                case "<>":
                case "!=":
                    result = !v.Equals(field.Value);
                    break;
            }

            return result;

        }


    }

    static class FilterValidateLightModel
    {


        public static bool ValidGroup(LightModel model, FilterGroup group)
        {
            bool result;

            if (group.Condition == FilterCondition.And)
            {
                result = ValidGroupAnd(model, group);
            }
            else
            {
                result = ValidGroupOr(model, group);
            }


            return result;
        }

        private static bool ValidGroupAnd(LightModel model, FilterGroup group)
        {
            bool result = true;

            foreach (FilterItem item in group.Items)
            {
                if (item is FilterField)
                {
                    bool itemResult = ValidField(model, (FilterField)item);

                    if (!itemResult)
                    {
                        result = false;
                        break;
                    }
                }
                else if (item is FilterGroup)
                {
                    bool itemResult = ValidGroup(model, (FilterGroup)item);

                    if (!itemResult)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private static bool ValidGroupOr(LightModel model, FilterGroup group)
        {
            bool result = false;

            foreach (FilterItem item in group.Items)
            {
                if (item is FilterField)
                {
                    bool itemResult = ValidField(model, (FilterField)item);

                    if (itemResult)
                    {
                        result = true;
                        break;
                    }
                }
                else if (item is FilterGroup)
                {
                    bool itemResult = ValidGroup(model, (FilterGroup)item);

                    if (itemResult)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private static bool ValidField(LightModel model, FilterField field)
        {
            bool result = false;

            object v = model[field.Name];

            switch (field.Logic)
            {
                case "=":
                case "==":
                    result = v.Equals(field.Value);
                    break;
                case "<>":
                case "!=":
                    result = !v.Equals(field.Value);
                    break;
            }

            return result;

        }


    }
}
