using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using EC5.Utility;
using Newtonsoft.Json.Linq;


namespace EasyClick.Web.Mini2.Data
{


    /// <summary>
    /// 数据仓库记录行
    /// </summary>
    public class DataRecord
    {        
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string ERROR = "ERROR";

        /// <summary>
        /// 警告信息
        /// </summary>
        public const string WARNING = "WARNING";




        //DataStoreRowState m_State;

        string m_Id;



        /// <summary>
        /// 客户端ID。js 产生的客户端唯一ID
        /// </summary>
        string m_ClientId;

        DataFieldCollection m_Fields = new DataFieldCollection();

        int m_Index = 0;

        DataRecordAction m_Action = DataRecordAction.None;


        /// <summary>
        /// 原实体对象
        /// </summary>
        object m_SrcModel; 



        /// <summary>
        /// 字段索引
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public object this[string fieldName]
        {
            get
            {
                if (StringUtil.IsBlank(fieldName))
                {
                    throw new ArgumentNullException("字段名不能为空.");
                }
                if (!m_Fields.Contains(fieldName))
                {
                    throw new Exception($"没有此字段名\"{fieldName}\".");
                }

                return m_Fields[fieldName].Value;
            }
            set
            {

                if (StringUtil.IsBlank(fieldName))
                {
                    throw new ArgumentNullException("字段名不能为空.");
                }
                if (!m_Fields.Contains(fieldName))
                {
                    throw new Exception($"没有此字段名\"{fieldName}\".");
                }

                m_Fields[fieldName].Value = value.ToString();
            }
        }

        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public object Get(string fieldName)
        {
            if (StringUtil.IsBlank(fieldName))
            {
                throw new ArgumentNullException("字段名不能为空.");
            }
            if (!m_Fields.Contains(fieldName))
            {
                throw new Exception($"没有此字段名\"{fieldName}\".");
            }

            return m_Fields[fieldName].Value;
        }

        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">字段值</param>
        public void Set(string fieldName, object value)
        {
            if (StringUtil.IsBlank(fieldName))
            {
                throw new ArgumentNullException("字段名不能为空.");
            }
            if (!m_Fields.Contains(fieldName))
            {
                throw new Exception($"没有此字段名\"{fieldName}\".");
            }

            m_Fields[fieldName].Value = value?.ToString();
        }


        /// <summary>
        /// 原实体对象
        /// </summary>
        public object Model
        {
            get { return m_SrcModel; }
            set { m_SrcModel = value; }
        }


        /// <summary>
        /// 客户端ID。js 产生的客户端唯一ID，与服务器配合交互使用。
        /// </summary>
        public string ClientId
        {
            get { return m_ClientId; }
            set { m_ClientId = value; }
        }

        /// <summary>
        /// 记录状态
        /// </summary>
        public DataRecordAction Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        /// <summary>
        /// 行索引
        /// </summary>
        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }





        /// <summary>
        /// 主键值
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        /// <summary>
        /// 判断是否有子字段
        /// </summary>
        /// <returns></returns>
        public bool HasCells()
        {
            return (m_Fields != null && m_Fields.Count > 0);
        }

        /// <summary>
        /// 字段集合
        /// </summary>
        public DataFieldCollection Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new DataFieldCollection();
                }
                return m_Fields;
            }
        }


        /// <summary>
        /// 创建异常提示信息
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="message">消息名称</param>
        public void MarkInvalid(string field, string message)
        {

        }



        /// <summary>
        /// 填充对象
        /// </summary>
        /// <param name="obj"></param>
        public void Fill(object obj)
        {

            Type objT = obj.GetType();

            if (m_Fields != null)
            {
                foreach (DataField cell in m_Fields)
                {
                    PropertyInfo propInfo = ObjectUtil.GetProperty(objT, cell.Name);

                    if (propInfo == null || !propInfo.CanWrite) { continue; }

                    object targetValue = StringUtil.ChangeType(cell.Value, propInfo.PropertyType);

                    propInfo.SetValue(obj, targetValue, null);
                }
            }


        }

        /// <summary>
        /// 解释行数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public static DataRecord Parse(JToken rowData)
        {
            if (rowData.Type != JTokenType.Object)
            {
                return null;
            }

            DataRecord record = new DataRecord();

            string stateStr = rowData["action"].Value<string>();

            record.Action = EnumUtil.Parse<DataRecordAction>(stateStr, true);
            record.ClientId = rowData["clientId"].Value<string>();
            record.Id = rowData["id"].Value<string>();
            record.Index = rowData["index"].Value<int>();


            JToken values = rowData["values"];

            if (values != null)
            {
                foreach (JProperty field in values.Children())
                {
                    DataField f = new DataField();

                    JValue jv = (JValue)values[field.Name];

                    f.Name = field.Name;

                    if (jv.Type == JTokenType.Float)
                    {
                        f.Value = Convert.ToDecimal( jv.Value<double>()).ToString();
                    }
                    else
                    {
                        f.Value = jv.Value<string>();
                    }

                    record.Fields.Add(f);
                }
            }

            return record;
        }



    }

}
