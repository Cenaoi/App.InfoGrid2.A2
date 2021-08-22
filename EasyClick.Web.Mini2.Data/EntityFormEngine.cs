using System;
using System.Collections.Generic;
using System.Text;
using EC5.Utility;
using HWQ.Entity.LightModels;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 实体表单引擎
    /// </summary>
    public partial class EntityFormEngine : IFormLayoutBindings
    {

        string m_Model;

        /// <summary>
        /// 实体名称
        /// </summary>
        public string Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        /// <summary>
        /// 查找字段的控件
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        private List<FieldBase> FindFieldControls(FormLayout layout)
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

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="data"></param>
        public void SetData(FormLayout layout, object data)
        {
            if (layout == null) { throw new ArgumentNullException("layout"); }

            LightModel model;

            if (data is DataRecord)
            {
                DataRecord record = (DataRecord)data;

                LModelElement modelElem = LModelDna.GetElementByName(this.Model);

                LModelFieldElement pkField = modelElem.Fields[modelElem.PrimaryKey];

                object pk = ModelConvert.ChangeType(record.Id, pkField);

                DbDecipher decipher = this.OpenDecipher();

                model = decipher.GetModelByPk(this.Model, pk);
            }
            else
            {
                model = data as LightModel;
            }


            List<FieldBase> cons = FindFieldControls(layout);


            if (model != null)
            {
                LModelElement modelElem;

                if (model is LModel)
                {
                    modelElem = ((LModel)model).GetModelElement();
                }
                else
                {
                    modelElem = LightModel.GetLModelElement(model.GetType());
                }
            
                LModelFieldElement fieldElem = null;

                foreach (FieldBase item in cons)
                {
                    IChecked chekcBox = item as IChecked;

                    if(!modelElem.TryGetField(item.DataField,out fieldElem))
                    {
                        throw new Exception(string.Format("实体“{0}”不存在此字段“{1}”", 
                            modelElem.DBTableName, item.DataField));
                    }

                    if (chekcBox != null)
                    {
                        object value = model[fieldElem];

                        if (value is bool)
                        {
                            chekcBox.Checked = (bool)value;
                        }
                        else
                        {
                            chekcBox.Checked = chekcBox.TrueValue.Equals(value);
                        }

                    }
                    else
                    {
                        object value = model[item.DataField];

                        if (value == null)
                        {
                            item.Value = string.Empty;
                        }
                        else if (fieldElem.DBType == LMFieldDBTypes.Boolean)
                        {
                            item.Value = BoolUtil.ToJson((bool)value);
                        }
                        else
                        {
                            item.Value = (value == null) ? string.Empty : value.ToString();
                        }
                    }
                }
            }
            else
            {
                //处理默认值

                foreach (FieldBase item in cons)
                {
                    IChecked chekcBox = item as IChecked;
                    
                    if (chekcBox != null)
                    {
                        bool boolValue ;

                        if (bool.TryParse(item.DefaultValue, out boolValue))
                        {
                            chekcBox.Checked = boolValue;
                        }
                        else
                        {
                            chekcBox.Checked = chekcBox.TrueValue.Equals(item.DefaultValue);
                        }
                    }
                    else
                    {
                        item.Value = item.DefaultValue;
                    }
                }
            }

        }


        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="data"></param>
        public void EditData(FormLayout layout, object data)
        {
            if (layout == null) { throw new ArgumentNullException("layout"); }

            LModel model = data as LModel;

            LModelFieldElement fieldElem;
            

            LModelElement modelElem = model.GetModelElement();

            LModelFieldElementCollection fieldElems = modelElem.Fields;

            List<FieldBase> cons = FindFieldControls(layout);

            object value;

            foreach (FieldBase item in cons)
            {
                fieldElem = fieldElems[item.DataField];

                if (item is IChecked)
                {
                    value = ModelConvert.ChangeType(((IChecked)item).Checked, fieldElem);
                }
                else
                {
                    string valueStr = item.Value;

                    value = ModelConvert.ChangeType(valueStr, fieldElem);
                }

                model[fieldElem] = value;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public object GetData(FormLayout layout)
        {
            if (layout == null) { throw new ArgumentNullException("layout"); }

            LModelElement modelElem = LModelDna.GetElementByName(m_Model);

            LModelFieldElementCollection fieldElems = modelElem.Fields;

            LModel model = new LModel(modelElem);

            

            EditData(layout, model);

            return model;
        }







    }
}
