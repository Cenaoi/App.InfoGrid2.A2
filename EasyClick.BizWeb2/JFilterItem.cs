using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 二次过滤的函数
    /// </summary>
    [Newtonsoft.Json.JsonObject]
    public class JFilterItem
    {
        string m_p_type = "DEFAULT";
        string m_logic = "=";

        [JsonProperty("p_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("DEFAULT")]
        public string P_Type
        {
            get { return m_p_type; }
            set { m_p_type = value; }
        }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("logic", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("=")]
        public string Logic
        {
            get { return m_logic; }
            set { m_logic = value; }
        }

        [JsonProperty("value")]
        public string Value { get; set; }

    }
}
