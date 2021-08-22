using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;
using HWQ.Entity;
using EasyClick.Web.Mini;

namespace EasyClick.BizWeb.UI
{
    /// <summary>
    /// 章鱼（将由 FromAction 静态对象代替)
    /// </summary>
    [Obsolete]
    [ParseChildren(true), PersistChildren(false)]
    public class EcOctopus:Control
    {
        string m_GroupName = string.Empty;

        [DefaultValue("")]
        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        private bool ExistPropName(PropertyInfo[] propList, string dbField)
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

        private PropertyInfo GetProp(PropertyInfo[] propList, string propName)
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
        /// （将由 FromAction 静态对象代替)
        /// </summary>
        /// <param name="obj"></param>
        [Obsolete]
        public void SetModel(object obj)
        {

            List<Control> controlList = new List<Control>();
            FindDBCotrols(this.Parent, m_GroupName, controlList);

            PropertyInfo[] propList = obj.GetType().GetProperties();
            PropertyInfo prop;

            foreach (Control con in controlList)
            {
                if ((con is IAttributeAccessor) == false)
                {
                    continue;
                }

                IAttributeAccessor attrs = (IAttributeAccessor)con;

                string dbField = attrs.GetAttribute("DBField");
                string group = attrs.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !ExistPropName(propList,dbField))
                {
                    continue;
                }


                prop = GetProp(propList, dbField);

                object value = prop.GetValue(obj, null) ;


                Type conT = con.GetType();

                DefaultPropertyAttribute dProp = LightModel.GetObjectOneAttr<DefaultPropertyAttribute>(conT);

                if (dProp == null) { continue; }

                PropertyInfo pi = conT.GetProperty(dProp.Name);

                //if (pi.PropertyType == typeof(string))
                //{
                //    value = ModelHelper.Format(value, fieldElem);
                //}

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
        /// （将由 FromAction 静态对象代替)
        /// </summary>
        /// <param name="obj"></param>
        [Obsolete]
        public void SetModel(LightModel model)
        {
            if (model == null)
            {
                return;
            }

            LModelElement modelElem;

            if (model is LModel)
            {
                modelElem = ((LModel)model).GetModelElement();
            }
            else
            {
                Type modelT = model.GetType();
                modelElem = LightModel.GetLModelElement(modelT);
            }

            List<Control> controlList = new List<Control>();
            FindDBCotrols(this.Parent, m_GroupName, controlList);


            foreach (Control con in controlList)
            {
                if ((con is IAttributeAccessor) == false)
                {
                    continue;
                }

                IAttributeAccessor attrs = (IAttributeAccessor)con;

                string dbField = attrs.GetAttribute("DBField");
                string group = attrs.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !modelElem.Fields.ContainsField(dbField))
                {
                    continue;
                }

                //if (!string.IsNullOrEmpty(group) && !m_GroupName.Equals(group, StringComparison.OrdinalIgnoreCase))
                //{
                //    continue;
                //}


                LModelFieldElement fieldElem = modelElem.Fields[dbField];

                object value = LightModel.GetFieldValue(model, dbField);

                //if (value == null)
                //{
                //    continue;
                //}

                Type conT = con.GetType();

                DefaultPropertyAttribute dProp = LightModel.GetObjectOneAttr<DefaultPropertyAttribute>(conT);

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


                if (pi.PropertyType == typeof(string))
                {
                    value = ModelHelper.ValueFormat(value, fieldElem);
                }

                pi.SetValue(con, value, null);
            }


        }

        /// <summary>
        /// （将由 FromAction 静态对象代替)
        /// </summary>
        /// <param name="obj"></param>
        [Obsolete]
        public T GetModel<T>()
        {
            object v = GetModel(typeof(T));

            return (T)v;
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object ModifyModel(object model)
        {
            return ModifyModel(this.Parent, model);
        }


        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public object ModifyModel(Control parent,object model)
        {
            if (model == null)
            {
                return model;
            }

            LModelElement modelElem;

            if (model is LModel)
            {
                modelElem = ((LModel)model).GetModelElement();
            }
            else
            {
                Type modelT = model.GetType();
                modelElem = LightModel.GetLModelElement(modelT);
            }

            List<Control> controlList = new List<Control>();
            FindDBCotrols(parent, m_GroupName, controlList);

            foreach (Control con in controlList)
            {
                if ((con is IAttributeAccessor) == false || !con.EnableViewState)
                {
                    continue;
                }

                IAttributeAccessor attrs = (IAttributeAccessor)con;

                string dbField = attrs.GetAttribute("DBField");
                string group = attrs.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !modelElem.Fields.ContainsField(dbField))
                {
                    continue;
                }

                //if (!string.IsNullOrEmpty(group) && !m_GroupName.Equals(group, StringComparison.OrdinalIgnoreCase))
                //{
                //    continue;
                //}

                LModelFieldElement fieldElem = modelElem.Fields[dbField];

                object conValue = MiniHelper.GetValue(con);


                conValue = ModelConvert.ChangeType(conValue, fieldElem.DBType);

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

        private void FindDBCotrols(Control parent, string group, List<Control> cons)
        {
            if (!parent.HasControls())
            {
                return;
            }

            foreach (Control con in parent.Controls)
            {
                IAttributeAccessor attr = con as IAttributeAccessor;

                if (attr != null)
                {
                    string groupStr = attr.GetAttribute("Groups");
                    string dbField = attr.GetAttribute("DBField");

                    if (!string.IsNullOrEmpty(group))
                    {
                        if (!string.IsNullOrEmpty(groupStr))
                        {
                            string[] groups = groupStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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
        /// （将由 FromAction 静态对象代替)
        /// </summary>
        /// <param name="obj"></param>
        [Obsolete]
        public object GetModel(Type modelT)
        {
            object m = Activator.CreateInstance(modelT);

            if (m is LightModel)
            {
                ((LightModel)m).SetTakeChange(true);
            }

            LModelElement modelElem = LightModel.GetLModelElement(modelT);

            List<Control> controlList = new List<Control>();

            FindDBCotrols(this.Parent, m_GroupName, controlList);

            foreach (Control con in controlList)
            {
                IAttributeAccessor attrs = (IAttributeAccessor)con;

                if (attrs == null || !con.EnableViewState)
                {
                    continue;
                }

                string dbField = attrs.GetAttribute("DBField");
                string group = attrs.GetAttribute("Groups");

                if (string.IsNullOrEmpty(dbField) || !modelElem.Fields.ContainsField(dbField))
                {
                    continue;
                }

                LModelFieldElement fieldElem = modelElem.Fields[dbField];

                object conValue = MiniHelper.GetValue(con);
                conValue = ModelConvert.ChangeType(conValue, fieldElem.DBType);

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


        public void Reset()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("var o = $(\"#" + this.ClientID + "\");");
            sb.AppendLine("$(o).closest('form').resetForm();");

            MiniHelper.Eval(sb.ToString());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("display", "none");
            writer.AddAttribute("id", this.ClientID);

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            //base.Render(writer);
        }
    }
}
