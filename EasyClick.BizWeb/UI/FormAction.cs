using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;
using EC5.Utility;
using EasyClick.Web.Mini;
using HWQ.Entity.LightModels;
using HWQ.Entity;

namespace EasyClick.BizWeb.UI
{
    /// <summary>
    /// 表单对象
    /// </summary>
    public static class FormAction
    {
        /// <summary>
        /// 查找 DBField 属性的控件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="group"></param>
        /// <param name="cons"></param>
        private static void FindDBCotrols(Control parent, string group, List<Control> cons)
        {
            if (!parent.HasControls())
            {
                return;
            }

            string groupStr;
            string dbField;

            foreach (Control con in parent.Controls)
            {
                IAttributeAccessor attr = con as IAttributeAccessor;

                if (attr != null)
                {
                    groupStr = attr.GetAttribute("Groups");
                    dbField = attr.GetAttribute("DBField");

                    if (!string.IsNullOrEmpty(group))
                    {
                        if (!string.IsNullOrEmpty(groupStr))
                        {
                            string[] groups = StringUtil.Split(groupStr);

                            if (Array.IndexOf<string>(groups, group) > -1)
                            {
                                cons.Add(con);
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(dbField))
                    {
                        cons.Add(con);
                    }
                }

                FindDBCotrols(con, group, cons);
            }
        }

        /// <summary>
        /// 判断集合中是否存在字段
        /// </summary>
        /// <param name="propList">属性集合</param>
        /// <param name="dbField">字段名称</param>
        /// <returns></returns>
        private static bool ExistPropName(PropertyInfo[] propList, string dbField)
        {
            foreach (PropertyInfo item in propList)
            {
                if (item.Name.Equals(dbField, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ExistPropName(PropertyDescriptorCollection propList, string dbField)
        {
            string name;

            foreach (PropertyDescriptor item in propList)
            {
                name = item.Name;

                int n = name.IndexOf('.');

                if (n > -1)
                {
                    name = name.Substring(n + 1);

                }

                if (name.Equals(dbField, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

            }

            return false;
        }

        private static bool TryGetProp(PropertyDescriptorCollection propList, string dbField,out PropertyDescriptor prop)
        {
            string name;

            foreach (PropertyDescriptor item in propList)
            {
                name = item.Name;

                int n = name.IndexOf('.');

                if (n > -1)
                {
                    name = name.Substring(n + 1);

                }

                if (name.Equals(dbField, StringComparison.OrdinalIgnoreCase))
                {
                    prop = item;
                    return true;
                }

            }

            prop = null;
            return false;
        }

        /// <summary>
        /// 按属性名称从属性列表中查找
        /// </summary>
        /// <param name="propList">属性列表</param>
        /// <param name="propName">要查找的属性名</param>
        /// <returns></returns>
        private static PropertyInfo GetProp(PropertyInfo[] propList, string propName)
        {
            foreach (PropertyInfo item in propList)
            {
                if (item.Name.Equals(propName, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }


        /// <summary>
        /// 编辑实体对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="groupName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object EditModel(Control parent,string groupName, object model)
        {
            if (model == null)
            {
                return model;
            }

            LModelElement modelElem = ModelElemHelper.GetElem(model);


            List<Control> controlList = new List<Control>();
            FindDBCotrols(parent, groupName, controlList);

            LModelFieldElementCollection fieldElems = modelElem.Fields;
            LModelFieldElement fieldElem;

            foreach (Control con in controlList)
            {
                if ((con is IAttributeAccessor) == false || !con.EnableViewState)
                {
                    continue;
                }

                IAttributeAccessor attrs = (IAttributeAccessor)con;

                string dbField = attrs.GetAttribute("DBField");
                string group = attrs.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !fieldElems.ContainsField(dbField))
                {
                    continue;
                }

                //if (!string.IsNullOrEmpty(group) && !m_GroupName.Equals(group, StringComparison.OrdinalIgnoreCase))
                //{
                //    continue;
                //}

                fieldElem = fieldElems[dbField];

                object conValue = MiniHelper.GetValue(con);

                conValue = ModelConvert.ChangeType(conValue, fieldElem.DBType,fieldElem.Mandatory);

                if (fieldElem.DBType == LMFieldDBTypes.String)
                {
                    if (conValue != null)
                    {
                        conValue = ((string)conValue).Trim();
                    }
                }

                LightModel.SetFieldValue(model, dbField, conValue);
            }

            return model;
        }


        /// <summary>
        /// 编辑实体对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object EditModel(Control parent, object model)
        {
            return EditModel(parent, string.Empty, model);
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="ModelT"></typeparam>
        /// <param name="parent"></param>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        public static ModelT GetModel<ModelT>(Control parent, string groupName) where ModelT : class
        {
            return (ModelT)GetModel(parent, groupName, typeof(ModelT));
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="ModelT"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static ModelT GetModel<ModelT>(Control parent) where ModelT : class
        {
            return (ModelT)GetModel(parent, string.Empty, typeof(ModelT));
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="groupName">组名</param>
        /// <param name="modelT">实体类型</param>
        /// <returns>实体对象</returns>
        public static object GetModel(Control parent, string groupName, Type modelT)
        {
            object m = Activator.CreateInstance(modelT);

            if (m is LightModel)
            {
                ((LightModel)m).SetTakeChange(true);
            }

            LModelElement modelElem = LightModel.GetLModelElement(modelT);


            LModelFieldElementCollection fieldElems = modelElem.Fields;
            LModelFieldElement fieldElem = null;

            List<Control> controlList = new List<Control>();

            FindDBCotrols(parent, groupName, controlList);

            foreach (Control con in controlList)
            {
                IAttributeAccessor attrs = (IAttributeAccessor)con;

                if (attrs == null || !con.EnableViewState)
                {
                    continue;
                }

                string dbField = attrs.GetAttribute("DBField");
                string group = attrs.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !fieldElems.ContainsField(dbField))
                {
                    continue;
                }

                fieldElem = fieldElems[dbField];

                object conValue = MiniHelper.GetValue(con);
                conValue = ModelConvert.ChangeType(conValue, fieldElem.DBType,fieldElem.Mandatory);

                if (fieldElem.DBType == LMFieldDBTypes.String && conValue != null)
                {
                    conValue = ((string)conValue).Trim();
                }

                LightModel.SetFieldValue(m, dbField, conValue);

                if (m is LightModel)
                {
                    ((LightModel)m).SetBlemish(dbField, true);
                }
            }

            return m;
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="modelT">实体类型</param>
        /// <returns>实体对象</returns>
        public static object GetModel(Control parent, Type modelT)
        {
            return GetModel(parent, string.Empty, modelT);
        }




        /// <summary>
        /// 设置实体对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="groupName">组名称</param>
        /// <param name="model">实体对象</param>
        public static void SetModel(Control parent, string groupName, object model)
        {

            List<Control> controlList = new List<Control>();
            FindDBCotrols(parent, groupName, controlList);

            Type modelT = model.GetType();

            PropertyInfo[] propList = modelT.GetProperties();
           

            PropertyInfo prop;

            foreach (Control con in controlList)
            {
                if ((con is IAttributeAccessor) == false)
                {
                    continue;
                }

                IAttributeAccessor attr = (IAttributeAccessor)con;

                string dbField = attr.GetAttribute("DBField");
                string group = attr.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !ExistPropName(propList, dbField))
                {
                    continue;
                }


                prop = GetProp(propList, dbField);

                object value = prop.GetValue(model, null);


                Type conT = con.GetType();

                DefaultPropertyAttribute dProp =  AttrUtil.GetAttr<DefaultPropertyAttribute>(conT);

                if (dProp == null) { continue; }

                PropertyInfo pi = conT.GetProperty(dProp.Name);


                if (value != null && con is IValueFormat)
                {
                    string valueFormat = ((IValueFormat)con).ValueFormat;

                    if (!string.IsNullOrEmpty(valueFormat))
                    {
                        value = string.Format(valueFormat, value);
                    }
                }


                if (value != null && pi.PropertyType != value.GetType())
                {
                    value = Convert.ChangeType(value, pi.PropertyType);
                }


                pi.SetValue(con, value, null);
            }


        }



        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="model"></param>
        public static void SetModel(Control parent, object model)
        {
            SetModel(parent, string.Empty, model);
        }


        public static void SetModel(Control parent, string groupName, ICustomTypeDescriptor model)
        {

            List<Control> controlList = new List<Control>();
            FindDBCotrols(parent, groupName, controlList);

            Type modelT = model.GetType();

            PropertyDescriptorCollection propList = model.GetProperties();


            PropertyDescriptor prop;

            foreach (Control con in controlList)
            {
                if ((con is IAttributeAccessor) == false)
                {
                    continue;
                }

                IAttributeAccessor attr = (IAttributeAccessor)con;

                string dbField = attr.GetAttribute("DBField");
                string group = attr.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField))
                {
                    continue;
                }

                if (!TryGetProp(propList, dbField,out prop))
                {
                    continue;
                }


                object value = prop.GetValue(model);


                Type conT = con.GetType();

                DefaultPropertyAttribute dProp = AttrUtil.GetAttr<DefaultPropertyAttribute>(conT);

                if (dProp == null) { continue; }

                PropertyInfo pi = conT.GetProperty(dProp.Name);


                if (value != null && con is IValueFormat)
                {
                    string valueFormat = ((IValueFormat)con).ValueFormat;

                    if (!string.IsNullOrEmpty(valueFormat))
                    {
                        value = string.Format(valueFormat, value);
                    }
                }


                if (value != null && pi.PropertyType != value.GetType())
                {
                    value = Convert.ChangeType(value, pi.PropertyType);
                }


                pi.SetValue(con, value, null);
            }

        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="model"></param>
        public static void SetModel(Control parent, ICustomTypeDescriptor model)
        {
            SetModel(parent, string.Empty, model);
        }

    }
}
