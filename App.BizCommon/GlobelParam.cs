using System;
using System.Collections.Generic;
using System.Text;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.BizCommon.Models;

namespace App.BizCommon
{
    public class GlobelParam:ModelAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 保存的值
        /// </summary>
        static SortedList<string, object> m_KeyValues = new SortedList<string, object>();

        static DateTime m_DateUpdate = new DateTime(2000, 1, 1);

        public static T GetValue<T>(string key)
        {
            return GetValue<T>(key, default(T),string.Empty);
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            return GetValue<T>(key, defaultValue, string.Empty);
        }

        public static T GetValue<T>(string key, T defaultValue, string desc)
        {
            if (!m_KeyValues.ContainsKey(key))
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter(typeof(C_PARAM));
                filter.And("PARAM_NAME", key);

                C_PARAM ps = decipher.SelectToOneModel<C_PARAM>(filter);

                if (ps == null)
                {
                    ps = new C_PARAM();
                    ps.PARAM_NAME = key;
                    ps.DATE_UPDATE = DateTime.Now;
                    ps.IS_ARRAY = "N";
                    ps.PARAM_TYPE = typeof(T).FullName;
                    ps.DESCRIPTION = desc;
                    ps.PARAM_VALEU = defaultValue.ToString();

                    decipher.InsertModel(ps);

                    m_KeyValues.Add(key, defaultValue);
                }
                else
                {
                    m_KeyValues.Add(key, Convert.ChangeType(ps.PARAM_VALEU, typeof(T)));
                }

            }

            object value = m_KeyValues[key];

            if (value.GetType() != typeof(T))
            {
                value = Convert.ChangeType(value, typeof(T));
            }

            return (T)value;
        }


        public static void SetValue<T>(string key, T value)
        {
            SetValue<T>(key, value, string.Empty);
        }

        static void UpdateParam(DbDecipher decipher,string key, Type valueT, object value, string desc)
        {
            C_PARAM ps = new C_PARAM();
            ps.PARAM_NAME = key;
            ps.PARAM_VALEU = value.ToString();
            ps.DATE_UPDATE = DateTime.Now;

            try
            {
                decipher.UpdateModelProps(ps, new string[] { "PARAM_VALEU", "DATE_UPDATE" });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        static void InsertParam(DbDecipher decipher, string key, Type valueT, object value, string desc)
        {
            C_PARAM ps = new C_PARAM();
            ps.PARAM_NAME = key;
            ps.DATE_UPDATE = DateTime.Now;
            ps.IS_ARRAY = "N";
            ps.PARAM_TYPE = valueT.FullName;
            ps.DESCRIPTION = desc;
            ps.PARAM_VALEU = value.ToString();

            try
            {
                decipher.InsertModel(ps);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public static void SetValue<T>(string key, T value, string desc)
        {
            if (!m_KeyValues.ContainsKey(key))
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter(typeof(C_PARAM));
                filter.And("PARAM_NAME", key);

                C_PARAM ps = decipher.SelectToOneModel<C_PARAM>(filter);

                if (ps == null)
                {
                    InsertParam(decipher, key, typeof(T), value, desc);
                }
                else
                {
                    UpdateParam(decipher, key, typeof(T), value, desc);
                }

                m_KeyValues.Add(key, value);
            }
            else
            {
                DbDecipher decipher = ModelAction.OpenDecipher();


                UpdateParam(decipher, key, typeof(T), value, desc);


                m_KeyValues[key] = value;
            }
        }
        

        public static T GetValue<T>(DbDecipher decipher, string key, T defaultValue, string desc)
        {
            object value;

            if (!m_KeyValues.ContainsKey(key))
            {
                LightModelFilter filter = new LightModelFilter(typeof(C_PARAM));
                filter.And("PARAM_NAME", key);

                C_PARAM ps = decipher.SelectToOneModel<C_PARAM>(filter);

                if (ps == null)
                {
                    ps = new C_PARAM();
                    ps.PARAM_NAME = key;
                    ps.DATE_UPDATE = DateTime.Now;
                    ps.IS_ARRAY = "N";
                    ps.PARAM_TYPE = typeof(T).FullName;
                    ps.DESCRIPTION = desc;
                    ps.PARAM_VALEU = defaultValue.ToString();

                    decipher.InsertModel(ps);

                    m_KeyValues.Add(key, defaultValue);
                }
                else
                {
                    T paramValue = (T)Convert.ChangeType(ps.PARAM_VALEU, typeof(T));

                    m_KeyValues.Add(key, paramValue);

                    value = paramValue;
                }

            }

            value = m_KeyValues[key];

            if (value.GetType() != typeof(T))
            {
                value = Convert.ChangeType(value, typeof(T));
            }

            return (T)value;
        }


        public static T GetValue<T>(DbDecipher decipher,string key)
        {
            return GetValue<T>(decipher, key, default(T), string.Empty);
        }

        public static T GetValue<T>(DbDecipher decipher, string key, T defaultValue)
        {
            return GetValue<T>(decipher, key, defaultValue, string.Empty);
        }

        public static void SetValue<T>(DbDecipher decipher, string key, T value,string desc)
        {
            if (!m_KeyValues.ContainsKey(key))
            {
                LightModelFilter filter = new LightModelFilter(typeof(C_PARAM));
                filter.And("PARAM_NAME", key);

                C_PARAM ps = decipher.SelectToOneModel<C_PARAM>(filter);

                if (ps == null)
                {
                    InsertParam(decipher, key, typeof(T), value, desc);
                }
                else
                {
                    UpdateParam(decipher, key, typeof(T), value, desc);
                }

                m_KeyValues.Add(key, value);

            }
            else
            {
                UpdateParam(decipher, key, typeof(T), value, desc);

                m_KeyValues[key] = value;
            }
        }

        public static void SetValue<T>(DbDecipher decipher, string key, T value)
        {
            SetValue<T>(decipher, key, value, string.Empty);
        }



        public static string GetValue(string key, string defaultValue, string desc)
        {
            return GetValue<string>(key, defaultValue, desc);
        }

        public static string GetValue(string key, string defaultValue)
        {
            return GetValue<string>(key, defaultValue, string.Empty);
        }

        public static string GetValue(string key)
        {
            return GetValue<string>(key, string.Empty, string.Empty);
        }


        public static void SetValue(string key, string value,string desc)
        {
            SetValue<string>(key, value, desc);
        }

        public static void SetValue(string key, string value)
        {
            SetValue<string>(key, value, string.Empty);
        }





        public static string GetValue(DbDecipher decipher, string key, string defaultValue, string desc)
        {
            return GetValue<string>(decipher, key, defaultValue, desc);
        }

        public static string GetValue(DbDecipher decipher, string key, string defaultValue)
        {
            return GetValue<string>(decipher,key, defaultValue, string.Empty);
        }

        public static string GetValue(DbDecipher decipher, string key)
        {
            return GetValue<string>(decipher, key, string.Empty, string.Empty);
        }


        public static void SetValue(DbDecipher decipher, string key, string value, string desc)
        {
            SetValue<string>(decipher, key, value, desc);
        }

        public static void SetValue(DbDecipher decipher, string key, string value)
        {
            SetValue<string>(decipher, key, value, string.Empty);
        }


    }
}
